
using BLL;
using Common;
using DAL;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{

    public class sFeeController : BaseController
    {
        ActiveMqHelper amh = new ActiveMqHelper();
        //
        // GET: /sFee/
        public ActionResult SameFeeOffset()
        {

            return View();
        }
        public ActionResult ChooseFastsOffset()
        {

            return View();
        }

        public ActionResult ChooseQuicksOffset()
        {

            return View();
        }
        public ActionResult sFeePrint()
        {
            return View();
        }
        public ActionResult sFeeList()
        {
            return View();
        }
        public ActionResult sFeeEdit()
        {
            return View();
        }
        public ActionResult ChooseFeeOffsetItem()
        {
            ViewBag.Title = "选择冲抵项";
            return View();
        }
        public ActionResult ChangeShouldMoney()
        {
            ViewBag.Title = "修改应收金额";
            return View();
        }
        public ActionResult ChooseFeePlan()
        {
            ViewBag.Title = "选择缴费方案";
            return View();
        }
        public ActionResult sFeeView()
        {
            ViewBag.Title = "查看收费信息";
            return View();
        }
        public ActionResult sFeeUpload()
        {
            ViewBag.Title = "导入收费信息";
            return View();
        }
        public ActionResult sFeeModify()
        {
            ViewBag.Title = "修改缴费信息";
            return View();
        }
        /// <summary>
        /// 获取选择缴费方案的列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetChooseFeePlanList()
        {
            string idCard = Request.Form["txtsIDCard"];
            string name = Request.Form["txtsName"];
            string enrollNum = Request.Form["txtsEnrollNum"];
            string deptId = Request.Form["DeptID"];
            string where = "";

            if (string.IsNullOrEmpty(deptId))
                deptId = "0";
            if (!string.IsNullOrEmpty(idCard))
                where += " and  s.IDCard like '%" + idCard + "%'";
            if (!string.IsNullOrEmpty(name))
                where += " and  s.Name like '%" + name + "%'";
            if (!string.IsNullOrEmpty(enrollNum))
                where += " and  se.EnrollNum  like '%" + enrollNum + "%'";

            string where1 = " and  DeptID=" + deptId + "";

            string cmdText = @"SELECT  so.PlanName ,
        s.Name StudentName ,
        pn.Name ProName ,
        s.IDCard ,
        so.PlanItemID ItemID ,
        so.sEnrollsProfessionID ,
        se.EnrollNum ,
        so.Year ,
        so.Month ,
        CONVERT(NVARCHAR(10), sep.BeforeEnrollTime, 23) BeforeEnrollTime ,
        CONVERT(NVARCHAR(10), sep.EnrollTime, 23) EnrollTime ,
        CONVERT(NVARCHAR(10), sep.FirstFeeTime, 23) FirstFeeTime
FROM    T_Stu_sOrder so
        LEFT JOIN T_Stu_sEnrollsProfession sep ON so.sEnrollsProfessionID = sep.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll se ON sep.sEnrollID = se.sEnrollID
        LEFT JOIN T_Pro_Student s ON se.StudentID = s.StudentID
        LEFT JOIN T_Stu_sProfession sp ON sp.sProfessionID = sep.sProfessionID
        LEFT JOIN T_Pro_Profession pn ON pn.ProfessionID = sp.ProfessionID
WHERE   so.sOrderID IN ( SELECT MIN(sOrderID)
                         FROM   T_Stu_sOrder
                         WHERE  1 = 1
                         GROUP BY sEnrollsProfessionID ,
                                PlanItemID )
        AND sep.Status IN ( 1, 2, 3, 4, 5 )
        AND sep.DeptID IN ( SELECT  DeptID
                            FROM    T_Sys_Dept
                            WHERE   ParentID = ( SELECT ParentID
                                                 FROM   T_Sys_Dept
                                                 WHERE  Status = 1
                                                        {0}
                                               ) ){1}

    ";
            cmdText = string.Format(cmdText, where1, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        /// <summary>
        /// 编辑收费
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetsFeeEdit(GetsFeeFormDataModel gf)
        {
            if (string.IsNullOrEmpty(gf.FeeMode))
            {
                return AjaxResult.Error("请选择收费方式");
            }

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                if (!string.IsNullOrEmpty(gf.Explain))
                    gf.Explain = gf.Explain.Replace("\r\n", "<br />");
                else
                    gf.Explain = "";

                if (!string.IsNullOrEmpty(gf.Remark))
                    gf.Remark = gf.Remark.Replace("\r\n", "<br />");
                else
                    gf.Remark = "";


                gf.GiveJsonData = "{\"rows\":" + gf.GiveJsonData + "}";
                List<sOrderListModel> order = JsonConvert.DeserializeObject<List<sOrderListModel>>(gf.OrderJsonData);
                sGiveListModel give = JsonConvert.DeserializeObject<sGiveListModel>(gf.GiveJsonData);


                bool flag = false;
                List<ReturnDataModel> temp = new List<ReturnDataModel>();
                foreach (var o in order)
                {
                    var orderlist = o.rows.Where(O => O.DiscountMoney > 0 || O.PaidMoney > 0 || O.OffsetMoney > 0).ToList();
                    if (orderlist.Count > 0)
                        flag = true;
                    foreach (var item in orderlist)//实收+充抵+优惠不能大于未缴金额
                    {
                        decimal paidMoney = 0;

                        decimal tempMoney = sOrderBLL.GetShouldMoney(item.sOrderID, ref paidMoney);
                        if (tempMoney < item.PaidMoney + paidMoney + item.DiscountMoney + item.OffsetMoney)
                            return AjaxResult.Error("" + item.NumName + item.DetailName + ": 实收+充抵+优惠+已收 不能大于应收金额" + tempMoney + "元");
                    }
                }
                if (!flag)
                    return AjaxResult.Error("没有收取任何费用");
                if (give.rows.Count > 0)
                {
                    var list = order.Where(O => O.NumItemID.Equals(give.rows.FirstOrDefault().NumItemID)).ToList();
                    foreach (var o in list)
                    {
                        var orderlist = o.rows.Where(O => O.DiscountMoney > 0 || O.PaidMoney > 0 || O.OffsetMoney > 0).ToList();
                        if (orderlist.Count == 0)
                            return AjaxResult.Error("配品选择的缴费次数下没有任何缴费记录，请重新选择配品归属的缴费次数");
                    }
                    foreach (var item in give.rows)
                    {
                        string where = " and Status=1 and sOrderGiveID=@sOrderGiveID";
                        SqlParameter[] paras = new SqlParameter[] {
                            new SqlParameter("@sOrderGiveID",item.sOrderGiveID)
                        };
                        DataTable dt = sFeesOrderGiveBLL.sFeesOrderGiveTableByWhere(where, paras, "");
                        if (dt.Rows.Count > 0)
                        {
                            return AjaxResult.Error("不能多次选择此配品");
                        }
                    }
                }
                try
                {
                    PaymentModel pm = new PaymentModel();

                    foreach (var o in order)
                    {
                        var orderlist = o.rows.Where(O => O.DiscountMoney > 0 || O.PaidMoney > 0 || O.OffsetMoney > 0).ToList();
                        if (orderlist.Count > 0)
                        {
                            decimal shouldMoney = o.rows.Sum(O => O.ShouldMoney);// 应缴金额
                            decimal paidMoney = o.rows.Sum(O => (O.PaidMoney + O.DiscountMoney + O.OffsetMoney));

                            pm.PaidInAmount = shouldMoney;
                            pm.ReceivableAmount = paidMoney;

                            gf.sFeeID = InsertsFee(gf, o.NumItemID, shouldMoney, paidMoney);//添加收费
                            ReturnDataModel rdm = new ReturnDataModel();
                            rdm.sFeeId = gf.sFeeID;
                            var typelist = orderlist.FirstOrDefault();
                            rdm.type = sOrderBLL.GetEnrollProfessionStatus(typelist.sOrderID);
                            temp.Add(rdm);//返回收费id用户打印

                            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(gf.sEnrollsProfessionID);

                            if (!epm.Status.Equals("2"))
                            {
                                DataTable enrollTable = sOrderBLL.GetEnrollNum(typelist.sOrderID);
                                if (string.IsNullOrEmpty(enrollTable.Rows[0]["EnrollNum"].ToString()))//没有学号分配学号
                                {
                                    string where = " and sEnrollID=@sEnrollID";
                                    SqlParameter[] paras = new SqlParameter[] {
                                         new SqlParameter("@sEnrollID",enrollTable.Rows[0]["sEnrollID"].ToString())
                                    };
                                    sEnrollModel em = sEnrollBLL.sEnrollModelByWhere(where, paras);
                                    em.EnrollNum = sEnrollBLL.getEnrollNum(enrollTable.Rows[0]["DeptID"].ToString(), enrollTable.Rows[0]["Year"].ToString(), UserId.ToString());
                                    em.UpdateID = UserId.ToString();
                                    em.UpdateTime = DateTime.Now.ToString();
                                    sEnrollBLL.UpdatesEnroll(em);
                                }

                            }
                            foreach (var item in orderlist)
                            {
                                List<sFeeOffset> offset = JsonConvert.DeserializeObject<List<sFeeOffset>>(item.OffsetDetail);//获取当前收费项目的充抵项
                                sFeesOrderModel fom = new sFeesOrderModel();
                                fom.CanMoney = (item.PaidMoney + item.OffsetMoney).ToString();
                                fom.CreateID = this.UserId.ToString();
                                fom.CreateTime = DateTime.Now.ToString();
                                fom.UpdateID = this.UserId.ToString();
                                fom.UpdateTime = DateTime.Now.ToString();
                                fom.DiscountMoney = item.DiscountMoney.ToString();
                                fom.PaidMoney = item.PaidMoney.ToString();
                                fom.sFeeID = gf.sFeeID;
                                fom.ShouldMoney = item.ShouldMoney.ToString();
                                fom.sOrderID = item.sOrderID;
                                fom.Status = "1";
                                int ralatedId = sFeesOrderBLL.InsertsFeesOrder(fom);//添加收费明细
                                foreach (var off in offset)
                                {
                                    sOffsetModel ofm = new sOffsetModel();
                                    ofm.ByRelatedID = off.ID;
                                    ofm.RelatedID = ralatedId.ToString();
                                    ofm.UpdateID = UserId.ToString();
                                    ofm.UpdateTime = DateTime.Now.ToString();
                                    ofm.CreateID = UserId.ToString();
                                    ofm.CreateTime = DateTime.Now.ToString();
                                    ofm.BySort = off.Sort;
                                    ofm.Money = off.Money.ToString();
                                    ofm.Status = "1";
                                    sOffsetBLL.InsertsOffset(ofm);//添加充抵
                                    UpdateCanMoney(off.Money, off.ID, off.Sort);//更新可用金额
                                    if (off.Sort.Equals("1"))//被冲抵的是否是学费
                                    {
                                        string sorderId = sFeesOrderBLL.GetsFeeOrderModel(off.ID).sOrderID;//获取被充抵的订单编号
                                        sOrderBLL.UpdatesOderPaidMoney(-off.Money, sorderId, UserId);//更新充抵订单的实缴金额
                                        sFeesOrderModel sfom = sFeesOrderBLL.GetsFeeOrderModel(off.ID);
                                        sFeeBLL.UpdatePaidMoney(sfom.sFeeID, -off.Money, UserId);
                                    }
                                    else if (off.Sort.Equals("3"))
                                    {
                                        string feeId = FeeDetailBLL.GetFeeDetailModel(off.ID).FeeID;
                                        FeeBLL.UpdateFeePaidMoney(feeId, -off.Money, UserId);
                                    }
                                }
                                decimal offsetMoney = offset.Sum(O => O.Money);
                                decimal tempMoney = decimal.Parse(fom.PaidMoney) + item.DiscountMoney + offsetMoney;
                                sOrderBLL.UpdatesOderPaidMoney(tempMoney, fom.sOrderID, UserId);//更新订单的已缴金额
                            }
                            foreach (var item in give.rows)
                            {
                                if (item.NumItemID == o.NumItemID)
                                {
                                    sFeesOrderGiveModel fogm = new sFeesOrderGiveModel();
                                    fogm.CreateID = this.UserId.ToString();
                                    fogm.CreateTime = DateTime.Now.ToString();
                                    fogm.sFeeID = gf.sFeeID;
                                    fogm.sOrderGiveID = item.sOrderGiveID;
                                    fogm.Status = "1";
                                    fogm.UpdateID = this.UserId.ToString();
                                    fogm.UpdateTime = DateTime.Now.ToString();
                                    sFeesOrderGiveBLL.InsertsFeesOrderGive(fogm);
                                    UpdatesOrderGiveStatus(item.sOrderGiveID, "2");
                                }
                            }

                            if (string.IsNullOrEmpty(epm.FirstFeeTime))
                            {
                                epm.FirstFeeTime = gf.FeeTime;
                                epm.UpdateID = UserId.ToString();
                                epm.UpdateTime = DateTime.Now.ToString();
                                sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                            }
                            else if (Convert.ToDateTime(epm.FirstFeeTime).ToString("yyyy-MM-dd") == "1900-01-01")
                            {
                                epm.FirstFeeTime = gf.FeeTime;
                                epm.UpdateID = UserId.ToString();
                                epm.UpdateTime = DateTime.Now.ToString();
                                sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                            }

                        }


                    }

                    sEnrollsProfessionBLL.UpdatesEnrollsProfessionStatus(gf.sEnrollsProfessionID, UserId.ToString());//修改报名状态


                    pm.IdCard = sEnrollsProfessionBLL.GetStudentIdCard(gf.sEnrollsProfessionID);
                    pm.OrgId = DeptDCPBLL.GetDcpDeptID(gf.DeptID).ToString();
                    pm.Major = ProfessionBLL.GetProfesionName(gf.sEnrollsProfessionID);
                    string message = OtherHelper.JsonSerializer(pm);
                    if (!pm.OrgId.Equals("0"))
                    {
                        amh.PublishMessage(OtherHelper.GetAppSettingsValue("SendMessageName"), message, null, false, false);
                    }
                    ts.Complete();
                    return AjaxResult.Success(OtherHelper.JsonSerializer(temp), "保存成功");
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    return AjaxResult.Error(ex.Message);
                }
                finally
                {
                    ts.Dispose();
                }
            }
        }
        /// <summary>
        /// 添加收费信息
        /// </summary>
        /// <param name="gf"></param>
        /// <param name="numItemId"></param>
        /// <param name="shouldMoney"></param>
        /// <param name="paidMoney"></param>
        /// <returns></returns>
        private string InsertsFee(GetsFeeFormDataModel gf, string numItemId, decimal shouldMoney, decimal paidMoney)
        {
            sFeeModel sfm = new sFeeModel();
            sfm.PlanItemID = sOrderBLL.GetPlanItemId(numItemId);
            sfm.NumItemID = numItemId;
            sfm.AffirmID = "0";
            sfm.AffirmTime = "1900-01-01";
            sfm.CreateID = this.UserId.ToString();
            sfm.CreateTime = DateTime.Now.ToString();
            sfm.DeptID = gf.DeptID;
            sfm.Explain = gf.Explain;
            sfm.FeeMode = gf.FeeMode;
            sfm.FeeTime = gf.FeeTime;
            sfm.NoteNum = "";
            sfm.PrintNum = "0";
            sfm.ShouldMoney = shouldMoney.ToString();
            sfm.PaidMoney = paidMoney.ToString();
            sfm.Remark = gf.Remark;
            sfm.sEnrollsProfessionID = gf.sEnrollsProfessionID;
            sfm.Status = "1";
            sfm.UpdateID = this.UserId.ToString();
            sfm.UpdateTime = DateTime.Now.ToString();
            sfm.VoucherNum = ConfigBLL.getVoucherNum("3", "X");
            return sFeeBLL.InsertsFee(sfm).ToString();
        }

        public AjaxResult GetsFeeListfoot(string MenuID)
        {
            string where = "";
            string name = Request.Form["txtName"];
            string deptId = Request.Form["deptId"];
            string idCard = Request.Form["idCard"];
            string voucherNum = Request.Form["voucherNum"];
            string status = Request.Form["status"];
            string feeMode = Request.Form["feeMode"];
            string feeTimeS = Request.Form["feeTimeS"];
            string feeTimeE = Request.Form["feeTimeE"];
            string feeUser = Request.Form["txtFeeUser"];
            if (!string.IsNullOrEmpty(feeUser))
            {
                where += " and u1.Name like '%" + feeUser + "%'";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " and s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and f.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where += " and  convert(nvarchar(10),f.FeeTime,23) >='" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where += " and  convert(nvarchar(10),f.FeeTime,23) <='" + feeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND f.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(feeMode))
            {
                where += OtherHelper.MultiSelectToSQLWhere(feeMode, "f.FeeMode");
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "f.Status");
            }

            string cmdText = @"SELECT  ( SELECT '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(f.ShouldMoney),0)) 
          FROM      T_Stu_sFee f
                    LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                    LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                    LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                    LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                               AND r1.RefeTypeID = 7
                    LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                               AND r2.RefeTypeID = 6
                    LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                    LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                    LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                    LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
          WHERE     1 = 1 {0}
        ) ShouldMoney ,
        ( SELECT  '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(PaidMoney),0))
          FROM      T_Stu_sFeesOrder
          WHERE     sFeeID IN (
                    SELECT  f.sFeeID
                    FROM    T_Stu_sFee f
                            LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                            LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                            LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                            LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                            LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                                       AND r1.RefeTypeID = 7
                            LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                                       AND r2.RefeTypeID = 6
                            LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                            LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                            LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                            LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
                    WHERE   1 = 1 {0} )
                    AND Status = 1
        ) PaidMoney ,
        ( SELECT   '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(DiscountMoney),0))  
          FROM      T_Stu_sFeesOrder
          WHERE     sFeeID IN (
                    SELECT  f.sFeeID
                    FROM    T_Stu_sFee f
                            LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                            LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                            LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                            LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                            LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                                       AND r1.RefeTypeID = 7
                            LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                                       AND r2.RefeTypeID = 6
                            LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                            LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                            LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                            LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
                    WHERE   1 = 1 {0}  )
                    AND Status = 1
        ) DiscountMoney ,
        ( SELECT   '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(Money),0)) 
          FROM      T_Stu_sOffset
          WHERE     RelatedID IN (
                    SELECT  sFeesOrderID
                    FROM    T_Stu_sFeesOrder
                    WHERE   sFeeID IN (
                            SELECT  f.sFeeID
                            FROM    T_Stu_sFee f
                                    LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                                    LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                                    LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                                    LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                                    LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                                              AND r1.RefeTypeID = 7
                                    LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                                              AND r2.RefeTypeID = 6
                                    LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                                    LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                                    LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                                    LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
                            WHERE   1 = 1 {0}  )
                            AND Status = 1 )
                    AND Status = 1
        ) OffsetMoney ,
        ( SELECT     '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(Money),0)) 
          FROM      T_Stu_sOffset
          WHERE     ByRelatedID IN (
                    SELECT  sFeesOrderID
                    FROM    T_Stu_sFeesOrder
                    WHERE   sFeeID IN (
                            SELECT  f.sFeeID
                            FROM    T_Stu_sFee f
                                    LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                                    LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                                    LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                                    LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                                    LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                                              AND r1.RefeTypeID = 7
                                    LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                                              AND r2.RefeTypeID = 6
                                    LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                                    LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                                    LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                                    LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
                            WHERE   1 = 1 {0} )
                            AND Status = 1 )
                    AND BySort = 1
                    AND Status = 1
        ) ByOffsetMoney ,
        ( SELECT    '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(RefundMoney),0)) 
          FROM      T_Stu_sRefund
          WHERE     sFeesOrderID IN (
                    SELECT  sFeesOrderID
                    FROM    T_Stu_sFeesOrder
                    WHERE   sFeeID IN (
                            SELECT  f.sFeeID
                            FROM    T_Stu_sFee f
                                    LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                                    LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                                    LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                                    LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                                    LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                                              AND r1.RefeTypeID = 7
                                    LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                                              AND r2.RefeTypeID = 6
                                    LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                                    LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                                    LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                                    LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
                            WHERE   1 = 1 {0} )
                            AND Status = 1 )
                    AND Status = 1
        ) RefundMoney";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "f.DeptID", "f.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return AjaxResult.Success(JsonHelper.DataTableToJson(DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0]), "success");

        }


        /// <summary>
        /// 获取收费信息列表
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public ActionResult GetsFeeList(string MenuID)
        {
            string where = "";
            string name = Request.Form["txtName"];
            string deptId = Request.Form["deptId"];
            string idCard = Request.Form["idCard"];
            string voucherNum = Request.Form["voucherNum"];
            string noteNum = Request.Form["noteNum"];
            string status = Request.Form["status"];
            string feeMode = Request.Form["feeMode"];
            string feeTimeS = Request.Form["feeTimeS"];
            string feeTimeE = Request.Form["feeTimeE"];
            string feeUser = Request.Form["txtFeeUser"];
            string year = Request.Form["selYear"];
            string month = Request.Form["selMonth"];
            string planSort = Request.Form["selPlanSort"];
            string address = Request.Form["address"];
            string enrollSchool = Request.Form["enrollSchool"];

            string enrollNum = Request.Form["enrollNum"];
            if (!string.IsNullOrEmpty(enrollNum))
            {
                where += " and e.EnrollNum like '%" + enrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(enrollSchool))
            {
                where += " and da.Name like '%" + enrollSchool + "%'";
            }
            if (!string.IsNullOrEmpty(address))
            {
                where += " and s.Address like '%" + address + "%'";
            }
            if (!string.IsNullOrEmpty(feeUser))
            {
                where += " and u1.Name like '%" + feeUser + "%'";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " and s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and f.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " and f.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where += " and  convert(nvarchar(10),f.FeeTime,23) >='" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where += " and  convert(nvarchar(10),f.FeeTime,23) <='" + feeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND f.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(feeMode))
            {
                where += OtherHelper.MultiSelectToSQLWhere(feeMode, "f.FeeMode");
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "f.Status");
            }
            if (!string.IsNullOrEmpty(year))
            {
                where += OtherHelper.MultiSelectToSQLWhere(year, "ep.Year");
            }
            if (!string.IsNullOrEmpty(month))
            {
                where += OtherHelper.MultiSelectToSQLWhere(month, "ep.Month");
            }
            if (!string.IsNullOrEmpty(planSort))
            {
                if (planSort.Substring(0, 1).Equals(","))
                {
                    planSort = planSort.Substring(1, planSort.Length - 1);
                }
                where += @" AND ep.sEnrollsProfessionID IN ( SELECT so.sEnrollsProfessionID
                                         FROM   T_Stu_sOrder so
                                         WHERE  so.Status <> 9
                                                AND so.PlanSort IN ( "+ planSort + " ) )";
            }
            string filed = "sFeeID";
            string cmdText = @"SELECT  f.sFeeID ,
        f.Status StatusValue ,
        d.Name DeptName ,
        f.VoucherNum ,
        f.NoteNum ,
        r1.RefeName Status ,
        r2.RefeName FeeModel ,
        f.ShouldMoney ,
        s.Name StudName ,
        e.EnrollNum ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        s.IDCard ,
        p.Name ProName ,
        ( SELECT    ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sFeesOrder
          WHERE     sFeeID = f.sFeeID
                    AND Status = 1
        ) PaidMoney ,
        ( SELECT    ISNULL(SUM(DiscountMoney), 0)
          FROM      T_Stu_sFeesOrder
          WHERE     sFeeID = f.sFeeID
                    AND Status = 1
        ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(Money), 0.00)
          FROM      T_Stu_sOffset
          WHERE     RelatedID IN ( SELECT   sFeesOrderID
                                   FROM     T_Stu_sFeesOrder
                                   WHERE    sFeeID = f.sFeeID
                                            AND Status = 1 )
                    AND Status = 1
        ) OffsetMoney ,
        ( SELECT    t + ','
          FROM      ( SELECT    fee.VoucherNum + '_' + s.Name + '_'
                                + CONVERT(NVARCHAR(10), so.Money) t
                      FROM      T_Stu_sOffset so
                                LEFT JOIN T_Pro_FeeDetail fd ON so.ByRelatedID = fd.FeeDetailID
                                LEFT JOIN T_Pro_Fee fee ON fd.FeeID = fee.FeeID
                                LEFT JOIN T_Pro_Prove prove ON fee.ProveID = prove.ProveID
                                LEFT JOIN T_Pro_Student s ON prove.StudentID = s.StudentID
                      WHERE     so.BySort = 3
                                AND so.Status = 1
                                AND so.RelatedID IN (
                                SELECT  sfo.sFeesOrderID
                                FROM    T_Stu_sFeesOrder sfo
                                WHERE   sfo.Status = 1
                                        AND sfo.sFeeID = f.sFeeID )
                      UNION ALL
                      SELECT    fee.VoucherNum + '_' + s.Name + '_'
                                + CONVERT(NVARCHAR(10), so.Money) t
                      FROM      T_Stu_sOffset so
                                LEFT JOIN T_Inc_iFee fee ON so.ByRelatedID = fee.iFeeID
                                LEFT JOIN T_Pro_Student s ON fee.StudentID = s.StudentID
                      WHERE     so.BySort = 2
                                AND so.Status = 1
                                AND so.RelatedID IN (
                                SELECT  sfo.sFeesOrderID
                                FROM    T_Stu_sFeesOrder sfo
                                WHERE   sfo.Status = 1
                                        AND sfo.sFeeID = f.sFeeID )
                      UNION ALL
                      SELECT    sf.VoucherNum + '_' + s.Name + '_'
                                + CONVERT(NVARCHAR(10), so.Money)
                      FROM      T_Stu_sOffset so
                                LEFT JOIN T_Stu_sFeesOrder sfo ON so.ByRelatedID = sfo.sFeesOrderID
                                LEFT JOIN T_Stu_sFee sf ON sfo.sFeeID = sf.sFeeID
                                LEFT JOIN T_Stu_sEnrollsProfession ep ON sf.sEnrollsProfessionID = ep.sEnrollsProfessionID
                                LEFT JOIN T_Stu_sEnroll se ON ep.sEnrollID = se.sEnrollID
                                LEFT JOIN T_Pro_Student s ON se.StudentID = s.StudentID
                      WHERE     so.BySort = 1
                                AND so.Status = 1
                                AND so.RelatedID IN (
                                SELECT  sfeeo.sFeesOrderID
                                FROM    T_Stu_sFeesOrder sfeeo
                                WHERE   sfeeo.Status = 1
                                        AND sfeeo.sFeeID = f.sFeeID )
                    ) AS byTemp
        FOR
          XML PATH('')
        ) OffsetString ,
        ( SELECT    ISNULL(SUM(Money), 0.00)
          FROM      T_Stu_sOffset
          WHERE     ByRelatedID IN ( SELECT sFeesOrderID
                                     FROM   T_Stu_sFeesOrder
                                     WHERE  sFeeID = f.sFeeID
                                            AND Status = 1 )
                    AND BySort = 1
                    AND Status = 1
        ) ByOffsetMoney ,
        ( SELECT    sf.VoucherNum + '_' + s.Name + '_'
                    + CONVERT(NVARCHAR(10), so.Money) + ','
          FROM      T_Stu_sOffset so
                    LEFT JOIN T_Stu_sFeesOrder sfo ON so.RelatedID = sfo.sFeesOrderID
                    LEFT JOIN T_Stu_sFee sf ON sfo.sFeeID = sf.sFeeID
                    LEFT JOIN T_Stu_sEnrollsProfession ep ON sf.sEnrollsProfessionID = ep.sEnrollsProfessionID
                    LEFT JOIN T_Stu_sEnroll se ON ep.sEnrollID = se.sEnrollID
                    LEFT JOIN T_Pro_Student s ON se.StudentID = s.StudentID
          WHERE     so.BySort = 1
                    AND so.Status = 1
                    AND so.ByRelatedID IN (
                    SELECT  sfeeo.sFeesOrderID
                    FROM    T_Stu_sFeesOrder sfeeo
                    WHERE   sfeeo.Status = 1
                            AND sfeeo.sFeeID = f.sFeeID )
        FOR
          XML PATH('')
        ) ByOffsetString ,
        ( SELECT    ISNULL(SUM(RefundMoney), 0.00)
          FROM      T_Stu_sRefund
          WHERE     sFeesOrderID IN ( SELECT    sFeesOrderID
                                      FROM      T_Stu_sFeesOrder
                                      WHERE     sFeeID = f.sFeeID
                                                AND Status = 1 )
                    AND Status = 1
        ) RefundMoney ,
        u.Name Affirm ,
        CASE CONVERT(NVARCHAR(23), f.AffirmTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(23), f.AffirmTime, 23)
        END AffirmTime ,
        ep.Status EnrollStatus ,
        u1.Name FeeUser,
		        ( SELECT TOP 1
                    o.NumName
          FROM      T_Stu_sOrder o
                    LEFT JOIN T_Stu_sFeesOrder fo ON fo.sOrderID = o.sOrderID
          WHERE     fo.sFeeID = f.sFeeID
        ) FeeNum,
		s.Address,
		da.Name EnrollSchool
