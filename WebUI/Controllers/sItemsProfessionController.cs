using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using BLL;
using System.Data;
using Common;

namespace WebUI.Controllers
{
    public class sItemsProfessionController : BaseController
    {
        //
        // GET: /sItemsProfession/

        public ActionResult sItemsProfessionEdit()
        {
            return View();
        }

        /// <summary>
        /// 编辑招生方案专业设置
        /// </summary>
        /// <param name="ipm"></param>
        /// <returns></returns>
        public string GetItemsProfessionEdit(sItemsProfessionFormModel ipfm)
        {
            if (string.IsNullOrEmpty(ipfm.ItemID))
            {
                return "没有选择收费方案";
            }
            if (string.IsNullOrEmpty(ipfm.sItemsProfessionID))
            {
                ipfm.sItemsProfessionID = "0";
            }

            bool flag = false;

            for (int i = 0; i < ipfm.sProfessionID.Length; i++)
            {
                if (ValidateItemsProfessionIsRepeat(ipfm.ItemID, ipfm.sProfessionID[i]))
                {
                    return "招生专业不能重复";
                }
            }
            for (int i = 0; i < ipfm.sProfessionID.Length; i++)
            {
                try
                {
                    if (ipfm.sItemsProfessionID.Equals("0"))
                    {
                        sItemsProfessionModel ipm = new sItemsProfessionModel();
                        ipm.sProfessionID = ipfm.sProfessionID[i];
                        ipm.ItemID = ipfm.ItemID;
                        if (sItemsProfessionBLL.InsertsItemsProfession(ipm) > 0)
                            flag = true;
                    }
                }
                catch (Exception)
                {
                    flag = false;
                    throw;
                }

            }


            if (flag)
                return "yes";
            return "出现未知错误，请联系管理员";
        }


        /// <summary>
        /// 根据id返回实体
        /// </summary>
        /// <param name="itemsProfessionId"></param>
        /// <returns></returns>
        private sItemsProfessionModel GetsItemsProfessionModel(string itemsProfessionId)
        {
            string where = " and ItemsProfessionID=@ItemsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemsProfessionID",itemsProfessionId)
            };
            sItemsProfessionModel ipm = sItemsProfessionBLL.sItemsProfessionModelByWhere(where, paras);
            return ipm;
        }
        /// <summary>
        /// 验证招生专业是否重复
        /// </summary>
        /// <param name="itemsProfessionId"></param>
        /// <param name="sprofessionId"></param>
        /// <returns></returns>
        private bool ValidateItemsProfessionIsRepeat(string itemId, string sprofessionId)
        {
            string where = " and ItemID=@ItemID and sProfessionID=@sProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemID",itemId),
                 new SqlParameter("@sProfessionID",sprofessionId)
            };
            DataTable dt = sItemsProfessionBLL.sItemsProfessionTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取招生方案专业信息
        /// </summary>
        /// <returns></returns>

        public ActionResult GetItemsProfessionList()
        {
            string itemId = Request.Form["ItemID"];
            if (string.IsNullOrEmpty(itemId))
            {
                itemId = "0";
            }
            string cmdText = @"SELECT  sip.sItemsProfessionID ,
        p.Name
FROM   T_Stu_sItemsProfession sip
        LEFT  JOIN T_Stu_sProfession sp ON sip.sProfessionID = sp.sProfessionID
        LEFT JOIN T_Pro_Profession p ON p.ProfessionID = sp.ProfessionID
        Where sip.ItemID=" + itemId + " AND sp.Status = 1";
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }


        /// <summary>
        /// 删除找生方案专业设置
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetDetelesItemProfession()
        {
            string sitemsProfessionId = Request.Form["ID"];
            if (string.IsNullOrEmpty(sitemsProfessionId))
            {
                return AjaxResult.Error();
            }
            string[] idArray = sitemsProfessionId.Split(',');
            for (int i = 0; i < idArray.Length; i++)
            {
                sItemsProfessionBLL.DeletesItemsProfession(idArray[i]);
            }
            return AjaxResult.Success();
        }
    }
}
