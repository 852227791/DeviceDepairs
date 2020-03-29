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
using Model;
using System.Transactions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebUI.Controllers
{
    public class sOrderController : BaseController
    {
        #region 页面
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sOrderList()
        {
            return View();
        }

        /// <summary>
        /// 查看页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sOrderDetail()
        {
            return View();
        }

        /// <summary>
        /// 修改页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sOrderEdit()
        {
            return View();
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sOrderAdd()
        {
            return View();
        }

        /// <summary>
        /// 设置收费项
        /// </summary>
        /// <returns></returns>
        public ActionResult SetsOrder()
        {
            return View();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取缴费次数下的缴费项
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetsOrderList()
        {
            string sEnrollsProfessionId = Request.Form["sEnrollsProfessionID"];
            string numItemId = Request.Form["NumItemID"];
            string itemId = Request.Form["ItemID"];
            string cmdText = @"SELECT  o.sOrderID ,
        o.ItemDetailID,
        d.Name DetailName ,
        o.ShouldMoney ShouldMoney ,
        o.PaidMoney ReceivedMoney ,
        o.ShouldMoney - o.PaidMoney UnPaid ,
        0.00 PaidMoney ,
        0.00 DiscountMoney ,
        0.00 OffsetMoney ,
        o.PlanItemID ItemID ,
        o.Year ,
        o.Month ,
        o.IsGive ,
        o.NumName ,
        '[]' OffsetDetail,
       CONVERT(NVARCHAR(23),o.LimitTime, 23) AS LimitTime  
FROM    T_Stu_sOrder o
        LEFT JOIN T_Pro_Detail d ON d.DetailID = o.DetailID
		WHERE o.sEnrollsProfessionID={0} and o.NumItemID={1} and o.PlanItemID={2}  and o.Status IN(1,2,3) ";
            cmdText = string.Format(cmdText, sEnrollsProfessionId, numItemId, itemId);
            return AjaxResult.Success(JsonGridData.GetGridJSON(cmdText, Request.Form, "ShouldMoney,ReceivedMoney,UnPaid"), "");
        }

        /// <summary>
        /// 获取缴费次数Tabs
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetOrderInfomation()
        {
            string itemId = Request.Form["ItemID"];
            string sEnrollsProfessionId = Request.Form["sEnrollsProfessionID"];
            return AjaxResult.Success(JsonHelper.DataTableToJson(sOrderBLL.GetsOrderTable(itemId, sEnrollsProfessionId)), "");
        }

        /// <summary>
        /// 获取缴费次数下拉
        /// </summary>
        /// <returns></returns>
        public string GetsOrderCombobox()
        {
            string planItemId = Request.Form["PlanItemID"];
            string proId = Request.Form["ProID"];
            return JsonHelper.DataTableToJson(sOrderBLL.GetsOrderCombobox(planItemId, proId));
        }

        /// <summary>
        /// 修改状态（9：停用）
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetUpdatesStatus()
        {
            string sOrderIDs = Request.Form["IDStr"];
            string status = Request.Form["Value"];
            int result = sOrderBLL.UpdateStatusBysOrders(sOrderIDs, status);
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
        public AjaxResult DownloadsOrder()
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
            string numname = Request.Form["txtNumName"];
            string feename = Request.Form["txtFeeName"];
            string limitTimeS = Request.Form["txtLimitTimeS"];
            string limitTimeE = Request.Form["txtLimitTimeE"];
            string selkindof = Request.Form["selKindof"];
            string selstatus = Request.Form["selStatus"];
            string where = "";
            if (!string.IsNullOrEmpty(dept))
            {
                where += " AND sorder.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
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
                where += " AND sorder.Year like '%" + year + "%'";
            }
            if (!string.IsNullOrEmpty(month))
            {
                where += " AND refe_month.RefeName like '%" + month + "%'";
            }
            if (!string.IsNullOrEmpty(planname))
            {
                where += " AND sorder.PlanName like '%" + planname + "%'";
            }
            if (!string.IsNullOrEmpty(numname))
            {
                where += " AND sorder.NumName like '%" + numname + "%'";
            }
            if (!string.IsNullOrEmpty(feename))
            {
                where += " AND detail.Name like '%" + feename + "%'";
            }
            if (!string.IsNullOrEmpty(limitTimeS))
            {
                where += " AND convert(NVARCHAR(10),sorder.LimitTime,23) >= '" + limitTimeS + "'";
            }
            if (!string.IsNullOrEmpty(limitTimeE))
            {
                where += " AND convert(NVARCHAR(10),sorder.LimitTime,23) <= '" + limitTimeE + "'";
            }
            if (!string.IsNullOrEmpty(selkindof))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selkindof, "sorder.IsGive");
            }
            if (!string.IsNullOrEmpty(selstatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selstatus, "sorder.Status");
            }
            string cmdText = @"SELECT  refe_orderstatus.RefeName AS '状态' ,
        dept.Name AS '校区' ,
        student.Name AS '学生姓名' ,
        senroll.EnrollNum AS '学号' ,
        student.IDCard AS '身份证号' ,
        sorder.Year AS '年份' ,
        refe_month.RefeName AS '月份' ,
        refe_level.RefeName AS '学历层次' ,
        pro.Name AS '报读专业' ,
        CONVERT(NVARCHAR(23), senrollpro.EnrollTime, 23) AS '报名时间' ,
        sorder.PlanName AS '缴费方案' ,
        sorder.NumName AS '缴费学年' ,
        refe_isgive.RefeName AS '收费类别' ,
        detail.Name AS '费用类别' ,
        CONVERT(NVARCHAR(23), sorder.LimitTime, 23) AS '供贷日期' ,
        sorder.ShouldMoney AS '应供贷金额' ,
        sorder.PaidMoney AS '已供贷金额' ,
        (sorder.ShouldMoney-sorder.PaidMoney) as '未供贷金额' ,
        CONVERT(NVARCHAR(23), sorder.CreateTime, 23) AS '创建时间'
FROM    T_Stu_sOrder AS sorder
        LEFT JOIN T_Sys_Dept AS dept ON sorder.DeptID = dept.DeptID
        LEFT JOIN T_Stu_sEnrollsProfession AS senrollpro ON sorder.sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll AS senroll ON senrollpro.sEnrollID = senroll.sEnrollID
        LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
        LEFT JOIN T_Sys_Refe AS refe_month ON sorder.Month = refe_month.Value
                                              AND refe_month.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                              AND refe_level.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe AS refe_orderstatus ON sorder.Status = refe_orderstatus.Value
                                                    AND refe_orderstatus.RefeTypeID = 19
        LEFT JOIN T_Sys_Refe AS refe_isgive ON sorder.IsGive = refe_isgive.Value
                                               AND refe_isgive.RefeTypeID = 15
        LEFT JOIN T_Stu_sProfession AS spro ON senrollpro.sProfessionID = spro.sProfessionID
        LEFT JOIN T_Pro_Profession AS pro ON spro.ProfessionID = pro.ProfessionID
        LEFT JOIN T_Pro_Detail AS detail ON sorder.DetailID = detail.DetailID WHERE 1 = 1 {0} ORDER BY sorder.sOrderID ASC";
            #endregion
            string filename = "缴费单信息.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "sorder.DeptID", "sorder.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }

        /// <summary>
        /// 得到缴费单列表信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetsOrderInfoList()
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
            string numname = Request.Form["txtNumName"];
            string feename = Request.Form["txtFeeName"];
            string limitTimeS = Request.Form["txtLimitTimeS"];
            string limitTimeE = Request.Form["txtLimitTimeE"];
            string selkindof = Request.Form["selKindof"];
            string selstatus = Request.Form["selStatus"];
            string where = "";
            if (!string.IsNullOrEmpty(dept))
            {
                where += " AND sorder.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
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
                where += " AND sorder.Year like '%" + year + "%'";
            }
            if (!string.IsNullOrEmpty(month))
            {
                where += " AND refe_month.RefeName like '%" + month + "%'";
            }
            if (!string.IsNullOrEmpty(planname))
            {
                where += " AND sorder.PlanName like '%" + planname + "%'";
            }
            if (!string.IsNullOrEmpty(numname))
            {
                where += " AND sorder.NumName like '%" + numname + "%'";
            }
            if (!string.IsNullOrEmpty(feename))
            {
                where += " AND detail.Name like '%" + feename + "%'";
            }
            if (!string.IsNullOrEmpty(limitTimeS))
            {
                where += " AND convert(NVARCHAR(10),sorder.LimitTime,23) >= '" + limitTimeS + "'";
            }
            if (!string.IsNullOrEmpty(limitTimeE))
            {
                where += " AND convert(NVARCHAR(10),sorder.LimitTime,23) <= '" + limitTimeE + "'";
            }
            if (!string.IsNullOrEmpty(selkindof))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selkindof, "sorder.IsGive");
            }
            if (!string.IsNullOrEmpty(selstatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selstatus, "sorder.Status");
            }
            #endregion
            string filed = "sOrderID";
            string cmdText = @"SELECT  sorder.Status ,
		refe_orderstatus.RefeName AS StatusName ,
        sorder.sOrderID ,
        sorder.sEnrollsProfessionID ,
        senroll.StudentID ,
        dept.Name AS DeptName ,
        student.Name AS StuName ,
        senroll.EnrollNum ,
        student.IDCard ,
        sorder.Year ,
        refe_month.RefeName AS MonthName ,
        refe_level.RefeName AS LevelName ,
        pro.Name AS MajorName ,
        CONVERT(NVARCHAR(23), senrollpro.EnrollTime, 23) AS EnrollTime ,
        sorder.PlanName ,
        sorder.NumName ,
        refe_isgive.RefeName AS FeeType ,
        detail.Name AS FeeName ,
        CONVERT(NVARCHAR(23), sorder.LimitTime, 23) AS LimitTime ,
        sorder.ShouldMoney ,
        sorder.PaidMoney ,
        (sorder.ShouldMoney-sorder.PaidMoney) as OwnMoney ,
        CONVERT(NVARCHAR(23), sorder.CreateTime, 23) AS CreateTime
