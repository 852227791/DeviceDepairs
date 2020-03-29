using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace WebUI.Controllers
{
    public class iDisableNoteController : BaseController
    {
        #region 页面
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult iDisableNoteList()
        {
            return View();
        }
        #endregion

        #region 获取列表数据
        public ActionResult GetiDisableNoteList()
        {
            string menuId = Request.Form["MenuID"];
            string voucherNum = Request.Form["txtVoucherNum"];
            string noteNum = Request.Form["txtNoteNum"];
            string where = "";
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and ifee.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " and dn.NoteNum like '%" + noteNum + "%'";
            }
            string cmdText = @"SELECT  dn.iDisableNoteID ,
        ifee.VoucherNum ,
        dn.NoteNum ,
        sysuser.Name ,
        dn.CreateTime
FROM    T_Inc_iDisableNote AS dn
        LEFT JOIN T_Inc_iFee AS ifee ON dn.iFeeID = ifee.iFeeID
        LEFT JOIN T_Sys_User AS sysuser ON dn.CreateID = sysuser.UserID
WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ifee.DeptID", "dn.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        #endregion
    }
}
