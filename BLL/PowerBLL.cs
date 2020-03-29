using Common;
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
    public class PowerBLL
    {
        public static int InsertPower(PowerModel pm)
        {
            string cmdText = @"INSERT INTO T_Sys_Power
(RoleID
,MenuID
,ButtonID
)
VALUES (@RoleID
,@MenuID
,@ButtonID
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@RoleID", pm.RoleID),
new SqlParameter("@MenuID", pm.MenuID),
new SqlParameter("@ButtonID", pm.ButtonID)
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

        public static int UpdatePower(PowerModel pm)
        {
            string cmdText = @"UPDATE T_Sys_Power SET
RoleID=@RoleID
,MenuID=@MenuID
,ButtonID=@ButtonID
WHERE PowerID=@PowerID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@PowerID", pm.PowerID),
new SqlParameter("@RoleID", pm.RoleID),
new SqlParameter("@MenuID", pm.MenuID),
new SqlParameter("@ButtonID", pm.ButtonID)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(pm.PowerID);
            }
            else
            {
                return -1;
            }
        }

        public static PowerModel PowerModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            PowerModel pm = new PowerModel();
            string cmdText = "SELECT * FROM T_Sys_Power WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                pm.PowerID = dt.Rows[0]["PowerID"].ToString();
                pm.RoleID = dt.Rows[0]["RoleID"].ToString();
                pm.MenuID = dt.Rows[0]["MenuID"].ToString();
                pm.ButtonID = dt.Rows[0]["ButtonID"].ToString();
            }
            return pm;
        }

        public static DataTable PowerTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Sys_Power WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="PowerData"></param>
        /// <returns></returns>
        public static string SavePower(string RoleID, string PowerData)
        {
            string cmdText = @"DELETE  FROM T_Sys_Power
WHERE   RoleID = @RoleID";
            SqlParameter[] paras1 = new SqlParameter[]{
                new SqlParameter("@RoleID", RoleID)
            };
            int result1 = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras1);

            if (!string.IsNullOrEmpty(PowerData))
            {
                var list = JsonHelper.FromJson<List<Dictionary<string, string>>>(PowerData);
                foreach (var items in list)
                {
                    string sql = @"INSERT  INTO T_Sys_Power
        ( RoleID, MenuID, ButtonID )
VALUES  ( @RoleID, @MenuID, @ButtonID )";
                    SqlParameter[] paras2 = new SqlParameter[]{
                        new SqlParameter("@RoleID", RoleID),
                        new SqlParameter("@MenuID", items["menuid"]),
                        new SqlParameter("@ButtonID", items["buttonid"])
                    };
                    int result2 = DatabaseFactory.ExecuteNonQuery(sql, CommandType.Text, paras2);
                }
            }
            return "yes";
        }

        public static string DelPower(string RoleID)
        {
            string cmdText = @"DELETE  FROM T_Sys_Power
WHERE   RoleID = @RoleID";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@RoleID", RoleID)
            };
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);

            return "yes";
        }

        public static string ValidatePower(string MenuID, string Num, string ButtonCode, string UserID)
        {
            string cmdText = @"SELECT  COUNT(PowerID)
FROM    T_Sys_Power
WHERE   MenuID = @MenuID
        AND ButtonID IN ( SELECT    ButtonID
                          FROM      T_Sys_Button
                          WHERE     Status = 1
                                    AND MenuID = @MenuID
                                    AND Num = @Num
                                    AND Code like @Code )
        AND RoleID IN ( SELECT  RoleID
                        FROM    T_Sys_UserRole
                        WHERE   UserID = @UserID )";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@MenuID", MenuID),
                new SqlParameter("@Num", Num),
                new SqlParameter("@Code", ButtonCode),
                new SqlParameter("@UserID", UserID)
            };
            if (Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras)) <= 0)
            {
                return "没有权限";
            }
            else
            {
                return "yes";
            }
        }

        public static string ValidatePagePower(string MenuID, int UserID)
        {
            string cmdText = @"SELECT  COUNT(PowerID)
FROM    T_Sys_Power
WHERE   MenuID = @MenuID
        AND RoleID IN ( SELECT  RoleID
                        FROM    T_Sys_UserRole
                        WHERE   UserID = @UserID )";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@MenuID", MenuID),
                new SqlParameter("@UserID", UserID)
            };
            if (Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras)) <= 0)
            {
                return "没有权限";
            }
            else
            {
                return "yes";
            }
        }
    }
}
