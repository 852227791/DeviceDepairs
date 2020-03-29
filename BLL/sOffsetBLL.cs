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
    public class sOffsetBLL
    {
        public static int InsertsOffset(sOffsetModel om)
        {
            string cmdText = @"INSERT INTO T_Stu_sOffset
(Status
,BySort
,RelatedID
,ByRelatedID
,Money
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@BySort
,@RelatedID
,@ByRelatedID
,@Money
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", om.Status),
new SqlParameter("@BySort", om.BySort),
new SqlParameter("@RelatedID", om.RelatedID),
new SqlParameter("@ByRelatedID", om.ByRelatedID),
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

        public static int UpdatesOffset(sOffsetModel om)
        {
            string cmdText = @"UPDATE T_Stu_sOffset SET
Status=@Status
,BySort=@BySort
,RelatedID=@RelatedID
,ByRelatedID=@ByRelatedID
,Money=@Money
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sOffsetID=@sOffsetID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sOffsetID", om.sOffsetID),
new SqlParameter("@Status", om.Status),
new SqlParameter("@BySort", om.BySort),
new SqlParameter("@RelatedID", om.RelatedID),
new SqlParameter("@ByRelatedID", om.ByRelatedID),
new SqlParameter("@Money", om.Money),
new SqlParameter("@CreateID", om.CreateID),
new SqlParameter("@CreateTime", om.CreateTime),
new SqlParameter("@UpdateID", om.UpdateID),
new SqlParameter("@UpdateTime", om.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(om.sOffsetID);
            }
            else
            {
                return -1;
            }
        }

        public static sOffsetModel sOffsetModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sOffsetModel om = new sOffsetModel();
            string cmdText = "SELECT * FROM T_Stu_sOffset WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                om.sOffsetID = dt.Rows[0]["sOffsetID"].ToString();
                om.Status = dt.Rows[0]["Status"].ToString();
                om.BySort = dt.Rows[0]["BySort"].ToString();
                om.RelatedID = dt.Rows[0]["RelatedID"].ToString();
                om.ByRelatedID = dt.Rows[0]["ByRelatedID"].ToString();
                om.Money = dt.Rows[0]["Money"].ToString();
                om.CreateID = dt.Rows[0]["CreateID"].ToString();
                om.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                om.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                om.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return om;
        }

        public static DataTable sOffsetTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sOffset WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 修改充抵信息状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="sfeesOrderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int UpdatasOffsetStatus(string status, string sfeesOrderId, int userId)
        {
            string cmdText = @"UPDATE T_Stu_sOffset SET
                Status=@Status
                ,UpdateID=@UpdateID
                ,UpdateTime=@UpdateTime
                WHERE RelatedID=@RelatedID";
                            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Status", status),
                new SqlParameter("@RelatedID", sfeesOrderId),
                new SqlParameter("@UpdateTime", DateTime.Now.ToString()),
                new SqlParameter("@UpdateID", userId)
                };
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        public static int GetsOffsetCount(string sfeesOrderId)
        {
            string where = " and (RelatedID=@RelatedID and Status=1) or (ByRelatedID=@ByRelatedID and BySort=3 and Status=1)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ByRelatedID",sfeesOrderId),
                new SqlParameter("@RelatedID",sfeesOrderId)
            };
            return sOffsetBLL.sOffsetTableByWhere(where, paras, "").Rows.Count;
        }
        public static int GetsByOffsetCount(string sfeesOrderId)
        {
            string where = " and  ByRelatedID=@ByRelatedID and BySort=3 and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ByRelatedID",sfeesOrderId),
                new SqlParameter("@RelatedID",sfeesOrderId)
            };
            return sOffsetBLL.sOffsetTableByWhere(where, paras, "").Rows.Count;
        }
        /// <summary>
        /// 获取收费明细的充抵总金额
        /// </summary>
        /// <param name="sfeesOrderId"></param>
        /// <returns></returns>
        public static decimal GetsOffsetMoney(string sfeesOrderId)
        {
            string cmdText = " SELECT ISNULL(SUM(Money),0) Money FROM T_Stu_sOffset WHERE RelatedID=@RelatedID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ByRelatedID",sfeesOrderId),
                new SqlParameter("@RelatedID",sfeesOrderId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return Convert.ToDecimal(dt.Rows[0]["Money"].ToString());
        }

        /// <summary>
        /// 获取收费明细的充抵总金额
        /// </summary>
        /// <param name="sfeesOrderId"></param>
        /// <returns></returns>
        public static decimal GetBysOffsetMoney(string sfeesOrderId)
        {
            string cmdText = " SELECT ISNULL(SUM(Money),0) Money FROM T_Stu_sOffset WHERE ByRelatedID=@ByRelatedID and Status=1 and BySort=3";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ByRelatedID",sfeesOrderId),
                new SqlParameter("@RelatedID",sfeesOrderId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return Convert.ToDecimal(dt.Rows[0]["Money"].ToString());
        }
    }
}
