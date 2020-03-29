using Common;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class sReportController : BaseController
    {
        //
        // GET: /sReport/
        public ActionResult Detail()
        {
            ViewBag.Title = "学费明细汇总";
            return View();
        }
        public ActionResult sFeeStudentSex()
        {
            return View();
        }
        public ActionResult sFeeTotal()
        {
            return View();
        }
        public ActionResult MajorFeeCount()
        {
            return View();
        }

        public ActionResult FeeUserReport()
        {
            return View();
        }
        public ActionResult Dept()
        {
            ViewBag.Title = "学费收费汇总";
            return View();
        }
        public ActionResult Enroll()
        {
            ViewBag.Title = "缴学费人数汇总";
            return View();
        }
        public ActionResult sFeeSearch()
        {
            ViewBag.Title = "学费信息统计";
            return View();
        }
        public ActionResult sFeeDetailSearch()
        {
            ViewBag.Title = "供款收取统计";
            return View();
        }

        public ActionResult GetDeptList()
        {
            string deptId = Request.Form["txtDeptID"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string menuId = Request.Form["MenuID"];
            string where1 = "";
            string where2 = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                string[] deptArr = deptId.Split(',');
                string bracketS = "(";
                string bracketE = ")";
                if (deptArr.Length < 2)
                {
                    bracketS = "";
                    bracketE = "";
                }
                for (int i = 0; i < deptArr.Length; i++)
                {
                    if (i == 0)
                    {
                        where1 += " and " + bracketS;
                    }
                    else
                    {
                        where1 += " or ";
                    }
                    where1 += " d.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptArr[i] + "))";
                }
                where1 += bracketE;
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) <= '" + feeTimeE + "'";
            }
            string cmdText = @"SELECT  d.DeptID ,
        d.ParentID ,
        d.Name DeptName ,
        ( SELECT    ISNULL(SUM(fo.PaidMoney), 0)
          FROM      T_Stu_sFeesOrder fo
          WHERE     fo.Status = 1
                    AND fo.sFeeID IN (
                    SELECT  f.sFeeID
                    FROM    T_Stu_sFee f
                    WHERE   f.DeptID IN (
                            SELECT  DeptID
                            FROM    dbo.GetChildrenDeptID(d.DeptID) )
                            AND f.Status <> 9 {1} )
        ) PaidMoney ,
        ( SELECT    ISNULL(SUM(fo.DiscountMoney), 0)
          FROM      T_Stu_sFeesOrder fo
          WHERE     fo.Status = 1
                    AND fo.sFeeID IN (
                    SELECT  f.sFeeID
                    FROM    T_Stu_sFee f
                    WHERE   f.DeptID IN (
                            SELECT  DeptID
                            FROM    dbo.GetChildrenDeptID(d.DeptID) )
                            AND f.Status <> 9 {1} )
        ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Stu_sOffset o
          WHERE     o.Status = 1
                    AND o.BySort = 1
                    AND o.RelatedID IN (
                    SELECT  fo.sFeesOrderID
                    FROM    T_Stu_sFeesOrder fo
                    WHERE   fo.Status = 1
                            AND fo.sFeeID IN (
                            SELECT  f.sFeeID
                            FROM    T_Stu_sFee f
                            WHERE   f.DeptID IN (
                                    SELECT  DeptID
                                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                                    AND f.Status <> 9 {1} ) )
        ) stu_Offset ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Stu_sOffset o
          WHERE     o.Status = 1
                    AND o.BySort = 2
                    AND o.RelatedID IN (
                    SELECT  fo.sFeesOrderID
                    FROM    T_Stu_sFeesOrder fo
                    WHERE   fo.Status = 1
                            AND fo.sFeeID IN (
                            SELECT  f.sFeeID
                            FROM    T_Stu_sFee f
                            WHERE   f.DeptID IN (
                                    SELECT  DeptID
                                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                                    AND f.Status <> 9 {1} ) )
        ) inc_Offset ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Stu_sOffset o
          WHERE     o.Status = 1
                    AND o.BySort = 3
                    AND o.RelatedID IN (
                    SELECT  fo.sFeesOrderID
                    FROM    T_Stu_sFeesOrder fo
                    WHERE   fo.Status = 1
                            AND fo.sFeeID IN (
                            SELECT  f.sFeeID
                            FROM    T_Stu_sFee f
                            WHERE   f.DeptID IN (
                                    SELECT  DeptID
                                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                                    AND f.Status <> 9 {1} ) )
        ) _Offset ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Stu_sOffset o
          WHERE     o.Status = 1
                    AND o.BySort = 1
                    AND o.ByRelatedID IN (
                    SELECT  fo.sFeesOrderID
                    FROM    T_Stu_sFeesOrder fo
                    WHERE   fo.Status = 1
                            AND fo.sFeeID IN (
                            SELECT  f.sFeeID
                            FROM    T_Stu_sFee f
                            WHERE   f.DeptID IN (
                                    SELECT  DeptID
                                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                                    AND f.Status <> 9 {1} ) )
        ) ByOffset ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Stu_sRefund r
          WHERE     r.Status = 1
                    AND r.sFeesOrderID IN (
                    SELECT  fo.sFeesOrderID
                    FROM    T_Stu_sFeesOrder fo
                    WHERE   fo.Status = 1
                            AND fo.sFeeID IN (
                            SELECT  f.sFeeID
                            FROM    T_Stu_sFee f
                            WHERE   f.DeptID IN (
                                    SELECT  DeptID
                                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                                    AND f.Status <> 9 {1} ) )
        ) RefundMoney
FROM    T_Sys_Dept d
WHERE   d.Status = 1
        AND d.DeptID <> 1
        {0}";
            string OrderText = " ORDER BY d.Name ASC";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "d.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where1 + powerStr, where2);

            string rows = JsonMenuTreeData.GetArrayJSON(cmdText + OrderText, "DeptID", "ParentID", CommandType.Text);

            //string fieldArray = "PaidMoney,DiscountMoney,_Offset,inc_Offset,stu_Offset,ByOffset,RefundMoney";
            //string footerStr = JsonGridData.GetGridSum(cmdText + tempDept, fieldArray);

            string json = @"{""rows"":" + rows + @",""total"":""0""}";//,""footer"":" + footerStr + "
            return ResponseWriteResult(json);
        }


        public ActionResult GetEnrollList()
        {
            string deptId = Request.Form["txtDeptID"];
            string selYear = Request.Form["selYear"];
            string selMonth = Request.Form["selMonth"];
            string timeS = Request.Form["txtTimeS"];
            string timeE = Request.Form["txtTimeE"];
            string menuId = Request.Form["MenuID"];
            string where1 = "";
            string where2 = "";
            string where3 = "";
            string where4 = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                string[] deptArr = deptId.Split(',');
                string bracketS = "(";
                string bracketE = ")";
                if (deptArr.Length < 2)
                {
                    bracketS = "";
                    bracketE = "";
                }
                for (int i = 0; i < deptArr.Length; i++)
                {
                    if (i == 0)
                    {
                        where1 += " and " + bracketS;
                    }
                    else
                    {
                        where1 += " or ";
                    }
                    where1 += " d.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptArr[i] + "))";
                }
                where1 += bracketE;
            }
            if (!string.IsNullOrEmpty(selYear))
            {
                where2 += OtherHelper.MultiSelectToSQLWhere(selYear, "ep.Year");
                where3 += OtherHelper.MultiSelectToSQLWhere(selYear, "ep.Year");
                where4 += OtherHelper.MultiSelectToSQLWhere(selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(selMonth))
            {
                where2 += OtherHelper.MultiSelectToSQLWhere(selMonth, "ep.Month");
                where3 += OtherHelper.MultiSelectToSQLWhere(selYear, "ep.Year");
                where4 += OtherHelper.MultiSelectToSQLWhere(selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(timeS))
            {
                where2 += " AND convert(NVARCHAR(10), ep.FirstFeeTime, 23) >= '" + timeS + "'";
                where3 += " AND convert(NVARCHAR(10), ep.EnrollTime, 23) >= '" + timeS + "'";
                where4 += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) >= '" + timeS + "'";
            }
            if (!string.IsNullOrEmpty(timeE))
            {
                where2 += " AND convert(NVARCHAR(10), ep.FirstFeeTime, 23) <= '" + timeE + "'";
                where3 += " AND convert(NVARCHAR(10), ep.EnrollTime, 23) <= '" + timeE + "'";
                where4 += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <= '" + timeE + "'";
            }
            string cmdText = @"SELECT  d.DeptID ,
        d.ParentID ,
        d.Name DeptName ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 8, 3 ) )
                    {1}
        ) T_FeeNumSum ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
                    LEFT JOIN T_Pro_DeptArea da ON ep.DeptAreaID = da.DeptAreaID
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND da.Queue = 1
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 8, 3 ) )
                    {1}
        ) T_FeeNum1 ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
                    LEFT JOIN T_Pro_DeptArea da ON ep.DeptAreaID = da.DeptAreaID
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND da.Queue = 2
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 8, 3 ) )
                    {1}
        ) T_FeeNum2 ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
                    LEFT JOIN T_Pro_DeptArea da ON ep.DeptAreaID = da.DeptAreaID
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND da.Queue = 3
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 8, 3 ) )
                    {1}
        ) T_FeeNum3 ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
                    LEFT JOIN T_Pro_DeptArea da ON ep.DeptAreaID = da.DeptAreaID
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND da.Queue = 4
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 8, 3 ) )
                    {1}
        ) T_FeeNum4 ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 4 ) )
                    {2}
        ) C_FeeNumSum ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
                    LEFT JOIN T_Pro_DeptArea da ON ep.DeptAreaID = da.DeptAreaID
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND da.Queue = 1
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 4 ) )
                    {2}
        ) C_FeeNum1 ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
                    LEFT JOIN T_Pro_DeptArea da ON ep.DeptAreaID = da.DeptAreaID
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND da.Queue = 2
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 4 ) )
                    {2}
        ) C_FeeNum2 ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
                    LEFT JOIN T_Pro_DeptArea da ON ep.DeptAreaID = da.DeptAreaID
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND da.Queue = 3
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 4 ) )
                    {2}
        ) C_FeeNum3 ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
                    LEFT JOIN T_Pro_DeptArea da ON ep.DeptAreaID = da.DeptAreaID
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND da.Queue = 4
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 4 ) )
                    {2}
        ) C_FeeNum4 ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.BeforeEnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <> '1900-01-01'
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 4 ) )
                    {2}
        ) ZFeeNumSum ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.BeforeEnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 4 ) )
                    {3}
        ) C_BeforeEnrollNum ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 6 ) )
                    {1}
        ) Z_FeeNumSum ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 7 ) )
                    {1}
        ) W_FeeNumSum ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      T_Stu_sEnrollsProfession ep
          WHERE     ep.Status <> 9
                    AND ep.DeptID IN (
                    SELECT  DeptID
                    FROM    dbo.GetChildrenDeptID(d.DeptID) )
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT DISTINCT
                            o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.PlanSort IN ( 5 ) )
                    {1}
        ) X_FeeNumSum
