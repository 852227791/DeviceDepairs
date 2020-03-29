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
    public class UserRoleBLL
    {
        public static int InsertUserRole(UserRoleModel urm)
        {
            string cmdText = @"INSERT INTO T_Sys_UserRole
(UserID
,RoleID
)
VALUES (@UserID
,@RoleID
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@UserID", urm.UserID),
new SqlParameter("@RoleID", urm.RoleID)
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

        public static int UpdateUserRole(UserRoleModel urm)
        {
            string cmdText = @"UPDATE T_Sys_UserRole SET
UserID=@UserID
,RoleID=@RoleID
WHERE UserRoleID=@UserRoleID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@UserRoleID", urm.UserRoleID),
new SqlParameter("@UserID", urm.UserID),
new SqlParameter("@RoleID", urm.RoleID)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(urm.UserRoleID);
            }
            else
            {
                return -1;
            }
        }

        public static UserRoleModel UserRoleModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            UserRoleModel urm = new UserRoleModel();
            string cmdText = "SELECT * FROM T_Sys_UserRole WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                urm.UserRoleID = dt.Rows[0]["UserRoleID"].ToString();
                urm.UserID = dt.Rows[0]["UserID"].ToString();
                urm.RoleID = dt.Rows[0]["RoleID"].ToString();
            }
            return urm;
        }

        public static DataTable UserRoleTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Sys_UserRole WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static string DeleteUserRole(string UserRoleID)
        {
            string cmdText = @"DELETE  FROM T_Sys_UserRole
WHERE   UserRoleID = @UserRoleID";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@UserRoleID", UserRoleID)
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
