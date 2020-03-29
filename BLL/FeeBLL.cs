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
    public class FeeBLL
    {
        public static int InsertFee(FeeModel fm)
        {
            string cmdText = @"INSERT INTO T_Pro_Fee
(Status
,DeptID
,VoucherNum
,NoteNum
,FeeTime
,ProveID
,PersonSort
,FeeMode
,ShouldMoney
,PaidMoney
,Teacher
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
,@FeeTime
,@ProveID
,@PersonSort
,@FeeMode
,@ShouldMoney
,@PaidMoney
,@Teacher
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
new SqlParameter("@FeeTime", fm.FeeTime),
new SqlParameter("@ProveID", fm.ProveID),
new SqlParameter("@PersonSort", fm.PersonSort),
new SqlParameter("@FeeMode", fm.FeeMode),
new SqlParameter("@ShouldMoney", fm.ShouldMoney),
new SqlParameter("@PaidMoney", fm.PaidMoney),
new SqlParameter("@Teacher", fm.Teacher),
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

        public static int UpdateFee(FeeModel fm)
        {
            string cmdText = @"UPDATE T_Pro_Fee SET
Status=@Status
,DeptID=@DeptID
,VoucherNum=@VoucherNum
,NoteNum=@NoteNum
,FeeTime=@FeeTime
,ProveID=@ProveID
,PersonSort=@PersonSort
,FeeMode=@FeeMode
,ShouldMoney=@ShouldMoney
,PaidMoney=@PaidMoney
,Teacher=@Teacher
,PrintNum=@PrintNum
,AffirmID=@AffirmID
,AffirmTime=@AffirmTime
,Explain=@Explain
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE FeeID=@FeeID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@FeeID", fm.FeeID),
new SqlParameter("@Status", fm.Status),
new SqlParameter("@DeptID", fm.DeptID),
new SqlParameter("@VoucherNum", fm.VoucherNum),
new SqlParameter("@NoteNum", fm.NoteNum),
new SqlParameter("@FeeTime", fm.FeeTime),
new SqlParameter("@ProveID", fm.ProveID),
new SqlParameter("@PersonSort", fm.PersonSort),
new SqlParameter("@FeeMode", fm.FeeMode),
new SqlParameter("@ShouldMoney", fm.ShouldMoney),
new SqlParameter("@PaidMoney", fm.PaidMoney),
new SqlParameter("@Teacher", fm.Teacher),
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
                return Convert.ToInt32(fm.FeeID);
            }
            else
            {
                return -1;
            }
        }

        public static FeeModel FeeModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            FeeModel fm = new FeeModel();
            string cmdText = "SELECT * FROM T_Pro_Fee WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                fm.FeeID = dt.Rows[0]["FeeID"].ToString();
                fm.Status = dt.Rows[0]["Status"].ToString();
                fm.DeptID = dt.Rows[0]["DeptID"].ToString();
                fm.VoucherNum = dt.Rows[0]["VoucherNum"].ToString();
                fm.NoteNum = dt.Rows[0]["NoteNum"].ToString();
                fm.FeeTime = dt.Rows[0]["FeeTime"].ToString();
                fm.ProveID = dt.Rows[0]["ProveID"].ToString();
                fm.PersonSort = dt.Rows[0]["PersonSort"].ToString();
                fm.FeeMode = dt.Rows[0]["FeeMode"].ToString();
                fm.ShouldMoney = dt.Rows[0]["ShouldMoney"].ToString();
                fm.PaidMoney = dt.Rows[0]["PaidMoney"].ToString();
                fm.Teacher = dt.Rows[0]["Teacher"].ToString();
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

        public static DataTable FeeTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Fee WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }


        public static FeeModel GetFeeModel(string feeId)
        {
            string where = " AND FeeID = @FeeID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FeeID",feeId)
            };
            FeeModel fm = FeeBLL.FeeModelByWhere(where, paras);
            return fm;
        }
        /// <summary>
        /// 根据凭证号获取收费信息
        /// </summary>
        /// <param name="voucherNum">凭证号</param>
        /// <returns></returns>
        public static FeeModel GetFeeModelByVoucherNum(string voucherNum)
        {
            string where = " AND VoucherNum = @VoucherNum AND Status <> 9";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@VoucherNum", voucherNum)
            };
            FeeModel fm = FeeBLL.FeeModelByWhere(where, paras);
            return fm;
        }

        /// <summary>
        /// 输出打印信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static DataTable FeeTablePrintByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  f.FeeID ,
        d2.Name ParentDeptName ,
        f.VoucherNum ,
        d1.Name DeptName ,
        s.Name StudentName ,
        s.IDCard ,
        r1.RefeName FeeMode ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        i.Name ItemName ,
        f.PaidMoney
        - ( SELECT  ISNULL(SUM(r.RefundMoney), 0)
            FROM    T_Pro_Refund r
            WHERE   r.Status = 1
                    AND r.Sort IN ( 1, 2 )
                    AND r.FeeDetailID IN ( SELECT   fd.FeeDetailID
                                           FROM     T_Pro_FeeDetail fd
                                           WHERE    fd.Status = 1
                                                    AND fd.FeeID = f.FeeID )
          )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.Status = 1
                    AND o.ByFeeDetailID IN ( SELECT fd.FeeDetailID
                                             FROM   T_Pro_FeeDetail fd
                                             WHERE  fd.Status = 1
                                                    AND fd.FeeID = f.FeeID )
          )+  ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.Status = 1
                    AND o.FeeDetailID IN ( SELECT fd.FeeDetailID
                                             FROM   T_Pro_FeeDetail fd
                                             WHERE  fd.Status = 1
                                                    AND fd.FeeID = f.FeeID )
          ) ShouldMoney ,
        ( SELECT    dl.Name + ' ' + CONVERT(NVARCHAR(10), fd.ShouldMoney)
          FROM      T_Pro_FeeDetail fd
                    LEFT JOIN T_Pro_ItemDetail id ON fd.ItemDetailID = id.ItemDetailID
                    LEFT JOIN  T_Pro_Detail dl ON dl.DetailID = id.DetailID
          WHERE     fd.Status = 1
                    AND fd.FeeID = f.FeeID
        FOR
          XML PATH('div')
        ) FeeContent ,
        CASE f.PaidMoney
          WHEN 0 THEN ''
          ELSE '实收' + CONVERT(NVARCHAR(10), f.PaidMoney) + ' '
        END Remark1 ,
        CASE ( SELECT   SUM(fd.DiscountMoney)
               FROM     T_Pro_FeeDetail fd
               WHERE    fd.Status = 1
                        AND fd.FeeID = f.FeeID
             )
          WHEN 0 THEN ''
          ELSE '优惠' + CONVERT(NVARCHAR(10), ( SELECT    SUM(fd.DiscountMoney)
                                              FROM      T_Pro_FeeDetail fd
                                              WHERE     fd.Status = 1
                                                        AND fd.FeeID = f.FeeID
                                            )) + ' '
        END Remark2 ,
        CASE ( SELECT   ISNULL(SUM(o.Money), 0)
               FROM     T_Pro_Offset o
               WHERE    o.Status = 1
                        AND o.FeeDetailID IN ( SELECT   fd.FeeDetailID
                                               FROM     T_Pro_FeeDetail fd
                                               WHERE    fd.Status = 1
                                                        AND fd.FeeID = f.FeeID )
             )
          WHEN 0 THEN ''
          ELSE '充抵'
               + CONVERT(NVARCHAR(10), ( SELECT ISNULL(SUM(o.Money), 0)
                                         FROM   T_Pro_Offset o
                                         WHERE  o.Status = 1
                                                AND o.FeeDetailID IN (
                                                SELECT  fd.FeeDetailID
                                                FROM    T_Pro_FeeDetail fd
                                                WHERE   fd.Status = 1
                                                        AND fd.FeeID = f.FeeID )
                                       )) + ' '
        END Remark3 ,
        CASE ( SELECT   ISNULL(SUM(r.RefundMoney), 0)
               FROM     T_Pro_Refund r
               WHERE    r.Status = 1
                        AND r.Sort IN ( 1, 2 )
                        AND r.FeeDetailID IN ( SELECT   fd.FeeDetailID
                                               FROM     T_Pro_FeeDetail fd
                                               WHERE    fd.Status = 1
                                                        AND fd.FeeID = f.FeeID )
             )
          WHEN 0 THEN ''
          ELSE '核销'
               + CONVERT(NVARCHAR(10), ( SELECT ISNULL(SUM(r.RefundMoney), 0)
                                         FROM   T_Pro_Refund r
                                         WHERE  r.Status = 1
                                                AND r.FeeDetailID IN (
                                                SELECT  fd.FeeDetailID
                                                FROM    T_Pro_FeeDetail fd
                                                WHERE   fd.Status = 1
                                                        AND r.Sort IN ( 1, 2 )
                                                        AND fd.FeeID = f.FeeID )
                                       )) + ' '
        END Remark4 ,
        CASE ( SELECT   ISNULL(SUM(o.Money), 0)
               FROM     T_Pro_Offset o
               WHERE    o.Status = 1
                        AND o.ByFeeDetailID IN ( SELECT fd.FeeDetailID
                                                 FROM   T_Pro_FeeDetail fd
                                                 WHERE  fd.Status = 1
                                                        AND fd.FeeID = f.FeeID )
             )
          WHEN 0 THEN ''
          ELSE '被充抵'
               + CONVERT(NVARCHAR(10), ( SELECT ISNULL(SUM(o.Money), 0)
                                         FROM   T_Pro_Offset o
                                         WHERE  o.Status = 1
                                                AND o.ByFeeDetailID IN (
                                                SELECT  fd.FeeDetailID
                                                FROM    T_Pro_FeeDetail fd
                                                WHERE   fd.Status = 1
                                                        AND fd.FeeID = f.FeeID )
                                       )) + ' '
        END Remark5 ,
        f.Explain Remark ,
        c.Name ClassText ,
        f.Teacher ,
        u.Name Feeer
