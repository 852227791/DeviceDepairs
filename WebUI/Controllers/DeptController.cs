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
    public class DeptController : BaseController
    {
        //
        // GET: /Dept/

        public ActionResult DeptList()
        {
            ViewBag.Title = "部门管理";
            return View();
        }

        public ActionResult DeptEdit()
        {
            ViewBag.Title = "部门编辑";
            return View();
        }

        public ActionResult ChooseDept()
        {
            ViewBag.Title = "选择部门";
            return View();
        }

        public string GetDeptTree()
        {
            string menuId = Request.Form["MenuID"];
            string isEdit = Request.Form["IsEdit"];
            string status = Request.Form["Status"];
            string cmdText = @"SELECT  DeptID id ,
        ParentID ,
        CASE Status
          WHEN 1 THEN Name
          WHEN 2 THEN '<span style=color:#ff0000>' + Name + '(停用)</span>'
        END text
FROM    T_Sys_Dept
WHERE   Status in ({0}) {1}
ORDER BY Queue ASC ,
        DeptID ASC";
            string powerStr = PurviewToTree(menuId, isEdit);
            powerStr = string.Format(powerStr, "DeptID");
            cmdText = String.Format(cmdText, status, powerStr);
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }

        public AjaxResult SelectViewDept()
        {
            string deptID = Request.Form["ID"];
            string cmdText = @"SELECT  d.Name ,
        pd.Name ParentName ,
        d.ShortName ,
        d.Code ,
        d.Queue ,
        d.Remark
FROM    T_Sys_Dept d
        LEFT JOIN T_Sys_Dept pd ON d.ParentID = pd.DeptID
WHERE   d.DeptID = {0}";
            cmdText = string.Format(cmdText, deptID);
            return AjaxResult.Success(JsonData.GetArray(cmdText));
        }

        public AjaxResult SelectDept()
        {
            string deptId = Request.Form["ID"];
            string where = " AND DeptID = @DeptID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@DeptID", deptId)
            };
            DataTable dt = DeptBLL.DeptTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        /// <summary>
        /// 验证部门名是否重复
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckDeptName()
        {
            string name = Request.Form["Value"];
            string deptId = Request.Form["ID"];
            string parentId = Request.Form["TypeId"];

            if (CheckDeptNameMethod(deptId, parentId, name))
            {
                return AjaxResult.Error("该部门名已经存在，请重新输入");
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        /// <summary>
        /// 验证部门名是否重复的方法
        /// </summary>
        /// <param name="RefeID"></param>
        /// <param name="RefeTypeID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool CheckDeptNameMethod(string DeptID, string ParentID, string Name)
        {
            string where = " AND DeptID <> @DeptID AND ParentID = @ParentID AND Name = @Name";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@DeptID", DeptID),
                new SqlParameter("@ParentID", ParentID),
                new SqlParameter("@Name", Name)
            };
            DataTable dt = DeptBLL.DeptTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证部门ID是否与父级ID相同
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckDeptIDIsParentID()
        {
            string deptId = Request.Form["ID"];
            string parentId = Request.Form["TypeId"];

            if (CheckDeptIDIsParentIDMethod(deptId, parentId))
            {
                return AjaxResult.Error("不能选择自己或自己的下级作为上级部门");
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        /// <summary>
        /// 验证部门ID是否与父级ID相同的方法
        /// </summary>
        /// <param name="DeptID"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public bool CheckDeptIDIsParentIDMethod(string DeptID, string ParentID)
        {
            string DeptIDStr = ",";
            DataTable dt = DeptBLL.SelectChildrenDeptID(DeptID);
            foreach (DataRow dr in dt.Rows)
            {
                DeptIDStr += dr["DeptID"].ToString() + ",";
            }

            if (DeptIDStr.IndexOf("," + ParentID + ",") > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证部门编码是否重复
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckCode()
        {
            string code = Request.Form["Value"];
            string deptId = Request.Form["ID"];

            if (CheckCodeMethod(deptId, code))
            {
                return AjaxResult.Error("该编码已经存在，请重新输入");
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        /// <summary>
        /// 验证部门编码是否重复的方法
        /// </summary>
        /// <param name="DeptID"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public bool CheckCodeMethod(string DeptID, string Code)
        {
            string where = " AND DeptID <> @DeptID AND Code = @Code";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptID", DeptID),
                new SqlParameter("@Code", Code)
            };
            DataTable dt = DeptBLL.DeptTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetDeptEdit(DeptModel dm)
        {
            if (string.IsNullOrEmpty(dm.DeptID))
            {
                dm.DeptID = "0";
            }

            if (dm.Name.Length < 1 || dm.Name.Length > 16)
            {
                return "部门名称不能为空，并且长度为1-16个字符！";
            }
            if (CheckDeptNameMethod(dm.DeptID, dm.ParentID, dm.Name))
            {
                return "该部门名已经存在，请重新输入";
            }
            if (string.IsNullOrEmpty(dm.ParentID))
            {
                return "请选择上级部门！";
            }
            if (CheckDeptIDIsParentIDMethod(dm.DeptID, dm.ParentID))
            {
                return "不能选择自己或自己的下级作为上级部门";
            }
            if (CheckCodeMethod(dm.DeptID, dm.Code))
            {
                return "该编码已经存在，请重新输入";
            }
            if (string.IsNullOrEmpty(dm.Queue))
            {
                return "排序不能为空！";
            }
            if (!string.IsNullOrEmpty(dm.Remark))
            {
                dm.Remark = dm.Remark.Replace("\r\n", "<br />");
            }
            string result = "no";
            if (dm.DeptID != "0")
            {
                string where = " AND DeptID=@DeptID";
                SqlParameter[] paras = new SqlParameter[] { 
                    new SqlParameter("@DeptID", dm.DeptID)
                };
                DeptModel editdm = DeptBLL.DeptModelByWhere(where, paras);

                LogBLL.CreateLog("T_Sys_Dept", this.UserId.ToString(), editdm, dm);//写日志

                editdm.Name = dm.Name;
                editdm.ParentID = dm.ParentID;
                editdm.ShortName = dm.ShortName;
                editdm.Code = dm.Code;
                editdm.Queue = dm.Queue;
                editdm.Remark = dm.Remark;
                editdm.UpdateID = this.UserId.ToString();
                editdm.UpdateTime = DateTime.Now.ToString();
                if (DeptBLL.UpdateDept(editdm) > 0)
                {
                    return "yes";
                }
            }
            else
            {
                dm.Status = "1";
                dm.CreateID = this.UserId.ToString();
                dm.CreateTime = DateTime.Now.ToString();
                dm.UpdateID = this.UserId.ToString();
                dm.UpdateTime = DateTime.Now.ToString();
                if (DeptBLL.InsertDept(dm) > 0)
                {
                    return "yes";
                }
            }
            return result;
        }

        public AjaxResult GetUpdateDeptStatus()
        {
            string deptId = Request.Form["ID"];
            string status = Request.Form["Value"];

            string where = " AND DeptID = @DeptID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@DeptID", deptId)
            };
            DeptModel dm = DeptBLL.DeptModelByWhere(where, paras);
            dm.Status = status;
            if (DeptBLL.UpdateDept(dm) > 0)
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }

        /// <summary>
        /// 启用、停用判断父、子节点状态
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckIsHandle()
        {
            string deptId = Request.Form["DeptID"];
            string status = Request.Form["Status"];
            if (status == "2")
            {
                string where = " AND Status = 1 AND ParentID = @DeptID  ";
                SqlParameter[] paras = new SqlParameter[] { 
                    new SqlParameter("@DeptID", deptId)
                };
                DataTable dt = DeptBLL.DeptTableByWhere(where, paras, "");
                if (dt.Rows.Count > 0)
                {
                    return AjaxResult.Error("请先停用所有子部门");
                }
                else
                {
                    return AjaxResult.Success();
                }
            }
            else if (status == "1")
            {
                string where = " AND Status = 2 AND DeptID = (Select ParentID from T_Sys_Dept WHERE DeptID = @DeptID)";
                SqlParameter[] paras = new SqlParameter[] { 
                    new SqlParameter("@DeptID", deptId)
                };
                DataTable dt = DeptBLL.DeptTableByWhere(where, paras, "");
                if (dt.Rows.Count > 0)
                {
                    return AjaxResult.Error("请先启用父部门");
                }
                else
                {
                    return AjaxResult.Success();
                }
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }

        /// <summary>
        /// 获取默认部门
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetDefaultDeptID()
        {
            string MenuID = Request.Form["MenuID"];
            string str = "";
            string isEditStr = "Code LIKE '%add%'";

            string where1 = @" AND UserID = @UserID
                AND RoleID IN ( SELECT  RoleID
                                FROM    T_Sys_Power
                                WHERE   MenuID = @MenuID
                                        AND RoleID IN ( SELECT  RoleID
                                                        FROM    T_Sys_UserRole
                                                        WHERE   UserID = @UserID )
                                        AND ButtonID IN ( SELECT    ButtonID
                                                          FROM      T_Sys_Button
                                                          WHERE     Status = 1
                                                                    AND MenuID = @MenuID
                                                                    AND " + isEditStr + " ) )";
            SqlParameter[] paras1 = new SqlParameter[] { 
                new SqlParameter("@MenuID", MenuID),
                new SqlParameter("@UserID", UserId)
            };
            DataTable dt1 = PurviewBLL.PurviewTableByWhere(where1, paras1, "");
            if (dt1.Rows.Count == 1)
            {
                string where2 = " AND ParentID = @DeptID";
                SqlParameter[] paras2 = new SqlParameter[] { 
                    new SqlParameter("@DeptID", dt1.Rows[0]["DeptID"].ToString())
                };
                DataTable dt2 = DeptBLL.DeptTableByWhere(where2, paras2, "");
                if (dt2.Rows.Count == 0)
                {
                    str = dt1.Rows[0]["DeptID"].ToString();
                }
            }
            return AjaxResult.Success(str);
        }
        #region 验证是否是末节点
        public static bool CheckIsLastDept(string deptId)
        {
            string where = " AND Status = 1 AND ParentID = @DeptID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@DeptID", deptId)
            };
            DataTable dt = DeptBLL.DeptTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
