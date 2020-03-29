using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class DeriveController : BaseController
    {
        //
        // GET: /Derive/
        [ValidateInput(false)]
        public AjaxResult DeriveList(FormCollection fc)
        {
            string filePath = Request.Form["FilePath"];
            string data = fc["Data"];
            DataTable dt = JsonHelper.JsonToDataTable(data);
            if (dt == null)
            {
                return AjaxResult.Error("表格没有任何数据");
            }
            OtherHelper.DeriveToExcel(dt, filePath);
            return AjaxResult.Success();
        }

        [ValidateInput(false)]
        public AjaxResult DeriveData(FormCollection fc)
        {
            try
            {
                string filePath = Request.Form["FilePath"];
                string html = fc["Html"];
                OtherHelper.DeriveToExcelByHtml(html, filePath);
                return AjaxResult.Success();
            }
            catch (Exception ex)
            {
                return AjaxResult.Error(ex.Message);
            }

        }
       

    }
}