FROM    T_Sys_Dept d
WHERE   d.Status = 1
        AND d.DeptID <> 1
        {0}";
            string OrderText = " ORDER BY d.Name ASC";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "d.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where1 + powerStr, where2, where3, where4);

            string rows = JsonMenuTreeData.GetArrayJSON(cmdText + OrderText, "DeptID", "ParentID", CommandType.Text);

            //string footerStr = "[{\"DeptName\":\"合计：\"";
            //string fieldArray = "T_FeeNumSum,T_FeeNum1,T_FeeNum2,T_FeeNum3,T_FeeNum4,C_FeeNumSum,C_FeeNum1,C_FeeNum2,C_FeeNum3,C_FeeNum4,C_BeforeEnrollNum,Z_FeeNumSum,W_FeeNumSum,X_FeeNumSum";
            //string[] fieldArr = fieldArray.Split(",");
            //for (int i = 0; i < fieldArr.Length; i++)
            //{
            //    decimal sum = JsonGridData.GetGridSum(cmdText + tempDept, fieldArr[i]);
            //    footerStr += ",\"" + fieldArr[i] + "\":\"" + sum.ToString() + "\"";
            //}
            //footerStr += "}]";

            string json = @"{""rows"":" + rows + @",""total"":""0""}";//,""footer"":" + footerStr + "
            return ResponseWriteResult(json);
        }



        public ActionResult GetMajorFeeCount()
        {

            string menuId = Request.Form["MenuID"];
            string deptId = Request.Form["treeDeptID"];
            string selYear = Request.Form["selYear"];
            string selMonth = Request.Form["selMonth"];
            string txtTimeS = Request.Form["txtTimeS"];
            string txtTimeE = Request.Form["txtTimeE"];
            string enrollLevel = Request.Form["selEnrollLevel"];
            string where1 = string.Empty;
            string where2 = string.Empty;
            string where3 = string.Empty;
            string where4 = string.Empty;

            if (!string.IsNullOrEmpty(deptId))
            {
                where1 += " AND sp.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(selYear))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selYear, "sp.Year");
            }
            if (!string.IsNullOrEmpty(selMonth))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selMonth, "sp.Month");
            }
            if (!string.IsNullOrEmpty(txtTimeS))
            {
                where2 += " AND convert(NVARCHAR(10), ep.FirstFeeTime, 23) >= '" + txtTimeS + "'";
                where3 += " AND convert(NVARCHAR(10), ep.EnrollTime, 23) >= '" + txtTimeS + "'";
                where4 += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) >= '" + txtTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtTimeE))
            {
                where2 += " AND convert(NVARCHAR(10), ep.FirstFeeTime, 23) <= '" + txtTimeE + "'";
                where3 += " AND convert(NVARCHAR(10), ep.EnrollTime, 23) <= '" + txtTimeE + "'";
                where4 += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <= '" + txtTimeE + "'";
            }
            if (!string.IsNullOrEmpty(enrollLevel))
            {
                where2 += OtherHelper.MultiSelectToSQLWhere(enrollLevel, "ep.EnrollLevel");
                where3 += OtherHelper.MultiSelectToSQLWhere(enrollLevel, "ep.EnrollLevel");
                where4 += OtherHelper.MultiSelectToSQLWhere(enrollLevel, "ep.EnrollLevel");
            }
            string cmdText = @"SELECT  p.Name MajorName ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
          WHERE     ep.sProfessionID = sp.sProfessionID
                    AND ep.Status <> 9
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort IN ( 8, 3 ) ) {1}
        ) TongZhao ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
          WHERE     ep.sProfessionID = sp.sProfessionID
                    AND ep.Status <> 9
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 4 ) {2}
        ) ChengJiao ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
          WHERE     ep.sProfessionID = sp.sProfessionID
                    AND ep.Status <> 9
                    AND ep.BeforeEnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 4 ) {3}
        ) ChengJiaoBefore ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
          WHERE     ep.sProfessionID = sp.sProfessionID
                    AND ep.Status <> 9
                    AND ep.BeforeEnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <> '1900-01-01'
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 4 ) {2}
        ) ChengJiaoZhuan ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
          WHERE     ep.sProfessionID = sp.sProfessionID
                    AND ep.Status <> 9
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 6 ) {1}
        ) ZhuanShenBen ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
          WHERE     ep.sProfessionID = sp.sProfessionID
                    AND ep.Status <> 9
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 7 ) {1}
        ) WuNian ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
          WHERE     ep.sProfessionID = sp.sProfessionID
                    AND ep.Status <> 9
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 5 ) {1}
        ) ZhongXiaoXue
