using BLL;
using Common;
using DAL;
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
    public class ChangeEnrollController : BaseController
    {
        //
        // GET: /ChangeEnroll/

        public ActionResult ChangeProfession()
        {
            return View();
        }
        public ActionResult ChangeProfessionView()
        {
            return View();
        }

        public ActionResult ChangeProfessionExceptionList()
        {
            return View();
        }
        public ActionResult GetChangeProfessionList(string MenuID, string name, string idCard, string enrollNum)
        {
            string where = string.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                where += " and s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and  s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(enrollNum))
            {
                where += " and  e1.EnrollNum like '%" + enrollNum + "%'";
            }
            string cmdText = @"SELECT  s.Name StudName ,
        s.IDCard ,
        e1.EnrollNum ,
        epo.sEnrollsProfessionID ,
        d1.Name BeSchool ,
        ep1.Year BeYear ,
        ep1.Month BeMonth ,
        r1.RefeName BeLevel ,
        pn1.Name BeMajor ,
        d2.Name AfSchool ,
        ep2.Year AfYear ,
        ep2.Month AfMonth ,
        r2.RefeName AfLevel ,
        pn2.Name AfMajor ,
        epo.Explain ChangeReson ,
        CONVERT(NVARCHAR(10), epo.NoteTime, 23) ChangeTime ,
        r3.RefeName SortName,
		epo.NewsEnrollsProfessionID,
        s.StudentID
FROM    T_Stu_sEnrollsProfessionNote epo
        INNER JOIN T_Stu_sEnrollsProfession ep1 ON ep1.sEnrollsProfessionID = epo.sEnrollsProfessionID
        INNER JOIN T_Stu_sEnroll e1 ON e1.sEnrollID = ep1.sEnrollID
        INNER JOIN T_Sys_Dept d1 ON d1.DeptID = ep1.DeptID
        INNER JOIN T_Pro_Student s ON s.StudentID = e1.StudentID
        INNER JOIN T_Sys_Refe r1 ON r1.Value = ep1.EnrollLevel
                                    AND r1.RefeTypeID = 17
        INNER JOIN T_Stu_sProfession sp1 ON sp1.sProfessionID = ep1.sProfessionID
        INNER JOIN T_Pro_Profession pn1 ON pn1.ProfessionID = sp1.ProfessionID
        INNER JOIN T_Stu_sEnrollsProfession ep2 ON ep2.sEnrollsProfessionID = epo.NewsEnrollsProfessionID
        INNER JOIN T_Sys_Dept d2 ON d2.DeptID = ep2.DeptID
        INNER JOIN T_Sys_Refe r2 ON r2.Value = ep2.EnrollLevel
                                    AND r2.RefeTypeID = 17
        INNER JOIN T_Stu_sProfession sp2 ON sp2.sProfessionID = ep2.sProfessionID
        INNER JOIN T_Pro_Profession pn2 ON pn2.ProfessionID = sp2.ProfessionID
        INNER JOIN T_Sys_Refe r3 ON r3.Value = epo.Sort
                                    AND r3.RefeTypeID = 22
        Where epo.Sort = 1  {0}";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "ep2.DeptID", "epo.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="MenuID"></param>
        /// <param name="name"></param>
        /// <param name="idCard"></param>
        /// <param name="enrollNum"></param>
        /// <returns></returns>
        public AjaxResult GetChangeProfessionDownload(string MenuID, string name, string idCard, string enrollNum)
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                where += " and s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and  s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(enrollNum))
            {
                where += " and  e1.EnrollNum like '%" + enrollNum + "%'";
            }
            string cmdText = @"SELECT
        s.Name 姓名 ,
        s.IDCard 身份证号 ,
        e1.EnrollNum 学号,
        d1.Name 转前校区  ,
        ep1.Year 转前年份 ,
        ep1.Month 转前月份 ,
        r1.RefeName 转前层次 ,
        pn1.Name 转前专业 ,
        ( SELECT    ISNULL(SUM(ShouldMoney),0.00)
          FROM      T_Stu_sOrder
          WHERE     sOrderID IN (
                    SELECT  sOrderID
                    FROM    T_Stu_sEnrollsProfessionsOrder
                    WHERE   sEnrollsProfessionID = epo.sEnrollsProfessionID )
        ) 转前总应缴 ,
        ( SELECT    ISNULL(SUM(PaidMoney),0.00)
          FROM      T_Stu_sOrder
          WHERE     sOrderID IN (
                    SELECT  sOrderID
                    FROM    T_Stu_sEnrollsProfessionsOrder
                    WHERE   sEnrollsProfessionID = epo.sEnrollsProfessionID )
        ) 转前总实缴 ,
        d2.Name 转后校区 ,
        ep2.Year 转后年份 ,
        ep2.Month 转后月份 ,
        r2.RefeName 转后层次 ,
        pn2.Name 转后专业 ,
        ( SELECT    ISNULL(SUM(ShouldMoney),0.00)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = epo.NewsEnrollsProfessionID
                    AND Status IN (1,2,3)
        ) 转后总应缴 ,
        ( SELECT    ISNULL(SUM(PaidMoney),0.00)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = epo.NewsEnrollsProfessionID
                    AND Status IN (1,2,3)
        ) 转后总实缴 ,
        epo.Explain 转专业原因 ,
        CONVERT(NVARCHAR(10), epo.NoteTime, 23) 转专业时间 ,
        r3.RefeName 记录类型
FROM    T_Stu_sEnrollsProfessionNote epo
        INNER JOIN T_Stu_sEnrollsProfession ep1 ON ep1.sEnrollsProfessionID = epo.sEnrollsProfessionID
        INNER JOIN T_Stu_sEnroll e1 ON e1.sEnrollID = ep1.sEnrollID
        INNER JOIN T_Sys_Dept d1 ON d1.DeptID = ep1.DeptID
        INNER JOIN T_Pro_Student s ON s.StudentID = e1.StudentID
        INNER JOIN T_Sys_Refe r1 ON r1.Value = ep1.EnrollLevel
                                    AND r1.RefeTypeID = 17
        INNER JOIN T_Stu_sProfession sp1 ON sp1.sProfessionID = ep1.sProfessionID
        INNER JOIN T_Pro_Profession pn1 ON pn1.ProfessionID = sp1.ProfessionID
        INNER JOIN T_Stu_sEnrollsProfession ep2 ON ep2.sEnrollsProfessionID = epo.NewsEnrollsProfessionID
        INNER JOIN T_Sys_Dept d2 ON d2.DeptID = ep2.DeptID
        INNER JOIN T_Sys_Refe r2 ON r2.Value = ep2.EnrollLevel
                                    AND r2.RefeTypeID = 17
        INNER JOIN T_Stu_sProfession sp2 ON sp2.sProfessionID = ep2.sProfessionID
        INNER JOIN T_Pro_Profession pn2 ON pn2.ProfessionID = sp2.ProfessionID
        INNER JOIN T_Sys_Refe r3 ON r3.Value = epo.Sort
                                    AND r3.RefeTypeID = 22
        Where 1=1  {0}";
            string powerStr = PurviewToList(MenuID);
            string filename = "转专业信息.xls";
            powerStr = string.Format(powerStr, "ep2.DeptID", UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }
        /// <summary>
        /// 查看缴费
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AjaxResult GetFeeinfo(string ID)
        {
            return AjaxResult.Success(JsonHelper.DataTableToJson(sEnrollBLL.GetsFeeInfo(ID)), "success");
        }

        public ActionResult GetChangeProfessionExceptionData(string MenuID, string name, string idCard, string enrollNum, string deptId)
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                where += " and s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and  s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(enrollNum))
            {
                where += " and  e.EnrollNum like '%" + enrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND ep.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            string cmdText = @"SELECT  d.Name DeptName ,
        s.Name StuName ,
        s.IDCard ,
        e.EnrollNum ,
        e.ExamNum ,
        p.Name ProName ,
		o.NumName,
        o.ShouldMoney ,
        o.PaidMoney ,
        r1.RefeName IsNumItem,
		epo.sEnrollsProfessionsOrderID
FROM    T_Stu_sEnrollsProfessionsOrder epo
        LEFT JOIN T_Stu_sOrder o ON o.sOrderID = epo.sOrderID
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = epo.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = epo.IsNumItem
                                       AND r1.RefeTypeID = 13
        LEFT JOIN T_Stu_sProfession spn ON spn.sProfessionID = ep.sProfessionID
        LEFT JOIN T_Pro_Profession p ON p.ProfessionID = spn.ProfessionID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = ep.DeptID
WHERE   epo.IsNumItem = 2
        AND o.PaidMoney > 0 {0}";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "ep.DeptID", UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        public AjaxResult GetChangeProfessionExceptionDownload(string MenuID, string name, string idCard, string enrollNum, string deptId)
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                where += " and s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and  s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(enrollNum))
            {
                where += " and  e.EnrollNum like '%" + enrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND ep.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            string cmdText = @"SELECT  d.Name 校区 ,
        s.Name 姓名 ,
        s.IDCard 身份证号,
        e.EnrollNum 学号,
        e.ExamNum 考生号,
        p.Name 专业 ,
		o.NumName 缴费次数,
        o.ShouldMoney 应缴金额 ,
        o.PaidMoney 实缴金额,
        r1.RefeName 是否已读
FROM    T_Stu_sEnrollsProfessionsOrder epo
        LEFT JOIN T_Stu_sOrder o ON o.sOrderID = epo.sOrderID
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = epo.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = epo.IsNumItem
                                       AND r1.RefeTypeID = 13
        LEFT JOIN T_Stu_sProfession spn ON spn.sProfessionID = ep.sProfessionID
        LEFT JOIN T_Pro_Profession p ON p.ProfessionID = spn.ProfessionID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = ep.DeptID
WHERE   epo.IsNumItem = 2
        AND o.PaidMoney > 0 {0}";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "ep.DeptID", UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            string filename = "转专业异常信息.xls";
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }

        public AjaxResult ChangeIsNumItem(string ID)
        {
            string[] array = ID.Split(',');
            if (array.Length.Equals(0))
            {
                return AjaxResult.Error("请选择转专业异常信息");
            }

            foreach (string item in array)
            {
                string where = " and sEnrollsProfessionsOrderID=@sEnrollsProfessionsOrderID";
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@sEnrollsProfessionsOrderID",item)
                };
                sEnrollsProfessionsOrderModel epom = sEnrollsProfessionsOrderBLL.sEnrollsProfessionsOrderModelByWhere(where, paras);
                epom.IsNumItem = "3";
                sEnrollsProfessionsOrderBLL.UpdatesEnrollsProfessionsOrder(epom);
            }
            return AjaxResult.Success("", "操作成功!");

        }
    }

}
