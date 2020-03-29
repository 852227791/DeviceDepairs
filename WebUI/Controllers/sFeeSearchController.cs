using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class sFeeSearchController : BaseController
    {
        //
        // GET: /sFeeSearch/
        /// <summary>
        /// 总费用
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public ActionResult GetsFeeAllList(SearchModel sm)
        {
            string menuId = Request.Form["MenuID"];
            string where = "";
            if (!string.IsNullOrEmpty(sm.treeDeptID))
            {
                where += " AND ep.DeptID IN ( SELECT   DeptID FROM GetChildrenDeptID(" + sm.treeDeptID + ") )";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollNum))
            {
                where += " AND e.EnrollNum LIKE '%" + sm.txtEnrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtStudentName))
            {
                where += " AND s.Name LIKE '%" + sm.txtStudentName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtIDCard))
            {
                where += " AND s.IDCard LIKE '%" + sm.txtIDCard + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selYear))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(sm.selMonth))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selMonth, "ep.Month");
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) >= '" + sm.txtBeforeEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) >= '" + sm.txtEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) >= '" + sm.txtFirstFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <= '" + sm.txtFirstFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.selStatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selStatus, "ep.Status");
            }
            if (!string.IsNullOrEmpty(sm.txtProfessionName))
            {
                where += " AND p.Name LIKE '%" + sm.txtProfessionName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selEnrollLevel))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selEnrollLevel, "ep.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(sm.selSort))
            {
                where += @" AND ep.sEnrollsProfessionID IN ( SELECT DISTINCT
                                                o.sEnrollsProfessionID
                                         FROM   T_Stu_sOrder o
                                         WHERE  o.Status != 9";
                where += OtherHelper.MultiSelectToSQLWhere(sm.selSort, "o.PlanSort");
                where += " )";
            }
            if (!string.IsNullOrEmpty(sm.treeDetail))
            {
                where += "";
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyS))
            {
                where += @" AND ( SELECT    ISNULL(SUM(o.ShouldMoney), 0) - ISNULL(SUM(o.PaidMoney),0)
              FROM T_Stu_sOrder o
         WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                        AND o.Status != 9
                        AND o.IsGive <> 2
                        AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
            ) >= " + sm.txtArrearsMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyE))
            {
                where += @" AND ( SELECT    ISNULL(SUM(o.ShouldMoney), 0) - ISNULL(SUM(o.PaidMoney),0)
              FROM T_Stu_sOrder o
         WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                        AND o.Status != 9
                        AND o.IsGive <> 2
                        AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
            ) <= " + sm.txtArrearsMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyS))
            {
                where += @" AND ( SELECT    ISNULL(SUM(o.PaidMoney), 0)
              FROM T_Stu_sOrder o
         WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                        AND o.Status != 9
            ) >= " + sm.txtPaidMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyE))
            {
                where += @" AND ( SELECT    ISNULL(SUM(o.PaidMoney), 0)
              FROM T_Stu_sOrder o
         WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                        AND o.Status != 9
            ) <= " + sm.txtPaidMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtLimitTimeS) || !string.IsNullOrEmpty(sm.txtLimitTimeE))
            {
                where += @" AND ep.sEnrollsProfessionID IN (
        SELECT DISTINCT
                o.sEnrollsProfessionID
        FROM    T_Stu_sOrder o
        WHERE   o.Status != 9 ";
                if (!string.IsNullOrEmpty(sm.txtLimitTimeS))
                {
                    where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) >= CONVERT(NVARCHAR(10), '" + sm.txtLimitTimeS + "', 23)";
                }
                if (!string.IsNullOrEmpty(sm.txtLimitTimeE))
                {
                    where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), '" + sm.txtLimitTimeE + "', 23)";
                }
                where += " )";
            }
            if (!string.IsNullOrEmpty(sm.selArrearsStatus))
            {
                string[] ArrearsStatusArr = sm.selArrearsStatus.Split(",");
                string bracketS = "(";
                string bracketE = ")";
                if (ArrearsStatusArr.Length < 2)
                {
                    bracketS = "";
                    bracketE = "";
                }
                for (int i = 0; i < ArrearsStatusArr.Length; i++)
                {
                    if (i == 0)
                    {
                        where += " and " + bracketS;
                    }
                    else
                    {
                        where += " or ";
                    }
                    switch (ArrearsStatusArr[i])
                    {
                        case "":
                            where += " 1 != 1";
                            break;
                        case "1":
                            where += @" ( SELECT  COUNT(o.sOrderID)
                    FROM    T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                  ) != 0 
                 AND ( SELECT  COUNT(o.sOrderID)
                    FROM T_Stu_sOrder o
                 WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                            AND o.ShouldMoney != o.PaidMoney
                  ) = 0";
                            break;
                        case "2":
                            where += @" ( SELECT  COUNT(o.sOrderID)
                    FROM    T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                            AND o.ShouldMoney > o.PaidMoney
                            AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                  ) > 0";
                            break;
                        case "3":
                            where += @" ( SELECT  COUNT(o.sOrderID)
                    FROM T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                            AND o.ShouldMoney != o.PaidMoney
                  ) != 0
                    AND ( SELECT  COUNT(o.sOrderID)
                    FROM    T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                            AND o.ShouldMoney > o.PaidMoney
                            AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                  ) = 0";
                            break;
                        case "4":
                            where += @" ( SELECT  COUNT(o.sOrderID)
                    FROM    T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                  ) = 0";
                            break;
                    }
                }
                where += bracketE;
            }
            string filedStr = "sEnrollsProfessionID";
            string cmdText = @"SELECT  ep.sEnrollsProfessionID ,
        r3.RefeName Status ,
        e.EnrollNum ,
        s.Name StudentName ,
        s.IDCard ,
        d.Name DeptName ,
        ( SELECT    r.RefeName + ','
          FROM      T_Stu_sOrder so
                    LEFT JOIN T_Sys_Refe r ON so.PlanSort = r.Value
                                              AND r.RefeTypeID = 14
          WHERE     so.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND so.Status <> 9
          GROUP BY  so.PlanItemID ,
                    r.RefeName
        FOR
          XML PATH('')
        ) PlanSort ,
        p.Name ProfessionName ,
        c.Name ClassName ,
        CASE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
        END BeforeEnrollTime ,
        CASE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
        END EnrollTime ,
        CASE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
        END FirstFeeTime ,
        ( SELECT    ISNULL(SUM(o.ShouldMoney), 0)
          FROM      T_Stu_sOrder o
          WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND o.Status != 9
        ) AS ShouldMoney ,
        ( SELECT    ISNULL(SUM(o.PaidMoney), 0)
          FROM      T_Stu_sOrder o
          WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND o.Status != 9
        ) AS PaidMoney ,
        ( SELECT    ISNULL(SUM(o.ShouldMoney), 0) - ISNULL(SUM(o.PaidMoney), 0)
          FROM      T_Stu_sOrder o
          WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND o.Status != 9
                    AND o.IsGive <> 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) AS ArrearsMoney ,
        ( SELECT    ISNULL(SUM(o.ShouldMoney), 0) - ISNULL(SUM(o.PaidMoney), 0)
          FROM      T_Stu_sOrder o
          WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND o.Status != 9
                    AND o.IsGive = 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) AS ArrearsMoneyGive ,
        ep.Year ,
        r1.RefeName Month ,
        r2.RefeName EnrollLevel
FROM    T_Stu_sEnrollsProfession ep
        LEFT JOIN T_Sys_Dept d ON ep.DeptID = d.DeptID
        LEFT JOIN T_Stu_sEnroll e ON ep.sEnrollID = e.sEnrollID
        LEFT JOIN T_Pro_Student s ON e.StudentID = s.StudentID
        LEFT JOIN T_Stu_sProfession AS sp ON ep.sProfessionID = sp.sProfessionID
        LEFT JOIN T_Pro_Profession AS p ON sp.ProfessionID = p.ProfessionID
        LEFT JOIN T_Pro_Class AS c ON ep.ClassID = c.ClassID
        LEFT JOIN T_Sys_Refe AS r1 ON ep.Month = r1.Value
                                      AND r1.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS r2 ON ep.EnrollLevel = r2.Value
                                      AND r2.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe AS r3 ON ep.Status = r3.Value
                                      AND r3.RefeTypeID = 21
