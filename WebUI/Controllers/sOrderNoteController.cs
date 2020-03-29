using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.Data.SqlClient;
using BLL;

namespace WebUI.Controllers
{
    public class sOrderNoteController : BaseController
    {
        //
        // GET: /sOrderNote/


        public AjaxResult GetsOrderNoteEdit(sOrderNoteFormModel onfm)
        {
            string tempValue = "";
            if (string.IsNullOrEmpty(onfm.sOrderID))
                onfm.sOrderID = "0";
            string where = " and sOrderID=@sOrderID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sOrderID",onfm.sOrderID)
            };
            sOrderModel editonfm = sOrderBLL.sOrderModelByWhere(where, paras);
            if (Convert.ToDecimal(onfm.ChangeShouldMoney) == Convert.ToDecimal(editonfm.ShouldMoney))
                return AjaxResult.Success("金额未修改", "error");

            if (decimal.Parse(editonfm.PaidMoney) > decimal.Parse(onfm.ChangeShouldMoney))
                return AjaxResult.Success("修改后的应收金额不能低于实缴金额", "error");
            tempValue = editonfm.ShouldMoney;
            editonfm.ShouldMoney = onfm.ChangeShouldMoney;
            editonfm.UpdateID = this.UserId.ToString();
            editonfm.UpdateTime = DateTime.Now.ToString();



            if (sOrderBLL.UpdatesOrder(editonfm) > 0)
            {
                sOrderBLL.UpdatesOderPaidMoney(0, onfm.sOrderID, UserId);
                sOrderNoteModel onm = new sOrderNoteModel();
                onm.sOrderID = onfm.sOrderID;
                onm.Explain = onfm.ChangeShouldMoneyRemark;
                onm.CreateID = this.UserId.ToString();
                onm.CreateTime = DateTime.Now.ToString();
                onm.UpdateID = this.UserId.ToString();
                onm.UpdateTime = DateTime.Now.ToString();
                onm.ValueOld = tempValue;
                onm.ValueNew = onfm.ChangeShouldMoney;
                onm.Status = "1";
                onm.FieldName = "ShouldMoney";
                if (sOrderNoteBLL.InsertsOrderNote(onm) > 0)
                {
                    return AjaxResult.Success("", "success");
                }
                else
                    return AjaxResult.Success("出现未知错误，请联系管理员", "error");
            }
            else
            {
                return AjaxResult.Success("出现未知错误，请联系管理员", "error");
            }
        }
    }
}
