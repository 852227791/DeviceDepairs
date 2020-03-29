using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class SelfHelpController : Controller
    {
        //
        // GET: /SelfHelp/

        public ActionResult Index()
        {
            ViewBag.Title = "自助终端首页";
            return View();
        }


        public ActionResult SearchFee()
        {
            ViewBag.Title = "费用信息查询";
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Type = Request.QueryString["Type"];
            ViewBag.Title = "自助终端登录";
            return View();
        }
        public ActionResult OnlineFee()
        {
            ViewBag.Title = "在线缴费";
            return View();
        }

        public ActionResult StudentLogin(string IdCard,string Type)
        {
            if (string.IsNullOrEmpty(IdCard))
            {
                ViewBag.Error = "请填写身份证号！";
                return View("Login");
            }
            if (Type.Equals("1"))
            {
                return RedirectToAction("SearchFee", "SelfHelp");
            }
            else  
            {
                return RedirectToAction("OnlineFee", "SelfHelp");
            }
        }


    }
}
