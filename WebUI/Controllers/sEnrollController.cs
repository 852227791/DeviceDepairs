using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common;
using DAL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebUI.Controllers
{
    public class sEnrollController : BaseController
    {
        #region 页面
        public ActionResult ClassGrouping()
        {
            return View();
        }
        public ActionResult UploadStudyNum()
        {
            return View();
        }
        public ActionResult sEnrollRefund()
        {
            return View();
        }

        public ActionResult ChangeTurnOfficial()
        {
            return View();
        }
        public ActionResult sEnrollChangeStatusRecord()
        {
            return View();
        }
        public ActionResult ModClass()
        {
            return View();
        }
        public ActionResult sEnrollUpload()
        {
            return View();
        }
        public ActionResult sEnrollsProfessionStatusUpload()
        {
            return View();
        }
        public ActionResult sEnrollModStatus()
        {
            return View();
        }
        public ActionResult sEnrollAudit()
        {
            return View();
        }

        public ActionResult ChangeLeaveYear()
        {
            return View();
        }
        /// <summary>
        /// 报名列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult sEnrollList()
        {
            return View();
        }
        public ActionResult sEnrollTurnOfficial()
        {
            return View();
        }
        public ActionResult sEnrollAdd()
        {
            return View();
        }
        /// <summary>
        /// 添加、修改页
        /// </summary>
        /// <returns></returns>
        public ActionResult sEnrollEdit()
        {
            return View();
        }

        /// <summary>
        /// 选择缴费方案页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ChooseSchemeList()
        {
            return View();
        }

        /// <summary>
        /// 查看页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sEnrollDetail()
        {
            return View();
        }

        /// <summary>
        /// 生成缴费单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sEnrollBuild()
        {
            return View();
        }
        public ActionResult UploadLeaveYear()
        {
            return View();
        }
        /// <summary>
        /// 批量生成缴费单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sEnrollBuildMore()
        {
            return View();
        }

        /// <summary>
        /// 停用页面
        /// </summary>
        /// <returns></returns>
        public ActionResult StopsEnroll()
        {
            return View();
        }

        /// <summary>
        /// 转专业页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TurnMajor()
        {
            return View();
        }

        public ActionResult UploadEnrollDept()
        {
            return View();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 得到报名列表信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetsEnrollList()
        {
            string menuId = Request.Form["MenuID"];
            string stuname = Request.Form["txtStuName"];
            string examNum = Request.Form["txtExamNum"];
            string enrollnum = Request.Form["txtEnrollNum"];
            string idcard = Request.Form["txtIDCard"];
            string signTimeS = Request.Form["txtSignTimeS"];
            string signTimeE = Request.Form["txtSignTimeE"];
            string dept = Request.Form["treeDept"];
            string level = Request.Form["selLevel"];
            string major = Request.Form["txtMajor"];
            string txtClass = Request.Form["txtClass"];
            string year = Request.Form["selYear"];
            string month = Request.Form["selMonth"];
            string status = Request.Form["selStatus"];
            string no = Request.Form["selNo"];
            string leaveYear = Request.Form["txtLeaveYear"];
            string where = "";

            string firstFeeTimeS = Request.Form["txtFirstFeeTimeS"];
            string firstFeeTimeE = Request.Form["txtFirstFeeTimeE"];
            string beforeEnrollTimeS = Request.Form["txtBeforeEnrollTimeS"];
            string beforeEnrollTimeE = Request.Form["txtBeforeEnrollTimeE"];

            string createTimeS = Request.Form["txtCreateTimeS"];
            string createTimeE = Request.Form["txtCreateTimeE"];


            string planSort = Request.Form["selPlanSort"];

            if (!string.IsNullOrEmpty(planSort))
            {

                if (planSort.Substring(0, 1).Equals(","))
                {
                    planSort = planSort.Substring(1, planSort.Length - 1);
                }
                where += @" AND senrollpro.sEnrollsProfessionID IN (
        SELECT  so1.sEnrollsProfessionID
        FROM    T_Stu_sOrder so1
        WHERE   so1.Status <> 9
                AND so1.PlanSort IN(" + planSort + ")  )";
            }


            if (!string.IsNullOrEmpty(createTimeS))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.CreateTime,23) >= '" + createTimeS + "'";
            }

            if (!string.IsNullOrEmpty(createTimeE))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.CreateTime,23) <= '" + createTimeE + "'";
            }

            if (!string.IsNullOrEmpty(firstFeeTimeS))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.FirstFeeTime,23) >= '" + firstFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(firstFeeTimeE))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.FirstFeeTime,23) <= '" + firstFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(beforeEnrollTimeS))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.BeforeEnrollTime,23) >= '" + beforeEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(beforeEnrollTimeE))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.BeforeEnrollTime,23) <= '" + beforeEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(stuname))
            {
                where += " AND student.Name like '%" + stuname + "%'";
            }
            if (!string.IsNullOrEmpty(examNum))
            {
                where += " AND senroll.ExamNum like '%" + examNum + "%'";
            }
            if (!string.IsNullOrEmpty(enrollnum))
            {
                where += " AND senroll.EnrollNum like '%" + enrollnum + "%'";
            }
            if (!string.IsNullOrEmpty(idcard))
            {
                where += " AND student.IDCard like '%" + idcard + "%'";
            }
            if (!string.IsNullOrEmpty(signTimeS))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.EnrollTime,23) >= '" + signTimeS + "'";
            }
            if (!string.IsNullOrEmpty(signTimeE))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.EnrollTime,23) <= '" + signTimeE + "'";
            }
            if (!string.IsNullOrEmpty(dept))
            {
                where += " AND senrollpro.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(leaveYear))
            {
                where += OtherHelper.MultiSelectToSQLWhere(leaveYear, "senrollpro.LeaveYear");
            }
            if (!string.IsNullOrEmpty(level))
            {
                where += OtherHelper.MultiSelectToSQLWhere(level, "senrollpro.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(major))
            {
                where += " AND pro.Name like '%" + major + "%'";
            }
            if (!string.IsNullOrEmpty(txtClass))
            {
                where += " AND c.Name like '%" + txtClass + "%'";
            }
            if (!string.IsNullOrEmpty(year))
            {
                where += OtherHelper.MultiSelectToSQLWhere(year, "senrollpro.Year");
            }
            if (!string.IsNullOrEmpty(month))
            {
                where += OtherHelper.MultiSelectToSQLWhere(month, "refe_month.Value");
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "senrollpro.Status");
            }
            if (!string.IsNullOrEmpty(no))
            {
                if (no == "1")
                {
                    where += @" AND ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
                    AND IsGive <> 2
                    AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) > 0";
                }
                else if (no == "2")
                {
                    where += @" AND ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
                    AND IsGive = 2
                    AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) > 0";
                }
            }
            string filed = "sEnrollsProfessionID";
            string cmdText = @"SELECT  senrollpro.sEnrollsProfessionID ,
        senrollpro.sEnrollID ,
        dept.Name AS DeptName ,
        da.Name DeptAreaName ,
        senroll.StudentID ,
        student.Name AS StudentName ,
        senroll.EnrollNum ,
        senroll.ExamNum ,
        student.IDCard ,
        senrollpro.Year ,
        refe_month.RefeName AS Month ,
        refe_level.RefeName AS Level ,
        pro.Name AS Major ,
        CONVERT(NVARCHAR(10), senrollpro.EnrollTime, 23) AS SignTime ,
        senrollpro.Status ,
        refe_luqu.RefeName AS StatusName ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
        ) AS ShouldMoney ,
        ( SELECT    ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
        ) AS PaidMoney ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
        ) - ( SELECT    ISNULL(SUM(PaidMoney), 0)
              FROM      T_Stu_sOrder
              WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                        AND Status != 9
            ) NoMoney ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
                    AND IsGive <> 2
                    AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) AS NoMoneyNoGive ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
                    AND IsGive = 2
                    AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) AS NoGive ,
        ( SELECT TOP 1
                    eps.Explain ,
                    r1.RefeName ,
                    CONVERT(NVARCHAR(10), eps.CreateTime, 23) CreateTime
          FROM      T_Stu_sEnrollsProfessionStatus eps
                    LEFT JOIN T_Sys_Refe r1 ON r1.Value = eps.StatusValue
                                               AND r1.RefeTypeID = 21
          WHERE     eps.sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
          ORDER BY  eps.CreateTime DESC
        FOR
          XML PATH('status')
        ) ChangeStatusReson ,
        c.Name ClassName ,
        spro.ProfessionID ,
        senrollpro.ClassID ,
        senrollpro.LeaveYear ,
        CONVERT(NVARCHAR(10), senrollpro.FirstFeeTime, 23) FirstFeeTime ,
        CONVERT(NVARCHAR(10), senrollpro.BeforeEnrollTime, 23) BeforeEnrollTime ,
        CONVERT(NVARCHAR(10), senrollpro.CreateTime, 23) CreateTime ,
        ( SELECT    r.RefeName + ','
          FROM      T_Stu_sOrder so
                    LEFT JOIN T_Sys_Refe r ON so.PlanSort = r.Value
                                              AND r.RefeTypeID = 14
          WHERE     so.sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND so.Status <> 9
          GROUP BY  so.PlanItemID ,
                    r.RefeName
        FOR
          XML PATH('')
        ) PlanSort
FROM    T_Stu_sEnrollsProfession AS senrollpro
        LEFT JOIN T_Stu_sEnroll AS senroll ON senroll.sEnrollID = senrollpro.sEnrollID
        LEFT JOIN T_Sys_Dept AS dept ON senrollpro.DeptID = dept.DeptID
        LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
        LEFT JOIN T_Stu_sProfession AS spro ON senrollpro.sProfessionID = spro.sProfessionID
        LEFT JOIN T_Pro_Profession AS pro ON spro.ProfessionID = pro.ProfessionID
        LEFT JOIN T_Pro_Class c ON c.ClassID = senrollpro.ClassID
        LEFT JOIN T_Sys_Refe AS refe_month ON senrollpro.Month = refe_month.Value
                                              AND refe_month.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS refe_luqu ON senrollpro.Status = refe_luqu.Value
                                             AND refe_luqu.RefeTypeID = 21
        LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                              AND refe_level.RefeTypeID = 17
        LEFT JOIN T_Pro_DeptArea da ON senrollpro.DeptAreaID = da.DeptAreaID
