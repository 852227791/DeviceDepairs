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

	public class DiscountDetailBLL
	{
		public static int InsertDiscountDetail(DiscountDetailModel ddm)		{
 			string cmdText = @"INSERT INTO T_Stu_DiscountDetail
			(Status
			,DiscountPlanID
			,NumItemID
			,ItemDetaiID
			,Money
			,CreateID
			,CreateTime
			,UpdateID
			,UpdateTime
						)
			VALUES(@Status
			,@DiscountPlanID
			,@NumItemID
			,@ItemDetaiID
			,@Money
			,@CreateID
			,@CreateTime
			,@UpdateID
			,@UpdateTime
						);SELECT CAST(scope_identity() AS int)";
			SqlParameter[] paras = new SqlParameter[] {
				new SqlParameter("@Status", ddm.Status)
				,new SqlParameter("@DiscountPlanID", ddm.DiscountPlanID)
				,new SqlParameter("@NumItemID", ddm.NumItemID)
				,new SqlParameter("@ItemDetaiID", ddm.ItemDetaiID)
				,new SqlParameter("@Money", ddm.Money)
				,new SqlParameter("@CreateID", ddm.CreateID)
				,new SqlParameter("@CreateTime", ddm.CreateTime)
				,new SqlParameter("@UpdateID", ddm.UpdateID)
				,new SqlParameter("@UpdateTime", ddm.UpdateTime)
							};
			return Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
		}

		public static int UpdateDiscountDetail(DiscountDetailModel ddm)
		{
			string cmdText = @"UPDATE T_Stu_DiscountDetail SET  Status=@Status
			,DiscountPlanID=@DiscountPlanID
			,NumItemID=@NumItemID
			,ItemDetaiID=@ItemDetaiID
			,Money=@Money
			,CreateID=@CreateID
			,CreateTime=@CreateTime
			,UpdateID=@UpdateID
			,UpdateTime=@UpdateTime
			Where DiscountDetailID=@DiscountDetailID " ;
			SqlParameter[] paras = new SqlParameter[] {
				new SqlParameter("@DiscountDetailID", ddm.DiscountDetailID)
				,new SqlParameter("@Status", ddm.Status)
				,new SqlParameter("@DiscountPlanID", ddm.DiscountPlanID)
				,new SqlParameter("@NumItemID", ddm.NumItemID)
				,new SqlParameter("@ItemDetaiID", ddm.ItemDetaiID)
				,new SqlParameter("@Money", ddm.Money)
				,new SqlParameter("@CreateID", ddm.CreateID)
				,new SqlParameter("@CreateTime", ddm.CreateTime)
				,new SqlParameter("@UpdateID", ddm.UpdateID)
				,new SqlParameter("@UpdateTime", ddm.UpdateTime)
				};
			return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
		}

		public static List<DiscountDetailModel> SelectDiscountDetailByWhere(string where, SqlParameter[] paras, string queue)
		{
			DataTable dt = new DataTable();
			List<DiscountDetailModel> list=new List<DiscountDetailModel>();
			string cmdText = "SELECT * FROM T_Stu_DiscountDetail WHERE 1 = 1 {0}{1}";
			cmdText = string.Format(cmdText, where, queue);
			dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				DiscountDetailModel ddm=new DiscountDetailModel();
				ddm.DiscountDetailID = dr["DiscountDetailID"].ToString();
				ddm.Status = dr["Status"].ToString();
				ddm.DiscountPlanID = dr["DiscountPlanID"].ToString();
				ddm.NumItemID = dr["NumItemID"].ToString();
				ddm.ItemDetaiID = dr["ItemDetaiID"].ToString();
				ddm.Money = dr["Money"].ToString();
				ddm.CreateID = dr["CreateID"].ToString();
				ddm.CreateTime = dr["CreateTime"].ToString();
				ddm.UpdateID = dr["UpdateID"].ToString();
				ddm.UpdateTime = dr["UpdateTime"].ToString();
				list.Add(ddm);
			}
			return list;
		}

		public static bool UpdateDiscountDetailStatus(string DiscountDetailID,string status,int userId)
		{
			string where=" and DiscountDetailID=@DiscountDetailID";
			SqlParameter[] paras = new SqlParameter[] {
				new SqlParameter("@DiscountDetailID", DiscountDetailID)
			};
			var list=SelectDiscountDetailByWhere(where,paras,"").FirstOrDefault();
			if(list!=null)
			{
				list.Status=status;
				list.UpdateTime=DateTime.Now.ToString();
				list.UpdateID=userId.ToString();
				if(UpdateDiscountDetail(list).Equals(1))
					return true;
				return false;
			}
			else{
				return false;
			}
		}

        public static DataTable GetDiscountDetailEdit(string PlanItemID) {
            string cmdText = @"SELECT  dd.DiscountDetailID ,
        dd.ItemDetaiID ,
        dd.NumItemID ItemID ,
        i.Name NumName ,
        dl.Name ,
        id.Money ShouldMoney ,
        dd.Money DiscountMoney
FROM     T_Stu_DiscountDetail dd
        LEFT JOIN  T_Pro_Item i ON i.ItemID = dd.NumItemID
        LEFT JOIN  T_Pro_ItemDetail id ON id.ItemDetailID = dd.ItemDetaiID
        LEFT JOIN  T_Pro_Detail dl ON dl.DetailID = id.DetailID
		WHERE  dd.DiscountPlanID=@DiscountPlanID Order By  i.Name ASC";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DiscountPlanID",PlanItemID)
            };
            return  DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        public static bool DeleteDiscountDetail(string idString) {
            string cmdText = "DELETE FROM T_Stu_DiscountDetail WHERE DiscountDetailID IN ("+ idString + ")";
            int result= DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, null);
            if (result>0)
            {
                return true;
            }
            return false;
        }
	}
}
