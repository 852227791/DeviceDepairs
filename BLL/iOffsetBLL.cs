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
    public class iOffsetBLL
    {
        public static int InsertiOffset(iOffsetModel om)
        {
            string cmdText = @"INSERT INTO T_Inc_iOffset
(Status
,iFeeID
,ByiFeeID
,Money
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@iFeeID
,@ByiFeeID
,@Money
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", om.Status),
new SqlParameter("@iFeeID", om.iFeeID),
new SqlParameter("@ByiFeeID", om.ByiFeeID),
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

        public static int UpdateiOffset(iOffsetModel om)
        {
            string cmdText = @"UPDATE T_Inc_iOffset SET
Status=@Status
,iFeeID=@iFeeID
,ByiFeeID=@ByiFeeID
,Money=@Money
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE iOffsetID=@iOffsetID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@iOffsetID", om.iOffsetID),
new SqlParameter("@Status", om.Status),
new SqlParameter("@iFeeID", om.iFeeID),
new SqlParameter("@ByiFeeID", om.ByiFeeID),
new SqlParameter("@Money", om.Money),
new SqlParameter("@CreateID", om.CreateID),
new SqlParameter("@CreateTime", om.CreateTime),
new SqlParameter("@UpdateID", om.UpdateID),
new SqlParameter("@UpdateTime", om.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(om.iOffsetID);
            }
            else
            {
                return -1;
            }
        }

        public static iOffsetModel iOffsetModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            iOffsetModel om = new iOffsetModel();
            string cmdText = "SELECT * FROM T_Inc_iOffset WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                om.iOffsetID = dt.Rows[0]["iOffsetID"].ToString();
                om.Status = dt.Rows[0]["Status"].ToString();
                om.iFeeID = dt.Rows[0]["iFeeID"].ToString();
                om.ByiFeeID = dt.Rows[0]["ByiFeeID"].ToString();
                om.Money = dt.Rows[0]["Money"].ToString();
                om.CreateID = dt.Rows[0]["CreateID"].ToString();
                om.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                om.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                om.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return om;
        }

        public static DataTable iOffsetTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Inc_iOffset WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
