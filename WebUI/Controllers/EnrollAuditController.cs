using Common;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class EnrollAuditController : Controller
    {
        //
        // GET: /EnrollAudit/

        public void Post()
        {
            Stream postData = Request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            EnrollPostModel epm = new EnrollPostModel();
            string data = OtherHelper.DecodeBase64("UTF-8", postContent);
        }

    }
}
