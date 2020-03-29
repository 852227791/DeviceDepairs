using BLL;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class RoleController : BaseController
    {
        //
        // GET: /Role/

        public ActionResult RoleList()
        {
            ViewBag.Title = "角色管理";
            return View();
        }

        public ActionResult RoleEdit()
        {
            ViewBag.Title = "角色编辑";
            return View();
        }

        public ActionResult ChooseRole()
        {
            ViewBag.Title = "选择角色";
            return View();
        }

        public ActionResult GetRoleList()
        {
            string name = Request.Form["txtName"];
            string status = Request.Form["selStatus"];
            string roleType = Request.Form["selRoleType"];
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += " and r.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "r.Status");
            }
            if (!string.IsNullOrEmpty(roleType))
            {
                where += OtherHelper.MultiSelectToSQLWhere(roleType, "r.RoleType");
            }
            string cmdText = @"SELECT  r.RoleID ,
        r.Name ,
        r.Status StatusValue ,
        r1.RefeName Status ,
        r.RoleType RoleTypeValue ,
        r2.RefeName RoleType ,
        r.Description
FROM    T_Sys_Role r
        LEFT JOIN T_Sys_Refe r1 ON r.Status = r1.Value
                                   AND r1.RefeTypeID = 1
        LEFT JOIN T_Sys_Refe r2 ON r.RoleType = r2.Value
                                   AND r2.RefeTypeID = 2
WHERE   r.RoleID <> 1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        /// <summary>
        /// 验证角色名是否重复
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckName()
        {
            string name = Request.Form["Value"];
            string roleId = Request.Form["ID"];

            if (string.IsNullOrEmpty(roleId))
            {
                roleId = "0";
            }
            if (CheckNameMethod(roleId, name))
            {
                return AjaxResult.Error("该角色名已经存在，请重新输入");
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        public string GetRoleEdit(RoleModel rm)
        {
            if (string.IsNullOrEmpty(rm.RoleID))
            {
                rm.RoleID = "0";
            }

            if (rm.Name.Length < 1 || rm.Name.Length > 16)
            {
                return "角色名不能为空，并且长度为1-16个字符";
            }
            if (CheckNameMethod(rm.RoleID, rm.Name))
            {
                return "该角色名已经存在，请重新输入";
            }
            if (string.IsNullOrEmpty(rm.RoleType))
            {
                return "请选择角色分类";
            }
            if (!string.IsNullOrEmpty(rm.Description))
            {
                if (rm.Description.Length > 128)
                {
                    return "角色描述不能大于128个字符";
                }
            }
            if (!string.IsNullOrEmpty(rm.Remark))
            {
                rm.Remark = rm.Remark.Replace("\r\n", "<br />");
            }

            string result = "no";
            if (rm.RoleID != "0")
            {
                string where = " AND RoleID=@RoleID";
                SqlParameter[] paras = new SqlParameter[] { 
                    new SqlParameter("@RoleID", rm.RoleID)
                };
                RoleModel editrm = RoleBLL.RoleModelByWhere(where, paras);

                LogBLL.CreateLog("T_Sys_Role", this.UserId.ToString(), editrm, rm);//写日志

                editrm.Name = rm.Name;
                editrm.RoleType = rm.RoleType;
                editrm.Description = rm.Description;
                editrm.Remark = rm.Remark;
                editrm.UpdateID = this.UserId.ToString();
                editrm.UpdateTime = DateTime.Now.ToString();
                if (RoleBLL.UpdateRole(editrm) > 0)
                {
                    return "yes";
                }
            }
            else
            {
                rm.Status = "1";
                rm.CreateID = this.UserId.ToString();
                rm.CreateTime = DateTime.Now.ToString();
                rm.UpdateID = this.UserId.ToString();
                rm.UpdateTime = DateTime.Now.ToString();
                if (RoleBLL.InsertRole(rm) > 0)
                {
                    return "yes";
                }
            }
            return result;
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <returns></returns>
        public AjaxResult SelectRole()
        {
            string roleID = Request.Form["ID"];
            string where = " AND RoleID = @RoleID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@RoleID", roleID)
            };
            DataTable dt = RoleBLL.RoleTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }


        public AjaxResult GetUpdateStatus()
        {
            string roleid = Request.Form["ID"];
            string status = Request.Form["Value"];

            string where = " AND RoleID = @RoleID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@RoleID", roleid)
            };
            RoleModel rm = RoleBLL.RoleModelByWhere(where, paras);
            rm.Status = status;
            if (RoleBLL.UpdateRole(rm) > 0)
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }

        /// <summary>
        /// 判断角色名是否重复的方法
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool CheckNameMethod(string RoleID, string Name)
        {
            string where = " AND RoleID <> @RoleID AND Name = @Name";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@RoleID", RoleID),
                new SqlParameter("@Name", Name)
            };
            DataTable dt = RoleBLL.RoleTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