FROM    T_Stu_sOrder AS sorder
        LEFT JOIN T_Sys_Dept AS dept ON sorder.DeptID = dept.DeptID
        LEFT JOIN T_Stu_sEnrollsProfession AS senrollpro ON sorder.sEnrollsProfessionID = senrollpro.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll AS senroll ON senrollpro.sEnrollID = senroll.sEnrollID
        LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
        LEFT JOIN T_Sys_Refe AS refe_month ON sorder.Month = refe_month.Value
                                              AND refe_month.RefeTypeID = 18
        LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                              AND refe_level.RefeTypeID = 17
        LEFT JOIN T_Sys_Refe AS refe_orderstatus ON sorder.Status = refe_orderstatus.Value
                                                    AND refe_orderstatus.RefeTypeID = 19
        LEFT JOIN T_Sys_Refe AS refe_isgive ON sorder.IsGive = refe_isgive.Value
                                               AND refe_isgive.RefeTypeID = 15
        LEFT JOIN T_Stu_sProfession AS spro ON senrollpro.sProfessionID = spro.sProfessionID
        LEFT JOIN T_Pro_Profession AS pro ON spro.ProfessionID = pro.ProfessionID
        LEFT JOIN T_Pro_Detail AS detail ON sorder.DetailID = detail.DetailID WHERE 1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "sorder.DeptID", "sorder.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, filed, Request.Form, "ShouldMoney,PaidMoney,OwnMoney"));
        }

        /// <summary>
        /// 转欠费（1：默认）
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetTrunOwnFee()
        {
            string sOrderIDs = Request.Form["IDStr"];
            string status = Request.Form["Value"];
            int result = sOrderBLL.UpdateIsGive(sOrderIDs, status);
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
        /// 修改绑定缴费单信息
        /// </summary>
        /// <returns></returns>
        public AjaxResult SelectsOrder()
        {
            string sorderid = Request.Form["ID"];
            DataTable dt = sOrderBLL.sOrderTableByWhere(" AND sOrderID=@sOrderID", new SqlParameter[] { new SqlParameter("@sOrderID", sorderid) }, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        /// <summary>
        /// 修改保存缴费单
        /// </summary>
        /// <returns></returns>
        public string GetsOrderEdit(sOrderModel sorder)
        {
            string result = "出现未知错误，请联系管理员";

            #region 后台验证
            if (string.IsNullOrEmpty(sorder.DetailID))
            {
                return "请选择缴费项";
            }
            if (string.IsNullOrEmpty(sorder.ShouldMoney))
            {
                return "应收金额不能为空";
            }
            if (string.IsNullOrEmpty(sorder.LimitTime))
            {
                return "请选择缴费截止日期";
            }
            #endregion

            #region 修改
            sOrderModel model = sOrderBLL.sOrderModelByWhere(" AND sOrderID=@sOrderID", new SqlParameter[] { new SqlParameter("@sOrderID", sorder.sOrderID) });

            LogBLL.CreateLog("T_Stu_sOrder", this.UserId.ToString(), model, sorder);//写日志

            model.DetailID = sorder.DetailID;
            model.ShouldMoney = sorder.ShouldMoney;
            model.LimitTime = sorder.LimitTime;
            model.UpdateID = this.UserId.ToString();
            model.UpdateTime = DateTime.Now.ToString();

            if (sOrderBLL.UpdatesOrder(model) > 0)
            {
                result = "yes";
            }
            #endregion
            return result;
        }

        /// <summary>
        /// 添加保存缴费单
        /// </summary>
        /// <param name="sorder"></param>
        /// <returns></returns>
        public string GetsOrderAdd()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                string tips = string.Empty;
                string str = "出现未知错误，请联系管理员";
                try
                {
                    string sEnrollsProfessionID = Request.Form["sEnrollMajor"];
                    string planItemID = Request.Form["sEnrollScheme"];
                    string numItemID = Request.Form["SchemeSemester"];
                    string sOrderDetail = Request.Form["sOrderContent"];

                    #region 添加缴费单
                    if (!string.IsNullOrEmpty(sOrderDetail))
                    {
                        //{"total":1,"rows":[{"DetailID":"2","DetailName":"报名费","Sort":"2","IsGive":"1","ShouldMoney":"200","LimitTime":"2015-02-15"}]}
                        JObject job = JObject.Parse(sOrderDetail);
                        string[] array = job.Properties().Where(x => x.Name == "rows").Select(x => x.Value.ToString()).ToArray();
                        List<sOrderModel.sOrderDetailModel> sOrderList = JsonConvert.DeserializeObject<List<sOrderModel.sOrderDetailModel>>(array[0]);//将json数据转化为对象类型并赋值给list
                        ////报名专业
                        //sEnrollsProfessionModel sProModel = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID) });
                        ////缴费方案
                        //ItemModel itemModel = ItemBLL.ItemModelByWhere(" AND ItemID=@ItemID", new SqlParameter[] { new SqlParameter("@ItemID", planItemID) });
                        ////缴费期数
                        //ItemModel itemNumModel = ItemBLL.ItemModelByWhere(" AND ItemID=@ItemID", new SqlParameter[] { new SqlParameter("@ItemID", numItemID) });
                        sOrderModel sorderModel = sOrderBLL.sOrderModelByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID AND PlanItemID=@PlanItemID AND NumItemID=@NumItemID AND Status!=9", new SqlParameter[] {
                                new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID),
                                new SqlParameter("@PlanItemID", planItemID),
                                new SqlParameter("@NumItemID", numItemID)
                        });
                        foreach (var item in sOrderList)
                        {
                            //判断收费项是否重复（在报名专业、缴费方案、缴费期数下）
                            DataTable repeatDt = sOrderBLL.sOrderTableByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID AND PlanItemID=@PlanItemID AND NumItemID=@NumItemID AND DetailID=@DetailID AND Status!=9", new SqlParameter[] {
                                new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID),
                                new SqlParameter("@PlanItemID", planItemID),
                                new SqlParameter("@NumItemID", numItemID),
                                new SqlParameter("@DetailID", item.DetailID)
                            }, "");
                            if (repeatDt.Rows.Count > 0)
                            {
                                //存在重复的
                                tips += "【" + item.DetailName + "】";
                            }
                            else
                            {
                                sOrderModel sOrder = new sOrderModel();
                                sOrder.Status = "1";
                                sOrder.DeptID = sorderModel.DeptID;
                                sOrder.sEnrollsProfessionID = sEnrollsProfessionID;
                                sOrder.PlanItemID = planItemID;
                                sOrder.PlanName = sorderModel.PlanName;
                                sOrder.PlanLevel = sorderModel.PlanLevel;
                                sOrder.PlanSort = sorderModel.PlanSort;
                                sOrder.Year = sorderModel.Year;
                                sOrder.Month = sorderModel.Month;
                                sOrder.NumItemID = numItemID;
                                sOrder.NumName = sorderModel.NumName;
                                sOrder.LimitTime = item.LimitTime;
                                sOrder.ItemDetailID = "0";
                                sOrder.DetailID = item.DetailID;
                                sOrder.Sort = item.Sort;
                                sOrder.ShouldMoney = item.ShouldMoney;
                                sOrder.PaidMoney = "0";
                                sOrder.IsGive = item.IsGive;
                                sOrder.ItemQueue = sorderModel.ItemQueue;
                                sOrder.ItemDetailQueue = sOrderBLL.GetItemDetailQueue(sEnrollsProfessionID, planItemID, numItemID);
                                sOrder.CreateID = this.UserId.ToString();
                                sOrder.CreateTime = DateTime.Now.ToString();
                                sOrder.UpdateID = this.UserId.ToString();
                                sOrder.UpdateTime = DateTime.Now.ToString();
                                sOrderBLL.InsertsOrder(sOrder);
                            }
                        }
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(tips))
                    {
                        str = "不能重复添加" + tips;
                    }
                    else
                    {
                        str = "yes";
                        ts.Complete();
                    }
                }
                catch
                {
                    str = "出现未知错误，请联系管理员";
                    Transaction.Current.Rollback();
                }
                finally
                {
                    ts.Dispose();
                }
                return str;
            }
        }

        /// <summary>
        /// 根据学生ID得到报名专业下拉数据（有权限控制）
        /// </summary>
        /// <param name="StudentID"></param>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public string SelsEnrollMajor(string StudentID, string MenuID)
        {
            string powerStr = PurviewToList(MenuID);
            DataTable dt = sOrderBLL.GetsEnrollMajor(StudentID, MenuID, powerStr);
            return JsonHelper.DataTableToJson(dt);
        }

        /// <summary>
        /// 根据报名专业ID得到缴费方案（从T_Stu_sOrder取数据）下拉数据
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <returns></returns>
        public string SelsEnrollScheme(string sEnrollsProfessionID)
        {
            DataTable dt = sOrderBLL.GetsEnrollScheme(sEnrollsProfessionID);
            return JsonHelper.DataTableToJson(dt);
        }

        /// <summary>
        /// 根据报名缴费方案ID得到缴费期数（从T_Stu_sOrder取数据）下拉数据
        /// </summary>
        /// <param name="planItemID"></param>
        /// <returns></returns>
        public string SelSchemeSemester(string planItemID)
        {
            DataTable dt = sOrderBLL.GetSchemeSemester(planItemID);
            return JsonHelper.DataTableToJson(dt);
        }
        #endregion

        public AjaxResult SelectPlan(string ID)
        {
            string examNum = Request.Form["ExamNum"];
            string deptId = Request.Form["DeptID"];
            if (string.IsNullOrEmpty(deptId))
            {
                return AjaxResult.Error("请选择收费单位");
            }
            string where = " and DeptID=@DeptID and (ExamNum=@ExamNum OR StudentID IN (SELECT s.StudentID FROM T_Pro_Student s WHERE s.Status=1 AND s.IDCard=@ExamNum))";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptID",deptId),
                new SqlParameter("@ExamNum",examNum)
            };
            sEnrollModel em = sEnrollBLL.sEnrollModelByWhere(where, paras);
            if (string.IsNullOrEmpty(em.sEnrollID))
            {
                return AjaxResult.Error("该收费单位下没有此学号的学生");
            }

            DataTable dt = sEnrollBLL.getEnrollData(examNum, deptId);
            if (dt.Rows.Count == 1)
            {
                string where1 = " and DeptID=@DeptID";
                SqlParameter[] paras1 = new SqlParameter[] {
                  new SqlParameter("@DeptID",deptId)

                };
                var ocm = sOrderCreateBLL.SelectsOrderCreateByWhere(where1, paras1, "").FirstOrDefault();
                if (ocm != null)
                {
                    DataTable dt1 = sEnrollsProfessionBLL.GetsEnrollProfessionTable(em.sEnrollID);
                    if (dt1.Rows.Count > 1)
                    {
                        return AjaxResult.Error("该学生就读多个专业,请手动生成缴费单！");
                    }
                    else if (dt1.Rows.Count == 1)
                    {
                        DataTable dt2 = sOrderBLL.GetsOrderTable(dt1.Rows[0]["sEnrollsProfessionID"].ToString(), false);
                        if (dt2.Rows.Count == 0)
                        {
                            sOrderBLL.BuildsOrder(dt1.Rows[0]["sEnrollsProfessionID"].ToString(), UserId.ToString());
                            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(dt1.Rows[0]["sEnrollsProfessionID"].ToString());
                            epm.Status = "3";
                            epm.UpdateID = UserId.ToString();
                            epm.UpdateTime = DateTime.Now.ToString();
                            sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                             DataTable dt3 = sEnrollBLL.getEnrollData(examNum, deptId);
                            return AjaxResult.Success(JsonHelper.DataTableToJson(dt3), "");
                        }
                    }
                    else if (dt1.Rows.Count == 0)
                    {
                        return AjaxResult.Error("该学生没有报读过任何专业！");
                    }

                }
                return AjaxResult.Success(JsonHelper.DataTableToJson(dt), "");

            }
            else if (dt.Rows.Count == 0)
            {
                return AjaxResult.Error("该学生没有生成缴费单,请先生成缴费单");
            }
            else
            {
                return AjaxResult.Error("该学生就读专业有多个缴费方案,请手动选择缴费方案");
            }
        }
    }
}
