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
    public class sItemsEnrollBLL
    {
        public static int InsertsItemsEnroll(sItemsEnrollModel iem)
        {
            string cmdText = @"INSERT INTO T_Stu_sItemsEnroll
(ItemID
,sEnrollsProfessionID
)
VALUES (@ItemID
,@sEnrollsProfessionID
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@ItemID", iem.ItemID),
new SqlParameter("@sEnrollsProfessionID", iem.sEnrollsProfessionID)
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

        public static int UpdatesItemsEnroll(sItemsEnrollModel iem)
        {
            string cmdText = @"UPDATE T_Stu_sItemsEnroll SET
ItemID=@ItemID
,sEnrollsProfessionID=@sEnrollsProfessionID
WHERE sItemsEnrollID=@sItemsEnrollID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@sItemsEnrollID", iem.sItemsEnrollID),
new SqlParameter("@ItemID", iem.ItemID),
new SqlParameter("@sEnrollsProfessionID", iem.sEnrollsProfessionID)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(iem.sItemsEnrollID);
            }
            else
            {
                return -1;
            }
        }

        public static sItemsEnrollModel sItemsEnrollModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sItemsEnrollModel iem = new sItemsEnrollModel();
            string cmdText = "SELECT * FROM T_Stu_sItemsEnroll WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                iem.sItemsEnrollID = dt.Rows[0]["sItemsEnrollID"].ToString();
                iem.ItemID = dt.Rows[0]["ItemID"].ToString();
                iem.sEnrollsProfessionID = dt.Rows[0]["sEnrollsProfessionID"].ToString();
            }
            return iem;
        }

        public static DataTable sItemsEnrollTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sItemsEnroll WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