WHERE   1 = 1
        {0}";

            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "senrollpro.DeptID", "senrollpro.Auditor");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, filed, Request.Form));
        }


        public AjaxResult GetsEnrollListFoot()
        {
            string menuId = Request.Form["MenuID"];
            string stuname = Request.Form["txtStuName"];
            string examNum = Request.Form["txtExamNum"];
            string enrollnum = Request.Form["txtEnrollNum"];
            string idcard = Request.Form["txtIDCard"];
            string signTimeS = Request.Form["txtSignTimeS"];
            string signTimeE = Request.Form["txtSignTimeE"];
            string dept = Request.Form["treeDept"];
            string level = Request.Form["selLevel"];
            string major = Request.Form["txtMajor"];
            string txtClass = Request.Form["txtClass"];
            string year = Request.Form["selYear"];
            string month = Request.Form["selMonth"];
            string status = Request.Form["selStatus"];
            string no = Request.Form["selNo"];
            string leaveYear = Request.Form["txtLeaveYear"];
            string where = "";
            if (!string.IsNullOrEmpty(stuname))
            {
                where += " AND student.Name like '%" + stuname + "%'";
            }
            if (!string.IsNullOrEmpty(examNum))
            {
                where += " AND senroll.ExamNum like '%" + examNum + "%'";
            }
            if (!string.IsNullOrEmpty(enrollnum))
            {
                where += " AND senroll.EnrollNum like '%" + enrollnum + "%'";
            }
            if (!string.IsNullOrEmpty(idcard))
            {
                where += " AND student.IDCard like '%" + idcard + "%'";
            }
            if (!string.IsNullOrEmpty(signTimeS))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.EnrollTime,23) >= '" + signTimeS + "'";
            }
            if (!string.IsNullOrEmpty(signTimeE))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.EnrollTime,23) <= '" + signTimeE + "'";
            }
            if (!string.IsNullOrEmpty(dept))
            {
                where += " AND senrollpro.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(leaveYear))
            {
                where += OtherHelper.MultiSelectToSQLWhere(leaveYear, "senrollpro.LeaveYear");
            }
            if (!string.IsNullOrEmpty(level))
            {
                where += OtherHelper.MultiSelectToSQLWhere(level, "senrollpro.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(major))
            {
                where += " AND pro.Name like '%" + major + "%'";
            }
            if (!string.IsNullOrEmpty(txtClass))
            {
                where += " AND c.Name like '%" + txtClass + "%'";
            }
            if (!string.IsNullOrEmpty(year))
            {
                where += OtherHelper.MultiSelectToSQLWhere(year, "senrollpro.Year");
            }
            if (!string.IsNullOrEmpty(month))
            {
                where += OtherHelper.MultiSelectToSQLWhere(month, "refe_month.Value");
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "senrollpro.Status");
            }
            if (!string.IsNullOrEmpty(no))
            {
                if (no == "1")
                {
                    where += @" AND ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
                    AND IsGive <> 2
                    AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) > 0";
                }
                else if (no == "2")
                {
                    where += @" AND ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
                    AND IsGive = 2
                    AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) > 0";
                }
            }
            string cmdText = @"SELECT  ( SELECT    '合计:' + CONVERT(NVARCHAR(18), ISNULL(SUM(ShouldMoney), 0))
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID IN (
                    SELECT  senrollpro.sEnrollsProfessionID
                    FROM    T_Stu_sEnroll AS senroll
                            LEFT JOIN T_Stu_sEnrollsProfession AS senrollpro ON senroll.sEnrollID = senrollpro.sEnrollID
                            LEFT JOIN T_Sys_Dept AS dept ON senrollpro.DeptID = dept.DeptID
                            LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
                            LEFT JOIN T_Stu_sProfession AS spro ON senrollpro.sProfessionID = spro.sProfessionID
                            LEFT JOIN T_Pro_Profession AS pro ON spro.ProfessionID = pro.ProfessionID
                            LEFT JOIN T_Pro_Class c ON c.ClassID = senrollpro.ClassID
                            LEFT JOIN T_Sys_Refe AS refe_month ON senrollpro.Month = refe_month.Value
                                                              AND refe_month.RefeTypeID = 18
                            LEFT JOIN T_Sys_Refe AS refe_luqu ON senrollpro.Status = refe_luqu.Value
                                                              AND refe_luqu.RefeTypeID = 21
                            LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                                              AND refe_level.RefeTypeID = 17
                            LEFT JOIN T_Pro_DeptArea da ON senrollpro.DeptAreaID = da.DeptAreaID
                    WHERE   1 = 1
                            AND senrollpro.sEnrollsProfessionID IS NOT NULL {0})
                    AND Status != 9
        ) AS ShouldMoney ,
        ( SELECT    '合计:' + CONVERT(NVARCHAR(18), ISNULL(SUM(PaidMoney), 0))
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID IN (
                    SELECT  senrollpro.sEnrollsProfessionID
                    FROM    T_Stu_sEnroll AS senroll
                            LEFT JOIN T_Stu_sEnrollsProfession AS senrollpro ON senroll.sEnrollID = senrollpro.sEnrollID
                            LEFT JOIN T_Sys_Dept AS dept ON senrollpro.DeptID = dept.DeptID
                            LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
                            LEFT JOIN T_Stu_sProfession AS spro ON senrollpro.sProfessionID = spro.sProfessionID
                            LEFT JOIN T_Pro_Profession AS pro ON spro.ProfessionID = pro.ProfessionID
                            LEFT JOIN T_Pro_Class c ON c.ClassID = senrollpro.ClassID
                            LEFT JOIN T_Sys_Refe AS refe_month ON senrollpro.Month = refe_month.Value
                                                              AND refe_month.RefeTypeID = 18
                            LEFT JOIN T_Sys_Refe AS refe_luqu ON senrollpro.Status = refe_luqu.Value
                                                              AND refe_luqu.RefeTypeID = 21
                            LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                                              AND refe_level.RefeTypeID = 17
                            LEFT JOIN T_Pro_DeptArea da ON senrollpro.DeptAreaID = da.DeptAreaID
                    WHERE   1 = 1
                            AND senrollpro.sEnrollsProfessionID IS NOT NULL {0})
                    AND Status != 9
        ) AS PaidMoney ,
        ( '合计:'
          + CONVERT(NVARCHAR(18), ( SELECT  ISNULL(SUM(ShouldMoney), 0)
                                    FROM    T_Stu_sOrder
                                    WHERE   sEnrollsProfessionID IN (
                                            SELECT  senrollpro.sEnrollsProfessionID
                                            FROM    T_Stu_sEnroll AS senroll
                                                    LEFT JOIN T_Stu_sEnrollsProfession
                                                    AS senrollpro ON senroll.sEnrollID = senrollpro.sEnrollID
                                                    LEFT JOIN T_Sys_Dept AS dept ON senrollpro.DeptID = dept.DeptID
                                                    LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
                                                    LEFT JOIN T_Stu_sProfession
                                                    AS spro ON senrollpro.sProfessionID = spro.sProfessionID
                                                    LEFT JOIN T_Pro_Profession
                                                    AS pro ON spro.ProfessionID = pro.ProfessionID
                                                    LEFT JOIN T_Pro_Class c ON c.ClassID = senrollpro.ClassID
                                                    LEFT JOIN T_Sys_Refe AS refe_month ON senrollpro.Month = refe_month.Value
                                                              AND refe_month.RefeTypeID = 18
                                                    LEFT JOIN T_Sys_Refe AS refe_luqu ON senrollpro.Status = refe_luqu.Value
                                                              AND refe_luqu.RefeTypeID = 21
                                                    LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                                              AND refe_level.RefeTypeID = 17
                                                    LEFT JOIN T_Pro_DeptArea da ON senrollpro.DeptAreaID = da.DeptAreaID
                                            WHERE   1 = 1
                                                    AND senrollpro.sEnrollsProfessionID IS NOT NULL {0})
                                            AND Status != 9
                                  )
          - ( SELECT    ISNULL(SUM(PaidMoney), 0)
              FROM      T_Stu_sOrder
              WHERE     sEnrollsProfessionID IN (
                        SELECT  senrollpro.sEnrollsProfessionID
                        FROM    T_Stu_sEnroll AS senroll
                                LEFT JOIN T_Stu_sEnrollsProfession AS senrollpro ON senroll.sEnrollID = senrollpro.sEnrollID
                                LEFT JOIN T_Sys_Dept AS dept ON senrollpro.DeptID = dept.DeptID
                                LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
                                LEFT JOIN T_Stu_sProfession AS spro ON senrollpro.sProfessionID = spro.sProfessionID
                                LEFT JOIN T_Pro_Profession AS pro ON spro.ProfessionID = pro.ProfessionID
                                LEFT JOIN T_Pro_Class c ON c.ClassID = senrollpro.ClassID
                                LEFT JOIN T_Sys_Refe AS refe_month ON senrollpro.Month = refe_month.Value
                                                              AND refe_month.RefeTypeID = 18
                                LEFT JOIN T_Sys_Refe AS refe_luqu ON senrollpro.Status = refe_luqu.Value
                                                              AND refe_luqu.RefeTypeID = 21
                                LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                                              AND refe_level.RefeTypeID = 17
                                LEFT JOIN T_Pro_DeptArea da ON senrollpro.DeptAreaID = da.DeptAreaID
                        WHERE   1 = 1
                                AND senrollpro.sEnrollsProfessionID IS NOT NULL {0})
                        AND Status != 9
            )) ) NoMoney ,
        ( '合计:'
          + CONVERT(NVARCHAR(18), ( SELECT  ISNULL(SUM(ShouldMoney), 0)
                                            - ISNULL(SUM(PaidMoney), 0)
                                    FROM    T_Stu_sOrder
                                    WHERE   sEnrollsProfessionID IN (
                                            SELECT  senrollpro.sEnrollsProfessionID
                                            FROM    T_Stu_sEnroll AS senroll
                                                    LEFT JOIN T_Stu_sEnrollsProfession
                                                    AS senrollpro ON senroll.sEnrollID = senrollpro.sEnrollID
                                                    LEFT JOIN T_Sys_Dept AS dept ON senrollpro.DeptID = dept.DeptID
                                                    LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
                                                    LEFT JOIN T_Stu_sProfession
                                                    AS spro ON senrollpro.sProfessionID = spro.sProfessionID
                                                    LEFT JOIN T_Pro_Profession
                                                    AS pro ON spro.ProfessionID = pro.ProfessionID
                                                    LEFT JOIN T_Pro_Class c ON c.ClassID = senrollpro.ClassID
                                                    LEFT JOIN T_Sys_Refe AS refe_month ON senrollpro.Month = refe_month.Value
                                                              AND refe_month.RefeTypeID = 18
                                                    LEFT JOIN T_Sys_Refe AS refe_luqu ON senrollpro.Status = refe_luqu.Value
                                                              AND refe_luqu.RefeTypeID = 21
                                                    LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                                              AND refe_level.RefeTypeID = 17
                                                    LEFT JOIN T_Pro_DeptArea da ON senrollpro.DeptAreaID = da.DeptAreaID
                                            WHERE   1 = 1
                                                    AND senrollpro.sEnrollsProfessionID IS NOT NULL {0})
                                            AND Status != 9
                                            AND IsGive <> 2
                                            AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                                  )) ) AS NoMoneyNoGive ,
        ( '合计:'
          + CONVERT(NVARCHAR(18), ( SELECT  ISNULL(SUM(ShouldMoney), 0)
                                            - ISNULL(SUM(PaidMoney), 0)
                                    FROM    T_Stu_sOrder
                                    WHERE   sEnrollsProfessionID IN (
                                            SELECT  senrollpro.sEnrollsProfessionID
                                            FROM    T_Stu_sEnroll AS senroll
                                                    LEFT JOIN T_Stu_sEnrollsProfession
                                                    AS senrollpro ON senroll.sEnrollID = senrollpro.sEnrollID
                                                    LEFT JOIN T_Sys_Dept AS dept ON senrollpro.DeptID = dept.DeptID
                                                    LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
                                                    LEFT JOIN T_Stu_sProfession
                                                    AS spro ON senrollpro.sProfessionID = spro.sProfessionID
                                                    LEFT JOIN T_Pro_Profession
                                                    AS pro ON spro.ProfessionID = pro.ProfessionID
                                                    LEFT JOIN T_Pro_Class c ON c.ClassID = senrollpro.ClassID
                                                    LEFT JOIN T_Sys_Refe AS refe_month ON senrollpro.Month = refe_month.Value
                                                              AND refe_month.RefeTypeID = 18
                                                    LEFT JOIN T_Sys_Refe AS refe_luqu ON senrollpro.Status = refe_luqu.Value
                                                              AND refe_luqu.RefeTypeID = 21
                                                    LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                                              AND refe_level.RefeTypeID = 17
                                                    LEFT JOIN T_Pro_DeptArea da ON senrollpro.DeptAreaID = da.DeptAreaID
                                            WHERE   1 = 1
                                                    AND senrollpro.sEnrollsProfessionID IS NOT NULL {0})
                                            AND Status != 9
                                            AND IsGive = 2
                                            AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
                                  )) ) AS NoGive";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "senrollpro.DeptID", "senrollpro.Auditor");
            cmdText = string.Format(cmdText, where + powerStr);
            return AjaxResult.Success(JsonHelper.DataTableToJson(DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0]), "success");
        }
        /// <summary>
        /// 根据主体、年、月获取专业（排除停用的）
        /// </summary>
        /// <returns></returns>
        public string SelMajor()
        {
            string dept = Request.Form["DeptID"];
            string year = Request.Form["Year"];
            string month = Request.Form["Month"];
            DataTable dt = sEnrollBLL.GetMajor(dept, year, month);
            return JsonHelper.DataTableToJson(dt);
        }

        /// <summary>
        /// 根据报名专业ID获取缴费方案json数据
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetEditShceme()
        {
            string str = string.Empty;
            string editsEnrollsProfessionID = Request.Form["ID"];
            //{"total":1,"rows":[{"ItemID":"2","Year":"2016","Month":"3月（春季）","Name":"缴费方案1"}]}
            DataTable dt = sEnrollBLL.GetEditShceme(editsEnrollsProfessionID);
            if (dt.Rows.Count > 0)
            {
                string content = string.Empty;
                str = "{\"total\":" + dt.Rows.Count + ",\"rows\":[@]}";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    content += "{\"ItemID\":\"" + dt.Rows[i]["ItemID"] + "\",\"Year\":\"" + dt.Rows[i]["Year"] + "\",\"Month\":\"" + dt.Rows[i]["Month"] + "\",\"Name\":\"" + dt.Rows[i]["Name"] + "\"},";
                }
                str = str.Replace("@", content.TrimEnd(','));
            }
            return AjaxResult.Success(str);
        }

        /// <summary>
        /// 验证报名专业是否已经缴费
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckIsGiveFee()
        {
            string sEnrollsProfessionID = Request.Form["ID"];
            bool result = sEnrollBLL.CheckIsGiveFee(sEnrollsProfessionID);
            return AjaxResult.Success(result);
        }

        /// <summary>
        /// 根据报名专业ID判断是否生成缴费单
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckIsShouldMoney()
        {
            string sEnrollsProfessionID = Request.Form["ID"];
            bool result = sEnrollBLL.CheckIsShouldMoney(sEnrollsProfessionID);
            return AjaxResult.Success(result);
        }

        /// <summary>
        /// 验证该学生是否已经生成报名号，在T_Stu_sEnroll表中是否存在
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckStudentIsEnroll()
        {
            string studentID = Request.Form["ID"];
            DataTable dt = sEnrollBLL.GetStudentEnroll(studentID);
            if (dt.Rows.Count > 0)
            {
                return AjaxResult.Success(true);
            }
            else
            {
                return AjaxResult.Success(false);
            }
        }

        /// <summary>
        /// 得到缴费方案（排除停用的和不在有效期之内的数据）
        /// </summary>
        public ActionResult GetChooseSchemeList()
        {
            string menuId = Request.Form["MenuID"];
            string major = Request.Form["Major"];
            string year = Request.Form["Year"];
            string month = Request.Form["Month"];
            string schemeName = Request.Form["txtSchemeName"];
            string level = Request.Form["Level"];
            string where = "";
            if (!string.IsNullOrEmpty(schemeName))
            {
                where += " AND item.Name like '%" + schemeName + "%'";
            }
            if (!string.IsNullOrEmpty(level))
            {
                if (level.Equals("1"))
                {
                    where += " and item.PlanLevel=1";
                }
                else if (level.Equals("2"))
                {
                    where += " and item.PlanLevel=2";
                }
                else if (level.Equals("3"))
                {
                    where += " and item.PlanLevel IN(3)";
                }
                else if (level.Equals("4"))
                {
                    where += " and item.PlanLevel IN(1,2)";
                }
                else if (level.Equals("5"))
                {
                    where += " and item.PlanLevel IN(2,3)";
                }
                else if (level.Equals("6"))
                {
                    where += " and item.PlanLevel IN(1,2,3)";
                }
                else if (level.Equals("7"))
                {
                    where += " and item.PlanLevel IN(5,6,7)";
                }

            }


            string cmdText = @"SELECT  item.ItemID ,
        item.Year ,
        refe_month.RefeName AS Month ,
        item.Name ,
        item.Queue
FROM    T_Stu_sItemsProfession AS sitempro
        LEFT JOIN T_Pro_Item AS item ON sitempro.ItemID = item.ItemID
        LEFT JOIN T_Sys_Refe AS refe_month ON item.Month = refe_month.Value
                                              AND refe_month.RefeTypeID = 16
WHERE   sitempro.sProfessionID = {0}
        AND item.Year = {1}
        AND item.Month = {2}
        AND item.Status = 1
        AND item.IsPlan = 1
        AND ( GETDATE() >= CONVERT(NVARCHAR(10), item.StartTime, 23)
              OR CONVERT(NVARCHAR(10), item.StartTime, 23) = '1900-01-01'
            )
        AND ( GETDATE() <= CONVERT(NVARCHAR(10), item.EndTime, 23)
              OR CONVERT(NVARCHAR(10), item.EndTime, 23) = '1900-01-01'
            ) {3}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "item.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, major, year, month, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, ""));
        }

        /// <summary>
        /// 添加、修改报名信息
        /// </summary>
        /// <returns></returns>
        public string GetsEnrollEdit(sEnrollsProfessionModel ifm)
        {
            string result = "出现未知错误，请联系管理员";

            #region 后台验证
            string sEnrollsProfessionID = "0";
            string studentID = Request.Form["editStudentID"];//学生
            string editScheme = Request.Form["editScheme"];//缴费方案，json格式字符串

            string editRepeatSign = Request.Form["editIsRepeatSign"];

            if (!string.IsNullOrEmpty(ifm.sEnrollsProfessionID))
            {
                sEnrollsProfessionID = ifm.sEnrollsProfessionID;
            }
            if (string.IsNullOrEmpty(ifm.DeptID))
            {
                return "请选择校区";
            }
            if (string.IsNullOrEmpty(studentID))
            {
                return "请选择学生";
            }
            if (string.IsNullOrEmpty(ifm.Year))
            {
                return "请选择年份";
            }
            if (string.IsNullOrEmpty(ifm.Month))
            {
                return "请选择月份";
            }
            if (string.IsNullOrEmpty(ifm.EnrollLevel))
            {
                return "请选择学习层次";
            }
            if (string.IsNullOrEmpty(ifm.sProfessionID))
            {
                return "请选择专业";
            }
            if (string.IsNullOrEmpty(ifm.EnrollTime))
            {
                return "报名时间不能为空";
            }
            if (string.IsNullOrEmpty(editScheme))
            {
                return "请选择缴费方案";
            }
            if (!string.IsNullOrEmpty(ifm.Remark))
            {
                ifm.Remark = ifm.Remark.Replace("\r\n", "<br />");
            }
            #endregion

            try
            {
                if (sEnrollsProfessionID.Trim() != "0")
                {
                    //判断是否重复报名
                    bool repeat = sEnrollBLL.CheckIsRepeat(ifm.DeptID, studentID, ifm.Year, ifm.Month, ifm.EnrollLevel, ifm.sProfessionID, sEnrollsProfessionID);
                    if (repeat)
                    {
                        result = "不能重复报名";
                    }
                    else
                    {
                        #region 修改
                        string editFlag = string.Empty;
                        sEnrollsProfessionModel model = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(" AND sEnrollsProfessionID = @sEnrollsProfessionID", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID) });
                        LogBLL.CreateLog("T_Stu_sEnrollsProfession", this.UserId.ToString(), model, ifm);//写日志

                        //修改T_Stu_sEnrollsProfession
                        model.DeptID = ifm.DeptID;
                        model.DeptAreaID = ifm.DeptAreaID;
                        model.Year = ifm.Year;
                        model.Month = ifm.Month;
                        model.EnrollTime = ifm.EnrollTime;
                        model.EnrollLevel = ifm.EnrollLevel;
                        model.sProfessionID = ifm.sProfessionID;
                        model.ClassID = ifm.ClassID;
                        model.Remark = ifm.Remark;
                        model.UpdateID = this.UserId.ToString();
                        model.UpdateTime = DateTime.Now.ToString();
                        sEnrollsProfessionBLL.UpdatesEnrollsProfession(model);
                        bool flag = sEnrollBLL.CheckIsShouldMoney(sEnrollsProfessionID);
                        if (!flag)
                        {
                            //修改T_Stu_sItemsEnroll，先删除，再插入
                            sEnrollBLL.DeleteItemsEnroll(sEnrollsProfessionID);

                            //添加T_Stu_sItemsEnroll，指定专业的缴费方案
                            //{"total":1,"rows":[{"ItemID":"2","Year":"2016","Month":"3月（春季）","Name":"缴费方案1"}]}
                            JObject job = JObject.Parse(editScheme);
                            string[] array = job.Properties().Where(x => x.Name == "rows").Select(item => item.Value.ToString()).ToArray();
                            List<sEnrollSchemeModel> schemeList = JsonConvert.DeserializeObject<List<sEnrollSchemeModel>>(array[0]);//将json数据转化为对象类型并赋值给list
                            foreach (var item in schemeList)
                            {
                                sItemsEnrollModel siemmodel = new sItemsEnrollModel();
                                siemmodel.ItemID = item.ItemID;
                                siemmodel.sEnrollsProfessionID = sEnrollsProfessionID;
                                sItemsEnrollBLL.InsertsItemsEnroll(siemmodel);
                            }
                        }
                        result = "yes";
                        #endregion
                    }
                }
                #region 弃用添加
                //else
                //{
                //    DataTable dt = sEnrollBLL.GetStudentEnroll(studentID);
                //    if (dt.Rows.Count > 0 && editRepeatSign == "default")
                //    {
                //        //没有选择报名方式
                //        result = "该生已报过名，请选择“续报名”或“重复报名”";
                //    }
                //    else
                //    {
                //        //判断是否重复报名
                //        bool repeat = sEnrollBLL.CheckIsRepeat(ifm.DeptID, studentID, ifm.Year, ifm.Month, ifm.EnrollLevel, ifm.sProfessionID, "");
                //        if (repeat)
                //        {
                //            result = "不能重复报名";
                //        }
                //        else
                //        {
                //            if (dt.Rows.Count > 0)
                //            {
                //                //续报名，不写T_Stu_sEnroll表
                //                if (editRepeatSign == "goon")
                //                {
                //                    #region 添加
                //                    //添加T_Stu_sEnrollsProfession
                //                    ifm.Status = "3";
                //                    ifm.sEnrollID = dt.Rows[0][0].ToString();
                //                    ifm.Auditor = this.UserId.ToString();
                //                    ifm.AuditTime = DateTime.Now.ToString();
                //                    ifm.AuditView = "";
                //                    ifm.CreateID = this.UserId.ToString();
                //                    ifm.CreateTime = DateTime.Now.ToString();
                //                    ifm.UpdateID = this.UserId.ToString();
                //                    ifm.UpdateTime = DateTime.Now.ToString();
                //                    int sEnrollProID = sEnrollsProfessionBLL.InsertsEnrollsProfession(ifm);

                //                    //添加T_Stu_sItemsEnroll，指定专业的缴费方案
                //                    //{"total":1,"rows":[{"ItemID":"2","Year":"2016","Month":"3月（春季）","Name":"缴费方案1"}]}
                //                    JObject job = JObject.Parse(editScheme);
                //                    string[] array = job.Properties().Where(x => x.Name == "rows").Select(item => item.Value.ToString()).ToArray();
                //                    List<sEnrollSchemeModel> schemeList = JsonConvert.DeserializeObject<List<sEnrollSchemeModel>>(array[0]);//将json数据转化为对象类型并赋值给list
                //                    foreach (var item in schemeList)
                //                    {
                //                        sItemsEnrollModel siemmodel = new sItemsEnrollModel();
                //                        siemmodel.ItemID = item.ItemID;
                //                        siemmodel.sEnrollsProfessionID = sEnrollProID.ToString();
                //                        sItemsEnrollBLL.InsertsItemsEnroll(siemmodel);
                //                    }

                //                    result = "yes";
                //                    #endregion
                //                }
                //                //重复报名，普通添加
                //                else if (editRepeatSign == "repeat")
                //                {
                //                    #region 添加
                //                    //添加T_Stu_sEnroll
                //                    sEnrollModel semodel = new sEnrollModel();
                //                    semodel.Status = "1";
                //                    semodel.DeptID = ifm.DeptID;
                //                    semodel.StudentID = studentID;
                //                    semodel.EnrollNum = sEnrollBLL.getEnrollNum(ifm.DeptID, ifm.Year, UserId.ToString());//报名号（学号）
                //                    semodel.CreateID = this.UserId.ToString();
                //                    semodel.CreateTime = DateTime.Now.ToString();
                //                    semodel.UpdateID = this.UserId.ToString();
                //                    semodel.UpdateTime = DateTime.Now.ToString();
                //                    int sEnrollID = sEnrollBLL.InsertsEnroll(semodel);

                //                    //添加T_Stu_sEnrollsProfession
                //                    ifm.Status = "3";
                //                    ifm.sEnrollID = sEnrollID.ToString();
                //                    ifm.Auditor = this.UserId.ToString();
                //                    ifm.AuditTime = DateTime.Now.ToString();
                //                    ifm.AuditView = "";
                //                    ifm.CreateID = this.UserId.ToString();
                //                    ifm.CreateTime = DateTime.Now.ToString();
                //                    ifm.UpdateID = this.UserId.ToString();
                //                    ifm.UpdateTime = DateTime.Now.ToString();
                //                    int sEnrollProID = sEnrollsProfessionBLL.InsertsEnrollsProfession(ifm);

                //                    //添加T_Stu_sItemsEnroll，指定专业的缴费方案
                //                    //{"total":1,"rows":[{"ItemID":"2","Year":"2016","Month":"3月（春季）","Name":"缴费方案1"}]}
                //                    JObject job = JObject.Parse(editScheme);
                //                    string[] array = job.Properties().Where(x => x.Name == "rows").Select(item => item.Value.ToString()).ToArray();
                //                    List<sEnrollSchemeModel> schemeList = JsonConvert.DeserializeObject<List<sEnrollSchemeModel>>(array[0]);//将json数据转化为对象类型并赋值给list
                //                    foreach (var item in schemeList)
                //                    {
                //                        sItemsEnrollModel siemmodel = new sItemsEnrollModel();
                //                        siemmodel.ItemID = item.ItemID;
                //                        siemmodel.sEnrollsProfessionID = sEnrollProID.ToString();
                //                        sItemsEnrollBLL.InsertsItemsEnroll(siemmodel);
                //                    }

                //                    result = "yes";
                //                    #endregion
                //                }
                //            }
                //            //普通添加
                //            else
                //            {
                //                #region 添加
                //                //添加T_Stu_sEnroll
                //                sEnrollModel semodel = new sEnrollModel();
                //                semodel.Status = "1";
                //                semodel.DeptID = ifm.DeptID;
                //                semodel.StudentID = studentID;
                //                semodel.EnrollNum = sEnrollBLL.getEnrollNum(ifm.DeptID, ifm.Year, UserId.ToString());//报名号（学号）
                //                semodel.CreateID = this.UserId.ToString();
                //                semodel.CreateTime = DateTime.Now.ToString();
                //                semodel.UpdateID = this.UserId.ToString();
                //                semodel.UpdateTime = DateTime.Now.ToString();
                //                int sEnrollID = sEnrollBLL.InsertsEnroll(semodel);

                //                //添加T_Stu_sEnrollsProfession
                //                ifm.Status = "3";
                //                ifm.sEnrollID = sEnrollID.ToString();
                //                ifm.Auditor = this.UserId.ToString();
                //                ifm.AuditTime = DateTime.Now.ToString();
                //                ifm.AuditView = "";
                //                ifm.CreateID = this.UserId.ToString();
                //                ifm.CreateTime = DateTime.Now.ToString();
                //                ifm.UpdateID = this.UserId.ToString();
                //                ifm.UpdateTime = DateTime.Now.ToString();
                //                int sEnrollProID = sEnrollsProfessionBLL.InsertsEnrollsProfession(ifm);

                //                //添加T_Stu_sItemsEnroll，指定专业的缴费方案
                //                //{"total":1,"rows":[{"ItemID":"2","Year":"2016","Month":"3月（春季）","Name":"缴费方案1"}]}
                //                JObject job = JObject.Parse(editScheme);
                //                string[] array = job.Properties().Where(x => x.Name == "rows").Select(item => item.Value.ToString()).ToArray();
                //                List<sEnrollSchemeModel> schemeList = JsonConvert.DeserializeObject<List<sEnrollSchemeModel>>(array[0]);//将json数据转化为对象类型并赋值给list
                //                foreach (var item in schemeList)
                //                {
                //                    sItemsEnrollModel siemmodel = new sItemsEnrollModel();
                //                    siemmodel.ItemID = item.ItemID;
                //                    siemmodel.sEnrollsProfessionID = sEnrollProID.ToString();
                //                    sItemsEnrollBLL.InsertsItemsEnroll(siemmodel);
                //                }

                //                result = "yes";
                //                #endregion
                //            }
                //        }
                //    }
                //}
                #endregion
            }
            catch
            {
                result = "出现未知错误，请联系管理员";
            }
            return result;
        }



        /// <summary>
        /// 修改绑定报名信息
        /// </summary>
        /// <returns></returns>
        public AjaxResult SelectsEnroll()
        {
            string sEnrollsProfessionID = Request.Form["ID"];
            DataTable dt = sEnrollBLL.SelectsEnroll(sEnrollsProfessionID);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        /// <summary>
        /// 修改状态（1：录取；2：报名；9：停用）
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetUpdatesStatus()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                bool result = true;
                try
                {
                    string sEnrollProId = Request.Form["ID"];
                    string status = Request.Form["Value"];
                    sEnrollsProfessionModel pm = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", sEnrollProId) });
                    pm.Status = status;
                    pm.UpdateID = this.UserId.ToString();
                    pm.UpdateTime = DateTime.Now.ToString();
                    sEnrollsProfessionBLL.UpdatesEnrollsProfession(pm);

                    //if (status == "9")
                    //{
                    //    //停用要停掉已生成的缴费单数据，停掉已生成的配品数据
                    //    sOrderBLL.UpdateStatus(sEnrollProId, status);
                    //}

                    ts.Complete();
                }
                catch
                {
                    result = false;
                    Transaction.Current.Rollback();
                }
                finally
                {
                    ts.Dispose();
                }
                if (result)
                {
                    return AjaxResult.Success();
                }
                else
                {
                    return AjaxResult.Error("出现未知错误，请联系管理员");
                }
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public AjaxResult DownloadsEnroll()
        {
            string menuId = Request.Form["MenuID"];
            string stuname = Request.Form["txtStuName"];
            string examNum = Request.Form["txtExamNum"];
            string enrollnum = Request.Form["txtEnrollNum"];
            string idcard = Request.Form["txtIDCard"];
            string signTimeS = Request.Form["txtSignTimeS"];
            string signTimeE = Request.Form["txtSignTimeE"];
            string dept = Request.Form["treeDept"];
            string level = Request.Form["selLevel"];
            string major = Request.Form["txtMajor"];
            string year = Request.Form["selYear"];
            string month = Request.Form["selMonth"];
            string status = Request.Form["selStatus"];
            string no = Request.Form["selNo"];
            string where = "";

            string firstFeeTimeS = Request.Form["txtFirstFeeTimeS"];
            string firstFeeTimeE = Request.Form["txtFirstFeeTimeE"];
            string beforeEnrollTimeS = Request.Form["txtBeforeEnrollTimeS"];
            string beforeEnrollTimeE = Request.Form["txtBeforeEnrollTimeE"];

            string createTimeS = Request.Form["txtCreateTimeS"];
            string createTimeE = Request.Form["txtCreateTimeE"];

            string planSort = Request.Form["selPlanSort"];

            if (!string.IsNullOrEmpty(planSort))
            {

                if (planSort.Substring(0, 1).Equals(","))
                {
                    planSort = planSort.Substring(1, planSort.Length - 1);
                }
                where += @"AND senrollpro.sEnrollsProfessionID IN (
        SELECT  so1.sEnrollsProfessionID
        FROM    T_Stu_sOrder so1
        WHERE   so1.Status <> 9
                AND so1.PlanSort IN(" + planSort + ")  )";
            }
            if (!string.IsNullOrEmpty(createTimeS))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.CreateTime,23) >= '" + createTimeS + "'";
            }

            if (!string.IsNullOrEmpty(createTimeE))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.CreateTime,23) <= '" + createTimeE + "'";
            }

            if (!string.IsNullOrEmpty(firstFeeTimeS))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.FirstFeeTime,23) >= '" + firstFeeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(firstFeeTimeE))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.FirstFeeTime,23) <= '" + firstFeeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(beforeEnrollTimeS))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.BeforeEnrollTime,23) >= '" + beforeEnrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(beforeEnrollTimeE))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.BeforeEnrollTime,23) <= '" + beforeEnrollTimeE + "'";
            }
            if (!string.IsNullOrEmpty(stuname))
            {
                where += " AND student.Name like '%" + stuname + "%'";
            }
            if (!string.IsNullOrEmpty(examNum))
            {
                where += " AND senroll.ExamNum like '%" + examNum + "%'";
            }
            if (!string.IsNullOrEmpty(enrollnum))
            {
                where += " AND senroll.EnrollNum like '%" + enrollnum + "%'";
            }
            if (!string.IsNullOrEmpty(idcard))
            {
                where += " AND student.IDCard like '%" + idcard + "%'";
            }
            if (!string.IsNullOrEmpty(signTimeS))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.EnrollTime,23) >= '" + signTimeS + "'";
            }
            if (!string.IsNullOrEmpty(signTimeE))
            {
                where += " AND convert(NVARCHAR(10),senrollpro.EnrollTime,23) <= '" + signTimeE + "'";
            }
            if (!string.IsNullOrEmpty(dept))
            {
                where += " AND senrollpro.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(level))
            {
                where += OtherHelper.MultiSelectToSQLWhere(level, "senrollpro.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(major))
            {
                where += " AND pro.Name like '%" + major + "%'";
            }
            if (!string.IsNullOrEmpty(year))
            {
                where += OtherHelper.MultiSelectToSQLWhere(year, "senrollpro.Year");
            }
            if (!string.IsNullOrEmpty(month))
            {
                where += OtherHelper.MultiSelectToSQLWhere(month, "refe_month.Value");
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "senrollpro.Status");
            }
            if (!string.IsNullOrEmpty(no))
            {
                if (no == "1")
                {
                    where += @" AND ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
                    AND IsGive <> 2
                    AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) > 0";
                }
                else if (no == "2")
                {
                    where += @" AND ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
                    AND IsGive = 2
                    AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) > 0";
                }
            }
            string cmdText = @"SELECT  refe_luqu.RefeName AS 状态 ,
        dept.Name AS 报名学校 ,
        da.Name 报名校区 ,
        senroll.ExamNum 考生号 ,
        student.Name AS 学生姓名 ,
        r1.RefeName 性别 ,
        senroll.EnrollNum 学号 ,
        student.IDCard 身份证号 ,
        student.Mobile 手机号 ,
        studentinfo.School 毕业学校 ,
        province.Name + city.Name + district.Name 生源地 ,
        student.Address 家庭地址 ,
        ( SELECT DISTINCT
                    PlanName + ','
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status <> 9
        FOR
          XML PATH('')
        ) 缴费方案 ,
        senrollpro.Year 年份 ,
        refe_month.RefeName AS 月份 ,
        ( SELECT    r.RefeName + ','
          FROM      T_Stu_sOrder so
                    LEFT JOIN T_Sys_Refe r ON so.PlanSort = r.Value
                                              AND r.RefeTypeID = 14
          WHERE     so.sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND so.Status <> 9
          GROUP BY  so.PlanItemID ,
                    r.RefeName
        FOR
          XML PATH('')
        ) 报读类别 ,
        refe_level.RefeName AS 学习层次 ,
        pro.Name AS 专业 ,
        c.Name 班级 ,
        CASE CONVERT(NVARCHAR(10), senrollpro.BeforeEnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), senrollpro.BeforeEnrollTime, 23)
        END 预报名时间 ,
        CASE CONVERT(NVARCHAR(10), senrollpro.EnrollTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), senrollpro.EnrollTime, 23)
        END 正式报名时间 ,
        CASE CONVERT(NVARCHAR(10), senrollpro.FirstFeeTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), senrollpro.FirstFeeTime, 23)
        END 第一次缴费时间 ,
        senrollpro.LeaveYear 离校年度 ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
        ) AS 总学费金额 ,
        ( SELECT    ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
        ) AS 已供贷金额 ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
        ) - ( SELECT    ISNULL(SUM(PaidMoney), 0)
              FROM      T_Stu_sOrder
              WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                        AND Status != 9
            ) 未供贷金额 ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
                    AND IsGive <> 2
                    AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) AS 逾期欠费金额 ,
        ( SELECT    ISNULL(SUM(ShouldMoney), 0) - ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
                    AND Status != 9
                    AND IsGive = 2
                    AND CONVERT(NVARCHAR(10), LimitTime, 23) <= CONVERT(NVARCHAR(10), GETDATE(), 23)
        ) AS '逾期欠费(补助)' ,
        ( SELECT TOP 1
                    eps.Explain + ',' + CONVERT(NVARCHAR(10), eps.CreateTime, 23)
          FROM      T_Stu_sEnrollsProfessionStatus eps
          WHERE     eps.sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
          ORDER BY  eps.sEnrollsProfessionStatusID DESC
        ) 状态变更原因 ,
        ( SELECT    sc.Name + ','
          FROM      T_Pro_StudentContact sc
          WHERE     sc.StudentID = student.StudentID
        FOR
          XML PATH('')
        ) 家长姓名 ,
        u.Name 创建人 ,
        CASE CONVERT(NVARCHAR(10), senrollpro.CreateTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), senrollpro.CreateTime, 23)
        END 创建时间
