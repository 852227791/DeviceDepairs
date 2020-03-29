using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Data.SqlClient;
using System.Data;
using BLL;
namespace WebUI.Controllers
{
    public class FeeDetailController : BaseController
    {
        //
        // GET: /FeeDetail/


        #region
        public AjaxResult GetItemDetailIdByFeeID()
        {
            string feeiId = Request.Form["ID"];
            string where = " and Status=1  and FeeID=@FeeID ";
            SqlParameter[] paras = new SqlParameter[] { 
               new SqlParameter("@FeeID",feeiId)
               };
            DataTable dt = FeeDetailBLL.FeeDetailTableByWhere(where, paras, "");
            string tempString = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tempString += dt.Rows[i]["ItemDetailID"].ToString() + ",";
            }
            if (!string.IsNullOrEmpty(tempString))
            {
                tempString = tempString.Substring(0, tempString.Length - 1);
            }
            return AjaxResult.Success(tempString,"");
        }
        #endregion
        /// <summary>
        /// 收费项下拉
        /// </summary>
        /// <returns></returns>
        public string GetFeeDeailCommbox()
        {
            string feeid = Request.QueryString["FeeID"];
            return FeeDetailBLL.GetFeeDeailCommbox(feeid);
        }

    }
}