WHERE   ep.Status != 9
        {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ep.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, filedStr, Request.Form));
        }

        /// <summary>
        /// 导出总学费
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public AjaxResult DownloadsFeeAllList(SearchModel sm)
        {
            string menuId = Request.Form["MenuID"];
            string where = "";
            if (!string.IsNullOrEmpty(sm.treeDeptID))
            {
                where += " AND ep.DeptID IN ( SELECT   DeptID FROM GetChildrenDeptID(" + sm.treeDeptID + ") )";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollNum))
            {
                where += " AND e.EnrollNum LIKE '%" + sm.txtEnrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtStudentName))
            {
                where += " AND s.Name LIKE '%" + sm.txtStudentName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtIDCard))
            {
                where += " AND s.IDCard LIKE '%" + sm.txtIDCard + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selYear))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(sm.selMonth))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selMonth, "ep.Month");
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) >= '" + sm.txtBeforeEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) >= '" + sm.txtEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) >= '" + sm.txtFirstFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <= '" + sm.txtFirstFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.selStatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selStatus, "ep.Status");
            }
            if (!string.IsNullOrEmpty(sm.txtProfessionName))
            {
                where += " AND p.Name LIKE '%" + sm.txtProfessionName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selEnrollLevel))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selEnrollLevel, "ep.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(sm.selSort))
            {
                where += @" AND ep.sEnrollsProfessionID IN ( SELECT DISTINCT
                                                o.sEnrollsProfessionID
                                         FROM   T_Stu_sOrder o
                                         WHERE  o.Status != 9";
                where += OtherHelper.MultiSelectToSQLWhere(sm.selSort, "o.PlanSort");
                where += " )";
            }
            if (!string.IsNullOrEmpty(sm.treeDetail))
            {
                where += "";
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyS))
            {
                where += @" AND ( SELECT    SUM(ISNULL(o.ShouldMoney, 0)) - SUM(ISNULL(o.PaidMoney,
                                                              0))
              FROM T_Stu_sOrder o
         WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                        AND o.Status != 9
                        AND o.IsGive <> 2
                        AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
            ) >= " + sm.txtArrearsMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyE))
            {
                where += @" AND ( SELECT    SUM(ISNULL(o.ShouldMoney, 0)) - SUM(ISNULL(o.PaidMoney,
                                                              0))
              FROM T_Stu_sOrder o
         WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                        AND o.Status != 9
                        AND o.IsGive <> 2
                        AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
            ) <= " + sm.txtArrearsMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyS))
            {
                where += @" AND ( SELECT    SUM(ISNULL(o.PaidMoney, 0))
              FROM T_Stu_sOrder o
         WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                        AND o.Status != 9
            ) >= " + sm.txtPaidMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyE))
            {
                where += @" AND ( SELECT    SUM(ISNULL(o.PaidMoney, 0))
              FROM T_Stu_sOrder o
         WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                        AND o.Status != 9
            ) <= " + sm.txtPaidMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtLimitTimeS) || !string.IsNullOrEmpty(sm.txtLimitTimeE))
            {
                where += @" AND ep.sEnrollsProfessionID IN (
        SELECT DISTINCT
                o.sEnrollsProfessionID
        FROM    T_Stu_sOrder o
        WHERE   o.Status != 9 ";
                if (!string.IsNullOrEmpty(sm.txtLimitTimeS))
                {
                    where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) >= CONVERT(NVARCHAR(10), '" + sm.txtLimitTimeS + "', 23)";
                }
                if (!string.IsNullOrEmpty(sm.txtLimitTimeE))
                {
                    where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), '" + sm.txtLimitTimeE + "', 23)";
                }
                where += " )";
            }
            if (!string.IsNullOrEmpty(sm.selArrearsStatus))
            {
                string[] ArrearsStatusArr = sm.selArrearsStatus.Split(",");
                string bracketS = "(";
                string bracketE = ")";
                if (ArrearsStatusArr.Length < 2)
                {
                    bracketS = "";
                    bracketE = "";
                }
                for (int i = 0; i < ArrearsStatusArr.Length; i++)
                {
                    if (i == 0)
                    {
                        where += " and " + bracketS;
                    }
                    else
                    {
                        where += " or ";
                    }
                    switch (ArrearsStatusArr[i])
                    {
                        case "":
                            where += " 1 != 1";
                            break;
                        case "1":
                            where += @" ( SELECT  COUNT(o.sOrderID)
                    FROM    T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                  ) != 0 
                 AND ( SELECT  COUNT(o.sOrderID)
                    FROM T_Stu_sOrder o
                 WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                            AND o.ShouldMoney != o.PaidMoney
                  ) = 0";
                            break;
                        case "2":
                            where += @" ( SELECT  COUNT(o.sOrderID)
                    FROM    T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                            AND o.ShouldMoney > o.PaidMoney
                            AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                  ) > 0";
                            break;
                        case "3":
                            where += @" ( SELECT  COUNT(o.sOrderID)
                    FROM T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                            AND o.ShouldMoney != o.PaidMoney
                  ) != 0
                    AND ( SELECT  COUNT(o.sOrderID)
                    FROM    T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                            AND o.ShouldMoney > o.PaidMoney
                            AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                  ) = 0";
                            break;
                        case "4":
                            where += @" ( SELECT  COUNT(o.sOrderID)
                    FROM    T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                  ) = 0";
                            break;
                    }
                }
                where += bracketE;
            }
            string cmdText = @"SELECT  r3.RefeName 在校状态 ,
        e.EnrollNum 学号 ,
        s.Name 学生姓名 ,
        s.IDCard 身份证号 ,
        d.Name 就读学校 ,
        ( SELECT    r.RefeName + ','
          FROM      T_Stu_sOrder so
                    LEFT JOIN T_Sys_Refe r ON so.PlanSort = r.Value
                                              AND r.RefeTypeID = 14
          WHERE     so.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND so.Status <> 9
          GROUP BY  so.PlanItemID ,
                    r.RefeName
        FOR
          XML PATH('')
        ) 报读类别 ,
        p.Name 报读专业 ,
        c.Name 班级 ,
        CASE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
        END 预报名日期 ,
        CASE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
        END 正式报名日期 ,
        CASE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
        END 首次缴费日期 ,
        ( SELECT    ISNULL(SUM(o.ShouldMoney), 0)
          FROM      T_Stu_sOrder o
          WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND o.Status != 9
        ) AS 总学费金额 ,
        ( SELECT    ISNULL(SUM(o.PaidMoney), 0)
          FROM      T_Stu_sOrder o
          WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND o.Status != 9
        ) AS 已供贷金额 ,
        ( SELECT    ISNULL(SUM(o.ShouldMoney), 0) - ISNULL(SUM(o.PaidMoney), 0)
          FROM      T_Stu_sOrder o
          WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND o.Status != 9
        ) 未供贷金额 ,
        ( SELECT    ISNULL(SUM(o.ShouldMoney), 0) - ISNULL(SUM(o.PaidMoney), 0)
          FROM      T_Stu_sOrder o
          WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND o.Status != 9
                    AND o.IsGive <> 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) AS 逾期欠费金额 ,
        ( SELECT    ISNULL(SUM(o.ShouldMoney), 0) - ISNULL(SUM(o.PaidMoney), 0)
          FROM      T_Stu_sOrder o
          WHERE     o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND o.Status != 9
                    AND o.IsGive = 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) AS '逾期欠费(资助)' ,
        ep.Year 年份 ,
        r1.RefeName 月份 ,
        r2.RefeName 学历层次 ,
        CASE WHEN ( SELECT  ISNULL(SUM(o.ShouldMoney), 0)
                    FROM    T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                  ) = 0 THEN '未收'
             WHEN ( SELECT  ISNULL(SUM(o.ShouldMoney), 0)
                            - ISNULL(SUM(o.PaidMoney), 0)
                    FROM    T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                  ) = 0 THEN '已清'
             WHEN ( SELECT  ISNULL(SUM(o.ShouldMoney), 0)
                            - ISNULL(SUM(o.PaidMoney), 0)
                    FROM    T_Stu_sOrder o
                    WHERE   o.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND o.Status != 9
                            AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                  ) = 0 THEN '正常'
             ELSE '逾期'
        END 逾期状态
FROM    T_Stu_sEnrollsProfession ep
        LEFT JOIN T_Sys_Dept d ON ep.DeptID = d.DeptID
        LEFT JOIN T_Stu_sEnroll e ON ep.sEnrollID = e.sEnrollID
        LEFT JOIN T_Pro_Student s ON e.StudentID = s.StudentID
        LEFT JOIN T_Stu_sProfession AS sp ON ep.sProfessionID = sp.sProfessionID
        LEFT JOIN T_Pro_Profession AS p ON sp.ProfessionID = p.ProfessionID
        LEFT JOIN T_Pro_Class AS c ON ep.ClassID = c.ClassID
        LEFT JOIN T_Sys_Refe AS r1 ON ep.Month = r1.Value
                                      AND r1.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS r2 ON ep.EnrollLevel = r2.Value
                                      AND r2.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe AS r3 ON ep.Status = r3.Value
                                      AND r3.RefeTypeID = 21
