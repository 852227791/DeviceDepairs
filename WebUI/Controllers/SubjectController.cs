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
    public class SubjectController : BaseController
    {
        //
        // GET: /Subject/

        public ActionResult SubjectList()
        {
            return View();
        }
        public ActionResult SubjectEdit()
        {
            return View();
        }
        /// <summary>
        /// 获取会计科目树
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public string GetSubjectList(string Status)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Status))
            {
                where += " and s.Status=" + Status + "";
            }

            string cmdText = @" SELECT  SubjectID id ,
        Name text ,
        ParentID ,
        EnglishName ,
        s.Remark ,
        r1.RefeName Status ,
        s.Status StatusValue
FROM     T_Pro_Subject s
        LEFT JOIN  T_Sys_Refe r1 ON r1.Value = s.Status
                                       AND r1.RefeTypeID = 1  
            Where 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }
        /// <summary>
        /// 编辑会计科目
        /// </summary>
        /// <returns></returns>
        public string GetSubjectEdit(SubjectModel sm)
        {
            if (string.IsNullOrEmpty(sm.SubjectID))
                sm.SubjectID = "0";

            if (string.IsNullOrEmpty(sm.Name))
                return "会计科目名称不能为空";

            if (string.IsNullOrEmpty(sm.ParentID))
                return "上级科目为空";
            if (ValidateNameIsRepeat(sm.SubjectID, sm.Name))
                return "名称不能重复";
            if (SubjectBLL.CheckSubject(sm.SubjectID, sm.ParentID))
                return "不能选择自己和自己的子级作为父级菜单";

            bool flag = false;
            if (sm.SubjectID.Equals("0"))
            {
                sm.CreateID = UserId.ToString();
                sm.CreateTime = DateTime.Now.ToString();
                sm.UpdateID = this.UserId.ToString();
                sm.UpdateTime = DateTime.Now.ToString();
                sm.Status = "1";
                if (SubjectBLL.InsertSubject(sm) > 0)
                    flag = true;
            }
            else
            {
                SubjectModel editsm = SubjectBLL.SubjectModelByWhere(sm.SubjectID);
                editsm.Name = sm.Name;
                editsm.EnglishName = sm.EnglishName;
                editsm.ParentID = sm.ParentID;
                editsm.Remark = sm.Remark;
                editsm.UpdateID = this.UserId.ToString();
                editsm.UpdateTime = DateTime.Now.ToString();
                if (SubjectBLL.UpdateSubject(editsm) > 0)
                    flag = true;
            }
            if (flag)
                return "yes";
            return "出现未知错误，请联系管理";
        }
        /// <summary>
        /// 验证名称是否重复
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool ValidateNameIsRepeat(string subjectId, string name)
        {
            if (string.IsNullOrEmpty(subjectId))
                subjectId = "0";
            string where = " and SubjectID<>@SubjectID and Name=@Name";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@SubjectID",subjectId),
                new SqlParameter("@Name",name)
            };
            DataTable dt = SubjectBLL.SubjectTableByWhere(where, paras, "");
            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// 表单验证名称是否重复
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public AjaxResult FormValidateNameIsRepeat(string ID, string Value)
        {
            if (ValidateNameIsRepeat(ID, Value))
                return AjaxResult.Error();
            return AjaxResult.Success();
        }

        /// <summary>
        /// 验证会计科目子父级关系
        /// </summary>
        /// <returns></returns>
        public AjaxResult FormValidateParentId()
        {
            string subjectId = Request.Form["ID"];
            string parentId = Request.Form["TypeId"];
            if (SubjectBLL.CheckSubject(subjectId, parentId))
                return AjaxResult.Error();
            return AjaxResult.Success();
        }
        /// <summary>
        /// 变更会计科目状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public AjaxResult GetUpdateStatus(string ID, string Value)
        {
            SubjectModel sm = SubjectBLL.SubjectModelByWhere(ID);
            sm.UpdateID = this.UserId.ToString();
            sm.UpdateTime = DateTime.Now.ToString();
            sm.Status = Value;
            if (SubjectBLL.UpdateSubject(sm) > 0)
                return AjaxResult.Success();
            return AjaxResult.Error("出现未知错误，请联系管理员");
        }

        public string GetSubjectCombobox()
        {
            string cmdText = @"SELECT  SubjectID id ,
        ParentID ,
        Name text
FROM    T_Pro_Subject
WHERE   Status = 1
ORDER BY EnglishName ASC";
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }
    }
}
