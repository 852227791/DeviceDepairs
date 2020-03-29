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
    public class OffsetBLL
    {
        public static int InsertOffset(OffsetModel om)
        {
            string cmdText = @"INSERT INTO T_Pro_Offset
(Status
,FeeDetailID
,ByFeeDetailID
,Money
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@FeeDetailID
,@ByFeeDetailID
,@Money
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", om.Status),
new SqlParameter("@FeeDetailID", om.FeeDetailID),
new SqlParameter("@ByFeeDetailID", om.ByFeeDetailID),
new SqlParameter("@Money", om.Money),
new SqlParameter("@CreateID", om.CreateID),
new SqlParameter("@CreateTime", om.CreateTime),
new SqlParameter("@UpdateID", om.UpdateID),
new SqlParameter("@UpdateTime", om.UpdateTime)
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

        public static int UpdateOffset(OffsetModel om)
        {
            string cmdText = @"UPDATE T_Pro_Offset SET
Status=@Status
,FeeDetailID=@FeeDetailID
,ByFeeDetailID=@ByFeeDetailID
,Money=@Money
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE OffsetID=@OffsetID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@OffsetID", om.OffsetID),
new SqlParameter("@Status", om.Status),
new SqlParameter("@FeeDetailID", om.FeeDetailID),
new SqlParameter("@ByFeeDetailID", om.ByFeeDetailID),
new SqlParameter("@Money", om.Money),
new SqlParameter("@CreateID", om.CreateID),
new SqlParameter("@CreateTime", om.CreateTime),
new SqlParameter("@UpdateID", om.UpdateID),
new SqlParameter("@UpdateTime", om.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(om.OffsetID);
            }
            else
            {
                return -1;
            }
        }

        public static OffsetModel OffsetModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            OffsetModel om = new OffsetModel();
            string cmdText = "SELECT * FROM T_Pro_Offset WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                om.OffsetID = dt.Rows[0]["OffsetID"].ToString();
                om.Status = dt.Rows[0]["Status"].ToString();
                om.FeeDetailID = dt.Rows[0]["FeeDetailID"].ToString();
                om.ByFeeDetailID = dt.Rows[0]["ByFeeDetailID"].ToString();
                om.Money = dt.Rows[0]["Money"].ToString();
                om.CreateID = dt.Rows[0]["CreateID"].ToString();
                om.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                om.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                om.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return om;
        }

        public static DataTable OffsetTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Offset WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        public static DataTable SelectOffsetByFeeId(string feeId)
        {
            string cmdText = @"SELECT  o.FeeDetailID FeeDetailID ,
        s.Name ,
        f.VoucherNum ,
        f.NoteNum ,
        d.Name Dept ,
        o.Money Offset ,
        f.FeeTime,
        i.Name ItemName,
        o.Money Offset,
		o.FeeDetailID,
		fd.ItemDetailID,
		(dl.Name +' '+  CONVERT(nvarchar(18),id.Money)) OffsetItem
FROM    T_Pro_Offset o
        LEFT JOIN T_Pro_FeeDetail fd ON o.FeeDetailID = fd.FeeDetailID
		LEFT JOIN T_Pro_Fee f ON f.FeeID=fd.FeeID
        LEFT JOIN T_Pro_Prove p ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_Item i ON i.ItemID = p.ItemID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
		LEFT JOIN T_Pro_ItemDetail  id ON fd.ItemDetailID=id.ItemDetailID
		LEFT JOIN T_Pro_Detail dl ON dl.DetailID=id.DetailID
        where o.Status=1 and  fd.FeeID=@FeeID
";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter ("@FeeID",feeId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        public static int UpdateOffsetStatus(SqlParameter[] paras)
        {
            string cmdText = @"UPDATE T_Pro_Offset SET
Status=@Status
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE FeeDetailID IN (Select FeeDetailID from T_Pro_FeeDetail where FeeID=@FeeID) and Status=1";
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }

       /// <summary>
       /// 获取被冲抵明细id，和被冲抵金额
       /// </summary>
       /// <param name="feeDetailId"></param>
       /// <returns></returns>
        public static DataTable  GetOffsetMoney(string feeDetailId)
        {
            string cmdText = @"SELECT  ISNULL(Money,0) Money,ByFeeDetailID
FROM    T_Pro_Offset
WHERE   FeeDetailID = @FeeDetailID
        AND Status = 1 ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FeeDetailID",feeDetailId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
          
        }


        public static DataTable GetSelectoffset(string feeId)
        {
            if (string.IsNullOrEmpty(feeId))
            {
                return null;
            }
            string cmdText = @"
SELECT  o.ByFeeDetailID FeeDetailID ,
        fd.ItemDetailID ,
        f.VoucherNum ,
        s.Name ,
        d.Name Dept ,
        i.Name ItemName ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        o.Money Offset ,
        dl.Name + ' ' +CONVERT(NVARCHAR(18),id.Money ) OffsetItem
FROM    T_Pro_Offset o
      
        LEFT JOIN T_Pro_FeeDetail bfd ON o.ByFeeDetailID = bfd.FeeDetailID
        LEFT JOIN T_Pro_Fee f ON f.FeeID = bfd.FeeID
        LEFT JOIN T_Pro_Prove p ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
        LEFT JOIN T_Pro_Item i ON i.ItemID = p.ItemID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
	    LEFT JOIN T_Pro_FeeDetail fd ON o.FeeDetailID = fd.FeeDetailID
        LEFT JOIN T_Pro_ItemDetail id ON id.ItemDetailID = fd.ItemDetailID
        LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
		LEFT JOIN T_Pro_Fee fee ON fee.FeeID=fd.FeeID
WHERE   fee.FeeID = @FeeID
        AND o.Status = 1";

            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter ("@FeeID",feeId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];

        }

        public static DataTable GetSelectdiscount(string feeId)
        {
            if (string.IsNullOrEmpty(feeId))
            {
                return null;
            }
            string cmdText = @"SELECT  fd.DiscountMoney ,
        fd.ItemDetailID ,
        dl.Name OffsetItem
FROM    T_Pro_FeeDetail fd
        LEFT JOIN T_Pro_ItemDetail id ON id.ItemDetailID = fd.ItemDetailID
        LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
WHERE   fd.Status = 1
        AND FeeID = @FeeID";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter ("@FeeID",feeId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
    }
}