WHERE   ep.Status != 9 {0}
ORDER BY ep.FirstFeeTime ASC";
            string filename = "总学费信息.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ep.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }
        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public ActionResult GetsFeeDetailList(SearchModel sm)
        {
            string menuId = Request.Form["MenuID"];
            string where = "";
            if (!string.IsNullOrEmpty(sm.treeDeptID))
            {
                where += " AND ep.DeptID IN ( SELECT   DeptID FROM GetChildrenDeptID(" + sm.treeDeptID + ") )";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollNum))
            {
                where += " AND e.EnrollNum LIKE '%" + sm.txtEnrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtStudentName))
            {
                where += " AND s.Name LIKE '%" + sm.txtStudentName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtIDCard))
            {
                where += " AND s.IDCard LIKE '%" + sm.txtIDCard + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selYear))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(sm.selMonth))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selMonth, "ep.Month");
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) >= '" + sm.txtBeforeEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) >= '" + sm.txtEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) >= '" + sm.txtFirstFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <= '" + sm.txtFirstFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.selStatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selStatus, "ep.Status");
            }
            if (!string.IsNullOrEmpty(sm.txtProfessionName))
            {
                where += " AND p.Name LIKE '%" + sm.txtProfessionName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selEnrollLevel))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selEnrollLevel, "ep.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(sm.selSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selSort, "o.PlanSort");
            }
            if (!string.IsNullOrEmpty(sm.treeDetail))
            {
                where += " AND o.DetailID = " + sm.treeDetail + "";
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyS))
            {
                where += " AND o.ShouldMoney - o.PaidMoney >= " + sm.txtArrearsMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyE))
            {
                where += " AND o.ShouldMoney - o.PaidMoney <= " + sm.txtArrearsMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyS))
            {
                where += " AND o.PaidMoney >= " + sm.txtPaidMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyE))
            {
                where += " AND o.PaidMoney <= " + sm.txtPaidMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtLimitTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) >= '" + sm.txtLimitTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtLimitTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= '" + sm.txtLimitTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.selArrearsStatus))
            {
                string[] ArrearsStatusArr = sm.selArrearsStatus.Split(",");
                string bracketS = "(";
                string bracketE = ")";
                if (ArrearsStatusArr.Length < 2)
                {
                    bracketS = "";
                    bracketE = "";
                }
                for (int i = 0; i < ArrearsStatusArr.Length; i++)
                {
                    if (i == 0)
                    {
                        where += " and " + bracketS;
                    }
                    else
                    {
                        where += " or ";
                    }
                    switch (ArrearsStatusArr[i])
                    {
                        case "":
                            where += " 1 != 1";
                            break;
                        case "1":
                            where += " o.ShouldMoney = o.PaidMoney";
                            break;
                        case "2":
                            where += @" o.ShouldMoney > o.PaidMoney
                  AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)";
                            break;
                        case "3":
                            where += @" o.ShouldMoney > o.PaidMoney
                  AND CONVERT(NVARCHAR(10), o.LimitTime, 23) > CONVERT(NVARCHAR(10), GETDATE(), 23)";
                            break;
                        case "4":
                            where += " o.ShouldMoney = 0";
                            break;
                    }
                }
                where += bracketE;
            }
            string filedStr = "sOrderID";
            string cmdText = @"SELECT  o.sOrderID ,
        r1.RefeName Status ,
        e.EnrollNum ,
        s.Name StudentName ,
        s.IDCard ,
        d.Name DeptName ,
        r4.RefeName PlanSort ,
        p.Name ProfessionName ,
        c.Name ClassName ,
        CASE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
        END BeforeEnrollTime ,
        CASE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
        END EnrollTime ,
        CASE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
        END FirstFeeTime ,
        CASE CONVERT(NVARCHAR(10), o.LimitTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), o.LimitTime, 23)
        END LimitTime ,
        o.NumName ,
        de.Name DetailName ,
        o.ShouldMoney ,
        o.PaidMoney ,
        o.ShouldMoney - o.PaidMoney NoMoney ,
        ep.Year ,
        r2.RefeName Month ,
        r3.RefeName EnrollLevel ,
        CASE WHEN o.ShouldMoney = 0 THEN '未收'
             WHEN o.ShouldMoney = o.PaidMoney
             THEN '<span style=color:#339900>已清</span>'
             WHEN o.ShouldMoney > o.PaidMoney
                  AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
             THEN '<span style=color:#ff0000>逾期</span>'
             ELSE '<span style=color:#1d5ea0>正常</span>'
        END ArrearsStatus
FROM    T_Stu_sOrder o
        LEFT JOIN T_Stu_sEnrollsProfession ep ON o.sEnrollsProfessionID = ep.sEnrollsProfessionID
        LEFT JOIN T_Sys_Dept d ON ep.DeptID = d.DeptID
        LEFT JOIN T_Stu_sEnroll e ON ep.sEnrollID = e.sEnrollID
        LEFT JOIN T_Pro_Student s ON e.StudentID = s.StudentID
        LEFT JOIN T_Stu_sProfession AS sp ON ep.sProfessionID = sp.sProfessionID
        LEFT JOIN T_Pro_Profession AS p ON sp.ProfessionID = p.ProfessionID
        LEFT JOIN T_Pro_Class AS c ON ep.ClassID = c.ClassID
        LEFT JOIN T_Pro_Detail de ON o.DetailID = de.DetailID
        LEFT JOIN T_Sys_Refe AS r1 ON ep.Status = r1.Value
                                      AND r1.RefeTypeID = 21
        LEFT JOIN T_Sys_Refe AS r2 ON ep.Month = r2.Value
                                      AND r2.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS r3 ON ep.EnrollLevel = r3.Value
                                      AND r3.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe r4 ON o.PlanSort = r4.Value
                                   AND r4.RefeTypeID = 14
WHERE   o.Status != 9
        {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ep.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, filedStr, Request.Form));//, "ShouldMoney,PaidMoney,NoMoney"
        }
        /// <summary>
        /// 导出学费明细
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public AjaxResult DownloadsFeeDetailList(SearchModel sm)
        {
            string menuId = Request.Form["MenuID"];
            string where = "";
            if (!string.IsNullOrEmpty(sm.treeDeptID))
            {
                where += " AND ep.DeptID IN ( SELECT   DeptID FROM GetChildrenDeptID(" + sm.treeDeptID + ") )";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollNum))
            {
                where += " AND e.EnrollNum LIKE '%" + sm.txtEnrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtStudentName))
            {
                where += " AND s.Name LIKE '%" + sm.txtStudentName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtIDCard))
            {
                where += " AND s.IDCard LIKE '%" + sm.txtIDCard + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selYear))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(sm.selMonth))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selMonth, "ep.Month");
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) >= '" + sm.txtBeforeEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) >= '" + sm.txtEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) >= '" + sm.txtFirstFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <= '" + sm.txtFirstFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.selStatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selStatus, "ep.Status");
            }
            if (!string.IsNullOrEmpty(sm.txtProfessionName))
            {
                where += " AND p.Name LIKE '%" + sm.txtProfessionName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selEnrollLevel))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selEnrollLevel, "ep.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(sm.selSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selSort, "o.PlanSort");
            }
            if (!string.IsNullOrEmpty(sm.treeDetail))
            {
                where += " AND o.DetailID = " + sm.treeDetail + "";
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyS))
            {
                where += " AND o.ShouldMoney - o.PaidMoney >= " + sm.txtArrearsMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyE))
            {
                where += " AND o.ShouldMoney - o.PaidMoney <= " + sm.txtArrearsMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyS))
            {
                where += " AND o.PaidMoney >= " + sm.txtPaidMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyE))
            {
                where += " AND o.PaidMoney <= " + sm.txtPaidMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtLimitTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) >= '" + sm.txtLimitTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtLimitTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= '" + sm.txtLimitTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.selArrearsStatus))
            {
                string[] ArrearsStatusArr = sm.selArrearsStatus.Split(",");
                string bracketS = "(";
                string bracketE = ")";
                if (ArrearsStatusArr.Length < 2)
                {
                    bracketS = "";
                    bracketE = "";
                }
                for (int i = 0; i < ArrearsStatusArr.Length; i++)
                {
                    if (i == 0)
                    {
                        where += " and " + bracketS;
                    }
                    else
                    {
                        where += " or ";
                    }
                    switch (ArrearsStatusArr[i])
                    {
                        case "":
                            where += " 1 != 1";
                            break;
                        case "1":
                            where += " o.ShouldMoney = o.PaidMoney";
                            break;
                        case "2":
                            where += @" o.ShouldMoney > o.PaidMoney
                  AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)";
                            break;
                        case "3":
                            where += @" o.ShouldMoney > o.PaidMoney
                  AND CONVERT(NVARCHAR(10), o.LimitTime, 23) > CONVERT(NVARCHAR(10), GETDATE(), 23)";
                            break;
                        case "4":
                            where += " o.ShouldMoney = 0";
                            break;
                    }
                }
                where += bracketE;
            }
            string cmdText = @"SELECT  r1.RefeName 在校状态 ,
        e.EnrollNum 学号 ,
        s.Name 学生姓名 ,
        s.IDCard 身份证号 ,
        d.Name 就读学校 ,
        r4.RefeName 报读类别 ,
        p.Name 报读专业 ,
        c.Name 班级 ,
        CASE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
        END 预报名日期 ,
        CASE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
        END 正式报名日期 ,
        CASE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
        END 首次缴费日期 ,
        CASE CONVERT(NVARCHAR(10), o.LimitTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), o.LimitTime, 23)
        END 应供贷日期 ,
        o.NumName 缴费学年 ,
        de.Name 费用类别 ,
        o.ShouldMoney 应供贷金额 ,
        o.PaidMoney 已供贷金额 ,
        o.ShouldMoney - o.PaidMoney 未供贷金额 ,
        ep.Year 年份 ,
        r2.RefeName 月份 ,
        r3.RefeName 学历层次 ,
        CASE WHEN o.ShouldMoney = 0 THEN '未收'
             WHEN o.ShouldMoney = o.PaidMoney
             THEN '<span style=color:#339900>已清</span>'
             WHEN o.ShouldMoney > o.PaidMoney
                  AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
             THEN '<span style=color:#ff0000>逾期</span>'
             ELSE '<span style=color:#1d5ea0>正常</span>'
        END 逾期状态
