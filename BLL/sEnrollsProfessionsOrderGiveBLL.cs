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
    public class sEnrollsProfessionsOrderGiveBLL
    {
        public static int InsertsEnrollsProfessionsOrderGive(sEnrollsProfessionsOrderGiveModel epogm)
        {
            string cmdText = @"INSERT INTO T_Stu_sEnrollsProfessionsOrderGive
(sEnrollsProfessionID
,sOrderGiveID
)
VALUES (@sEnrollsProfessionID
,@sOrderGiveID
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@sEnrollsProfessionID", epogm.sEnrollsProfessionID),
new SqlParameter("@sOrderGiveID", epogm.sOrderGiveID)
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

        public static int UpdatesEnrollsProfessionsOrderGive(sEnrollsProfessionsOrderGiveModel epogm)
        {
            string cmdText = @"UPDATE T_Stu_sEnrollsProfessionsOrderGive SET
sEnrollsProfessionID=@sEnrollsProfessionID
,sOrderGiveID=@sOrderGiveID
WHERE sEnrollsProfessionsOrderGiveID=@sEnrollsProfessionsOrderGiveID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@sEnrollsProfessionsOrderGiveID", epogm.sEnrollsProfessionsOrderGiveID),
new SqlParameter("@sEnrollsProfessionID", epogm.sEnrollsProfessionID),
new SqlParameter("@sOrderGiveID", epogm.sOrderGiveID)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(epogm.sEnrollsProfessionsOrderGiveID);
            }
            else
            {
                return -1;
            }
        }

        public static sEnrollsProfessionsOrderGiveModel sEnrollsProfessionsOrderGiveModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sEnrollsProfessionsOrderGiveModel epogm = new sEnrollsProfessionsOrderGiveModel();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfessionsOrderGive WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                epogm.sEnrollsProfessionsOrderGiveID = dt.Rows[0]["sEnrollsProfessionsOrderGiveID"].ToString();
                epogm.sEnrollsProfessionID = dt.Rows[0]["sEnrollsProfessionID"].ToString();
                epogm.sOrderGiveID = dt.Rows[0]["sOrderGiveID"].ToString();
            }
            return epogm;
        }

        public static DataTable sEnrollsProfessionsOrderGiveTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfessionsOrderGive WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
