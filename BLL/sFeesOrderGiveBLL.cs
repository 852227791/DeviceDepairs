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
    public class sFeesOrderGiveBLL
    {
        public static int InsertsFeesOrderGive(sFeesOrderGiveModel fogm)
        {
            string cmdText = @"INSERT INTO T_Stu_sFeesOrderGive
(Status
,sFeeID
,sOrderGiveID
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@sFeeID
,@sOrderGiveID
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", fogm.Status),
new SqlParameter("@sFeeID", fogm.sFeeID),
new SqlParameter("@sOrderGiveID", fogm.sOrderGiveID),
new SqlParameter("@CreateID", fogm.CreateID),
new SqlParameter("@CreateTime", fogm.CreateTime),
new SqlParameter("@UpdateID", fogm.UpdateID),
new SqlParameter("@UpdateTime", fogm.UpdateTime)
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

        public static int UpdatesFeesOrderGive(sFeesOrderGiveModel fogm)
        {
            string cmdText = @"UPDATE T_Stu_sFeesOrderGive SET
Status=@Status
,sFeeID=@sFeeID
,sOrderGiveID=@sOrderGiveID
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sFeesOrderGiveID=@sFeesOrderGiveID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sFeesOrderGiveID", fogm.sFeesOrderGiveID),
new SqlParameter("@Status", fogm.Status),
new SqlParameter("@sFeeID", fogm.sFeeID),
new SqlParameter("@sOrderGiveID", fogm.sOrderGiveID),
new SqlParameter("@CreateID", fogm.CreateID),
new SqlParameter("@CreateTime", fogm.CreateTime),
new SqlParameter("@UpdateID", fogm.UpdateID),
new SqlParameter("@UpdateTime", fogm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(fogm.sFeesOrderGiveID);
            }
            else
            {
                return -1;
            }
        }

        public static sFeesOrderGiveModel sFeesOrderGiveModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sFeesOrderGiveModel fogm = new sFeesOrderGiveModel();
            string cmdText = "SELECT * FROM T_Stu_sFeesOrderGive WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                fogm.sFeesOrderGiveID = dt.Rows[0]["sFeesOrderGiveID"].ToString();
                fogm.Status = dt.Rows[0]["Status"].ToString();
                fogm.sFeeID = dt.Rows[0]["sFeeID"].ToString();
                fogm.sOrderGiveID = dt.Rows[0]["sOrderGiveID"].ToString();
                fogm.CreateID = dt.Rows[0]["CreateID"].ToString();
                fogm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                fogm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                fogm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return fogm;
        }

        public static DataTable sFeesOrderGiveTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sFeesOrderGive WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        /// <summary>
        /// 获取收费配品信息
        /// </summary>
        /// <param name="sfeeId"></param>
        /// <returns></returns>
        public static DataTable GetFeeOrderGive(string sfeeId)
        {
            if (string.IsNullOrEmpty(sfeeId))
                sfeeId = "0";
            string where = " and  sFeeID=@sFeeID and Status=1 ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeeID",sfeeId)
            };
            return sFeesOrderGiveBLL.sFeesOrderGiveTableByWhere(where, paras, "");
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sfeeId">收费ID</param>
        /// <param name="userId">修改人</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public static int UpdatesFeesOrderGiveStatus(string sfeeId, int userId, string status)
        {
            string cmdText = @"UPDATE T_Stu_sFeesOrderGive SET
Status=@Status
,UpdateID=@UpdateID
,UpdateTime=getDate()
WHERE sFeeID=@sFeeID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", status),
new SqlParameter("@sFeeID", sfeeId),
new SqlParameter("@UpdateID", userId)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
                return result;
            else
                return -1;
        }
        /// <summary>
        /// 根据sfeeId返回收费配品
        /// </summary>
        /// <param name="sfeeId"></param>
        /// <returns></returns>
        public static DataTable sFeesOrderGiveTableByWhere(string sfeeId)
        {
            string where = " and sFeeID=@sFeeID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeeID",sfeeId)
            };
            return sFeesOrderGiveBLL.sFeesOrderGiveTableByWhere(where, paras, "");
        }
    }
}
