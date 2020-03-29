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
    public class ClassController : BaseController
    {
        //
        // GET: /Class/

        public ActionResult ClassEdit()
        {
            return View();
        }


        #region 班级列表
        public ActionResult GetClassList()
        {
            string professionId = Request.Form["ProfessionID"];
            string where = "";
            if (!string.IsNullOrEmpty(professionId))
            {
                where += "AND C.ProfessionID = " + professionId + "";
            }
            string cmdText = @"SELECT  c.ClassID ,
        c.Name ,
        r1.RefeName Status ,
        c.Status StatusValue
FROM    T_Pro_Class c
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = c.Status
                                      AND r1.RefeTypeID = 1
WHERE   1 = 1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        #endregion


        #region 编辑班级
        /// <summary>
        /// 编辑班级
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        public string GetClassEdit(ClassModel cm, string cProfessionID, string ClassName)
        {
            cm.ProfessionID = cProfessionID;
            cm.Name = ClassName;
            if (string.IsNullOrEmpty(cm.ClassID))
            {
                cm.ClassID = "0";
            }
            if (string.IsNullOrEmpty(cm.Name))
            {
                return "班级名称不能为空";
            }
            else
            {
                if (cm.Name.Length > 32)
                {
                    return "班级名称长度不能超过32个字符";
                }
            }
            if (ValidateClassName(cm.ClassID, cm.Name, cm.ProfessionID))
            {
                return "专业已存在";
            }
            bool flag = false;
            if (cm.ClassID == "0")
            {
                cm.Status = "1";
                cm.CreateID = this.UserId.ToString();
                cm.CreateTime = DateTime.Now.ToString();
                cm.UpdateID = this.UserId.ToString();
                cm.UpdateTime = DateTime.Now.ToString();
                if (ClassBLL.InsertClass(cm) > 0)
                {
                    flag = true;
                }
            }
            else
            {
                ClassModel editcm = GetClassModel(cm.ClassID);
                LogBLL.CreateLog("T_Pro_Class", this.UserId.ToString(), editcm, cm);
                editcm.Name = cm.Name;
                editcm.ProfessionID = cm.ProfessionID;
                editcm.UpdateTime = DateTime.Now.ToString();
                editcm.UpdateID = this.UserId.ToString();
                if (ClassBLL.UpdateClass(editcm) > 0)
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
        private static ClassModel GetClassModel(string classId)
        {
            string where = "and ClassID=@ClassID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ClassID",classId)
                };
            ClassModel editcm = ClassBLL.ClassModelByWhere(where, paras);
            return editcm;
        }
        #endregion

        #region 修改时表单赋值
        public AjaxResult SelectClass()
        {
            string classId = Request.Form["ID"];
            DataTable dt = GetClassTable(classId);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }
        #endregion

        #region  获取专业Table
        private static DataTable GetClassTable(string classId)
        {
            string where = "and ClassID=@ClassID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ClassID",classId)
                };
            DataTable dt = ClassBLL.ClassTableByWhere(where, paras, "");
            return dt;
        }
        #endregion

        #region 验证班级名称是否重复
        private static bool ValidateClassName(string classId, string name, string professionId)
        {
            string where = " and ClassID<>@ClassID and Name=@Name and ProfessionID=@ProfessionID ";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@ClassID",classId),
            new SqlParameter("@Name",name),
            new SqlParameter("@ProfessionID",professionId)
            };
            DataTable dt = ClassBLL.ClassTableByWhere(where, paras, "");
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
        public AjaxResult GetValidataClassName()
        {
            string classId = Request.Form["ID"];
            string name = Request.Form["Value"];
            string professionId = Request.Form["TypeId"];
            if (ValidateClassName(classId, name, professionId))
            {
                return AjaxResult.Error();
            }
            else
            {
                return AjaxResult.Success();
            }
        }
        #endregion

        #region 修改班级状态
        public AjaxResult GetUpdateClassStatus()
        {
            string classId = Request.Form["ID"];
            string status = Request.Form["Value"];
            ClassModel cm = GetClassModel(classId);
            cm.Status = status;
            cm.UpdateID = this.UserId.ToString();
            cm.UpdateTime = DateTime.Now.ToString();
            if (ClassBLL.UpdateClass(cm) > 0)
            {
                return AjaxResult.Success();

            }
            else
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }

        }
        #endregion

        #region 根据专业获得班级下拉列表
        public string ClassCombobox()
        {
            string professionId = Request.Form["ProfessionID"];
            DataTable dt = ClassBLL.ClassCombobox(professionId);
            return JsonHelper.DataTableToJson(dt);
        }
        #endregion

        #region 根据报名专业获得班级下拉列表
        public string ClassComboboxBysProfessionID()
        {
            string sProfessionId = Request.Form["sProfessionID"];
            DataTable dt = ClassBLL.ClassComboboxBysProfessionID(sProfessionId);
            return JsonHelper.DataTableToJson(dt);
        }
        #endregion
    }
}