FROM    T_Stu_sEnrollsProfession AS senrollpro
        LEFT JOIN T_Stu_sEnroll AS senroll ON senroll.sEnrollID = senrollpro.sEnrollID
        LEFT JOIN T_Sys_Dept AS dept ON senrollpro.DeptID = dept.DeptID
        LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
        LEFT JOIN T_Pro_StudentInfo AS studentinfo ON student.StudentID = studentinfo.StudentID
        LEFT JOIN T_Stu_sProfession AS spro ON senrollpro.sProfessionID = spro.sProfessionID
        LEFT JOIN T_Pro_Profession AS pro ON spro.ProfessionID = pro.ProfessionID
        LEFT JOIN T_Pro_Class c ON c.ClassID = senrollpro.ClassID
        LEFT JOIN T_Sys_Refe AS refe_month ON senrollpro.Month = refe_month.Value
                                              AND refe_month.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS refe_luqu ON senrollpro.Status = refe_luqu.Value
                                             AND refe_luqu.RefeTypeID = 21
        LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                              AND refe_level.RefeTypeID = 17
        LEFT JOIN T_Pro_DeptArea da ON senrollpro.DeptAreaID = da.DeptAreaID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = student.Sex
                                   AND r1.RefeTypeID = 3
        LEFT JOIN T_Pro_Area province ON studentinfo.ProvinceID = province.AreaID
        LEFT JOIN T_Pro_Area city ON studentinfo.CityID = city.AreaID
        LEFT JOIN T_Pro_Area district ON studentinfo.DistrictID = district.AreaID
        LEFT JOIN T_Sys_User u ON senrollpro.CreateID = u.UserID
