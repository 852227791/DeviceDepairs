using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace BLL
{
    public class sOrderBLL
    {
        public static int InsertsOrder(sOrderModel om)
        {
            string cmdText = @"INSERT INTO T_Stu_sOrder
(Status
,DeptID
,sEnrollsProfessionID
,PlanItemID
,PlanName
,PlanSort
,PlanLevel
,Year
,Month
,NumItemID
,NumName
,LimitTime
,ItemDetailID
,DetailID
,Sort
,ShouldMoney
,PaidMoney
,IsGive
,ItemQueue
,ItemDetailQueue
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@DeptID
,@sEnrollsProfessionID
,@PlanItemID
,@PlanName
,@PlanSort
,@PlanLevel
,@Year
,@Month
,@NumItemID
,@NumName
,@LimitTime
,@ItemDetailID
,@DetailID
,@Sort
,@ShouldMoney
,@PaidMoney
,@IsGive
,@ItemQueue
,@ItemDetailQueue
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", om.Status),
new SqlParameter("@DeptID", om.DeptID),
new SqlParameter("@sEnrollsProfessionID", om.sEnrollsProfessionID),
new SqlParameter("@PlanItemID", om.PlanItemID),
new SqlParameter("@PlanName", om.PlanName),
new SqlParameter("@PlanSort", om.PlanSort),
new SqlParameter("@PlanLevel", om.PlanLevel),
new SqlParameter("@Year", om.Year),
new SqlParameter("@Month", om.Month),
new SqlParameter("@NumItemID", om.NumItemID),
new SqlParameter("@NumName", om.NumName),
new SqlParameter("@LimitTime", om.LimitTime),
new SqlParameter("@ItemDetailID", om.ItemDetailID),
new SqlParameter("@DetailID", om.DetailID),
new SqlParameter("@Sort", om.Sort),
new SqlParameter("@ShouldMoney", om.ShouldMoney),
new SqlParameter("@PaidMoney", om.PaidMoney),
new SqlParameter("@IsGive", om.IsGive),
new SqlParameter("@ItemQueue", om.ItemQueue),
new SqlParameter("@ItemDetailQueue", om.ItemDetailQueue),
new SqlParameter("@CreateID", om.CreateID),
new SqlParameter("@CreateTime", om.CreateTime),
new SqlParameter("@UpdateID", om.UpdateID),
new SqlParameter("@UpdateTime", om.UpdateTime)
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

        public static int UpdatesOrder(sOrderModel om)
        {
            string cmdText = @"UPDATE T_Stu_sOrder SET
Status=@Status
,DeptID=@DeptID
,sEnrollsProfessionID=@sEnrollsProfessionID
,PlanItemID=@PlanItemID
,PlanName=@PlanName
,PlanSort=@PlanSort
,PlanLevel=@PlanLevel
,Year=@Year
,Month=@Month
,NumItemID=@NumItemID
,NumName=@NumName
,LimitTime=@LimitTime
,ItemDetailID=@ItemDetailID
,DetailID=@DetailID
,Sort=@Sort
,ShouldMoney=@ShouldMoney
,PaidMoney=@PaidMoney
,IsGive=@IsGive
,ItemQueue=@ItemQueue
,ItemDetailQueue=@ItemDetailQueue
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sOrderID=@sOrderID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sOrderID", om.sOrderID),
new SqlParameter("@Status", om.Status),
new SqlParameter("@DeptID", om.DeptID),
new SqlParameter("@sEnrollsProfessionID", om.sEnrollsProfessionID),
new SqlParameter("@PlanItemID", om.PlanItemID),
new SqlParameter("@PlanName", om.PlanName),
new SqlParameter("@PlanSort", om.PlanSort),
new SqlParameter("@PlanLevel", om.PlanLevel),
new SqlParameter("@Year", om.Year),
new SqlParameter("@Month", om.Month),
new SqlParameter("@NumItemID", om.NumItemID),
new SqlParameter("@NumName", om.NumName),
new SqlParameter("@LimitTime", om.LimitTime),
new SqlParameter("@ItemDetailID", om.ItemDetailID),
new SqlParameter("@DetailID", om.DetailID),
new SqlParameter("@Sort", om.Sort),
new SqlParameter("@ShouldMoney", om.ShouldMoney),
new SqlParameter("@PaidMoney", om.PaidMoney),
new SqlParameter("@IsGive", om.IsGive),
new SqlParameter("@ItemQueue", om.ItemQueue),
new SqlParameter("@ItemDetailQueue", om.ItemDetailQueue),
new SqlParameter("@CreateID", om.CreateID),
new SqlParameter("@CreateTime", om.CreateTime),
new SqlParameter("@UpdateID", om.UpdateID),
new SqlParameter("@UpdateTime", om.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(om.sOrderID);
            }
            else
            {
                return -1;
            }
        }

        public static sOrderModel sOrderModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sOrderModel om = new sOrderModel();
            string cmdText = "SELECT * FROM T_Stu_sOrder WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                om.sOrderID = dt.Rows[0]["sOrderID"].ToString();
                om.Status = dt.Rows[0]["Status"].ToString();
                om.DeptID = dt.Rows[0]["DeptID"].ToString();
                om.sEnrollsProfessionID = dt.Rows[0]["sEnrollsProfessionID"].ToString();
                om.PlanItemID = dt.Rows[0]["PlanItemID"].ToString();
                om.PlanName = dt.Rows[0]["PlanName"].ToString();
                om.PlanSort = dt.Rows[0]["PlanSort"].ToString();
                om.PlanLevel = dt.Rows[0]["PlanLevel"].ToString();
                om.Year = dt.Rows[0]["Year"].ToString();
                om.Month = dt.Rows[0]["Month"].ToString();
                om.NumItemID = dt.Rows[0]["NumItemID"].ToString();
                om.NumName = dt.Rows[0]["NumName"].ToString();
                om.LimitTime = dt.Rows[0]["LimitTime"].ToString();
                om.ItemDetailID = dt.Rows[0]["ItemDetailID"].ToString();
                om.DetailID = dt.Rows[0]["DetailID"].ToString();
                om.Sort = dt.Rows[0]["Sort"].ToString();
                om.ShouldMoney = dt.Rows[0]["ShouldMoney"].ToString();
                om.PaidMoney = dt.Rows[0]["PaidMoney"].ToString();
                om.IsGive = dt.Rows[0]["IsGive"].ToString();
                om.ItemQueue = dt.Rows[0]["ItemQueue"].ToString();
                om.ItemDetailQueue = dt.Rows[0]["ItemDetailQueue"].ToString();
                om.CreateID = dt.Rows[0]["CreateID"].ToString();
                om.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                om.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                om.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return om;
        }

        public static DataTable sOrderTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sOrder WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        /// <summary>
        /// 根据方案ID返回订单
        /// </summary>
        /// <param name="planItemId"></param>
        /// <returns></returns>
        public static DataTable GetsOrderTable(string planItemId, string sEnrollsProfessionID)
        {
            if (string.IsNullOrEmpty(planItemId))
                planItemId = "0";
            if (string.IsNullOrEmpty(sEnrollsProfessionID))
                sEnrollsProfessionID = "0";
            string cmdText = @"SELECT  DISTINCT
					o.ItemQueue,
                    o.NumItemID ,
                    o.NumName ,
                    o.PlanItemID ItemID
            FROM    T_Stu_sOrder o
                    LEFT JOIN T_Pro_Detail d ON d.DetailID = o.DetailID
        Where o.PlanItemID=@PlanItemID and sEnrollsProfessionID=@sEnrollsProfessionID and o.Status<>9  Order BY o.ItemQueue ASC";//CONVERT(NVARCHAR(10), o.LimitTime, 23) LimitTime ,
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@PlanItemID",planItemId),
                new SqlParameter("@sEnrollsProfessionID",sEnrollsProfessionID)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
        /// <summary>
        /// 获取缴费方案ID
        /// </summary>
        /// <param name="numPlanId"></param>
        /// <returns></returns>
        public static string GetPlanItemId(string numPlanId)
        {
            string where = " and NumItemID=@NumItemID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@NumItemID",numPlanId)
            };
            return sOrderBLL.sOrderModelByWhere(where, paras).PlanItemID;
        }
        /// <summary>
        /// 获取未缴金额
        /// </summary>
        /// <param name="sorderId"></param>
        /// <returns></returns>
        public static decimal GetShouldMoney(string sorderId, ref decimal paidMoney)
        {
            string cmdText = " Select isnull(ShouldMoney,0.00) ShouldMoney,isnull(PaidMoney,0.00) PaidMoney FROM    T_Stu_sOrder Where sOrderID=@sOrderID and Status<>9 ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sOrderID",sorderId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                paidMoney = decimal.Parse(dt.Rows[0]["PaidMoney"].ToString());
                return decimal.Parse(dt.Rows[0]["ShouldMoney"].ToString());
            }
            else
            {
                paidMoney = 0;
                return 0;
            }

        }

        /// <summary>
        /// 获取缴费次数下拉
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public static DataTable GetsOrderCombobox(string planId, string proId)
        {
            string cmdText = "Select  DISTINCT NumName  name, NumItemID id,ItemQueue  FROM    T_Stu_sOrder Where PlanItemID=@PlanItemID and sEnrollsProfessionID=@sEnrollsProfessionID and Status<>9  ORDER BY ItemQueue ASC";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@PlanItemID",planId),
                new SqlParameter("@sEnrollsProfessionID",proId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
        /// <summary>
        /// 修改订单已缴金额
        /// </summary>
        /// <param name="om"></param>
        /// <returns></returns>
        public static int UpdatesOrderPaidMoney(sOrderModel om)
        {
            string cmdText = @"UPDATE T_Stu_sOrder SET
PaidMoney=@PaidMoney
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sOrderID=@sOrderID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sOrderID", om.sOrderID),
new SqlParameter("@PaidMoney", om.PaidMoney),
new SqlParameter("@UpdateID", om.UpdateID),
new SqlParameter("@UpdateTime", om.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
                return Convert.ToInt32(om.sOrderID);
            else
                return -1;
        }

        /// <summary>
        /// 停用要停掉已生成的缴费单数据，停掉已生成的配品数据
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <param name="status"></param>
        public static void UpdateStatus(string sEnrollsProfessionID, string status)
        {
            string cmdText = @"UPDATE T_Stu_sOrder SET Status=@Status WHERE sEnrollsProfessionID=@sEnrollsProfessionID;UPDATE T_Stu_sOrderGive SET Status=@Status WHERE sEnrollsProfessionID=@sEnrollsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", status),
new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID)
};
            DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 修改状态，根据sOrderID
        /// </summary>
        /// <param name="sOrderIDs">可能有多个ID（1,2,3）</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static int UpdateStatusBysOrders(string sOrderIDs, string status)
        {
            string cmdText = @"UPDATE T_Stu_sOrder SET Status=@Status WHERE sOrderID in (" + sOrderIDs + ")";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Status", status)
            };
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 变更订单的实缴金额
        /// </summary>
        /// <param name="money">金额</param>
        /// <param name="sOrderId">订单编号</param>
        /// <param name="userId">修改人</param>
        /// <returns></returns>
        public static int UpdatesOderPaidMoney(decimal paidMoney, string sorderId, int userId)
        {

            sOrderModel som = sOrderBLL.sOrderModelByWhere(sorderId);//获取订单信息
            if (!string.IsNullOrEmpty(som.sOrderID))//订单信息不为空
            {
                string status = "1";
                decimal tempShouldMoney = decimal.Parse(som.ShouldMoney);
                decimal tempPaidMoney = decimal.Parse(som.PaidMoney);
                if (paidMoney + tempPaidMoney < tempShouldMoney && paidMoney + tempPaidMoney > 0)//应缴金额大于实缴
                    status = "2";//部分缴费
                else if (paidMoney + tempPaidMoney == tempShouldMoney)
                    status = "3";//已缴费
                else if (paidMoney + tempPaidMoney <= 0)
                    status = "1";//未缴费 
                if (som.Status.Equals("9"))
                    status = "9";
                som.PaidMoney = (decimal.Parse(som.PaidMoney) + paidMoney).ToString();//变更后的实缴金额
                som.Status = status;//变更后的状态
                som.UpdateID = userId.ToString();
                som.UpdateTime = DateTime.Now.ToString();
                return sOrderBLL.UpdatesOrder(som);
            }
            else
                return 0;
        }
        /// <summary>
        /// 获取订单实体
        /// </summary>
        /// <param name="sorderId"></param>
        /// <returns></returns>
        public static sOrderModel sOrderModelByWhere(string sorderId)
        {

            string where = " and sOrderID=@sOrderID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sOrderID",sorderId)
            };
            return sOrderBLL.sOrderModelByWhere(where, paras);

        }

        /// <summary>
        /// 修改IsGive，根据sOrderID
        /// </summary>
        /// <param name="sOrderIDs">可能有多个ID（1,2,3）</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static int UpdateIsGive(string sOrderIDs, string status)
        {
            string cmdText = @"UPDATE T_Stu_sOrder SET IsGive=@IsGive WHERE sOrderID in (" + sOrderIDs + ")";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@IsGive", status)
};
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 根据学生ID得到报名专业下拉数据（有权限控制）
        /// </summary>
        /// <param name="StudentID"></param>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public static DataTable GetsEnrollMajor(string StudentID, string MenuID, string PowerStr)
        {
            string cmdText = @"SELECT  pro.sEnrollsProfessionID AS Id ,
        ( CAST(pro.Year AS NVARCHAR) + '_' + CAST(pro.Month AS NVARCHAR) + '_'
          + refe.RefeName + '_' + basicpro.Name ) AS Text
FROM    T_Stu_sEnrollsProfession AS pro
        LEFT JOIN T_Stu_sEnroll AS senroll ON pro.sEnrollID = senroll.sEnrollID
        LEFT JOIN T_Sys_Refe AS refe ON refe.Value = pro.EnrollLevel
                                            AND refe.RefeTypeID = 17
        LEFT JOIN T_Stu_sProfession AS spro ON pro.sProfessionID = spro.sProfessionID
        LEFT JOIN T_Pro_Profession AS basicpro ON spro.ProfessionID = basicpro.ProfessionID
WHERE   senroll.StudentID = {0}
        AND pro.Status != 9 {1}";
            string powerStr = string.Format(PowerStr, "pro.DeptID", "pro.CreateID");
            cmdText = string.Format(cmdText, StudentID, powerStr);
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
        }

        /// <summary>
        /// 根据报名专业ID得到缴费方案（从T_Stu_sOrder取数据）下拉数据
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <returns></returns>
        public static DataTable GetsEnrollScheme(string sEnrollsProfessionID)
        {
            string cmdText = @"SELECT DISTINCT
        PlanItemID AS Id ,
        PlanName AS Text
FROM    T_Stu_sOrder
WHERE   sEnrollsProfessionID = {0}
        AND Status != 9
ORDER BY PlanItemID ASC";
            cmdText = string.Format(cmdText, sEnrollsProfessionID);
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
        }

        /// <summary>
        /// 根据报名缴费方案ID得到缴费期数（从T_Stu_sOrder取数据）下拉数据
        /// </summary>
        /// <param name="planItemID"></param>
        /// <returns></returns>
        public static DataTable GetSchemeSemester(string planItemID)
        {
            string cmdText = @"SELECT DISTINCT
        t.*
FROM    ( SELECT TOP ( 100 ) PERCENT
                    NumItemID AS Id ,
                    NumName AS Text
          FROM      T_Stu_sOrder
          WHERE     PlanItemID = {0}
                    AND Status != 9
          ORDER BY  ItemQueue ASC
        ) AS t";
            cmdText = string.Format(cmdText, planItemID);
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
        }

        /// <summary>
        /// 得到费用项排序
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <param name="planItemID"></param>
        /// <param name="numItemID"></param>
        /// <returns></returns>
        public static string GetItemDetailQueue(string sEnrollsProfessionID, string planItemID, string numItemID)
        {
            string cmdText = @"SELECT  ISNULL(MAX(ItemDetailQueue),0)+1
FROM    T_Stu_sOrder
WHERE   sEnrollsProfessionID = @sEnrollsProfessionID
        AND PlanItemID = @PlanItemID
        AND NumItemID = @NumItemID
        AND Status != 9";
            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",sEnrollsProfessionID),
                new SqlParameter("@PlanItemID",planItemID),
                new SqlParameter("@NumItemID",numItemID)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, para).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 创建缴费单
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static string BuildsOrder(string sEnrollsProfessionID, string UserID)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder result2 = new StringBuilder();
            StringBuilder result3 = new StringBuilder();
            //判断该报名学生是否有身份证号码，没有就不能生成缴费单
            //bool IsHaveIDCard = sEnrollBLL.CheckIsHaveIDCard(sEnrollsProfessionID);
            //得到报名专业数据（1条）
            DataTable majorDt = sEnrollBLL.SelectsEnroll(sEnrollsProfessionID);
            //得到报名专业对应的学费缴费方案（多条），T_Pro_Item，排除作废的
            DataTable schemeDt = sEnrollBLL.GetsFeeScheme(sEnrollsProfessionID);
            //if (IsHaveIDCard)
            //{
            //校区、学生姓名、学号、身份证号、年份、月份、学习层次、专业、缴费方案、生成结果、备注
            if (schemeDt.Rows.Count > 0)
            {
                result.Clear();
                for (int i = 0; i < schemeDt.Rows.Count; i++)
                {
                    #region 生成缴费单
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        try
                        {
                            result2.Clear();
                            result2.Append("{");
                            //判断缴费方案是否缴过费（排除停用的）
                            DataTable sOrderDt = sOrderBLL.sOrderTableByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID AND PlanItemID=@PlanItemID AND Status!=9", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID), new SqlParameter("@PlanItemID", schemeDt.Rows[i]["ItemID"].ToString()) }, "");
                            decimal paidMoney = sOrderDt.AsEnumerable().Select(x => x.Field<decimal>("PaidMoney")).Sum();
                            //得到缴费方案下面的缴费期数（多条），T_Pro_Item，排除作废的
                            DataTable semesterDt = sEnrollBLL.GetSemester(schemeDt.Rows[i]["ItemID"].ToString());
                            if (paidMoney > 0)
                            {
                                #region 方案缴过费
                                //缴过费就判断学期
                                if (semesterDt.Rows.Count > 0)
                                {
                                    #region 有缴费期数
                                    //拼接原始方案的缴费期数ID字符串
                                    string oldItemIDs = string.Join(",", semesterDt.AsEnumerable().Select(x => x.Field<int>("ItemID")).ToArray());
                                    //拼接缴费单中方案的缴费期数字符串，根据ItemQueue正序排列
                                    string buildItemIDs = string.Join(",", sOrderDt.AsEnumerable().OrderBy(x => x.Field<int>("ItemQueue")).Select(x => x.Field<int>("NumItemID")).Distinct().ToArray());
                                    if (oldItemIDs.Contains(buildItemIDs))
                                    {
                                        int tempCount1 = 0;
                                        //方案调整在可识别范围内
                                        for (int a = 0; a < semesterDt.Rows.Count; a++)
                                        {
                                            //判断该学期是否缴过费
                                            int itemID = int.Parse(semesterDt.Rows[a]["ItemID"].ToString());
                                            decimal paid = sOrderDt.AsEnumerable().Where(x => x.Field<int>("NumItemID") == itemID).Select(x => x.Field<decimal>("PaidMoney")).Sum();
                                            if (paid > 0)
                                            {
                                                //该期缴过费，把缴费单中该期的数据的ItemQueue给修改了
                                                sEnrollBLL.UpdatesOrderItemQueue(sEnrollsProfessionID, schemeDt.Rows[i]["ItemID"].ToString(), semesterDt.Rows[a]["ItemID"].ToString(), semesterDt.Rows[a]["Queue"].ToString());
                                                tempCount1++;
                                            }
                                            else
                                            {
                                                //该期未缴费，停掉该期缴费单，然后重新生成该期的缴费单
                                                sEnrollBLL.StopsOrder(sEnrollsProfessionID, schemeDt.Rows[i]["ItemID"].ToString(), semesterDt.Rows[a]["ItemID"].ToString());
                                                //得到每期下面的详细费用项，T_Pro_ItemDetail，排除作废的
                                                DataTable itemDetailDt = sEnrollBLL.GetItemDetail(semesterDt.Rows[a]["ItemID"].ToString());
                                                if (itemDetailDt.Rows.Count > 0)
                                                {
                                                    for (int k = 0; k < itemDetailDt.Rows.Count; k++)
                                                    {
                                                        #region 写入缴费单sOrder
                                                        sOrderModel sOrder = new sOrderModel();
                                                        sOrder.Status = "1";
                                                        sOrder.DeptID = majorDt.Rows[0]["DeptID"].ToString();
                                                        sOrder.sEnrollsProfessionID = sEnrollsProfessionID;
                                                        sOrder.PlanItemID = schemeDt.Rows[i]["ItemID"].ToString();
                                                        sOrder.PlanName = schemeDt.Rows[i]["Name"].ToString();
                                                        sOrder.PlanSort = schemeDt.Rows[i]["Sort"].ToString();
                                                        sOrder.PlanLevel = schemeDt.Rows[i]["PlanLevel"].ToString();
                                                        sOrder.Year = schemeDt.Rows[i]["Year"].ToString();
                                                        sOrder.Month = schemeDt.Rows[i]["Month"].ToString();
                                                        sOrder.NumItemID = semesterDt.Rows[a]["ItemID"].ToString();
                                                        sOrder.NumName = semesterDt.Rows[a]["Name"].ToString();
                                                        //sOrder.LimitTime = semesterDt.Rows[a]["LimitTime"].ToString();
                                                        //第一次缴费方案的第一期缴费，截止日期为报名日期
                                                        if (i == 0 && a == 0)
                                                        {
                                                            if (string.IsNullOrEmpty(majorDt.Rows[0]["EnrollTime"].ToString()))//如果报名时间为空，则第一次第一期截止日期为当前时间
                                                            {
                                                                sOrder.LimitTime = DateTime.Now.ToString("yyyy-MM-dd");
                                                            }
                                                            else
                                                            {
                                                                if (Convert.ToDateTime(majorDt.Rows[0]["EnrollTime"]).ToString("yyyy-MM-dd") == "1900-01-01")
                                                                {
                                                                    sOrder.LimitTime = DateTime.Now.ToString("yyyy-MM-dd");
                                                                }
                                                                else
                                                                {
                                                                    sOrder.LimitTime = majorDt.Rows[0]["EnrollTime"].ToString();
                                                                }
                                                            }
                                                            if (DateTime.Parse(semesterDt.Rows[a]["LimitTime"].ToString())<DateTime.Parse(sOrder.LimitTime))
                                                            {
                                                                sOrder.LimitTime = semesterDt.Rows[a]["LimitTime"].ToString();
                                                            }

                                                            
                                                        }
                                                        else
                                                        {
                                                            sOrder.LimitTime = semesterDt.Rows[a]["LimitTime"].ToString();
                                                        }
                                                        sOrder.ItemDetailID = itemDetailDt.Rows[k]["ItemDetailID"].ToString();
                                                        sOrder.DetailID = itemDetailDt.Rows[k]["DetailID"].ToString();
                                                        sOrder.Sort = itemDetailDt.Rows[k]["Sort"].ToString();
                                                        sOrder.ShouldMoney = itemDetailDt.Rows[k]["Money"].ToString();
                                                        sOrder.PaidMoney = "0";
                                                        sOrder.IsGive = itemDetailDt.Rows[k]["IsGive"].ToString();
                                                        sOrder.ItemQueue = semesterDt.Rows[a]["Queue"].ToString();
                                                        sOrder.ItemDetailQueue = itemDetailDt.Rows[k]["Queue"].ToString();
                                                        sOrder.CreateID = UserID;
                                                        sOrder.CreateTime = DateTime.Now.ToString();
                                                        sOrder.UpdateID = UserID;
                                                        sOrder.UpdateTime = DateTime.Now.ToString();
                                                        sOrderBLL.InsertsOrder(sOrder);

                                                        tempCount1++;
                                                        #endregion
                                                    }
                                                }
                                            }
                                        }
                                        if (tempCount1 > 0)
                                        {
                                            result2.Append("\"Result\":\"缴费单生成成功\",");
                                            result2.Append("\"Remark\":\"\",");
                                            ts.Complete();//提交
                                        }
                                        else
                                        {
                                            result2.Append("\"Result\":\"缴费单生成失败\",");
                                            result2.Append("\"Remark\":\"方案设置有误\",");
                                        }
                                    }
                                    else
                                    {
                                        //方案调整过大，系统不能判断
                                        result2.Append("\"Result\":\"缴费单生成失败\",");
                                        result2.Append("\"Remark\":\"缴费期数不匹配\",");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 无缴费期数
                                    result2.Append("\"Result\":\"缴费单生成失败\",");
                                    result2.Append("\"Remark\":\"方案设置有误\",");
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region 方案未缴费
                                //没有缴过费就直接生成该方案的费用，把之前该方案的缴费单停掉
                                sEnrollBLL.StopsOrder(sEnrollsProfessionID, schemeDt.Rows[i]["ItemID"].ToString());
                                if (semesterDt.Rows.Count > 0)
                                {
                                    #region 有缴费期数
                                    int tempCount = 0;
                                    for (int n = 0; n < semesterDt.Rows.Count; n++)
                                    {
                                        //得到每期下面的详细费用项，T_Pro_ItemDetail，排除作废的
                                        DataTable itemDetailDt = sEnrollBLL.GetItemDetail(semesterDt.Rows[n]["ItemID"].ToString());
                                        if (itemDetailDt.Rows.Count > 0)
                                        {
                                            for (int k = 0; k < itemDetailDt.Rows.Count; k++)
                                            {
                                                #region 写入缴费单sOrder
                                                sOrderModel sOrder = new sOrderModel();
                                                sOrder.Status = "1";
                                                sOrder.DeptID = majorDt.Rows[0]["DeptID"].ToString();
                                                sOrder.sEnrollsProfessionID = sEnrollsProfessionID;
                                                sOrder.PlanItemID = schemeDt.Rows[i]["ItemID"].ToString();
                                                sOrder.PlanName = schemeDt.Rows[i]["Name"].ToString();
                                                sOrder.PlanSort = schemeDt.Rows[i]["Sort"].ToString();
                                                sOrder.PlanLevel = schemeDt.Rows[i]["PlanLevel"].ToString();
                                                sOrder.Year = schemeDt.Rows[i]["Year"].ToString();
                                                sOrder.Month = schemeDt.Rows[i]["Month"].ToString();
                                                sOrder.NumItemID = semesterDt.Rows[n]["ItemID"].ToString();
                                                sOrder.NumName = semesterDt.Rows[n]["Name"].ToString();
                                                //第一次缴费方案的第一期缴费，截止日期为报名日期
                                                if (i == 0 && n == 0)
                                                {
                                                    if (string.IsNullOrEmpty(majorDt.Rows[0]["EnrollTime"].ToString()))//如果报名时间为空，则第一次第一期截止日期为当前时间
                                                    {
                                                        sOrder.LimitTime = DateTime.Now.ToString("yyyy-MM-dd");
                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToDateTime(majorDt.Rows[0]["EnrollTime"]).ToString("yyyy-MM-dd") == "1900-01-01")
                                                        {
                                                            sOrder.LimitTime = DateTime.Now.ToString("yyyy-MM-dd");
                                                        }
                                                        else
                                                        {
                                                            sOrder.LimitTime = majorDt.Rows[0]["EnrollTime"].ToString();
                                                        }
                                                    }
                                                    if (DateTime.Parse(semesterDt.Rows[n]["LimitTime"].ToString()) < DateTime.Parse(sOrder.LimitTime))
                                                    {
                                                        sOrder.LimitTime = semesterDt.Rows[n]["LimitTime"].ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    sOrder.LimitTime = semesterDt.Rows[n]["LimitTime"].ToString();
                                                }
                                                sOrder.ItemDetailID = itemDetailDt.Rows[k]["ItemDetailID"].ToString();
                                                sOrder.DetailID = itemDetailDt.Rows[k]["DetailID"].ToString();
                                                sOrder.Sort = itemDetailDt.Rows[k]["Sort"].ToString();
                                                sOrder.ShouldMoney = itemDetailDt.Rows[k]["Money"].ToString();
                                                sOrder.PaidMoney = "0";
                                                sOrder.IsGive = itemDetailDt.Rows[k]["IsGive"].ToString();
                                                sOrder.ItemQueue = semesterDt.Rows[n]["Queue"].ToString();
                                                sOrder.ItemDetailQueue = itemDetailDt.Rows[k]["Queue"].ToString();
                                                sOrder.CreateID = UserID;
                                                sOrder.CreateTime = DateTime.Now.ToString();
                                                sOrder.UpdateID = UserID;
                                                sOrder.UpdateTime = DateTime.Now.ToString();
                                                sOrderBLL.InsertsOrder(sOrder);

                                                tempCount++;
                                                #endregion
                                            }
                                        }
                                    }
                                    if (tempCount > 0)
                                    {
                                        result2.Append("\"Result\":\"缴费单生成成功\",");
                                        result2.Append("\"Remark\":\"\",");
                                        ts.Complete();//提交
                                    }
                                    else
                                    {
                                        result2.Append("\"Result\":\"缴费单生成失败\",");
                                        result2.Append("\"Remark\":\"方案设置有误\",");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 无缴费期数
                                    result2.Append("\"Result\":\"缴费单生成失败\",");
                                    result2.Append("\"Remark\":\"方案设置有误\",");
                                    #endregion
                                }
                                #endregion
                            }
                            result2.Append("\"StuName\":\"" + majorDt.Rows[0]["StuName"] + "\",");
                            result2.Append("\"Year\":\"" + majorDt.Rows[0]["Year"] + "\",");
                            result2.Append("\"Month\":\"" + majorDt.Rows[0]["MonthName"] + "\",");
                            result2.Append("\"Level\":\"" + majorDt.Rows[0]["LevelName"] + "\",");
                            result2.Append("\"Major\":\"" + majorDt.Rows[0]["MajorName"] + "\",");
                            result2.Append("\"Scheme\":\"" + schemeDt.Rows[i]["Name"] + "\",");
                            result2.Append("\"IDCard\":\"" + majorDt.Rows[0]["IDCard"] + "\"");
                            result2.Append("}");
                            result2.Append(",");
                        }
                        catch
                        {
                            #region 系统报错
                            result2.Clear();
                            result2.Append("{");
                            result2.Append("\"Result\":\"缴费单生成失败\",");
                            result2.Append("\"Remark\":\"系统出现未知错误，请联系管理员！\",");
                            result2.Append("\"StuName\":\"" + majorDt.Rows[0]["StuName"] + "\",");
                            result2.Append("\"Year\":\"" + majorDt.Rows[0]["Year"] + "\",");
                            result2.Append("\"Month\":\"" + majorDt.Rows[0]["MonthName"] + "\",");
                            result2.Append("\"Level\":\"" + majorDt.Rows[0]["LevelName"] + "\",");
                            result2.Append("\"Major\":\"" + majorDt.Rows[0]["MajorName"] + "\",");
                            result2.Append("\"Scheme\":\"" + schemeDt.Rows[i]["Name"] + "\",");
                            result2.Append("\"IDCard\":\"" + majorDt.Rows[0]["IDCard"] + "\"");
                            result2.Append("}");
                            result2.Append(",");
                            Transaction.Current.Rollback();
                            #endregion
                        }
                        finally
                        {
                            ts.Dispose();
                        }
                        result.Append(result2);
                    }
                    #endregion

                    #region 生成配品
                    using (TransactionScope ts2 = new TransactionScope(TransactionScopeOption.Required))
                    {
                        try
                        {
                            result3.Clear();
                            result3.Append("{");
                            //得到缴费方案下面的配品（多条），T_Stu_sItemsGive
                            DataTable giveDt = sEnrollBLL.GetsGive(schemeDt.Rows[i]["ItemID"].ToString());
                            //判断缴费方案的配品是否领取（排除停用的），2已领取
                            bool isTake = sEnrollBLL.IsTakeGive(sEnrollsProfessionID, schemeDt.Rows[i]["ItemID"].ToString());
                            if (isTake)
                            {
                                #region 方案配品已领取
                                if (giveDt.Rows.Count > 0)
                                {
                                    #region 有配品
                                    DataTable sOrderGiveDt = sOrderGiveBLL.sOrderGiveTableByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID AND PlanItemID=@PlanItemID AND Status!=9", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID), new SqlParameter("@PlanItemID", schemeDt.Rows[i]["ItemID"].ToString()) }, "");
                                    //拼接原始方案的配品ID字符串
                                    string oldGiveIDs = string.Join(",", giveDt.AsEnumerable().Select(x => x.Field<int>("sGiveID")).ToArray());
                                    //拼接已生成配品中方案的配品ID字符串，根据Queue正序排列
                                    string buildGiveIDs = string.Join(",", sOrderGiveDt.AsEnumerable().OrderBy(x => x.Field<int>("Queue")).Select(x => x.Field<int>("sGiveID")).ToArray());
                                    if (oldGiveIDs.Contains(buildGiveIDs))
                                    {
                                        int giveTempCount2 = 0;
                                        //方案配品调整在可识别范围内
                                        for (int b = 0; b < giveDt.Rows.Count; b++)
                                        {
                                            //判断配品是否领取
                                            int sGiveID = int.Parse(giveDt.Rows[b]["sGiveID"].ToString());
                                            object status = sOrderGiveDt.AsEnumerable().Where(x => x.Field<int>("sGiveID") == sGiveID).Select(x => x.Field<byte>("Status")).FirstOrDefault();
                                            if (status != null && status.ToString() == "2")
                                            {
                                                //配品已领取，只修改配品的Queue
                                                sEnrollBLL.UpdatesOrderGiveQueue(sEnrollsProfessionID, schemeDt.Rows[i]["ItemID"].ToString(), giveDt.Rows[b]["sGiveID"].ToString(), giveDt.Rows[b]["Queue"].ToString());
                                                giveTempCount2++;
                                            }
                                            else
                                            {
                                                //配品未领取，停用该配品，然后重新生成
                                                sEnrollBLL.StopsOrderGive(sEnrollsProfessionID, schemeDt.Rows[i]["ItemID"].ToString(), giveDt.Rows[b]["sGiveID"].ToString());
                                                #region 写入配品sOrderGive
                                                sOrderGiveModel sOrderGive = new sOrderGiveModel();
                                                sOrderGive.Status = "1";
                                                sOrderGive.DeptID = majorDt.Rows[0]["DeptID"].ToString();
                                                sOrderGive.sEnrollsProfessionID = sEnrollsProfessionID;
                                                sOrderGive.PlanItemID = schemeDt.Rows[i]["ItemID"].ToString();
                                                sOrderGive.Year = schemeDt.Rows[i]["Year"].ToString();
                                                sOrderGive.Month = schemeDt.Rows[i]["Month"].ToString();
                                                sOrderGive.sGiveID = giveDt.Rows[b]["sGiveID"].ToString();
                                                sOrderGive.Queue = giveDt.Rows[b]["Queue"].ToString();
                                                sOrderGive.CreateID = UserID;
                                                sOrderGive.CreateTime = DateTime.Now.ToString();
                                                sOrderGive.UpdateID = UserID;
                                                sOrderGive.UpdateTime = DateTime.Now.ToString();
                                                sOrderGiveBLL.InsertsOrderGive(sOrderGive);

                                                giveTempCount2++;
                                                #endregion
                                            }
                                        }
                                        if (giveTempCount2 > 0)
                                        {
                                            result3.Append("\"Result\":\"配品生成成功\",");
                                            result3.Append("\"Remark\":\"\",");
                                            ts2.Complete();//提交
                                        }
                                        else
                                        {
                                            result3.Append("\"Result\":\"配品生成失败\",");
                                            result3.Append("\"Remark\":\"方案配品设置有误\",");
                                        }
                                    }
                                    else
                                    {
                                        //系统不可识别
                                        result3.Append("\"Result\":\"配品生成失败\",");
                                        result3.Append("\"Remark\":\"配品不匹配\",");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 无配品
                                    result3.Append("\"Result\":\"未生成配品\",");
                                    result3.Append("\"Remark\":\"方案未设置配品\",");
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region 方案配品未领取，将之前生成的配品禁用，然后重新生成
                                sEnrollBLL.StopsOrderGive(sEnrollsProfessionID, schemeDt.Rows[i]["ItemID"].ToString());
                                if (giveDt.Rows.Count > 0)
                                {
                                    #region 有配品
                                    int giveTempCount = 0;
                                    for (int t = 0; t < giveDt.Rows.Count; t++)
                                    {
                                        #region 写入配品sOrderGive
                                        sOrderGiveModel sOrderGive = new sOrderGiveModel();
                                        sOrderGive.Status = "1";
                                        sOrderGive.DeptID = majorDt.Rows[0]["DeptID"].ToString();
                                        sOrderGive.sEnrollsProfessionID = sEnrollsProfessionID;
                                        sOrderGive.PlanItemID = schemeDt.Rows[i]["ItemID"].ToString();
                                        sOrderGive.Year = schemeDt.Rows[i]["Year"].ToString();
                                        sOrderGive.Month = schemeDt.Rows[i]["Month"].ToString();
                                        sOrderGive.sGiveID = giveDt.Rows[t]["sGiveID"].ToString();
                                        sOrderGive.Queue = giveDt.Rows[t]["Queue"].ToString();
                                        sOrderGive.CreateID = UserID;
                                        sOrderGive.CreateTime = DateTime.Now.ToString();
                                        sOrderGive.UpdateID = UserID;
                                        sOrderGive.UpdateTime = DateTime.Now.ToString();
                                        sOrderGiveBLL.InsertsOrderGive(sOrderGive);

                                        giveTempCount++;
                                        #endregion
                                    }
                                    if (giveTempCount > 0)
                                    {
                                        result3.Append("\"Result\":\"配品生成成功\",");
                                        result3.Append("\"Remark\":\"\",");
                                        ts2.Complete();//提交
                                    }
                                    else
                                    {
                                        result3.Append("\"Result\":\"配品生成失败\",");
                                        result3.Append("\"Remark\":\"方案配品设置有误\",");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 无配品
                                    result3.Append("\"Result\":\"未生成配品\",");
                                    result3.Append("\"Remark\":\"方案未设置配品\",");
                                    #endregion
                                }
                                #endregion
                            }
                            result3.Append("\"StuName\":\"" + majorDt.Rows[0]["StuName"] + "\",");
                            result3.Append("\"Year\":\"" + majorDt.Rows[0]["Year"] + "\",");
                            result3.Append("\"Month\":\"" + majorDt.Rows[0]["MonthName"] + "\",");
                            result3.Append("\"Level\":\"" + majorDt.Rows[0]["LevelName"] + "\",");
                            result3.Append("\"Major\":\"" + majorDt.Rows[0]["MajorName"] + "\",");
                            result3.Append("\"Scheme\":\"" + schemeDt.Rows[i]["Name"] + "\",");
                            result3.Append("\"IDCard\":\"" + majorDt.Rows[0]["IDCard"] + "\"");
                            result3.Append("}");
                            result3.Append(",");
                        }
                        catch
                        {
                            #region 系统错误
                            result3.Clear();
                            result3.Append("{");
                            result3.Append("\"Result\":\"配品生成失败\",");
                            result3.Append("\"Remark\":\"系统出现未知错误，请联系管理员！\",");
                            result3.Append("\"StuName\":\"" + majorDt.Rows[0]["StuName"] + "\",");
                            result3.Append("\"Year\":\"" + majorDt.Rows[0]["Year"] + "\",");
                            result3.Append("\"Month\":\"" + majorDt.Rows[0]["MonthName"] + "\",");
                            result3.Append("\"Level\":\"" + majorDt.Rows[0]["LevelName"] + "\",");
                            result3.Append("\"Major\":\"" + majorDt.Rows[0]["MajorName"] + "\",");
                            result3.Append("\"Scheme\":\"" + schemeDt.Rows[i]["Name"] + "\",");
                            result3.Append("\"IDCard\":\"" + majorDt.Rows[0]["IDCard"] + "\"");
                            result3.Append("}");
                            result3.Append(",");
                            Transaction.Current.Rollback();
                            #endregion
                        }
                        finally
                        {
                            ts2.Dispose();
                        }
                        result.Append(result3);
                    }
                    #endregion
                }

                return "[" + result.ToString().Trim(',') + "]";
            }
            else
            {
                #region 无可用缴费方案
                result.Clear();
                result.Append("[{");
                result.Append("\"Result\":\"缴费单生成失败\",");
                result.Append("\"Remark\":\"方案设置有误\",");
                result.Append("\"StuName\":\"" + majorDt.Rows[0]["StuName"] + "\",");
                result.Append("\"Year\":\"" + majorDt.Rows[0]["Year"] + "\",");
                result.Append("\"Month\":\"" + majorDt.Rows[0]["MonthName"] + "\",");
                result.Append("\"Level\":\"" + majorDt.Rows[0]["LevelName"] + "\",");
                result.Append("\"Major\":\"" + majorDt.Rows[0]["MajorName"] + "\",");
                result.Append("\"Scheme\":\"" + majorDt.Rows[0]["SchemeName"] + "\",");
                result.Append("\"IDCard\":\"" + majorDt.Rows[0]["IDCard"] + "\"");
                result.Append("}]");
                #endregion

                return result.ToString();
            }
            //}
            //else
            //{
            #region 无身份证
            //result.Clear();
            //result.Append("[{");
            //result.Append("\"Result\":\"缴费单生成失败\",");
            //result.Append("\"Remark\":\"没有身份证号码\",");
            //result.Append("\"StuName\":\"" + majorDt.Rows[0]["StuName"] + "\",");
            //result.Append("\"Year\":\"" + majorDt.Rows[0]["Year"] + "\",");
            //result.Append("\"Month\":\"" + majorDt.Rows[0]["MonthName"] + "\",");
            //result.Append("\"Level\":\"" + majorDt.Rows[0]["LevelName"] + "\",");
            //result.Append("\"Major\":\"" + majorDt.Rows[0]["MajorName"] + "\",");
            //result.Append("\"Scheme\":\"" + majorDt.Rows[0]["SchemeName"] + "\",");
            //result.Append("\"IDCard\":\"" + majorDt.Rows[0]["IDCard"] + "\"");
            //result.Append("}]");
            #endregion

            //return result.ToString();
            //}
        }

        /// <summary>
        /// 根据订单id返回报名专业状态
        /// </summary>
        /// <param name="sorderId"></param>
        /// <returns></returns>
        public static string GetEnrollProfessionStatus(string sorderId)
        {
            string cmdText = @"SELECT  ep.Status 
FROM    T_Stu_sOrder o
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = o.sEnrollsProfessionID
        Where o.sOrderID=@sOrderID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sOrderID", sorderId)
};
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt.Rows[0]["Status"].ToString();

        }
        /// <summary>
        /// 根据订单id返回报名号
        /// </summary>
        /// <param name="sorderId"></param>
        /// <returns></returns>
        public static DataTable GetEnrollNum(string sorderId)
        {
            string cmdText = @"SELECT DISTINCT e.sEnrollID,e.EnrollNum,ep.DeptID,ep.Year
FROM    T_Stu_sOrder o
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = o.sEnrollsProfessionID
		LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID=ep.sEnrollID
        Where o.sOrderID=@sOrderID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sOrderID", sorderId)
};
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;

        }
        /// <summary>
        /// 根据报名专业id返回订单id
        /// </summary>
        /// <param name="senrollProfessionId"></param>
        /// <param name="isFee">是否缴费</param>
        /// <returns></returns>
        public static DataTable GetsOrderTable(string senrollProfessionId, bool isFee)
        {
            string where = " and sEnrollsProfessionID=@sEnrollsProfessionID ";
            if (isFee)
            {
                where += " and PaidMoney>0";
            }
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",senrollProfessionId)
            };
            return sOrderBLL.sOrderTableByWhere(where, paras, "");
        }
        /// <summary>
        /// 根据报名/缴费次数/缴费类别返回订单信息
        /// </summary>
        /// <param name="senrollsProfessionId"></param>
        /// <param name="yearName"></param>
        /// <param name="detailName"></param>
        /// <returns></returns>
        public static sOrderModel GetsOrderModel(string senrollsProfessionId, string yearName, string detailName)
        {

            string where = " and sEnrollsProfessionID=@sEnrollsProfessionID and NumName=@NumName and DetailID=(SELECT  DetailID FROM  T_Pro_Detail WHERE Status=1 AND Name=@Name)  and Status IN (1,2,3)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",senrollsProfessionId),
                new SqlParameter("@NumName",yearName),
                new SqlParameter("@Name",detailName)
            };
            return sOrderBLL.sOrderModelByWhere(where, paras);
        }


        public static sOrderModel GetsOrderModelByWhere(string snerollsProfessionId, string numItemId, string planItemId)
        {
            string where = " and sEnrollsProfessionID=@sEnrollsProfessionID and  NumItemID=@NumItemID and PlanItemID=@PlanItemID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",snerollsProfessionId),
                new SqlParameter("@NumItemID",numItemId),
                new SqlParameter("@PlanItemID",planItemId)
            };
            return sOrderBLL.sOrderModelByWhere(where, paras);
        }

        public static List<sOrderModel> SelectsOrderByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            List<sOrderModel> list = new List<sOrderModel>();
            string cmdText = "SELECT * FROM T_Stu_sOrder WHERE 1 = 1 {0}{1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                sOrderModel om = new sOrderModel();
                om.sOrderID = dr["sOrderID"].ToString();
                om.Status = dr["Status"].ToString();
                om.DeptID = dr["DeptID"].ToString();
                om.sEnrollsProfessionID = dr["sEnrollsProfessionID"].ToString();
                om.PlanItemID = dr["PlanItemID"].ToString();
                om.PlanName = dr["PlanName"].ToString();
                om.PlanSort = dr["PlanSort"].ToString();
                om.PlanLevel = dr["PlanLevel"].ToString();
                om.Year = dr["Year"].ToString();
                om.Month = dr["Month"].ToString();
                om.NumItemID = dr["NumItemID"].ToString();
                om.NumName = dr["NumName"].ToString();
                om.LimitTime = dr["LimitTime"].ToString();
                om.ItemDetailID = dr["ItemDetailID"].ToString();
                om.DetailID = dr["DetailID"].ToString();
                om.Sort = dr["Sort"].ToString();
                om.ShouldMoney = dr["ShouldMoney"].ToString();
                om.PaidMoney = dr["PaidMoney"].ToString();
                om.IsGive = dr["IsGive"].ToString();
                om.ItemQueue = dr["ItemQueue"].ToString();
                om.ItemDetailQueue = dr["ItemDetailQueue"].ToString();
                om.CreateID = dr["CreateID"].ToString();
                om.CreateTime = dr["CreateTime"].ToString();
                om.UpdateID = dr["UpdateID"].ToString();
                om.UpdateTime = dr["UpdateTime"].ToString();
                list.Add(om);
            }
            return list;
        }

        public static List<sOrderModel> GetOrderList(string senrollProfessionId, string planItemId,string numItemId) {
            string where = "  and sEnrollsProfessionID=@sEnrollsProfessionID and PlanItemID=@PlanItemID and NumItemID=@NumItemID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",senrollProfessionId),
                new SqlParameter("@PlanItemID",planItemId),
                new SqlParameter("@NumItemID",numItemId)
            };
            return SelectsOrderByWhere(where, paras, "");
        }

        public static List<sOrderModel> GetOrderList(string senrollProfessionId)
        {
            string where = "  and sEnrollsProfessionID=@sEnrollsProfessionID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",senrollProfessionId)
            };
            return SelectsOrderByWhere(where, paras, "");
        }
    }
}
