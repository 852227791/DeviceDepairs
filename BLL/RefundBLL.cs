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
    public class RefundBLL
    {
        public static int InsertRefund(RefundModel rm)
        {
            string cmdText = @"INSERT INTO T_Pro_Refund
(Status
,FeeDetailID
,Sort
,RefundMoney
,RefundTime
,PayObject
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@FeeDetailID
,@Sort
,@RefundMoney
,@RefundTime
,@PayObject
,@Remark
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", rm.Status),
new SqlParameter("@FeeDetailID", rm.FeeDetailID),
new SqlParameter("@Sort", rm.Sort),
new SqlParameter("@RefundMoney", rm.RefundMoney),
new SqlParameter("@RefundTime", rm.RefundTime),
new SqlParameter("@PayObject", rm.PayObject),
new SqlParameter("@Remark", rm.Remark),
new SqlParameter("@CreateID", rm.CreateID),
new SqlParameter("@CreateTime", rm.CreateTime),
new SqlParameter("@UpdateID", rm.UpdateID),
new SqlParameter("@UpdateTime", rm.UpdateTime)
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

        public static int UpdateRefund(RefundModel rm)
        {
            string cmdText = @"UPDATE T_Pro_Refund SET
Status=@Status
,FeeDetailID=@FeeDetailID
,Sort=@Sort
,RefundMoney=@RefundMoney
,RefundTime=@RefundTime
,PayObject=@PayObject
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE RefundID=@RefundID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@RefundID", rm.RefundID),
new SqlParameter("@Status", rm.Status),
new SqlParameter("@FeeDetailID", rm.FeeDetailID),
new SqlParameter("@Sort", rm.Sort),
new SqlParameter("@RefundMoney", rm.RefundMoney),
new SqlParameter("@RefundTime", rm.RefundTime),
new SqlParameter("@PayObject", rm.PayObject),
new SqlParameter("@Remark", rm.Remark),
new SqlParameter("@CreateID", rm.CreateID),
new SqlParameter("@CreateTime", rm.CreateTime),
new SqlParameter("@UpdateID", rm.UpdateID),
new SqlParameter("@UpdateTime", rm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(rm.RefundID);
            }
            else
            {
                return -1;
            }
        }

        public static RefundModel RefundModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            RefundModel rm = new RefundModel();
            string cmdText = "SELECT * FROM T_Pro_Refund WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                rm.RefundID = dt.Rows[0]["RefundID"].ToString();
                rm.Status = dt.Rows[0]["Status"].ToString();
                rm.FeeDetailID = dt.Rows[0]["FeeDetailID"].ToString();
                rm.Sort = dt.Rows[0]["Sort"].ToString();
                rm.RefundMoney = dt.Rows[0]["RefundMoney"].ToString();
                rm.RefundTime = dt.Rows[0]["RefundTime"].ToString();
                rm.PayObject = dt.Rows[0]["PayObject"].ToString();
                rm.Remark = dt.Rows[0]["Remark"].ToString();
                rm.CreateID = dt.Rows[0]["CreateID"].ToString();
                rm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                rm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                rm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return rm;
        }

        public static DataTable RefundTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Refund WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static DataTable SelectRefund(string refundId)
        {
            string cmdText = @"SELECT  f.FeeID ,
        r.RefundID ,
        s.Name + '_' + f.VoucherNum + '_' + d.Name FeeName ,
        r.Sort ,
        r.FeeDetailID ,
        r.RefundMoney ,
        CONVERT(NVARCHAR(10), r.RefundTime, 23) RefundTime ,
        r.PayObject ,
        r.Remark
FROM    T_Pro_Refund r
        LEFT JOIN  T_Pro_FeeDetail fd ON fd.FeeDetailID = r.FeeDetailID
        LEFT JOIN T_Pro_Fee f ON f.FeeID = fd.FeeID
        LEFT JOIN T_Pro_Prove p ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_ItemDetail id ON fd.ItemDetailID = id.ItemDetailID
        LEFT JOIN T_Pro_Detail d ON id.DetailID = d.DetailID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
WHERE   r.RefundID = @RefundID";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@RefundID",refundId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
