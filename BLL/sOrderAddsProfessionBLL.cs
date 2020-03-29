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
    public class sOrderAddsProfessionBLL
    {
        public static int InsertsOrderAddsProfession(sOrderAddsProfessionModel oapm)
        {
            string cmdText = @"INSERT INTO T_Stu_sOrderAddsProfession
(Status
,sOrderAddID
,sProfessionID
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@sOrderAddID
,@sProfessionID
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", oapm.Status),
new SqlParameter("@sOrderAddID", oapm.sOrderAddID),
new SqlParameter("@sProfessionID", oapm.sProfessionID),
new SqlParameter("@CreateID", oapm.CreateID),
new SqlParameter("@CreateTime", oapm.CreateTime),
new SqlParameter("@UpdateID", oapm.UpdateID),
new SqlParameter("@UpdateTime", oapm.UpdateTime)
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

        public static int UpdatesOrderAddsProfession(sOrderAddsProfessionModel oapm)
        {
            string cmdText = @"UPDATE T_Stu_sOrderAddsProfession SET
Status=@Status
,sOrderAddID=@sOrderAddID
,sProfessionID=@sProfessionID
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sOrderAddsProfessionID=@sOrderAddsProfessionID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@sOrderAddsProfessionID", oapm.sOrderAddsProfessionID),
new SqlParameter("@Status", oapm.Status),
new SqlParameter("@sOrderAddID", oapm.sOrderAddID),
new SqlParameter("@sProfessionID", oapm.sProfessionID),
new SqlParameter("@CreateID", oapm.CreateID),
new SqlParameter("@CreateTime", oapm.CreateTime),
new SqlParameter("@UpdateID", oapm.UpdateID),
new SqlParameter("@UpdateTime", oapm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(oapm.sOrderAddsProfessionID);
            }
            else
            {
                return -1;
            }
        }

        public static sOrderAddsProfessionModel sOrderAddsProfessionModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sOrderAddsProfessionModel oapm = new sOrderAddsProfessionModel();
            string cmdText = "SELECT * FROM T_Stu_sOrderAddsProfession WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                oapm.sOrderAddsProfessionID = dt.Rows[0]["sOrderAddsProfessionID"].ToString();
                oapm.Status = dt.Rows[0]["Status"].ToString();
                oapm.sOrderAddID = dt.Rows[0]["sOrderAddID"].ToString();
                oapm.sProfessionID = dt.Rows[0]["sProfessionID"].ToString();
                oapm.CreateID = dt.Rows[0]["CreateID"].ToString();
                oapm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                oapm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                oapm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return oapm;
        }

        public static DataTable sOrderAddsProfessionTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sOrderAddsProfession WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
