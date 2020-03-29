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
    public class iFeeBLL
    {
        public static int InsertiFee(iFeeModel fm)
        {
            string cmdText = @"INSERT INTO T_Inc_iFee
(Status
,DeptID
,DeptAreaID
,VoucherNum
,NoteNum
,StudentID
,FeeTime
,ItemDetailID
,PersonSort
,FeeMode
,ShouldMoney
,PaidMoney
,DiscountMoney
,CanMoney
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
,@DeptAreaID
,@VoucherNum
,@NoteNum
,@StudentID
,@FeeTime
,@ItemDetailID
,@PersonSort
,@FeeMode
,@ShouldMoney
,@PaidMoney
,@DiscountMoney
,@CanMoney
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
new SqlParameter("@DeptAreaID", fm.DeptAreaID),
new SqlParameter("@VoucherNum", fm.VoucherNum),
new SqlParameter("@NoteNum", fm.NoteNum),
new SqlParameter("@StudentID", fm.StudentID),
new SqlParameter("@FeeTime", fm.FeeTime),
new SqlParameter("@ItemDetailID", fm.ItemDetailID),
new SqlParameter("@PersonSort", fm.PersonSort),
new SqlParameter("@FeeMode", fm.FeeMode),
new SqlParameter("@ShouldMoney", fm.ShouldMoney),
new SqlParameter("@PaidMoney", fm.PaidMoney),
new SqlParameter("@DiscountMoney", fm.DiscountMoney),
new SqlParameter("@CanMoney", fm.CanMoney),
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

        public static int UpdateiFee(iFeeModel fm)
        {
            string cmdText = @"UPDATE T_Inc_iFee SET
Status=@Status
,DeptID=@DeptID
,DeptAreaID=@DeptAreaID
,VoucherNum=@VoucherNum
,NoteNum=@NoteNum
,StudentID=@StudentID
,FeeTime=@FeeTime
,ItemDetailID=@ItemDetailID
,PersonSort=@PersonSort
,FeeMode=@FeeMode
,ShouldMoney=@ShouldMoney
,PaidMoney=@PaidMoney
,DiscountMoney=@DiscountMoney
,CanMoney=@CanMoney
,PrintNum=@PrintNum
,AffirmID=@AffirmID
,AffirmTime=@AffirmTime
,Explain=@Explain
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE iFeeID=@iFeeID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@iFeeID", fm.iFeeID),
new SqlParameter("@Status", fm.Status),
new SqlParameter("@DeptID", fm.DeptID),
new SqlParameter("@DeptAreaID", fm.DeptAreaID),
new SqlParameter("@VoucherNum", fm.VoucherNum),
new SqlParameter("@NoteNum", fm.NoteNum),
new SqlParameter("@StudentID", fm.StudentID),
new SqlParameter("@FeeTime", fm.FeeTime),
new SqlParameter("@ItemDetailID", fm.ItemDetailID),
new SqlParameter("@PersonSort", fm.PersonSort),
new SqlParameter("@FeeMode", fm.FeeMode),
new SqlParameter("@ShouldMoney", fm.ShouldMoney),
new SqlParameter("@PaidMoney", fm.PaidMoney),
new SqlParameter("@DiscountMoney", fm.DiscountMoney),
new SqlParameter("@CanMoney", fm.CanMoney),
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
                return Convert.ToInt32(fm.iFeeID);
            }
            else
            {
                return -1;
            }
        }

        public static iFeeModel iFeeModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            iFeeModel fm = new iFeeModel();
            string cmdText = "SELECT * FROM T_Inc_iFee WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                fm.iFeeID = dt.Rows[0]["iFeeID"].ToString();
                fm.Status = dt.Rows[0]["Status"].ToString();
                fm.DeptID = dt.Rows[0]["DeptID"].ToString();
                fm.DeptAreaID = dt.Rows[0]["DeptAreaID"].ToString();
                fm.VoucherNum = dt.Rows[0]["VoucherNum"].ToString();
                fm.NoteNum = dt.Rows[0]["NoteNum"].ToString();
                fm.StudentID = dt.Rows[0]["StudentID"].ToString();
                fm.FeeTime = dt.Rows[0]["FeeTime"].ToString();
                fm.ItemDetailID = dt.Rows[0]["ItemDetailID"].ToString();
                fm.PersonSort = dt.Rows[0]["PersonSort"].ToString();
                fm.FeeMode = dt.Rows[0]["FeeMode"].ToString();
                fm.ShouldMoney = dt.Rows[0]["ShouldMoney"].ToString();
                fm.PaidMoney = dt.Rows[0]["PaidMoney"].ToString();
                fm.DiscountMoney = dt.Rows[0]["DiscountMoney"].ToString();
                fm.CanMoney = dt.Rows[0]["CanMoney"].ToString();
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

        public static DataTable iFeeTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Inc_iFee WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static iFeeModel GetiFeeModel(string iFeeId)
        {
            string where = " AND iFeeID = @iFeeID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@iFeeID",iFeeId)
            };
            iFeeModel ifm = iFeeBLL.iFeeModelByWhere(where, paras);
            return ifm;
        }

        /// <summary>
        /// 输出打印信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static DataTable iFeeTablePrintByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  f.iFeeID ,
        d2.Name ParentDeptName ,
        f.VoucherNum ,
        d1.Name DeptName ,
        s.Name StudentName ,
        s.IDCard ,
        r1.RefeName FeeMode ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        d.Name ItemName ,
        f.PaidMoney - ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
                          FROM      T_Inc_iRefund r
                          WHERE     r.Status = 1
                                    AND r.Sort IN ( 1, 2 )
                                    AND r.iFeeID = f.iFeeID
                        ) - ( SELECT    ISNULL(SUM(o.Money), 0)
                              FROM      T_Inc_iOffset o
                              WHERE     o.Status = 1
                                        AND o.ByiFeeID = f.iFeeID
                            ) +( SELECT    ISNULL(SUM(o.Money), 0)
                              FROM      T_Inc_iOffset o
                              WHERE     o.Status = 1
                                        AND o.iFeeID = f.iFeeID
                            ) ShouldMoney ,
        CASE f.PaidMoney
          WHEN 0 THEN ''
          ELSE '实收' + CONVERT(NVARCHAR(10), f.PaidMoney) + ' '
        END Remark1 ,
        CASE f.DiscountMoney
          WHEN 0 THEN ''
          ELSE '优惠' + CONVERT(NVARCHAR(10), f.DiscountMoney) + ' '
        END Remark2 ,
        CASE ( SELECT   ISNULL(SUM(o.Money), 0)
               FROM     T_Inc_iOffset o
               WHERE    o.Status = 1
                        AND o.iFeeID = f.iFeeID
             )
          WHEN 0 THEN ''
          ELSE '充抵'
               + CONVERT(NVARCHAR(10), ( SELECT ISNULL(SUM(o.Money), 0)
                                         FROM   T_Inc_iOffset o
                                         WHERE  o.Status = 1
                                                AND o.iFeeID = f.iFeeID
                                       )) + ' '
        END Remark3 ,
        CASE ( SELECT   ISNULL(SUM(r.RefundMoney), 0)
               FROM     T_Inc_iRefund r
               WHERE    r.Status = 1
                        AND r.Sort IN ( 1, 2 )
                        AND r.iFeeID = f.iFeeID
             )
          WHEN 0 THEN ''
          ELSE '核销'
               + CONVERT(NVARCHAR(10), ( SELECT ISNULL(SUM(r.RefundMoney), 0)
                                         FROM   T_Inc_iRefund r
                                         WHERE  r.Status = 1
                                                AND r.Sort IN ( 1, 2 )
                                                AND r.iFeeID = f.iFeeID
                                       )) + ' '
        END Remark4 ,
        CASE ( SELECT   ISNULL(SUM(o.Money), 0)
               FROM     T_Inc_iOffset o
               WHERE    o.Status = 1
                        AND o.ByiFeeID = f.iFeeID
             )
          WHEN 0 THEN ''
          ELSE '被充抵'
               + CONVERT(NVARCHAR(10), ( SELECT ISNULL(SUM(o.Money), 0)
                                         FROM   T_Inc_iOffset o
                                         WHERE  o.Status = 1
                                                AND o.ByiFeeID = f.iFeeID
                                       )) + ' '
        END Remark5 ,
        f.Explain Remark ,
        u.Name Feeer
