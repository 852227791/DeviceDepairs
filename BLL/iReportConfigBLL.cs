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
    public class iReportConfigBLL
    {
        public static int InsertiReportConfig(iReportConfigModel rcm)
        {
            string cmdText = @"INSERT INTO T_Inc_iReportConfig
(Sort
,ID
,DetailID
)
VALUES (@Sort
,@ID
,@DetailID
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Sort", rcm.Sort),
new SqlParameter("@ID", rcm.ID),
new SqlParameter("@DetailID", rcm.DetailID)
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

        public static int UpdateiReportConfig(iReportConfigModel rcm)
        {
            string cmdText = @"UPDATE T_Inc_iReportConfig SET
Sort=@Sort
,ID=@ID
,DetailID=@DetailID
WHERE iReportConfigID=@iReportConfigID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@iReportConfigID", rcm.iReportConfigID),
new SqlParameter("@Sort", rcm.Sort),
new SqlParameter("@ID", rcm.ID),
new SqlParameter("@DetailID", rcm.DetailID)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(rcm.iReportConfigID);
            }
            else
            {
                return -1;
            }
        }

        public static iReportConfigModel iReportConfigModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            iReportConfigModel rcm = new iReportConfigModel();
            string cmdText = "SELECT * FROM T_Inc_iReportConfig WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                rcm.iReportConfigID = dt.Rows[0]["iReportConfigID"].ToString();
                rcm.Sort = dt.Rows[0]["Sort"].ToString();
                rcm.ID = dt.Rows[0]["ID"].ToString();
                rcm.DetailID = dt.Rows[0]["DetailID"].ToString();
            }
            return rcm;
        }

        public static DataTable iReportConfigTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Inc_iReportConfig WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static int DeleteiReportConfig()
        {
            string cmdText = @"DELETE  FROM T_Inc_iReportConfig";
            int result = Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text));
            if (result > 0)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }
    }
}
