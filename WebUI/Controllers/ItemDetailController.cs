using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using Common;
using System.Data;
using System.Data.SqlClient;
namespace WebUI.Controllers
{
    public class ItemDetailController : BaseController
    {
        //
        // GET: /ItemDetail/

        #region 页面加载
        public ActionResult ItemDetailEdit()
        {
            return View();
        }

        #endregion

        #region 项目明细列表
        public ActionResult GetItemDetailList()
        {
            string itemId = Request.Form["ItemID"];
            string isS = Request.Form["Is"];
            string where = "";
            if (isS != "Yes")
            {
                where += " AND id.Status = 1";
            }
            string cmdText = @"SELECT  id.Status StatusValue ,
        r1.RefeName Status ,
        d.EnglishName + ' ' + d.Name Detail ,
        id.Money ,
        id.ItemDetailID ,
        r2.RefeName Sort ,
        id.Queue ,
        r3.RefeName IsGive ,
        r4.RefeName IsReport,
		r5.RefeName IsShow
FROM    T_Pro_ItemDetail AS id
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = id.Status
                                      AND r1.RefeTypeID = 1
        LEFT JOIN T_Pro_Detail d ON id.DetailID = d.DetailID
        LEFT JOIN T_Sys_Refe r2 ON r2.Value = id.Sort
                                   AND r2.RefeTypeID = 5
        LEFT JOIN T_Sys_Refe r3 ON r3.Value = id.IsGive
                                   AND r3.RefeTypeID = 15
        LEFT JOIN T_Sys_Refe r4 ON r4.Value = id.IsReport
                                   AND r4.RefeTypeID = 13
        LEFT JOIN T_Sys_Refe r5 ON r5.Value = id.IsShow
                                   AND r5.RefeTypeID = 13
        where id.ItemID=" + itemId + where + "";
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "Money"));
        }
        #endregion

        #region 编辑项目明细
        public string GetItemDetailEdit(ItemDetailFormModel idfm)
        {
            ItemDetailModel idm = new ItemDetailModel();

            idm.DetailID = idfm.DetailID;
            idm.IsGive = idfm.IsGive;
            idm.ItemID = idfm.ItemID1;
            idm.Money = idfm.Money;
            idm.Queue = idfm.QueueID;
            idm.Remark = idfm.Remark1;
            idm.Sort = idfm.SortID;
            idm.ItemDetailID = idfm.ItemDetailID;
            idm.DetailID = idfm.DetailID;
            idm.IsReport = idfm.IsReport1;
            idm.IsShow = idfm.IsShow1;
            if (string.IsNullOrEmpty(idm.ItemDetailID))
            {
                idm.ItemDetailID = "0";
            }
            if (string.IsNullOrEmpty(idm.DetailID))
            {
                return "请选择收费项";
            }
            if (string.IsNullOrEmpty(idm.Money))
            {
                return "请输入金额";
            }
            //if (ValideteDetail(idm.ItemID, idm.ItemDetailID, idm.DetailID))
            //{
            //    return "此收费项已经存在";
            //}
            bool flag = false;
            if (idm.ItemDetailID.Equals("0"))
            {
                idm.CreateID = this.UserId.ToString();
                idm.CreateTime = DateTime.Now.ToString();
                idm.UpdateID = this.UserId.ToString();
                idm.UpdateTime = DateTime.Now.ToString();
                idm.Status = "1";
                if (ItemDetailBLL.InsertItemDetail(idm) > 0)
                {
                    flag = true;
                }
            }
            else
            {
                ItemDetailModel editIdm = GetItemDetailModel(idm.ItemDetailID);
                LogBLL.CreateLog("T_Pro_ItemDetail", this.UserId.ToString(), editIdm, idm);
                editIdm.IsShow = idm.IsShow;
                editIdm.IsReport = idm.IsReport;
                editIdm.Remark = idm.Remark;
                editIdm.Sort = idm.Sort;
                editIdm.IsGive = idm.IsGive;
                editIdm.Queue = idm.Queue;
                editIdm.DetailID = idm.DetailID;
                editIdm.Money = idm.Money;
                editIdm.UpdateID = this.UserId.ToString();
                editIdm.UpdateTime = DateTime.Now.ToString();
                if (ItemDetailBLL.UpdateItemDetail(editIdm) > 0)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                return "yes";
            }
            else
            {
                return "出现未知错误，请联系管理员";
            }
        }


        #endregion

        #region 获取明细实体
        private static ItemDetailModel GetItemDetailModel(string idtemDetailId)
        {
            string where = " and ItemDetailID=@ItemDetailID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemDetailID",idtemDetailId)
                };
            ItemDetailModel editIdm = ItemDetailBLL.ItemDetailModelByWhere(where, paras);
            return editIdm;
        }
        #endregion

        #region 表单验证项目名称
        public AjaxResult CheckItemDetailName()
        {
            string name = Request.Form["Value"];
            string itemDetailId = Request.Form["ID"];
            string parentId = Request.Form["TypeId"];

            string where = " AND Name=@Name And Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Name",name)
            };
            DetailModel dm = DetailBLL.DetailModelByWhere(where, paras);
            string detail = dm.DetailID;

            if (ValideteDetail(parentId, itemDetailId, detail))
            {
                return AjaxResult.Error("此收费项已经存在，请重新输入");
            }
            else
            {
                return AjaxResult.Success();
            }
        }


        #endregion

        #region 验证收费项是否重复
        public bool ValideteDetail(string itemId, string itemDetailID, string detail)
        {
            string where = " and ItemID=@ItemID and ItemDetailID<>@ItemDetailID and DetailID=@DetailID";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ItemID",itemId),
            new SqlParameter("@ItemDetailID",itemDetailID),
            new SqlParameter("@DetailID",detail)
            };
            DataTable dt = ItemDetailBLL.ItemDetailTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 变更状态
        public AjaxResult UpdateItemDetailStatus()
        {
            string itemDetailId = Request.Form["ID"];
            string status = Request.Form["Value"];
            ItemDetailModel idm = GetItemDetailModel(itemDetailId);
            idm.Status = status;
            idm.UpdateID = this.UserId.ToString();
            idm.UpdateTime = DateTime.Now.ToString();
            if (ItemDetailBLL.UpdateItemDetail(idm) > 0)
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error();
            }

        }
        #endregion

        #region 表单赋值
        public AjaxResult SelectItemDetail()
        {
            string id = Request.Form["ID"];
            string where = " and ItemDetailID=@ItemDetailID ";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ItemDetailID",id)
            };
            DataTable dt = ItemDetailBLL.ItemDetailTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }
        #endregion

        #region
        public AjaxResult GetMoney()
        {
            string id = Request.Form["ID"];
            if (!string.IsNullOrEmpty(id))
            {
                if (id.IndexOf(",", 0, 1) > -1)
                {
                    id = id.Substring(1, id.Length - 1);
                }
            }
            else
            {
                id = "0";
            }

            DataTable dt = ItemDetailBLL.ItemDetailGetMoney(id);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        public AjaxResult GetMoneyAll()
        {
            string id = Request.Form["ID"];
            DataTable dt = ItemDetailBLL.ItemDetailGetMoneyAll(id);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        #endregion

        #region 项目明细下拉树
        public string ItemDetailCombotree()
        {
            string itemId = Request.Form["ItemID"];
            if (string.IsNullOrEmpty(itemId))
                itemId = "0";
            string cmdText = @"SELECT  id.ItemDetailID id ,
        id.ItemID ParentID ,
        dl.Name + ' ' + CONVERT(NVARCHAR(10), id.Money) text
FROM    T_Pro_ItemDetail id
        LEFT JOIN  T_Pro_Detail dl ON dl.DetailID=id.DetailID
WHERE   id.Status = 1  
and id.ItemID IN({0})
UNION ALL
SELECT  ItemID id ,
        -1 ,
        Name text
FROM    T_Pro_Item
WHERE ItemID IN ({0}) ";
            cmdText = string.Format(cmdText, itemId);
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }
        #endregion

        #region 项目明细下拉列表
        public string ItemDetailCombobox()
        {
            string itemId = Request.Form["ItemID"];
            string cmdText = @"SELECT  id.ItemDetailID id ,
         dl.Name + ' ' + convert(NVARCHAR(10),id.Money) text
FROM     T_Pro_ItemDetail id
LEFT JOIN  T_Pro_Detail dl ON dl.DetailID=id.DetailID
WHERE   id.Status = 1
        AND id.ItemID = {0}";
            cmdText = string.Format(cmdText, itemId);
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }
        #endregion

        #region 项目明细下拉树（包含分类明细和项明细）

        /// <summary>
        /// 得到杂费收费项目下拉树
        /// </summary>
        /// <returns></returns>
        public string ItemDetailAllCombotree()
        {
            string deptId = Request.Form["DeptID"];
            string type = Request.Form["Type"];
            if (string.IsNullOrEmpty(deptId))
            {
                deptId = "0";
            }
            else if (string.IsNullOrEmpty(type))
            {
                type = "0";
            }
            string cmdText = @"SELECT  t1.id ,
        t1.ParentID ,
        t1.text
FROM    ( SELECT    'i_' + CONVERT(NVARCHAR(10), ItemID) id ,
                    'i_' + CONVERT(NVARCHAR(10), ParentID) ParentID ,
                    EnglishName + ' ' + Name text ,
                    EnglishName
          FROM      T_Pro_Item
          WHERE     Status = 1
                    AND ParentID <> -1
                    AND DeptID = {0}
                    AND Sort = {1}
                    AND IsShow = 1
          UNION ALL
          SELECT    'id_' + CONVERT(NVARCHAR(10), itemdetail.ItemDetailID) id ,
                    'i_' + CONVERT(NVARCHAR(10), item.ItemID) ParentID ,
                    detail.EnglishName + ' ' + detail.Name text ,
                    detail.EnglishName
          FROM      T_Pro_ItemDetail AS itemdetail
                    LEFT JOIN T_Pro_Item AS item ON itemdetail.ItemID = item.ItemID
                    LEFT JOIN T_Pro_Detail AS detail ON itemdetail.DetailID = detail.DetailID
          WHERE     item.Status = 1
                    AND itemdetail.Status = 1
                    AND detail.Status = 1
                    AND item.DeptID = {0}
                    AND item.Sort = {1}
                    AND item.IsShow = 1
                    AND itemdetail.IsShow = 1
        ) AS t1
ORDER BY t1.EnglishName ASC";
            cmdText = string.Format(cmdText, deptId, type);
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }

        #endregion


        public ActionResult GetItemDetailByItem(string ItemID) {
            if (string.IsNullOrEmpty(ItemID))
            {
                return null;
            }
            string cmdText = @"SELECT  dl.Name ,
        id.Money ShouldMoney ,
        id.ItemDetailID,
        '0.00' DiscountMoney
FROM    T_Pro_ItemDetail id
        LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
WHERE   id.Status = 1
        AND id.ItemID = " + ItemID+"";
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
    }
}
