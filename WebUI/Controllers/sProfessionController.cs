using BLL;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
namespace WebUI.Controllers
{
    public class sProfessionController : BaseController
    {
        //
        // GET: /sProfession/

        public ActionResult sProfessionList()
        {
            return View();
        }
        public ActionResult sProfessionEdit()
        {
            return View();
        }
        /// <summary>
        /// 编辑招生专业信息
        /// </summary>
        /// <param name="spm"></param>
        /// <returns></returns>
        public string GetsProfessionEdit(sProfessionFormModel spfm)
        {
            if (spfm.ProfessionID.Length == 0)
            {
                return "请选择专业";
            }
            if (string.IsNullOrEmpty(spfm.Year))
            {
                return "请选择年份";
            }
            if (string.IsNullOrEmpty(spfm.Month))
            {
                return "请选月份";
            }
            for (int i = 0; i < spfm.ProfessionID.Length; i++)
            {
                if (ValidatePrfessionIsRepeat(spfm.ProfessionID[i], spfm.sProfessionID, spfm.DeptID, spfm.Year, spfm.Month))
                {
                    return "招生专业不能重复";
                }
            }


            if (string.IsNullOrEmpty(spfm.sProfessionID))
            {
                spfm.sProfessionID = "0";
            }
            bool flag = false;
            if (spfm.sProfessionID.Equals("0"))
            {
                try
                {
                    for (int i = 0; i < spfm.ProfessionID.Length; i++)
                    {
                        sProfessionModel spm = new sProfessionModel();
                        spm.DeptID = spfm.DeptID;
                        spm.Month = spfm.Month;
                        spm.Year = spfm.Year;
                        spm.ProfessionID = spfm.ProfessionID[i];
                        spm.CreateID = this.UserId.ToString();
                        spm.CreateTime = DateTime.Now.ToString();
                        spm.Status = "1";
                        spm.UpdateID = this.UserId.ToString();
                        spm.UpdateTime = DateTime.Now.ToString();
                        sProfessionBLL.InsertsProfession(spm);
                    }
                    flag = true;
                }
                catch (Exception)
                {
                    flag = false;
                    throw;
                }
            }
            else
            {
                string where = " and  sProfessionID=@sProfessionID";
                SqlParameter[] paras = new SqlParameter[] {
                   new SqlParameter("@sProfessionID",spfm.sProfessionID )
                };
                sProfessionModel editspm = sProfessionBLL.sProfessionModelByWhere(where, paras);
                LogBLL.CreateLog("T_Stu_Profession", this.UserId.ToString(), editspm, spfm);
                editspm.Year = spfm.Year;
                editspm.Month = spfm.Month;
                editspm.ProfessionID = spfm.ProfessionID[0];
                editspm.UpdateID = this.UserId.ToString();
                editspm.UpdateTime = DateTime.Now.ToString();
                if (sProfessionBLL.UpdatesProfession(editspm) > 0)
                    flag = true;
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
        /// <summary>
        /// 获取招生专业列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetsProfessionList()
        {
            string year = Request.Form["Year"];
            string month = Request.Form["Month"];
            string menuId = Request.Form["MenuID"];
            string deptId = Request.Form["DeptID"];
            string where = "";
            if (!string.IsNullOrEmpty(month) && !month.Equals("0"))
            {
                where += " and sp.Month=" + month + "";
            }
            if (!string.IsNullOrEmpty(year) && !year.Equals("0"))
            {
                where += " and sp.Year=" + year + "";
            }
            if (string.IsNullOrEmpty(deptId))
            {
                deptId = "0";
            }
            string cmdText = @"SELECT  sp.Year,r2.RefeName Month,r1.RefeName Status, sp.Status StatusValue, p.Name ,sProfessionID
FROM    T_Stu_sProfession sp
        LEFT JOIN T_Pro_Profession p ON p.ProfessionID = sp.ProfessionID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = sp.Status
                                       AND r1.RefeTypeID = 1
									    LEFT JOIN T_Sys_Refe r2 ON r2.Value = sp.Month
                                       AND r2.RefeTypeID = 18
        Where sp.DeptID=" + deptId + " {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "sp.DeptID", "sp.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        /// <summary>
        /// 修改时表单赋值
        /// </summary>
        /// <returns></returns>
        public AjaxResult SelectsProfession()
        {
            string id = Request.Form["ID"];
            string where = " and  sProfessionID=@sProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
                   new SqlParameter("@sProfessionID",id )
                };
            DataTable dt = sProfessionBLL.sProfessionTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt), "获取信息成功");
        }
        /// <summary>
        /// 验证专业是否重复
        /// </summary>
        /// <param name="profeeionId">基础专业id</param>
        /// <param name="sProfessionId">招生专业id</param>
        /// <param name="deptId">机构id</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns></returns>
        private bool ValidatePrfessionIsRepeat(string profeeionId, string sProfessionId, string deptId, string year, string month)
        {
            if (string.IsNullOrEmpty(sProfessionId))
            {
                sProfessionId = "0";
            }
            string where = "and ProfessionID=@ProfessionID  and sProfessionID<>@sProfessionID and DeptID=@DeptID and Year=@Year and Month=@Month";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ProfessionID",profeeionId),
                new SqlParameter("@sProfessionID",sProfessionId),
                new SqlParameter("@DeptID",deptId),
                new SqlParameter("@Year",year),
                new SqlParameter("@Month",month)
            };
            DataTable dt = sProfessionBLL.sProfessionTableByWhere(where, paras, "");
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
        /// 招生专业状态
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetUpdateStatus()
        {
            string id = Request.Form["ID"];
            string status = Request.Form["Value"];
            string where = " and  sProfessionID=@sProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
                   new SqlParameter("@sProfessionID",id )
                };
            sProfessionModel editspm = sProfessionBLL.sProfessionModelByWhere(where, paras);
            editspm.Status = status;
            editspm.UpdateID = this.UserId.ToString();
            editspm.UpdateTime = DateTime.Now.ToString();
            if (sProfessionBLL.UpdatesProfession(editspm) > 0)
                return AjaxResult.Success();
            return AjaxResult.Error();
        }

        /// <summary>
        /// 获取招生方案专业设置下列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetsItemProfessionCombobox()
        {
            string deptId = Request.Form["DeptID"];
            string year = Request.Form["Year"];
            string month = Request.Form["Month"];
            string cmdText = @"SELECT  sp.sProfessionID id ,
        p.EnglishName+'-'+p.Name name,p.EnglishName
FROM T_Stu_sProfession sp
    LEFT JOIN T_Pro_Profession p ON p.ProfessionID = sp.ProfessionID where sp.DeptID={0} and Year={1} and Month={2} and sp.Status=1";
            cmdText = string.Format(cmdText, deptId, year, month);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
    }
}