FROM    T_Stu_sOrder o
        LEFT JOIN T_Stu_sEnrollsProfession ep ON o.sEnrollsProfessionID = ep.sEnrollsProfessionID
        LEFT JOIN T_Sys_Dept d ON ep.DeptID = d.DeptID
        LEFT JOIN T_Stu_sEnroll e ON ep.sEnrollID = e.sEnrollID
        LEFT JOIN T_Pro_Student s ON e.StudentID = s.StudentID
        LEFT JOIN T_Stu_sProfession AS sp ON ep.sProfessionID = sp.sProfessionID
        LEFT JOIN T_Pro_Profession AS p ON sp.ProfessionID = p.ProfessionID
        LEFT JOIN T_Pro_Class AS c ON ep.ClassID = c.ClassID
        LEFT JOIN T_Pro_Detail de ON o.DetailID = de.DetailID
        LEFT JOIN T_Sys_Refe AS r1 ON ep.Status = r1.Value
                                      AND r1.RefeTypeID = 21
        LEFT JOIN T_Sys_Refe AS r2 ON ep.Month = r2.Value
                                      AND r2.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS r3 ON ep.EnrollLevel = r3.Value
                                      AND r3.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe r4 ON o.PlanSort = r4.Value
                                   AND r4.RefeTypeID = 14
WHERE   o.Status != 9
        {0}
ORDER BY o.sOrderID ASC";
            string filename = "学费供贷明细.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ep.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }

        public ActionResult GetsFeesOrderList()
        {
            string menuId = Request.Form["MenuID"];
            string treeDeptID = Request.Form["treeDeptID"];
            string txtVoucherNum = Request.Form["txtVoucherNum"];
            string txtNoteNum = Request.Form["txtNoteNum"];
            string txtEnrollNum = Request.Form["txtEnrollNum"];
            string txtStudentName = Request.Form["txtStudentName"];
            string txtIDCard = Request.Form["txtIDCard"];
            string selYear = Request.Form["selYear"];
            string selMonth = Request.Form["selMonth"];
            string selFeeMode = Request.Form["selFeeMode"];
            string txtShouldFeeTimeS = Request.Form["txtShouldFeeTimeS"];
            string txtShouldFeeTimeE = Request.Form["txtShouldFeeTimeE"];
            string txtFeeTimeS = Request.Form["txtFeeTimeS"];
            string txtFeeTimeE = Request.Form["txtFeeTimeE"];
            string selEnrollLevel = Request.Form["selEnrollLevel"];
            string selSort = Request.Form["selSort"];
            string treeDetail = Request.Form["treeDetail"];
            string txtFeeName = Request.Form["txtFeeName"];
            string where = "";
            if (!string.IsNullOrEmpty(treeDeptID))
            {
                where += " AND f.DeptID IN ( SELECT   DeptID FROM GetChildrenDeptID(" + treeDeptID + ") )";
            }
            if (!string.IsNullOrEmpty(txtVoucherNum))
            {
                where += " AND f.VoucherNum LIKE '%" + txtVoucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(txtNoteNum))
            {
                where += " AND f.NoteNum LIKE '%" + txtNoteNum + "%'";
            }
            if (!string.IsNullOrEmpty(txtEnrollNum))
            {
                where += " AND e.EnrollNum LIKE '%" + txtEnrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(txtStudentName))
            {
                where += " AND s.Name LIKE '%" + txtStudentName + "%'";
            }
            if (!string.IsNullOrEmpty(txtIDCard))
            {
                where += " AND s.IDCard LIKE '%" + txtIDCard + "%'";
            }
            if (!string.IsNullOrEmpty(selYear))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(selMonth))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selMonth, "ep.Month");
            }
            if (!string.IsNullOrEmpty(selFeeMode))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selFeeMode, "f.FeeMode");
            }
            if (!string.IsNullOrEmpty(txtShouldFeeTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) >= '" + txtShouldFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtShouldFeeTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= '" + txtShouldFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(txtFeeTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), f.FeeTime, 23) >= '" + txtFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtFeeTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), f.FeeTime, 23) <= '" + txtFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(selEnrollLevel))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selEnrollLevel, "ep.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(selSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selSort, "o.PlanSort");
            }
            if (!string.IsNullOrEmpty(treeDetail))
            {
                where += " AND o.DetailID = " + treeDetail + "";
            }
            if (!string.IsNullOrEmpty(txtFeeName))
            {
                where += " AND u.Name LIKE '%" + txtFeeName + "%'";
            }
            string filedStr = "sFeesOrderID";
            string cmdText = @"SELECT  fo.sFeesOrderID ,
        f.VoucherNum ,
        f.NoteNum ,
        s.Name StudentName ,
        e.EnrollNum ,
        s.IDCard ,
        d.Name DeptName ,
        r1.RefeName FeeMode ,
        CASE CONVERT(NVARCHAR(10), o.LimitTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), o.LimitTime, 23)
        END LimitTime ,
        CASE CONVERT(NVARCHAR(10), f.FeeTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), f.FeeTime, 23)
        END FeeTime ,
        de.Name DetailName ,
        o.ShouldMoney ,
        fo.PaidMoney ,
        fo.DiscountMoney ,
        ( SELECT    ISNULL(SUM(of1.Money), 0)
          FROM      T_Stu_sOffset of1
          WHERE     of1.Status = 1
                    AND of1.RelatedID = fo.sFeesOrderID
        ) OffsetMoney ,
        ( SELECT    ISNULL(SUM(of1.Money), 0)
          FROM      T_Stu_sOffset of1
          WHERE     of1.Status = 1
                    AND of1.BySort = 1
                    AND of1.ByRelatedID = fo.sFeesOrderID
        ) ByOffsetMoney ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Stu_sRefund r
          WHERE     r.Status = 1
                    AND r.sFeesOrderID = fo.sFeesOrderID
        ) RefundMoney ,
        ep.Year ,
        r2.RefeName Month ,
        r3.RefeName EnrollLevel ,
        r4.RefeName PlanSort ,
        u.Name UserName
FROM    T_Stu_sFeesOrder fo
        LEFT JOIN T_Stu_sFee f ON fo.sFeeID = f.sFeeID
        LEFT JOIN T_Stu_sEnrollsProfession ep ON f.sEnrollsProfessionID = ep.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON ep.sEnrollID = e.sEnrollID
        LEFT JOIN T_Pro_Student s ON e.StudentID = s.StudentID
        LEFT JOIN T_Sys_Dept d ON f.DeptID = d.DeptID
        LEFT JOIN T_Sys_Refe AS r1 ON f.FeeMode = r1.Value
                                      AND r1.RefeTypeID = 6
        LEFT JOIN T_Stu_sOrder o ON fo.sOrderID = o.sOrderID
        LEFT JOIN T_Pro_Detail de ON o.DetailID = de.DetailID
        LEFT JOIN T_Sys_User u ON f.CreateID = u.UserID
        LEFT JOIN T_Sys_Refe AS r2 ON ep.Month = r2.Value
                                      AND r2.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS r3 ON ep.EnrollLevel = r3.Value
                                      AND r3.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe r4 ON o.PlanSort = r4.Value
                                   AND r4.RefeTypeID = 14
