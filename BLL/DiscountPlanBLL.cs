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

    public class DiscountPlanBLL
    {
        public static int InsertDiscountPlan(DiscountPlanModel dpm)
        {
            string cmdText = @"INSERT INTO T_Stu_DiscountPlan
			(Status
			,Name
			,PlanItemID
			,CreateID
			,CreateTime
			,UpdateID
			,UpdateTime
						)
			VALUES(@Status
			,@Name
			,@PlanItemID
			,@CreateID
			,@CreateTime
			,@UpdateID
			,@UpdateTime
						);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Status", dpm.Status)
                ,new SqlParameter("@Name", dpm.Name)
                ,new SqlParameter("@PlanItemID", dpm.PlanItemID)
                ,new SqlParameter("@CreateID", dpm.CreateID)
                ,new SqlParameter("@CreateTime", dpm.CreateTime)
                ,new SqlParameter("@UpdateID", dpm.UpdateID)
                ,new SqlParameter("@UpdateTime", dpm.UpdateTime)
                            };
            return Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
        }

        public static int UpdateDiscountPlan(DiscountPlanModel dpm)
        {
            string cmdText = @"UPDATE T_Stu_DiscountPlan SET  Status=@Status
			,Name=@Name
			,PlanItemID=@PlanItemID
			,CreateID=@CreateID
			,CreateTime=@CreateTime
			,UpdateID=@UpdateID
			,UpdateTime=@UpdateTime
			Where DiscountPlanID=@DiscountPlanID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DiscountPlanID", dpm.DiscountPlanID)
                ,new SqlParameter("@Status", dpm.Status)
                ,new SqlParameter("@Name", dpm.Name)
                ,new SqlParameter("@PlanItemID", dpm.PlanItemID)
                ,new SqlParameter("@CreateID", dpm.CreateID)
                ,new SqlParameter("@CreateTime", dpm.CreateTime)
                ,new SqlParameter("@UpdateID", dpm.UpdateID)
                ,new SqlParameter("@UpdateTime", dpm.UpdateTime)
                };
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        public static List<DiscountPlanModel> SelectDiscountPlanByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            List<DiscountPlanModel> list = new List<DiscountPlanModel>();
            string cmdText = "SELECT * FROM T_Stu_DiscountPlan WHERE 1 = 1 {0}{1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                DiscountPlanModel dpm = new DiscountPlanModel();
                dpm.DiscountPlanID = dr["DiscountPlanID"].ToString();
                dpm.Status = dr["Status"].ToString();
                dpm.Name = dr["Name"].ToString();
                dpm.PlanItemID = dr["PlanItemID"].ToString();
                dpm.CreateID = dr["CreateID"].ToString();
                dpm.CreateTime = dr["CreateTime"].ToString();
                dpm.UpdateID = dr["UpdateID"].ToString();
                dpm.UpdateTime = dr["UpdateTime"].ToString();
                list.Add(dpm);
            }
            return list;
        }

        public static bool UpdateDiscountPlanStatus(string DiscountPlanID, string status, int userId)
        {
            string where = " and DiscountPlanID=@DiscountPlanID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DiscountPlanID", DiscountPlanID)
            };
            var list = DiscountPlanBLL.SelectDiscountPlanByWhere(where, paras, "").FirstOrDefault();
            if (list != null)
            {
                list.Status = status;
                list.UpdateTime = DateTime.Now.ToString();
                list.UpdateID = userId.ToString();
                if (DiscountPlanBLL.UpdateDiscountPlan(list).Equals(1))
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取优惠方案下拉列表
        /// </summary>
        /// <param name="planItemId"></param>
        /// <returns></returns>
        public static string GetDiscountPlanCombobox(string planItemId)
        {
            if (string.IsNullOrEmpty(planItemId))
            {
                return string.Empty;
            }
            string cmdText = @"SELECT  '没有优惠方案' text ,
        0 id
		UNION ALL 
		SELECT  Name text ,
        DiscountPlanID id
FROM    T_Stu_DiscountPlan
WHERE   Status = 1
        AND PlanItemID = @PlanItemID";

            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@PlanItemID",planItemId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return JsonHelper.DataTableToJson(dt);
        }

    }
}
