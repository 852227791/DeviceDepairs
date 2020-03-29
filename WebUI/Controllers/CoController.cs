using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class CoController : Controller
    {
        //
        // GET: /Co/
        
        public ActionResult CoClass()
        {
            ViewBag.Title = "生成";

            return View();
        }

    }
}
