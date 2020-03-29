using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BLL
{
    public class LogBLL
    {
        public static int InsertLog(LogModel lm)
        {
            string cmdText = @"INSERT INTO T_Sys_Log
(TableName
,FieldName
,ValueOld
,ValueNew
,CreateID
,CreateTime
)
VALUES (@TableName
,@FieldName
,@ValueOld
,@ValueNew
,@CreateID
,@CreateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@TableName", lm.TableName),
new SqlParameter("@FieldName", lm.FieldName),
new SqlParameter("@ValueOld", lm.ValueOld),
new SqlParameter("@ValueNew", lm.ValueNew),
new SqlParameter("@CreateID", lm.CreateID),
new SqlParameter("@CreateTime", lm.CreateTime)
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

        public static int UpdateLog(LogModel lm)
        {
            string cmdText = @"UPDATE T_Sys_Log SET
TableName=@TableName
,FieldName=@FieldName
,ValueOld=@ValueOld
,ValueNew=@ValueNew
,CreateID=@CreateID
,CreateTime=@CreateTime
WHERE LogID=@LogID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@LogID", lm.LogID),
new SqlParameter("@TableName", lm.TableName),
new SqlParameter("@FieldName", lm.FieldName),
new SqlParameter("@ValueOld", lm.ValueOld),
new SqlParameter("@ValueNew", lm.ValueNew),
new SqlParameter("@CreateID", lm.CreateID),
new SqlParameter("@CreateTime", lm.CreateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(lm.LogID);
            }
            else
            {
                return -1;
            }
        }

        public static LogModel LogModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            LogModel lm = new LogModel();
            string cmdText = "SELECT * FROM T_Sys_Log WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                lm.LogID = dt.Rows[0]["LogID"].ToString();
                lm.TableName = dt.Rows[0]["TableName"].ToString();
                lm.FieldName = dt.Rows[0]["FieldName"].ToString();
                lm.ValueOld = dt.Rows[0]["ValueOld"].ToString();
                lm.ValueNew = dt.Rows[0]["ValueNew"].ToString();
                lm.CreateID = dt.Rows[0]["CreateID"].ToString();
                lm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
            }
            return lm;
        }

        public static DataTable LogTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Sys_Log WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <typeparam name="M1"></typeparam>
        /// <typeparam name="M2"></typeparam>
        /// <param name="TableName">表明</param>
        /// <param name="a">修改前实体类</param>
        /// <param name="b">修改后实体类</param>
        /// <returns></returns>
        public static bool CreateLog<M1, M2>(string TableName,string UserID, M1 a, M2 b)
        {
            PropertyInfo[] properties = a.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int i = 0;
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value1 = item.GetValue(a, null);
                object value2 = item.GetValue(b, null);
                if (value2 != null)
                {
                    if (value1.ToString() != value2.ToString())
                    {
                        LogModel lm = new LogModel();
                        lm.TableName = TableName;
                        lm.FieldName = name;
                        lm.ValueOld = value1.ToString();
                        lm.ValueNew = value2.ToString();
                        lm.CreateID = UserID;
                        lm.CreateTime = DateTime.Now.ToString();

                        InsertLog(lm);
                    }
                }
                i += 1;
            }
            return true;
        }
    }
}
