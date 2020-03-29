using System;
using Model;
using Common;
using DAL;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{

    public class sRefundDiscountBLL
    {
        public static int InsertsRefundDiscount(sRefundDiscountModel rdm)
        {
            string cmdText = @"INSERT INTO T_Stu_sRefundDiscount
			(Status
			,sFeesOrderID
			,Sort
			,RefundMoney
			,RefundTime
			,PayObject
			,Remark
			,CreateID
			,CreateTime
			,UpdateID
			,UpdateTime
						)
			VALUES(@Status
			,@sFeesOrderID
			,@Sort
			,@RefundMoney
			,@RefundTime
			,@PayObject
			,@Remark
			,@CreateID
			,@CreateTime
			,@UpdateID
			,@UpdateTime
						);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Status", rdm.Status)
                ,new SqlParameter("@sFeesOrderID", rdm.sFeesOrderID)
                ,new SqlParameter("@Sort", rdm.Sort)
                ,new SqlParameter("@RefundMoney", rdm.RefundMoney)
                ,new SqlParameter("@RefundTime", rdm.RefundTime)
                ,new SqlParameter("@PayObject", rdm.PayObject)
                ,new SqlParameter("@Remark", rdm.Remark)
                ,new SqlParameter("@CreateID", rdm.CreateID)
                ,new SqlParameter("@CreateTime", rdm.CreateTime)
                ,new SqlParameter("@UpdateID", rdm.UpdateID)
                ,new SqlParameter("@UpdateTime", rdm.UpdateTime)
                            };
            return Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
        }

        public static int UpdatesRefundDiscount(sRefundDiscountModel rdm)
        {
            string cmdText = @"UPDATE T_Stu_sRefundDiscount SET  Status=@Status
			,sFeesOrderID=@sFeesOrderID
			,Sort=@Sort
			,RefundMoney=@RefundMoney
			,RefundTime=@RefundTime
			,PayObject=@PayObject
			,Remark=@Remark
			,CreateID=@CreateID
			,CreateTime=@CreateTime
			,UpdateID=@UpdateID
			,UpdateTime=@UpdateTime
			Where sRefundDiscountID=@sRefundDiscountID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sRefundDiscountID", rdm.sRefundDiscountID)
                ,new SqlParameter("@Status", rdm.Status)
                ,new SqlParameter("@sFeesOrderID", rdm.sFeesOrderID)
                ,new SqlParameter("@Sort", rdm.Sort)
                ,new SqlParameter("@RefundMoney", rdm.RefundMoney)
                ,new SqlParameter("@RefundTime", rdm.RefundTime)
                ,new SqlParameter("@PayObject", rdm.PayObject)
                ,new SqlParameter("@Remark", rdm.Remark)
                ,new SqlParameter("@CreateID", rdm.CreateID)
                ,new SqlParameter("@CreateTime", rdm.CreateTime)
                ,new SqlParameter("@UpdateID", rdm.UpdateID)
                ,new SqlParameter("@UpdateTime", rdm.UpdateTime)
                };
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        public static List<sRefundDiscountModel> SelectsRefundDiscountByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            List<sRefundDiscountModel> list = new List<sRefundDiscountModel>();
            string cmdText = "SELECT * FROM T_Stu_sRefundDiscount WHERE 1 = 1 {0}{1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                sRefundDiscountModel rdm = new sRefundDiscountModel();
                rdm.sRefundDiscountID = dr["sRefundDiscountID"].ToString();
                rdm.Status = dr["Status"].ToString();
                rdm.sFeesOrderID = dr["sFeesOrderID"].ToString();
                rdm.Sort = dr["Sort"].ToString();
                rdm.RefundMoney = dr["RefundMoney"].ToString();
                rdm.RefundTime = dr["RefundTime"].ToString();
                rdm.PayObject = dr["PayObject"].ToString();
                rdm.Remark = dr["Remark"].ToString();
                rdm.CreateID = dr["CreateID"].ToString();
                rdm.CreateTime = dr["CreateTime"].ToString();
                rdm.UpdateID = dr["UpdateID"].ToString();
                rdm.UpdateTime = dr["UpdateTime"].ToString();
                list.Add(rdm);
            }
            return list;
        }

        public static sRefundDiscountModel GetsRefundDiscountModel(string sRefundDiscountID)
        {
            string where = " and sRefundDiscountID=@sRefundDiscountID";
            SqlParameter[] paras = new SqlParameter[] {
             new SqlParameter("@sRefundDiscountID", sRefundDiscountID)
             };
            return sRefundDiscountBLL.SelectsRefundDiscountByWhere(where, paras, "").FirstOrDefault();
        }

        public static bool UpdatesRefundDiscountStatus(string sRefundDiscountID, string status, string userId)
        {
            var list = sRefundDiscountBLL.GetsRefundDiscountModel(sRefundDiscountID);
            if (list != null)
            {
                list.Status = status;
                list.UpdateTime = DateTime.Now.ToString();
                list.UpdateID = userId;
                if (sRefundDiscountBLL.UpdatesRefundDiscount(list).Equals(1))
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取核销金额
        /// </summary>
        /// <param name="sFeeOrderId"></param>
        /// <returns></returns>
        public static decimal SelectsCanMoney(string sFeeOrderId)
        {
            string cmdText = @"SELECT  DiscountMoney - ( SELECT    ISNULL(SUM(RefundMoney), 0)
                          FROM dbo.T_Stu_sRefundDiscount
                     WHERE     Status = 1
                                    AND sFeesOrderID = fo.sFeesOrderID
                        )RefundMoney
FROM dbo.T_Stu_sFeesOrder fo
WHERE fo.sFeesOrderID = @sFeesOrderID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeesOrderID",sFeeOrderId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count>0)
            {
                return decimal.Parse(dt.Rows[0]["RefundMoney"].ToString());
            }
            return 0;
        }
    }
}
