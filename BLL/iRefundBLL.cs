using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DAL;
using Model;

namespace BLL
{
    public class iRefundBLL
    {
        public static int InsertiRefund(iRefundModel rm)
        {
            string cmdText = @"INSERT INTO T_Inc_iRefund
(Status
,iFeeID
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
,@iFeeID
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
new SqlParameter("@iFeeID", rm.iFeeID),
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

        public static int UpdateiRefund(iRefundModel rm)
        {
            string cmdText = @"UPDATE T_Inc_iRefund SET
Status=@Status
,iFeeID=@iFeeID
,Sort=@Sort
,RefundMoney=@RefundMoney
,RefundTime=@RefundTime
,PayObject=@PayObject
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE iRefundID=@iRefundID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@iRefundID", rm.iRefundID),
new SqlParameter("@Status", rm.Status),
new SqlParameter("@iFeeID", rm.iFeeID),
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
                return Convert.ToInt32(rm.iRefundID);
            }
            else
            {
                return -1;
            }
        }

        public static iRefundModel iRefundModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            iRefundModel rm = new iRefundModel();
            string cmdText = "SELECT * FROM T_Inc_iRefund WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                rm.iRefundID = dt.Rows[0]["iRefundID"].ToString();
                rm.Status = dt.Rows[0]["Status"].ToString();
                rm.iFeeID = dt.Rows[0]["iFeeID"].ToString();
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

        public static DataTable iRefundTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Inc_iRefund WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static DataTable SelectiRefund(string iRefundId)
        {
            string cmdText = @"SELECT  irefund.iRefundID ,
        irefund.iFeeID ,
        irefund.Sort ,
        ( student.Name + '_' + ifee.VoucherNum + '_' + detail.Name ) AS FeeName ,
        irefund.RefundMoney ,
        CONVERT(NVARCHAR(23), irefund.RefundTime, 23) AS RefundTime ,
        irefund.PayObject ,
        irefund.Remark
FROM    T_Inc_iRefund AS irefund
        LEFT JOIN T_Inc_iFee AS ifee ON irefund.iFeeID = ifee.iFeeID
        LEFT JOIN T_Pro_Student AS student ON ifee.StudentID = student.StudentID
        LEFT JOIN T_Pro_ItemDetail AS itemdetail ON ifee.ItemDetailID = itemdetail.ItemDetailID
        LEFT JOIN T_Pro_Detail AS detail ON itemdetail.DetailID = detail.DetailID
WHERE   irefund.iRefundID = @iRefundID";
            SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@iRefundID", iRefundId) };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
    }
}
