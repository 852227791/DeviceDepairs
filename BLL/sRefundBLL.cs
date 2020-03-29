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
    public class sRefundBLL
    {
        public static int InsertsRefund(sRefundModel rm)
        {
            string cmdText = @"INSERT INTO T_Stu_sRefund
(Status
,sFeesOrderID
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
,@sFeesOrderID
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
new SqlParameter("@sFeesOrderID", rm.sFeesOrderID),
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

        public static int UpdatesRefund(sRefundModel rm)
        {
            string cmdText = @"UPDATE T_Stu_sRefund SET
Status=@Status
,sFeesOrderID=@sFeesOrderID
,Sort=@Sort
,RefundMoney=@RefundMoney
,RefundTime=@RefundTime
,PayObject=@PayObject
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sRefundID=@sRefundID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sRefundID", rm.sRefundID),
new SqlParameter("@Status", rm.Status),
new SqlParameter("@sFeesOrderID", rm.sFeesOrderID),
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
                return Convert.ToInt32(rm.sRefundID);
            }
            else
            {
                return -1;
            }
        }

        public static sRefundModel sRefundModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sRefundModel rm = new sRefundModel();
            string cmdText = "SELECT * FROM T_Stu_sRefund WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                rm.sRefundID = dt.Rows[0]["sRefundID"].ToString();
                rm.Status = dt.Rows[0]["Status"].ToString();
                rm.sFeesOrderID = dt.Rows[0]["sFeesOrderID"].ToString();
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

        public static DataTable sRefundTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sRefund WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获取已核销金额
        /// </summary>
        /// <param name="sfeesOrderId"></param>
        /// <param name="sreundId"></param>
        /// <returns></returns>
        public static decimal GetRefundMoney(string sfeesOrderId, string sreundId)
        {
            string cmdText = @"SELECT  ISNULL(sum(RefundMoney), 0.00) RefundMoney
FROM    T_Stu_sRefund
WHERE   sFeesOrderID = @sFeesOrderID
        AND sRefundID <> @sRefundID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sRefundID",sreundId),
                new SqlParameter("@sFeesOrderID",sfeesOrderId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return decimal.Parse(dt.Rows[0]["RefundMoney"].ToString());
        }

        public static int GetRefundCount(string sfeesOrderId)
        {
            string where = " and  sFeesOrderID=@sFeesOrderID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("sFeesOrderID",sfeesOrderId)
            };
            return sRefundBLL.sRefundTableByWhere(where, paras, "").Rows.Count;
        }
    }
}
