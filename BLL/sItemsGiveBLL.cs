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
    public class sItemsGiveBLL
    {
        public static int InsertsItemsGive(sItemsGiveModel igm)
        {
            string cmdText = @"INSERT INTO T_Stu_sItemsGive
(ItemID
,sGiveID
,Queue
)
VALUES (@ItemID
,@sGiveID
,@Queue
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ItemID", igm.ItemID),
new SqlParameter("@sGiveID", igm.sGiveID),
new SqlParameter("@Queue", igm.Queue)
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

        public static int UpdatesItemsGive(sItemsGiveModel igm)
        {
            string cmdText = @"UPDATE T_Stu_sItemsGive SET
ItemID=@ItemID
,sGiveID=@sGiveID
,Queue=@Queue
WHERE sItemsGiveID=@sItemsGiveID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sItemsGiveID", igm.sItemsGiveID),
new SqlParameter("@ItemID", igm.ItemID),
new SqlParameter("@sGiveID", igm.sGiveID),
new SqlParameter("@Queue", igm.Queue)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(igm.sItemsGiveID);
            }
            else
            {
                return -1;
            }
        }

        public static sItemsGiveModel sItemsGiveModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sItemsGiveModel igm = new sItemsGiveModel();
            string cmdText = "SELECT * FROM T_Stu_sItemsGive WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                igm.sItemsGiveID = dt.Rows[0]["sItemsGiveID"].ToString();
                igm.ItemID = dt.Rows[0]["ItemID"].ToString();
                igm.sGiveID = dt.Rows[0]["sGiveID"].ToString();
                igm.Queue = dt.Rows[0]["Queue"].ToString();
            }
            return igm;
        }

        public static DataTable sItemsGiveTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sItemsGive WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 删除招生方案配品
        /// </summary>
        /// <param name="sitemsGiveId"></param>
        /// <returns></returns>
        public static int DeletesItemsGive(string sitemsGiveId)
        {
            string cmdText = @" DELETE FROM T_Stu_sItemsGive WHERE sItemsGiveID=@sItemsGiveID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sItemsGiveID",sitemsGiveId)
            };
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
                return result;
            return -1;
        }
        /// <summary>
        /// 获取方案下配品
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static DataTable GetItemGive(string itemId,string sEnrollsProfessionID)
        {
            string cmdText = @"SELECT  g.Name GiveName ,
        og.sOrderGiveID ,
        og.Queue
FROM    T_Stu_sOrderGive og
        LEFT JOIN T_Stu_sGive g ON g.sGiveID = og.sGiveID
WHERE   og.Status IN (1,2)
        AND og.PlanItemID = @ItemID
        AND og.sEnrollsProfessionID = @sEnrollsProfessionID
ORDER BY og.Queue";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemID",itemId),
                new SqlParameter("@sEnrollsProfessionID",sEnrollsProfessionID)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];

        }
        /// <summary>
        /// 修改时获取当前的配品（作废）
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="sFeeId"></param>
        /// <returns></returns>
        public static DataTable GetsFeeOrderGive(string itemId,string sFeeId)
        {
            string cmdText = @"
            SELECT  g.Name GiveName ,
                    og.sOrderGiveID ,
                    og.Queue
            FROM    T_Stu_sOrderGive og
                    LEFT JOIN T_Stu_sGive g ON g.sGiveID = og.sGiveID
            WHERE   og.Status = 1
                    AND og.PlanItemID = @ItemID
                    or og.sOrderGiveID IN ( SELECT sOrderGiveID
                                             FROM   T_Stu_sFeesOrderGive
                                             WHERE  Status = 1
                                                    AND sFeeID =@sFeeID )
            ORDER BY og.Queue";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ItemID",itemId),
                new SqlParameter("@sFeeID",sFeeId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
    }
}
