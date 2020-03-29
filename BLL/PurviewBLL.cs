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
    public class PurviewBLL
    {
        public static int InsertPurview(PurviewModel pm)
        {
            string cmdText = @"INSERT INTO T_Sys_Purview
(UserID
,RoleID
,DeptID
,Range
)
VALUES (@UserID
,@RoleID
,@DeptID
,@Range
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@UserID", pm.UserID),
new SqlParameter("@RoleID", pm.RoleID),
new SqlParameter("@DeptID", pm.DeptID),
new SqlParameter("@Range", pm.Range)
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

        public static int UpdatePurview(PurviewModel pm)
        {
            string cmdText = @"UPDATE T_Sys_Purview SET
UserID=@UserID
,RoleID=@RoleID
,DeptID=@DeptID
,Range=@Range
WHERE PurviewID=@PurviewID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@PurviewID", pm.PurviewID),
new SqlParameter("@UserID", pm.UserID),
new SqlParameter("@RoleID", pm.RoleID),
new SqlParameter("@DeptID", pm.DeptID),
new SqlParameter("@Range", pm.Range)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(pm.PurviewID);
            }
            else
            {
                return -1;
            }
        }

        public static PurviewModel PurviewModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            PurviewModel pm = new PurviewModel();
            string cmdText = "SELECT * FROM T_Sys_Purview WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                pm.PurviewID = dt.Rows[0]["PurviewID"].ToString();
                pm.UserID = dt.Rows[0]["UserID"].ToString();
                pm.RoleID = dt.Rows[0]["RoleID"].ToString();
                pm.DeptID = dt.Rows[0]["DeptID"].ToString();
                pm.Range = dt.Rows[0]["Range"].ToString();
            }
            return pm;
        }

        public static DataTable PurviewTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Sys_Purview WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }


        public static string DeletePurview(string PurviewID)
        {
            string cmdText = @"DELETE  FROM T_Sys_Purview
WHERE   PurviewID = @PurviewID";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@PurviewID", PurviewID)
            };
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);

            if (result > 0)
            {
                return "yes";
            }
            else
            {
                return "发现系统错误";
            }
        }
    }
}
