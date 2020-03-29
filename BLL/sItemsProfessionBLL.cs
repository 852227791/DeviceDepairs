
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
    public class sItemsProfessionBLL
    {
        public static int InsertsItemsProfession(sItemsProfessionModel ipm)
        {
            string cmdText = @"INSERT INTO T_Stu_sItemsProfession
(ItemID
,sProfessionID
)
VALUES (@ItemID
,@sProfessionID
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ItemID", ipm.ItemID),
new SqlParameter("@sProfessionID", ipm.sProfessionID)
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

        public static int UpdatesItemsProfession(sItemsProfessionModel ipm)
        {
            string cmdText = @"UPDATE T_Stu_sItemsProfession SET
ItemID=@ItemID
,sProfessionID=@sProfessionID
WHERE sItemsProfessionID=@sItemsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sItemsProfessionID", ipm.sItemsProfessionID),
new SqlParameter("@ItemID", ipm.ItemID),
new SqlParameter("@sProfessionID", ipm.sProfessionID)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(ipm.sItemsProfessionID);
            }
            else
            {
                return -1;
            }
        }

        public static sItemsProfessionModel sItemsProfessionModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sItemsProfessionModel ipm = new sItemsProfessionModel();
            string cmdText = "SELECT * FROM T_Stu_sItemsProfession WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ipm.sItemsProfessionID = dt.Rows[0]["sItemsProfessionID"].ToString();
                ipm.ItemID = dt.Rows[0]["ItemID"].ToString();
                ipm.sProfessionID = dt.Rows[0]["sProfessionID"].ToString();
            }
            return ipm;
        }

        public static DataTable sItemsProfessionTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sItemsProfession WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 删除招生方案专业
        /// </summary>
        /// <param name="sitemProfessionId"></param>
        /// <returns></returns>
        public static int DeletesItemsProfession(string sitemProfessionId)
        {
            string cmdText = "DELETE FROM T_Stu_sItemsProfession WHERE sItemsProfessionID=@sItemsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sItemsProfessionID", sitemProfessionId)
};

            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 根据年、月、专业、类型返回方案信息
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="sProfessionId"></param>
        /// <param name="upSort"></param>
        /// <returns></returns>
        public static DataTable GetItemsProfession(string year, string month, string sProfessionId,string upSort)
        {
            string cmdText = @"SELECT  item.ItemID ,
        item.Year ,
        refe_month.RefeName AS Month ,
        item.Name
FROM    T_Stu_sItemsProfession AS sitempro
        LEFT JOIN T_Pro_Item AS item ON sitempro.ItemID = item.ItemID
        LEFT JOIN T_Sys_Refe AS refe_month ON item.Month = refe_month.Value
                                              AND refe_month.RefeTypeID = 16
WHERE   sitempro.sProfessionID = @sProfessionID
        AND item.Year = @Year
        AND item.Month = @Month
        AND item.Status = 1
        AND item.IsPlan = 1
        and item.Sort =@Sort
        AND ( GETDATE() >= CONVERT(NVARCHAR(10), item.StartTime, 23)
              OR CONVERT(NVARCHAR(10), item.StartTime, 23) = '1900-01-01'
            )
        AND ( GETDATE() <= CONVERT(NVARCHAR(10), item.EndTime, 23)
              OR CONVERT(NVARCHAR(10), item.EndTime, 23) = '1900-01-01'
            )";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Year",year),
                 new SqlParameter("@sProfessionID",sProfessionId),
                  new SqlParameter("@Month",month),
                   new SqlParameter("@Sort",upSort)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
        /// <summary>
        /// 根据招生专业id返回方案
        /// </summary>
        /// <param name="sprofesionId"></param>
        /// <returns></returns>
        public static DataTable GetItemsProfessionTable(string sprofesionId)
        {
            string where = " and sProfessionID=@sProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sProfessionID",sprofesionId)
            };
            return sItemsProfessionBLL.sItemsProfessionTableByWhere(where, paras, "");
        }
        public static DataTable GetItemsProfessionTable(string sprofesionId, string type, string level)
        {
            string where = " and sProfessionID=@sProfessionID AND ItemID IN (SELECT ItemID FROM T_Pro_Item WHERE Status=1 AND Sort=@Sort )";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sProfessionID",sprofesionId),
                new SqlParameter("@Sort",type)
            };
            return sItemsProfessionBLL.sItemsProfessionTableByWhere(where, paras, "");
        }
    }
}