FROM    T_Pro_Fee f
        LEFT JOIN T_Sys_Dept d1 ON f.DeptID = d1.DeptID
        LEFT JOIN T_Sys_Dept d2 ON d1.ParentID = d2.DeptID
        LEFT JOIN T_Pro_Prove pr ON f.ProveID = pr.ProveID
        LEFT JOIN T_Pro_Student s ON pr.StudentID = s.StudentID
        LEFT JOIN T_Sys_Refe r1 ON f.FeeMode = r1.Value
                                   AND r1.RefeTypeID = 6
        LEFT JOIN T_Pro_Item i ON pr.ItemID = i.ItemID
        LEFT JOIN T_Pro_Class c ON pr.ClassID = c.ClassID
        LEFT JOIN T_Sys_User u ON f.CreateID = u.UserID
WHERE   1 = 1{0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static DataTable SelectFee(string feeId)
        {
            string cmdText = @"SELECT  f.FeeID ,
        f.ProveID ,
        f.PersonSort ,
        s.Name + '_' + i.Name + '_' + s.IDCard Name ,
        f.FeeMode ,
        f.ShouldMoney ,
        f.PaidMoney ,
        ( SELECT    ISNULL(SUM(DiscountMoney), 0.00)
          FROM      T_Pro_FeeDetail
          WHERE     FeeID = f.FeeID
                    AND Status = 1
        ) DiscountMoney ,
        f.DeptID ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Pro_Offset
          WHERE     Status = 1
                    AND FeeDetailID IN ( SELECT FeeDetailID
                                         FROM   T_Pro_FeeDetail
                                         WHERE  Status = 1
                                                AND FeeID = f.FeeID )
        ) OffsetMoney ,
        f.Teacher ,
        p.ItemID ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        f.Explain ,
        f.Remark
FROM    T_Pro_Fee f
        LEFT JOIN T_Pro_Prove AS p ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
        LEFT JOIN T_Pro_Item AS i ON i.ItemID = p.ItemID
WHERE   FeeID = @FeeID";
            SqlParameter[] paras = new SqlParameter[] {
            new  SqlParameter("@FeeID",feeId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static decimal GetOffsetMoney(string feeId, string refundId, string offsetFeeId)
        {
            string cmdText = @"SELECT  ISNULL(f.PaidMoney, 0) - ( SELECT   ISNULL(SUM(RefundMoney), 0)
                                   FROM     T_Pro_Refund r
                                   WHERE    r.Status = 1
                                           AND fd.FeeDetailID=r.FeeDetailID
                                            AND r.RefundID <> @RefundId
                                 ) - ( SELECT   ISNULL(SUM(o.Money), 0)
                                       FROM     T_Pro_Offset o
                                       WHERE    o.Status = 1
                                                AND o.ByFeeID = f.FeeID
                                                AND o.FeeID <> @OffsetFeeID
                                     ) + ( SELECT   ISNULL(SUM(o.Money), 0)
                                           FROM     T_Pro_Offset o
                                           WHERE    o.Status = 1
                                                    AND o.FeeID = f.FeeID
                                         ) Money
FROM    T_Pro_Fee f
LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeID=f.FeeID
WHERE   f.FeeID = @FeeID
        "; SqlParameter[] paras = new SqlParameter[] {
            new  SqlParameter("@FeeID",feeId),
            new  SqlParameter("@RefundId",refundId),
            new  SqlParameter("@OffsetFeeID",offsetFeeId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return Convert.ToDecimal(dt.Rows[0]["Money"].ToString());
        }
        /// <summary>
        /// 根据收费Id获取该收费数据可用金额
        /// </summary>
        /// <param name="feeid"></param>
        /// <returns></returns>
        public static decimal GetUsableMoney(string feeId)
        {
            string cmdText = @"SELECT  ISNULL(f.PaidMoney, 0) - ( SELECT   ISNULL(SUM(RefundMoney), 0)
                                   FROM     T_Pro_Refund r
                                   WHERE    r.Status = 1
                                            AND r.FeeID = f.FeeID
                                 ) - ( SELECT   ISNULL(SUM(o.Money), 0)
                                       FROM     T_Pro_Offset o
                                       WHERE    o.Status = 1
                                                AND o.ByFeeID = f.FeeID
                                     ) + ( SELECT   ISNULL(SUM(o.Money), 0)
                                           FROM     T_Pro_Offset o
                                           WHERE    o.Status = 1
                                                    AND o.FeeID = f.FeeID
                                         ) Money
FROM    T_Pro_Fee f
WHERE   f.FeeID = @FeeID";
            SqlParameter[] paras = new SqlParameter[] {
            new  SqlParameter("@FeeID",feeId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return Convert.ToDecimal(dt.Rows[0]["Money"].ToString());
        }

        public static DataTable SelectFeeByFeeID(string feeId)
        {
            string cmdText = @"SELECT  ( SELECT    CONVERT(NVARCHAR(10), ItemDetailID) + ','
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID = f.FeeID
          ORDER BY  ItemDetailID ASC
        FOR
          XML PATH('')
        ) ItemDetailID ,
        ( SELECT    ISNULL(SUM(fd.ShouldMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID = f.FeeID
        ) ShouldMoney ,
        ( SELECT    ISNULL(SUM(fd.PaidMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID = f.FeeID
        ) PaidMoney ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID = f.FeeID
        ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(fd.CanMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID = f.FeeID
        ) CanMoney ,
        ( SELECT    ISNULL(COUNT(o.OffsetID), 0)
          FROM      T_Pro_Offset o
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = o.FeeDetailID
          WHERE     o.Status = 1
                    AND fd.FeeID = f.FeeID
        ) OffsetNum ,
        ( SELECT    ISNULL(COUNT(o.OffsetID), 0)
          FROM      T_Pro_Offset o
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = o.ByFeeDetailID
          WHERE     o.Status = 1
                    AND fd.FeeID = f.FeeID
        ) ByOffsetNum,
        ( SELECT    ISNULL(COUNT(r.RefundID), 0)
          FROM      dbo.T_Pro_Refund  r
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = r.FeeDetailID
          WHERE     r.Status = 1
                    AND fd.FeeID = f.FeeID
        ) RefundNum,
		f.VoucherNum
FROM    T_Pro_Fee f where FeeID IN (" + feeId + ")";
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, null).Tables[0];
        }
        /// <summary>
        /// 获取批量打印内容
        /// </summary>
        /// <param name="feeIdString"></param>
        /// <returns></returns>
        public static DataTable GetPrintMoreContent(string feeIdString)
        {
            string cmdText = @"SELECT  DISTINCT
        d.Name SchoolName ,
        d1.Name DeptName ,
        pn.Name ProName ,
        c.Name ClassName ,
        i.Name ProveName ,
        ( SELECT  DISTINCT
                    dl.Name + ' ' + CONVERT(NVARCHAR(10), fd.ShouldMoney)
          FROM      T_Pro_FeeDetail fd
                    LEFT JOIN T_Pro_ItemDetail id ON id.ItemDetailID = fd.ItemDetailID
                    LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
          WHERE     fd.Status = 1
                    AND fd.FeeID IN ( {0} )
        FOR
          XML PATH('div')
        ) FeeContent ,
        ( SELECT    SUM(PaidMoney)
          FROM      T_Pro_Fee
          WHERE     FeeID IN ( {0} )
        ) ShouldMoney ,
        ( SELECT    s.Name + ','
          FROM      T_Pro_Student s
                    LEFT JOIN T_Pro_Prove p1 ON p1.StudentID = s.StudentID
                    LEFT JOIN T_Pro_Fee f1 ON f1.ProveID = p1.ProveID
          WHERE     f1.FeeID IN ( {0} )
        FOR
          XML PATH('')
        ) FeeUser ,
        u.Name Creater ,
        CONVERT(NVARCHAR(10),f.FeeTime,23)FeeTime  ,
		r1.RefeName FeeMode
FROM    T_Pro_Fee f
        LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
        LEFT JOIN T_Sys_Dept d1 ON d1.DeptID = d.ParentID
        LEFT JOIN T_Pro_Prove p ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_Class c ON c.ClassID = p.ClassID
        LEFT JOIN T_Pro_Profession pn ON pn.ProfessionID = c.ProfessionID
        LEFT JOIN T_Pro_Item i ON i.ItemID = p.ItemID
        LEFT JOIN T_Sys_User u ON u.UserID = f.CreateID
		LEFT JOIN T_Sys_Refe r1 ON r1.Value=f.FeeMode AND r1.RefeTypeID=6
WHERE   f.FeeID IN ( {0} )";

            cmdText = string.Format(cmdText, feeIdString);
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, null).Tables[0];
        }

        public static void UpdateFeePaidMoney(string feeId, decimal money, int userId)
        {
            FeeModel fm = FeeBLL.GetFeeModel(feeId);
            fm.PaidMoney = (Convert.ToDecimal(fm.PaidMoney) + money).ToString();
            fm.UpdateID = userId.ToString();
            fm.UpdateTime = DateTime.Now.ToString();
            FeeBLL.UpdateFee(fm);
        }
    }
}
