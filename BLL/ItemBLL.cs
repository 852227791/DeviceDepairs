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
    public class ItemBLL
    {
        public static int InsertItem(ItemModel im)
        {
            string cmdText = @"INSERT INTO T_Pro_Item
			(Status
			,DeptID
			,ParentID
			,Sort
			,Year
			,Month
			,Name
			,EnglishName
			,IsPlan
			,PlanLevel
			,IsReport
			,IsShow
			,StartTime
			,EndTime
			,LimitTime
			,Queue
			,Remark
			,CreateID
			,CreateTime
			,UpdateID
			,UpdateTime
						)
			VALUES(
            @Status
			,@DeptID
			,@ParentID
			,@Sort
			,@Year
			,@Month
			,@Name
			,@EnglishName
			,@IsPlan
			,@PlanLevel
			,@IsReport
			,@IsShow
			,@StartTime
			,@EndTime
			,@LimitTime
			,@Queue
			,@Remark
			,@CreateID
			,@CreateTime
			,@UpdateID
			,@UpdateTime
						);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
                 new SqlParameter("@Status", im.Status)
                ,new SqlParameter("@DeptID", im.DeptID)
                ,new SqlParameter("@ParentID", im.ParentID)
                ,new SqlParameter("@Sort", im.Sort)
                ,new SqlParameter("@Year", im.Year)
                ,new SqlParameter("@Month", im.Month)
                ,new SqlParameter("@Name", im.Name)
                ,new SqlParameter("@EnglishName", im.EnglishName)
                ,new SqlParameter("@IsPlan", im.IsPlan)
                ,new SqlParameter("@PlanLevel", im.PlanLevel)
                ,new SqlParameter("@IsReport", im.IsReport)
                ,new SqlParameter("@IsShow", im.IsShow)
                ,new SqlParameter("@StartTime", im.StartTime)
                ,new SqlParameter("@EndTime", im.EndTime)
                ,new SqlParameter("@LimitTime", im.LimitTime)
                ,new SqlParameter("@Queue", im.Queue)
                ,new SqlParameter("@Remark", im.Remark)
                ,new SqlParameter("@CreateID", im.CreateID)
                ,new SqlParameter("@CreateTime", im.CreateTime)
                ,new SqlParameter("@UpdateID", im.UpdateID)
                ,new SqlParameter("@UpdateTime", im.UpdateTime)
                            };
            return Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
        }

        public static int UpdateItem(ItemModel im)
        {
            string cmdText = @"UPDATE T_Pro_Item SET 
			Status=@Status
			,DeptID=@DeptID
			,ParentID=@ParentID
			,Sort=@Sort
			,Year=@Year
			,Month=@Month
			,Name=@Name
			,EnglishName=@EnglishName
			,IsPlan=@IsPlan
			,PlanLevel=@PlanLevel
			,IsReport=@IsReport
			,IsShow=@IsShow
			,StartTime=@StartTime
			,EndTime=@EndTime
			,LimitTime=@LimitTime
			,Queue=@Queue
			,Remark=@Remark
			,CreateID=@CreateID
			,CreateTime=@CreateTime
			,UpdateID=@UpdateID
			,UpdateTime=@UpdateTime
            Where ItemID=@ItemID
			";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemID", im.ItemID)
                ,new SqlParameter("@Status", im.Status)
                ,new SqlParameter("@DeptID", im.DeptID)
                ,new SqlParameter("@ParentID", im.ParentID)
                ,new SqlParameter("@Sort", im.Sort)
                ,new SqlParameter("@Year", im.Year)
                ,new SqlParameter("@Month", im.Month)
                ,new SqlParameter("@Name", im.Name)
                ,new SqlParameter("@EnglishName", im.EnglishName)
                ,new SqlParameter("@IsPlan", im.IsPlan)
                ,new SqlParameter("@PlanLevel", im.PlanLevel)
                ,new SqlParameter("@IsReport", im.IsReport)
                ,new SqlParameter("@IsShow", im.IsShow)
                ,new SqlParameter("@StartTime", im.StartTime)
                ,new SqlParameter("@EndTime", im.EndTime)
                ,new SqlParameter("@LimitTime", im.LimitTime)
                ,new SqlParameter("@Queue", im.Queue)
                ,new SqlParameter("@Remark", im.Remark)
                ,new SqlParameter("@CreateID", im.CreateID)
                ,new SqlParameter("@CreateTime", im.CreateTime)
                ,new SqlParameter("@UpdateID", im.UpdateID)
                ,new SqlParameter("@UpdateTime", im.UpdateTime)
                };
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        public static List<ItemModel> SelectItemByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            List<ItemModel> list = new List<ItemModel>();
            ItemModel im = new ItemModel();
            string cmdText = "SELECT * FROM T_Pro_Item WHERE 1 = 1 {0}{1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                im.PlanLevel = dr["PlanLevel"].ToString();
                im.ItemID = dr["ItemID"].ToString();
                im.Status = dr["Status"].ToString();
                im.DeptID = dr["DeptID"].ToString();
                im.ParentID = dr["ParentID"].ToString();
                im.Sort = dr["Sort"].ToString();
                im.Year = dr["Year"].ToString();
                im.Month = dr["Month"].ToString();
                im.Name = dr["Name"].ToString();
                im.EnglishName = dr["EnglishName"].ToString();
                im.IsPlan = dr["IsPlan"].ToString();
                im.PlanLevel = dr["PlanLevel"].ToString();
                im.IsReport = dr["IsReport"].ToString();
                im.IsShow = dr["IsShow"].ToString();
                im.StartTime = dr["StartTime"].ToString();
                im.EndTime = dr["EndTime"].ToString();
                im.LimitTime = dr["LimitTime"].ToString();
                im.Queue = dr["Queue"].ToString();
                im.Remark = dr["Remark"].ToString();
                im.CreateID = dr["CreateID"].ToString();
                im.CreateTime = dr["CreateTime"].ToString();
                im.UpdateID = dr["UpdateID"].ToString();
                im.UpdateTime = dr["UpdateTime"].ToString();
                list.Add(im);
            }
            return list;
        }

        public static bool UpdateItemStatus(string ItemID, string status, int userId)
        {
            string where = " and ItemID=@ItemID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemID", ItemID)
            };
            var list = ItemBLL.SelectItemByWhere(where, paras, "").FirstOrDefault();
            if (list != null)
            {
                list.Status = status;
                list.UpdateTime = DateTime.Now.ToString();
                list.UpdateID = userId.ToString();
                if (ItemBLL.UpdateItem(list).Equals(1))
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }

        public static ItemModel ItemModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            ItemModel im = new ItemModel();
            string cmdText = "SELECT * FROM T_Pro_Item WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                im.ItemID = dt.Rows[0]["ItemID"].ToString();
                im.PlanLevel = dt.Rows[0]["PlanLevel"].ToString();
                im.Status = dt.Rows[0]["Status"].ToString();
                im.DeptID = dt.Rows[0]["DeptID"].ToString();
                im.ParentID = dt.Rows[0]["ParentID"].ToString();
                im.Sort = dt.Rows[0]["Sort"].ToString();
                im.Year = dt.Rows[0]["Year"].ToString();
                im.Month = dt.Rows[0]["Month"].ToString();
                im.Name = dt.Rows[0]["Name"].ToString();
                im.EnglishName = dt.Rows[0]["EnglishName"].ToString();
                im.IsPlan = dt.Rows[0]["IsPlan"].ToString();
                im.IsReport = dt.Rows[0]["IsReport"].ToString();
                im.IsShow = dt.Rows[0]["IsShow"].ToString();
                im.StartTime = dt.Rows[0]["StartTime"].ToString();
                im.EndTime = dt.Rows[0]["EndTime"].ToString();
                im.LimitTime = dt.Rows[0]["LimitTime"].ToString();
                im.Queue = dt.Rows[0]["Queue"].ToString();
                im.Remark = dt.Rows[0]["Remark"].ToString();
                im.CreateID = dt.Rows[0]["CreateID"].ToString();
                im.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                im.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                im.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return im;
        }

        public static DataTable ItemTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Item WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }



        public static DataTable SelectChildrenItemID(string itemId)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT ItemID FROM GetChildrenItemID(" + itemId + ")";
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            return dt;
        }

        public static DataTable GetUNRepeatItem(string itemDetailId)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT DISTINCT
        *