WHERE   1 = 1
        {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "f.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, filedStr, Request.Form, "ShouldMoney,PaidMoney,DiscountMoney,OffsetMoney,ByOffsetMoney,RefundMoney"));
        }
        public AjaxResult DownloadsFeesOrderList(SearchModel sm)
        {
            string menuId = Request.Form["MenuID"];
            string treeDeptID = Request.Form["treeDeptID"];
            string txtVoucherNum = Request.Form["txtVoucherNum"];
            string txtNoteNum = Request.Form["txtNoteNum"];
            string txtEnrollNum = Request.Form["txtEnrollNum"];
            string txtStudentName = Request.Form["txtStudentName"];
            string txtIDCard = Request.Form["txtIDCard"];
            string selYear = Request.Form["selYear"];
            string selMonth = Request.Form["selMonth"];
            string selFeeMode = Request.Form["selFeeMode"];
            string txtShouldFeeTimeS = Request.Form["txtShouldFeeTimeS"];
            string txtShouldFeeTimeE = Request.Form["txtShouldFeeTimeE"];
            string txtFeeTimeS = Request.Form["txtFeeTimeS"];
            string txtFeeTimeE = Request.Form["txtFeeTimeE"];
            string selEnrollLevel = Request.Form["selEnrollLevel"];
            string selSort = Request.Form["selSort"];
            string treeDetail = Request.Form["treeDetail"];
            string txtFeeName = Request.Form["txtFeeName"];
            string where = "";
            if (!string.IsNullOrEmpty(treeDeptID))
            {
                where += " AND f.DeptID IN ( SELECT   DeptID FROM GetChildrenDeptID(" + treeDeptID + ") )";
            }
            if (!string.IsNullOrEmpty(txtVoucherNum))
            {
                where += " AND f.VoucherNum LIKE '%" + txtVoucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(txtNoteNum))
            {
                where += " AND f.NoteNum LIKE '%" + txtNoteNum + "%'";
            }
            if (!string.IsNullOrEmpty(txtEnrollNum))
            {
                where += " AND e.EnrollNum LIKE '%" + txtEnrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(txtStudentName))
            {
                where += " AND s.Name LIKE '%" + txtStudentName + "%'";
            }
            if (!string.IsNullOrEmpty(txtIDCard))
            {
                where += " AND s.IDCard LIKE '%" + txtIDCard + "%'";
            }
            if (!string.IsNullOrEmpty(selYear))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(selMonth))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selMonth, "ep.Month");
            }
            if (!string.IsNullOrEmpty(selFeeMode))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selFeeMode, "f.FeeMode");
            }
            if (!string.IsNullOrEmpty(txtShouldFeeTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) >= '" + txtShouldFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtShouldFeeTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= '" + txtShouldFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(txtFeeTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), f.FeeTime, 23) >= '" + txtFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtFeeTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), f.FeeTime, 23) <= '" + txtFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(selEnrollLevel))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selEnrollLevel, "ep.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(selSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selSort, "o.PlanSort");
            }
            if (!string.IsNullOrEmpty(treeDetail))
            {
                where += " AND o.DetailID = " + treeDetail + "";
            }
            if (!string.IsNullOrEmpty(txtFeeName))
            {
                where += " AND u.Name LIKE '%" + txtFeeName + "%'";
            }
            string cmdText = @"SELECT  f.VoucherNum 凭证号 ,
        f.NoteNum 票据号 ,
        s.Name 学生姓名 ,
        e.EnrollNum 学号 ,
        s.IDCard 身份证号 ,
        d.Name 收费学校 ,
        r1.RefeName 交费方式 ,
        CASE CONVERT(NVARCHAR(10), o.LimitTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), o.LimitTime, 23)
        END 应供款日期 ,
        CASE CONVERT(NVARCHAR(10), f.FeeTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), f.FeeTime, 23)
        END 实际供款日期 ,
        de.Name 费用类别 ,
        o.ShouldMoney 应供款金额 ,
        fo.PaidMoney 实际供款金额 ,
        fo.DiscountMoney 优惠金额 ,
        ( SELECT    ISNULL(SUM(of1.Money), 0)
          FROM      T_Stu_sOffset of1
          WHERE     of1.Status = 1
                    AND of1.RelatedID = fo.sFeesOrderID
        ) 充抵金额 ,
        ( SELECT    ISNULL(SUM(of1.Money), 0)
          FROM      T_Stu_sOffset of1
          WHERE     of1.Status = 1
                    AND of1.BySort = 1
                    AND of1.ByRelatedID = fo.sFeesOrderID
        ) 被充抵金额 ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Stu_sRefund r
          WHERE     r.Status = 1
                    AND r.sFeesOrderID = fo.sFeesOrderID
        ) 核销金额 ,
        ep.Year 年份 ,
        r2.RefeName 月份 ,
        r3.RefeName 学历层次 ,
        r4.RefeName 报读类别 ,
        u.Name 收费员
FROM    T_Stu_sFeesOrder fo
        LEFT JOIN T_Stu_sFee f ON fo.sFeeID = f.sFeeID
        LEFT JOIN T_Stu_sEnrollsProfession ep ON f.sEnrollsProfessionID = ep.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON ep.sEnrollID = e.sEnrollID
        LEFT JOIN T_Pro_Student s ON e.StudentID = s.StudentID
        LEFT JOIN T_Sys_Dept d ON f.DeptID = d.DeptID
        LEFT JOIN T_Sys_Refe AS r1 ON f.FeeMode = r1.Value
                                      AND r1.RefeTypeID = 6
        LEFT JOIN T_Stu_sOrder o ON fo.sOrderID = o.sOrderID
        LEFT JOIN T_Pro_Detail de ON o.DetailID = de.DetailID
        LEFT JOIN T_Sys_User u ON f.CreateID = u.UserID
        LEFT JOIN T_Sys_Refe AS r2 ON ep.Month = r2.Value
                                      AND r2.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS r3 ON ep.EnrollLevel = r3.Value
                                      AND r3.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe r4 ON o.PlanSort = r4.Value
                                   AND r4.RefeTypeID = 14
WHERE   1 = 1
        {0}
ORDER BY fo.sFeesOrderID ASC";
            string filename = "学费供贷收取明细.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "f.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }

        public ActionResult GetOrderByYear(SearchModel sm)
        {
            string menuId = Request.Form["MenuID"];
            string where = "";
            if (!string.IsNullOrEmpty(sm.treeDeptID))
            {
                where += " AND ep.DeptID IN ( SELECT   DeptID FROM GetChildrenDeptID(" + sm.treeDeptID + ") )";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollNum))
            {
                where += " AND e.EnrollNum LIKE '%" + sm.txtEnrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtStudentName))
            {
                where += " AND s.Name LIKE '%" + sm.txtStudentName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtIDCard))
            {
                where += " AND s.IDCard LIKE '%" + sm.txtIDCard + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selYear))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(sm.selMonth))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selMonth, "ep.Month");
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) >= '" + sm.txtBeforeEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) >= '" + sm.txtEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) >= '" + sm.txtFirstFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <= '" + sm.txtFirstFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.selStatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selStatus, "ep.Status");
            }
            if (!string.IsNullOrEmpty(sm.txtProfessionName))
            {
                where += " AND p.Name LIKE '%" + sm.txtProfessionName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selEnrollLevel))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selEnrollLevel, "ep.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(sm.selSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selSort, "o.PlanSort");
            }
            if (!string.IsNullOrEmpty(sm.treeDetail))
            {
                where += "";// AND o.DetailID = " + sm.treeDetail + "
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyS))
            {
                where += @" AND ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
                    AND IsGive <> 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) >= " + sm.txtArrearsMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyE))
            {
                where += @" AND ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
                    AND IsGive <> 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) <= " + sm.txtArrearsMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyS))
            {
                where += @" AND ( SELECT    ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
        ) >= " + sm.txtPaidMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyE))
            {
                where += @" AND ( SELECT    ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
        ) <= " + sm.txtPaidMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtLimitTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) >= '" + sm.txtLimitTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtLimitTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= '" + sm.txtLimitTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.selArrearsStatus))
            {
                string[] ArrearsStatusArr = sm.selArrearsStatus.Split(",");
                string bracketS = "(";
                string bracketE = ")";
                if (ArrearsStatusArr.Length < 2)
                {
                    bracketS = "";
                    bracketE = "";
                }
                for (int i = 0; i < ArrearsStatusArr.Length; i++)
                {
                    if (i == 0)
                    {
                        where += " and " + bracketS;
                    }
                    else
                    {
                        where += " or ";
                    }
                    switch (ArrearsStatusArr[i])
                    {
                        case "":
                            where += " 1 != 1";
                            break;
                        case "1":
                            where += @" ( SELECT  COUNT(so.sOrderID)
                    FROM    T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                  ) != 0 
                 AND ( SELECT  COUNT(so.sOrderID)
                    FROM T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                            AND so.ShouldMoney != so.PaidMoney
                  ) = 0";
                            break;
                        case "2":
                            where += @" ( SELECT  COUNT(so.sOrderID)
                    FROM    T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                            AND so.ShouldMoney > so.PaidMoney
                            AND CONVERT(NVARCHAR(10), so.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                  ) > 0";
                            break;
                        case "3":
                            where += @" ( SELECT  COUNT(so.sOrderID)
                    FROM T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                            AND so.ShouldMoney != so.PaidMoney
                  ) != 0
                    AND ( SELECT  COUNT(so.sOrderID)
                    FROM    T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                            AND so.ShouldMoney > so.PaidMoney
                            AND CONVERT(NVARCHAR(10), so.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                  ) = 0";
                            break;
                        case "4":
                            where += @" ( SELECT  COUNT(so.sOrderID)
                    FROM    T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                  ) = 0";
                            break;
                    }
                }
                where += bracketE;
            }
            string filedStr = "sOrderID";
            string cmdText = @"SELECT  o.sOrderID ,
        r1.RefeName Status ,
        e.EnrollNum ,
        s.Name StudentName ,
        s.IDCard ,
        d.Name DeptName ,
        r3.RefeName PlanSort ,
        p.Name ProfessionName ,
        c.Name ClassName ,
        CASE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
        END BeforeEnrollTime ,
        CASE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
        END EnrollTime ,
        CASE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
        END FirstFeeTime ,
        CASE CONVERT(NVARCHAR(10), o.LimitTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), o.LimitTime, 23)
        END LimitTime ,
        o.NumName ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
        ) ShouldMoney ,
        ( SELECT    ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
        ) PaidMoney ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
                    AND IsGive <> 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) ArrearsMoney ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
                    AND IsGive = 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) ArrearsMoneyGive ,
        ep.Year ,
        ep.Month ,
        r2.RefeName EnrollLevel
