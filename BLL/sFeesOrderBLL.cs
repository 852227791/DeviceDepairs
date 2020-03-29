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
    public class sFeesOrderBLL
    {
        public static int InsertsFeesOrder(sFeesOrderModel fom)
        {
            string cmdText = @"INSERT INTO T_Stu_sFeesOrder
(Status
,sFeeID
,sOrderID
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
,@sFeeID
,@sOrderID
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
new SqlParameter("@Status", fom.Status),
new SqlParameter("@sFeeID", fom.sFeeID),
new SqlParameter("@sOrderID", fom.sOrderID),
new SqlParameter("@ShouldMoney", fom.ShouldMoney),
new SqlParameter("@PaidMoney", fom.PaidMoney),
new SqlParameter("@DiscountMoney", fom.DiscountMoney),
new SqlParameter("@CanMoney", fom.CanMoney),
new SqlParameter("@CreateID", fom.CreateID),
new SqlParameter("@CreateTime", fom.CreateTime),
new SqlParameter("@UpdateID", fom.UpdateID),
new SqlParameter("@UpdateTime", fom.UpdateTime)
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

        public static int UpdatesFeesOrder(sFeesOrderModel fom)
        {
            string cmdText = @"UPDATE T_Stu_sFeesOrder SET
Status=@Status
,sFeeID=@sFeeID
,sOrderID=@sOrderID
,ShouldMoney=@ShouldMoney
,PaidMoney=@PaidMoney
,DiscountMoney=@DiscountMoney
,CanMoney=@CanMoney
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sFeesOrderID=@sFeesOrderID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sFeesOrderID", fom.sFeesOrderID),
new SqlParameter("@Status", fom.Status),
new SqlParameter("@sFeeID", fom.sFeeID),
new SqlParameter("@sOrderID", fom.sOrderID),
new SqlParameter("@ShouldMoney", fom.ShouldMoney),
new SqlParameter("@PaidMoney", fom.PaidMoney),
new SqlParameter("@DiscountMoney", fom.DiscountMoney),
new SqlParameter("@CanMoney", fom.CanMoney),
new SqlParameter("@CreateID", fom.CreateID),
new SqlParameter("@CreateTime", fom.CreateTime),
new SqlParameter("@UpdateID", fom.UpdateID),
new SqlParameter("@UpdateTime", fom.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(fom.sFeesOrderID);
            }
            else
            {
                return -1;
            }
        }

        public static sFeesOrderModel sFeesOrderModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sFeesOrderModel fom = new sFeesOrderModel();
            string cmdText = "SELECT * FROM T_Stu_sFeesOrder WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                fom.sFeesOrderID = dt.Rows[0]["sFeesOrderID"].ToString();
                fom.Status = dt.Rows[0]["Status"].ToString();
                fom.sFeeID = dt.Rows[0]["sFeeID"].ToString();
                fom.sOrderID = dt.Rows[0]["sOrderID"].ToString();
                fom.ShouldMoney = dt.Rows[0]["ShouldMoney"].ToString();
                fom.PaidMoney = dt.Rows[0]["PaidMoney"].ToString();
                fom.DiscountMoney = dt.Rows[0]["DiscountMoney"].ToString();
                fom.CanMoney = dt.Rows[0]["CanMoney"].ToString();
                fom.CreateID = dt.Rows[0]["CreateID"].ToString();
                fom.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                fom.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                fom.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return fom;
        }

        public static DataTable sFeesOrderTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sFeesOrder WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 修改学费的可用金额
        /// </summary>
        /// <param name="money"></param>
        /// <param name="sfeesOrderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int UpdatasFeesOrderCanMoney(decimal money, string sfeesOrderId, int userId)
        {
            string cmdText = @"UPDATE T_Stu_sFeesOrder SET
 CanMoney=CanMoney-@CanMoney
,UpdateID=@UpdateID
,UpdateTime=getDate()
WHERE sFeesOrderID=@sFeesOrderID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sFeesOrderID", sfeesOrderId),
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
        /// 根据收费信息id和收费项名称返回收费明细id
        /// </summary>
        /// <param name="sfeeId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetsFeesOrderID(string sfeeId, string name)
        {
            string cmdText = @"SELECT  fo.sFeesOrderID
FROM    T_Stu_sFeesOrder fo
        LEFT JOIN T_Stu_sOrder o ON fo.sOrderID = o.sOrderID
        LEFT JOIN T_Pro_Detail d ON d.DetailID = o.DetailID
        Where fo.Status=1 and fo.sFeeID=@sFeeID and d.Name=@Name";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeeID", sfeeId),
                new SqlParameter("@Name", name)
                };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["sFeesOrderID"].ToString();
            return string.Empty;
        }
        /// <summary>
        /// 获取收费明细的可用金额
        /// </summary>
        /// <param name="sfeesOrderId"></param>
        /// <returns></returns>
        public static decimal GetsFeesOrderCanMoney(string sfeesOrderId)
        {
            string where = " and sFeesOrderID=@sFeesOrderID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeesOrderID", sfeesOrderId)
                };
            return decimal.Parse(sFeesOrderBLL.sFeesOrderModelByWhere(where, paras).CanMoney);
        }
        /// <summary>
        /// 获取收费项的订单编号
        /// </summary>
        /// <param name="sfeesOrderId">收费项ID</param>
        /// <returns></returns>
        public static sFeesOrderModel GetsFeeOrderModel(string sfeesOrderId)
        {
            string where = " and sFeesOrderID=@sFeesOrderID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeesOrderID", sfeesOrderId)
                };
            return sFeesOrderBLL.sFeesOrderModelByWhere(where, paras);
        }
        /// <summary>
        /// 根据收费id返回收费明细
        /// </summary>
        /// <param name="sfeeId"></param>
        /// <returns></returns>
        public static DataTable GetsFeeOrderTable(string sfeeId)
        {
            string where = " and sFeeID=@sFeeID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeeID", sfeeId)
                };
            return sFeesOrderBLL.sFeesOrderTableByWhere(where, paras, "");
        }
        /// <summary>
        /// 删除收费明细
        /// </summary>
        /// <param name="sfeesOrderId"></param>
        public static void DeletesFeesOrder(string sfeesOrderId)
        {
            string cmdText = "DELETE T_Stu_sFeesOrder WHERE sFeesOrderID=@sFeesOrderID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeesOrderID",sfeesOrderId)
            };
            DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras);
        }

        public static List<sFeesOrderModel> SelectsFeesOrderByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            List<sFeesOrderModel> list = new List<sFeesOrderModel>();
            string cmdText = "SELECT * FROM T_Stu_sFeesOrder WHERE 1 = 1 {0}{1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                sFeesOrderModel fom = new sFeesOrderModel();
                fom.sFeesOrderID = dr["sFeesOrderID"].ToString();
                fom.Status = dr["Status"].ToString();
                fom.sFeeID = dr["sFeeID"].ToString();
                fom.sOrderID = dr["sOrderID"].ToString();
                fom.ShouldMoney = dr["ShouldMoney"].ToString();
                fom.PaidMoney = dr["PaidMoney"].ToString();
                fom.DiscountMoney = dr["DiscountMoney"].ToString();
                fom.CanMoney = dr["CanMoney"].ToString();
                fom.CreateID = dr["CreateID"].ToString();
                fom.CreateTime = dr["CreateTime"].ToString();
                fom.UpdateID = dr["UpdateID"].ToString();
                fom.UpdateTime = dr["UpdateTime"].ToString();
                list.Add(fom);
            }
            return list;
        }

        public static DataTable GetFeeOrderTable(string sorderId)
        {
            string cmdText = @"SELECT  fo.sFeesOrderID ID ,
        d.Name DeptName ,
        f.VoucherNum ,
        f.NoteNum ,
        s.Name StudName ,
        e.EnrollNum ,
        p.Name ProName ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        s.IDCard ,
        dl.Name FeeContent ,
        fo.CanMoney ,
        0.00 Money ,
        o.NumName ,
        '1' Sort
FROM    T_Stu_sFeesOrder fo
        LEFT JOIN T_Stu_sFee f ON f.sFeeID = fo.sFeeID
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = f.sEnrollsProfessionID
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
        LEFT JOIN dbo.T_Stu_sOrder o ON o.sOrderID = fo.sOrderID
        LEFT JOIN dbo.T_Pro_Detail dl ON dl.DetailID = o.DetailID
        LEFT JOIN T_Stu_sProfession sp ON ep.sProfessionID = sp.sProfessionID
        LEFT JOIN T_Pro_Profession p ON sp.ProfessionID = p.ProfessionID
WHERE   fo.Status = 1  and fo.sOrderID IN (" + sorderId + ")";

            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, null).Tables[0];
        }
    }
}
