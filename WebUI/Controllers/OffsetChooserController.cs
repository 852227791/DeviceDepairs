using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class OffsetChooserController : BaseController
    {
        //
        // GET: /OffsetChooser/
        /// <summary>
        /// 学费
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public ActionResult GetsFeeList(OffsetChooserModel ocm)
        {
            string where = "";
            if (!string.IsNullOrEmpty(ocm.name))
                where += "and  s.Name like '%" + ocm.name + "%'";
            if (!string.IsNullOrEmpty(ocm.idCard))
                where += "and s.IDCard like '%" + ocm.idCard + "%'";
            if (!string.IsNullOrEmpty(ocm.voucherNum))
                where += "and  f.VoucherNum like '%" + ocm.voucherNum + "%'";
            if (!string.IsNullOrEmpty(ocm.noteNum))
                where += " and f.NoteNum like '%" + ocm.noteNum + "%'";

            string cmdText = @"SELECT  fo.sFeesOrderID ID ,
        f.Status StatusValue ,
        d.Name DeptName ,
        f.VoucherNum ,
        f.NoteNum ,
        r1.RefeName Status ,
        r2.RefeName FeeModel ,
        s.Name StudName ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        ( SELECT DISTINCT
                   o.NumName+ ' ' +	dl.Name  --+ ' ' + o.PlanName
          FROM      T_Stu_sOrder o
          WHERE     o.NumItemID = f.NumItemID
                    AND o.sEnrollsProfessionID = f.sEnrollsProfessionID
        ) Content ,
        s.IDCard ,
        1 Type ,
		fo.CanMoney ,
        0.00 Money
	
FROM    T_Stu_sFeesOrder fo
        LEFT JOIN T_Stu_sFee f ON f.sFeeID = fo.sFeeID
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                   AND r1.RefeTypeID = 7
        LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                   AND r2.RefeTypeID = 6
		LEFT JOIN  T_Stu_sOrder  o ON o.sOrderID=fo.sOrderID
		LEFT JOIN  T_Pro_Detail dl ON dl.DetailID=o.DetailID
        Where fo.Status<>9 {0}
";
            string powerStr = PurviewToList(ocm.MenuID);
            powerStr = string.Format(powerStr, "f.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, ""));
        }
        /// <summary>
        /// 杂费
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public ActionResult GetiFeeList()
        {
            string where = "";
            string MenuID = Request.Form["MenuID"];
            string studName = Request.Form["studName"];
            string voucherNum = Request.Form["voucherNum"];
            string idCard = Request.Form["idCard"];
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and i.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(studName))
            {
                where += " and s.Name like '%" + studName + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and s.IDCard like '%" + idCard + "%'";
            }
            string cmdText = @"SELECT  i.iFeeID ID ,
        VoucherNum ,
        s.Name StudName ,
        s.IDCard ,
        i.CanMoney ,
        0.00 Money ,
        '2' Sort ,
        i.NoteNum ,
        dl.Name FeeContent ,
        d.Name DeptName
FROM    T_Inc_iFee i
        LEFT JOIN T_Sys_Dept d ON i.DeptID = d.DeptID
        LEFT JOIN T_Pro_Student s ON s.StudentID = i.StudentID
        LEFT JOIN T_Pro_ItemDetail id ON id.ItemDetailID = i.ItemDetailID
        LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
WHERE   i.Status <> 9 {0}
";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "i.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));

        }

        public ActionResult GetFeeList()
        {
            string where = "";
            string MenuID = Request.Form["MenuID"];
            string studName = Request.Form["studName"];
            string voucherNum = Request.Form["voucherNum"];
            string idCard = Request.Form["idCard"];
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and f.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(studName))
            {
                where += " and s.Name like '%" + studName + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and s.IDCard like '%" + idCard + "%'";
            }
            string cmdText = @"SELECT  fd.FeeDetailID ID ,
        f.VoucherNum ,
        s.Name StudName ,
        s.IDCard ,
        fd.CanMoney ,
        0.00 Money ,
        '3' Sort ,
        f.NoteNum ,
        i.Name + ' ' + dl.Name FeeContent ,
        d.Name DeptName
FROM    T_Pro_FeeDetail AS fd
        LEFT JOIN T_Pro_Fee f ON f.FeeID = fd.FeeID
        LEFT JOIN T_Pro_Prove AS p ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
        LEFT JOIN T_Pro_Item AS i ON i.ItemID = p.ItemID
        LEFT JOIN T_Pro_ItemDetail id ON id.ItemDetailID = fd.ItemDetailID
        LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
        LEFT JOIN T_Sys_Dept d ON f.DeptID = d.DeptID
WHERE   f.Status IN ( 1, 2 ) {0}
";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "f.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));

        }
    }
}