FROM    T_Stu_sOrder o
        LEFT JOIN T_Stu_sEnrollsProfession ep ON o.sEnrollsProfessionID = ep.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON ep.sEnrollID = e.sEnrollID
        LEFT JOIN T_Pro_Student s ON e.StudentID = s.StudentID
        LEFT JOIN T_Sys_Dept d ON ep.DeptID = d.DeptID
        LEFT JOIN T_Stu_sProfession sp ON sp.sProfessionID = ep.sProfessionID
        LEFT JOIN T_Pro_Profession p ON p.ProfessionID = sp.ProfessionID
        LEFT JOIN T_Pro_Class AS c ON ep.ClassID = c.ClassID
        LEFT JOIN T_Sys_Refe r1 ON ep.Status = r1.Value
                                  AND r1.RefeTypeID = 21
        LEFT JOIN T_Sys_Refe r2 ON ep.EnrollLevel = r2.Value
                                   AND r2.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe r3 ON o.PlanSort = r3.Value
                                   AND r3.RefeTypeID = 14
WHERE   o.sOrderID IN ( SELECT  MIN(sOrderID)
                        FROM    T_Stu_sOrder
                        WHERE   Status <> 9
                        GROUP BY sEnrollsProfessionID ,
                                PlanItemID ,
                                NumItemID ) {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ep.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, filedStr, Request.Form));
        }

        public AjaxResult DownloadsFeeByYear(SearchModel sm)
        {
            string menuId = Request.Form["MenuID"];
            string where = "";
            if (!string.IsNullOrEmpty(sm.treeDeptID))
            {
                where += " AND ep.DeptID IN ( SELECT   DeptID FROM GetChildrenDeptID(" + sm.treeDeptID + ") )";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollNum))
            {
                where += " AND e.EnrollNum LIKE '%" + sm.txtEnrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtStudentName))
            {
                where += " AND s.Name LIKE '%" + sm.txtStudentName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.txtIDCard))
            {
                where += " AND s.IDCard LIKE '%" + sm.txtIDCard + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selYear))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(sm.selMonth))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selMonth, "ep.Month");
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) >= '" + sm.txtBeforeEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtBeforeEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) >= '" + sm.txtEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtEnrollTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <= '" + sm.txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) >= '" + sm.txtFirstFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtFirstFeeTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <= '" + sm.txtFirstFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.selStatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selStatus, "ep.Status");
            }
            if (!string.IsNullOrEmpty(sm.txtProfessionName))
            {
                where += " AND p.Name LIKE '%" + sm.txtProfessionName + "%'";
            }
            if (!string.IsNullOrEmpty(sm.selEnrollLevel))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selEnrollLevel, "ep.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(sm.selSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sm.selSort, "o.PlanSort");
            }
            if (!string.IsNullOrEmpty(sm.treeDetail))
            {
                where += "";// AND o.DetailID = " + sm.treeDetail + "
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyS))
            {
                where += @" AND ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
                    AND IsGive <> 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) >= " + sm.txtArrearsMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtArrearsMoneyE))
            {
                where += @" AND ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
                    AND IsGive <> 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) <= " + sm.txtArrearsMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyS))
            {
                where += @" AND ( SELECT    ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
        ) >= " + sm.txtPaidMoneyS + "";
            }
            if (!string.IsNullOrEmpty(sm.txtPaidMoneyE))
            {
                where += @" AND ( SELECT    ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
        ) <= " + sm.txtPaidMoneyE + "";
            }
            if (!string.IsNullOrEmpty(sm.txtLimitTimeS))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) >= '" + sm.txtLimitTimeS + "'";
            }
            if (!string.IsNullOrEmpty(sm.txtLimitTimeE))
            {
                where += " AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= '" + sm.txtLimitTimeE + "'";
            }
            if (!string.IsNullOrEmpty(sm.selArrearsStatus))
            {
                string[] ArrearsStatusArr = sm.selArrearsStatus.Split(",");
                string bracketS = "(";
                string bracketE = ")";
                if (ArrearsStatusArr.Length < 2)
                {
                    bracketS = "";
                    bracketE = "";
                }
                for (int i = 0; i < ArrearsStatusArr.Length; i++)
                {
                    if (i == 0)
                    {
                        where += " and " + bracketS;
                    }
                    else
                    {
                        where += " or ";
                    }
                    switch (ArrearsStatusArr[i])
                    {
                        case "":
                            where += " 1 != 1";
                            break;
                        case "1":
                            where += @" ( SELECT  COUNT(so.sOrderID)
                    FROM    T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                  ) != 0 
                 AND ( SELECT  COUNT(so.sOrderID)
                    FROM T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                            AND so.ShouldMoney != so.PaidMoney
                  ) = 0";
                            break;
                        case "2":
                            where += @" ( SELECT  COUNT(so.sOrderID)
                    FROM    T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                            AND so.ShouldMoney > so.PaidMoney
                            AND CONVERT(NVARCHAR(10), so.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                  ) > 0";
                            break;
                        case "3":
                            where += @" ( SELECT  COUNT(so.sOrderID)
                    FROM T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                            AND so.ShouldMoney != so.PaidMoney
                  ) != 0
                    AND ( SELECT  COUNT(so.sOrderID)
                    FROM    T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                            AND so.ShouldMoney > so.PaidMoney
                            AND CONVERT(NVARCHAR(10), so.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                  ) = 0";
                            break;
                        case "4":
                            where += @" ( SELECT  COUNT(so.sOrderID)
                    FROM    T_Stu_sOrder so
                    WHERE   so.sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND so.PlanItemID = o.PlanItemID
                            AND so.NumItemID = o.NumItemID
                            AND so.Status != 9
                  ) = 0";
                            break;
                    }
                }
                where += bracketE;
            }
            string cmdText = @"SELECT  r1.RefeName 在校状态 ,
        e.EnrollNum 学号 ,
        s.Name 学生姓名 ,
        s.IDCard 身份证号 ,
        d.Name 就读学校 ,
        r3.RefeName 报读类别 ,
        p.Name 报读专业 ,
        c.Name 班级 ,
        CASE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
        END 预报名日期 ,
        CASE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
        END 正式报名日期 ,
        CASE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
        END 首次缴费日期 ,
        CASE CONVERT(NVARCHAR(10), o.LimitTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), o.LimitTime, 23)
        END 应供贷日期 ,
        o.NumName 缴费学年 ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
        ) 应供贷金额 ,
        ( SELECT    ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
        ) 已供贷金额 ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
        ) 未供贷金额 ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
                    AND IsGive <> 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) 逾期欠费金额 ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = o.sEnrollsProfessionID
                    AND PlanItemID = o.PlanItemID
                    AND NumItemID = o.NumItemID
                    AND Status <> 9
                    AND IsGive = 2
                    AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) '逾期欠费(资助)' ,
        ep.Year 年份 ,
        ep.Month 月份 ,
        r2.RefeName 学历层次 ,
        CASE WHEN ( SELECT  ISNULL(SUM(ShouldMoney), 0)
                    FROM    T_Stu_sOrder
                    WHERE   sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND PlanItemID = o.PlanItemID
                            AND NumItemID = o.NumItemID
                            AND Status <> 9
                  ) = 0 THEN '未收'
             WHEN ( SELECT  ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney),
                                                              0)
                    FROM    T_Stu_sOrder
                    WHERE   sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND PlanItemID = o.PlanItemID
                            AND NumItemID = o.NumItemID
                            AND Status <> 9
                  ) = 0 THEN '已清'
             WHEN ( SELECT  ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney),
                                                              0)
                    FROM    T_Stu_sOrder
                    WHERE   sEnrollsProfessionID = o.sEnrollsProfessionID
                            AND PlanItemID = o.PlanItemID
                            AND NumItemID = o.NumItemID
                            AND Status <> 9
                            AND CONVERT(NVARCHAR(10), o.LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                  ) = 0 THEN '正常'
             ELSE '逾期'
        END 逾期状态
FROM    T_Stu_sOrder o
        LEFT JOIN T_Stu_sEnrollsProfession ep ON o.sEnrollsProfessionID = ep.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON ep.sEnrollID = e.sEnrollID
        LEFT JOIN T_Pro_Student s ON e.StudentID = s.StudentID
        LEFT JOIN T_Sys_Dept d ON ep.DeptID = d.DeptID
        LEFT JOIN T_Stu_sProfession sp ON sp.sProfessionID = ep.sProfessionID
        LEFT JOIN T_Pro_Profession p ON p.ProfessionID = sp.ProfessionID
        LEFT JOIN T_Pro_Class AS c ON ep.ClassID = c.ClassID
        LEFT JOIN T_Sys_Refe r1 ON ep.Status = r1.Value
                                  AND r1.RefeTypeID = 21
        LEFT JOIN T_Sys_Refe r2 ON ep.EnrollLevel = r2.Value
                                   AND r2.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe r3 ON o.PlanSort = r3.Value
                                   AND r3.RefeTypeID = 14
