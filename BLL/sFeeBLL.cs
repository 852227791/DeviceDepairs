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
    public class sFeeBLL
    {
        public static List<sFeeModel> SelectsFeeByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            List<sFeeModel> list = new List<sFeeModel>();
            string cmdText = "SELECT * FROM T_Stu_sFee WHERE 1 = 1 {0}{1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {

                sFeeModel fm = new sFeeModel();
                fm.sFeeID = dr["sFeeID"].ToString();
                fm.Status = dr["Status"].ToString();
                fm.DeptID = dr["DeptID"].ToString();
                fm.VoucherNum = dr["VoucherNum"].ToString();
                fm.NoteNum = dr["NoteNum"].ToString();
                fm.sEnrollsProfessionID = dr["sEnrollsProfessionID"].ToString();
                fm.PlanItemID = dr["PlanItemID"].ToString();
                fm.NumItemID = dr["NumItemID"].ToString();
                fm.FeeTime = dr["FeeTime"].ToString();
                fm.FeeMode = dr["FeeMode"].ToString();
                fm.ShouldMoney = dr["ShouldMoney"].ToString();
                fm.PaidMoney = dr["PaidMoney"].ToString();
                fm.PrintNum = dr["PrintNum"].ToString();
                fm.AffirmID = dr["AffirmID"].ToString();
                fm.AffirmTime = dr["AffirmTime"].ToString();
                fm.Explain = dr["Explain"].ToString();
                fm.Remark = dr["Remark"].ToString();
                fm.CreateID = dr["CreateID"].ToString();
                fm.CreateTime = dr["CreateTime"].ToString();
                fm.UpdateID = dr["UpdateID"].ToString();
                fm.UpdateTime = dr["UpdateTime"].ToString();
                list.Add(fm);
            }
            return list;
        }
        public static int InsertsFee(sFeeModel fm)
        {
            string cmdText = @"INSERT INTO T_Stu_sFee
(Status
,DeptID
,VoucherNum
,NoteNum
,sEnrollsProfessionID
,PlanItemID
,NumItemID
,FeeTime
,FeeMode
,ShouldMoney
,PaidMoney
,PrintNum
,AffirmID
,AffirmTime
,Explain
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@DeptID
,@VoucherNum
,@NoteNum
,@sEnrollsProfessionID
,@PlanItemID
,@NumItemID
,@FeeTime
,@FeeMode
,@ShouldMoney
,@PaidMoney
,@PrintNum
,@AffirmID
,@AffirmTime
,@Explain
,@Remark
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", fm.Status),
new SqlParameter("@DeptID", fm.DeptID),
new SqlParameter("@VoucherNum", fm.VoucherNum),
new SqlParameter("@NoteNum", fm.NoteNum),
new SqlParameter("@sEnrollsProfessionID", fm.sEnrollsProfessionID),
new SqlParameter("@PlanItemID", fm.PlanItemID),
new SqlParameter("@NumItemID", fm.NumItemID),
new SqlParameter("@FeeTime", fm.FeeTime),
new SqlParameter("@FeeMode", fm.FeeMode),
new SqlParameter("@ShouldMoney", fm.ShouldMoney),
new SqlParameter("@PaidMoney", fm.PaidMoney),
new SqlParameter("@PrintNum", fm.PrintNum),
new SqlParameter("@AffirmID", fm.AffirmID),
new SqlParameter("@AffirmTime", fm.AffirmTime),
new SqlParameter("@Explain", fm.Explain),
new SqlParameter("@Remark", fm.Remark),
new SqlParameter("@CreateID", fm.CreateID),
new SqlParameter("@CreateTime", fm.CreateTime),
new SqlParameter("@UpdateID", fm.UpdateID),
new SqlParameter("@UpdateTime", fm.UpdateTime)
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

        public static int UpdatesFee(sFeeModel fm)
        {
            string cmdText = @"UPDATE T_Stu_sFee SET
Status=@Status
,DeptID=@DeptID
,VoucherNum=@VoucherNum
,NoteNum=@NoteNum
,sEnrollsProfessionID=@sEnrollsProfessionID
,PlanItemID=@PlanItemID
,NumItemID=@NumItemID
,FeeTime=@FeeTime
,FeeMode=@FeeMode
,ShouldMoney=@ShouldMoney
,PaidMoney=@PaidMoney
,PrintNum=@PrintNum
,AffirmID=@AffirmID
,AffirmTime=@AffirmTime
,Explain=@Explain
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sFeeID=@sFeeID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sFeeID", fm.sFeeID),
new SqlParameter("@Status", fm.Status),
new SqlParameter("@DeptID", fm.DeptID),
new SqlParameter("@VoucherNum", fm.VoucherNum),
new SqlParameter("@NoteNum", fm.NoteNum),
new SqlParameter("@sEnrollsProfessionID", fm.sEnrollsProfessionID),
new SqlParameter("@PlanItemID", fm.PlanItemID),
new SqlParameter("@NumItemID", fm.NumItemID),
new SqlParameter("@FeeTime", fm.FeeTime),
new SqlParameter("@FeeMode", fm.FeeMode),
new SqlParameter("@ShouldMoney", fm.ShouldMoney),
new SqlParameter("@PaidMoney", fm.PaidMoney),
new SqlParameter("@PrintNum", fm.PrintNum),
new SqlParameter("@AffirmID", fm.AffirmID),
new SqlParameter("@AffirmTime", fm.AffirmTime),
new SqlParameter("@Explain", fm.Explain),
new SqlParameter("@Remark", fm.Remark),
new SqlParameter("@CreateID", fm.CreateID),
new SqlParameter("@CreateTime", fm.CreateTime),
new SqlParameter("@UpdateID", fm.UpdateID),
new SqlParameter("@UpdateTime", fm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(fm.sFeeID);
            }
            else
            {
                return -1;
            }
        }

        public static sFeeModel sFeeModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sFeeModel fm = new sFeeModel();
            string cmdText = "SELECT * FROM T_Stu_sFee WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                fm.sFeeID = dt.Rows[0]["sFeeID"].ToString();
                fm.Status = dt.Rows[0]["Status"].ToString();
                fm.DeptID = dt.Rows[0]["DeptID"].ToString();
                fm.VoucherNum = dt.Rows[0]["VoucherNum"].ToString();
                fm.NoteNum = dt.Rows[0]["NoteNum"].ToString();
                fm.sEnrollsProfessionID = dt.Rows[0]["sEnrollsProfessionID"].ToString();
                fm.PlanItemID = dt.Rows[0]["PlanItemID"].ToString();
                fm.NumItemID = dt.Rows[0]["NumItemID"].ToString();
                fm.FeeTime = dt.Rows[0]["FeeTime"].ToString();
                fm.FeeMode = dt.Rows[0]["FeeMode"].ToString();
                fm.ShouldMoney = dt.Rows[0]["ShouldMoney"].ToString();
                fm.PaidMoney = dt.Rows[0]["PaidMoney"].ToString();
                fm.PrintNum = dt.Rows[0]["PrintNum"].ToString();
                fm.AffirmID = dt.Rows[0]["AffirmID"].ToString();
                fm.AffirmTime = dt.Rows[0]["AffirmTime"].ToString();
                fm.Explain = dt.Rows[0]["Explain"].ToString();
                fm.Remark = dt.Rows[0]["Remark"].ToString();
                fm.CreateID = dt.Rows[0]["CreateID"].ToString();
                fm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                fm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                fm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return fm;
        }

        public static DataTable sFeeTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sFee WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 重置打印次数
        /// </summary>
        /// <param name="sFeeIdArray"></param>
        /// <param name="printNum"></param>
        /// <returns></returns>
        public static int UpdatesFeePrintNum(string sFeeIdArray, string printNum, int userId)
        {
            if (string.IsNullOrEmpty(sFeeIdArray))
                sFeeIdArray = "0";
            string cmdText = @"UPDATE T_Stu_sFee SET
PrintNum=@PrintNum
,UpdateID=@UpdateID
,UpdateTime=getDate()
WHERE sFeeID IN (" + sFeeIdArray + ")";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@PrintNum", printNum),
new SqlParameter("@UpdateID", userId)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
                return result;
            else
                return -1;
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sFeeIdArray"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int UpdatesFeeStatus(string sFeeIdArray, string status, int userId)
        {
            if (string.IsNullOrEmpty(sFeeIdArray))
                sFeeIdArray = "0";
            string cmdText = @"UPDATE T_Stu_sFee SET
Status=@Status
,UpdateID=@UpdateID
,UpdateTime=getDate()
WHERE sFeeID IN (" + sFeeIdArray + ")";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", status),
new SqlParameter("@UpdateID", userId)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
                return result;
            else
                return -1;
        }
        /// <summary>
        /// 修改收费信息，用于表单赋值
        /// </summary>
        /// <param name="sfeeId"></param>
        /// <returns></returns>
        public static DataTable SelectsFeeInfo(string sfeeId)
        {
            if (string.IsNullOrEmpty(sfeeId))
                return null;
            string cmdText = @"SELECT
        f.sFeeID ,
        f.sEnrollsProfessionID ,
        f.DeptID ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        f.FeeMode ,
        s.Name + '_' + o.PlanName + '_' + pn.Name PlanName ,
        f.Remark ,
        f.Explain ,
        ( SELECT    ISNULL(SUM(PaidMoney), 0.00)
          FROM      T_Stu_sFeesOrder
          WHERE     sFeeID = f.sFeeID
                    AND Status = 1
        ) ShouldMoney,
        fo.sOrderID,
        o.NumItemID,
        o.PlanItemID ItemID,
        o.NumName,
        o.Year,
        o.Month ,
        ( SELECT      ISNULL(SUM(Money),0.00) 
          FROM      T_Stu_sOffset
          WHERE     RelatedID IN ( SELECT   sFeesOrderID
                                   FROM     T_Stu_sFeesOrder
                                   WHERE    sFeeID = f.sFeeID
                                            AND Status = 1 )
                    AND Status = 1
        ) OffsetMoney ,
        ( SELECT   ISNULL(SUM(Money),0.00) 
          FROM      T_Stu_sOffset
          WHERE     ByRelatedID IN ( SELECT sFeesOrderID
                                     FROM   T_Stu_sFeesOrder
                                     WHERE  sFeeID = f.sFeeID
                                            AND Status = 1 )
                    AND BySort = 3
                    AND Status = 1
        ) ByOffsetMoney,
		(SELECT ISNULL(SUM(fo.DiscountMoney),0) FROM T_Stu_sFeesOrder fo WHERE fo.Status=1 AND fo.sFeeID=f.sFeeID)DiscountMoney
FROM    T_Stu_sFee f
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Stu_sProfession sp ON sp.sProfessionID = ep.sProfessionID
        LEFT JOIN T_Pro_Profession pn ON pn.ProfessionID = sp.ProfessionID
        LEFT JOIN T_Stu_sFeesOrder fo ON fo.sFeeID = f.sFeeID
        LEFT JOIN T_Stu_sOrder o ON o.sOrderID = fo.sOrderID
        Where f.sFeeID=@sFeeID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeeID",sfeeId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];

        }
        /// <summary>
        /// 查看收费详情
        /// </summary>
        /// <param name="sfeeId"></param>
        /// <returns></returns>
        public static DataTable SelectsFeeView(string sfeeId)
        {
            if (string.IsNullOrEmpty(sfeeId))
                return null;
            string cmdText = @"SELECT  f.sFeeID ,
        f.Status StatusValue ,
        d.Name DeptName ,
        f.VoucherNum ,
        f.NoteNum ,
        r1.RefeName Status ,
        r2.RefeName FeeMode ,
        f.ShouldMoney ,
        f.PaidMoney ,
        s.Name StudName ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        ( SELECT DISTINCT
                    o.PlanName + ' ' + o.NumName
          FROM      T_Stu_sOrder o
          WHERE     o.NumItemID = f.NumItemID
                    AND o.sEnrollsProfessionID = f.sEnrollsProfessionID
        ) PlanName ,
        s.IDCard ,
        ( SELECT    ISNULL(SUM(DiscountMoney), 0)
          FROM      T_Stu_sFeesOrder
          WHERE     sFeeID = f.sFeeID
                    AND Status = 1
        ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(RefundMoney), 0)
          FROM      T_Stu_sRefund r
                    LEFT JOIN T_Stu_sFeesOrder sfo ON sfo.sFeesOrderID = r.sFeesOrderID
          WHERE     sfo.sFeeID = f.sFeeID
                    AND r.Status = 1
        ) RefundMoney,
		f.Remark,
		f.Explain, 
		u1.Name Affirm,
		CASE CONVERT(NVARCHAR(10),f.AffirmTime,23)
		WHEN '1900-01-01'
		THEN ''
		ELSE 
		CONVERT(NVARCHAR(10),f.AffirmTime,23)
		END AffirmTime,
		s.StudentID ,
        ( SELECT    dl.Name + ','
          FROM      T_Stu_sFeesOrder fo
                    LEFT JOIN T_Stu_sOrder o ON o.sOrderID = fo.sOrderID
                    LEFT JOIN T_Pro_Detail dl ON dl.DetailID = o.DetailID
					WHERE  fo.sFeeID=f.sFeeID
        FOR
          XML PATH('')
        ) FeeContent,
        ( SELECT      ISNULL(SUM(Money),0.00) 
          FROM      T_Stu_sOffset
          WHERE     RelatedID IN ( SELECT   sFeesOrderID
                                   FROM     T_Stu_sFeesOrder
                                   WHERE    sFeeID = f.sFeeID
                                            AND Status = 1 )
                    AND Status = 1
        ) OffsetMoney ,
        ( SELECT   ISNULL(SUM(Money),0.00) 
          FROM      T_Stu_sOffset
          WHERE     ByRelatedID IN ( SELECT sFeesOrderID
                                     FROM   T_Stu_sFeesOrder
                                     WHERE  sFeeID = f.sFeeID
                                            AND Status = 1 )
                    AND BySort = 3
                    AND Status = 1
        ) ByOffsetMoney ,
		 ( SELECT    g.Name + ','
          FROM      T_Stu_sFeesOrderGive fog
                    LEFT JOIN T_Stu_sOrderGive og ON og.sOrderGiveID = fog.sOrderGiveID
                    LEFT JOIN T_Stu_sGive g ON g.sGiveID = og.sGiveID
          WHERE     fog.sFeeID = f.sFeeID
                    AND fog.Status = 1
        FOR
          XML PATH('div')
        ) GiveName 