FROM    T_Stu_sFee f
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                   AND r1.RefeTypeID = 7
        LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                   AND r2.RefeTypeID = 6
        LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
        LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
        LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
        LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
		LEFT JOIN T_Pro_DeptArea da ON  da.DeptAreaID=ep.DeptAreaID
WHERE   1 = 1
        {0}";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "f.DeptID", "f.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);

            string cmdText2 = @"SELECT  ( SELECT '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(f.ShouldMoney),0)) 
          FROM      T_Stu_sFee f
                    LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                    LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                    LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                    LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                    LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                               AND r1.RefeTypeID = 7
                    LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                               AND r2.RefeTypeID = 6
                    LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                    LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                    LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                    LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
          WHERE     1 = 1 {0}
        ) ShouldMoney ,
        ( SELECT  '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(PaidMoney),0))
          FROM      T_Stu_sFeesOrder
          WHERE     sFeeID IN (
                    SELECT  f.sFeeID
                    FROM    T_Stu_sFee f
                            LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                            LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                            LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                            LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                            LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                                       AND r1.RefeTypeID = 7
                            LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                                       AND r2.RefeTypeID = 6
                            LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                            LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                            LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                            LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
                    WHERE   1 = 1 {0} )
                    AND Status = 1
        ) PaidMoney ,
        ( SELECT   '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(DiscountMoney),0))  
          FROM      T_Stu_sFeesOrder
          WHERE     sFeeID IN (
                    SELECT  f.sFeeID
                    FROM    T_Stu_sFee f
                            LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                            LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                            LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                            LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                            LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                                       AND r1.RefeTypeID = 7
                            LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                                       AND r2.RefeTypeID = 6
                            LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                            LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                            LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                            LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
                    WHERE   1 = 1 {0}  )
                    AND Status = 1
        ) DiscountMoney ,
        ( SELECT   '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(Money),0)) 
          FROM      T_Stu_sOffset
          WHERE     RelatedID IN (
                    SELECT  sFeesOrderID
                    FROM    T_Stu_sFeesOrder
                    WHERE   sFeeID IN (
                            SELECT  f.sFeeID
                            FROM    T_Stu_sFee f
                                    LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                                    LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                                    LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                                    LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                                    LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                                              AND r1.RefeTypeID = 7
                                    LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                                              AND r2.RefeTypeID = 6
                                    LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                                    LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                                    LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                                    LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
                            WHERE   1 = 1 {0}  )
                            AND Status = 1 )
                    AND Status = 1
        ) OffsetMoney ,
        ( SELECT     '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(Money),0)) 
          FROM      T_Stu_sOffset
          WHERE     ByRelatedID IN (
                    SELECT  sFeesOrderID
                    FROM    T_Stu_sFeesOrder
                    WHERE   sFeeID IN (
                            SELECT  f.sFeeID
                            FROM    T_Stu_sFee f
                                    LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                                    LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                                    LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                                    LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                                    LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                                              AND r1.RefeTypeID = 7
                                    LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                                              AND r2.RefeTypeID = 6
                                    LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                                    LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                                    LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                                    LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
                            WHERE   1 = 1 {0} )
                            AND Status = 1 )
                    AND BySort = 1
                    AND Status = 1
        ) ByOffsetMoney ,
        ( SELECT    '合计:'+CONVERT(NVARCHAR(18),ISNULL(SUM(RefundMoney),0)) 
          FROM      T_Stu_sRefund
          WHERE     sFeesOrderID IN (
                    SELECT  sFeesOrderID
                    FROM    T_Stu_sFeesOrder
                    WHERE   sFeeID IN (
                            SELECT  f.sFeeID
                            FROM    T_Stu_sFee f
                                    LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
                                    LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
                                    LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
                                    LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
                                    LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                                              AND r1.RefeTypeID = 7
                                    LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                                              AND r2.RefeTypeID = 6
                                    LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
                                    LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
                                    LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
                                    LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
                            WHERE   1 = 1 {0} )
                            AND Status = 1 )
                    AND Status = 1
        ) RefundMoney";
            cmdText2 = string.Format(cmdText2, where + powerStr);

            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, filed, cmdText2, Request.Form));//"ShouldMoney,PaidMoney,DiscountMoney,RefundMoney"
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public AjaxResult DownloadsFee()
        {
            string where = "";
            string menuId = Request.Form["MenuID"];
            string name = Request.Form["txtName"];
            string deptId = Request.Form["deptId"];
            string idCard = Request.Form["idCard"];
            string voucherNum = Request.Form["voucherNum"];
            string noteNum = Request.Form["noteNum"];
            string status = Request.Form["status"];
            string feeMode = Request.Form["feeMode"];
            string feeTimeS = Request.Form["feeTimeS"];
            string feeTimeE = Request.Form["feeTimeE"];
            string address = Request.Form["address"];
            string enrollSchool = Request.Form["enrollSchool"];
            string enrollNum = Request.Form["enrollNum"];
            string feeUser = Request.Form["txtFeeUser"];
            string year = Request.Form["selYear"];
            string month = Request.Form["selMonth"];
            string planSort = Request.Form["selPlanSort"];
            if (!string.IsNullOrEmpty(enrollNum))
            {
                where += " and e.EnrollNum like '%" + enrollNum + "%'";
            }
            if (!string.IsNullOrEmpty(enrollSchool))
            {
                where += " and da.Name like '%" + enrollSchool + "%'";
            }
            if (!string.IsNullOrEmpty(address))
            {
                where += " and s.Address like '%" + address + "%'";
            }
            if (!string.IsNullOrEmpty(feeUser))
            {
                where += " and u1.Name like '%" + feeUser + "%'";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " and s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and f.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " and f.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where += " and  convert(nvarchar(10),f.FeeTime,23) >='" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where += " and  convert(nvarchar(10),f.FeeTime,23) <='" + feeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND f.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(feeMode))
            {
                where += OtherHelper.MultiSelectToSQLWhere(feeMode, "f.FeeMode");
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "f.Status");
            }
            if (!string.IsNullOrEmpty(year))
            {
                where += OtherHelper.MultiSelectToSQLWhere(year, "ep.Year");
            }
            if (!string.IsNullOrEmpty(month))
            {
                where += OtherHelper.MultiSelectToSQLWhere(month, "ep.Month");
            }
            if (!string.IsNullOrEmpty(planSort))
            {
                if (planSort.Substring(0, 1).Equals(","))
                {
                    planSort = planSort.Substring(1, planSort.Length - 1);
                }
                where += @" AND ep.sEnrollsProfessionID IN ( SELECT so.sEnrollsProfessionID
                                         FROM   T_Stu_sOrder so
                                         WHERE  so.Status <> 9
                                                AND so.PlanSort IN ( " + planSort + " ) )";
            }
            string cmdText = @"SELECT  f.VoucherNum 凭证号 ,
        f.NoteNum 票据号 ,
        s.Name 学生姓名 ,
        e.EnrollNum 学号 ,
        s.IDCard 身份证号 ,
        p.Name 专业 ,
        ep.Year 年份 ,
        ep.Month 月份 ,
        ( SELECT    r.RefeName
          FROM      T_Stu_sOrder so
                    LEFT JOIN T_Sys_Refe r ON so.PlanSort = r.Value
                                              AND r.RefeTypeID = 14
          WHERE     so.sOrderID = ( SELECT TOP 1
                                            fo.sOrderID
                                    FROM    T_Stu_sFeesOrder fo
                                    WHERE   fo.Status = 1
                                            AND fo.sFeeID = f.sFeeID
                                  )
        ) 报读类别 ,
        ( SELECT TOP 1
                    o.NumName
          FROM      T_Stu_sOrder o
                    LEFT JOIN T_Stu_sFeesOrder fo ON fo.sOrderID = o.sOrderID
          WHERE     fo.sFeeID = f.sFeeID
        ) 缴费次数 ,
        f.ShouldMoney 应缴金额 ,
        ( SELECT    ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sFeesOrder
          WHERE     sFeeID = f.sFeeID
                    AND Status = 1
        ) 实缴金额 ,
        ( SELECT    ISNULL(SUM(DiscountMoney), 0)
          FROM      T_Stu_sFeesOrder
          WHERE     sFeeID = f.sFeeID
                    AND Status = 1
        ) 优惠金额 ,
        ( SELECT    ISNULL(SUM(Money), 0.00)
          FROM      T_Stu_sOffset
          WHERE     RelatedID IN ( SELECT   sFeesOrderID
                                   FROM     T_Stu_sFeesOrder
                                   WHERE    sFeeID = f.sFeeID
                                            AND Status = 1 )
                    AND Status = 1
        ) 充抵金额 ,
        ( SELECT    ISNULL(SUM(Money), 0.00)
          FROM      T_Stu_sOffset
          WHERE     ByRelatedID IN ( SELECT sFeesOrderID
                                     FROM   T_Stu_sFeesOrder
                                     WHERE  sFeeID = f.sFeeID
                                            AND Status = 1 )
                    AND BySort = 1
                    AND Status = 1
        ) 被充抵金额 ,
        ( SELECT    ISNULL(SUM(RefundMoney), 0.00)
          FROM      T_Stu_sRefund
          WHERE     sFeesOrderID IN ( SELECT    sFeesOrderID
                                      FROM      T_Stu_sFeesOrder
                                      WHERE     sFeeID = f.sFeeID
                                                AND Status = 1 )
                    AND Status = 1
        ) 核销金额 ,
        r2.RefeName 缴费方式 ,
        u1.Name 收费人 ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) 收费时间 ,
        u.Name 结账人 ,
        CASE CONVERT(NVARCHAR(23), f.AffirmTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(23), f.AffirmTime, 23)
        END 结账时间 ,
        d.Name 收费单位 ,
        da.Name 报名校区 ,
        s.Address 地址 ,
        r1.RefeName 状态 ,
        REPLACE(f.Explain, '<br />', ' ') 说明 ,
        REPLACE(f.Remark, '<br />', ' ') 备注