WHERE   o.sOrderID IN ( SELECT  MIN(sOrderID)
                        FROM    T_Stu_sOrder
                        WHERE   Status <> 9
                        GROUP BY sEnrollsProfessionID ,
                                PlanItemID ,
                                NumItemID )
{0}
ORDER BY o.sOrderID ASC";
            string filename = "按年缴费报表.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ep.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }



        public ActionResult GetsFeeTotalList()
        {
            string menuId = Request.Form["MenuID"];
            string txtEnrollNum = Request.Form["txtEnrollNum"];
            string txtStudentName = Request.Form["txtStudentName"];
            string txtIDCard = Request.Form["txtIDCard"];
            string selYear = Request.Form["selYear"];
            string selMonth = Request.Form["selMonth"];
            string txtFeeTimeS = Request.Form["txtFeeTimeS"];
            string txtFeeTimeE = Request.Form["txtFeeTimeE"];
            string treeDeptID = Request.Form["treeDeptID"];
            string selStatus = Request.Form["selStatus"];
            string txtBeforeEnrollTimeS = Request.Form["txtBeforeEnrollTimeS"];
            string txtBeforeEnrollTimeE = Request.Form["txtBeforeEnrollTimeE"];
            string txtEnrollTimeS = Request.Form["txtEnrollTimeS"];
            string txtEnrollTimeE = Request.Form["txtEnrollTimeE"];
            string txtFirstFeeTimeS = Request.Form["txtFirstFeeTimeS"];
            string txtFirstFeeTimeE = Request.Form["txtFirstFeeTimeE"];
            string txtProfessionName = Request.Form["txtProfessionName"];
            string selEnrollLevel = Request.Form["selEnrollLevel"];
            string selSort = Request.Form["selSort"];
            string txtMoneyS = Request.Form["txtMoneyS"];
            string txtMoneyE = Request.Form["txtMoneyE"];
            string where1 = "";
            string where2 = "";
            if (!string.IsNullOrEmpty(txtEnrollNum))
            {
                where1 += " AND e.EnrollNum LIKE '%" + txtEnrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(txtStudentName))
            {
                where1 += " AND s.Name LIKE '%" + txtStudentName + "%'";
            }
            if (!string.IsNullOrEmpty(txtIDCard))
            {
                where1 += " AND s.IDCard LIKE '%" + txtIDCard + "%'";
            }
            if (!string.IsNullOrEmpty(selYear))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(selMonth))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selMonth, "ep.Month");
            }
            if (!string.IsNullOrEmpty(txtFeeTimeS))
            {
                where2 += " AND CONVERT(NVARCHAR(10), f.FeeTime, 23) >= '" + txtFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtFeeTimeE))
            {
                where2 += " AND CONVERT(NVARCHAR(10), f.FeeTime, 23) <= '" + txtFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(treeDeptID))
            {
                where1 += " AND ep.DeptID IN ( SELECT   DeptID FROM GetChildrenDeptID(" + treeDeptID + ") )";
            }
            if (!string.IsNullOrEmpty(selStatus))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selStatus, "ep.Status");
            }
            if (!string.IsNullOrEmpty(txtBeforeEnrollTimeS))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) >= '" + txtBeforeEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtBeforeEnrollTimeE))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <= '" + txtBeforeEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(txtEnrollTimeS))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) >= '" + txtEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtEnrollTimeE))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <= '" + txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(txtFirstFeeTimeS))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) >= '" + txtFirstFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtFirstFeeTimeE))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <= '" + txtFirstFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(txtProfessionName))
            {
                where1 += " AND p.Name LIKE '%" + txtProfessionName + "%'";
            }
            if (!string.IsNullOrEmpty(selEnrollLevel))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selEnrollLevel, "ep.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(selSort))
            {
                where1 += @" AND ep.sEnrollsProfessionID IN ( SELECT DISTINCT
                                                o.sEnrollsProfessionID
                                         FROM   T_Stu_sOrder o
                                         WHERE  o.Status != 9";
                where1 += OtherHelper.MultiSelectToSQLWhere(selSort, "o.PlanSort");
                where1 += " )";
            }
            if (!string.IsNullOrEmpty(txtMoneyS))
            {
                where1 += @" AND ( SELECT    ISNULL(SUM(fo.PaidMoney), 0)
          FROM T_Stu_sFeesOrder fo
               LEFT JOIN T_Stu_sFee f2 ON fo.sFeeID = f2.sFeeID
          WHERE f2.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND fo.Status <> 2";
                if (!string.IsNullOrEmpty(txtFeeTimeS))
                {
                    where1 += " AND CONVERT(NVARCHAR(10), f2.FeeTime, 23) >= '" + txtFeeTimeS + "'";
                }
                if (!string.IsNullOrEmpty(txtFeeTimeE))
                {
                    where1 += " AND CONVERT(NVARCHAR(10), f2.FeeTime, 23) <= '" + txtFeeTimeE + "'";
                }
                where1 += " ) >= " + txtMoneyS + "";
            }
            if (!string.IsNullOrEmpty(txtMoneyE))
            {
                where1 += @" AND ( SELECT    ISNULL(SUM(fo.PaidMoney), 0)
          FROM T_Stu_sFeesOrder fo
               LEFT JOIN T_Stu_sFee f2 ON fo.sFeeID = f2.sFeeID
          WHERE f2.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND fo.Status <> 2";
                if (!string.IsNullOrEmpty(txtFeeTimeS))
                {
                    where1 += " AND CONVERT(NVARCHAR(10), f2.FeeTime, 23) >= '" + txtFeeTimeS + "'";
                }
                if (!string.IsNullOrEmpty(txtFeeTimeE))
                {
                    where1 += " AND CONVERT(NVARCHAR(10), f2.FeeTime, 23) <= '" + txtFeeTimeE + "'";
                }
                where1 += " ) <= " + txtMoneyE + "";
            }
            string filedStr = "sEnrollsProfessionID";
            string cmdText = @"SELECT  ep.sEnrollsProfessionID ,
        r3.RefeName Status ,
        s.Name StudentName ,
        e.EnrollNum ,
        s.IDCard ,
        d.Name DeptName ,
        ( SELECT    r.RefeName + ','
          FROM      T_Stu_sOrder so
                    LEFT JOIN T_Sys_Refe r ON so.PlanSort = r.Value
                                              AND r.RefeTypeID = 14
          WHERE     so.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND so.Status <> 9
          GROUP BY  so.PlanItemID ,
                    r.RefeName
        FOR
          XML PATH('')
        ) PlanSort ,
        p.Name ProfessionName ,
        c.Name ClassName ,
        CASE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
        END BeforeEnrollTime ,
        CASE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
        END EnrollTime ,
        CASE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
        END FirstFeeTime ,
        ( SELECT    ISNULL(SUM(fo.PaidMoney), 0)
          FROM      T_Stu_sFeesOrder fo
                    LEFT JOIN T_Stu_sFee f ON fo.sFeeID = f.sFeeID
          WHERE     f.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND fo.Status <> 2 {1}
        ) AS PaidMoney ,
        ( SELECT    ISNULL(SUM(fo.DiscountMoney), 0)
          FROM      T_Stu_sFeesOrder fo
                    LEFT JOIN T_Stu_sFee f ON fo.sFeeID = f.sFeeID
          WHERE     f.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND fo.Status <> 2 {1}
        ) AS DiscountMoney ,
        ep.Year ,
        r1.RefeName Month ,
        r2.RefeName EnrollLevel
FROM    T_Stu_sEnrollsProfession ep
        LEFT JOIN T_Sys_Dept d ON ep.DeptID = d.DeptID
        LEFT JOIN T_Stu_sEnroll e ON ep.sEnrollID = e.sEnrollID
        LEFT JOIN T_Pro_Student s ON e.StudentID = s.StudentID
        LEFT JOIN T_Stu_sProfession AS sp ON ep.sProfessionID = sp.sProfessionID
        LEFT JOIN T_Pro_Profession AS p ON sp.ProfessionID = p.ProfessionID
        LEFT JOIN T_Pro_Class AS c ON ep.ClassID = c.ClassID
        LEFT JOIN T_Sys_Refe AS r1 ON ep.Month = r1.Value
                                      AND r1.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS r2 ON ep.EnrollLevel = r2.Value
                                      AND r2.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe AS r3 ON ep.Status = r3.Value
                                      AND r3.RefeTypeID = 21
