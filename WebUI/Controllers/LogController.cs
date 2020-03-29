using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class LogController : BaseController
    {
        //
        // GET: /Log/

        public ActionResult LogInfo()
        {
            return View();
        }
        public ActionResult GetLogList()
        {
            string where = "";
            string tablename = Request.Form["txtTableName"];
            string feildname = Request.Form["txtFieldName"];
            string createid = Request.Form["txtCreateID"];
            string valueold = Request.Form["txtValueOld"];
            string valuenew = Request.Form["txtValueNew"];
            string createtimeS = Request.Form["txtCreateTimeS"];
            string createtimeE = Request.Form["txtCreateTimeE"];
            if (!string.IsNullOrEmpty(tablename))
            {
                where += " and l.TableName like '%" + tablename + "%'";
            }
            if (!string.IsNullOrEmpty(feildname))
            {
                where += " and l.FieldName like '%" + feildname + "%'";
            }
            if (!string.IsNullOrEmpty(createid))
            {
                where += " and u.Name like '%" + createid + "%'";
            }
            if (!string.IsNullOrEmpty(valueold))
            {
                where += " and l.ValueOld like '%" + valueold + "%'";
            }
            if (!string.IsNullOrEmpty(valuenew))
            {
                where += " and l.ValueNew like '%" + valuenew + "%'";
            }
            if (!string.IsNullOrEmpty(createtimeS))
            {
                where += " and CONVERT(NVARCHAR(100),l.CreateTime,23)>= '" + createtimeS + "'";
            }
            if (!string.IsNullOrEmpty(createtimeE))
            {
                where += " and CONVERT(NVARCHAR(100),l.CreateTime,23) <= '" + createtimeE + "'";
            }
            string cmdText = @"SELECT  l.LogID ,
        l.TableName ,
        l.FieldName ,
        u.Name AS CreateID,
        l.ValueOld,
        l.ValueNew,
       CONVERT(NVARCHAR(100),l.CreateTime,20) AS CreateTime
FROM    T_Sys_Log AS l
        LEFT JOIN T_Sys_User AS u ON u.UserID = l.CreateID
        where 1=1 {0}  ";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
    }
}
