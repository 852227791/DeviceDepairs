using BLL;
using Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DAL;

namespace WebUI.Controllers
{
    public class sOrderGiveController : BaseController
    {
        #region 页面
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sOrderGiveList()
        {
            return View();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 修改状态（9：停用）
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetUpdatesStatus()
        {
            string sOrderGiveIDs = Request.Form["IDStr"];
            string status = Request.Form["Value"];
            int result = sOrderGiveBLL.UpdateStatusBysOrderGiveIDs(sOrderGiveIDs, status);
            if (result > 0)
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public AjaxResult DownloadsOrderGive()
        {
            #region 查询条件
            string menuId = Request.Form["MenuID"];
            string dept = Request.Form["treeDept"];
            string stuname = Request.Form["txtStuName"];
            string enrollnum = Request.Form["txtEnrollNum"];
            string idcard = Request.Form["txtIDCard"];
            string signTimeS = Request.Form["txtSignTimeS"];
            string signTimeE = Request.Form["txtSignTimeE"];
            string sellevel = Request.Form["selLevel"];
            string major = Request.Form["txtMajor"];
            string year = Request.Form["txtYear"];
            string month = Request.Form["txtMonth"];
            string planname = Request.Form["txtPlanName"];
            string givename = Request.Form["txtGiveName"];
            string selstatus = Request.Form["selStatus"];
            string where = "";
            if (!string.IsNullOrEmpty(dept))
            {
                where += " AND sordergive.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(stuname))
            {
                where += " AND student.Name like '%" + stuname + "%'";
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
            if (!string.IsNullOrEmpty(sellevel))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sellevel, "senrollpro.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(major))
            {
                where += " AND pro.Name like '%" + major + "%'";
            }
            if (!string.IsNullOrEmpty(year))
            {
                where += " AND sordergive.Year like '%" + year + "%'";
            }
            if (!string.IsNullOrEmpty(month))
            {
                where += " AND refe_month.RefeName like '%" + month + "%'";
            }
            if (!string.IsNullOrEmpty(planname))
            {
                where += " AND item.Name like '%" + planname + "%'";
            }
            if (!string.IsNullOrEmpty(givename))
            {
                where += " AND sgive.Name like '%" + givename + "%'";
            }
            if (!string.IsNullOrEmpty(selstatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selstatus, "sordergive.Status");
            }
            string cmdText = @"SELECT  refe_ordergivestatus.RefeName AS '状态' ,
        dept.Name AS '校区' ,
        student.Name AS '学生姓名' ,
        senroll.EnrollNum AS '学号' ,
        student.IDCard AS '身份证号' ,
        sordergive.Year AS '年份' ,
        refe_month.RefeName AS '月份' ,
        refe_level.RefeName AS '学习层次' ,
        pro.Name AS '专业' ,
        CONVERT(NVARCHAR(23), senrollpro.EnrollTime, 23) AS '报名时间' ,
        item.Name AS '缴费方案名称' ,
        sgive.Name AS '配品名称' ,
        CONVERT(NVARCHAR(23), sordergive.CreateTime, 23) AS '创建时间'
FROM    T_Stu_sOrderGive AS sordergive
        LEFT JOIN T_Sys_Dept AS dept ON sordergive.DeptID = dept.DeptID
        LEFT JOIN T_Stu_sEnrollsProfession AS senrollpro ON sordergive.sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll AS senroll ON senrollpro.sEnrollID = senroll.sEnrollID
        LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
        LEFT JOIN T_Sys_Refe AS refe_month ON sordergive.Month = refe_month.Value
                                              AND refe_month.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                              AND refe_level.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe AS refe_ordergivestatus ON sordergive.Status = refe_ordergivestatus.Value
                                                        AND refe_ordergivestatus.RefeTypeID = 20
        LEFT JOIN T_Stu_sProfession AS spro ON senrollpro.sProfessionID = spro.sProfessionID
        LEFT JOIN T_Pro_Profession AS pro ON spro.ProfessionID = pro.ProfessionID
        LEFT JOIN T_Pro_Item AS item ON sordergive.PlanItemID = item.ItemID
        LEFT JOIN T_Stu_sGive AS sgive ON sgive.sGiveID = sordergive.sGiveID WHERE 1 = 1 {0} ORDER BY sordergive.sOrderGiveID DESC";
            #endregion
            string filename = "配品信息.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "sordergive.DeptID", "sordergive.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }

        /// <summary>
        /// 得到缴费单列表信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetsOrderGiveInfoList()
        {
            #region 查询条件
            string menuId = Request.Form["MenuID"];
            string dept = Request.Form["treeDept"];
            string stuname = Request.Form["txtStuName"];
            string enrollnum = Request.Form["txtEnrollNum"];
            string idcard = Request.Form["txtIDCard"];
            string signTimeS = Request.Form["txtSignTimeS"];
            string signTimeE = Request.Form["txtSignTimeE"];
            string sellevel = Request.Form["selLevel"];
            string major = Request.Form["txtMajor"];
            string year = Request.Form["txtYear"];
            string month = Request.Form["txtMonth"];
            string planname = Request.Form["txtPlanName"];
            string givename = Request.Form["txtGiveName"];
            string selstatus = Request.Form["selStatus"];
            string where = "";
            if (!string.IsNullOrEmpty(dept))
            {
                where += " AND sordergive.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(stuname))
            {
                where += " AND student.Name like '%" + stuname + "%'";
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
            if (!string.IsNullOrEmpty(sellevel))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sellevel, "senrollpro.EnrollLevel");
            }
            if (!string.IsNullOrEmpty(major))
            {
                where += " AND pro.Name like '%" + major + "%'";
            }
            if (!string.IsNullOrEmpty(year))
            {
                where += " AND sordergive.Year like '%" + year + "%'";
            }
            if (!string.IsNullOrEmpty(month))
            {
                where += " AND refe_month.RefeName like '%" + month + "%'";
            }
            if (!string.IsNullOrEmpty(planname))
            {
                where += " AND item.Name like '%" + planname + "%'";
            }
            if (!string.IsNullOrEmpty(givename))
            {
                where += " AND sgive.Name like '%" + givename + "%'";
            }
            if (!string.IsNullOrEmpty(selstatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selstatus, "sordergive.Status");
            }
            #endregion
            string cmdText = @"SELECT  sordergive.Status ,
        refe_ordergivestatus.RefeName AS StatusName ,
        sordergive.sOrderGiveID ,
        dept.Name AS DeptName ,
        student.Name AS StuName ,
        senroll.EnrollNum ,
        student.IDCard ,
        sordergive.Year ,
        refe_month.RefeName AS MonthName ,
        refe_level.RefeName AS LevelName ,
        pro.Name AS MajorName ,
        CONVERT(NVARCHAR(23), senrollpro.EnrollTime, 23) AS EnrollTime ,
        item.Name AS PlanName ,
        sgive.Name AS GiveName ,
        CONVERT(NVARCHAR(23), sordergive.CreateTime, 23) AS CreateTime
FROM    T_Stu_sOrderGive AS sordergive
        LEFT JOIN T_Sys_Dept AS dept ON sordergive.DeptID = dept.DeptID
        LEFT JOIN T_Stu_sEnrollsProfession AS senrollpro ON sordergive.sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll AS senroll ON senrollpro.sEnrollID = senroll.sEnrollID
        LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
        LEFT JOIN T_Sys_Refe AS refe_month ON sordergive.Month = refe_month.Value
                                              AND refe_month.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                              AND refe_level.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe AS refe_ordergivestatus ON sordergive.Status = refe_ordergivestatus.Value
                                                        AND refe_ordergivestatus.RefeTypeID = 20
        LEFT JOIN T_Stu_sProfession AS spro ON senrollpro.sProfessionID = spro.sProfessionID
        LEFT JOIN T_Pro_Profession AS pro ON spro.ProfessionID = pro.ProfessionID
        LEFT JOIN T_Pro_Item AS item ON sordergive.PlanItemID = item.ItemID
        LEFT JOIN T_Stu_sGive AS sgive ON sgive.sGiveID = sordergive.sGiveID WHERE 1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "sordergive.DeptID", "sordergive.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, ""));
        }
        #endregion
    }
}