FROM    T_Pro_Item
WHERE   ItemID IN ( SELECT  ItemID
                    FROM    T_Pro_ItemDetail
                    WHERE   ItemDetailID IN ( {0} ) ) ";
            cmdText = string.Format(cmdText, itemDetailId);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            return dt;
        }
        #region
        public static ItemModel GetItemModel(string deptid, string name)
        {
            string where = " and DeptID=@DeptID and Name=@Name";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@DeptID",deptid),
            new SqlParameter("@Name",name)
            };
            ItemModel im = ItemBLL.ItemModelByWhere(where, paras);
            return im;
        }
        #endregion

        public static ItemModel GetItemModel(string itemId)
        {
            string where = " and ItemID=@ItemID ";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ItemID",itemId)
            };
            ItemModel im = ItemBLL.ItemModelByWhere(where, paras);
            return im;
        }

        public static DataTable GetItemTable(string itemId)
        {
            string cmdText = @" Select * from T_Pro_Item  where ItemID IN (SELECT ItemID FROM GetChildrenItemID(" + itemId + "))";
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
        }

        public static string GetItemName(string itemDetailId)
        {
            string cmdText = @"SELECT  
        i.Name ItemName
FROM    T_Pro_ItemDetail AS id
        LEFT JOIN T_Pro_Item i ON i.ItemID = id.ItemID
		WHERE id.ItemDetailID=@ItemDetailID";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ItemDetailID",itemDetailId)
            };

            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return "" + dt.Rows[0]["ItemName"] + "";
            }
            else
            {
                return "";
            }
        }
        public static DataTable GetItemDeptID()
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT i.DeptID ,d.Name FROM T_Pro_Item i LEFT JOIN T_Sys_Dept d ON d.DeptID=i.DeptID WHERE i.Status=1 and d.Status=1  and i.DeptID<>77 GROUP BY i.DeptID,d.Name  ORDER BY DeptID ASC";
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, null).Tables[0];
            return dt;
        }


        public static DataTable ItemDetailTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  id.ItemDetailID ,
        dl.Name ,
        d.Name DeptName ,
        i.Name ParentName ,
        id.IsReport,
        id.DetailID