FROM    T_Inc_iFee f
        LEFT JOIN T_Sys_Dept d1 ON f.DeptID = d1.DeptID
        LEFT JOIN T_Sys_Dept d2 ON d1.ParentID = d2.DeptID
        LEFT JOIN T_Pro_ItemDetail id ON f.ItemDetailID = id.ItemDetailID
        LEFT JOIN T_Pro_Detail d ON id.DetailID = d.DetailID
        LEFT JOIN T_Pro_Student s ON f.StudentID = s.StudentID
        LEFT JOIN T_Sys_Refe r1 ON f.FeeMode = r1.Value
                                   AND r1.RefeTypeID = 6
        LEFT JOIN T_Sys_User u ON f.CreateID = u.UserID
WHERE   1 = 1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static DataTable SelectiFee(string ifeeId)
        {
            string cmdText = @"SELECT  ifee.iFeeID ,
        dept.Name AS Dept ,
        ifee.DeptID ,
        ifee.DeptAreaID ,
        student.Name ,
        ifee.StudentID ,
        ifee.PersonSort ,
        CONVERT(NVARCHAR(23), ifee.FeeTime, 23) AS FeeTime ,
        ifee.ItemDetailID ,
        ifee.FeeMode ,
        ifee.ShouldMoney ,
        ifee.PaidMoney ,
        ifee.DiscountMoney ,
        ( SELECT  ISNULL(SUM(Money), 0)
FROM    T_Inc_iOffset
WHERE   iFeeID = ifee.iFeeID
        AND Status = 1
        ) AS OffsetMoney ,
        ifee.Explain ,
        ifee.Remark ,
        itemdetail.Sort
FROM    T_Inc_iFee AS ifee
        LEFT JOIN T_Pro_Student AS student ON ifee.StudentID = student.StudentID
        LEFT JOIN T_Sys_Dept AS dept ON ifee.DeptID = dept.DeptID
        LEFT JOIN T_Pro_ItemDetail AS itemdetail ON ifee.ItemDetailID=itemdetail.ItemDetailID
WHERE   ifee.iFeeID = @ifeeId";
            SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@ifeeId", ifeeId) };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        /// <summary>
        /// 修改杂费的可用金额
        /// </summary>
        /// <param name="money"></param>
        /// <param name="ifeeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int UpdateiFeeCanMoney(decimal money, string ifeeId, int userId)
        {
            string cmdText = @"UPDATE T_Inc_iFee SET
 CanMoney=CanMoney-@CanMoney
,UpdateID=@UpdateID
,UpdateTime=getDate()
WHERE iFeeID=@iFeeID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@iFeeID",ifeeId),
new SqlParameter("@CanMoney", money),
new SqlParameter("@UpdateID", userId)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
                return result;
            else
                return -1;
        }
        /// <summary>
        /// 查询杂费收费用于合并打印验证
        /// </summary>
        /// <param name="iFeeString"></param>
        /// <returns></returns>
        public static DataTable SelectiFeeByiFeeId(string iFeeString)
        {
            string cmdText = @"SELECT  f.iFeeID ,
        f.DeptID ,
        f.VoucherNum ,
        f.FeeTime ,
        f.FeeMode ,
        f.ItemDetailID ,
        f.ShouldMoney ShouldMoney ,
        f.PaidMoney ,
        f.DiscountMoney ,
        f.PrintNum,
        f.CanMoney ,
        ( SELECT    COUNT(o.iOffsetID)
          FROM       T_Inc_iOffset o
          WHERE     o.Status = 1
                    AND o.iFeeID = f.iFeeID
        )OffsetNum,
		(SELECT    COUNT(o.iOffsetID)
          FROM       T_Inc_iOffset o
          WHERE     o.Status = 1
                    AND o.ByiFeeID = f.iFeeID)
					ByOffsetNum,
      (SELECT COUNT(iRefundID) FROM T_Inc_iRefund WHERE iFeeID=f.iFeeID AND Status=1) RefundNum
FROM    T_Inc_iFee f 
        Where f.iFeeID IN (" + iFeeString + ")";
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, null).Tables[0];
        }
        /// <summary>
        /// 获取合并打印杂费的内容
        /// </summary>
        /// <param name="iFeeString"></param>
        /// <returns></returns>
        public static DataTable GetiFeePrintContent(string iFeeString) {

            string cmdText = @"SELECT DISTINCT
        d1.Name DeptName ,
        d.Name SchoolName ,
        dl.Name + ' ' + CONVERT(NVARCHAR(10), f.PaidMoney) FeeContent ,
        ( SELECT    SUM(i.PaidMoney)
          FROM       T_Inc_iFee i
          WHERE     i.iFeeID IN ( {0} )
        ) ShouldMoney ,
        ( SELECT    s.Name + ','
          FROM       T_Pro_Student s
          WHERE     s.StudentID IN ( SELECT i.StudentID
                                     FROM    T_Inc_iFee i
                                     WHERE  i.iFeeID IN ( {0}) )
        FOR
          XML PATH('')
        ) FeeUser ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime,
		r1.RefeName FeeMode
		,u.Name Creater
FROM     T_Inc_iFee f
        LEFT JOIN  T_Sys_Dept d ON d.DeptID = f.DeptID
        LEFT JOIN  T_Sys_Dept d1 ON d1.DeptID = d.ParentID
        LEFT JOIN  T_Pro_ItemDetail id ON id.ItemDetailID = f.ItemDetailID
        LEFT JOIN  T_Pro_Detail dl ON dl.DetailID = id.DetailID
		LEFT JOIN  T_Sys_Refe r1 ON r1.Value=f.FeeMode AND r1.RefeTypeID=6
		LEFT JOIN T_Sys_User  u ON u.UserID=f.CreateID 
    Where f.iFeeID IN ({0})
";

            cmdText = string.Format(cmdText, iFeeString);
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, null).Tables[0];
        }

        public static DataTable GetiFeePrintByStudentContent(string iFeeString)
        {

            string cmdText = @"SELECT DISTINCT
        d1.Name DeptName ,
        d.Name SchoolName ,
        ( SELECT    dl.Name + ' ' + CONVERT(NVARCHAR(10), ifee.PaidMoney)
          FROM      dbo.T_Inc_iFee ifee
                    LEFT JOIN T_Pro_ItemDetail id ON id.ItemDetailID = ifee.ItemDetailID
                    LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
          WHERE     ifee.iFeeID IN ( {0} )
        FOR
          XML PATH('div')
        ) FeeContent ,
		(SELECT SUM(PaidMoney) FROM dbo.T_Inc_iFee WHERE iFeeID IN ({0})) ShouldMoney,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        u.Name Creater,
		s.Name FeeUser,
        s.IDCard
FROM    T_Inc_iFee f
        LEFT JOIN dbo.T_Pro_Student s ON s.StudentID = f.StudentID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
        LEFT JOIN T_Sys_Dept d1 ON d1.DeptID = d.ParentID
        LEFT JOIN T_Sys_User u ON u.UserID = f.CreateID
WHERE   f.iFeeID IN ( {0} )
";

            cmdText = string.Format(cmdText, iFeeString);
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, null).Tables[0];
        }

    }
}
