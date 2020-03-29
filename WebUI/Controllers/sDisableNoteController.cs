using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class sDisableNoteController : BaseController
    {
        //
        // GET: /sDisableNote/

        public ActionResult sDisableNoteList()
        {
            return View();
        }


        public ActionResult GetsDisableNoteList()
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
            string cmdText = @"SELECT  dn.sDisableNoteID,f.VoucherNum,dn.NoteNum,u.Name,dn.CreateTime
FROM     T_Stu_sDisableNote dn
        LEFT JOIN  T_Stu_sFee f ON f.sFeeID = dn.sFeeID
        LEFT JOIN  T_Sys_User u ON u.UserID = dn.CreateID
WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "f.DeptID", "dn.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));

        }
    }
}