FROM    T_Stu_sFee f
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                   AND r1.RefeTypeID = 7
        LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                   AND r2.RefeTypeID = 6
        LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
        LEFT JOIN T_Sys_User u1 ON u1.UserID = f.CreateID
        LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
        LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
        LEFT JOIN T_Pro_DeptArea da ON da.DeptAreaID = ep.DeptAreaID
WHERE   1 = 1
        {0}
ORDER BY f.sFeeID DESC";
            string filename = "学费收费信息.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "f.DeptID", "f.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }

        /// <summary>
        /// /重置打印次数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AjaxResult GetsFeeResetPrintNum(string ID)
        {
            if (sFeeBLL.UpdatesFeePrintNum(ID, "0", this.UserId) > 0)
                return AjaxResult.Success("", "success");
            else
                return AjaxResult.Error("出现未知错误，请联系管理员");
        }
        /// <summary>
        /// 变更状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public AjaxResult GetUpdatesFeeStatus(string ID, string Value)
        {
            if (sFeeBLL.UpdatesFeeStatus(ID, Value, this.UserId) > 0)
                return AjaxResult.Success("", "success");
            else
                return AjaxResult.Error("出现未知错误，请联系管理员");
        }
        /// <summary>
        /// 修改时查询，表单赋值
        /// </summary>
        /// <param name="sFeeID"></param>
        /// <returns></returns>
        public AjaxResult GetsFeeInfomation(string sFeeID)
        {
            return AjaxResult.Success(JsonHelper.DataTableToJson(sFeeBLL.SelectsFeeInfo(sFeeID)), "");
        }

        /// <summary>
        /// 获取收配品信息
        /// </summary>
        /// <param name="ID">收费信息编号</param>
        /// <returns></returns>
        public AjaxResult GetsFeeOrder(string ID)
        {
            return AjaxResult.Success(JsonHelper.DataTableToJson(sFeesOrderGiveBLL.GetFeeOrderGive(ID)), "");
        }
        /// <summary>
        /// 修改收费信息
        /// </summary>
        /// <param name="fefm"></param>
        /// <returns></returns>
        public AjaxResult GetMondfiysFee(sFeeEditFormModel fefm)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                if (string.IsNullOrEmpty(fefm.editsFeeID))
                    return AjaxResult.Error("出现未知错误，请联系管理员");

                if (!string.IsNullOrEmpty(fefm.editExplain))
                    fefm.editExplain = fefm.editExplain.Replace("\r\n", "<br />");
                else
                    fefm.editExplain = "";

                if (!string.IsNullOrEmpty(fefm.editRemark))
                    fefm.editRemark = fefm.editRemark.Replace("\r\n", "<br />");
                else
                    fefm.editRemark = "";
                try
                {
                    fefm.editGiveJsonData = "{\"rows\":" + fefm.editGiveJsonData + "}";
                    sGiveListModel give = JsonConvert.DeserializeObject<sGiveListModel>(fefm.editGiveJsonData);
                    sFeesOrderListModel order = JsonConvert.DeserializeObject<sFeesOrderListModel>(fefm.editOrderJsonData);
                    if (order.rows.Count == 0)
                    {
                        return AjaxResult.Error("数据异常，请联系管理员");
                    }
                    foreach (var or in order.rows)
                    {
                        if (or.PaidMoney + or.DiscountMoney + or.OffsetMoney > or.ShouldMoney)
                            return AjaxResult.Error("" + or.DetailName + ": 实收+充抵+优惠 不能大于应收金额" + or.ShouldMoney + "元");
                        if (sRefundBLL.GetRefundCount(or.sFeesOrderID) > 0)
                        {
                            return AjaxResult.Error("" + or.DetailName + "已经被核销，不能修改");
                        }
                    }

                    string where = "  and sFeeID=@sFeeID ";
                    SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@sFeeID",fefm.editsFeeID)
                    };

                    sFeeModel fm = sFeeBLL.sFeeModelByWhere(where, paras);
                    fm.PaidMoney = order.rows.Sum(O => O.PaidMoney + O.DiscountMoney + O.OffsetMoney).ToString();
                    fm.FeeMode = fefm.editFeeMode;
                    fm.FeeTime = fefm.editFeeTime;
                    fm.Explain = fefm.editExplain;
                    fm.Remark = fefm.editRemark;
                    fm.UpdateID = this.UserId.ToString();
                    fm.UpdateTime = DateTime.Now.ToString();
                    sFeeBLL.UpdatesFee(fm);//修改收费
                    DataTable giveDT = sFeesOrderGiveBLL.sFeesOrderGiveTableByWhere(fm.sFeeID);//获取配品dataTable
                    foreach (DataRow dr in giveDT.Rows)
                    {
                        UpdatesOrderGiveStatus(dr["sOrderGiveID"].ToString(), "1");//修改订单配品状态
                    }
                    sFeesOrderGiveBLL.UpdatesFeesOrderGiveStatus(fm.sFeeID, UserId, "2");
                    foreach (var item in give.rows)//新增新的配品
                    {
                        sFeesOrderGiveModel fogm = new sFeesOrderGiveModel();
                        fogm.sFeeID = fm.sFeeID;
                        fogm.CreateID = UserId.ToString();
                        fogm.CreateTime = DateTime.Now.ToString();
                        fogm.sOrderGiveID = item.sOrderGiveID;
                        fogm.Status = "1";
                        fogm.UpdateID = UserId.ToString();
                        fogm.UpdateTime = DateTime.Now.ToString();
                        sFeesOrderGiveBLL.InsertsFeesOrderGive(fogm);
                        UpdatesOrderGiveStatus(item.sOrderGiveID, "2");//修改配品状态
                    }
                    foreach (var item in order.rows)
                    {
                        List<sFeeOffset> off = JsonConvert.DeserializeObject<List<sFeeOffset>>(item.OffsetDetail);//修改后的充抵

                        sFeesOrderModel fom = sFeesOrderBLL.GetsFeeOrderModel(item.sFeesOrderID);
                        if (Convert.ToDecimal(fom.PaidMoney) != item.PaidMoney || Convert.ToDecimal(fom.DiscountMoney) != item.DiscountMoney || off.Count > 0)
                        {
                            decimal byOffsetMoney = sOffsetBLL.GetBysOffsetMoney(fom.sFeesOrderID);
                            decimal offsetMoney = sOffsetBLL.GetsOffsetMoney(fom.sFeesOrderID);
                            decimal temp = (item.OffsetMoney + item.PaidMoney + item.DiscountMoney) - (Convert.ToDecimal(fom.PaidMoney) + Convert.ToDecimal(fom.DiscountMoney) + offsetMoney - byOffsetMoney);//计算修改前后的金额差
                            decimal tempPaidMoney = Convert.ToDecimal(fom.PaidMoney);
                            decimal tempDiscountMoney = Convert.ToDecimal(fom.DiscountMoney);

                            decimal tempOffsetMoney = off.Sum(O => O.Money);

                            fom.DiscountMoney = item.DiscountMoney.ToString();
                            fom.PaidMoney = item.PaidMoney.ToString();
                            fom.CanMoney = (item.PaidMoney + item.OffsetMoney).ToString();
                            fom.UpdateID = UserId.ToString();
                            fom.UpdateTime = DateTime.Now.ToString();
                            if (tempPaidMoney != item.PaidMoney || tempDiscountMoney != item.DiscountMoney || tempOffsetMoney != offsetMoney)
                            {
                                sOrderBLL.UpdatesOderPaidMoney(temp, fom.sOrderID, UserId);//变更订单的实缴金额
                                // sFeeBLL.UpdatePaidMoney(fom.sFeeID, temp, UserId);
                            }

                            sFeesOrderBLL.UpdatesFeesOrder(fom);//修改收费明细
                        }

                        if (off.Count > 0)
                        {
                            string whereoff = " and RelatedID=@RelatedID  and Status=1 ";
                            SqlParameter[] parasoff = new SqlParameter[] {
                                    new SqlParameter("@RelatedID",item.sFeesOrderID)
                                  };
                            DataTable dt = sOffsetBLL.sOffsetTableByWhere(whereoff, parasoff, "");
                            foreach (DataRow dr in dt.Rows)
                            {
                                decimal tempMoney = -decimal.Parse(dr["Money"].ToString());
                                string tempId = dr["ByRelatedID"].ToString();
                                UpdateCanMoney(tempMoney, tempId, dr["BySort"].ToString());  //修改被冲抵的收费明细的可用金额
                                if (dr["BySort"].ToString().Equals("1"))
                                {
                                    sFeesOrderModel sfom = sFeesOrderBLL.GetsFeeOrderModel(tempId);
                                    sFeeBLL.UpdatePaidMoney(sfom.sFeeID, -tempMoney, UserId);
                                    sOrderBLL.UpdatesOderPaidMoney(-tempMoney, sfom.sOrderID, UserId);
                                }
                            }

                            sOffsetBLL.UpdatasOffsetStatus("2", item.sFeesOrderID, UserId);//停用此收费项以前的充抵项

                            foreach (var im in off)
                            {
                                sOffsetModel om = new sOffsetModel();
                                om.RelatedID = item.sFeesOrderID;
                                om.BySort = im.Sort;
                                om.ByRelatedID = im.ID;
                                om.CreateID = UserId.ToString();
                                om.CreateTime = DateTime.Now.ToString();
                                om.UpdateID = UserId.ToString();
                                om.UpdateTime = DateTime.Now.ToString();
                                om.Status = "1";
                                om.Money = im.Money.ToString();
                                sOffsetBLL.InsertsOffset(om);//添加新的充抵项
                                decimal tempMoney = im.Money;
                                UpdateCanMoney(tempMoney, im.ID, im.Sort);//修改可用金额
                                if (im.Sort.Equals("1"))
                                {
                                    sFeesOrderModel sfom = sFeesOrderBLL.GetsFeeOrderModel(im.ID);
                                    sOrderBLL.UpdatesOderPaidMoney(-tempMoney, sfom.sOrderID, UserId);//变更订单的实缴金额
                                    sFeeBLL.UpdatePaidMoney(sfom.sFeeID, -im.Money, UserId);
                                }


                            }
                        }
                    }
                    ts.Complete();
                    return AjaxResult.Success("修改成功！", "success");
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    return AjaxResult.Error(ex.Message);
                }
                finally
                {
                    ts.Dispose();
                }
            }
        }
        /// <summary>
        /// 查看收费详细信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AjaxResult SelectsFeeView(string ID)
        {
            return AjaxResult.Success(JsonHelper.DataTableToJson(sFeeBLL.SelectsFeeView(ID)), "success");
        }

        /// <summary>
        /// 结账/反结账
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public AjaxResult UpdatesFeeAffirm(string ID, string Value)
        {
            if (string.IsNullOrEmpty(ID))
                return AjaxResult.Error("出现未知错误，请联系管理员");
            string[] array = ID.Split(",");
            for (int i = 0; i < array.Length; i++)
            {
                sFeeModel fm = sFeeBLL.GetsFeeModel(array[i]);
                if (fm.Status.Equals(Value))
                {
                    return AjaxResult.Error("" + fm.VoucherNum + "变更状态和当前状态相同");
                }
            }
            string arrfirmTime = "1900-01-01";
            int arrfirmId = 0;
            if (Value.Equals("2"))
            {
                arrfirmTime = DateTime.Now.ToString();
                arrfirmId = this.UserId;
            }

            if (sFeeBLL.UpdatesFeeAffirm(ID, UserId, Value, arrfirmTime, arrfirmId) > 0)
                return AjaxResult.Success("", "success");
            else
                return AjaxResult.Error("出现未知错误，请联系管理员");
        }

        /// <summary>
        /// 修改可用金额
        /// </summary>
        /// <param name="money">金额</param>
        /// <param name="relatedId">相关id</param>
        /// <param name="userId">修改人</param>
        /// <param name="sort">充抵分类:1学费;2:杂费;3:证书</param>
        private void UpdateCanMoney(decimal money, string relatedId, string sort)
        {
            //修改被冲抵的收费明细的可用金额
            if (sort.Equals("1"))//学费
                sFeesOrderBLL.UpdatasFeesOrderCanMoney(money, relatedId, UserId);
            else if (sort.Equals("2"))//杂费
                iFeeBLL.UpdateiFeeCanMoney(money, relatedId, UserId);
            else if (sort.Equals("3"))//证书
                FeeDetailBLL.UpdateFeeDetailCanMoney(money, UserId, relatedId);
        }
        /// <summary>
        /// 变更订单配品领取状态
        /// </summary>
        /// <param name="sOrderGiveId">订单配品id</param>
        /// <param name="status">状态</param>
        private void UpdatesOrderGiveStatus(string sOrderGiveId, string status)
        {
            string where = " and sOrderGiveID=@sOrderGiveID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sOrderGiveID",sOrderGiveId)
            };
            sOrderGiveModel ogm = sOrderGiveBLL.sOrderGiveModelByWhere(where, paras);
            ogm.Status = status;
            ogm.UpdateID = this.UserId.ToString();
            ogm.UpdateTime = DateTime.Now.ToString();
            sOrderGiveBLL.UpdatesOrderGive(ogm);
        }
        /// <summary>
        /// 获取打印信息
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetPrintInfo()
        {
            string feeId = Request.Form["ID"];

            string where = " AND sFeeID IN (" + feeId + ")";
            SqlParameter[] paras = new SqlParameter[] {
            };
            ConfigModel c = ConfigBLL.ConfigModelByWhere(" AND ConfigID = 3", null);
            int printNum = int.Parse(c.PrintNum);
            DataTable dt = sFeeBLL.GetsFeePrintContent(feeId);
            string errorString = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (int.Parse(dr["PrintNum"].ToString()) >= printNum)
                {
                    errorString += "" + dr["VoucherNum"].ToString() + "打印次数超过" + printNum + "次 \n";
                }
            }
            if (!string.IsNullOrEmpty(errorString))
                return AjaxResult.Error(errorString);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt), "success");
        }
        public AjaxResult GetPrintInfoNew(string sfeeId, bool isRePrint)
        {

            string where = " AND sFeeID IN (" + sfeeId + ")";
            ConfigModel c = ConfigBLL.ConfigModelByWhere(" AND ConfigID = 3", null);
            int printNum = int.Parse(c.PrintNum);
            DataTable dt = sFeeBLL.GetPrintsFeeContent(sfeeId);

            string errorString = "";
            if (!isRePrint)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (int.Parse(dr["PrintNum"].ToString()) >= printNum)
                    {
                        errorString += "" + dr["VoucherNum"].ToString() + "打印次数超过" + printNum + "次 \n";
                    }
                }
            }
            if (!string.IsNullOrEmpty(errorString))
                return AjaxResult.Error(errorString);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt), "success");
        }

        /// <summary>
        /// 获取票据号
        /// </summary>
        /// <returns></returns>
        public AjaxResult UpdateNoteNum()
        {
            string sfeeId = Request.Form["sFeeID"];

            sFeeModel fm = sFeeBLL.GetsFeeModel(sfeeId);

            if (!string.IsNullOrEmpty(fm.NoteNum))
            {
                sDisableNoteModel dnm = new sDisableNoteModel();
                dnm.Status = "1";
                dnm.sFeeID = fm.sFeeID;
                dnm.NoteNum = fm.NoteNum;
                dnm.CreateID = this.UserId.ToString();
                dnm.CreateTime = DateTime.Now.ToString();
                dnm.UpdateID = this.UserId.ToString();
                dnm.UpdateTime = DateTime.Now.ToString();
                sDisableNoteBLL.InsertsDisableNote(dnm);
            }

            string printNumStr = ConfigBLL.getNoteNum("3", "3");
            fm.NoteNum = printNumStr;
            fm.PrintNum = (Convert.ToInt32(fm.PrintNum) + 1).ToString();

            sFeeBLL.UpdatesFee(fm);

            return AjaxResult.Success(printNumStr, "success");
        }
        /// <summary>
        /// 获取打印数据，验证打印次数
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetRePrintInfo()
        {
            string feeId = Request.Form["ID"];

            string where = " AND sFeeID IN (" + feeId + ")";
            SqlParameter[] paras = new SqlParameter[] {
            };
            DataTable dt = sFeeBLL.GetsFeePrintContent(feeId);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt), "success");
        }
        public AjaxResult GetPrintInfoNoPrintNum(string ID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return AjaxResult.Error("请选择要打印的收费项目");
            }

            string where = " AND sFeeID IN (" + ID + ")";
            SqlParameter[] paras = new SqlParameter[] {
            };
            ConfigModel c = ConfigBLL.ConfigModelByWhere(" AND ConfigID = 3", null);
            int printNum = int.Parse(c.PrintNum);
            DataTable dt = sFeeBLL.GetsFeePrintContent(ID);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt), "success");
        }
        /// <summary>
        /// 快速充抵
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public ActionResult GetChoosesFeeList()
        {
            string where = "";
            string MenuID = Request.Form["MenuID"];
            string studName = Request.Form["studName"];
            string voucherNum = Request.Form["voucherNum"];
            string idCard = Request.Form["idCard"];
            string major= Request.Form["major"];
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and f.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(major))
            {
                where += " and p.Name like '%" + major + "%'";
            }
            if (!string.IsNullOrEmpty(studName))
            {
                where += " and s.Name like '%" + studName + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and s.IDCard like '%" + idCard + "%'";
            }
            string cmdText = @"SELECT  fo.sFeesOrderID ID ,
        d.Name DeptName ,
        f.VoucherNum ,
        f.NoteNum ,
        s.Name StudName ,
        e.EnrollNum ,
        p.Name ProName ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        s.IDCard ,
        dl.Name FeeContent ,
        fo.CanMoney ,
        0.00 Money ,
        o.NumName ,
        '1' Sort
