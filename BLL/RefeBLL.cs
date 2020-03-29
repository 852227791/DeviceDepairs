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
    public class RefeBLL
    {
        public static int InsertRefe(RefeModel rm)
        {
            string cmdText = @"INSERT INTO T_Sys_Refe
(Status
,RefeTypeID
,RefeName
,Value
,Queue
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@RefeTypeID
,@RefeName
,@Value
,@Queue
,@Remark
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", rm.Status),
new SqlParameter("@RefeTypeID", rm.RefeTypeID),
new SqlParameter("@RefeName", rm.RefeName),
new SqlParameter("@Value", rm.Value),
new SqlParameter("@Queue", rm.Queue),
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

        public static int UpdateRefe(RefeModel rm)
        {
            string cmdText = @"UPDATE T_Sys_Refe SET
Status=@Status
,RefeTypeID=@RefeTypeID
,RefeName=@RefeName
,Value=@Value
,Queue=@Queue
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE RefeID=@RefeID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@RefeID", rm.RefeID),
new SqlParameter("@Status", rm.Status),
new SqlParameter("@RefeTypeID", rm.RefeTypeID),
new SqlParameter("@RefeName", rm.RefeName),
new SqlParameter("@Value", rm.Value),
new SqlParameter("@Queue", rm.Queue),
new SqlParameter("@Remark", rm.Remark),
new SqlParameter("@CreateID", rm.CreateID),
new SqlParameter("@CreateTime", rm.CreateTime),
new SqlParameter("@UpdateID", rm.UpdateID),
new SqlParameter("@UpdateTime", rm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(rm.RefeID);
            }
            else
            {
                return -1;
            }
        }

        public static RefeModel RefeModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            RefeModel rm = new RefeModel();
            string cmdText = "SELECT * FROM T_Sys_Refe WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                rm.RefeID = dt.Rows[0]["RefeID"].ToString();
                rm.Status = dt.Rows[0]["Status"].ToString();
                rm.RefeTypeID = dt.Rows[0]["RefeTypeID"].ToString();
                rm.RefeName = dt.Rows[0]["RefeName"].ToString();
                rm.Value = dt.Rows[0]["Value"].ToString();
                rm.Queue = dt.Rows[0]["Queue"].ToString();
                rm.Remark = dt.Rows[0]["Remark"].ToString();
                rm.CreateID = dt.Rows[0]["CreateID"].ToString();
                rm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                rm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                rm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return rm;
        }

        public static DataTable RefeTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Sys_Refe WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        #region 获取字码表Value
        /// <summary>
        /// 获取字码表Value
        /// </summary>
        /// <param name="Name">名称</param>
        /// <param name="RefeTypeID">类型Id</param>
        /// <returns></returns>
        public static string GetRefeValue(string Name, string RefeTypeID)
        {
            string where = " and RefeName=@RefeName and RefeTypeID=@RefeTypeID";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@RefeName",Name),
            new SqlParameter("@RefeTypeID",RefeTypeID)
            };
            DataTable dt = RefeBLL.RefeTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Value"].ToString();
            }
            else
            {
                return "-1";
            }
        }
        #endregion
        #region 获取字码表名称
        public static string GetRefeName(string value, string refeTypeId)
        {
            string where = " and Value=@Value and RefeTypeID=@RefeTypeID";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@Value",value),
            new SqlParameter("@RefeTypeID",refeTypeId)
            };
            DataTable dt = RefeBLL.RefeTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["RefeName"].ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion

    }
}
