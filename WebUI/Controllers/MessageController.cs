using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class MessageController : BaseController
    {
        //
        // GET: /Message/

        public ActionResult MessageList()
        {
            return View();
        }
        public ActionResult GetMessageList() {
            string timeS = Request.Form["txtTimeS"];
            string timeE = Request.Form["txtTimeE"];
            string where = string.Empty;
            if (!string.IsNullOrEmpty(timeS))
            {
                where += " AND convert(NVARCHAR(10),Date,23) >= '" + timeS + "'";
            }
            if (!string.IsNullOrEmpty(timeE))
            {
                where += " AND convert(NVARCHAR(10),Date,23) <= '" + timeE + "'";
            }
            string cmdText = "SELECT id,Date,Thread,Levels,Message,REPLACE(Exception,'\\','/') Exception,Logger FROM T_Service_Log  WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            var result = JsonGridData.GetGridJSON(cmdText, Request.Form).Replace("\r\n","");
            return ResponseWriteResult(result);
        }
    }
}
