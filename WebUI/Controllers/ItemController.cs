using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;
using BLL;
using System.Data;
using System.Data.SqlClient;
using Common;
using System.Collections;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace WebUI.Controllers
{
    public class ItemController : BaseController
    {
        //
        // GET: /Item/
        #region 页面加载
        public ActionResult ItemCheck()
        {
            return View();
        }
        public ActionResult ItemUpload()
        {
            return View();
        }
        public ActionResult ItemList()
        {
            return View();
        }
        public ActionResult ItemCopyDetail()
        {
            return View();
        }
        public ActionResult ItemEdit()
        {
            return View();
        }
        public ActionResult ItemCopy()
        {
            return View();
        }
        #endregion

        #region 收费项目列表
        public string GetItemTree()
        {
            string menuId = Request.Form["MenuID"];
            string status = Request.Form["Status"];
            string deptId = Request.Form["DeptID"];
            string cmdText = @"SELECT  i.ItemID id ,
        i.ParentID ,
        CASE i.Status
          WHEN 1 THEN i.EnglishName + ' ' + i.Name
          WHEN 2
          THEN '<span style=color:#ff0000>' + i.EnglishName + ' ' + i.Name
               + '(停用)</span>'
        END text ,
        i.EnglishName ,
        r1.RefeName IsPlan ,
        CASE CONVERT(NVARCHAR(23), i.StartTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(23), i.StartTime, 23)
        END StartTime ,
        CASE CONVERT(NVARCHAR(23), i.EndTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(23), i.EndTime, 23)
        END EndTime ,
        CONVERT(NVARCHAR(23), i.Queue) Queue ,
        r2.RefeName Sort ,
        CONVERT(NVARCHAR(23), i.Year) Year ,
        CASE CONVERT(NVARCHAR(23), i.LimitTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(23), i.LimitTime, 23)
        END LimitTime ,
        r3.RefeName MonthName ,
        i.Month ,
        CASE CONVERT(NVARCHAR(23), i.Year)
          WHEN '0' THEN '无限制'
          ELSE CONVERT(NVARCHAR(23), i.Year)
        END YearName ,
        r4.RefeName IsReport,
		r5.RefeName IsShow,
        r6.RefeName PlanLevel
FROM    T_Pro_Item AS i
        LEFT JOIN T_Pro_Item AS i1 ON i1.ItemID = i.ParentID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = i.IsPlan
                                   AND r1.RefeTypeID = 13
        LEFT JOIN T_Sys_Refe r2 ON r2.Value = i.Sort
                                   AND r2.RefeTypeID = 14
        LEFT JOIN T_Sys_Refe r3 ON r3.Value = i.Month
                                   AND r3.RefeTypeID = 16
        LEFT JOIN T_Sys_Refe r4 ON r4.Value = i.IsReport
                                   AND r4.RefeTypeID = 13
        LEFT JOIN T_Sys_Refe r5 ON r5.Value = i.IsShow
                                   AND r5.RefeTypeID = 13
        LEFT JOIN T_Sys_Refe r6 ON r6.Value = i.PlanLevel
                                   AND r6.RefeTypeID = 25
WHERE   i.Status IN ( {0} )
        AND i.DeptID = {1}
{2}
ORDER BY i.EnglishName ASC ,
        i.ItemID ASC";
            string temp = @"
UNION ALL
SELECT  -1 ,
        -2 ,
        '全部收费项目' ,
        '','','','','','','','','',0,'','','',''";
            if (deptId == "0")
            {
                temp = "";
            }
            cmdText = String.Format(cmdText, status, deptId, temp);
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }
        #endregion

        #region  编辑收费项目
        public AjaxResult GetItemEdit(ItemModel im)
        {
            string tempId = "0";
            if (string.IsNullOrEmpty(im.ItemID))
            {
                im.ItemID = "0";
            }
            if (string.IsNullOrEmpty(im.Name))
            {
                AjaxResult.Error("项目名称不能为空");
            }
            else
            {
                if (im.Name.Length > 32)
                {
                    return AjaxResult.Error("项目名称长度不能超过32个字符");
                }
            }
            if (string.IsNullOrEmpty(im.EnglishName))
            {
                return AjaxResult.Error("首字母不能为空");
            }
            else
            {
                if (im.EnglishName.Length > 4)
                {
                    return AjaxResult.Error("首字母长度能超过4个字符");
                }
            }
            if (string.IsNullOrEmpty(im.ParentID))
            {
                return AjaxResult.Error("请选择所属项目");
            }
            if (!string.IsNullOrEmpty(im.Remark))
            {
                im.Remark = im.Remark.Replace("\r\n", "<br />");
            }
            else
            {
                im.Remark = "";
            }
            if (CheckItemNameMethod(im.ItemID, im.DeptID, im.Sort, im.ParentID, im.Name))
            {
                return AjaxResult.Error("项目名称已经存在");
            }
            if (CheckItemIDIsParentIDMethod(im.ItemID, im.ParentID))
            {
                return AjaxResult.Error("不能选择自己或自己的下级作为所属项目");
            }

            if (string.IsNullOrEmpty(im.IsPlan))
            {
                return AjaxResult.Error("是否是方案不能为空");
            }
            if (string.IsNullOrEmpty(im.Queue))
            {
                return AjaxResult.Error("排序不能为空");
            }
            if (!string.IsNullOrEmpty(im.StartTime) && string.IsNullOrEmpty(im.EndTime))
            {
                return AjaxResult.Error("开始时间和结束时间不能其一为空");
            }
            if (string.IsNullOrEmpty(im.StartTime) && !string.IsNullOrEmpty(im.EndTime))
            {
                return AjaxResult.Error("开始时间和结束时间不能其一为空");
            }

            //if (im.IsPlan.Equals("2") && string.IsNullOrEmpty(im.LimitTime))
            //    return "缴费期数的缴费截止时间不能为空";

            if (im.StartTime == null)
                im.StartTime = "1900-01-01";
            if (im.EndTime == null)
                im.EndTime = "1900-01-01";
            if (im.LimitTime == null)
                im.LimitTime = "1900-01-01";

            bool flag = false;
            if (im.ItemID.Equals("0"))
            {
                im.Status = "1";
                im.CreateID = this.UserId.ToString();
                im.CreateTime = DateTime.Now.ToString();
                im.UpdateID = this.UserId.ToString();
                im.UpdateTime = DateTime.Now.ToString();
                tempId = ItemBLL.InsertItem(im).ToString();
                if (!tempId.Equals("-1"))
                {
                    flag = true;
                }
            }
            else
            {
                ItemModel editim = GetItemModel(im.ItemID);
                LogBLL.CreateLog("T_Pro_Item", this.UserId.ToString(), editim, im);
                editim.PlanLevel = im.PlanLevel;
                editim.IsShow = im.IsShow;
                editim.IsReport = im.IsReport;
                editim.Queue = im.Queue;
                editim.IsPlan = im.IsPlan;
                editim.Sort = im.Sort;
                editim.StartTime = im.StartTime;
                editim.EndTime = im.EndTime;
                editim.Year = im.Year;
                editim.Month = im.Month;
                editim.LimitTime = im.LimitTime;
                editim.UpdateID = this.UserId.ToString();
                editim.UpdateTime = DateTime.Now.ToString();
                editim.ParentID = im.ParentID;
                editim.Name = im.Name;
                editim.Remark = im.Remark;
                editim.EnglishName = im.EnglishName;
                tempId = editim.ItemID;

                if (ItemBLL.UpdateItem(editim) > 0)
                {
                    string where = " and ItemID=@ItemID and Status=1";
                    SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@ItemID",tempId)
                };
                    DataTable dt = ItemDetailBLL.ItemDetailTableByWhere(where, paras, "");
                    foreach (DataRow dr in dt.Rows)
                    {
                        string where1 = " and ItemDetailID=@ItemDetailID";
                        SqlParameter[] paras1 = new SqlParameter[] {
                        new SqlParameter("@ItemDetailID",dr["ItemDetailID"].ToString())
                    };
                        ItemDetailModel idm = ItemDetailBLL.ItemDetailModelByWhere(where1, paras1);
                        idm.IsShow = im.IsShow;
                        idm.UpdateID = this.UserId.ToString();
                        idm.UpdateTime = DateTime.Now.ToString();
                        ItemDetailBLL.UpdateItemDetail(idm);
                    }
                    flag = true;
                }
            }
            if (flag)
            {
                return AjaxResult.Success(tempId, "success");
            }
            else
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
        }
        #endregion

        #region 返回收费项目Model
        /// <summary>
        /// 返回收费项目Model
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static ItemModel GetItemModel(string itemId)
        {
            string where = " and ItemID=@ItemID ";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter ("@ItemID",itemId)
            };
            ItemModel im = ItemBLL.ItemModelByWhere(where, paras);
            return im;
        }
        #endregion

        #region  修改时表单赋值
        public AjaxResult SelectItem()
        {
            string itemId = Request.Form["ID"];
            string where = " and ItemID=@ItemID";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ItemID",itemId)
            };
            DataTable dt = ItemBLL.ItemTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }
        #endregion

        #region 变更状态
        public AjaxResult GetUpdateStatus()
        {
            string itemId = Request.Form["ID"];
            string status = Request.Form["Value"];
            ItemModel im = GetItemModel(itemId);
            im.UpdateID = this.UserId.ToString();
            im.UpdateTime = DateTime.Now.ToString();
            im.Status = status;
            if (ItemBLL.UpdateItem(im) > 0)
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
        }
        #endregion

        #region 启用停用验证节点

        /// <summary>
        /// 启用、停用判断父、子节点状态
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckIsHandle()
        {
            string itemId = Request.Form["ItemID"];
            string status = Request.Form["Status"];
            if (status == "2")
            {
                string where = " AND Status = 1 AND ParentID = @ItemID  ";
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@ItemID", itemId)
                };
                DataTable dt = ItemBLL.ItemTableByWhere(where, paras, "");
                if (dt.Rows.Count > 0)
                {
                    return AjaxResult.Error("请先停用所有子项目");
                }
                else
                {
                    return AjaxResult.Success();
                }
            }
            else if (status == "1")
            {
                string where = " AND Status = 2 AND ItemID = (Select ParentID from T_Pro_Item WHERE ItemID = @ItemID)";
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@ItemID", itemId)
                };
                DataTable dt = ItemBLL.ItemTableByWhere(where, paras, "");
                if (dt.Rows.Count > 0)
                {
                    return AjaxResult.Error("请先启用父项目");
                }
                else
                {
                    return AjaxResult.Success();
                }
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }
        #endregion

        #region 验证项目ID是否与父级ID相同的方法
        /// <summary>
        /// 验证部门ID是否与父级ID相同的方法
        /// </summary>
        /// <param name="DeptID"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public bool CheckItemIDIsParentIDMethod(string itemId, string parentId)
        {
            string itemString = ",";
            DataTable dt = ItemBLL.SelectChildrenItemID(itemId);
            foreach (DataRow dr in dt.Rows)
            {
                itemString += dr["ItemID"].ToString() + ",";
            }

            if (itemString.IndexOf("," + parentId + ",") > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region  验证项目ID是否与父级ID相同
        /// <summary>
        /// 验证项目ID是否与父级ID相同
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckItemIDIsParentID()
        {
            string itemId = Request.Form["ID"];
            string parentId = Request.Form["TypeId"];

            if (CheckItemIDIsParentIDMethod(itemId, parentId))
            {
                return AjaxResult.Error("不能选择自己或自己的下级作为所属项目");
            }
            else
            {
                return AjaxResult.Success();
            }
        }
        #endregion

        #region 表单验证项目名称
        public AjaxResult CheckItemName()
        {
            string name = Request.Form["Value"];
            string itemId = Request.Form["ID"];
            string typeId = Request.Form["TypeId"];
            string[] typeIdArr = typeId.Split(",");
            string deptId = typeIdArr[0];
            string sort = typeIdArr[1];
            string parentId = typeIdArr[2];

            if (CheckItemNameMethod(itemId, deptId, sort, parentId, name))
            {
                return AjaxResult.Error("该项目名已经存在，请重新输入");
            }
            else
            {
                return AjaxResult.Success();
            }
        }


        #endregion

        #region 验证名称的方法
        public bool CheckItemNameMethod(string itemId, string deptId, string sort, string parentId, string name)
        {
            string tempWhere = "";
            if (sort == "3" || sort == "4" || sort == "5" || sort == "6" || sort == "7" || sort == "8")
            {
                tempWhere = " AND ParentID = @ParentID AND DeptID = @DeptID";
            }
            else
            {
                tempWhere = " AND DeptID = @DeptID";
            }
            string where = " AND ItemID <> @ItemID AND Name = @Name" + tempWhere;
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@DeptID", deptId),
                new SqlParameter("@Name", name),
                new SqlParameter("@ParentID", parentId)
            };
            DataTable dt = ItemBLL.ItemTableByWhere(where, paras, "");
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

        #region 收费项下拉
        public string ItemCombotree()
        {
            string deptId = Request.Form["DeptID"];
            string type = Request.Form["Type"];
            if (string.IsNullOrEmpty(deptId))
                deptId = "0";
            else if (string.IsNullOrEmpty(type))
                type = "0";
            string cmdText = @"SELECT  ItemID id ,
        ParentID ,
        EnglishName + ' ' + Name text ,
        EnglishName
FROM    T_Pro_Item
WHERE   Status = 1
        AND DeptID = {0} and Sort={1}
ORDER BY EnglishName ASC";
            cmdText = string.Format(cmdText, deptId, type);
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }
        public string ItemCombotreeMore()
        {
            string deptId = Request.Form["DeptID"];
            if (string.IsNullOrEmpty(deptId))
                deptId = "0";
            string cmdText = @"SELECT  ItemID id ,
        ParentID ,
        EnglishName + ' ' + Name text ,
        EnglishName
FROM    T_Pro_Item
WHERE   Status = 1
        AND DeptID = {0} 
 UNION ALL SELECT -1,-2,'全部收费项目',''
ORDER BY EnglishName ASC
";
            cmdText = string.Format(cmdText, deptId);
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }

        public string ItemDetailCombotree()
        {
            string deptId = Request.Form["DeptID"];
            string cmdText = @"SELECT  'i_' + CONVERT(NVARCHAR(10), ItemID) id ,
        'i_' + CONVERT(NVARCHAR(10), ParentID) ParentID ,
        EnglishName + ' ' + Name text ,
        EnglishName ,
        Queue
FROM    T_Pro_Item
WHERE   Status = 1
        AND DeptID = {0}
UNION ALL
SELECT  'i_-1' ,
        'i_-2' ,
        '全部收费项目' ,
        '' ,
        0
UNION ALL
SELECT  CONVERT(NVARCHAR(10), id.ItemDetailID) id ,
        'i_' + CONVERT(NVARCHAR(10), id.ItemID) ParentID ,
        dl.EnglishName +' '+ dl.Name text ,
        dl.EnglishName ,
        id.Queue
FROM    T_Pro_ItemDetail id
        LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
        LEFT JOIN T_Pro_Item i ON i.ItemID = id.ItemID
		WHERE   id.Status = 1
        AND i.DeptID = {0}
ORDER BY Queue ASC";
            cmdText = string.Format(cmdText, deptId);
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }
        #endregion

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public string GetItemCopy()
        {
            string toDeptId = Request.Form["ToDeptID"];
            string toItemId = Request.Form["ToItemID"];
            string fromItemId = Request.Form["FromItemID"];
            string[] array = fromItemId.Split(",");
            try
            {
                for (int i = 0; i < array.Length; i++)
                {
                    InsertItemInfomation(array[i], toItemId, toDeptId);
                }
                return "yes";
            }
            catch (Exception)
            {
                return "出现未知错误，请联系管理员";

            }
        }

        private static DataTable GetItemDetailTable(string itemId)
        {

            string where = " and ItemID=@ItemID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@ItemID",itemId)
                    };
            return ItemDetailBLL.ItemDetailTableByWhere(where, paras, "");
        }

        private static DataTable GetChuildItem(string itemId)
        {
            string where = " and ParentID=@ParentID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ParentID",itemId)
                };
            return ItemBLL.ItemTableByWhere(where, paras, "");
        }



        private void InsertItemInfomation(string itemId, string parentId, string deptId)
        {

            ItemModel im = ItemBLL.GetItemModel(itemId);
            ItemModel newim = new ItemModel();
            newim.CreateID = this.UserId.ToString();
            newim.CreateTime = DateTime.Now.ToString();
            newim.DeptID = deptId;
            newim.EndTime = im.EndTime;
            newim.EnglishName = im.EnglishName;
            newim.IsPlan = im.IsPlan;
            newim.LimitTime = im.LimitTime;
            newim.Month = im.Month;
            newim.IsReport = im.IsReport;
            newim.IsShow = im.IsShow;
            newim.Name = GetItemName(im.Name, parentId, 1, deptId);

            newim.PlanLevel = im.PlanLevel;
            newim.ParentID = parentId;
            newim.Queue = im.Queue;
            newim.Remark = im.Remark;
            newim.Sort = im.Sort;
            newim.StartTime = im.StartTime;
            newim.Status = im.Status;
            newim.UpdateID = this.UserId.ToString();
            newim.UpdateTime = DateTime.Now.ToString();
            newim.Year = im.Year;
            newim.ItemID = ItemBLL.InsertItem(newim).ToString();
            InsertItemDetailInfomation(itemId, newim.ItemID);//添加收费项
            DataTable dt = GetChuildItem(itemId);
            foreach (DataRow dr in dt.Rows)
            {
                InsertItemInfomation(dr["ItemID"].ToString(), newim.ItemID, deptId);
            }
        }

        private void InsertItemDetailInfomation(string itemId, string newItemId)
        {
            DataTable dt = GetItemDetailTable(itemId);
            foreach (DataRow dr in dt.Rows)
            {
                ItemDetailModel idm = new ItemDetailModel();
                idm.CreateID = this.UserId.ToString();
                idm.CreateTime = DateTime.Now.ToString();
                idm.DetailID = dr["DetailID"].ToString();
                idm.IsGive = dr["IsGive"].ToString();
                idm.IsReport = dr["IsReport"].ToString();
                idm.IsShow = dr["IsShow"].ToString();
                idm.ItemID = newItemId;
                idm.Money = dr["Money"].ToString();
                idm.Queue = dr["Queue"].ToString();
                idm.UpdateID = this.UserId.ToString();
                idm.UpdateTime = DateTime.Now.ToString();
                idm.Remark = dr["Remark"].ToString();
                idm.Sort = dr["Sort"].ToString();
                idm.Status = dr["Status"].ToString();
                ItemDetailBLL.InsertItemDetail(idm);
            }
        }
        /// <summary>
        /// 返回收费项目名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        private string GetItemName(string name, string parentId, int i, string deptId)
        {
            string where = " and  Name=@Name and ParentID=@ParentID and DeptID=@DeptID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Name",name),
                new SqlParameter("@ParentID",parentId),
                new SqlParameter("@DeptID",deptId)
            };
            DataTable dt = ItemBLL.ItemTableByWhere(where, paras, "");
            if (dt.Rows.Count == 0)
            {
                return name;
            }
            else
            {
                if (i == 1)
                {
                    name = name + "- 副本";
                }
                else if (i == 2)
                {
                    name = name + " (" + i + ")";
                }
                else
                {
                    name = name.Replace("(" + (i - 1).ToString() + ")", "(" + i.ToString() + ")");
                }
                i++;
                return GetItemName(name, parentId, i, deptId);
            }
        }

        ///// <summary>
        ///// 根据方案id返回方案下的收费项明细
        ///// </summary>
        ///// <returns></returns>
        //public AjaxResult GetItemDetailInfomation()
        //{
        //    string itemId = Request.Form["ID"];
        //    string where = " And ParentID=@ParentID";
        //    SqlParameter[] paras = new SqlParameter[] {
        //        new SqlParameter("@ParentID",itemId)
        //    };
        //    DataTable dt = ItemBLL.ItemTableByWhere(where, paras, " Order By Queue ASC");
        //    return AjaxResult.Success(JsonHelper.DataTableToJson(dt), "success");
        //}

        public string GetCopyDetail(CopyDetailModel cdm)
        {
            if (cdm.fromItemDetailId.Length == 0)
            {
                return "请选择收费明细";
            }
            if (string.IsNullOrEmpty(cdm.toDeptId))
            {
                return "请选择复制到的校区";
            }
            if (string.IsNullOrEmpty(cdm.toItemId))
            {
                return "请选择复制到的收费项";
            }
            try
            {
                for (int i = 0; i < cdm.fromItemDetailId.Length; i++)
                {
                    string where = " and ItemDetailID=@ItemDetailID";
                    SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@ItemDetailID", cdm.fromItemDetailId[i])
                     };
                    ItemDetailModel idm = ItemDetailBLL.ItemDetailModelByWhere(where, paras);

                    idm.CreateID = this.UserId.ToString();
                    idm.CreateTime = DateTime.Now.ToString();
                    idm.ItemID = cdm.toItemId;
                    idm.UpdateID = this.UserId.ToString();
                    idm.UpdateTime = DateTime.Now.ToString();
                    ItemDetailBLL.InsertItemDetail(idm);
                }
                return "yes";
            }
            catch (Exception)
            {

                return "出现未知错误，请联系管理员";
            }

        }
        /// <summary>
        /// 数据对比
        /// </summary>
        /// <returns></returns>
        public AjaxResult DataContrast()
        {
            var standard = StandardData(GetItemTable("77"));
            DataTable dt = ItemBLL.GetItemDeptID();//获取所有主体
            List<ItemModels> itemlist = new List<ItemModels>();//初始化集合
            List<ItemDetailModels> detailist = new List<ItemDetailModels>();
            foreach (DataRow dr in dt.Rows)
            {
                var tempList = StandardData(GetItemTable(dr["DeptID"].ToString()));//获取当前主体下的杂费收费项
                var errorlist1 = tempList.Where(O => !standard.Exists(S => O.Name.Equals(S.Name) && O.IsReport.Equals(S.IsReport))).ToList();
                var errorlist2 = standard.Where(O => !tempList.Exists(S => S.Name.Equals(O.Name) && S.IsReport.Equals(O.IsReport))).ToList();
                if (tempList.Count == 0)
                {
                    foreach (var item in standard)
                    {
                        itemlist.Add(AddItem(item, dr["Name"].ToString(), "少"));
                    }

                }
                foreach (var item in errorlist1)
                {
                    itemlist.Add(AddItem(item, "", "多"));//将数据不一致的Item加入集合
                }
                foreach (var item in errorlist2)
                {
                    itemlist.Add(AddItem(item, dr["Name"].ToString(), "少"));//将数据不一致的Item加入集合
                }
                var righelist = tempList.Where(O => standard.Exists(S => S.Name.Equals(O.Name) && S.IsReport.Equals(O.IsReport))).ToList();//数据一致的数据
                if (righelist != null)
                {
                    foreach (var i in standard)
                    {
                        var standardDetail = GetItemDetailList(i.ItemID);//标准的收费明细
                        var templist = righelist.Where(O => O.IsReport.Equals(i.IsReport) && O.Name.Equals(i.Name)).FirstOrDefault();//待比对的收费项

                        List<ItemDetailModels> tempdetaillist = null;
                        if (templist != null)
                        {
                            tempdetaillist = GetItemDetailList(templist.ItemID);//待比对的收费项目明细
                        }
                        else
                        {
                            foreach (var item in standardDetail)
                            {
                                ItemDetailModels im = new ItemDetailModels();
                                im.IsReport = item.IsReport;
                                im.ItemDetailID = item.ItemDetailID;
                                im.Name = item.Name;
                                im.ParentName = item.ParentName;
                                im.Type = "少";
                                im.DetailID = item.DetailID;
                                im.DeptName = dr["Name"].ToString();
                                detailist.Add(im);
                            }
                        }

                        List<ItemDetailModels> errordetaillist1 = null;

                        List<ItemDetailModels> errordetaillist2 = null;
                        if (tempdetaillist != null)
                        {
                            errordetaillist1 = standardDetail.Where(O => !tempdetaillist.Exists(S => S.IsReport.Equals(O.IsReport) && S.DetailID.Equals(O.DetailID))).ToList();
                            errordetaillist2 = tempdetaillist.Where(O => !standardDetail.Exists(S => S.IsReport.Equals(O.IsReport) && S.DetailID.Equals(O.DetailID))).ToList();
                        }
                        if (errordetaillist1 != null)
                        {
                            foreach (var dl in errordetaillist1)
                            {
                                ItemDetailModels idm = new ItemDetailModels();
                                idm.DeptName = dr["Name"].ToString();
                                idm.DetailID = dl.DetailID;
                                idm.IsReport = dl.IsReport;
                                idm.ItemDetailID = dl.ItemDetailID;
                                idm.Name = dl.Name;
                                idm.ParentName = dl.ParentName;
                                idm.Type = "少";
                                detailist.Add(idm);
                            }
                        }
                        if (errordetaillist2 != null)
                        {
                            foreach (var dl in errordetaillist2)
                            {
                                ItemDetailModels idm = new ItemDetailModels();
                                idm.DeptName = dr["Name"].ToString();
                                idm.DetailID = dl.DetailID;
                                idm.IsReport = dl.IsReport;
                                idm.ItemDetailID = dl.ItemDetailID;
                                idm.Name = dl.Name;
                                idm.ParentName = dl.ParentName;
                                idm.Type = "多";
                                detailist.Add(idm);
                            }
                        }

                    }
                }
            }
            string itemstring = "{\"rows\":" + OtherHelper.JsonSerializer(itemlist) + "}";
            string detailstring = "{\"rows\":" + OtherHelper.JsonSerializer(detailist) + "}";
            return AjaxResult.Success(itemstring, detailstring);
        }

        public List<ItemModels> StandardData(DataTable dt)
        {
            List<ItemModels> list = new List<ItemModels>();
            foreach (DataRow dr in dt.Rows)
            {
                ItemModels im = new ItemModels();
                im.ItemID = dr["ItemID"].ToString();
                im.IsReport = dr["IsReport"].ToString();
                im.Name = dr["Name"].ToString();
                im.DeptName = dr["DeptName"].ToString();
                im.ParentName = dr["ParentName"].ToString();
                list.Add(im);
            }
            return list;
        }
        /// <summary>
        /// 获取Item DataTable
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        private DataTable GetItemTable(string deptId)
        {
            string where = " and i.DeptID=@DeptID and i.Sort=2 and i.Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptID",deptId)
            };
            return ItemBLL.ItemTableByWhere2(where, paras, "");
        }

        public List<ItemDetailModels> GetItemDetailList(string itemId)
        {
            string where = @" and id.ItemID=@ItemID and id.Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemID",itemId)
                };
            DataTable dt = ItemBLL.ItemDetailTableByWhere(where, paras, "");
            List<ItemDetailModels> list = new List<ItemDetailModels>();
            foreach (DataRow dr in dt.Rows)
            {
                ItemDetailModels idm = new ItemDetailModels();
                idm.ItemDetailID = dr["ItemDetailID"].ToString();
                idm.Name = dr["Name"].ToString();
                idm.DeptName = dr["DeptName"].ToString();
                idm.IsReport = dr["IsReport"].ToString();
                idm.DeptName = dr["DeptName"].ToString();
                idm.DetailID = dr["DetailID"].ToString();
                idm.ParentName = dr["ParentName"].ToString();
                list.Add(idm);
            }
            return list;
        }

        public string GetDeptName(string deptId)
        {
            string where = " and DeptID IN (" + deptId + ")";

            return JsonGridData.GetGridJSON(DeptBLL.DeptTableByWhere(where, null, ""));
        }

        /// <summary>
        /// 生成收费项，用于杂费报表
        /// </summary>
        /// <returns></returns>
        public AjaxResult MakeData()
        {
            iReportConfigBLL.DeleteiReportConfig();
            string cmdText1 = @"SELECT  'i_' Sort ,
        i.ItemID ID
FROM    T_Pro_Item i
WHERE   Status = 1
        AND i.ParentID <> -1
        AND i.DeptID IN ( SELECT    DeptID
                          FROM      dbo.GetChildrenDeptID({0}) )
        AND i.Sort = 2
        AND i.IsReport = 1
        AND ( ( SELECT  COUNT(ItemDetailID)
                FROM    T_Pro_ItemDetail
                WHERE   Status = 1
                        AND ItemID = i.ItemID
              ) > 0
              OR ( SELECT   COUNT(ItemDetailID)
                   FROM     T_Pro_ItemDetail
                   WHERE    Status = 1
                            AND ItemID IN (
                            SELECT  ItemID
                            FROM    dbo.GetChildrenItemID(i.ItemID) )
                 ) > 0
            )
UNION ALL
SELECT  'id_' ,
        id.ItemDetailID
FROM    T_Pro_ItemDetail id
WHERE   id.Status = 1
        AND id.IsReport = 1
        AND id.ItemID IN (
        SELECT  ItemID
        FROM    T_Pro_Item
        WHERE   Status = 1
                AND ParentID <> -1
                AND DeptID IN ( SELECT  DeptID
                                FROM    dbo.GetChildrenDeptID({0}) )
                AND Sort = 2 )";
            cmdText1 = string.Format(cmdText1, "77");
            List<Hashtable> list1 = JsonData.GetArray(cmdText1);
            for (int i = 0; i < list1.Count; i++)
            {
                string cmdText2 = "";
                if (list1[i]["Sort"].ToString() == "id_")
                {
                    cmdText2 = @"SELECT  DetailID
FROM    T_Pro_ItemDetail
WHERE   Status = 1
        AND ItemDetailID = {0}";
                }
                else if (list1[i]["Sort"].ToString() == "i_")
                {
                    cmdText2 = @"SELECT  DetailID
FROM    T_Pro_ItemDetail
WHERE   Status = 1
        AND ItemID IN ( SELECT  ItemID
                        FROM    dbo.GetChildrenItemID({0}) )";
                }
                cmdText2 = string.Format(cmdText2, list1[i]["ID"].ToString());
                List<Hashtable> list2 = JsonData.GetArray(cmdText2);
                for (int j = 0; j < list2.Count; j++)
                {
                    iReportConfigModel rcm = new iReportConfigModel();
                    rcm.Sort = list1[i]["Sort"].ToString();
                    rcm.ID = list1[i]["ID"].ToString();
                    rcm.DetailID = list2[j]["DetailID"].ToString();
                    iReportConfigBLL.InsertiReportConfig(rcm);
                }
            }
            return AjaxResult.Success();
        }

        public ItemModels AddItem(ItemModels item, string deptName, string type)
        {
            ItemModels im = new ItemModels();
            im.Name = item.Name;
            im.ParentName = item.ParentName;
            im.DeptName = deptName == "" ? item.DeptName : deptName;
            im.Type = type;
            im.IsReport = item.IsReport;
            im.ItemID = item.ItemID;
            return im;
        }

        public void DownLoadItem(string itemId)
        {
            List<DataTable> ds = new List<DataTable>();

            DataTable dt1 = ItemBLL.GetItemDataTable(itemId);
            ds.Add(dt1);
            string numItemId = string.Empty;
            foreach (DataRow dr in dt1.Rows)
            {
                numItemId += dr["Id"] + ",";
            }
            numItemId = numItemId.Substring(0, numItemId.Length - 1);
            DataTable dt2 = ItemDetailBLL.GetItemDetailDataTable(numItemId);
            ds.Add(dt2);

            Response.ClearContent();
            Response.BufferOutput = true;
            Response.Charset = "utf-8";
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.AppendHeader("Content-Disposition", "attachment;filename="+Server.UrlEncode("孟宪会Excel表格测试")+".xls");  
            // 采用下面的格式，将兼容 Excel 2003,Excel 2007, Excel 2010。  

            string FileName = "学费缴费方案";
            if (!string.IsNullOrEmpty(Request.UserAgent))
            {
                // firefox 里面文件名无需编码。  
                if (!(Request.UserAgent.IndexOf("Firefox") > -1 && Request.UserAgent.IndexOf("Gecko") > -1))
                {
                    FileName = Server.UrlEncode(FileName);
                }
            }
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.Write("<?xml version='1.0'?><?mso-application progid='Excel.Sheet'?>");
            Response.Write(@"<Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet'  
              xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel'  
              xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet' xmlns:html='http://www.w3.org/TR/REC-html40'>");
            Response.Write(@"<DocumentProperties xmlns='urn:schemas-microsoft-com:office:office'>");
            Response.Write(@"<Author></Author><LastAuthor></LastAuthor>  
                <Created>2010-09-08T14:07:11Z</Created><Company>mxh</Company><Version>1990</Version>");
            Response.Write("</DocumentProperties>");
            Response.Write(@"<Styles><Style ss:ID='Default' ss:Name='Normal'><Alignment ss:Vertical='Center'/>  
                <Borders/><Font ss:FontName='宋体' x:CharSet='134' ss:Size='12'/><Interior/><NumberFormat/><Protection/></Style>");
            //定义标题样式      
            Response.Write(@"<Style ss:ID='Header'><Borders><Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'/>  
               <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'/>  
               <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'/>  
               <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'/></Borders>  
               <Font ss:FontName='宋体' x:CharSet='134' ss:Size='12'  ss:Bold='1'/></Style>");

            //定义边框  
            Response.Write(@"<Style ss:ID='border'><NumberFormat ss:Format='@'/><Borders>  
              <Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'/>  
              <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'/>  
              <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'/>  
              <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'/></Borders></Style>");
            Response.Write("</Styles>");
            for (int i = 0; i < ds.Count; i++)
            {
                string sheetName = string.Empty;
                if (i.Equals(0))
                {
                    sheetName = "缴费方案";
                }
                else if (i.Equals(1))
                {
                    sheetName = "缴费明细";
                }
                Response.Write("<Worksheet ss:Name='" + sheetName + "'>");
                Response.Write("<Table x:FullColumns='1' x:FullRows='1'>");
                Response.Write("\r\n<Row ss:AutoFitHeight='1'>");
                for (int j = 0; j < ds[i].Columns.Count; j++)
                {
                    Response.Write("<Cell ss:StyleID='Header'><Data ss:Type='String'>" + ds[i].Columns[j].ColumnName + "</Data></Cell>");
                }
                Response.Write("\r\n</Row>");
                for (int k = 0; k < ds[i].Rows.Count; k++)
                {
                    Response.Write("<Row>");
                    for (int j = 0; j < ds[i].Columns.Count; j++)
                    {
                        Response.Write("<Cell ss:StyleID='border'><Data ss:Type='String'>" + ds[i].Rows[k][j] + "</Data></Cell>");
                    }

                    Response.Write("</Row>");
                }

                Response.Write("</Table>");
                Response.Write("</Worksheet>");
                Response.Flush();
            }

            Response.Write("</Workbook>");
            Response.End();
        }

        public AjaxResult UploadItem(string filePath, string upDeptId, string upItemId)
        {
            try
            {
                List<DataTable> list = OtherHelper.UploadExeclFile(filePath);
                if (list.Count > 2 || list.Count < 2)
                {
                    return AjaxResult.Error("Excel错误");
                }
                string itemJson = JsonHelper.DataTableToJson(list[0]);
                string detailJson = JsonHelper.DataTableToJson(list[1]);
                List<ItemUploadModel> item = JsonConvert.DeserializeObject<List<ItemUploadModel>>(itemJson);
                List<ItemDetailUploadModel> detail = JsonConvert.DeserializeObject<List<ItemDetailUploadModel>>(detailJson);
                if (item.Count == 0)
                {
                    return AjaxResult.Error("方案数据不能为空");
                }
                if (detail.Count == 0)
                {
                    return AjaxResult.Error("费用明细不能为空");
                }
                var plan = item.Where(O => O.IsPlan.Equals("1"));//所有的方案
                foreach (var pn in plan)
                {
                    if (validateItemName(upDeptId, pn.Text, upItemId))
                    {
                        return AjaxResult.Error(pn.Text + "重复");
                    }
                }

                foreach (var pn in plan)
                {
                    int parentId = InsertItem(upDeptId, upItemId, pn);
                    var numItem = item.Where(O => O.Pid.Equals(pn.Id));
                    foreach (var ni in numItem)
                    {
                        int itemId = InsertItem(upDeptId, parentId.ToString(), ni);
                        var itemdetil = detail.Where(O => O.Pid.Equals(ni.Id));
                        foreach (var id in itemdetil)
                        {
                            ItemDetailModel idm = new ItemDetailModel();
                            idm.CreateID = UserId.ToString();
                            idm.CreateTime = DateTime.Now.ToString();
                            idm.DetailID = id.Text;
                            idm.IsGive = id.IsGive;
                            idm.IsReport = id.IsReport;
                            idm.IsShow = id.IsShow;
                            idm.ItemID = itemId.ToString();
                            idm.Money = id.Money;
                            idm.Queue = id.Queue;
                            idm.Remark = id.Remark;
                            idm.Sort = id.Sort;
                            idm.Status = "1";
                            idm.UpdateID = UserId.ToString();
                            idm.UpdateTime = DateTime.Now.ToString();
                            ItemDetailBLL.InsertItemDetail(idm);
                        }
                    }
                }
                return AjaxResult.Success();
            }
            catch (Exception ex)
            {

                return AjaxResult.Error(ex.Message);
            }
        }
        /// <summary>
        /// 验证方案是否重复
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="name"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private bool validateItemName(string deptId, string name, string parentId)
        {
            string where = " and DeptID=@DeptID and Name=@Name and ParentID=@ParentID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptID",deptId),
                new SqlParameter("@Name",name),
                new SqlParameter("@ParentID",parentId)
            };
            DataTable dt = ItemBLL.ItemTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        private int InsertItem(string upDeptId, string upItemId, ItemUploadModel pn)
        {
            ItemModel im = new ItemModel();
            im.CreateID = UserId.ToString();
            im.CreateTime = DateTime.Now.ToString();
            im.UpdateID = UserId.ToString();
            im.UpdateTime = DateTime.Now.ToString();
            im.DeptID = upDeptId;
            im.EndTime = pn.EndTime;
            im.EnglishName = pn.EnglishName;
            im.IsPlan = pn.IsPlan;
            im.IsReport = pn.IsReport;
            im.IsShow = pn.IsShow;
            im.LimitTime = pn.LimitTime;
            im.Month = pn.Month;
            im.Name = pn.Text;
            im.ParentID = upItemId;
            im.PlanLevel = pn.PlanLevel;
            im.Queue = pn.Queue;
            im.Remark = pn.Remark;
            im.Sort = pn.Sort;
            im.StartTime = pn.StartTime;
            im.Status = "1";
            im.Year = pn.Year;
            return ItemBLL.InsertItem(im);
        }

        public ActionResult GetItemNum(string ItemID)
        {
            if (string.IsNullOrEmpty(ItemID))
            {
                return null;
            }
            string cmdText = "SELECT ItemID,Name FROM T_Pro_Item WHERE ParentID="+ ItemID + " and Status=1";
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        
    }
}