FROM    T_Stu_sFeesOrder fo
        LEFT JOIN T_Stu_sFee f ON f.sFeeID = fo.sFeeID
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
        LEFT JOIN dbo.T_Stu_sOrder o ON o.sOrderID = fo.sOrderID
        LEFT JOIN dbo.T_Pro_Detail dl ON dl.DetailID = o.DetailID
        LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
        LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
WHERE   fo.Status = 1 {0}";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "f.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        /// <summary>
        /// 验证模板返回数据
        /// </summary>
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
                if (!dt.Columns[0].ColumnName.Trim().Equals("姓名") || !dt.Columns[1].ColumnName.Trim().Equals("身份证号")
                    || !dt.Columns[2].ColumnName.Trim().Equals("学历层次") || !dt.Columns[3].ColumnName.Trim().Equals("专业")
                    || !dt.Columns[4].ColumnName.Trim().Equals("缴费学年") || !dt.Columns[5].ColumnName.Trim().Equals("费用类别")
                    || !dt.Columns[6].ColumnName.Trim().Equals("缴费类别") || !dt.Columns[7].ColumnName.Trim().Equals("供款金额")
                    || !dt.Columns[8].ColumnName.Trim().Equals("缴费方式") || !dt.Columns[9].ColumnName.Trim().Equals("打印说明")
                    || !dt.Columns[10].ColumnName.Trim().Equals("备注"))
                {
                    return AjaxResult.Error("模板错误");
                }
            }
            catch (Exception ex)
            {

                return AjaxResult.Error(ex.Message);
            }
            return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "success");
        }
        /// <summary>
        /// 导入学费收费
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Dept"></param>
        /// <param name="Paras"></param>
        /// <returns></returns>
        public AjaxResult GetsFeeUpload(string filePath, string Dept, string Paras)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return AjaxResult.Error("请选择文件");
                if (string.IsNullOrEmpty(Dept))
                    return AjaxResult.Error("请选择校区");

                List<UploadsFeeCHModel> list = JsonConvert.DeserializeObject<List<UploadsFeeCHModel>>(Paras);
                if (list == null || list.Count == 0)
                {
                    return AjaxResult.Error("不能导入空数据");
                }
                DataTable dt = TableTitle(new DataTable());
                decimal successNum = 0;
                decimal errorNum = 0;
                foreach (var item in list)
                {
                    sOrderModel om = new sOrderModel();
                    UploadsFeeModel ufm = GetUploadsFeeModel(item);
                    string feeModel = string.Empty;
                    string errorString = Validate(ufm, Dept, ref om, ref feeModel);
                    if (string.IsNullOrEmpty(errorString) && !string.IsNullOrEmpty(om.sOrderID))
                    {
                        sFeeModel fm = new sFeeModel();
                        fm.AffirmID = "0";
                        fm.AffirmTime = "1900-01-01";
                        fm.CreateID = UserId.ToString();
                        fm.CreateTime = DateTime.Now.ToString();
                        fm.UpdateID = UserId.ToString();
                        fm.UpdateTime = DateTime.Now.ToString();
                        fm.DeptID = Dept;
                        fm.Explain = ufm.printRemark;
                        fm.FeeMode = feeModel;
                        fm.FeeTime = DateTime.Now.ToString();
                        fm.NoteNum = "";
                        fm.NumItemID = om.NumItemID;
                        fm.ShouldMoney = om.ShouldMoney;
                        fm.PaidMoney = ufm.money;
                        fm.PlanItemID = om.PlanItemID;
                        fm.PrintNum = "0";
                        fm.Remark = ufm.remark;
                        fm.sEnrollsProfessionID = om.sEnrollsProfessionID;
                        fm.Status = "1";
                        fm.VoucherNum = ConfigBLL.getVoucherNum("3", "X");
                        fm.sFeeID = sFeeBLL.InsertsFee(fm).ToString();
                        sFeesOrderModel fom = new sFeesOrderModel();
                        fom.CreateID = UserId.ToString();
                        fom.CreateTime = DateTime.Now.ToString();
                        fom.UpdateID = UserId.ToString();
                        fom.UpdateTime = DateTime.Now.ToString();
                        fom.ShouldMoney = om.ShouldMoney;
                        if (ufm.type.Equals("优惠"))
                        {
                            fom.DiscountMoney = ufm.money;
                            fom.PaidMoney = "0";
                            fom.CanMoney = "0";

                        }
                        else if (ufm.type.Equals("实收"))
                        {
                            fom.DiscountMoney = "0";
                            fom.PaidMoney = ufm.money;
                            fom.CanMoney = ufm.money;
                        }
                        fom.sOrderID = om.sOrderID;
                        fom.Status = "1";
                        fom.sFeeID = fm.sFeeID;
                        sFeesOrderBLL.InsertsFeesOrder(fom);
                        sOrderBLL.UpdatesOderPaidMoney(Convert.ToDecimal(ufm.money), fom.sOrderID, UserId);//更新订单的已缴金额
                        successNum++;

                    }
                    else
                    {
                        errorNum++;
                        item.系统备注 = errorString;
                        dt = TableRow(dt, item);
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
                nm.InFile = filePath;
                nm.OutFile = url;
                nm.Sort = "10";
                nm.DeptID = Dept;
                nm.Status = "1";
                nm.SuccessNum = successNum.ToString();
                nm.ErrorNum = errorNum.ToString();
                NoteBLL.InsertNote(nm);
                return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "操作成功");
            }
            catch (Exception)
            {

                return AjaxResult.Error("系统错误，请联系管理员");
            }

        }
        /// <summary>
        /// 错误文件Table
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable TableTitle(DataTable dt)
        {
            dt.Columns.Add("姓名", Type.GetType("System.String"));
            dt.Columns.Add("身份证号", Type.GetType("System.String"));
            dt.Columns.Add("学历层次", Type.GetType("System.String"));
            dt.Columns.Add("专业", Type.GetType("System.String"));
            dt.Columns.Add("缴费学年", Type.GetType("System.String"));
            dt.Columns.Add("费用类别", Type.GetType("System.String"));
            dt.Columns.Add("缴费类别", Type.GetType("System.String"));
            dt.Columns.Add("供款金额", Type.GetType("System.String"));
            dt.Columns.Add("缴费方式", Type.GetType("System.String"));
            dt.Columns.Add("打印说明", Type.GetType("System.String"));
            dt.Columns.Add("备注", Type.GetType("System.String"));
            dt.Columns.Add("系统备注", Type.GetType("System.String"));
            return dt;
        }
        /// <summary>
        /// 错误文件Rows
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="em"></param>
        /// <returns></returns>
        private DataTable TableRow(DataTable dt, UploadsFeeCHModel em)
        {
            DataRow dr = dt.NewRow();
            dr["姓名"] = em.姓名;
            dr["身份证号"] = em.身份证号;
            dr["学历层次"] = em.学历层次;
            dr["专业"] = em.专业;
            dr["缴费学年"] = em.缴费学年;
            dr["费用类别"] = em.费用类别;
            dr["缴费类别"] = em.缴费类别;
            dr["供款金额"] = em.供款金额;
            dr["缴费方式"] = em.缴费方式;
            dr["打印说明"] = em.打印说明;
            dr["备注"] = em.备注;
            dr["系统备注"] = em.系统备注;
            dt.Rows.Add(dr);
            return dt;
        }
        /// <summary>
        /// 实体类转换
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public UploadsFeeModel GetUploadsFeeModel(UploadsFeeCHModel em)
        {
            UploadsFeeModel cpm = new UploadsFeeModel();
            cpm.IdCard = em.身份证号.Replace(" ", "");
            cpm.name = em.姓名.Replace(" ", "");
            cpm.studyLevel = em.学历层次.Replace(" ", "");
            cpm.major = em.专业.Replace(" ", "");
            cpm.year = em.缴费学年.Replace(" ", "");
            cpm.detail = em.费用类别.Replace(" ", "");
            cpm.type = em.缴费类别.Replace(" ", "");
            cpm.money = em.供款金额.Replace(" ", "");
            cpm.feeMode = em.缴费方式.Replace(" ", "");
            cpm.printRemark = em.打印说明.Replace(" ", "");
            cpm.remark = em.备注.Replace(" ", "");
            return cpm;

        }
        /// <summary>
        /// 验证导入收费字段
        /// </summary>
        /// <param name="ufm"></param>
        /// <param name="deptId"></param>
        /// <param name="om"></param>
        /// <returns></returns>
        private string Validate(UploadsFeeModel ufm, string deptId, ref sOrderModel om, ref string feeModel)
        {
            string errorString = string.Empty;
            string studentId = string.Empty;
            if (string.IsNullOrEmpty(ufm.name))
            {
                errorString += "姓名不能为空;";
            }
            else
            {
                if (ufm.name.Length > 16)
                {
                    errorString += "姓名不能超过16个字符;";
                }
            }

            if (!OtherHelper.CheckIDCard(ufm.IdCard))
            {
                errorString += "身份证号不规范;";
            }
            else
            {
                StudentModel sm = StudentBLL.GetStudentModel(ufm.IdCard);
                if (!string.IsNullOrEmpty(sm.Name))
                {
                    if (!ufm.name.Equals(sm.Name))
                    {
                        errorString += "姓名和身份证号不匹配;";
                    }
                }
                studentId = sm.StudentID;
            }
            if (ufm.type != "实收" && ufm.type != "优惠")
            {
                errorString += "缴费类别必须是实收或者优惠;";
            }
            if (!OtherHelper.IsDecimal(ufm.money))
            {
                errorString += "供款金额只能是数字;";
            }
            else
            {
                if (Convert.ToDecimal(ufm.money) <= 0)
                {
                    errorString += "供款金额必须大于等于0;";
                }
            }
            feeModel = RefeBLL.GetRefeValue(ufm.feeMode, "6");
            if (feeModel.Equals("-1"))
            {
                errorString += "缴费方式不存在;";
            }
            string studyLevel = RefeBLL.GetRefeValue(ufm.studyLevel, "17");
            if (studyLevel.Equals("-1"))
            {
                errorString += "学习层次不存在;";
            }

            if (string.IsNullOrEmpty(ufm.year))
            {
                errorString += "缴费学年不能为空;";
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
                    errorString += "此学生多次报名，请手动收费;";

                }
                else if (enrollTab.Rows.Count == 0)
                {
                    errorString += "此学生未报名;";
                }
                else
                {
                    //string sprofessionId = sProfessionBLL.GetsProfesssionID(ufm.major, deptId);
                    //if (string.IsNullOrEmpty(sprofessionId))
                    //{
                    //    errorString += "专业不存在;";
                    //}
                    if (string.IsNullOrEmpty(errorString))
                    {
                        DataTable senrollPro = sEnrollsProfessionBLL.GetsEnrollsProfessionTable(studyLevel, enrollTab.Rows[0]["sEnrollID"].ToString(), ufm.major, deptId);
                        if (senrollPro.Rows.Count > 1)
                        {
                            errorString += "此学生已经多次报名此专业，请手动收费;";
                        }
                        else if (senrollPro.Rows.Count == 0)
                        {
                            errorString += "此学生没有报过该层次下的任何专业;";
                        }
                        else
                        {
                            om = sOrderBLL.GetsOrderModel(senrollPro.Rows[0]["sEnrollsProfessionID"].ToString(), ufm.year, ufm.detail);
                            if (!string.IsNullOrEmpty(om.sOrderID))
                            {
                                if (om.Status.Equals("3"))
                                {
                                    errorString += "此缴费单已交清;";
                                }
                                if (Convert.ToDecimal(om.ShouldMoney) - Convert.ToDecimal(om.PaidMoney) < Convert.ToDecimal(ufm.money))
                                {
                                    errorString += "缴费金额超过此订单的金额;";
                                }
                            }
                            else
                            {
                                errorString += "此缴费单不存在;";
                            }
                        }
                    }

                }
            }
            return errorString;
        }
        /// <summary>
        /// 删除收费
        /// </summary>
        /// <param name="sfeeId"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult DeletesFee(string sfeeId)
        {
            if (string.IsNullOrEmpty(sfeeId))
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    sFeeModel fm = sFeeBLL.GetsFeeModel(sfeeId);
                    DataTable sfeeOrder = sFeesOrderBLL.GetsFeeOrderTable(sfeeId);
                    foreach (DataRow dr in sfeeOrder.Rows)
                    {
                        if (sOffsetBLL.GetsOffsetCount(dr["sFeesOrderID"].ToString()) > 0)
                        {
                            return AjaxResult.Error("该收费的收费明细已经有充抵或者被冲抵，不能删除");
                        }
                        if (sRefundBLL.GetRefundCount(dr["sFeesOrderID"].ToString()) > 0)
                        {
                            return AjaxResult.Error("该收费的收费明细已经被核销，不能删除");
                        }
                    }
                    foreach (DataRow dr in sfeeOrder.Rows)
                    {
                        decimal paidMoney = Convert.ToDecimal(dr["PaidMoney"].ToString()) + Convert.ToDecimal(dr["DiscountMoney"].ToString());
                        sOrderBLL.UpdatesOderPaidMoney(-paidMoney, dr["sOrderID"].ToString(), UserId);//更新订单的已缴金额
                        sFeesOrderBLL.DeletesFeesOrder(dr["sFeesOrderID"].ToString());//删除收费明细


                    }
                    sFeeBLL.DeletesFee(sfeeId);
                    ts.Complete();
                    return AjaxResult.Success("", "删除成功");
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    return AjaxResult.Error(ex.Message);
                }
                finally
                {
                    ts.Dispose();
                }
            }

        }
        /// <summary>
        /// 删除收费明细
        /// </summary>
        /// <param name="sfeesOrderId"></param>
        /// <returns></returns>
        //[HttpPost]
        //public AjaxResult DeletesFeesOrder(string sfeesOrderId)
        //{

        //    if (string.IsNullOrEmpty(sfeesOrderId))
        //    {
        //        return AjaxResult.Error("出现未知错误，请联系管理员");
        //    }
        //    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
        //    {
        //        try
        //        {
        //            if (sOffsetBLL.GetsOffsetCount(sfeesOrderId) > 0)
        //            {
        //                return AjaxResult.Error("该收费的收费明细已经有充抵或者被冲抵，不能删除");
        //            }
        //            if (sRefundBLL.GetRefundCount(sfeesOrderId) > 0)
        //            {
        //                return AjaxResult.Error("该收费的收费明细已经被核销，不能删除");
        //            }
        //            sFeesOrderModel fom = sFeesOrderBLL.GetsFeeOrderModel(sfeesOrderId);
        //            decimal paidMoney = Convert.ToDecimal(fom.PaidMoney) + Convert.ToDecimal(fom.DiscountMoney);
        //            sOrderBLL.UpdatesOderPaidMoney(-paidMoney, fom.sOrderID, UserId);//更新订单的已缴金额
        //            sFeesOrderBLL.DeletesFeesOrder(sfeesOrderId);//删除收费明细

        //            sFeeModel fm = sFeeBLL.GetsFeeModel(fom.sFeeID);
        //            fm.PaidMoney = (Convert.ToDecimal(fm.PaidMoney) - paidMoney).ToString();
        //            fm.UpdateID = UserId.ToString();
        //            fm.UpdateTime = DateTime.Now.ToString();
        //            sFeeBLL.UpdatesFee(fm);//修改收费实缴金额
        //            ts.Complete();
        //            return AjaxResult.Success("", "删除成功");
        //        }
        //        catch (Exception ex)
        //        {

        //            Transaction.Current.Rollback();
        //            return AjaxResult.Error(ex.Message);
        //        }
        //        finally
        //        {
        //            ts.Dispose();
        //        }
        //    }
        //}

        /// <summary>
        /// 删除充抵项
        /// </summary>
        /// <param name="sfeesOrderId"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult DeletesOffset(string sfeesOrderId)
        {
            if (string.IsNullOrEmpty(sfeesOrderId))
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    if (sOffsetBLL.GetsByOffsetCount(sfeesOrderId) > 0)
                    {
                        return AjaxResult.Error("该收费项已经被冲抵，不能删除");
                    }
                    if (sOffsetBLL.GetsOffsetCount(sfeesOrderId) == 0)
                    {
                        return AjaxResult.Error("该收费项没有充抵项，不能删除");
                    }
                    sFeesOrderModel fom = sFeesOrderBLL.GetsFeeOrderModel(sfeesOrderId);

                    decimal paidMoney = sOffsetBLL.GetsOffsetMoney(sfeesOrderId);

                    sOrderBLL.UpdatesOderPaidMoney(-paidMoney, fom.sOrderID, UserId);//更新订单的已缴金额

                    string where = " and RelatedID=@RelatedID and Status=1";
                    SqlParameter[] paras = new SqlParameter[] {
                        new SqlParameter("@RelatedID",sfeesOrderId)
                    };
                    DataTable dt = sOffsetBLL.sOffsetTableByWhere(where, paras, "");

                    foreach (DataRow dr in dt.Rows)
                    {
                        UpdateCanMoney(-Convert.ToDecimal(dr["Money"].ToString()), dr["ByRelatedID"].ToString(), dr["BySort"].ToString());

                        if (dr["BySort"].ToString().Equals("1"))//被冲抵的是否是学费
                        {
                            sFeesOrderModel sfom = sFeesOrderBLL.GetsFeeOrderModel(dr["ByRelatedID"].ToString());
                            sFeeBLL.UpdatePaidMoney(sfom.sFeeID, Convert.ToDecimal(dr["Money"].ToString()), UserId);
                            string where1 = " and ByRelatedID=@ByRelatedID and BySort=1 and Status=1 and RelatedID=@RelatedID ";
                            SqlParameter[] paras1 = new SqlParameter[] {
                                new SqlParameter("@ByRelatedID", dr["ByRelatedID"].ToString()),
                                new SqlParameter("@RelatedID", dr["RelatedID"].ToString())
                            };
                            sOffsetModel om = sOffsetBLL.sOffsetModelByWhere(where1, paras1);
                            sFeesOrderModel byfom = sFeesOrderBLL.GetsFeeOrderModel(om.ByRelatedID);

                            sOrderBLL.UpdatesOderPaidMoney(decimal.Parse(om.Money), byfom.sOrderID, UserId);

                        }
                        else if (dr["BySort"].ToString().Equals("3"))
                        {
                            string feeId = FeeDetailBLL.GetFeeDetailModel(dr["ByRelatedID"].ToString()).FeeID;
                            FeeBLL.UpdateFeePaidMoney(feeId, Convert.ToDecimal(dr["Money"].ToString()), UserId);
                        }

                    }

                    sOffsetBLL.UpdatasOffsetStatus("2", sfeesOrderId, UserId);//停用充抵项
                    UpdateCanMoney(paidMoney, sfeesOrderId, "1");
                    sFeeModel fm = sFeeBLL.GetsFeeModel(fom.sFeeID);
                    fm.PaidMoney = (Convert.ToDecimal(fm.PaidMoney) - paidMoney).ToString();
                    fm.UpdateID = UserId.ToString();
                    fm.UpdateTime = DateTime.Now.ToString();
                    sFeeBLL.UpdatesFee(fm);//修改收费实缴金额
                    ts.Complete();
                    return AjaxResult.Success("", "删除成功");
                }
                catch (Exception ex)
                {

                    Transaction.Current.Rollback();
                    return AjaxResult.Error(ex.Message);
                }
                finally
                {
                    ts.Dispose();
                }
            }
        }

        public ActionResult GetsFeeOrderList(string sFeeID)
        {
            string cmdText = @"SELECT
         fo.sFeesOrderID ,
        d.Name DetailName,
        fo.ShouldMoney ,
        fo.PaidMoney ,
        fo.DiscountMoney ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Stu_sOffset
          WHERE     Status = 1
                    AND RelatedID = fo.sFeesOrderID
        ) OffsetMoney ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Stu_sOffset
          WHERE     Status = 1
                    AND ByRelatedID = fo.sFeesOrderID
                    AND o.Sort = 1
        ) ByOffsetMoney ,
        ( SELECT  ISNULL(SUM(RefundMoney),0)  
          FROM      T_Stu_sRefund
          WHERE     Status = 1
                    AND sFeesOrderID = fo.sFeesOrderID
        ) RefundMoney
FROM    T_Stu_sFeesOrder fo
        LEFT JOIN T_Stu_sOrder o ON o.sOrderID = fo.sOrderID
        LEFT JOIN T_Pro_Detail d ON d.DetailID = o.DetailID
        Where fo.sFeeID=" + sFeeID + "";
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "ShouldMoney,PaidMoney,DiscountMoney,OffsetMoney,ByOffsetMoney,RefundMoney"));
        }

    }

}
