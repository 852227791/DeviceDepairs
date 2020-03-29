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
    public class StudentContactController : BaseController
    {
        //
        // GET: /Default1/

        public string GetStudentContactList(string StudentID)
        {
            string where = "and StudentID=@StudentID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",StudentID)
            };
            DataTable dt = StudentContactBLL.StudentContactTableByWhere(where, paras, "");
            return JsonGridData.GetGridJSON(dt);
        }
        public string GetStudentContactEdit(StudentContactModel scm)
        {

            if (string.IsNullOrEmpty(scm.Tel) || string.IsNullOrEmpty(scm.Name))
            {
                return "";
            }
            if (scm.StudentID.Equals("0"))
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(scm.StudentContactID))
            {
                string where = " and StudentContactID=@StudentContactID";
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@StudentContactID",scm.StudentContactID)
                };
                StudentContactModel newscm = StudentContactBLL.StudentContactModelByWhere(where, paras);
                LogBLL.CreateLog("T_Pro_StudentContact", UserId.ToString(), newscm, scm);
                newscm.Name = scm.Name;
                newscm.Tel = scm.Tel;
                newscm.UpdateID = UserId.ToString();
                newscm.UpdateTime = DateTime.Now.ToString();
                StudentContactBLL.UpdateStudentContact(newscm);
            }
            else
            {
                scm.UpdateTime = DateTime.Now.ToString();
                scm.UpdateID = UserId.ToString();
                scm.CreateID = this.UserId.ToString();
                scm.CreateTime = DateTime.Now.ToString();
                scm.Status = "1";
                scm.StudentContactID = StudentContactBLL.InsertStudentContact(scm).ToString();
            }
            return OtherHelper.JsonSerializer(scm);
        }
    }
}
