using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class ItemDetailBLL
    {
        public static int InsertItemDetail(ItemDetailModel idm)
        {
            string cmdText = @"INSERT INTO T_Pro_ItemDetail
(Status
,ItemID
,DetailID
,Sort
,Money
,IsGive
,IsReport
,IsShow
,Queue
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@ItemID
,@DetailID
,@Sort
,@Money
,@IsGive
,@IsReport
,@IsShow
,@Queue
,@Remark
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", idm.Status),
new SqlParameter("@ItemID", idm.ItemID),
new SqlParameter("@DetailID", idm.DetailID),
new SqlParameter("@Sort", idm.Sort),
new SqlParameter("@Money", idm.Money),
new SqlParameter("@IsGive", idm.IsGive),
new SqlParameter("@IsReport", idm.IsReport),
new SqlParameter("@IsShow", idm.IsShow),
new SqlParameter("@Queue", idm.Queue),
new SqlParameter("@Remark", idm.Remark),
new SqlParameter("@CreateID", idm.CreateID),
new SqlParameter("@CreateTime", idm.CreateTime),
new SqlParameter("@UpdateID", idm.UpdateID),
new SqlParameter("@UpdateTime", idm.UpdateTime)
};
            int result = Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
            if (result > 0)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }

        public static int UpdateItemDetail(ItemDetailModel idm)
        {
            string cmdText = @"UPDATE T_Pro_ItemDetail SET
Status=@Status
,ItemID=@ItemID
,DetailID=@DetailID
,Sort=@Sort
,Money=@Money
,IsGive=@IsGive
,IsReport=@IsReport
,IsShow=@IsShow
,Queue=@Queue
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE ItemDetailID=@ItemDetailID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ItemDetailID", idm.ItemDetailID),
new SqlParameter("@Status", idm.Status),
new SqlParameter("@ItemID", idm.ItemID),
new SqlParameter("@DetailID", idm.DetailID),
new SqlParameter("@Sort", idm.Sort),
new SqlParameter("@Money", idm.Money),
new SqlParameter("@IsGive", idm.IsGive),
new SqlParameter("@IsReport", idm.IsReport),
new SqlParameter("@IsShow", idm.IsShow),
new SqlParameter("@Queue", idm.Queue),
new SqlParameter("@Remark", idm.Remark),
new SqlParameter("@CreateID", idm.CreateID),
new SqlParameter("@CreateTime", idm.CreateTime),
new SqlParameter("@UpdateID", idm.UpdateID),
new SqlParameter("@UpdateTime", idm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(idm.ItemDetailID);
            }
            else
            {
                return -1;
            }
        }

        public static ItemDetailModel ItemDetailModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            ItemDetailModel idm = new ItemDetailModel();
            string cmdText = "SELECT * FROM T_Pro_ItemDetail WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                idm.ItemDetailID = dt.Rows[0]["ItemDetailID"].ToString();
                idm.Status = dt.Rows[0]["Status"].ToString();
                idm.ItemID = dt.Rows[0]["ItemID"].ToString();
                idm.DetailID = dt.Rows[0]["DetailID"].ToString();
                idm.Sort = dt.Rows[0]["Sort"].ToString();
                idm.Money = dt.Rows[0]["Money"].ToString();
                idm.IsGive = dt.Rows[0]["IsGive"].ToString();
                idm.IsReport = dt.Rows[0]["IsReport"].ToString();
                idm.IsShow = dt.Rows[0]["IsShow"].ToString();
                idm.Queue = dt.Rows[0]["Queue"].ToString();
                idm.Remark = dt.Rows[0]["Remark"].ToString();
                idm.CreateID = dt.Rows[0]["CreateID"].ToString();
                idm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                idm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                idm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return idm;
        }

        public static DataTable ItemDetailTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_ItemDetail WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static DataTable ItemDetailGetMoney(string itemDetailId)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(itemDetailId))
            {
                itemDetailId = "0";
            }
            string cmdText = "SELECT isnull(sum(money),0) Money FROM T_Pro_ItemDetail WHERE ItemDetailID in ({0}) and Status=1";
            cmdText = string.Format(cmdText, itemDetailId);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            return dt;
        }

        public static decimal ItemDetailGetMoney(string itemDetailId, string itemId)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(itemDetailId))
            {
                itemDetailId = "0";
            }
            string cmdText = "SELECT isnull(sum(money),0) Money FROM T_Pro_ItemDetail WHERE ItemDetailID in ({0}) and itemId=@itemId";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter ("@ItemID",itemId)
            };
            cmdText = string.Format(cmdText, itemDetailId);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return Convert.ToDecimal(dt.Rows[0]["Money"].ToString());
        }

        public static string GetItemDetailName(string itemDetailId)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(itemDetailId))
            {
                itemDetailId = "0";
            }
            string cmdText = @"SELECT  dl.Name