WHERE   ( ( SELECT  ISNULL(SUM(fo.PaidMoney), 0)
                FROM    T_Stu_sFeesOrder fo
                        LEFT JOIN T_Stu_sFee f ON fo.sFeeID = f.sFeeID
                WHERE   f.sEnrollsProfessionID = ep.sEnrollsProfessionID
                        AND fo.Status <> 2 {1}
              ) > 0
              OR ( SELECT   ISNULL(SUM(fo.DiscountMoney), 0)
                   FROM     T_Stu_sFeesOrder fo
                            LEFT JOIN T_Stu_sFee f ON fo.sFeeID = f.sFeeID
                   WHERE    f.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND fo.Status <> 2 {1}
                 ) > 0
            )
        {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ep.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where1 + powerStr, where2);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, filedStr, Request.Form, "PaidMoney,DiscountMoney"));
        }


        public AjaxResult DownloadsFeeTotalList()
        {
            string menuId = Request.Form["MenuID"];
            string txtEnrollNum = Request.Form["txtEnrollNum"];
            string txtStudentName = Request.Form["txtStudentName"];
            string txtIDCard = Request.Form["txtIDCard"];
            string selYear = Request.Form["selYear"];
            string selMonth = Request.Form["selMonth"];
            string txtFeeTimeS = Request.Form["txtFeeTimeS"];
            string txtFeeTimeE = Request.Form["txtFeeTimeE"];
            string treeDeptID = Request.Form["treeDeptID"];
            string selStatus = Request.Form["selStatus"];
            string txtBeforeEnrollTimeS = Request.Form["txtBeforeEnrollTimeS"];
            string txtBeforeEnrollTimeE = Request.Form["txtBeforeEnrollTimeE"];
            string txtEnrollTimeS = Request.Form["txtEnrollTimeS"];
            string txtEnrollTimeE = Request.Form["txtEnrollTimeE"];
            string txtFirstFeeTimeS = Request.Form["txtFirstFeeTimeS"];
            string txtFirstFeeTimeE = Request.Form["txtFirstFeeTimeE"];
            string txtProfessionName = Request.Form["txtProfessionName"];
            string selEnrollLevel = Request.Form["selEnrollLevel"];
            string selSort = Request.Form["selSort"];
            string txtMoneyS = Request.Form["txtMoneyS"];
            string txtMoneyE = Request.Form["txtMoneyE"];
            string where1 = "";
            string where2 = "";
            if (!string.IsNullOrEmpty(txtEnrollNum))
            {
                where1 += " AND e.EnrollNum LIKE '%" + txtEnrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(txtStudentName))
            {
                where1 += " AND s.Name LIKE '%" + txtStudentName + "%'";
            }
            if (!string.IsNullOrEmpty(txtIDCard))
            {
                where1 += " AND s.IDCard LIKE '%" + txtIDCard + "%'";
            }
            if (!string.IsNullOrEmpty(selYear))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selYear, "ep.Year");
            }
            if (!string.IsNullOrEmpty(selMonth))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selMonth, "ep.Month");
            }
            if (!string.IsNullOrEmpty(txtFeeTimeS))
            {
                where2 += " AND CONVERT(NVARCHAR(10), f.FeeTime, 23) >= '" + txtFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtFeeTimeE))
            {
                where2 += " AND CONVERT(NVARCHAR(10), f.FeeTime, 23) <= '" + txtFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(treeDeptID))
            {
                where1 += " AND ep.DeptID IN ( SELECT   DeptID FROM GetChildrenDeptID(" + treeDeptID + ") )";
            }
            if (!string.IsNullOrEmpty(selStatus))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selStatus, "ep.Status");
            }
            if (!string.IsNullOrEmpty(txtBeforeEnrollTimeS))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) >= '" + txtBeforeEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtBeforeEnrollTimeE))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) <= '" + txtBeforeEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(txtEnrollTimeS))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) >= '" + txtEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtEnrollTimeE))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.EnrollTime, 23) <= '" + txtEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(txtFirstFeeTimeS))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) >= '" + txtFirstFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(txtFirstFeeTimeE))
            {
                where1 += " AND CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23) <= '" + txtFirstFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(txtProfessionName))
            {
                where1 += " AND p.Name LIKE '%" + txtProfessionName + "%'";
            }
            if (!string.IsNullOrEmpty(selEnrollLevel))
            {
                where1 += OtherHelper.MultiSelectToSQLWhere(selEnrollLevel, "ep.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(selSort))
            {
                where1 += @" AND ep.sEnrollsProfessionID IN ( SELECT DISTINCT
                                                o.sEnrollsProfessionID
                                         FROM   T_Stu_sOrder o
                                         WHERE  o.Status != 9";
                where1 += OtherHelper.MultiSelectToSQLWhere(selSort, "o.PlanSort");
                where1 += " )";
            }
            if (!string.IsNullOrEmpty(txtMoneyS))
            {
                where1 += @" AND ( SELECT    ISNULL(SUM(fo.PaidMoney), 0)
          FROM T_Stu_sFeesOrder fo
               LEFT JOIN T_Stu_sFee f2 ON fo.sFeeID = f2.sFeeID
          WHERE f2.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND fo.Status <> 2";
                if (!string.IsNullOrEmpty(txtFeeTimeS))
                {
                    where1 += " AND CONVERT(NVARCHAR(10), f2.FeeTime, 23) >= '" + txtFeeTimeS + "'";
                }
                if (!string.IsNullOrEmpty(txtFeeTimeE))
                {
                    where1 += " AND CONVERT(NVARCHAR(10), f2.FeeTime, 23) <= '" + txtFeeTimeE + "'";
                }
                where1 += " ) >= " + txtMoneyS + "";
            }
            if (!string.IsNullOrEmpty(txtMoneyE))
            {
                where1 += @" AND ( SELECT    ISNULL(SUM(fo.PaidMoney), 0)
          FROM T_Stu_sFeesOrder fo
               LEFT JOIN T_Stu_sFee f2 ON fo.sFeeID = f2.sFeeID
          WHERE f2.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND fo.Status <> 2";
                if (!string.IsNullOrEmpty(txtFeeTimeS))
                {
                    where1 += " AND CONVERT(NVARCHAR(10), f2.FeeTime, 23) >= '" + txtFeeTimeS + "'";
                }
                if (!string.IsNullOrEmpty(txtFeeTimeE))
                {
                    where1 += " AND CONVERT(NVARCHAR(10), f2.FeeTime, 23) <= '" + txtFeeTimeE + "'";
                }
                where1 += " ) <= " + txtMoneyE + "";
            }
            string cmdText = @"SELECT  r3.RefeName 在校状态 ,
        s.Name 学生姓名 ,
        e.EnrollNum 学号 ,
        s.IDCard 身份证号 ,
        d.Name 报读学校 ,
        ( SELECT    r.RefeName + ','
          FROM      T_Stu_sOrder so
                    LEFT JOIN T_Sys_Refe r ON so.PlanSort = r.Value
                                              AND r.RefeTypeID = 14
          WHERE     so.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND so.Status <> 9
          GROUP BY  so.PlanItemID ,
                    r.RefeName
        FOR
          XML PATH('')
        ) 报读类别 ,
        p.Name 报读专业 ,
        c.Name 班级 ,
        CASE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23)
        END 预报日期 ,
        CASE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.EnrollTime, 23)
        END 正报日期 ,
        CASE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), ep.FirstFeeTime, 23)
        END 首次缴费日期 ,
        ( SELECT    ISNULL(SUM(fo.PaidMoney), 0)
          FROM      T_Stu_sFeesOrder fo
                    LEFT JOIN T_Stu_sFee f ON fo.sFeeID = f.sFeeID
          WHERE     f.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND fo.Status <> 2 {1}
        ) AS 供款金额 ,
        ( SELECT    ISNULL(SUM(fo.DiscountMoney), 0)
          FROM      T_Stu_sFeesOrder fo
                    LEFT JOIN T_Stu_sFee f ON fo.sFeeID = f.sFeeID
          WHERE     f.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    AND fo.Status <> 2 {1}
        ) AS 优惠金额 ,
        ep.Year 年份 ,
        r1.RefeName 月份 ,
        r2.RefeName 学历层次
FROM    T_Stu_sEnrollsProfession ep
        LEFT JOIN T_Sys_Dept d ON ep.DeptID = d.DeptID
        LEFT JOIN T_Stu_sEnroll e ON ep.sEnrollID = e.sEnrollID
        LEFT JOIN T_Pro_Student s ON e.StudentID = s.StudentID
        LEFT JOIN T_Stu_sProfession AS sp ON ep.sProfessionID = sp.sProfessionID
        LEFT JOIN T_Pro_Profession AS p ON sp.ProfessionID = p.ProfessionID
        LEFT JOIN T_Pro_Class AS c ON ep.ClassID = c.ClassID
        LEFT JOIN T_Sys_Refe AS r1 ON ep.Month = r1.Value
                                      AND r1.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS r2 ON ep.EnrollLevel = r2.Value
                                      AND r2.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe AS r3 ON ep.Status = r3.Value
                                      AND r3.RefeTypeID = 21
WHERE   ( ( SELECT  ISNULL(SUM(fo.PaidMoney), 0)
                FROM    T_Stu_sFeesOrder fo
                        LEFT JOIN T_Stu_sFee f ON fo.sFeeID = f.sFeeID
                WHERE   f.sEnrollsProfessionID = ep.sEnrollsProfessionID
                        AND fo.Status <> 2 {1}
              ) > 0
              OR ( SELECT   ISNULL(SUM(fo.DiscountMoney), 0)
                   FROM     T_Stu_sFeesOrder fo
                            LEFT JOIN T_Stu_sFee f ON fo.sFeeID = f.sFeeID
                   WHERE    f.sEnrollsProfessionID = ep.sEnrollsProfessionID
                            AND fo.Status <> 2 {1}
                 ) > 0
            )
        {0}";
            string filename = "供款合计统计.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ep.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where1 + powerStr, where2);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }
    }

}
