using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class NoteController : BaseController
    {
        //
        // GET: /Note/

        public ActionResult NoteList()
        {
            ViewBag.Title = "导入记录";
            return View();
        }

        #region 倒入记录列表
        public ActionResult GetNoteList()
        {
            string menuId = Request.Form["MenuID"];
            string deptId = Request.Form["selDeptID"];
            string createTime = Request.Form["txtCreateTime"];
            string where = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where += " AND n.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(createTime))
            {
                where += " AND convert(nvarchar(23),n.CreateTime,23) = '" + createTime + "'";
            }
            string cmdText = @"SELECT  n.NoteID ,
        d.Name Dept ,
        r1.RefeName SortName ,
        n.InFile ,
        n.OutFile ,
        n.SuccessNum ,
        n.ErrorNum ,
        n.CreateTime
FROM    T_Pro_Note AS n
        LEFT JOIN T_Sys_Dept AS d ON d.DeptID = n.DeptID
		LEFT JOIN  T_Sys_Refe r1 ON r1.Value=n.Sort and r1.RefeTypeID=12
WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "n.DeptID", "n.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        #endregion
    }
}
