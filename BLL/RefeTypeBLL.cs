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
    public class RefeTypeBLL
    {
        public static int InsertRefeType(RefeTypeModel rtm)
        {
            string cmdText = @"INSERT INTO T_Sys_RefeType
(Status
,ModuleName
,TypeName
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@ModuleName
,@TypeName
,@Remark
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", rtm.Status),
new SqlParameter("@ModuleName", rtm.ModuleName),
new SqlParameter("@TypeName", rtm.TypeName),
new SqlParameter("@Remark", rtm.Remark),
new SqlParameter("@CreateID", rtm.CreateID),
new SqlParameter("@CreateTime", rtm.CreateTime),
new SqlParameter("@UpdateID", rtm.UpdateID),
new SqlParameter("@UpdateTime", rtm.UpdateTime)
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

        public static int UpdateRefeType(RefeTypeModel rtm)
        {
            string cmdText = @"UPDATE T_Sys_RefeType SET
Status=@Status
,ModuleName=@ModuleName
,TypeName=@TypeName
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE RefeTypeID=@RefeTypeID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@RefeTypeID", rtm.RefeTypeID),
new SqlParameter("@Status", rtm.Status),
new SqlParameter("@ModuleName", rtm.ModuleName),
new SqlParameter("@TypeName", rtm.TypeName),
new SqlParameter("@Remark", rtm.Remark),
new SqlParameter("@CreateID", rtm.CreateID),
new SqlParameter("@CreateTime", rtm.CreateTime),
new SqlParameter("@UpdateID", rtm.UpdateID),
new SqlParameter("@UpdateTime", rtm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(rtm.RefeTypeID);
            }
            else
            {
                return -1;
            }
        }

        public static RefeTypeModel RefeTypeModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            RefeTypeModel rtm = new RefeTypeModel();
            string cmdText = "SELECT * FROM T_Sys_RefeType WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                rtm.RefeTypeID = dt.Rows[0]["RefeTypeID"].ToString();
                rtm.Status = dt.Rows[0]["Status"].ToString();
                rtm.ModuleName = dt.Rows[0]["ModuleName"].ToString();
                rtm.TypeName = dt.Rows[0]["TypeName"].ToString();
                rtm.Remark = dt.Rows[0]["Remark"].ToString();
                rtm.CreateID = dt.Rows[0]["CreateID"].ToString();
                rtm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                rtm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                rtm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return rtm;
        }

        public static DataTable RefeTypeTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Sys_RefeType WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
