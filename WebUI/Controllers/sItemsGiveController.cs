using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using Common;

namespace WebUI.Controllers
{
    public class sItemsGiveController : BaseController
    {
        //
        // GET: /sItemsGive/

        public ActionResult sItemsGiveEdit()
        {
            return View();
        }
        /// <summary>
        /// 编辑招生方案配品设置
        /// </summary>
        /// <param name="igm"></param>
        /// <returns></returns>
        public string GetsItemsGiveEdit(sItemsGiveModel igm)
        {
            if (string.IsNullOrEmpty(igm.ItemID))
            {
                return "没有选择收费方案";
            }
            if (string.IsNullOrEmpty(igm.sGiveID))
            {
                return "请选择配品";
            }
            if (string.IsNullOrEmpty(igm.sItemsGiveID))
            {
                igm.sItemsGiveID = "0";
            }
            if (ValidatesItemGiveIsRepeat(igm.ItemID, igm.sGiveID))
            {
                return "配品不能重复";
            }

            if (sItemsGiveBLL.InsertsItemsGive(igm) > 0)
                return "yes";
            return "出现未知错误，请联系管理员！";

        }

        /// <summary>
        /// 验证配品是否重复
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="sgiveId"></param>
        /// <returns></returns>
        private bool ValidatesItemGiveIsRepeat(string itemId, string sgiveId)
        {
            string where = " and ItemID=@ItemID and sGiveID=@sGiveID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemID",itemId),
                new SqlParameter("@sGiveID",sgiveId)
            };
            DataTable dt = sItemsGiveBLL.sItemsGiveTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 删除招生方案品
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetDeletesItemsGive()
        {
            string sitemsGiveId = Request.Form["ID"];
            if (string.IsNullOrEmpty(sitemsGiveId))
            {
                return AjaxResult.Error();
            }
            string[] idArray = sitemsGiveId.Split(',');
            for (int i = 0; i < idArray.Length; i++)
            {
                sItemsGiveBLL.DeletesItemsGive(idArray[i]);
            }
            return AjaxResult.Success();


        }

        public ActionResult GetsItemsGiveList()
        {
            string itemId = Request.Form["ItemID"];
            if (string.IsNullOrEmpty(itemId))
                itemId = "0";
            string cmdText = @"SELECT  sg.Name ,
        sig.sItemsGiveID ,
        sig.Queue
FROM    T_Stu_sItemsGive sig
        LEFT JOIN T_Stu_sGive sg ON sg.sGiveID = sig.sGiveID
where sig.ItemID=" + itemId + " AND sg.Status = 1";
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        /// <summary>
        /// 获取招生方案下的；配品
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetGiveByItem()
        {
            string itemId = Request.Form["ItemID"];
            string sEnrollsProfessionID = Request.Form["sEnrollsProfessionID"];
            DataTable dt = sItemsGiveBLL.GetItemGive(itemId, sEnrollsProfessionID);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt), "success");
        }
        /// <summary>
        /// 获取招生方案下的；配品(作废)
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetsFeeGive()
        {
            string itemId = Request.Form["ItemID"];
            string sfeeId = Request.Form["sFeeID"];
            DataTable dt = sItemsGiveBLL.GetsFeeOrderGive(itemId, sfeeId);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt), "success");
        }


    }
}