FROM    dbo.T_Stu_sProfession sp
        LEFT JOIN dbo.T_Pro_Profession p ON p.ProfessionID = sp.ProfessionID
WHERE   1 = 1
{0}
ORDER BY p.EnglishName ASC";
            cmdText = string.Format(cmdText, where1, where2, where3, where4);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            return ResponseWriteResult(JsonGridData.GetGridJSON(dt));

        }



        public ActionResult GetFeeUserReport()
        {

            string menuId = Request.Form["MenuID"];
            string deptId = Request.Form["treeDeptID"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string where = string.Empty;
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where += " AND convert(NVARCHAR(10), f.FeeTime, 23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where += " AND convert(NVARCHAR(10), f.FeeTime, 23) <= '" + feeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                where += " and f.DeptID=" + deptId + "";
            }
            string cmdText = @"SELECT  u.Name FeeUser ,
        ( SELECT    COUNT(f.sFeeID)
          FROM      T_Stu_sFee f
          WHERE     f.Status <> 9
                    AND f.PaidMoney > 0
                    AND f.CreateID = u.UserID {0}
        ) VoucherNumCount ,
        ( SELECT    COUNT(DISTINCT f.sEnrollsProfessionID)
          FROM      T_Stu_sFee f
          WHERE     f.Status <> 9
                    AND f.PaidMoney > 0
                    AND f.CreateID = u.UserID {0}
        ) FeeStuCount ,
        ( SELECT    ISNULL(SUM(f1.ShouldMoney), 0)
          FROM      T_Stu_sFee f1
          WHERE     f1.sEnrollsProfessionID IN (
                    SELECT  f.sEnrollsProfessionID
                    FROM    T_Stu_sFee f
                    WHERE   f.Status <> 9
                            AND f.PaidMoney > 0
                            AND f.CreateID = u.UserID {0} )
        ) FeeShouldMoney ,
        ( SELECT    ISNULL(SUM(fo.PaidMoney), 0)
          FROM      T_Stu_sFeesOrder fo
          WHERE     fo.sFeeID IN ( SELECT   f.sFeeID
                                   FROM     T_Stu_sFee f
                                   WHERE    f.Status <> 9
                                            AND f.PaidMoney > 0
                                            AND f.CreateID = u.UserID {0} )
        ) FeePaidMoney ,
        ( SELECT    ISNULL(SUM(fo.DiscountMoney), 0)
          FROM      T_Stu_sFeesOrder fo
          WHERE     fo.sFeeID IN ( SELECT   f.sFeeID
                                   FROM     T_Stu_sFee f
                                   WHERE    f.Status <> 9
                                            AND f.PaidMoney > 0
                                            AND f.CreateID = u.UserID {0} )
        ) FeeDiscountMoney ,
        ( SELECT    ISNULL(SUM(fo.PaidMoney), 0)
          FROM      T_Stu_sFeesOrder fo
          WHERE     fo.sFeeID IN ( SELECT   f.sFeeID
                                   FROM     T_Stu_sFee f
                                   WHERE    f.Status <> 9
                                            AND f.PaidMoney > 0
                                            AND f.FeeMode = 1
                                            AND f.CreateID = u.UserID {0} )
        ) FeeMoney ,
        ( SELECT    ISNULL(SUM(fo.PaidMoney), 0)
          FROM      T_Stu_sFeesOrder fo
          WHERE     fo.sFeeID IN ( SELECT   f.sFeeID
                                   FROM     T_Stu_sFee f
                                   WHERE    f.Status <> 9
                                            AND f.PaidMoney > 0
                                            AND f.FeeMode = 2
                                            AND f.CreateID = u.UserID {0} )
        ) PosMoney ,
        ( SELECT    ISNULL(SUM(fo.PaidMoney), 0)
          FROM      T_Stu_sFeesOrder fo
          WHERE     fo.sFeeID IN ( SELECT   f.sFeeID
                                   FROM     T_Stu_sFee f
                                   WHERE    f.Status <> 9
                                            AND f.PaidMoney > 0
                                            AND f.FeeMode <> 1
                                            AND f.FeeMode <> 2
                                            AND f.CreateID = u.UserID {0} )
        ) Other
FROM    T_Sys_User u
WHERE   u.UserID IN ( SELECT    MIN(f.CreateID)
                      FROM      T_Stu_sFee f
                      WHERE     f.Status <> 9
                                AND f.PaidMoney > 0 {0}
                      GROUP BY  f.CreateID )";
            cmdText = string.Format(cmdText, where);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            return ResponseWriteResult(JsonGridData.GetGridJSON(dt));
        }


        public ActionResult GetFeeStudentSexList(string deptId)
        {
            string selYear = Request.Form["selYear"];
            string selMonth = Request.Form["selMonth"];
            string where1 = string.Empty;
            string where2 = string.Empty;
            if (!string.IsNullOrEmpty(deptId))
            {
                where2 += " AND d.DeptID = " + deptId + "";
            }
            if (!string.IsNullOrEmpty(selYear))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(selMonth))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selMonth, "ep.Month");
            }
            string cmdText = @"SELECT  d.Name DeptName ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND s.Sex = 1
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort IN ( 8, 3 ) ) {0}
        ) TongZhaoNan ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND s.Sex = 2
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort IN ( 8, 3 ) ) {0}
        ) TongZhaoNv ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND s.Sex = 1
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 4 ) {0}
        ) ChengJiaoNan ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND s.Sex = 2
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 4 ) {0}
        ) ChengJiaoNv ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.BeforeEnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <> '1900-01-01'
                    AND s.Sex = 1
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 4 ) {0}
        ) ChengJiaoBeforeNan ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.BeforeEnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <> '1900-01-01'
                    AND s.Sex = 2
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 4 ) {0}
        ) ChengJiaoBeforeNv ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.BeforeEnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <> '1900-01-01'
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND s.Sex = 1
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 4 ) {0}
        ) ChengJiaoZhuanNan ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.BeforeEnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <> '1900-01-01'
                    AND ep.EnrollTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <> '1900-01-01'
                    AND s.Sex = 2
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 4 ) {0}
        ) ChengJiaoZhuanNv ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND s.Sex = 1
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 6 ) {0}
        ) ZhuanShenBenNan ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND s.Sex = 2
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 6 ) {0}
        ) ZhuanShenBenNv ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND s.Sex = 1
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 7 ) {0}
        ) WuNianNan ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND s.Sex = 2
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 7 ) {0}
        ) WuNianNv ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND s.Sex = 1
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 5 ) {0}
        ) ZhongXiaoXueNan ,
        ( SELECT    COUNT(ep.sEnrollsProfessionID)
          FROM      dbo.T_Stu_sEnrollsProfession ep
                    LEFT JOIN dbo.T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = e.StudentID
          WHERE     ep.Status <> 9
                    AND ep.DeptID = d.DeptID
                    AND ep.FirstFeeTime IS NOT NULL
                    AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <> '1900-01-01'
                    AND s.Sex = 2
                    AND ep.sEnrollsProfessionID IN (
                    SELECT  o.sEnrollsProfessionID
                    FROM    T_Stu_sOrder o
                    WHERE   o.Status <> 9
                            AND o.PlanSort = 5 ) {0}
        ) ZhongXiaoXueNv
