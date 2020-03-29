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
    public class sEnrollNumBLL
    {
        public static int InsertsEnrollNum(sEnrollNumModel enm)
        {
            string cmdText = @"INSERT INTO T_Stu_sEnrollNum
(Status
,DeptID
,Year
,Num
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@DeptID
,@Year
,@Num
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", enm.Status),
new SqlParameter("@DeptID", enm.DeptID),
new SqlParameter("@Year", enm.Year),
new SqlParameter("@Num", enm.Num),
new SqlParameter("@CreateID", enm.CreateID),
new SqlParameter("@CreateTime", enm.CreateTime),
new SqlParameter("@UpdateID", enm.UpdateID),
new SqlParameter("@UpdateTime", enm.UpdateTime)
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

        public static int UpdatesEnrollNum(sEnrollNumModel enm)
        {
            string cmdText = @"UPDATE T_Stu_sEnrollNum SET
Status=@Status
,DeptID=@DeptID
,Year=@Year
,Num=@Num
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sEnrollNumID=@sEnrollNumID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@sEnrollNumID", enm.sEnrollNumID),
new SqlParameter("@Status", enm.Status),
new SqlParameter("@DeptID", enm.DeptID),
new SqlParameter("@Year", enm.Year),
new SqlParameter("@Num", enm.Num),
new SqlParameter("@CreateID", enm.CreateID),
new SqlParameter("@CreateTime", enm.CreateTime),
new SqlParameter("@UpdateID", enm.UpdateID),
new SqlParameter("@UpdateTime", enm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(enm.sEnrollNumID);
            }
            else
            {
                return -1;
            }
        }

        public static sEnrollNumModel sEnrollNumModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sEnrollNumModel enm = new sEnrollNumModel();
            string cmdText = "SELECT * FROM T_Stu_sEnrollNum WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                enm.sEnrollNumID = dt.Rows[0]["sEnrollNumID"].ToString();
                enm.Status = dt.Rows[0]["Status"].ToString();
                enm.DeptID = dt.Rows[0]["DeptID"].ToString();
                enm.Year = dt.Rows[0]["Year"].ToString();
                enm.Num = dt.Rows[0]["Num"].ToString();
                enm.CreateID = dt.Rows[0]["CreateID"].ToString();
                enm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                enm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                enm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return enm;
        }

        public static DataTable sEnrollNumTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sEnrollNum WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        
        public static string UpdateEnrollNumToNum(string sEnrollNumID)
        {
            string cmdText = @"UPDATE  T_Stu_sEnrollNum
SET     Num = Num + 1
OUTPUT  Inserted.Num
WHERE   sEnrollNumID = @sEnrollNumID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollNumID", sEnrollNumID)
};
            string result = DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras).ToString();
            return result;
        }
    }
}
