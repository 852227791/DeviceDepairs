using DAL;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class UserBLL
    {
        public static int InsertUser(UserModel um)
        {
            string cmdText = @"INSERT INTO T_Sys_User
(Status
,LoginName
,Password
,Name
,UserType
,Sex
,Mobile
,Remark
,LoginTime
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@LoginName
,@Password
,@Name
,@UserType
,@Sex
,@Mobile
,@Remark
,@LoginTime
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", um.Status),
new SqlParameter("@LoginName", um.LoginName),
new SqlParameter("@Password", um.Password),
new SqlParameter("@Name", um.Name),
new SqlParameter("@UserType", um.UserType),
new SqlParameter("@Sex", um.Sex),
new SqlParameter("@Mobile", um.Mobile),
new SqlParameter("@Remark", um.Remark),
new SqlParameter("@LoginTime", um.LoginTime),
new SqlParameter("@CreateID", um.CreateID),
new SqlParameter("@CreateTime", um.CreateTime),
new SqlParameter("@UpdateID", um.UpdateID),
new SqlParameter("@UpdateTime", um.UpdateTime)
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

        public static int UpdateUser(UserModel um)
        {
            string cmdText = @"UPDATE T_Sys_User SET
Status=@Status
,LoginName=@LoginName
,Password=@Password
,Name=@Name
,UserType=@UserType
,Sex=@Sex
,Mobile=@Mobile
,Remark=@Remark
,LoginTime=@LoginTime
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE UserID=@UserID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@UserID", um.UserID),
new SqlParameter("@Status", um.Status),
new SqlParameter("@LoginName", um.LoginName),
new SqlParameter("@Password", um.Password),
new SqlParameter("@Name", um.Name),
new SqlParameter("@UserType", um.UserType),
new SqlParameter("@Sex", um.Sex),
new SqlParameter("@Mobile", um.Mobile),
new SqlParameter("@Remark", um.Remark),
new SqlParameter("@LoginTime", um.LoginTime),
new SqlParameter("@CreateID", um.CreateID),
new SqlParameter("@CreateTime", um.CreateTime),
new SqlParameter("@UpdateID", um.UpdateID),
new SqlParameter("@UpdateTime", um.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(um.UserID);
            }
            else
            {
                return -1;
            }
        }

        public static UserModel UserModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            UserModel um = new UserModel();
            string cmdText = "SELECT * FROM T_Sys_User WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                um.UserID = dt.Rows[0]["UserID"].ToString();
                um.Status = dt.Rows[0]["Status"].ToString();
                um.LoginName = dt.Rows[0]["LoginName"].ToString();
                um.Password = dt.Rows[0]["Password"].ToString();
                um.Name = dt.Rows[0]["Name"].ToString();
                um.UserType = dt.Rows[0]["UserType"].ToString();
                um.Sex = dt.Rows[0]["Sex"].ToString();
                um.Mobile = dt.Rows[0]["Mobile"].ToString();
                um.Remark = dt.Rows[0]["Remark"].ToString();
                um.LoginTime = dt.Rows[0]["LoginTime"].ToString();
                um.CreateID = dt.Rows[0]["CreateID"].ToString();
                um.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                um.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                um.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return um;
        }

        public static DataTable UserTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Sys_User WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
