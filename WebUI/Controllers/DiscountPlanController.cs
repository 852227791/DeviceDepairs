using BLL;
using Common;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class DiscountPlanController : BaseController
    {
        //
        // GET: /DiscountPlan/

        public ActionResult DiscountPlanEdit()
        {
            return View();
        }
        public ActionResult GetDiscountPlanList(string ItemID)
        {
            string cmdText = @"SELECT dp.DiscountPlanID,  dp.Name ,
        r1.RefeName Status ,
        ( SELECT    ISNULL(SUM(Money),0)
          FROM      T_Stu_DiscountDetail
          WHERE     Status = 1
                    AND DiscountPlanID = dp.DiscountPlanID
        )Money
FROM    T_Stu_DiscountPlan dp
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = dp.Status
                                       AND r1.RefeTypeID = 1
            WHERE   dp.PlanItemID=" + ItemID + "";
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        public AjaxResult ValidatePlanName(string DiscountPlanID,string Name,string ItemID) {
            string where2 = "  and DiscountPlanID<>@DiscountPlanID and Name=@Name and PlanItemID=@PlanItemID";
            SqlParameter[] paras2 = new SqlParameter[] {
                    new SqlParameter("@DiscountPlanID",DiscountPlanID),
                     new SqlParameter("@Name",Name.Trim()),
                new SqlParameter("@PlanItemID",ItemID.Trim())
                };
            var l2 = DiscountPlanBLL.SelectDiscountPlanByWhere(where2, paras2, "");
            if (l2.Count > 0)
            {
                return AjaxResult.Success("优惠方案名称不能重复！");
            }
            return AjaxResult.Error();
        }
        public AjaxResult GetDiscountPlanEdit(string ItemID, string PlanName, string Detail, string DiscountPlanID)
        {
            string where2 = "  and DiscountPlanID<>@DiscountPlanID and Name=@Name and PlanItemID=@PlanItemID";
            SqlParameter[] paras2 = new SqlParameter[] {
                    new SqlParameter("@DiscountPlanID",DiscountPlanID),
                     new SqlParameter("@Name",PlanName.Trim()),
                new SqlParameter("@PlanItemID",ItemID.Trim())
                };
            var l2 = DiscountPlanBLL.SelectDiscountPlanByWhere(where2, paras2, "");
            if (l2.Count > 0)
            {
                return AjaxResult.Error("优惠方案名称不能重复！");
            }

            if (string.IsNullOrEmpty(PlanName))
            {
                return AjaxResult.Error("优惠方案名称不能为空！");
            }
            List<DiscountDetail> list = JsonConvert.DeserializeObject<List<DiscountDetail>>(Detail);
            if (list.Count == 0)
            {
                return AjaxResult.Error("优惠方案明细不能为空！");
            }
            if (string.IsNullOrEmpty(DiscountPlanID))
            {
                DiscountPlanModel dpm = new DiscountPlanModel();
                dpm.Name = PlanName;
                dpm.PlanItemID = ItemID;
                dpm.Status = "1";
                dpm.CreateID = UserId.ToString();
                dpm.CreateTime = DateTime.Now.ToString();
                dpm.UpdateID = UserId.ToString();
                dpm.UpdateTime = DateTime.Now.ToString();
                dpm.DiscountPlanID = DiscountPlanBLL.InsertDiscountPlan(dpm).ToString();
                foreach (var item in list)
                {
                    DiscountDetailModel ddm = new DiscountDetailModel();
                    ddm.CreateID = UserId.ToString();
                    ddm.CreateTime = DateTime.Now.ToString();
                    ddm.DiscountPlanID = dpm.DiscountPlanID;
                    ddm.ItemDetaiID = item.ItemDetailID;
                    ddm.NumItemID = item.ItemID;
                    ddm.Money = item.DiscountMoney;
                    ddm.Status = "1";
                    ddm.UpdateID = UserId.ToString();
                    ddm.UpdateTime = DateTime.Now.ToString();
                    DiscountDetailBLL.InsertDiscountDetail(ddm);
                }
                return AjaxResult.Success("保存成功！");
            }
            else
            {
                string where = "  and DiscountPlanID=@DiscountPlanID";
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@DiscountPlanID",DiscountPlanID)
                };
                var l = DiscountPlanBLL.SelectDiscountPlanByWhere(where, paras, "").FirstOrDefault();
                l.Name = PlanName;
                l.UpdateID = UserId.ToString();
                l.UpdateTime = DateTime.Now.ToString();
                DiscountPlanBLL.UpdateDiscountPlan(l);
                foreach (var item in list)
                {
                    if (string.IsNullOrEmpty(item.DiscountDetailID))
                    {
                        DiscountDetailModel ddm = new DiscountDetailModel();
                        ddm.CreateID = UserId.ToString();
                        ddm.CreateTime = DateTime.Now.ToString();
                        ddm.DiscountPlanID = l.DiscountPlanID;
                        ddm.ItemDetaiID = item.ItemDetailID;
                        ddm.NumItemID = item.ItemID;
                        ddm.Money = item.DiscountMoney;
                        ddm.Status = "1";
                        ddm.UpdateID = UserId.ToString();
                        ddm.UpdateTime = DateTime.Now.ToString();
                        DiscountDetailBLL.InsertDiscountDetail(ddm);
                    }
                    else
                    {
                        string where1 = "  and DiscountDetailID=@DiscountDetailID";
                        SqlParameter[] paras1 = new SqlParameter[] {
                               new SqlParameter("@DiscountDetailID",item.DiscountDetailID)
                     };
                        var detail = DiscountDetailBLL.SelectDiscountDetailByWhere(where1, paras1, "").FirstOrDefault();
                        detail.Money = item.DiscountMoney;
                        detail.UpdateID = UserId.ToString();
                        detail.UpdateTime = DateTime.Now.ToString();
                        DiscountDetailBLL.UpdateDiscountDetail(detail);

                    }
                }
                return AjaxResult.Success("保存成功！");
            }
        }

        public AjaxResult GetDiscountPlan(string ID)
        {
            string where = "  and DiscountPlanID=@DiscountPlanID";
            SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@DiscountPlanID",ID)
                };
            var l = DiscountPlanBLL.SelectDiscountPlanByWhere(where, paras, "").FirstOrDefault();

            return AjaxResult.Success(OtherHelper.JsonSerializer(l), "");
        }

        public AjaxResult GetDicountDetail(string ID)
        {

            return AjaxResult.Success(JsonHelper.DataTableToJson(DiscountDetailBLL.GetDiscountDetailEdit(ID)), "");
        }

        public AjaxResult GetDeleteDiscountDetail(string ID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return AjaxResult.Error("请选择优惠明细！");
            }
            if (DiscountDetailBLL.DeleteDiscountDetail(ID))
            {
                return AjaxResult.Success("删除成功！");
            }
            else
            {
                return AjaxResult.Error("删除失败！");
            }
        }

        public string GetDiscountPlanCombobox(string ItemID)
        {
            return DiscountPlanBLL.GetDiscountPlanCombobox(ItemID);
        }
        /// <summary>
        /// 获取优惠详情
        /// </summary>
        /// <param name="discountPlanId"></param>
        /// <returns></returns>
        public AjaxResult GetDiscountDetail(string ID)
        {
            string where = " and Status=1 and DiscountPlanID=@DiscountPlanID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DiscountPlanID",ID)
            };

            return AjaxResult.Success(OtherHelper.JsonSerializer(DiscountDetailBLL.SelectDiscountDetailByWhere(where, paras, "")), "");
        }

        public AjaxResult Disable(string ID, string Value)
        {
            if (DiscountPlanBLL.UpdateDiscountPlanStatus(ID, Value, UserId))
            {
                return AjaxResult.Success();
            }
            return AjaxResult.Error();
        }
    }
}
