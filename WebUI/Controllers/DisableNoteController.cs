using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class DisableNoteController : BaseController
    {
        //
        // GET: /DisableNote/

        public ActionResult DisableNoteList()
        {
            ViewBag.Title = "作废票据管理";
            return View();
        }

        public ActionResult GetDisableNoteList()
        {
            string menuId = Request.Form["MenuID"];
            string voucherNum = Request.Form["txtVoucherNum"];
            string noteNum = Request.Form["txtNoteNum"];
            string where = "";
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and f.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " and dn.NoteNum like '%" + noteNum + "%'";
            }
            string cmdText = @"SELECT  dn.DisableNoteID ,
        f.VoucherNum ,
        dn.NoteNum ,
        u.Name ,
        dn.CreateTime
FROM    T_Pro_DisableNote dn
        LEFT JOIN T_Pro_Fee f ON dn.FeeID = f.FeeID
        LEFT JOIN T_Sys_User u ON dn.CreateID = u.UserID
WHERE   1 = 1{0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "f.DeptID", "dn.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
    }
}