FROM    T_Pro_ItemDetail id
        LEFT JOIN T_Pro_Item i ON i.ItemID = id.ItemID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = i.DeptID
        LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        public static DataTable ItemTableByWhere2(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  i.ItemID ,
        i.Name ,
        d.Name DeptName ,
        ip.Name ParentName ,
        i.IsReport
FROM    T_Pro_Item i
        LEFT JOIN T_Sys_Dept d ON d.DeptID = i.DeptID
        LEFT JOIN T_Pro_Item ip ON ip.ItemID = i.ParentID
        WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static DataTable GetItemChildModel(string parentId)
        {
            string where = " And ParentID IN(" + parentId + ") ";
            return ItemBLL.ItemTableByWhere(where, null, " ORDER BY LimitTime DESC");
        }

        public static DataTable GetItemDataTable(string itemId)
        {
            string cmdText = @"SELECT  i.ItemID Id ,
		CASE i.IsPlan
          WHEN 1 THEN 0
          ELSE i.ParentID
        END Pid ,
        i.Name Text ,
        i.EnglishName ,
        i.Sort ,
        i.Year ,
        i.Month ,
        i.IsPlan ,
        i.PlanLevel ,
        i.StartTime ,
        i.EndTime ,
        i.LimitTime ,
        i.Queue ,
        i.IsReport ,
        i.IsShow ,
        i.Remark
FROM    T_Pro_Item i
        Where  i.ItemID IN (" + itemId+") or i.ParentID IN (" + itemId + ") ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemID",itemId)
            };
            DataSet ds = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras);
            return  ds.Tables[0];
        }
    }
}