FROM    T_Sys_Dept d
WHERE   d.Status = 1
        AND d.DeptID <> 1 {1}";
            cmdText = string.Format(cmdText, where1, where2);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            return ResponseWriteResult(JsonGridData.GetGridJSON(dt));
        }


        public ActionResult GetDetailList()
        {
            string deptId = Request.Form["txtDeptID"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string menuId = Request.Form["MenuID"];
            string where1 = "";
            string where2 = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where1 += " AND f.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where1 += " AND convert(NVARCHAR(10),f.FeeTime,23) >= '" + feeTimeS + "'";
                where2 += " AND convert(NVARCHAR(10),f1.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where1 += " AND convert(NVARCHAR(10),f.FeeTime,23) <= '" + feeTimeE + "'";
                where2 += " AND convert(NVARCHAR(10),f1.FeeTime,23) <= '" + feeTimeE + "'";
            }
            string cmdText = @"SELECT  de.Name DeptName ,
        d.Name ,
        ( SELECT    ISNULL(SUM(fo1.PaidMoney), 0)
          FROM      T_Stu_sFeesOrder fo1
                    LEFT JOIN T_Stu_sFee f1 ON fo1.sFeeID = f1.sFeeID
                    LEFT JOIN T_Stu_sOrder o1 ON fo1.sOrderID = o1.sOrderID
                    LEFT JOIN T_Pro_Detail d1 ON o1.DetailID = d1.DetailID
          WHERE     f1.Status <> 9
                    AND f1.DeptID = f.DeptID
                    AND d1.DetailID = d.DetailID
                    {1}
        ) PaidMoney ,
        ( SELECT    ISNULL(SUM(fo1.DiscountMoney), 0)
          FROM      T_Stu_sFeesOrder fo1
                    LEFT JOIN T_Stu_sFee f1 ON fo1.sFeeID = f1.sFeeID
                    LEFT JOIN T_Stu_sOrder o1 ON fo1.sOrderID = o1.sOrderID
                    LEFT JOIN T_Pro_Detail d1 ON o1.DetailID = d1.DetailID
          WHERE     f1.Status <> 9
                    AND f1.DeptID = f.DeptID
                    AND d1.DetailID = d.DetailID
                    {1}
        ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(o1.Money), 0)
          FROM      T_Stu_sOffset o1
          WHERE     o1.Status = 1
                    AND o1.RelatedID IN (
                    SELECT  fo1.sFeesOrderID
                    FROM    T_Stu_sFeesOrder fo1
                            LEFT JOIN T_Stu_sFee f1 ON fo1.sFeeID = f1.sFeeID
                            LEFT JOIN T_Stu_sOrder o1 ON fo1.sOrderID = o1.sOrderID
                            LEFT JOIN T_Pro_Detail d1 ON o1.DetailID = d1.DetailID
                    WHERE   f1.Status <> 9
                            AND f1.DeptID = f.DeptID
                            AND d1.DetailID = d.DetailID
                            {1} )
        ) OffsetMoney ,
        ( SELECT    ISNULL(SUM(o2.Money), 0)
          FROM      T_Stu_sOffset o2
          WHERE     o2.Status = 1
                    AND o2.BySort = 1
                    AND o2.ByRelatedID IN (
                    SELECT  fo1.sFeesOrderID
                    FROM    T_Stu_sFeesOrder fo1
                            LEFT JOIN T_Stu_sFee f1 ON fo1.sFeeID = f1.sFeeID
                            LEFT JOIN T_Stu_sOrder o1 ON fo1.sOrderID = o1.sOrderID
                            LEFT JOIN T_Pro_Detail d1 ON o1.DetailID = d1.DetailID
                    WHERE   f1.Status <> 9
                            AND f1.DeptID = f.DeptID
                            AND d1.DetailID = d.DetailID
                            {1} )
        ) ByOffsetMoney ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Stu_sRefund r
          WHERE     r.Status = 1
                    AND r.sFeesOrderID IN (
                    SELECT  fo1.sFeesOrderID
                    FROM    T_Stu_sFeesOrder fo1
                            LEFT JOIN T_Stu_sFee f1 ON fo1.sFeeID = f1.sFeeID
                            LEFT JOIN T_Stu_sOrder o1 ON fo1.sOrderID = o1.sOrderID
                            LEFT JOIN T_Pro_Detail d1 ON o1.DetailID = d1.DetailID
                    WHERE   f1.Status <> 9
                            AND f1.DeptID = f.DeptID
                            AND d1.DetailID = d.DetailID
                            {1} )
        ) RefundMoney
FROM    T_Stu_sFeesOrder fo
        LEFT JOIN T_Stu_sFee f ON fo.sFeeID = f.sFeeID
        LEFT JOIN T_Stu_sOrder o ON fo.sOrderID = o.sOrderID
        LEFT JOIN T_Pro_Detail d ON o.DetailID = d.DetailID
        LEFT JOIN T_Sys_Dept de ON f.DeptID = de.DeptID
WHERE   f.Status <> 9
        {0}
GROUP BY de.Name ,
        d.Name ,
        d.DetailID ,
        f.DeptID
ORDER BY d.Name ASC";
            cmdText = string.Format(cmdText, where1, where2);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            return ResponseWriteResult(JsonGridData.GetGridJSON(dt));
        }
    }
}
