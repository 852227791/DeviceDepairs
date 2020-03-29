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
    public class RoleBLL
    {
        public static int InsertRole(RoleModel rm)
        {
            string cmdText = @"INSERT INTO T_Sys_Role
(Status
,RoleType
,Name
,Description
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@RoleType
,@Name
,@Description
,@Remark
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", rm.Status),
new SqlParameter("@RoleType", rm.RoleType),
new SqlParameter("@Name", rm.Name),
new SqlParameter("@Description", rm.Description),
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

        public static int UpdateRole(RoleModel rm)
        {
            string cmdText = @"UPDATE T_Sys_Role SET
Status=@Status
,RoleType=@RoleType
,Name=@Name
,Description=@Description
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE RoleID=@RoleID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@RoleID", rm.RoleID),
new SqlParameter("@Status", rm.Status),
new SqlParameter("@RoleType", rm.RoleType),
new SqlParameter("@Name", rm.Name),
new SqlParameter("@Description", rm.Description),
new SqlParameter("@Remark", rm.Remark),
new SqlParameter("@CreateID", rm.CreateID),
new SqlParameter("@CreateTime", rm.CreateTime),
new SqlParameter("@UpdateID", rm.UpdateID),
new SqlParameter("@UpdateTime", rm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(rm.RoleID);
            }
            else
            {
                return -1;
            }
        }

        public static RoleModel RoleModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            RoleModel rm = new RoleModel();
            string cmdText = "SELECT * FROM T_Sys_Role WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                rm.RoleID = dt.Rows[0]["RoleID"].ToString();
                rm.Status = dt.Rows[0]["Status"].ToString();
                rm.RoleType = dt.Rows[0]["RoleType"].ToString();
                rm.Name = dt.Rows[0]["Name"].ToString();
                rm.Description = dt.Rows[0]["Description"].ToString();
                rm.Remark = dt.Rows[0]["Remark"].ToString();
                rm.CreateID = dt.Rows[0]["CreateID"].ToString();
                rm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                rm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                rm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return rm;
        }

        public static DataTable RoleTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Sys_Role WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
