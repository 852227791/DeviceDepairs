using Common;
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
    public class FeeDetailBLL
    {
        public static int InsertFeeDetail(FeeDetailModel fdm)
        {
            string cmdText = @"INSERT INTO T_Pro_FeeDetail
(Status
,FeeID
,ItemDetailID
,ShouldMoney
,PaidMoney
,DiscountMoney
,CanMoney
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@FeeID
,@ItemDetailID
,@ShouldMoney
,@PaidMoney
,@DiscountMoney
,@CanMoney
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", fdm.Status),
new SqlParameter("@FeeID", fdm.FeeID),
new SqlParameter("@ItemDetailID", fdm.ItemDetailID),
new SqlParameter("@ShouldMoney", fdm.ShouldMoney),
new SqlParameter("@PaidMoney", fdm.PaidMoney),
new SqlParameter("@DiscountMoney", fdm.DiscountMoney),
new SqlParameter("@CanMoney", fdm.CanMoney),
new SqlParameter("@CreateID", fdm.CreateID),
new SqlParameter("@CreateTime", fdm.CreateTime),
new SqlParameter("@UpdateID", fdm.UpdateID),
new SqlParameter("@UpdateTime", fdm.UpdateTime)
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

        public static int UpdateFeeDetail(FeeDetailModel fdm)
        {
            string cmdText = @"UPDATE T_Pro_FeeDetail SET
Status=@Status
,FeeID=@FeeID
,ItemDetailID=@ItemDetailID
,ShouldMoney=@ShouldMoney
,PaidMoney=@PaidMoney
,DiscountMoney=@DiscountMoney
,CanMoney=@CanMoney
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE FeeDetailID=@FeeDetailID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@FeeDetailID", fdm.FeeDetailID),
new SqlParameter("@Status", fdm.Status),
new SqlParameter("@FeeID", fdm.FeeID),
new SqlParameter("@ItemDetailID", fdm.ItemDetailID),
new SqlParameter("@ShouldMoney", fdm.ShouldMoney),
new SqlParameter("@PaidMoney", fdm.PaidMoney),
new SqlParameter("@DiscountMoney", fdm.DiscountMoney),
new SqlParameter("@CanMoney", fdm.CanMoney),
new SqlParameter("@CreateID", fdm.CreateID),
new SqlParameter("@CreateTime", fdm.CreateTime),
new SqlParameter("@UpdateID", fdm.UpdateID),
new SqlParameter("@UpdateTime", fdm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(fdm.FeeDetailID);
            }
            else
            {
                return -1;
            }
        }

        public static FeeDetailModel FeeDetailModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            FeeDetailModel fdm = new FeeDetailModel();
            string cmdText = "SELECT * FROM T_Pro_FeeDetail WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                fdm.FeeDetailID = dt.Rows[0]["FeeDetailID"].ToString();
                fdm.Status = dt.Rows[0]["Status"].ToString();
                fdm.FeeID = dt.Rows[0]["FeeID"].ToString();
                fdm.ItemDetailID = dt.Rows[0]["ItemDetailID"].ToString();
                fdm.ShouldMoney = dt.Rows[0]["ShouldMoney"].ToString();
                fdm.PaidMoney = dt.Rows[0]["PaidMoney"].ToString();
                fdm.DiscountMoney = dt.Rows[0]["DiscountMoney"].ToString();
                fdm.CanMoney = dt.Rows[0]["CanMoney"].ToString();
                fdm.CreateID = dt.Rows[0]["CreateID"].ToString();
                fdm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                fdm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                fdm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return fdm;
        }

        public static DataTable FeeDetailTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_FeeDetail WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        public static int UpdateFeeDetailStatus(string feeId)
        {
            string cmdText = @"UPDATE T_Pro_FeeDetail SET
Status=@Status
WHERE FeeID=@FeeID";
            SqlParameter[] paras = new SqlParameter[] {
             new SqlParameter("@Status", "2"),
             new SqlParameter("@FeeID",feeId)
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

        public static int UpdateFeeDetailStatus(SqlParameter[] paras)
        {
            string cmdText = @"UPDATE T_Pro_FeeDetail SET
Status=@Status
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE  FeeID=@FeeID";
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

        public static DataTable FeeDetailFeeTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  ItemDetailID,f.ProveID
FROM    T_Pro_FeeDetail fd
        LEFT JOIN T_Pro_Fee f ON fd.FeeID = f.FeeID
WHERE   1 = 1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获取收费明细对象
        /// </summary>
        /// <param name="feeDetailId"></param>
        /// <returns></returns>
        public static FeeDetailModel GetFeeDetailModel(string feeDetailId)
        {
            string where = " and FeeDetailID=@FeeDetailID";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@FeeDetailID",feeDetailId)
            };
            return FeeDetailModelByWhere(where, paras);
        }
        /// <summary>
        /// 根据收费feeid生成收费明细Commbobox
        /// </summary>
        /// <param name="feeId"></param>
        /// <returns></returns>
        public static string GetFeeDeailCommbox(string feeId)
        {
            string cmdText = @"SELECT  dl.Name + ' ' + CONVERT(NVARCHAR(18), fd.Money) text ,
        fd.FeeDetailID value
FROM    T_Pro_FeeDetail fd
        LEFT JOIN T_Pro_ItemDetail id ON fd.ItemDetailID = id.ItemDetailID
        LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
        where fd.FeeID=@FeeID";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("FeeID",feeId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return JsonHelper.DataTableToJson(dt);
        }
        /// <summary>
        /// 获取收费明细的可用金额
        /// </summary>
        /// <param name="feedetailId"></param>
        /// <returns></returns>
        public static decimal GetuseableMoney(string feedetailId)
        {
            string cmdText = @"SELECT  CanMoney
            FROM    T_Pro_FeeDetail
            where FeeDetailID=@FeeDetailID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("FeeDetailID",feedetailId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
                return decimal.Parse(dt.Rows[0]["CanMoney"].ToString());
            else
                return 0;

        }
        /// <summary>
        /// 获取收费信息下所有可用金额的总和
        /// </summary>
        /// <param name="feeid"></param>
        /// <returns></returns>
        public static decimal GetuseableMoneyByFeeID(string feeid)
        {
            string cmdText = @"SELECT  isnull(SUM(CanMoney),0) CanMoney
FROM    T_Pro_FeeDetail
WHERE   FeeID = @FeeID
        AND Status = 1";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("FeeID",feeid)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return decimal.Parse(dt.Rows[0]["CanMoney"].ToString());
        }
        /// <summary>
        /// 获取收费明细的应交金额
        /// </summary>
        /// <param name="feedetailId"></param>
        /// <returns></returns>
        public static decimal GetshouldMoney(string feedetailId)
        {
            string cmdText = @"SELECT  ShouldMoney
            FROM    T_Pro_FeeDetail
            where FeeDetailID=@FeeDetailID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("FeeDetailID",feedetailId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return decimal.Parse(dt.Rows[0]["Money"].ToString());
        }

        /// <summary>
        /// 修改可用金额
        /// </summary>
        /// <param name="money"></param>
        /// <param name="userId"></param>
        /// <param name="feedetailId"></param>
        /// <returns></returns>
        public static bool UpdateFeeDetailCanMoney(decimal money, int userId, string feedetailId)
        {
            string cmdText = @"UPDATE T_Pro_FeeDetail SET
CanMoney=CanMoney-@CanMoney
,UpdateID=@UpdateID
,UpdateTime=GETDATE()
WHERE FeeDetailID=@FeeDetailID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@CanMoney", money),
                new SqlParameter("@UpdateID", userId),
                new SqlParameter("FeeDetailID",feedetailId)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 修改可用金额（重新读取所有相关数据）
        /// </summary>
        /// <param name="feeDetailId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool UpdateFeeDetailCanMoney(string feeDetailId, int userId)
        {
            decimal canMoney = GetCanMoney(feeDetailId, "0");
            string cmdText = @"UPDATE  T_Pro_FeeDetail
SET     CanMoney = @CanMoney ,
        UpdateID = @UpdateID ,
        UpdateTime = GETDATE()
WHERE   FeeDetailID = @FeeDetailID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@CanMoney", canMoney),
                new SqlParameter("@UpdateID", userId),
                new SqlParameter("FeeDetailID",feeDetailId)
            };
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 根据收费明细id计算可用金额
        /// </summary>
        /// <param name="feeDetailId"></param>
        /// <returns></returns>
        public static decimal GetCanMoney(string feeDetailId, string refundId)
        {
            string cmdText = @"SELECT  ISNULL(fd.PaidMoney, 0) - ( SELECT  ISNULL(SUM(RefundMoney), 0)
                                    FROM    T_Pro_Refund r
                                    WHERE   r.Status = 1
                                            AND fd.FeeDetailID = r.FeeDetailID
                                            AND r.RefundID <> @RefundID
                                  )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.Status = 1
                    AND o.ByFeeDetailID = fd.FeeDetailID
          ) + ( SELECT  ISNULL(SUM(o.Money), 0)
                FROM    T_Pro_Offset o
                WHERE   o.Status = 1
                        AND o.FeeDetailID = fd.FeeDetailID
              ) Money
FROM    T_Pro_FeeDetail fd
WHERE   fd.FeeDetailID =  @FeeDetailID
        "; SqlParameter[] paras = new SqlParameter[] {
            new  SqlParameter("@FeeDetailID", feeDetailId),
            new  SqlParameter("@RefundID", refundId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            string Money = "0";
            if (dt.Rows.Count > 0)
            {
                Money = dt.Rows[0]["Money"].ToString();
            }
            return Convert.ToDecimal(Money);
        }

        /// <summary>
        /// 根据收费项ID、收费类别名称验证是否存在
        /// </summary>
        /// <param name="feeId"></param>
        /// <param name="detailName"></param>
        /// <returns></returns>
        public static string GetFeeModelByItemNum(string feeId, string detailName)
        {
            string cmdText = @"SELECT  fd.FeeDetailID
FROM    T_Pro_FeeDetail fd
        LEFT JOIN T_Pro_ItemDetail id ON fd.ItemDetailID = id.ItemDetailID
        LEFT JOIN T_Pro_Detail d ON id.DetailID = d.DetailID
WHERE   fd.Status = 1
        AND fd.FeeID = @FeeID
        AND d.Name = @DetailName";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FeeID", feeId),
                new SqlParameter("@DetailName", detailName)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            string feeDetailID = "0";
            if (dt.Rows.Count > 0)
            {
                feeDetailID = dt.Rows[0]["FeeDetailID"].ToString();
            }
            return feeDetailID;
        }
        /// <summary>
        /// 根据feeid返回收费项目明细
        /// </summary>
        /// <param name="feeId"></param>
        /// <returns></returns>
        public static DataTable FeeDetailTableByWhere(string feeId) {
            string where = " and FeeID=@FeeID and Status=1 ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FeeID",feeId)
            };
            return FeeDetailBLL.FeeDetailTableByWhere(where,paras,"");
        }
    }
}