FROM    T_Stu_sFee f
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.Status
                                   AND r1.RefeTypeID = 7
        LEFT JOIN T_Sys_Refe r2 ON r2.Value = f.FeeMode
                                   AND r2.RefeTypeID = 6
		LEFT JOIN T_Sys_User u1 ON u1.UserID=f.AffirmID
        Where f.sFeeID=@sFeeID    ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeeID",sfeeId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
        /// <summary>
        /// 结账/反结账
        /// </summary>
        /// <param name="sfeeString"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static int UpdatesFeeAffirm(string sfeeString, int userId, string status, string affirmTime, int affirmId)
        {
            string cmdText = @"UPDATE T_Stu_sFee SET
Status=@Status
,AffirmID=@AffirmID
,AffirmTime=@AffirmTime
,UpdateID=@UpdateID
,UpdateTime=getDate()
WHERE sFeeID IN (" + sfeeString + ")";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", status),
new SqlParameter("@UpdateID", userId),
new SqlParameter("@AffirmID",affirmId),
new SqlParameter("@AffirmTime",affirmTime),
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
                return result;
            else
                return -1;
        }
        /// <summary>
        /// 获取收费实体s
        /// </summary>
        /// <param name="sfeeId"></param>
        /// <returns></returns>
        public static sFeeModel GetsFeeModel(string sfeeId)
        {
            string where = " and sFeeID=@sFeeID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeeID",sfeeId)
            };
            return sFeeBLL.sFeeModelByWhere(where, paras);
        }
        /// <summary>
        /// 获取打印收费信息内容
        /// </summary>
        /// <param name="sfeeId"></param>
        /// <returns></returns>
        public static DataTable GetsFeePrintContent(string sfeeId)
        {
            if (string.IsNullOrEmpty(sfeeId))
            {
                return null;
            }
            string cmdText = @"SELECT DISTINCT
        f.sFeeID ,
        f.PrintNum ,
        d2.Name DeptName ,
        f.VoucherNum ,
        d1.Name SchoolName ,
        s.Name StuName ,
        s.IDCard ,
        o.NumName ,
        ( SELECT    d.Name + ' ' + CONVERT(NVARCHAR(18), sfo.CanMoney)
          FROM      T_Stu_sOrder so
                    LEFT JOIN T_Stu_sFeesOrder sfo ON sfo.sOrderID = so.sOrderID
                    LEFT JOIN T_Pro_Detail d ON d.DetailID = so.DetailID
          WHERE     sfo.sFeeID = f.sFeeID
        FOR
          XML PATH('div')
        ) sfeeContent ,
        pn.Name ProfeessName ,
        u.Name UserName ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        f.Explain ,
        CASE ISNULL(( SELECT    SUM(PaidMoney)
                      FROM      T_Stu_sFeesOrder
                      WHERE     Status = 1
                                AND sFeeID = f.sFeeID
                    ), 0.00)
          WHEN 0.00 THEN ''
          ELSE '实收' + CONVERT(NVARCHAR(18), ( SELECT    SUM(PaidMoney)
                                              FROM      T_Stu_sFeesOrder
                                              WHERE     Status = 1
                                                        AND sFeeID = f.sFeeID
                                            )) + ' '
        END PaidRemark ,
        CASE ISNULL(( SELECT    SUM(Money)
                      FROM      T_Stu_sOffset so
                                LEFT JOIN T_Stu_sFeesOrder sfo ON sfo.sFeesOrderID = so.ByRelatedID
                      WHERE     so.Status = 1
                                AND sfo.sFeeID = f.sFeeID
                    ), 0.00)
          WHEN 0.00 THEN ''
          ELSE '被充抵'
               + CONVERT(NVARCHAR(18), ( SELECT SUM(Money)
                                         FROM   T_Stu_sOffset so
                                                LEFT JOIN T_Stu_sFeesOrder sfo ON sfo.sFeesOrderID = so.ByRelatedID
                                         WHERE  so.Status = 1
                                                AND sfo.sFeeID = f.sFeeID
                                       )) + ' '
        END ByOffsetRemark ,
        CASE ISNULL(( SELECT    SUM(Money)
                      FROM      T_Stu_sOffset so
                                LEFT JOIN T_Stu_sFeesOrder sfo ON sfo.sFeesOrderID = so.RelatedID
                      WHERE     so.Status = 1
                                AND sfo.sFeeID = f.sFeeID
                    ), 0.00)
          WHEN 0.00 THEN ''
          ELSE '充抵'
               + CONVERT(NVARCHAR(18), ( SELECT SUM(Money)
                                         FROM   T_Stu_sOffset so
                                                LEFT JOIN T_Stu_sFeesOrder sfo ON sfo.sFeesOrderID = so.RelatedID
                                         WHERE  so.Status = 1
                                                AND sfo.sFeeID = f.sFeeID
                                       )) + ' '
        END OffsetRemark ,
        CASE ISNULL(( SELECT    SUM(RefundMoney)
                      FROM      T_Stu_sRefund sr
                                LEFT JOIN T_Stu_sFeesOrder sfo ON sfo.sFeesOrderID = sr.sFeesOrderID
                      WHERE     sr.Status = 1
                                AND sfo.sFeeID = f.sFeeID
                    ), 0.00)
          WHEN 0.00 THEN ''
          ELSE '核销'
               + CONVERT(NVARCHAR(18), ( SELECT SUM(RefundMoney)
                                         FROM   T_Stu_sRefund sr
                                                LEFT JOIN T_Stu_sFeesOrder sfo ON sfo.sFeesOrderID = sr.sFeesOrderID
                                         WHERE  sr.Status = 1
                                                AND sfo.sFeeID = f.sFeeID
                                       ))
        END RefundRemark ,
        CASE ( ISNULL(( SELECT  SUM(DiscountMoney)
                        FROM    T_Stu_sFeesOrder
                        WHERE   Status = 1
                                AND sFeeID = f.sFeeID
                      ), 0.00) )
          WHEN 0.00 THEN ''
          ELSE '优惠' + CONVERT(NVARCHAR(18), ( SELECT    SUM(DiscountMoney)
                                              FROM      T_Stu_sFeesOrder
                                              WHERE     Status = 1
                                                        AND sFeeID = f.sFeeID
                                            )) + ' '
        END DiscountRemark ,
        ( SELECT    ISNULL(SUM(CanMoney), 0)
          FROM      T_Stu_sFeesOrder
          WHERE     sFeeID = f.sFeeID
                    AND Status = 1
        ) PaidMoney ,
        ( SELECT    g.Name + ','
          FROM      T_Stu_sFeesOrderGive fog
                    LEFT JOIN T_Stu_sOrderGive og ON og.sOrderGiveID = fog.sOrderGiveID
                    LEFT JOIN T_Stu_sGive g ON g.sGiveID = og.sGiveID
          WHERE     fog.sFeeID = f.sFeeID
                    AND fog.Status = 1
        FOR
          XML PATH('div')
        ) GiveName ,
        r1.RefeName FeeMode ,
        da.Name DeptAreaName ,
        e.EnrollNum ,
        r2.RefeName EnrollLevel
FROM    T_Stu_sFee f
        LEFT JOIN T_Sys_Dept d1 ON d1.DeptID = f.DeptID
        LEFT JOIN T_Sys_Dept d2 ON d2.DeptID = d1.ParentID
        LEFT JOIN T_Stu_sFeesOrder fo ON fo.sFeeID = f.sFeeID
        LEFT JOIN T_Stu_sOrder o ON o.sOrderID = fo.sOrderID
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = o.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Stu_sProfession sp ON sp.sProfessionID = ep.sProfessionID
        LEFT JOIN T_Pro_Profession pn ON pn.ProfessionID = sp.ProfessionID
        LEFT JOIN T_Sys_User u ON u.UserID = f.CreateID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.FeeMode
                                   AND r1.RefeTypeID = 6
        LEFT JOIN T_Pro_DeptArea da ON da.DeptAreaID = ep.DeptAreaID
        LEFT JOIN T_Sys_Refe r2 ON r2.Value = o.PlanLevel
                                   AND r2.RefeTypeID = 25
        Where f.sFeeID IN ({0})";
            cmdText = string.Format(cmdText, sfeeId);
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
        }
        /// <summary>
        /// 修改实缴金额
        /// </summary>
        /// <param name="sfeeId"></param>
        /// <param name="money"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int UpdatePaidMoney(string sfeeId, decimal money, int userId)
        {
            string cmdText = @"UPDATE T_Stu_sFee SET
                    PaidMoney=PaidMoney+@PaidMoney
                    ,UpdateID=@UpdateID
                    ,UpdateTime=@UpdateTime
                    WHERE sFeeID=@sFeeID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeeID",sfeeId),
                new SqlParameter("@PaidMoney",money),
                new SqlParameter("@UpdateID",userId),
                new SqlParameter("@UpdateTime",DateTime.Now.ToString())
            };
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }
        /// <summary>
        /// 删除收费
        /// </summary>
        /// <param name="sfeeId"></param>
        public static void DeletesFee(string sfeeId)
        {
            string cmdText = "DELETE T_Stu_sFee WHERE sFeeID=@sFeeID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeeID",sfeeId)
            };
            DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras);
        }

        public static DataTable GetPrintsFeeContent(string sfeeId) {
            string cmdText = @"SELECT DISTINCT
        f.sFeeID ,
        f.PrintNum ,
        d2.Name DeptName ,
        f.VoucherNum ,
        d1.Name SchoolName ,
        s.Name StuName ,
        s.IDCard ,
        ( SELECT    (d.Name + ',' + CONVERT(NVARCHAR(18), sfo.ShouldMoney)
                    + ','
                    + CONVERT(NVARCHAR(18), ( SELECT    ISNULL(SUM(sfee.CanMoney
                                                              + sfee.DiscountMoney),
                                                              0)
                                              FROM      T_Stu_sFeesOrder sfee
                                              WHERE     sOrderID = sfo.sOrderID
                                                        AND CreateTime > sfo.CreateTime
                                            )) + ','
                    + CONVERT(NVARCHAR(18), sfo.PaidMoney) + ','
                    + CONVERT(NVARCHAR(18), sfo.DiscountMoney) + ','
                    + CONVERT(NVARCHAR(18), ( SELECT    ISNULL(SUM(Money), 0)
                                              FROM      T_Stu_sOffset
                                              WHERE     RelatedID = sfo.sFeesOrderID
                                                        AND Status = 1
                                            ))) 
          FROM      T_Stu_sOrder so
                    LEFT JOIN T_Stu_sFeesOrder sfo ON sfo.sOrderID = so.sOrderID
                    LEFT JOIN T_Pro_Detail d ON d.DetailID = so.DetailID
          WHERE     sfo.sFeeID = f.sFeeID
        FOR
          XML PATH('sFeeDetail')
        ) sfeeContent ,
        pn.Name Major ,
        u.Name UserName ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        f.Explain ,
        ( SELECT    g.Name
          FROM      T_Stu_sFeesOrderGive fog
                    LEFT JOIN T_Stu_sOrderGive og ON og.sOrderGiveID = fog.sOrderGiveID
                    LEFT JOIN T_Stu_sGive g ON g.sGiveID = og.sGiveID
          WHERE     fog.sFeeID = f.sFeeID
                    AND fog.Status = 1
        FOR
          XML PATH('')
        ) GiveName ,
        r1.RefeName FeeMode ,
        da.Name DeptAreaName ,
        e.EnrollNum,
		o.NumName
FROM    T_Stu_sFee f
        LEFT JOIN T_Sys_Dept d1 ON d1.DeptID = f.DeptID
        LEFT JOIN T_Sys_Dept d2 ON d2.DeptID = d1.ParentID
        LEFT JOIN T_Stu_sFeesOrder fo ON fo.sFeeID = f.sFeeID
        LEFT JOIN T_Stu_sOrder o ON o.sOrderID = fo.sOrderID
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = o.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Stu_sProfession sp ON sp.sProfessionID = ep.sProfessionID
        LEFT JOIN T_Pro_Profession pn ON pn.ProfessionID = sp.ProfessionID
        LEFT JOIN T_Sys_User u ON u.UserID = f.CreateID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = f.FeeMode
                                   AND r1.RefeTypeID = 6
        LEFT JOIN T_Pro_DeptArea da ON da.DeptAreaID = ep.DeptAreaID
        LEFT JOIN T_Sys_Refe r2 ON r2.Value = o.PlanLevel
                                   AND r2.RefeTypeID = 25
WHERE   f.sFeeID  IN ({0})";
            cmdText = string.Format(cmdText, sfeeId);
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
        }
    }
}
