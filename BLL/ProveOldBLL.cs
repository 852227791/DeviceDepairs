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
 public   class ProveOldBLL
    {
        public static int InsertProveOld(ProveOldModel pom)
        {
            string cmdText = @"INSERT INTO T_Pro_ProveOld
(OldID
)
VALUES (@OldID
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@OldID", pom.OldID)
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

        public static int UpdateProveOld(ProveOldModel pom)
        {
            string cmdText = @"UPDATE T_Pro_ProveOld SET
OldID=@OldID
WHERE ProveOldID=@ProveOldID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ProveOldID", pom.ProveOldID),
new SqlParameter("@OldID", pom.OldID)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(pom.ProveOldID);
            }
            else
            {
                return -1;
            }
        }

        public static ProveOldModel ProveOldModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            ProveOldModel pom = new ProveOldModel();
            string cmdText = "SELECT * FROM T_Pro_ProveOld WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                pom.ProveOldID = dt.Rows[0]["ProveOldID"].ToString();
                pom.OldID = dt.Rows[0]["OldID"].ToString();
            }
            return pom;
        }

        public static DataTable ProveOldTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_ProveOld WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static DataTable ProveOldTableByWhere(string itemId,string studentId)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  *
FROM    T_Pro_ProveOld
WHERE   OldID IN ( SELECT   ProveID
                   FROM     dbo.T_Pro_Prove
                   WHERE    Status IN ( 1, 2, 3 )  AND StudentID=@StudentID  AND ItemID=@ItemID) ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",studentId),
                new SqlParameter("@ItemID",itemId)
            };
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

    }
}
