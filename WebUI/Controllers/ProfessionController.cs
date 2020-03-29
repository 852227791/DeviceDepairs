using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using System.Data;
using System.Data.SqlClient;
namespace WebUI.Controllers
{
    public class ProfessionController : BaseController
    {
        //
        // GET: /Profession/
        #region 页面加载
        public ActionResult ProfessionList()
        {
            return View();
        }
        public ActionResult ProfessionEdit()
        {
            return View();
        }
        #endregion

        #region 专业列表
        public ActionResult GetProfessionList()
        {
            string deptId = Request.Form["DeptID"];
            string where = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND p.DeptID = " + deptId + "";
            }
            string cmdText = @"SELECT  p.ProfessionID ,
        p.Name ,
        p.EnglishName ,
        r1.RefeName Status ,
        p.Status StatusValue
FROM    T_Pro_Profession p
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = p.Status
                                      AND r1.RefeTypeID = 1
WHERE   1 = 1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        #endregion

        #region 编辑专业
        /// <summary>
        /// 编辑专业
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        public string GetProfessionEdit(ProfessionModel pm, string ProfessionName)
        {
            pm.Name = ProfessionName;
            if (string.IsNullOrEmpty(pm.ProfessionID))
            {
                pm.ProfessionID = "0";
            }
            if (string.IsNullOrEmpty(pm.Name))
            {
                return "专业名称不能为空";
            }
            else
            {
                if (pm.Name.Length > 32)
                {
                    return "专业名称长度不能超过32个字符";
                }
            }
            if (string.IsNullOrEmpty(pm.EnglishName))
            {
                return "首字母不能为空";
            }
            else
            {
                if (pm.EnglishName.Length > 8)
                {
                    return "首字母长度不能超过8个字符";
                }
            }
            if (ValidateProfessionName(pm.ProfessionID, pm.Name, pm.DeptID))
            {
                return "专业已存在";
            }
            bool flag = false;
            if (pm.ProfessionID == "0")
            {
                pm.Status = "1";
                pm.CreateID = this.UserId.ToString();
                pm.CreateTime = DateTime.Now.ToString();
                pm.UpdateID = this.UserId.ToString();
                pm.UpdateTime = DateTime.Now.ToString();
                if (ProfessionBLL.InsertProfession(pm) > 0)
                {
                    flag = true;
                }
            }
            else
            {
                ProfessionModel editpm = GetProfessionModel(pm.ProfessionID);
                LogBLL.CreateLog("T_Pro_Profession", this.UserId.ToString(), editpm, pm);
                editpm.Name = pm.Name;
                editpm.EnglishName = pm.EnglishName;
                editpm.DeptID = pm.DeptID;
                editpm.UpdateTime = DateTime.Now.ToString();
                editpm.UpdateID = this.UserId.ToString();
                if (ProfessionBLL.UpdateProfession(editpm) > 0)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                return "yes";
            }
            else
            {
                return "出现未知错误，请联系管理员";
            }
        }
        #endregion

        #region  获取专业model
        private static ProfessionModel GetProfessionModel(string professionId)
        {
            string where = "and ProfessionID=@ProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ProfessionID",professionId)
                };
            ProfessionModel editpm = ProfessionBLL.ProfessionModelByWhere(where, paras);
            return editpm;
        }
        #endregion

        #region 修改时表单赋值
        public AjaxResult SelectProfession()
        {
            string professionId = Request.Form["ID"];
            DataTable dt = GetProfessionTable(professionId);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }
        #endregion

        #region  获取专业Table
        private static DataTable GetProfessionTable(string professionId)
        {
            string where = "and ProfessionID=@ProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ProfessionID",professionId)
                };
            DataTable dt = ProfessionBLL.ProfessionTableByWhere(where, paras, "");
            return dt;
        }
        #endregion

        #region 验证专业名称是否重复
        private static bool ValidateProfessionName(string professionId, string name, string deptId)
        {
            string where = " and ProfessionID<>@ProfessionID and Name=@Name and DeptID=@DeptID ";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@ProfessionID",professionId),
            new SqlParameter("@Name",name),
            new SqlParameter("@DeptID",deptId)
            };
            DataTable dt = ProfessionBLL.ProfessionTableByWhere(where, paras, "");
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

        #region 表单验证名称是否重复
        public AjaxResult GetValidataProfessionName()
        {
            string professionId = Request.Form["ID"];
            string name = Request.Form["Value"];
            string deptId = Request.Form["TypeId"];
            if (ValidateProfessionName(professionId, name, deptId))
            {
                return AjaxResult.Error();
            }
            else
            {
                return AjaxResult.Success();
            }
        }
        #endregion

        #region 修改专业状态
        public AjaxResult GetUpdateProfessionStatus()
        {
            string professionId = Request.Form["ID"];
            string status = Request.Form["Value"];
            ProfessionModel pm = GetProfessionModel(professionId);
            pm.Status = status;
            pm.UpdateID = this.UserId.ToString();
            pm.UpdateTime = DateTime.Now.ToString();
            if (ProfessionBLL.UpdateProfession(pm) > 0)
            {
                return AjaxResult.Success();

            }
            else
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }

        }
        #endregion

        #region 专业下拉列表
        public string ProfessionCombobox()
        {
            string deptId = Request.Form["DeptID"];
            DataTable dt = ProfessionBLL.ProfessionCombobox(deptId);
            return JsonHelper.DataTableToJson(dt);
        }
        #endregion
  
    }
}
