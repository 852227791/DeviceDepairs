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
    public class sEnrollsProfessionStatusBLL
    {
        public static int InsertsEnrollsProfessionStatus(sEnrollsProfessionStatusModel epsm)
        {
            string cmdText = @"INSERT INTO T_Stu_sEnrollsProfessionStatus
(Status
,sEnrollsProfessionID
,StatusValue
,Explain
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@sEnrollsProfessionID
,@StatusValue
,@Explain
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", epsm.Status),
new SqlParameter("@sEnrollsProfessionID", epsm.sEnrollsProfessionID),
new SqlParameter("@StatusValue", epsm.StatusValue),
new SqlParameter("@Explain", epsm.Explain),
new SqlParameter("@CreateID", epsm.CreateID),
new SqlParameter("@CreateTime", epsm.CreateTime),
new SqlParameter("@UpdateID", epsm.UpdateID),
new SqlParameter("@UpdateTime", epsm.UpdateTime)
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

        public static int UpdatesEnrollsProfessionStatus(sEnrollsProfessionStatusModel epsm)
        {
            string cmdText = @"UPDATE T_Stu_sEnrollsProfessionStatus SET
Status=@Status
,sEnrollsProfessionID=@sEnrollsProfessionID
,StatusValue=@StatusValue
,Explain=@Explain
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sEnrollsProfessionStatusID=@sEnrollsProfessionStatusID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionStatusID", epsm.sEnrollsProfessionStatusID),
new SqlParameter("@Status", epsm.Status),
new SqlParameter("@sEnrollsProfessionID", epsm.sEnrollsProfessionID),
new SqlParameter("@StatusValue", epsm.StatusValue),
new SqlParameter("@Explain", epsm.Explain),
new SqlParameter("@CreateID", epsm.CreateID),
new SqlParameter("@CreateTime", epsm.CreateTime),
new SqlParameter("@UpdateID", epsm.UpdateID),
new SqlParameter("@UpdateTime", epsm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(epsm.sEnrollsProfessionStatusID);
            }
            else
            {
                return -1;
            }
        }

        public static sEnrollsProfessionStatusModel sEnrollsProfessionStatusModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sEnrollsProfessionStatusModel epsm = new sEnrollsProfessionStatusModel();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfessionStatus WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                epsm.sEnrollsProfessionStatusID = dt.Rows[0]["sEnrollsProfessionStatusID"].ToString();
                epsm.Status = dt.Rows[0]["Status"].ToString();
                epsm.sEnrollsProfessionID = dt.Rows[0]["sEnrollsProfessionID"].ToString();
                epsm.StatusValue = dt.Rows[0]["StatusValue"].ToString();
                epsm.Explain = dt.Rows[0]["Explain"].ToString();
                epsm.CreateID = dt.Rows[0]["CreateID"].ToString();
                epsm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                epsm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                epsm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return epsm;
        }

        public static DataTable sEnrollsProfessionStatusTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfessionStatus WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
