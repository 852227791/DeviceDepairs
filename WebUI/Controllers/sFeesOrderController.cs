using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class sFeesOrderController : BaseController
    {

        public ActionResult ChoosesFeeOrderList() {
            return View();
        }
        //
        public AjaxResult GetsOrderList(string ID)
        {

            string cmdText = @"SELECT 

		fo.sFeesOrderID,d.Name DetailName ,
        fo.ShouldMoney  ,
        fo.PaidMoney ,
        fo.DiscountMoney ,
		fo.ShouldMoney-fo.PaidMoney- fo.DiscountMoney-(SELECT ISNULL(SUM(soff.Money),0.00) FROM T_Stu_sOffset soff WHERE soff.RelatedID=fo.sFeesOrderID and soff.Status=1) UnPaid ,
        (SELECT ISNULL(SUM(soff.Money),0.00) FROM T_Stu_sOffset soff WHERE soff.RelatedID=fo.sFeesOrderID and soff.Status=1) OffsetMoney ,
        '[]' OffsetDetail
FROM    T_Stu_sFeesOrder fo
        LEFT JOIN T_Stu_sOrder o ON fo.sOrderID = o.sOrderID
        LEFT JOIN T_Pro_Detail d ON d.DetailID = o.DetailID
		WHERE  fo.Status=1 and  fo.sFeeID={0} ";
            cmdText = string.Format(cmdText, ID);
            return AjaxResult.Success(JsonGridData.GetGridJSON(cmdText, Request.Form), "");
        }
        /// <summary>
        /// 核销收费明细选择器
        /// </summary>
        /// <returns></returns>
        public ActionResult GetChoosesFeeOrderList()
        {
            string where = "";
            string name =Request.Form["studName"];
            string voucherNum = Request.Form["vaoucherNum"];
            if (!string.IsNullOrEmpty(name))
            {
                where += " and s.Name like'%"+ name + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and f.VoucherNum like'%" + voucherNum + "%'";
            }
            string cmdText = @"SELECT  fo.sFeesOrderID ,
        f.VoucherNum ,
        f.NoteNum ,
        s.Name ,
        s.IDCard ,
        so.PlanName + ' ' + so.NumName Content ,
        d.Name DetailName ,
        fo.PaidMoney ,
        fo.DiscountMoney ,
        ( SELECT    ISNULL(SUM(Money), 0.00)
          FROM      T_Stu_sOffset
          WHERE     RelatedID = fo.sFeesOrderID
                    AND Status = 1
        ) OffsetMoney ,
        ( SELECT    ISNULL(SUM(Money), 0.00)
          FROM      T_Stu_sOffset
          WHERE     ByRelatedID = fo.sFeesOrderID
                    AND Status = 1
        ) ByOffsetMoney ,
        fo.CanMoney ,
        fo.ShouldMoney ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        ( SELECT    ISNULL(SUM(RefundMoney), 0.00)
          FROM      T_Stu_sRefund
          WHERE     sFeesOrderID = fo.sFeesOrderID
                    AND Status = 1
        ) RefundMoney ,
        r1.RefeName FeeMode ,
        u.Name Affirm ,
        CASE CONVERT(NVARCHAR(10), f.AffirmTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), f.AffirmTime, 23)
        END AffirmTime,
        dt.Name Dept
FROM    T_Stu_sFeesOrder fo
        LEFT JOIN T_Stu_sFee f ON f.sFeeID = fo.sFeeID
        LEFT JOIN T_Stu_sOrder so ON so.sOrderID = fo.sOrderID
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = so.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Pro_Detail d ON d.DetailID = so.DetailID
        LEFT JOIN T_Sys_Dept dt ON dt.DeptID = f.DeptID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.FeeMode
                                   AND r1.RefeTypeID = 6
        LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID

        Where 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
    }
}
