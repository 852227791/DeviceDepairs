using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Title = "收费管理系统";
            return View();
        }

        public ActionResult CurrentUser(string cmd)
        {
            string result = "";
            switch (cmd.ToLower())
            {
                case "getname":
                    result = UserName;
                    break;
            }
            return Json(new { Result = result, Message = "200" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Menu()
        {
            string json = "[]";
            json = GetMenuTreeJSON();
            Response.Write(json);
            Response.End();

            return new EmptyResult();
        }

        /// <summary>
        /// 获取菜单列表(JSON Tree格式)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetMenuTreeJSON()
        {
            string cmdText = @"SELECT  MenuID ,
        ParentID ,
        Name ,
        PagePath ,
        IconPath
FROM    T_Sys_Menu
WHERE   Status = 1
        AND MenuID IN ( SELECT  MenuID
                        FROM    T_Sys_Power
                        WHERE   RoleID IN ( SELECT  RoleID
                                                FROM    T_Sys_UserRole
                                                WHERE   UserID = @userId ) )
ORDER BY Queue ASC ,
        MenuID ASC";
            SqlParameter[] paras = new SqlParameter[] 
            { 
                new SqlParameter("@userId", this.UserId)
            };
            return JsonMenuTreeData.GetArrayJSON("MenuID", "ParentID", cmdText, CommandType.Text, paras);
        }

        public ActionResult Main()
        {
            ViewBag.Title = "主页";

            return View();
        }
    }
}
