using BLL;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class StudentInfoController : BaseController
    {
        //
        // GET: /StudentInfo/

        public string GetStudentInfoEdit(StudentInfoModel sim)
        {
            string qq = Request.Form["QQ"];
            string wechat = Request.Form["WeChat"];
            string address = Request.Form["Address"];
            bool flag = false;
            if (!string.IsNullOrEmpty(sim.StudentInfoID))
            {
                string where = " and StudentInfoID=@StudentInfoID ";
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@StudentInfoID",sim.StudentInfoID)
                };
                StudentInfoModel newsim = StudentInfoBLL.StudentInfoModelByWhere(where, paras);
                LogBLL.CreateLog("T_Pro_StudentInfo", UserId.ToString(), newsim, sim);
                newsim.CityID = sim.CityID;
                newsim.Nation = sim.Nation;
                newsim.ProvinceID = sim.ProvinceID;
                newsim.DistrictID = sim.DistrictID;
                newsim.School = sim.School;
                newsim.UpdateID = this.UserId.ToString();
                newsim.UpdateTime = DateTime.Now.ToString();
                newsim.Zip = sim.Zip;
                if (StudentInfoBLL.UpdateStudentInfo(newsim) > 0)
                {
                    flag = true;
                }

            }
            else
            {
                sim.Status = "1";
                sim.CreateID = this.UserId.ToString();
                sim.CreateTime = DateTime.Now.ToString();
                sim.UpdateID = this.UserId.ToString();
                sim.UpdateTime = DateTime.Now.ToString();
                if (StudentInfoBLL.InsertStudentInfo(sim) > 0)
                {
                    flag = true;
                }
            }
            string where1 = "and StudentID=@StudentID";
            SqlParameter[] paras1 = new SqlParameter[] {
                new SqlParameter("@StudentID",sim.StudentID)
            };
            StudentModel sm = StudentBLL.StudentModelByWhere(where1,paras1);
            sm.Address = address;
            sm.QQ = qq;
            sm.WeChat = wechat;
            sm.UpdateID = UserId.ToString();
            sm.UpdateTime = DateTime.Now.ToString();
            StudentBLL.UpdateStudent(sm);
            if (flag)
            {
                return "yes";
            }
            else
            {
                return "出现未知错误，请联系管理员";
            }
        }


        public AjaxResult GetStudentInfo(string ID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return AjaxResult.Error("数据错误，请联系管理员");
            }
            string where = " and StudentID=@StudentID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@StudentID",ID)
                };
            StudentInfoModel newsim = StudentInfoBLL.StudentInfoModelByWhere(where, paras);
            return AjaxResult.Success(OtherHelper.JsonSerializer(newsim),"");
        }
    }
}
