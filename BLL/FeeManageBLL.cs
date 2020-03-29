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
 public   class FeeManageBLL
    {
        public static int InsertFeeManage(FeeManageModel fmm)
        {
            string cmdText = @"INSERT INTO T_Pro_FeeManage
(Status
,IsForce
,FeeID
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@IsForce
,@FeeID
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", fmm.Status),
new SqlParameter("@IsForce", fmm.IsForce),
new SqlParameter("@FeeID", fmm.FeeID),
new SqlParameter("@CreateID", fmm.CreateID),
new SqlParameter("@CreateTime", fmm.CreateTime),
new SqlParameter("@UpdateID", fmm.UpdateID),
new SqlParameter("@UpdateTime", fmm.UpdateTime)
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

        public static int UpdateFeeManage(FeeManageModel fmm)
        {
            string cmdText = @"UPDATE T_Pro_FeeManage SET
Status=@Status
,IsForce=@IsForce
,FeeID=@FeeID
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE FeeManageID=@FeeManageID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@FeeManageID", fmm.FeeManageID),
new SqlParameter("@Status", fmm.Status),
new SqlParameter("@IsForce", fmm.IsForce),
new SqlParameter("@FeeID", fmm.FeeID),
new SqlParameter("@CreateID", fmm.CreateID),
new SqlParameter("@CreateTime", fmm.CreateTime),
new SqlParameter("@UpdateID", fmm.UpdateID),
new SqlParameter("@UpdateTime", fmm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(fmm.FeeManageID);
            }
            else
            {
                return -1;
            }
        }

        public static FeeManageModel FeeManageModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            FeeManageModel fmm = new FeeManageModel();
            string cmdText = "SELECT * FROM T_Pro_FeeManage WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                fmm.FeeManageID = dt.Rows[0]["FeeManageID"].ToString();
                fmm.Status = dt.Rows[0]["Status"].ToString();
                fmm.IsForce = dt.Rows[0]["IsForce"].ToString();
                fmm.FeeID = dt.Rows[0]["FeeID"].ToString();
                fmm.CreateID = dt.Rows[0]["CreateID"].ToString();
                fmm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                fmm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                fmm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return fmm;
        }

        public static DataTable FeeManageTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_FeeManage WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