WHERE   1 = 1
{0}
ORDER BY senrollpro.sEnrollsProfessionID DESC";
            string filename = "报名信息.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "senrollpro.DeptID", "senrollpro.Auditor");
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }

        /// <summary>
        /// 验证选择的报名信息中是否有不能生成的数据（只有在录取、报名、在校状态下才能生成缴费单，1,2,3,4）
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckIsBuildsFee()
        {
            string sEnrollsProfessionIDs = Request.Form["ID"];
            bool result = sEnrollBLL.CheckIsBuildsFee(sEnrollsProfessionIDs);
            return AjaxResult.Success(result);
        }

        /// <summary>
        /// 生成缴费单
        /// </summary>
        /// <returns></returns>
        public AjaxResult BuildsFee()
        {
            string sEnrollsProfessionID = Request.Form["ID"];
            string result = sOrderBLL.BuildsOrder(sEnrollsProfessionID, this.UserId.ToString());
            return AjaxResult.Success(result);
        }

        /// <summary>
        /// 根据主体、年、月获取缴费方案（排除停用的）
        /// </summary>
        /// <returns></returns>
        public string SelScheme()
        {
            string dept = Request.Form["DeptID"];
            string year = Request.Form["Year"];
            string month = Request.Form["Month"];
            DataTable dt = sEnrollBLL.GetScheme(dept, year, month);
            return JsonHelper.DataTableToJson(dt);
        }

        /// <summary>
        /// 批量生成缴费单时，得到报名专业ID
        /// </summary>
        /// <returns></returns>
        public string GetBuildsProfessionIDs()
        {
            string level = Request.Form["buildEnrollLevel"];
            string sProfessionIDs = Request.Form["buildsProfessionID"];
            string schemes = Request.Form["buildScheme"];
            return sEnrollBLL.GetBuildsProfessionIDs(level, sProfessionIDs, schemes);
        }

        /// <summary>
        /// 得到报名专业状态
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetsEnrollsProfessionStatus()
        {
            string sEnrollsProfessionID = Request.Form["ID"];
            sEnrollsProfessionModel model = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID) });
            return AjaxResult.Success(model.Status);
        }

        /// <summary>
        /// 保存停用
        /// </summary>
        /// <returns></returns>
        public string GetsEnrollStop(sEnrollsProfessionNoteModel pm)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                bool result = true;
                try
                {
                    #region MyRegion
                    //插入T_Stu_sEnrollsProfessionNote（停用记录表）
                    pm.Status = "1";
                    pm.NewsEnrollsProfessionID = "0";
                    pm.NoteTime = DateTime.Now.ToString();
                    pm.Explain = pm.Explain.Replace("\r\n", "<br />");
                    pm.CreateID = this.UserId.ToString();
                    pm.CreateTime = DateTime.Now.ToString();
                    pm.UpdateID = this.UserId.ToString();
                    pm.UpdateTime = DateTime.Now.ToString();
                    sEnrollsProfessionNoteBLL.InsertsEnrollsProfessionNote(pm);

                    //插入T_Stu_sEnrollsProfessionsOrder（停用报名专业缴费单表）
                    DataTable sOrderDt = sOrderBLL.sOrderTableByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID AND Status!=9", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", pm.sEnrollsProfessionID) }, "");
                    if (sOrderDt.Rows.Count > 0)
                    {
                        for (var i = 0; i < sOrderDt.Rows.Count; i++)
                        {
                            sEnrollsProfessionsOrderModel pom = new sEnrollsProfessionsOrderModel();
                            pom.sEnrollsProfessionID = pm.sEnrollsProfessionID;
                            pom.IsNumItem = "2";
                            pom.sOrderID = sOrderDt.Rows[i]["sOrderID"].ToString();
                            sEnrollsProfessionsOrderBLL.InsertsEnrollsProfessionsOrder(pom);
                        }
                    }

                    //插入T_Stu_sEnrollsProfessionsOrderGive（停用报名专业缴费单配品表）
                    DataTable sOrderGiveDt = sOrderGiveBLL.sOrderGiveTableByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID AND Status!=9", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", pm.sEnrollsProfessionID) }, "");
                    if (sOrderGiveDt.Rows.Count > 0)
                    {
                        for (var i = 0; i < sOrderGiveDt.Rows.Count; i++)
                        {
                            sEnrollsProfessionsOrderGiveModel pgm = new sEnrollsProfessionsOrderGiveModel();
                            pgm.sEnrollsProfessionID = pm.sEnrollsProfessionID;
                            pgm.sOrderGiveID = sOrderGiveDt.Rows[i]["sOrderGiveID"].ToString();
                            sEnrollsProfessionsOrderGiveBLL.InsertsEnrollsProfessionsOrderGive(pgm);
                        }
                    }

                    //停用要停掉已生成的缴费单数据，停掉已生成的配品数据
                    sEnrollsProfessionModel spm = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", pm.sEnrollsProfessionID) });
                    spm.Status = "9";
                    spm.UpdateID = this.UserId.ToString();
                    spm.UpdateTime = DateTime.Now.ToString();
                    sEnrollsProfessionBLL.UpdatesEnrollsProfession(spm);
                    sOrderBLL.UpdateStatus(pm.sEnrollsProfessionID, "9");

                    ts.Complete();
                    #endregion
                }
                catch
                {
                    result = false;
                    Transaction.Current.Rollback();
                }
                finally
                {
                    ts.Dispose();
                }
                if (result)
                {
                    return "yes";
                }
                else
                {
                    return "出现未知错误，请联系管理员";
                }
            }
        }

        /// <summary>
        /// 保存转专业
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetsEnrollMajor(sEnrollsProfessionModel model)
        {
            string OldID = Request.Form["MajorsEnrollsProfessionID"];
            JObject job = JObject.Parse(Request.Form["MajorScheme"]);
            string MajorScheme = Request.Form["MajorScheme"];
            string MajorChangeTime = Request.Form["MajorChangeTime"];
            string MajorExplain = Request.Form["MajorExplain"];
            string[] readYear = Request.Form["Read"].Split(",");
            for (int i = 0; i < readYear.Length; i++)
            {
                if (readYear[i].Equals("0") && readYear.Length > 1)
                {
                    return AjaxResult.Error("新生报到和其他选项不能混合选择");
                }
                if (!readYear[i].Equals("0"))
                {
                    string where = "and  NumItemID=@NumItemID AND sEnrollsProfessionID=@sEnrollsProfessionID  AND IsGive<>2 AND ShouldMoney<>PaidMoney";
                    SqlParameter[] paras = new SqlParameter[] {
                        new SqlParameter("@NumItemID",readYear[i]),
                         new SqlParameter("@sEnrollsProfessionID",OldID)
                    };
                    DataTable dt = sOrderBLL.sOrderTableByWhere(where, paras, "");
                    if (dt.Rows.Count > 0)
                    {
                        string level = RefeBLL.GetRefeName(dt.Rows[0]["PlanLevel"].ToString(), "17");
                        return AjaxResult.Error(level + "_" + dt.Rows[0]["NumName"].ToString() + "欠费,请缴清欠费再转专业！");
                    }
                }
            }

            string result = sEnrollBLL.ChangesEnrollProfession(model, UserId.ToString(), MajorScheme, OldID, MajorChangeTime, MajorExplain, readYear);
            if (!string.IsNullOrEmpty(result))
            {
                //DataTable feeOrder = sOrderBLL.GetsOrderTable(result, true);
                //EnrollSendModel esm = new EnrollSendModel();
                //esm.returnUrl = OtherHelper.GetAppSettingsValue("ReturnURL");
                //esm.idCard = sEnrollsProfessionBLL.GetStudentIdCard(result);
                //esm.deptId = DeptDCPBLL.GetDcpDeptID(model.DeptID).ToString();
                //esm.major = ProfessionBLL.GetProfesionName(result);
                //esm.payed = feeOrder.Rows.Count > 0 ? "1" : "0";
                //string key = esm.deptId + esm.idCard + esm.major + esm.payed + OtherHelper.GetAppSettingsValue("Key");
                //esm.key = Encryption.MD5Encrypt(key).ToLower();
                //OtherHelper.HostingReturn(OtherHelper.GetAppSettingsValue("EnrollAuditURL"), OtherHelper.JsonSerializer(esm));
                return AjaxResult.Success(result);
            }
            else
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
        }

        /// <summary>
        /// 得到报名专业的缴费方案
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetSchemeJson()
        {
            string cmdText = string.Empty;
            string sEnrollsProfessionID = Request.Form["ID"];
            sEnrollsProfessionModel model = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID) });
            if (string.IsNullOrEmpty(model.Status))
            {
                return AjaxResult.Success("[]");
            }
            else
            {
                if (model.Status == "9")
                {
                    //停用的报名专业从T_Stu_sEnrollsProfessionsOrder表读取缴费单，从T_Stu_sEnrollsProfessionsOrderGive表读取配品
                    cmdText = @"SELECT DISTINCT
        sorder.PlanItemID AS id ,
        sorder.PlanName AS text
FROM    T_Stu_sEnrollsProfessionsOrder AS proorder
        LEFT JOIN T_Stu_sOrder AS sorder ON sorder.sOrderID = proorder.sOrderID
WHERE   proorder.sEnrollsProfessionID = " + sEnrollsProfessionID;
                }
                else
                {
                    //未停用的报名专业从T_Stu_sOrder表读取缴费单，从T_Stu_sOrderGive表读取配品
                    cmdText = @"SELECT DISTINCT
        PlanItemID AS id ,
        PlanName AS text
FROM    T_Stu_sOrder
WHERE   sEnrollsProfessionID = {0}
        AND Status != 9";
                    cmdText = string.Format(cmdText, sEnrollsProfessionID);
                }
                DataTable dt = sEnrollBLL.GetDataTable(cmdText);
                return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
            }
        }

        /// <summary>
        /// 得到缴费方案的缴费期数
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetSemesterJson()
        {
            string cmdText = string.Empty;
            string majorID = Request.Form["MajorID"];
            string planItemID = Request.Form["PlanItemID"];
            sEnrollsProfessionModel model = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", majorID) });
            if (string.IsNullOrEmpty(model.Status))
            {
                return AjaxResult.Success("[]");
            }
            else
            {
                if (model.Status == "9")
                {
                    //停用的报名专业从T_Stu_sEnrollsProfessionsOrder表读取缴费单，从T_Stu_sEnrollsProfessionsOrderGive表读取配品
                    cmdText = @"SELECT DISTINCT
        sorder.NumItemID ,
        sorder.NumName ,
        sorder.ItemQueue
FROM    T_Stu_sEnrollsProfessionsOrder AS proorder
        LEFT JOIN T_Stu_sOrder AS sorder ON sorder.sOrderID = proorder.sOrderID
WHERE   proorder.sEnrollsProfessionID = {0}
        AND sorder.PlanItemID = {1}
ORDER BY sorder.ItemQueue ASC";
                    cmdText = string.Format(cmdText, majorID, planItemID);
                }
                else
                {
                    //未停用的报名专业从T_Stu_sOrder表读取缴费单，从T_Stu_sOrderGive表读取配品
                    cmdText = @"SELECT DISTINCT
        NumItemID ,
        NumName ,
        ItemQueue
FROM    T_Stu_sOrder
WHERE   sEnrollsProfessionID = {0}
        AND PlanItemID = {1}
        AND Status != 9
ORDER BY ItemQueue ASC";
                    cmdText = string.Format(cmdText, majorID, planItemID);
                }
                DataTable dt = sEnrollBLL.GetDataTable(cmdText);
                return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
            }
        }

        /// <summary>
        /// 得到缴费方案的缴费单
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetSchemeOrderJson()
        {
            string cmdText = string.Empty;
            string majorID = Request.Form["MajorID"];
            string planItemID = Request.Form["PlanItemID"];
            sEnrollsProfessionModel model = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", majorID) });
            if (string.IsNullOrEmpty(model.Status))
            {
                return AjaxResult.Success("[]");
            }
            else
            {
                if (model.Status == "9")
                {
                    //停用的报名专业从T_Stu_sEnrollsProfessionsOrder表读取缴费单，从T_Stu_sEnrollsProfessionsOrderGive表读取配品
                    cmdText = @"SELECT  sorder.sOrderID ,
        sorder.NumItemID ,
        detail.Name ,
        sorder.ShouldMoney ,
        sorder.PaidMoney ,
        ( sorder.ShouldMoney - sorder.PaidMoney ) AS NoPayMoney ,
        CONVERT(NVARCHAR(23), sorder.LimitTime, 23) AS LimitTime
FROM    T_Stu_sEnrollsProfessionsOrder AS proorder
        LEFT JOIN T_Stu_sOrder AS sorder ON sorder.sOrderID = proorder.sOrderID
        LEFT JOIN T_Pro_Detail AS detail ON sorder.DetailID = detail.DetailID
WHERE   proorder.sEnrollsProfessionID = {0}
        AND sorder.PlanItemID = {1}
		ORDER BY sorder.ItemDetailQueue ASC";
                    cmdText = string.Format(cmdText, majorID, planItemID);
                }
                else
                {
                    //未停用的报名专业从T_Stu_sOrder表读取缴费单，从T_Stu_sOrderGive表读取配品
                    cmdText = @"SELECT  sorder.sOrderID ,
        sorder.NumItemID ,
        detail.Name ,
        sorder.ShouldMoney ,
        sorder.PaidMoney ,
        ( sorder.ShouldMoney - sorder.PaidMoney ) AS NoPayMoney ,
        CONVERT(NVARCHAR(23), sorder.LimitTime, 23) AS LimitTime
FROM    dbo.T_Stu_sOrder AS sorder
        LEFT JOIN T_Pro_Detail AS detail ON sorder.DetailID = detail.DetailID
WHERE   sorder.sEnrollsProfessionID = {0}
        AND sorder.PlanItemID = {1}
        AND sorder.Status != 9
ORDER BY sorder.ItemDetailQueue ASC";
                    cmdText = string.Format(cmdText, majorID, planItemID);
                }
                DataTable dt = sEnrollBLL.GetDataTable(cmdText);
                return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
            }
        }

        /// <summary>
        /// 得到缴费方案的配品
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetSchemeOrderGiveJson()
        {
            string cmdText = string.Empty;
            string majorID = Request.Form["MajorID"];
            string planItemID = Request.Form["PlanItemID"];
            sEnrollsProfessionModel model = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", majorID) });
            if (string.IsNullOrEmpty(model.Status))
            {
                return AjaxResult.Success("[]");
            }
            else
            {
                if (model.Status == "9")
                {
                    //停用的报名专业从T_Stu_sEnrollsProfessionsOrder表读取缴费单，从T_Stu_sEnrollsProfessionsOrderGive表读取配品
                    cmdText = @"SELECT DISTINCT
        sordergive.sOrderGiveID ,
        sgive.Name ,
        sordergive.Queue
FROM    T_Stu_sEnrollsProfessionsOrderGive AS proordergive
        LEFT JOIN T_Stu_sOrderGive AS sordergive ON sordergive.sOrderGiveID = proordergive.sOrderGiveID
        LEFT JOIN T_Stu_sGive AS sgive ON sgive.sGiveID = sordergive.sGiveID
WHERE   proordergive.sEnrollsProfessionID = {0}
        AND sordergive.PlanItemID = {1}
ORDER BY sordergive.Queue ASC";
                    cmdText = string.Format(cmdText, majorID, planItemID);
                }
                else
                {
                    //未停用的报名专业从T_Stu_sOrder表读取缴费单，从T_Stu_sOrderGive表读取配品
                    cmdText = @"SELECT DISTINCT
        sordergive.sOrderGiveID ,
        sgive.Name ,
        sordergive.Queue
FROM    T_Stu_sOrderGive AS sordergive
        LEFT JOIN T_Stu_sGive AS sgive ON sgive.sGiveID = sordergive.sGiveID
WHERE   sordergive.sEnrollsProfessionID = {0}
        AND sordergive.PlanItemID = {1}
        AND sordergive.Status != 9
ORDER BY sordergive.Queue ASC";
                    cmdText = string.Format(cmdText, majorID, planItemID);
                }
                DataTable dt = sEnrollBLL.GetDataTable(cmdText);
                return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
            }
        }

        /// <summary>
        /// 得到报名专业的缴费单状态
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetsOrderStatus()
        {
            string sEnrollsProfessionID = Request.Form["ID"];
            string statusStr = sEnrollBLL.GetsOrderStatus(sEnrollsProfessionID);
            return AjaxResult.Success(statusStr);
        }
        #endregion
        /// <summary>
        /// 添加报名
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public AjaxResult AddEnroll(sEnrollProfessionViewModel em)
        {
            string enrollProfessionId = "";
            if (string.IsNullOrEmpty(em.name))
            {
                return AjaxResult.Error("姓名不能为空");
            }
            else
            {
                if (em.name.Length > 16)
                {
                    return AjaxResult.Error("姓名不能超过16个字符");
                }
            }
            if (!string.IsNullOrEmpty(em.idCard))
            {
                if (!OtherHelper.CheckIDCard(em.idCard))
                {
                    return AjaxResult.Error("身份证不规范");
                }
                StudentModel sm = StudentBLL.GetStudentModel(em.idCard);
                if (!string.IsNullOrEmpty(sm.StudentID))
                {
                    if (!sm.Name.Equals(em.name))
                    {
                        return AjaxResult.Error("身份证号和姓名不匹配");

                    }
                    else
                    {
                        em.studentId = sm.StudentID;
                    }
                }
            }
            if (string.IsNullOrEmpty(em.sex))
            {
                return AjaxResult.Error("请选择性别");
            }
            if (string.IsNullOrEmpty(em.mobile))
            {
                return AjaxResult.Error("联系电话不能为空");
            }
            else
            {
                if (em.mobile.Length > 16)
                {
                    return AjaxResult.Error("联系电话不能超过16个字符");
                }
            }

            if (!string.IsNullOrEmpty(em.qq))
            {
                if (em.qq.Length > 16)
                {
                    return AjaxResult.Error("QQ不能超过16个字符");
                }
            }
            if (!string.IsNullOrEmpty(em.weChat))
            {
                if (em.weChat.Length > 32)
                {
                    return AjaxResult.Error("微信不能超过32个字符");
                }
            }
            if (!string.IsNullOrEmpty(em.address))
            {
                if (em.address.Length > 128)
                {
                    return AjaxResult.Error("地址不能超过128个字符");
                }
            }
            if (!string.IsNullOrEmpty(em.parentName))
            {
                if (em.parentName.Length > 32)
                {
                    return AjaxResult.Error("父母姓名不能超过32个字符");
                }
            }
            if (!string.IsNullOrEmpty(em.tel))
            {
                if (em.tel.Length > 32)
                {
                    return AjaxResult.Error("父母电话不能超过32个字符");
                }
            }

            if (!string.IsNullOrEmpty(em.school))
            {
                if (em.school.Length > 64)
                {
                    return AjaxResult.Error("毕业学校不能超过64个字符");
                }
            }
            if (!string.IsNullOrEmpty(em.zip))
            {
                if (em.zip.Length > 8)
                {
                    return AjaxResult.Error("邮编不能超过8个字符");
                }
            }
            if (string.IsNullOrEmpty(em.deptId))
            {
                return AjaxResult.Error("请选择校区");
            }
            if (string.IsNullOrEmpty(em.year))
            {
                return AjaxResult.Error("请选择年份");
            }
            if (string.IsNullOrEmpty(em.month))
            {
                return AjaxResult.Error("请选择月份");
            }
            if (string.IsNullOrEmpty(em.enrollLevel))
            {
                return AjaxResult.Error("请选择学习层次");
            }
            if (string.IsNullOrEmpty(em.professionId))
            {
                return AjaxResult.Error("请选择专业");
            }
            if (string.IsNullOrEmpty(em.enrollTime))
            {
                return AjaxResult.Error("报名时间不能为空");
            }
            if (string.IsNullOrEmpty(em.scheme))
            {
                return AjaxResult.Error("请选择缴费方案");
            }
            if (string.IsNullOrEmpty(em.studentId))
            {
                em.studentId = InsertStudent(em);
            }
            if (!string.IsNullOrEmpty(em.studentId))
            {
                DataTable dt = sEnrollBLL.GetStudentEnroll(em.studentId);
                if (dt.Rows.Count > 0 && (em.isRepeat.Equals("1") || em.isRepeat.Equals("2")))
                {
                    return AjaxResult.Error("该生已报过名，请选择“续报名”或“重复报名”");
                }
                else
                {
                    bool repeat = sEnrollBLL.CheckIsRepeat(em.deptId, em.studentId, em.year, em.month, em.enrollLevel, em.professionId, "");
                    if (repeat)
                    {
                        return AjaxResult.Error("不能重复报名");
                    }
                    else
                    {
                        UpdateStudent(em);
                        UpdateStudentInfo(em);
                        if (!string.IsNullOrEmpty(em.parentName) || !string.IsNullOrEmpty(em.tel))
                        {
                            UpdateStudentContact(em);
                        }
                        string enrollId = "";
                        if (dt.Rows.Count > 0)
                        {
                            if (em.isRepeat.Equals("3") || em.isRepeat.Equals("5"))
                            {
                                enrollId = dt.Rows[0]["sEnrollID"].ToString();
                                if (em.isRepeat.Equals("3"))
                                {
                                    if (string.IsNullOrEmpty(dt.Rows[0]["EnrollNum"].ToString()))
                                    {
                                        UpdateEnrollNum(enrollId, em.year, em.deptId);
                                    }
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(enrollId))
                        {
                            enrollId = InsertEnroll(em);
                        }
                        enrollProfessionId = InsertEnrollProfession(em, enrollId);
                        InsertItemEnroll(em.scheme, enrollProfessionId);

                        List<sEnrollSchemeModel> list = JsonConvert.DeserializeObject<List<sEnrollSchemeModel>>(em.scheme);
                        if (list.Count > 0)
                        {

                            string parentId = string.Empty;
                            foreach (var item in list)
                            {
                                parentId += item.ItemID + ",";
                            }
                            parentId = parentId.Substring(0, parentId.Length - 1);
                            DataTable itemTable = ItemBLL.GetItemChildModel(parentId);
                            if (itemTable.Rows.Count > 0)
                            {
                                DateTime date = Convert.ToDateTime(itemTable.Rows[0]["LimitTime"].ToString());
                                string leaveYear = (date.Year + 1).ToString();
                                sEnrollsProfessionModel newepm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(enrollProfessionId);
                                newepm.LeaveYear = leaveYear;
                                newepm.UpdateID = UserId.ToString();
                                newepm.UpdateTime = DateTime.Now.ToString();
                                sEnrollsProfessionBLL.UpdatesEnrollsProfession(newepm);

                            }
                        }

                    }

                }

            }
            return AjaxResult.Success(enrollProfessionId);
        }
        /// <summary>
        /// 修改学生信息
        /// </summary>
        /// <param name="em"></param>
        private void UpdateStudent(sEnrollProfessionViewModel em)
        {
            string where = " and StudentID=@StudentID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",em.studentId)
            };
            StudentModel sm = StudentBLL.StudentModelByWhere(where, paras);
            sm.Address = em.address;
            sm.Mobile = em.mobile;
            sm.QQ = em.qq;
            sm.WeChat = em.weChat;
            sm.Sex = em.sex;
            sm.UpdateID = UserId.ToString();
            sm.UpdateTime = DateTime.Now.ToString();
            StudentBLL.UpdateStudent(sm);
        }
        /// <summary>
        /// 修改学生扩展信息
        /// </summary>
        /// <param name="em"></param>
        private void UpdateStudentInfo(sEnrollProfessionViewModel em)
        {
            if (!string.IsNullOrEmpty(em.studentInfoId))
            {
                string where = " and StudentInfoID=@StudentInfoID";
                SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentInfoID",em.studentInfoId)
            };
                StudentInfoModel sim = StudentInfoBLL.StudentInfoModelByWhere(where, paras);
                sim.ProvinceID = em.province;
                sim.DistrictID = em.district;
                sim.CityID = em.city;
                sim.Nation = em.nation;
                sim.School = em.school;
                sim.Zip = em.zip;
                sim.UpdateID = UserId.ToString();
                sim.UpdateTime = DateTime.Now.ToString();
                StudentInfoBLL.UpdateStudentInfo(sim);
            }
            else
            {
                InsertStudentInfo(em);
            }
        }
        /// <summary>
        /// 修改学生联系人信息
        /// </summary>
        /// <param name="em"></param>
        private void UpdateStudentContact(sEnrollProfessionViewModel em)
        {

            if (!string.IsNullOrEmpty(em.studentContactId))
            {
                string where = " and StudentContactID=@StudentContactID";
                SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentContactID",em.studentContactId)
                    };
                StudentContactModel scm = StudentContactBLL.StudentContactModelByWhere(where, paras);
                scm.Name = em.parentName;
                scm.Tel = em.tel;
                scm.UpdateID = UserId.ToString();
                scm.UpdateTime = DateTime.Now.ToString();
                StudentContactBLL.UpdateStudentContact(scm);
            }
            else
            {
                InsertStudentContact(em);
            }
        }
        /// <summary>
        /// 添加报名专业信息
        /// </summary>
        /// <param name="em"></param>
        /// <param name="enrollId">报名id</param>
        /// <returns></returns>
        private string InsertEnrollProfession(sEnrollProfessionViewModel em, string enrollId)
        {
            sEnrollsProfessionModel epm = new sEnrollsProfessionModel();
            epm.CreateID = UserId.ToString();
            epm.CreateTime = DateTime.Now.ToString();
            epm.DeptID = em.deptId;
            epm.EnrollLevel = em.enrollLevel;

            epm.Month = em.month;
            if (!string.IsNullOrEmpty(em.remark))
            {
                epm.Remark = em.remark.Replace("\r\n", "<br />");
            }
            else
                epm.Remark = "";

            epm.sEnrollID = enrollId;
            epm.sProfessionID = em.professionId;
            epm.ClassID = em.classId;
            epm.Status = "3";
            if (em.isRepeat.Equals("2") || em.isRepeat.Equals("5"))
            {
                epm.Status = "2";
                epm.BeforeEnrollTime = em.enrollTime;
                epm.EnrollTime = "1900-01-01";
                epm.FirstFeeTime = "1900-01-01";
            }
            else if (em.isRepeat.Equals("1") || em.isRepeat.Equals("3"))
            {
                epm.BeforeEnrollTime = "1900-01-01";
                epm.EnrollTime = em.enrollTime;
                epm.FirstFeeTime = "1900-01-01";
            }
            epm.DeptAreaID = em.senrollDeptId;
            epm.Auditor = UserId.ToString();
            epm.AuditTime = DateTime.Now.ToString();
            epm.AuditView = "";
            epm.UpdateID = UserId.ToString();
            epm.UpdateTime = DateTime.Now.ToString();
            epm.Year = em.year;
            epm.sEnrollsProfessionID = sEnrollsProfessionBLL.InsertsEnrollsProfession(epm).ToString();
            return epm.sEnrollsProfessionID;
        }

        /// <summary>
        /// 添加报名专业和方案的关系表
        /// </summary>
        /// <param name="scheme">缴费方案信息</param>
        /// <param name="enrollProfessionId">报名专业id</param>
        private void InsertItemEnroll(string scheme, string enrollProfessionId)
        {
            List<sEnrollSchemeModel> list = JsonConvert.DeserializeObject<List<sEnrollSchemeModel>>(scheme);
            foreach (var item in list)
            {
                sItemsEnrollModel iem = new sItemsEnrollModel();
                iem.ItemID = item.ItemID;
                iem.sEnrollsProfessionID = enrollProfessionId;
                sItemsEnrollBLL.InsertsItemsEnroll(iem);
            }
        }
        /// <summary>
        /// 添加报名信息
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        private string InsertEnroll(sEnrollProfessionViewModel em)
        {
            sEnrollModel e = new sEnrollModel();
            e.CreateID = UserId.ToString();
            e.CreateTime = DateTime.Now.ToString();
            e.DeptID = em.deptId;
            if (!em.isRepeat.Equals("2") && !em.isRepeat.Equals("5"))
                e.EnrollNum = sEnrollBLL.getEnrollNum(em.deptId, em.year, UserId.ToString());
            else
            {
                e.EnrollNum = "";
            }
            e.Status = "1";
            e.StudentID = em.studentId;
            e.UpdateID = UserId.ToString();
            e.UpdateTime = DateTime.Now.ToString();
            e.sEnrollID = sEnrollBLL.InsertsEnroll(e).ToString();
            return e.sEnrollID;
        }
        /// <summary>
        /// 添加学生信息
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        private string InsertStudent(sEnrollProfessionViewModel em)
        {
            StudentModel sm = new StudentModel();
            sm.Address = em.address;
            sm.CreateID = UserId.ToString();
            sm.CreateTime = DateTime.Now.ToString();
            sm.DeptID = em.deptId;
            sm.IDCard = em.idCard;
            sm.Mobile = em.mobile;
            sm.Name = em.name;
            sm.QQ = em.qq;
            sm.Remark = "";
            sm.Sex = em.sex;
            sm.Status = "1";
            sm.WeChat = em.weChat;
            sm.UpdateID = UserId.ToString();
            sm.UpdateTime = DateTime.Now.ToString();
            sm.StudentID = StudentBLL.InsertStudent(sm).ToString();
            return sm.StudentID;
        }
        /// <summary>
        /// 新增学生扩展信息
        /// </summary>
        /// <param name="em"></param>
        private void InsertStudentInfo(sEnrollProfessionViewModel em)
        {
            StudentInfoModel sim = new StudentInfoModel();
            sim.ProvinceID = em.province;
            sim.CityID = em.city;
            sim.DistrictID = em.district;
            sim.CreateID = this.UserId.ToString();
            sim.CreateTime = DateTime.Now.ToString();
            sim.Zip = em.zip;
            sim.Nation = em.nation;
            sim.School = em.school;
            sim.Status = "1";
            sim.StudentID = em.studentId;
            sim.UpdateID = UserId.ToString();
            sim.UpdateTime = DateTime.Now.ToString();
            sim.Zip = em.zip;
            StudentInfoBLL.InsertStudentInfo(sim);
        }
        /// <summary>
        /// 添加学生联系人信息
        /// </summary>
        /// <param name="em"></param>
        private void InsertStudentContact(sEnrollProfessionViewModel em)
        {
            StudentContactModel scm = new StudentContactModel();
            scm.CreateID = this.UserId.ToString();
            scm.CreateTime = DateTime.Now.ToString();
            scm.UpdateID = UserId.ToString();
            scm.UpdateTime = DateTime.Now.ToString();
            scm.Status = "1";
            scm.Name = em.parentName;
            scm.Tel = em.tel;
            scm.StudentID = em.studentId;
            StudentContactBLL.InsertStudentContact(scm);
        }

        /// <summary>
        /// 保存转正报
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="stuId"></param>
        /// <param name="sEnrollTime"></param>
        /// <returns></returns>
        public string GetsEnrollTurn(string Id, string stuId, string sEnrollTime, string idCardTurn)
        {
            if (!OtherHelper.CheckIDCard(idCardTurn))
            {
                return "身份证号不规范";
            }

            if (StudentBLL.ValidateIDCard(stuId, idCardTurn))
            {
                return "身份证号已被占用,请核对身份证号！";
            }

            if (string.IsNullOrEmpty(Id))
            {
                return "出现未知错误，请联系管理员";
            }
            if (string.IsNullOrEmpty(stuId))
            {
                return "出现未知错误，请联系管理员";
            }
            if (string.IsNullOrEmpty(sEnrollTime))
            {
                return "报名时间不能为空";
            }
            string where1 = " and sEnrollsProfessionID=@sEnrollsProfessionID";
            SqlParameter[] paras1 = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",Id)
            };
            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(where1, paras1);
            if (!epm.Status.Equals("2"))
            {
                return "只有预报名学生才能转正式报名";
            }
            string where2 = " and StudentID=@StudentID";
            SqlParameter[] paras2 = new SqlParameter[] {
                new SqlParameter("@StudentID",stuId)
            };

            StudentModel sm = StudentBLL.StudentModelByWhere(where2, paras2);
            if (!string.IsNullOrEmpty(sm.IDCard))
            {
                if (idCardTurn != sm.IDCard)
                {
                    return "身份证号与此学生身份证号不匹配";
                }
            }

            string where3 = " and sEnrollID=@sEnrollID";
            SqlParameter[] paras3 = new SqlParameter[] {
                new SqlParameter("@sEnrollID",epm.sEnrollID)
            };
            sEnrollModel em = sEnrollBLL.sEnrollModelByWhere(where3, paras3);
            if (string.IsNullOrEmpty(em.sEnrollID))
            {
                return "数据异常，请联系管理员";
            }

            sm.IDCard = idCardTurn;
            sm.UpdateID = this.UserId.ToString();
            sm.UpdateTime = DateTime.Now.ToString();
            StudentBLL.UpdateStudent(sm);
            if (string.IsNullOrEmpty(em.EnrollNum))
            {
                em.EnrollNum = sEnrollBLL.getEnrollNum(epm.DeptID, epm.Year, UserId.ToString());
            }
            em.UpdateID = UserId.ToString();
            em.UpdateTime = DateTime.Now.ToString();
            sEnrollBLL.UpdatesEnroll(em);

            epm.Status = "3";
            epm.EnrollTime = sEnrollTime;
            epm.UpdateID = UserId.ToString();
            epm.UpdateTime = DateTime.Now.ToString();
            sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
            sEnrollsProfessionBLL.UpdatesEnrollsProfessionStatus(Id, UserId.ToString());
            sEnrollsProfessionChangeModel epcm = new sEnrollsProfessionChangeModel();
            epcm.Status = "1";
            epcm.sEnrollsProfessionID = Id;
            epcm.UpdateID = UserId.ToString();
            epcm.UpdateTime = DateTime.Now.ToString();
            epcm.CreateID = UserId.ToString();
            epcm.CreateTime = DateTime.Now.ToString();
            sEnrollsProfessionChangeBLL.InsertsEnrollsProfessionChange(epcm);

            return "yes";

        }
        /// <summary>
        /// 获取Excel数据 并验证模板
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AjaxResult GetExcelData(string ID)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = OtherHelper.ExcelToDataTable(ID);
            }
            catch (Exception ex)
            {

                return AjaxResult.Error(ex.Message);
            }
            try
            {
                if (!dt.Columns[0].ColumnName.Trim().Equals("考生号") || !dt.Columns[1].ColumnName.Trim().Equals("姓名") ||
                    !dt.Columns[2].ColumnName.Trim().Equals("身份证号") || !dt.Columns[3].ColumnName.Trim().Equals("性别") ||
                    !dt.Columns[4].ColumnName.Trim().Equals("手机号") || !dt.Columns[5].ColumnName.Trim().Equals("QQ") ||
                    !dt.Columns[6].ColumnName.Trim().Equals("微信") || !dt.Columns[7].ColumnName.Trim().Equals("地址") || !dt.Columns[8].ColumnName.Trim().Equals("民族") ||
                    !dt.Columns[9].ColumnName.Trim().Equals("生源地_省") || !dt.Columns[10].ColumnName.Trim().Equals("生源地_市") || !dt.Columns[11].ColumnName.Trim().Equals("生源地_县") ||
                    !dt.Columns[12].ColumnName.Trim().Equals("邮政编码") || !dt.Columns[13].ColumnName.Trim().Equals("毕业学校") ||
                    !dt.Columns[14].ColumnName.Trim().Equals("父母姓名") || !dt.Columns[15].ColumnName.Trim().Equals("父母电话") ||
                    !dt.Columns[16].ColumnName.Trim().Equals("学习层次") || !dt.Columns[17].ColumnName.Trim().Equals("录取专业") || !dt.Columns[18].ColumnName.Trim().Equals("招生部门"))
                {
                    return AjaxResult.Error("模板错误");
                }
            }
            catch (Exception)
            {

                return AjaxResult.Error("模板错误");
            }
            return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "success");
        }

        /// <summary>
        /// 导入报名
        /// </summary>
        /// <param name="uefm"></param>
        /// <returns></returns>
        public AjaxResult UploadsEnroll(UploadsEnrollFormModel uefm)
        {
            try
            {


                if (string.IsNullOrEmpty(uefm.upDeptId))
                {
                    return AjaxResult.Error("请选择报名学校");
                }
                if (string.IsNullOrEmpty(uefm.upSort))
                {
                    return AjaxResult.Error("请选择层次");
                }
                //if (string.IsNullOrEmpty(uefm.upEnrollDeptId))
                //{
                //    return AjaxResult.Error("请选择报名校区");
                //}
                if (string.IsNullOrEmpty(uefm.upYear))
                {
                    return AjaxResult.Error("请选择报名年份");
                }
                if (string.IsNullOrEmpty(uefm.upMonth))
                {
                    return AjaxResult.Error("请选择报名月份");
                }

                List<UploadsExcelEnrollModel> list = JsonConvert.DeserializeObject<List<UploadsExcelEnrollModel>>(uefm.Paras);
                if (list == null || list.Count == 0)
                {
                    return AjaxResult.Error("不能导入空数据");
                }
                DataTable dt = TableTitle(new DataTable());
                decimal successNum = 0;
                decimal errorNum = 0;
                foreach (var item in list)
                {
                    string studentId = string.Empty;
                    string sex = string.Empty;
                    string levelId = string.Empty;
                    string nationId = string.Empty;
                    string sprofessionId = string.Empty; ;
                    string provinceId = string.Empty;
                    string cityId = string.Empty;
                    string deptAreaId = string.Empty;
                    string senrollId = string.Empty;
                    string districtId = string.Empty;
                    UploadsEnrollModel uem = GetUploadsEnrollModel(item);
                    string errorString = string.Empty;
                    errorString = ValidateInfo(uefm, uem, ref studentId, ref nationId, ref provinceId, ref sex, ref cityId, ref districtId, ref levelId, ref sprofessionId, ref deptAreaId, ref senrollId);
                    if (string.IsNullOrEmpty(errorString))
                    {
                        uem.city = cityId;
                        uem.province = provinceId;
                        uem.enrollLevel = levelId;
                        uem.sex = sex;
                        uem.enrollProfession = sprofessionId;
                        uem.nation = nationId;
                        uem.district = districtId;
                        if (string.IsNullOrEmpty(studentId))
                        {
                            StudentModel sm = new StudentModel();
                            sm.Address = uem.address;
                            sm.CreateID = UserId.ToString();
                            sm.CreateTime = DateTime.Now.ToString();
                            sm.UpdateID = UserId.ToString();
                            sm.UpdateTime = DateTime.Now.ToString();
                            sm.WeChat = uem.weChat;
                            sm.DeptID = uefm.upDeptId;
                            sm.IDCard = uem.idCard;
                            sm.Mobile = uem.mobile;
                            sm.Name = uem.name;
                            sm.QQ = uem.qq;
                            sm.Remark = "";
                            sm.Sex = sex;
                            sm.Status = "1";
                            studentId = StudentBLL.InsertStudent(sm).ToString();//添加学生
                        }

                        UpdateStudentInfo(uem, studentId);//添加/修改信息扩展信息
                        if (!string.IsNullOrEmpty(uem.parentMobile) || !string.IsNullOrEmpty(uem.parentName))
                        {
                            UpdateStudentContact(uem, studentId);//添加/修改联系人信息;
                        }

                        if (string.IsNullOrEmpty(senrollId))
                        {
                            sEnrollModel em = new sEnrollModel();
                            em.CreateID = UserId.ToString();
                            em.CreateTime = DateTime.Now.ToString();
                            em.UpdateID = UserId.ToString();
                            em.UpdateTime = DateTime.Now.ToString();
                            em.ExamNum = uem.examNum;
                            em.DeptID = uefm.upDeptId;
                            em.EnrollNum = "";
                            em.Status = "1";
                            em.StudentID = studentId;
                            senrollId = sEnrollBLL.InsertsEnroll(em).ToString();//添加报名

                        }
                        else
                        {
                            sEnrollModel em = sEnrollBLL.GetsEnrollModel(senrollId);
                            em.UpdateID = UserId.ToString();
                            em.UpdateTime = DateTime.Now.ToString();
                            em.ExamNum = uem.examNum;
                            sEnrollBLL.UpdatesEnroll(em);
                        }
                        sEnrollsProfessionModel epm = new sEnrollsProfessionModel();
                        epm.CreateID = UserId.ToString();
                        epm.CreateTime = DateTime.Now.ToString();
                        epm.DeptAreaID = deptAreaId;
                        epm.DeptID = uefm.upDeptId;
                        epm.EnrollLevel = levelId;
                        epm.Month = uefm.upMonth;
                        epm.Year = uefm.upYear;
                        epm.Remark = "";
                        epm.Status = "1";
                        epm.EnrollTime = "1900-01-01";
                        epm.FirstFeeTime = "1900-01-01";
                        epm.BeforeEnrollTime = "1900-01-01";
                        epm.sProfessionID = uem.enrollProfession;
                        epm.sEnrollID = senrollId;
                        epm.UpdateID = UserId.ToString();
                        epm.UpdateTime = DateTime.Now.ToString();
                        epm.sEnrollsProfessionID = sEnrollsProfessionBLL.InsertsEnrollsProfession(epm).ToString();//添加报名专业
                        DataTable scheme = sItemsProfessionBLL.GetItemsProfession(uefm.upYear, uefm.upMonth, sprofessionId, uefm.upSort);
                        if (scheme.Rows.Count == 1)
                        {
                            sItemsEnrollModel iem = new sItemsEnrollModel();
                            iem.ItemID = scheme.Rows[0]["ItemID"].ToString();
                            iem.sEnrollsProfessionID = epm.sEnrollsProfessionID;
                            sItemsEnrollBLL.InsertsItemsEnroll(iem);

                            DataTable itemTable = ItemBLL.GetItemChildModel(iem.ItemID);
                            if (itemTable.Rows.Count > 0)
                            {
                                DateTime date = Convert.ToDateTime(itemTable.Rows[0]["LimitTime"].ToString());
                                string leaveYear = (date.Year + 1).ToString();
                                sEnrollsProfessionModel newepm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(epm.sEnrollsProfessionID);
                                newepm.LeaveYear = leaveYear;
                                newepm.UpdateID = UserId.ToString();
                                newepm.UpdateTime = DateTime.Now.ToString();
                                sEnrollsProfessionBLL.UpdatesEnrollsProfession(newepm);

                            }
                        }
                        successNum++;
                    }
                    else
                    {
                        dt = TableRow(dt, item, errorString);
                        errorNum++;
                    }

                }
                string url = "";
                if (dt.Rows.Count > 0)
                {
                    string FileName = OtherHelper.FilePathAndName();
                    OtherHelper.DeriveToExcel(dt, FileName);
                    url = "../Temp/" + FileName + "";
                }
                NoteModel nm = new NoteModel();
                nm.CreateID = this.UserId.ToString();
                nm.CreateTime = DateTime.Now.ToString();
                nm.InFile = uefm.filePath;
                nm.OutFile = url;
                nm.Sort = "8";
                nm.DeptID = uefm.upDeptId;
                nm.Status = "1";
                nm.SuccessNum = successNum.ToString();
                nm.ErrorNum = errorNum.ToString();
                NoteBLL.InsertNote(nm);
                return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "操作成功");
            }
            catch (Exception ex)
            {

                return AjaxResult.Error("系统错误:" + ex.Message + "");
            }
        }
        /// <summary>
        /// 生成datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable TableTitle(DataTable dt)
        {
            dt.Columns.Add("考生号", Type.GetType("System.String"));
            dt.Columns.Add("姓名", Type.GetType("System.String"));
            dt.Columns.Add("身份证号", Type.GetType("System.String"));
            dt.Columns.Add("性别", Type.GetType("System.String"));
            dt.Columns.Add("手机号", Type.GetType("System.String"));
            dt.Columns.Add("QQ", Type.GetType("System.String"));
            dt.Columns.Add("微信", Type.GetType("System.String"));
            dt.Columns.Add("地址", Type.GetType("System.String"));
            dt.Columns.Add("民族", Type.GetType("System.String"));
            dt.Columns.Add("生源地_省", Type.GetType("System.String"));
            dt.Columns.Add("生源地_市", Type.GetType("System.String"));
            dt.Columns.Add("生源地_县", Type.GetType("System.String"));
            dt.Columns.Add("邮政编码", Type.GetType("System.String"));
            dt.Columns.Add("毕业学校", Type.GetType("System.String"));
            dt.Columns.Add("父母姓名", Type.GetType("System.String"));
            dt.Columns.Add("父母电话", Type.GetType("System.String"));
            dt.Columns.Add("学习层次", Type.GetType("System.String"));
            dt.Columns.Add("录取专业", Type.GetType("System.String"));
            dt.Columns.Add("招生部门", Type.GetType("System.String"));
            dt.Columns.Add("系统备注", Type.GetType("System.String"));
            return dt;
        }
        /// <summary>
        /// 生成列
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="em"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public DataTable TableRow(DataTable dt, UploadsExcelEnrollModel em, string remark)
        {
            DataRow dr = dt.NewRow();
            dr["考生号"] = em.考生号;
            dr["姓名"] = em.姓名;
            dr["身份证号"] = em.身份证号;
            dr["性别"] = em.性别;
            dr["手机号"] = em.手机号;
            dr["QQ"] = em.QQ;
            dr["微信"] = em.微信;
            dr["地址"] = em.地址;
            dr["民族"] = em.民族;
            dr["生源地_省"] = em.生源地_省;
            dr["生源地_市"] = em.生源地_市;
            dr["生源地_县"] = em.生源地_县;
            dr["邮政编码"] = em.邮政编码;
            dr["毕业学校"] = em.毕业学校;
            dr["父母姓名"] = em.父母姓名;
            dr["父母电话"] = em.父母电话;
            dr["学习层次"] = em.学习层次;
            dr["录取专业"] = em.录取专业;
            dr["招生部门"] = em.招生部门;
            dr["系统备注"] = remark;
            dt.Rows.Add(dr);
            return dt;
        }
        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="uem"></param>
        /// <returns></returns>
        private UploadsEnrollModel GetUploadsEnrollModel(UploadsExcelEnrollModel uem)
        {
            UploadsEnrollModel m = new UploadsEnrollModel();
            m.address = uem.地址.Trim();
            m.city = uem.生源地_市.Replace(" ", "");
            m.enrollLevel = uem.学习层次.Replace(" ", "");
            m.enrollProfession = uem.录取专业.Replace(" ", "");
            m.examNum = uem.考生号.Replace(" ", "");
            m.idCard = uem.身份证号.Replace(" ", "");
            m.mobile = uem.手机号.Replace(" ", "");
            m.name = uem.姓名.Replace(" ", "");
            m.nation = uem.民族.Replace(" ", "");
            m.parentMobile = uem.父母电话.Replace(" ", "");
            m.parentName = uem.父母姓名.Replace(" ", "");
            m.province = uem.生源地_省.Replace(" ", "");
            m.district = uem.生源地_县.Replace(" ", "");
            m.qq = uem.QQ.Replace(" ", "");
            m.school = uem.毕业学校.Replace(" ", "");
            m.sex = uem.性别.Replace(" ", "");
            m.weChat = uem.微信.Replace(" ", "");
            m.zip = uem.邮政编码.Replace(" ", "");
            m.deptAreaId = uem.招生部门.Replace(" ", "");
            return m;

        }
        /// <summary>
        /// 验证导入数据
        /// </summary>
        /// <param name="uefm"></param>
        /// <param name="uem"></param>
        /// <param name="studentId"></param>
        /// <param name="nationId"></param>
        /// <param name="provinceId"></param>
        /// <param name="sex"></param>
        /// <param name="cityId"></param>
        /// <param name="levelId"></param>
        /// <param name="sprfessionId"></param>
        /// <returns></returns>
        private string ValidateInfo(UploadsEnrollFormModel uefm, UploadsEnrollModel uem, ref string studentId, ref string nationId, ref string provinceId, ref string sex, ref string cityId, ref string districtId, ref string levelId, ref string sprfessionId, ref string deptAreaId, ref string senrollId)
        {
            #region
            string errorString = "";
            if (string.IsNullOrEmpty(uem.name))
            {
                errorString += "姓名不能为空;";
            }
            else
            {
                if (uem.name.Length > 16)
                {
                    errorString += "姓名不能超过16个字符;";
                }
            }

            if (!OtherHelper.CheckIDCard(uem.idCard))
            {
                errorString += "身份证号不规范;";
            }
            else
            {
                StudentModel sm = StudentBLL.GetStudentModel(uem.idCard);
                if (!string.IsNullOrEmpty(sm.Name))
                {
                    if (!uem.name.Equals(sm.Name))
                    {
                        errorString += "姓名和身份证号不匹配;";
                    }
                    else
                    {

                        studentId = sm.StudentID;
                        DataTable enrollTable = sEnrollBLL.GetStudentEnroll(studentId);
                        if (enrollTable.Rows.Count > 1)
                        {
                            errorString += "该学生已经报过名了,不能重复报名;";
                        }
                        else if (enrollTable.Rows.Count == 1)
                        {
                            DataTable enrollProfessionTable = sEnrollsProfessionBLL.GetsEnrollProfessionTable(enrollTable.Rows[0]["sEnrollID"].ToString());
                            if (enrollProfessionTable.Rows.Count > 0)
                            {
                                errorString += "该学生已经报过名了,不能重复报名;";
                            }
                            else
                            {
                                senrollId = enrollTable.Rows[0]["sEnrollID"].ToString();
                            }
                        }

                    }
                }
            }
            if (!string.IsNullOrEmpty(uem.examNum))
            {
                if (uem.examNum.Length > 16)
                {
                    errorString += "考生号不能超过16个字符;";
                }

            }
            sex = RefeBLL.GetRefeValue(uem.sex, "3");
            if (sex.Equals("-1"))
            {
                errorString += "性别必须是男/女;";
            }

            if (string.IsNullOrEmpty(uem.mobile))
            {
                errorString += "手机号不能为空;";
            }
            else
            {
                if (uem.mobile.Length > 16)
                {
                    errorString += "手机号不能超过16个字符;";
                }
            }
            if (!string.IsNullOrEmpty(uem.nation))
            {
                nationId = RefeBLL.GetRefeValue(uem.nation, "24");
                if (nationId.Equals("-1"))
                {
                    errorString += "民族不存在;";
                }

            }
            if ((!string.IsNullOrEmpty(uem.city) || !string.IsNullOrEmpty(uem.district)) && string.IsNullOrEmpty(uem.province))
            {
                errorString += "省为空市、县区必须为空";
            }
            if ((string.IsNullOrEmpty(uem.city) && !string.IsNullOrEmpty(uem.district)))
            {
                errorString += "市为空、县区必须为空";
            }
            AreaModel province = AreaBLL.GetAreaModel(uem.province);
            AreaModel city = AreaBLL.GetAreaModel(uem.city);
            if (!string.IsNullOrEmpty(uem.city) && !string.IsNullOrEmpty(uem.province) && !string.IsNullOrEmpty(uem.district))
            {
                AreaModel district = AreaBLL.GetAreaModel(uem.district, city.AreaID);

                if (string.IsNullOrEmpty(province.AreaID))
                {
                    errorString += "生源地_省不存在;";
                }
                if (string.IsNullOrEmpty(city.AreaID))
                {
                    errorString += "生源地_市不存在;";
                }
                if (string.IsNullOrEmpty(district.AreaID))
                {
                    errorString += "生源地_县不存在;";

                }
                if (!string.IsNullOrEmpty(province.AreaID) && !string.IsNullOrEmpty(city.AreaID) && !string.IsNullOrEmpty(district.AreaID))
                {
                    if (!city.ParentID.Equals(province.AreaID))
                    {
                        errorString += "生源地_省和生源地_市没有从属关系;";
                    }
                    else
                    {
                        if (!district.ParentID.Equals(city.AreaID))
                        {
                            errorString += "生源地_市和生源地_县没有从属关系;";
                        }
                        else
                        {
                            cityId = city.AreaID;
                            provinceId = province.AreaID;
                            districtId = district.AreaID;
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(uem.city) && !string.IsNullOrEmpty(uem.province))
            {
                if (string.IsNullOrEmpty(province.AreaID))
                {
                    errorString += "生源地_省不存在;";
                }
                if (string.IsNullOrEmpty(city.AreaID))
                {
                    errorString += "生源地_市不存在;";
                }
                if (!string.IsNullOrEmpty(province.AreaID) && !string.IsNullOrEmpty(city.AreaID))
                {
                    if (!city.ParentID.Equals(province.AreaID))
                    {
                        errorString += "生源地_省和生源地_市没有从属关系;";
                    }
                    else
                    {
                        cityId = city.AreaID;
                        provinceId = province.AreaID;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(uem.province))
            {
                if (string.IsNullOrEmpty(province.AreaID))
                {
                    errorString += "生源地_省不存在;";
                }
                else
                {
                    provinceId = province.AreaID;
                }
            }

            if (!string.IsNullOrEmpty(uem.zip))
            {
                if (uem.zip.Length > 8)
                {
                    errorString += "邮政编码不能超过8个字符;";
                }
            }
            if (!string.IsNullOrEmpty(uem.school))
            {
                if (uem.school.Length > 64)
                {
                    errorString += "毕业学校不能超过64个字符;";
                }
            }
            if (!string.IsNullOrEmpty(uem.parentMobile))
            {
                if (uem.parentMobile.Length > 32)
                {
                    errorString += "父母电话不能超过64个字符;";
                }
            }
            if (!string.IsNullOrEmpty(uem.parentName))
            {
                if (uem.parentName.Length > 32)
                {
                    errorString += "父母姓名不能超过32个字符;";
                }
            }
            levelId = RefeBLL.GetRefeValue(uem.enrollLevel, "17");
            if (levelId.Equals("-1"))
            {
                errorString += "学习层次不存在";
            }
            sprfessionId = sProfessionBLL.GetsProfesssionID(uem.enrollProfession, uefm);
            if (string.IsNullOrEmpty(sprfessionId))
            {
                errorString += "录取专业不存在，请先在招生专业设置下录入该专业;";
            }

            deptAreaId = DeptAreaBLL.GetFirstDeptArea(uefm.upDeptId, uem.deptAreaId);
            if (string.IsNullOrEmpty(deptAreaId))
            {
                errorString += "该校区下没有此招生部门;";
            }

            if (!string.IsNullOrEmpty(uem.address))
            {
                if (uem.address.Length > 128)
                {
                    errorString += "地址不能超过128个字符;";
                }
            }
            if (!string.IsNullOrEmpty(uem.qq))
            {
                if (uem.qq.Length > 16)
                {
                    errorString += "QQ不能超过16个字符;";
                }
            }
            if (!string.IsNullOrEmpty(uem.weChat))
            {
                if (uem.weChat.Length > 32)
                {
                    errorString += "微信不能超过32个字符;";
                }
            }
            return errorString;
            #endregion
        }

        /// <summary>
        /// 新增学生扩展信息
        /// </summary>
        /// <param name="em"></param>
        private void InsertStudentInfo(UploadsEnrollModel em, string studentId)
        {
            StudentInfoModel sim = new StudentInfoModel();
            if (!string.IsNullOrEmpty(em.examNum))
            {
                sim.Photo = "../ELoad/Picture/" + DateTime.Now.Year.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd") + "/" + em.examNum + ".jpg";
            }
            sim.ProvinceID = em.province;
            sim.DistrictID = em.district;
            sim.CityID = em.city;
            sim.CreateID = this.UserId.ToString();
            sim.CreateTime = DateTime.Now.ToString();
            sim.Zip = em.zip;
            sim.Nation = em.nation;
            sim.School = em.school;
            sim.Status = "1";
            sim.StudentID = studentId;
            sim.UpdateID = UserId.ToString();
            sim.UpdateTime = DateTime.Now.ToString();
            sim.Zip = em.zip;
            StudentInfoBLL.InsertStudentInfo(sim);
        }
        /// <summary>
        /// 添加学生联系人信息
        /// </summary>
        /// <param name="em"></param>
        private void InsertStudentContact(UploadsEnrollModel em, string studentId)
        {
            StudentContactModel scm = new StudentContactModel();
            scm.CreateID = this.UserId.ToString();
            scm.CreateTime = DateTime.Now.ToString();
            scm.UpdateID = UserId.ToString();
            scm.UpdateTime = DateTime.Now.ToString();
            scm.Status = "1";
            scm.Name = em.parentName;
            scm.Tel = em.parentMobile;
            scm.StudentID = studentId;
            StudentContactBLL.InsertStudentContact(scm);
        }

        /// <summary>
        /// 修改学生扩展信息
        /// </summary>
        /// <param name="em"></param>
        private void UpdateStudentInfo(UploadsEnrollModel em, string studentId)
        {
            StudentInfoModel s = StudentInfoBLL.GetStudentInfoModel(studentId);

            if (!string.IsNullOrEmpty(s.StudentInfoID))
            {
                string where = " and StudentInfoID=@StudentInfoID";
                SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentInfoID",s.StudentInfoID)
            };
                StudentInfoModel sim = StudentInfoBLL.StudentInfoModelByWhere(where, paras);
                if (!string.IsNullOrEmpty(em.examNum))
                {
                    sim.Photo = "../ELoad/Picture/" + DateTime.Now.Year.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd") + "/" + em.examNum + ".jpg";
                }
                sim.ProvinceID = em.province;
                sim.DistrictID = em.district;
                sim.CityID = em.city;
                sim.Nation = em.nation;
                sim.School = em.school;
                sim.Zip = em.zip;
                sim.UpdateID = UserId.ToString();
                sim.UpdateTime = DateTime.Now.ToString();
                StudentInfoBLL.UpdateStudentInfo(sim);
            }
            else
            {

                InsertStudentInfo(em, studentId);
            }
        }
        /// <summary>
        /// 修改学生联系人信息
        /// </summary>
        /// <param name="em"></param>
        private void UpdateStudentContact(UploadsEnrollModel em, string studentId)
        {
            StudentContactModel s = StudentContactBLL.GetStudentContactModel(studentId);
            if (!string.IsNullOrEmpty(s.StudentContactID))
            {
                string where = " and StudentContactID=@StudentContactID";
                SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentContactID",s.StudentContactID)
                    };
                StudentContactModel scm = StudentContactBLL.StudentContactModelByWhere(where, paras);
                scm.Name = em.parentName;
                scm.Tel = em.parentMobile;
                scm.UpdateID = UserId.ToString();
                scm.UpdateTime = DateTime.Now.ToString();
                StudentContactBLL.UpdateStudentContact(scm);
            }
            else
            {
                InsertStudentContact(em, studentId);
            }
        }

        /// <summary>
        /// 保存审查
        /// </summary>
        /// <param name="eafm"></param>
        /// <returns></returns>
        public string GetsEnrollAudit(sEnrollAuditFormModel eafm)
        {
            try
            {
                if (string.IsNullOrEmpty(eafm.audId))
                {
                    return "出现未知错误，请联系管理员";
                }

                if (string.IsNullOrEmpty(eafm.audsEnrollTime))
                {
                    return "请选择报名时间";
                }
                if (string.IsNullOrEmpty(eafm.audExplain))
                {
                    return "请选填写审查意见";
                }
                string where = " and sEnrollsProfessionID=@sEnrollsProfessionID";
                SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",eafm.audId)
            };
                sEnrollsProfessionModel epm = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(where, paras);

                if (!epm.Status.Equals("1"))
                {
                    return "该报名信息不符合审核条件!";
                }


                string where1 = " and sEnrollID=@sEnrollID ";
                SqlParameter[] paras1 = new SqlParameter[] {
                new SqlParameter("@sEnrollID",epm.sEnrollID)
            };
                sEnrollModel em = sEnrollBLL.sEnrollModelByWhere(where1, paras1);


                epm.Auditor = UserId.ToString();
                epm.Status = "3";
                epm.AuditTime = DateTime.Now.ToString();
                epm.EnrollTime = eafm.audsEnrollTime;
                epm.AuditView = eafm.audExplain;
                epm.UpdateID = UserId.ToString();
                epm.UpdateTime = DateTime.Now.ToString();
                sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);

                if (string.IsNullOrEmpty(em.EnrollNum))
                {
                    em.EnrollNum = sEnrollBLL.getEnrollNum(epm.DeptID, epm.Year, UserId.ToString());
                }
                em.UpdateID = this.UserId.ToString();
                em.UpdateTime = DateTime.Now.ToString();
                sEnrollBLL.UpdatesEnroll(em);
                sEnrollsProfessionBLL.UpdatesEnrollsProfessionStatus(eafm.audId, UserId.ToString());//修改报名状态
                return "yes";

            }
            catch (Exception)
            {
                return "出现未知错误，请联系管理员";
            }


        }
        /// <summary>
        /// 更新续报名学号（如果没有学号）
        /// </summary>
        /// <param name="enrollId"></param>
        /// <param name="year"></param>
        /// <param name="deptId"></param>
        private void UpdateEnrollNum(string enrollId, string year, string deptId)
        {
            string where = " and sEnrollID=@sEnrollID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollID",enrollId)
            };
            sEnrollModel em = sEnrollBLL.sEnrollModelByWhere(where, paras);
            em.EnrollNum = sEnrollBLL.getEnrollNum(deptId, year, UserId.ToString());
            em.UpdateID = UserId.ToString();
            em.UpdateTime = DateTime.Now.ToString();
            sEnrollBLL.UpdatesEnroll(em);

        }
        /// <summary>
        /// 获取方案名称和报名id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AjaxResult GetPlanName(string ID)
        {
            return AjaxResult.Success(JsonHelper.DataTableToJson(sEnrollsProfessionBLL.GetPlanName(ID)), "");
        }
        /// <summary>
        /// 变更报名信息在校状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public AjaxResult ModifyStatus(string Id, string status, string explain)
        {

            if (string.IsNullOrEmpty(status))
            {
                return AjaxResult.Error("请选择要变更的状态");
            }
            if (!status.Equals("4") && !status.Equals("5") && !status.Equals("6") && !status.Equals("7") && !status.Equals("8"))
            {
                return AjaxResult.Error("不能改为此状态");
            }
            if (string.IsNullOrEmpty(explain))
            {
                return AjaxResult.Error("请填写变更状态原因");
            }
            if (string.IsNullOrEmpty(Id))
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(Id);

            if (!epm.Status.Equals("4") && !epm.Status.Equals("5") && !epm.Status.Equals("6") && !epm.Status.Equals("7") && !epm.Status.Equals("8"))
            {
                return AjaxResult.Error("该报名信息状态不能修改");
            }
            if (epm.Status.Equals(status))
            {
                return AjaxResult.Error("变更状态与当前状态相同！");
            }
            epm.Status = status;
            epm.UpdateID = UserId.ToString();
            epm.UpdateTime = DateTime.Now.ToString();
            sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
            sEnrollsProfessionStatusModel epsm = new sEnrollsProfessionStatusModel();
            epsm.Status = "1";
            epsm.sEnrollsProfessionID = Id;
            epsm.Explain = explain;
            epsm.StatusValue = status;
            epsm.UpdateID = UserId.ToString();
            epsm.UpdateTime = DateTime.Now.ToString();
            epsm.CreateID = UserId.ToString();
            epsm.CreateTime = DateTime.Now.ToString();
            sEnrollsProfessionStatusBLL.InsertsEnrollsProfessionStatus(epsm);

            return AjaxResult.Success("", "变更成功");
        }
        /// <summary>
        /// 修改班级
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public AjaxResult ModifyClass(string Id, string classId)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
            if (string.IsNullOrEmpty(classId))
            {
                return AjaxResult.Error("请选择班级");
            }
            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(Id);
            epm.ClassID = classId;
            epm.UpdateID = UserId.ToString();
            epm.UpdateTime = DateTime.Now.ToString();
            sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
            return AjaxResult.Success("", "变更成功");
        }

        public AjaxResult ModifyLeaveYear(string Id, string leaveYear)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
            if (string.IsNullOrEmpty(leaveYear))
            {
                return AjaxResult.Error("请选择离校年度");
            }
            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(Id);
            epm.LeaveYear = leaveYear;
            epm.UpdateID = UserId.ToString();
            epm.UpdateTime = DateTime.Now.ToString();
            sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
            return AjaxResult.Success("", "修改成功");
        }
        /// <summary>
        /// 转专业选已读年分
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <returns></returns>
        public string GetChangeMajorYearCombobox(string sEnrollsProfessionID)
        {
            return JsonHelper.DataTableToJson(sEnrollBLL.ChangeMajorYearCombobox(sEnrollsProfessionID));

        }


        public ActionResult GetChangeStatusList(string MenuID)
        {
            string where = "";
            string status = Request.Form["Status"];
            string timeS = Request.Form["timeS"];
            string timeE = Request.Form["timeE"];
            string name = Request.Form["name"];
            string idCard = Request.Form["idCard"];
            string deptId = Request.Form["deptId"];
            string userName = Request.Form["userName"];
            if (!string.IsNullOrEmpty(status))
            {
                where += " AND epc.Status IN(" + status + ")";

            }
            if (!string.IsNullOrEmpty(userName))
            {
                where += " AND u.Name like '%" + userName + "%'";
            }
            if (!string.IsNullOrEmpty(timeS))
            {
                where += " AND convert(NVARCHAR(10),epc.CreateTime,23) >= '" + timeS + "'";
            }
            if (!string.IsNullOrEmpty(timeE))
            {
                where += " AND convert(NVARCHAR(10),epc.CreateTime,23) <= '" + timeE + "'";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " AND s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                where += " AND ep.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            string cmdText = @"SELECT 
        d.Name DeptName ,
	    s.Name StudName ,
        s.IDCard ,
        p.Name Major ,
        CONVERT(NVARCHAR(10), ep.BeforeEnrollTime, 23) BeforeEnrollTime ,
        e.EnrollNum ,
        u.Name UserName ,
        epc.CreateTime,
         epc.Status
FROM    T_Stu_sEnrollsProfessionChange epc
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = epc.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Sys_User u ON u.UserID = epc.CreateID
        LEFT JOIN T_Stu_sProfession sp ON sp.sProfessionID = ep.sProfessionID
        LEFT JOIN T_Pro_Profession p ON p.ProfessionID = sp.ProfessionID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = ep.DeptID  
        Where 1=1 {0}  ";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "d.DeptID", "epc.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        public AjaxResult DownChangeStatusList(string MenuID)
        {
            string where = "";
            string timeS = Request.Form["timeS"];
            string timeE = Request.Form["timeE"];
            string name = Request.Form["name"];
            string idCard = Request.Form["idCard"];
            string deptId = Request.Form["deptId"];
            string userName = Request.Form["userName"];
            if (!string.IsNullOrEmpty(userName))
            {
                where += " AND u.Name like '%" + userName + "%'";
            }
            if (!string.IsNullOrEmpty(timeS))
            {
                where += " AND convert(NVARCHAR(10),epc.CreateTime,23) >= '" + timeS + "'";
            }
            if (!string.IsNullOrEmpty(timeE))
            {
                where += " AND convert(NVARCHAR(10),epc.CreateTime,23) <= '" + timeE + "'";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " AND s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                where += " AND ep.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            string cmdText = @"SELECT 
        d.Name 校区 ,
	    s.Name 姓名 ,
        s.IDCard 身份证号,
        p.Name 专业 ,
        e.EnrollNum 学号,
        CONVERT(NVARCHAR(10), ep.EnrollTime, 23) 报名时间 ,
        u.Name 操作人 ,
        epc.CreateTime 转正报时间
FROM    T_Stu_sEnrollsProfessionChange epc
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = epc.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Sys_User u ON u.UserID = epc.CreateID
        LEFT JOIN T_Stu_sProfession sp ON sp.sProfessionID = ep.sProfessionID
        LEFT JOIN T_Pro_Profession p ON p.ProfessionID = sp.ProfessionID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = ep.DeptID  
        Where 1=1 {0}  ";
            string filename = "转正报记录信息.xls";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "ep.DeptID", "epc.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }

        public AjaxResult SaveChangeTrun(string Id, string Type)
        {
            if (string.IsNullOrEmpty(Type))
            {
                return AjaxResult.Error("请选择类型");
            }
            if (string.IsNullOrEmpty(Id))
            {
                return AjaxResult.Error("请选择报名信息");
            }
            try
            {
                sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(Id);
                sEnrollModel em = sEnrollBLL.GetsEnrollModel(epm.sEnrollID);
                if (string.IsNullOrEmpty(em.EnrollNum))
                {
                    em.EnrollNum = sEnrollBLL.getEnrollNum(epm.DeptID, epm.Year, UserId.ToString());
                    em.UpdateID = UserId.ToString();
                    em.UpdateTime = DateTime.Now.ToString();
                    sEnrollBLL.UpdatesEnroll(em);
                }
                if (Type.Equals("1"))
                {
                    epm.Status = "3";
                    epm.EnrollTime = epm.BeforeEnrollTime;
                    epm.BeforeEnrollTime = "1900-01-01";
                    sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);

                    //判断是否交费
                    sEnrollsProfessionBLL.UpdatesEnrollsProfessionStatus(Id, UserId.ToString());
                }
                else if (Type.Equals("2"))
                {
                    epm.Status = "2";
                    epm.BeforeEnrollTime = epm.EnrollTime;
                    epm.EnrollTime = "1900-01-01";
                    sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                }
                else if (Type.Equals("3"))
                {
                    epm.Status = "2";
                    epm.EnrollTime = "1900-01-01";
                    sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                    sEnrollsProfessionChangeModel epcm = sEnrollsProfessionChangeBLL.SelectsEnrollsProfessionChangeByWhere(" and  sEnrollsProfessionID=" + epm.sEnrollsProfessionID + " and Status=1", null, "").FirstOrDefault();
                    if (!string.IsNullOrEmpty(epcm.sEnrollsProfessionChangeID))
                    {
                        sEnrollsProfessionChangeBLL.UpdatesEnrollsProfessionChangeStatus(epcm.sEnrollsProfessionChangeID, "2", UserId);
                    }
                }
                return AjaxResult.Success("操作成功！");
            }
            catch (Exception ex)
            {
                return AjaxResult.Error(ex.Message);
            }
        }

        public AjaxResult UpdateStatus(string ID, string Remark)
        {
            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(ID);
            if (!epm.Status.Equals("2"))
            {
                return AjaxResult.Error("只有预报名的学生才能进行此项操作！");
            }
            if (string.IsNullOrEmpty(Remark))
            {
                return AjaxResult.Error("请填写预报退费原因！");
            }
            epm.Status = "10";
            epm.UpdateID = UserId.ToString();
            epm.UpdateTime = DateTime.Now.ToString();
            sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
            sEnrollsProfessionStatusModel epsm = new sEnrollsProfessionStatusModel();
            epsm.Status = "1";
            epsm.sEnrollsProfessionID = ID;
            epsm.Explain = Remark;
            epsm.StatusValue = "10";
            epsm.UpdateID = UserId.ToString();
            epsm.UpdateTime = DateTime.Now.ToString();
            epsm.CreateID = UserId.ToString();
            epsm.CreateTime = DateTime.Now.ToString();
            sEnrollsProfessionStatusBLL.InsertsEnrollsProfessionStatus(epsm);
            return AjaxResult.Success("操作成功！");
        }
        /// <summary>
        /// 验证导入报名校区模板
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AjaxResult GetUploadEnrollDeptExcel(string ID)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = OtherHelper.ExcelToDataTable(ID);
            }
            catch (Exception)
            {

                return AjaxResult.Error("模板错误");
            }
            try
            {
                if (!dt.Columns[0].ColumnName.Trim().Equals("姓名") || !dt.Columns[1].ColumnName.Trim().Equals("身份证号")
                    || !dt.Columns[2].ColumnName.Trim().Equals("学历层次") || !dt.Columns[3].ColumnName.Trim().Equals("专业")
                    || !dt.Columns[4].ColumnName.Trim().Equals("校区"))
                {
                    return AjaxResult.Error("模板错误");
                }
            }
            catch (Exception)
            {

                return AjaxResult.Error("模板错误");
            }
            return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "success");
        }

        public AjaxResult GetUploadEnrollDept()
        {
            string deptId = Request.Form["enrollDeptId"];
            string filePath = Request.Form["enrollDeptFilePath"];
            string Paras = Request.Form["Paras"];
            if (string.IsNullOrEmpty(deptId))
            {
                return AjaxResult.Error("校区不能为空");
            }
            if (string.IsNullOrEmpty(filePath))
            {
                return AjaxResult.Error("上传文件不能为空");
            }
            List<EnrollDeptModelCH> list = JsonConvert.DeserializeObject<List<EnrollDeptModelCH>>(Paras);
            if (list == null || list.Count == 0)
            {
                return AjaxResult.Error("不能导入空数据");
            }
            DataTable dt = TableTitle(new DataTable());
            decimal successNum = 0;
            decimal errorNum = 0;
            string senrollsProfession = string.Empty;
            List<EnrollDeptModelCH> edm = new List<EnrollDeptModelCH>();
            foreach (var item in list)
            {
                EnrollDeptModel uem = ReturnEnrollDept(item);
                string deptAreaId = string.Empty;
                string errorString = Validate(uem, deptId, ref senrollsProfession, ref deptAreaId);
                if (string.IsNullOrEmpty(errorString) && !string.IsNullOrEmpty(senrollsProfession) && !string.IsNullOrEmpty(deptAreaId))
                {
                    sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(senrollsProfession);
                    epm.DeptAreaID = deptAreaId;
                    epm.UpdateID = UserId.ToString();
                    epm.UpdateTime = DateTime.Now.ToString();
                    sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                    successNum++;
                }
                else
                {
                    item.系统备注 = errorString;
                    edm.Add(item);
                    errorNum++;
                }
            }

            string url = "";
            string errorData = string.Empty;
            if (edm.Count > 0)
            {
                string FileName = OtherHelper.FilePathAndName();
                OtherHelper.DeriveToExcel(edm, FileName);
                url = "../Temp/" + FileName + "";
                errorData = OtherHelper.JsonSerializer(edm);
            }
            NoteModel nm = new NoteModel();
            nm.CreateID = this.UserId.ToString();
            nm.CreateTime = DateTime.Now.ToString();
            nm.InFile = filePath;
            nm.OutFile = url;
            nm.Sort = "14";
            nm.DeptID = deptId;
            nm.Status = "1";
            nm.SuccessNum = successNum.ToString();
            nm.ErrorNum = errorNum.ToString();
            NoteBLL.InsertNote(nm);
            return AjaxResult.Success(errorData, "操作成功");
        }

        public EnrollDeptModel ReturnEnrollDept(EnrollDeptModelCH em)
        {
            EnrollDeptModel cpm = new EnrollDeptModel();
            cpm.idCard = em.身份证号.Replace(" ", "");
            cpm.name = em.姓名.Replace(" ", "");
            cpm.level = em.学历层次.Replace(" ", "");
            cpm.major = em.专业.Replace(" ", "");
            cpm.deptId = em.校区.Replace(" ", "");
            return cpm;

        }

        /// <summary>
        /// 验证字段
        /// </summary>
        /// <param name="uem"></param>
        /// <returns></returns>
        public string Validate(EnrollDeptModel uem, string deptId, ref string senrollsProfession, ref string deptAreaId)
        {
            string errorString = string.Empty;
            string studentId = string.Empty;
            string sprofessionId = string.Empty;
            if (string.IsNullOrEmpty(uem.name))
            {
                errorString += "姓名不能为空;";
            }
            else
            {
                if (uem.name.Length > 16)
                {
                    errorString += "姓名不能超过16个字符;";
                }
            }

            if (!OtherHelper.CheckIDCard(uem.idCard))
            {
                errorString += "身份证号不规范;";
            }
            else
            {
                StudentModel sm = StudentBLL.GetStudentModel(uem.idCard);
                if (!string.IsNullOrEmpty(sm.Name))
                {
                    if (!uem.name.Equals(sm.Name))
                    {
                        errorString += "姓名和身份证号不匹配;";
                    }
                }
                studentId = sm.StudentID;

            }
            deptAreaId = DeptAreaBLL.GetFirstDeptArea(deptId, uem.deptId);
            if (string.IsNullOrEmpty(deptAreaId))
            {
                errorString += "报名校区不存在;";
            }

            string studyLevel = RefeBLL.GetRefeValue(uem.level, "17");
            if (studyLevel.Equals("-1"))
            {
                errorString += "学习层次不存在;";
            }

            if (string.IsNullOrEmpty(studentId))
            {
                errorString += "此学生不存在;";
            }

            if (!string.IsNullOrEmpty(studentId) && string.IsNullOrEmpty(errorString))
            {
                DataTable enrollTab = sEnrollBLL.GetsEnrollTable(studentId);
                if (enrollTab.Rows.Count > 1)
                {
                    errorString += "此学生已经多次报名此专业，请手动修改报名校区;";
                }
                else if (enrollTab.Rows.Count == 0)
                {
                    errorString += "此学生未报名;";
                }
                else
                {

                    if (string.IsNullOrEmpty(errorString))
                    {
                        DataTable senrollPro = sEnrollsProfessionBLL.GetsEnrollsProfessionTable(studyLevel, enrollTab.Rows[0]["sEnrollID"].ToString(), uem.major, deptId);
                        if (senrollPro.Rows.Count > 1)
                        {
                            errorString += "此学生已经多次报名此专业，请手动修改报名校区;";
                        }
                        else if (senrollPro.Rows.Count == 0)
                        {
                            errorString += "此学生没有报过任何专业;";
                        }
                        else
                        {
                            if (!deptAreaId.Equals(senrollPro.Rows[0]["DeptAreaID"].ToString()))
                            {
                                senrollsProfession = senrollPro.Rows[0]["sEnrollsProfessionID"].ToString();
                            }
                            else
                            {
                                errorString += "报名校区和当前报名校区相同;";
                            }
                        }
                    }

                }
            }

            return errorString;
        }
        public AjaxResult GetUploadStudyNumExcel(string ID)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = OtherHelper.ExcelToDataTable(ID);
            }
            catch (Exception)
            {

                return AjaxResult.Error("模板错误");
            }
            try
            {
                if (!dt.Columns[0].ColumnName.Trim().Equals("姓名") || !dt.Columns[1].ColumnName.Trim().Equals("身份证号")
                    || !dt.Columns[2].ColumnName.Trim().Equals("学号"))
                {
                    return AjaxResult.Error("模板错误");
                }
            }
            catch (Exception)
            {

                return AjaxResult.Error("模板错误");
            }
            return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "success");
        }

        public StudyModel ReturnStudyNumModel(StudyModelCH em)
        {
            StudyModel cpm = new StudyModel();
            cpm.idCard = em.身份证号.Replace(" ", "");
            cpm.name = em.姓名.Replace(" ", "");
            cpm.studyNum = em.学号.Replace(" ", "");
            return cpm;

        }


        public AjaxResult GetUploadStudyNum()
        {
            string deptId = Request.Form["studyNumDeptId"];
            string filePath = Request.Form["studyNumFilePath"];
            string Paras = Request.Form["Paras"];
            if (string.IsNullOrEmpty(deptId))
            {
                return AjaxResult.Error("校区不能为空");
            }
            if (string.IsNullOrEmpty(filePath))
            {
                return AjaxResult.Error("上传文件不能为空");
            }
            List<StudyModelCH> list = JsonConvert.DeserializeObject<List<StudyModelCH>>(Paras);
            if (list == null || list.Count == 0)
            {
                return AjaxResult.Error("不能导入空数据");
            }
            DataTable dt = TableTitle(new DataTable());
            decimal successNum = 0;
            decimal errorNum = 0;
            string senrollsProfession = string.Empty;
            List<StudyModelCH> edm = new List<StudyModelCH>();
            foreach (var item in list)
            {
                StudyModel uem = ReturnStudyNumModel(item);
                string enrollId = string.Empty;
                string errorString = ValidatStudyNumUpload(uem, ref enrollId);
                if (string.IsNullOrEmpty(errorString) && !string.IsNullOrEmpty(enrollId))
                {
                    sEnrollModel em = sEnrollBLL.GetEnrollModel(enrollId);
                    em.StudyNum = uem.studyNum;
                    sEnrollBLL.UpdatesEnroll(em);
                    successNum++;
                }
                else
                {
                    item.系统备注 = errorString;
                    edm.Add(item);
                    errorNum++;
                }
            }

            string url = "";
            string errorData = string.Empty;
            if (edm.Count > 0)
            {
                string FileName = OtherHelper.FilePathAndName();
                OtherHelper.DeriveToExcel(edm, FileName);
                url = "../Temp/" + FileName + "";
                errorData = OtherHelper.JsonSerializer(edm);
            }
            NoteModel nm = new NoteModel();
            nm.CreateID = this.UserId.ToString();
            nm.CreateTime = DateTime.Now.ToString();
            nm.InFile = filePath;
            nm.OutFile = url;
            nm.Sort = "15";
            nm.DeptID = deptId;
            nm.Status = "1";
            nm.SuccessNum = successNum.ToString();
            nm.ErrorNum = errorNum.ToString();
            NoteBLL.InsertNote(nm);
            return AjaxResult.Success(errorData, "操作成功");
        }
        string ValidatStudyNumUpload(StudyModel uem, ref string enrollId)
        {
            string errorString = string.Empty;
            string studentId = string.Empty;
            string sprofessionId = string.Empty;
            if (string.IsNullOrEmpty(uem.name))
            {
                errorString += "姓名不能为空;";
            }
            else
            {
                if (uem.name.Length > 16)
                {
                    errorString += "姓名不能超过16个字符;";
                }
            }

            if (!OtherHelper.CheckIDCard(uem.idCard))
            {
                errorString += "身份证号不规范;";
            }
            else
            {
                StudentModel sm = StudentBLL.GetStudentModel(uem.idCard);
                if (!string.IsNullOrEmpty(sm.Name))
                {
                    if (!uem.name.Equals(sm.Name))
                    {
                        errorString += "姓名和身份证号不匹配;";
                    }
                }
                studentId = sm.StudentID;

            }
            if (string.IsNullOrEmpty(uem.studyNum))
            {
                errorString += "学号不能为空;";
            }
            else
            {
                if (uem.studyNum.Length > 16)
                {
                    errorString += "学号不能超过16个字符;";
                }
            }
            if (!string.IsNullOrEmpty(studentId) && string.IsNullOrEmpty(errorString))
            {
                DataTable enrollTab = sEnrollBLL.GetsEnrollTable(studentId);
                if (enrollTab.Rows.Count != 1)
                {
                    errorString += "报名信息不存在";
                }
                else
                {
                    enrollId = enrollTab.Rows[0]["sEnrollID"].ToString();
                }
            }

            return errorString;
        }
        /// <summary>
        /// 验证同费用充抵是否符合条件
        /// </summary>
        /// <param name="NumItemID"></param>
        /// <param name="sEnrollsProfessionID"></param>
        /// <param name="PlanItemID"></param>
        /// <returns></returns>
        public AjaxResult ValidateIsSame(string NumItemID, string sEnrollsProfessionID, string PlanItemID)
        {
            sEnrollsProfessionNoteModel epnm = sEnrollsProfessionNoteBLL.sEnrollsProfessionNoteModelByWhere(sEnrollsProfessionID);
            if (string.IsNullOrEmpty(epnm.sEnrollsProfessionNoteID))
            {
                return AjaxResult.Error("该学生未转专业,不能使用此充抵！");
            }
            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(sEnrollsProfessionID);
            sOrderModel om = sOrderBLL.GetsOrderModelByWhere(sEnrollsProfessionID, NumItemID, PlanItemID);

            List<sOrderModel> newOrderList = sOrderBLL.GetOrderList(sEnrollsProfessionID, PlanItemID, NumItemID);
            List<sOrderModel> oldOrderList = sOrderBLL.GetOrderList(epnm.sEnrollsProfessionID, PlanItemID, NumItemID);
            bool flag = false;

            foreach (var item in newOrderList)
            {
                if (oldOrderList.Count > 0)
                {
                    var model = oldOrderList.Where(O => O.DetailID.Equals(item.DetailID)&& O.ShouldMoney.Equals(item.ShouldMoney)).ToList();
                    if (!model.Count.Equals(1))
                    {
                        flag = true;
                        break;
                    }
                }
                else
                {
                    flag = true;
                }
            }
            if (flag)
            {
                return AjaxResult.Error("充抵不符合条件");
            }
            string sOrderId = string.Empty;
            foreach (var item in oldOrderList)
            {
                sOrderId += item.sOrderID+",";
            }
            sOrderId = sOrderId.Substring(0, sOrderId.Length - 1);

            string json = JsonHelper.DataTableToJson(sFeesOrderBLL.GetFeeOrderTable(sOrderId));

            return AjaxResult.Success(json, "success");
        }


    }

}
