using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.Data;
using System.Data.SqlClient;
using BLL;
using Common;
namespace WebUI.Controllers
{
    public class ConfigController : BaseController
    {
        //
        // GET: /Config/
        #region 页面加载
        public ActionResult ConfigList()
        {
            return View();
        }
        public ActionResult ConfigEdit()
        {
            return View();
        }
        #endregion

        #region 配置列表
        public ActionResult GetConfigList()
        {
            string cmdText = @"SELECT * FROM  T_Pro_Config WHERE 1=1";
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        #endregion

        #region 编辑配置信息
        public string GetConfigEdit(ConfigModel cm)
        {
            if (string.IsNullOrEmpty(cm.ConfigID))
            {
                return "请选择配置信息";
            }
            if (string.IsNullOrEmpty(cm.PrintNum))
            {
                return "请输入打印次数";
            }
            string where = " and ConfigID=@ConfigID";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ConfigID",cm.ConfigID)
            };
            ConfigModel editcm = ConfigBLL.ConfigModelByWhere(where, paras);
            LogBLL.CreateLog("T_Pro_Config", this.UserId.ToString(), editcm, cm);
            editcm.PrintNum = cm.PrintNum;
            if (ConfigBLL.UpdateConfig(editcm) > 0)
            {
                return "yes";
            }
            else
            {
                return "出现未知错误，请联系管理员";
            }
        }
        #endregion


    }
}
