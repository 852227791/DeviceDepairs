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
    public class sOrderAddBLL
    {
        public static int InsertsOrderAdd(sOrderAddModel oam)
        {
            string cmdText = @"INSERT INTO T_Stu_sOrderAdd
(Status
,DeptID
,Year
,Month
,Name
,PlanItemID
,NumItemID
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@DeptID
,@Year
,@Month
,@Name
,@PlanItemID
,@NumItemID
,@Remark
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", oam.Status),
new SqlParameter("@DeptID", oam.DeptID),
new SqlParameter("@Year", oam.Year),
new SqlParameter("@Month", oam.Month),
new SqlParameter("@Name", oam.Name),
new SqlParameter("@PlanItemID", oam.PlanItemID),
new SqlParameter("@NumItemID", oam.NumItemID),
new SqlParameter("@Remark", oam.Remark),
new SqlParameter("@CreateID", oam.CreateID),
new SqlParameter("@CreateTime", oam.CreateTime),
new SqlParameter("@UpdateID", oam.UpdateID),
new SqlParameter("@UpdateTime", oam.UpdateTime)
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

        public static int UpdatesOrderAdd(sOrderAddModel oam)
        {
            string cmdText = @"UPDATE T_Stu_sOrderAdd SET
Status=@Status
,DeptID=@DeptID
,Year=@Year
,Month=@Month
,Name=@Name
,PlanItemID=@PlanItemID
,NumItemID=@NumItemID
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sOrderAddID=@sOrderAddID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@sOrderAddID", oam.sOrderAddID),
new SqlParameter("@Status", oam.Status),
new SqlParameter("@DeptID", oam.DeptID),
new SqlParameter("@Year", oam.Year),
new SqlParameter("@Month", oam.Month),
new SqlParameter("@Name", oam.Name),
new SqlParameter("@PlanItemID", oam.PlanItemID),
new SqlParameter("@NumItemID", oam.NumItemID),
new SqlParameter("@Remark", oam.Remark),
new SqlParameter("@CreateID", oam.CreateID),
new SqlParameter("@CreateTime", oam.CreateTime),
new SqlParameter("@UpdateID", oam.UpdateID),
new SqlParameter("@UpdateTime", oam.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(oam.sOrderAddID);
            }
            else
            {
                return -1;
            }
        }

        public static sOrderAddModel sOrderAddModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sOrderAddModel oam = new sOrderAddModel();
            string cmdText = "SELECT * FROM T_Stu_sOrderAdd WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                oam.sOrderAddID = dt.Rows[0]["sOrderAddID"].ToString();
                oam.Status = dt.Rows[0]["Status"].ToString();
                oam.DeptID = dt.Rows[0]["DeptID"].ToString();
                oam.Year = dt.Rows[0]["Year"].ToString();
                oam.Month = dt.Rows[0]["Month"].ToString();
                oam.Name = dt.Rows[0]["Name"].ToString();
                oam.PlanItemID = dt.Rows[0]["PlanItemID"].ToString();
                oam.NumItemID = dt.Rows[0]["NumItemID"].ToString();
                oam.Remark = dt.Rows[0]["Remark"].ToString();
                oam.CreateID = dt.Rows[0]["CreateID"].ToString();
                oam.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                oam.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                oam.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return oam;
        }

        public static DataTable sOrderAddTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sOrderAdd WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        /// <summary>
        /// 根据专业ID取缴费方案（排除停用的），可能有多个sProfessionID
        /// </summary>
        /// <param name="majorIDs"></param>
        /// <returns></returns>
        public static DataTable GetScheme(string majorIDs)
        {
            string cmdText = @"SELECT DISTINCT
        t.*
FROM    ( SELECT TOP ( 100 ) PERCENT
                    item.ItemID AS Id ,
                    item.EnglishName + ' ' + item.Name AS Text
          FROM      T_Stu_sItemsProfession AS sitempro
                    LEFT JOIN T_Pro_Item AS item ON sitempro.ItemID = item.ItemID
          WHERE     sitempro.sProfessionID IN ( {0} )
                    AND item.Status = 1
          ORDER BY  item.EnglishName ASC
        ) AS t";
            cmdText = string.Format(cmdText, majorIDs);
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
        }

        /// <summary>
        /// 根据缴费方案ID得到缴费期数（排除停用的）
        /// </summary>
        /// <param name="schemeID"></param>
        /// <returns></returns>
        public static DataTable GetSemester(string schemeID)
        {
            string cmdText = @"SELECT  ItemID AS Id ,
        Name AS Text
FROM    T_Pro_Item
WHERE   ParentID = {0}
        AND Status = 1
ORDER BY Queue ASC";
            cmdText = string.Format(cmdText, schemeID);
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
        }

        public static DataTable GetInfo(string DeptID, string sProfessionID, string Scheme, string Semester, string Year, string Month)
        {
            string cmdText = @"SELECT DISTINCT
        student.Name AS StuName ,
        student.IDCard ,
        item.Name AS PlanName ,
        item_num.Name AS NumName ,
        sorder.sEnrollsProfessionID ,
        sorder.PlanItemID ,
        sorder.NumItemID,
        item_num.PlanLevel
FROM    T_Stu_sOrder AS sorder
        LEFT JOIN T_Stu_sEnrollsProfession AS spro ON spro.sEnrollsProfessionID = sorder.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll AS senroll ON senroll.sEnrollID = spro.sEnrollID
        LEFT JOIN T_Pro_Student AS student ON student.StudentID = senroll.StudentID
        LEFT JOIN T_Pro_Item AS item ON item.ItemID = sorder.PlanItemID
        LEFT JOIN T_Pro_Item AS item_num ON item_num.ItemID = sorder.NumItemID
WHERE   sorder.DeptID = {0}
        AND sorder.sEnrollsProfessionID IN ( SELECT DISTINCT
                                                    sEnrollsProfessionID
                                             FROM   T_Stu_sEnrollsProfession
                                             WHERE  DeptID = {0}
                                                    AND Year = {1}
                                                    AND Month = {2}
                                                    AND sProfessionID IN ( {3} )
                                                    AND Status != 9 )
        AND sorder.PlanItemID = {4}
        AND sorder.NumItemID = {5}
        AND sorder.Year = {1}
        AND sorder.Month = {2}
        AND sorder.Status != 9";
            cmdText = string.Format(cmdText, DeptID, Year, Month, sProfessionID, Scheme, Semester);
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
        }

        /// <summary>
        /// 撤销批量缴费单批量添加
        /// </summary>
        /// <param name="sOrderAddID"></param>
        public static string RebacksOrderAdd(string sOrderAddID)
        {
            sOrderAddModel model = sOrderAddBLL.sOrderAddModelByWhere(" AND sOrderAddID=@sOrderAddID",
                new SqlParameter[] { new SqlParameter("@sOrderAddID", sOrderAddID) });
            string cmdText = @"UPDATE  T_Stu_sOrder
        SET     Status = 9
        WHERE   DeptID = @DeptID
                AND sEnrollsProfessionID IN (
                SELECT DISTINCT
                        sEnrollsProfessionID
                FROM    T_Stu_sEnrollsProfession
                WHERE   DeptID = @DeptID
                        AND Year = @Year
                        AND Month = @Month
                        AND sProfessionID IN (
                        SELECT DISTINCT
                                sProfessionID
                        FROM    T_Stu_sOrderAddsProfession
                        WHERE   sOrderAddID = @sOrderAddID ) )
                AND PlanItemID = @PlanItemID
                AND NumItemID = @NumItemID
                AND Year = @Year
                AND Month = @Month
                AND ItemDetailID = 0
                AND DetailID IN ( SELECT DISTINCT
                                            DetailID
                                  FROM      T_Stu_sOrderAddDetail
                                  WHERE     sOrderAddID = @sOrderAddID ) AND PaidMoney=0";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@DeptID", model.DeptID),
new SqlParameter("@Year", model.Year),
new SqlParameter("@Month", model.Month),
new SqlParameter("@sOrderAddID", sOrderAddID),
new SqlParameter("@PlanItemID", model.PlanItemID),
new SqlParameter("@NumItemID", model.NumItemID)
};
            string cmdText2 = @"SELECT * FROM  T_Stu_sOrder
        WHERE   DeptID = @DeptID
                AND sEnrollsProfessionID IN (
                SELECT DISTINCT
                        sEnrollsProfessionID
                FROM    T_Stu_sEnrollsProfession
                WHERE   DeptID = @DeptID
                        AND Year = @Year
                        AND Month = @Month
                        AND sProfessionID IN (
                        SELECT DISTINCT
                                sProfessionID
                        FROM    T_Stu_sOrderAddsProfession
                        WHERE   sOrderAddID = @sOrderAddID ) )
                AND PlanItemID = @PlanItemID
                AND NumItemID = @NumItemID
                AND Year = @Year
                AND Month = @Month
                AND ItemDetailID = 0
                AND DetailID IN ( SELECT DISTINCT
                                            DetailID
                                  FROM      T_Stu_sOrderAddDetail
                                  WHERE     sOrderAddID = @sOrderAddID ) AND PaidMoney>0";
            DatabaseFactory.ExecuteNonQuery("UPDATE T_Stu_sOrderAdd SET Status=2 WHERE sOrderAddID=" + sOrderAddID, CommandType.Text);
            int successCount = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            DataTable failDt = DatabaseFactory.ExecuteDataset(cmdText2, CommandType.Text, paras).Tables[0];
            if (failDt.Rows.Count > 0)
            {
                string failID = string.Join(",", failDt.AsEnumerable().Select(x => x.Field<int>("sOrderID")).ToArray());
                return "成功" + successCount + "条，失败" + failDt.Rows.Count + "条。失败的缴费单ID【" + failID + "】";
            }
            else
            {
                return "成功" + successCount + "条，失败0条。";
            }
        }
    }
}