FROM    T_Pro_ItemDetail AS id
        LEFT JOIN  T_Pro_Detail dl ON dl.DetailID = id.DetailID
        where ItemDetailID=@ItemDetailID and id.Status=1";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ItemDetailID",itemDetailId)
            };

            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return "" + dt.Rows[0]["Name"] + "";
            }
            else
            {
                return "";
            }
        }

        public static DataTable GetItemDetailTable(string itemId, string itemDetailId)
        {
            string where = " and ItemID=@ItemID and ItemDetailID IN(" + itemDetailId + ")";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ItemID",itemId)
            };
            return ItemDetailBLL.ItemDetailTableByWhere(where, paras, "");
        }

        public static DataTable ItemDetailGetMoneyAll(string itemDetailId)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(itemDetailId))
            {
                itemDetailId = "0";
            }
            string cmdText = "SELECT * FROM T_Pro_ItemDetail WHERE ItemDetailID={0}";
            cmdText = string.Format(cmdText, itemDetailId);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获取收费项金额
        /// </summary>
        /// <param name="itemDetialId"></param>
        /// <returns></returns>
        public static decimal GetItemDetialMoney(string itemDetialId)
        {
            string cmdText = @"SELECT  Money
FROM     T_Pro_ItemDetail where ItemDetailID=@ItemDetailID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ItemDetailID",itemDetialId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return Convert.ToDecimal(dt.Rows[0]["Money"].ToString());
        }

        /// <summary>
        /// 验证收费明细ID是否存在
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="sort"></param>
        /// <param name="itemDetailId"></param>
        /// <returns></returns>
        public static bool ValideteItemDetailID(string deptId, string sort, string itemDetailId)
        {
            string cmdText = @"SELECT  itemdetail.ItemDetailID
FROM    T_Pro_ItemDetail AS itemdetail
        LEFT JOIN T_Pro_Item AS item ON itemdetail.ItemID = item.ItemID
        LEFT JOIN T_Pro_Detail AS detail ON itemdetail.DetailID = detail.DetailID
WHERE   item.Status = 1
        AND itemdetail.Status = 1
        AND detail.Status = 1
        AND item.DeptID = @DeptID
        AND item.Sort = @Sort
        AND itemdetail.ItemDetailID = @ItemDetailID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptID", deptId),
                new SqlParameter("@Sort", sort),
                new SqlParameter("@ItemDetailID", itemDetailId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DataTable GetItemDetailDataTable(string itemId)
        {
            string cmdText = @"SELECT  id.ItemDetailID Id ,
        id.ItemID Pid ,
        id.DetailID Text,
        id.Sort ,
        id.IsGive ,
        id.IsShow ,
        id.IsReport ,
        id.Queue ,
        id.Money,
        id.Remark
FROM    T_Pro_ItemDetail id
       Where id.Status=1 and id.ItemID IN(" + itemId + ")  Order by id.ItemID ASC";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemID",itemId)
            };
            DataSet ds = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras);
            return ds.Tables[0];
        }
    }
}
