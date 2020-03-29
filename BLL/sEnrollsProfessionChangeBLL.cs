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

namespace BLL{

	public class sEnrollsProfessionChangeBLL
	{
		public static int InsertsEnrollsProfessionChange(sEnrollsProfessionChangeModel epcm)		{
 			string cmdText = @"INSERT INTO T_Stu_sEnrollsProfessionChange
			(Status
			,sEnrollsProfessionID
			,CreateID
			,CreateTime
			,UpdateID
			,UpdateTime
						)
			VALUES(@Status
			,@sEnrollsProfessionID
			,@CreateID
			,@CreateTime
			,@UpdateID
			,@UpdateTime
						);SELECT CAST(scope_identity() AS int)";
			SqlParameter[] paras = new SqlParameter[] {
				new SqlParameter("@Status", epcm.Status)
				,new SqlParameter("@sEnrollsProfessionID", epcm.sEnrollsProfessionID)
				,new SqlParameter("@CreateID", epcm.CreateID)
				,new SqlParameter("@CreateTime", epcm.CreateTime)
				,new SqlParameter("@UpdateID", epcm.UpdateID)
				,new SqlParameter("@UpdateTime", epcm.UpdateTime)
							};
			return Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
		}

		public static int UpdatesEnrollsProfessionChange(sEnrollsProfessionChangeModel epcm)
		{
			string cmdText = @"UPDATE T_Stu_sEnrollsProfessionChange SET  Status=@Status
			,sEnrollsProfessionID=@sEnrollsProfessionID
			,CreateID=@CreateID
			,CreateTime=@CreateTime
			,UpdateID=@UpdateID
			,UpdateTime=@UpdateTime
			Where sEnrollsProfessionChangeID=@sEnrollsProfessionChangeID " ;
			SqlParameter[] paras = new SqlParameter[] {
				new SqlParameter("@sEnrollsProfessionChangeID", epcm.sEnrollsProfessionChangeID)
				,new SqlParameter("@Status", epcm.Status)
				,new SqlParameter("@sEnrollsProfessionID", epcm.sEnrollsProfessionID)
				,new SqlParameter("@CreateID", epcm.CreateID)
				,new SqlParameter("@CreateTime", epcm.CreateTime)
				,new SqlParameter("@UpdateID", epcm.UpdateID)
				,new SqlParameter("@UpdateTime", epcm.UpdateTime)
				};
			return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
		}

		public static List<sEnrollsProfessionChangeModel> SelectsEnrollsProfessionChangeByWhere(string where, SqlParameter[] paras, string queue)
		{
			DataTable dt = new DataTable();
			List<sEnrollsProfessionChangeModel> list=new List<sEnrollsProfessionChangeModel>();
			string cmdText = "SELECT * FROM T_Stu_sEnrollsProfessionChange WHERE 1 = 1 {0}{1}";
			cmdText = string.Format(cmdText, where, queue);
			dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
			foreach(DataRow dr in dt.Rows)
            {
                sEnrollsProfessionChangeModel epcm = new sEnrollsProfessionChangeModel();
                epcm.sEnrollsProfessionChangeID = dr["sEnrollsProfessionChangeID"].ToString();
				epcm.Status = dr["Status"].ToString();
				epcm.sEnrollsProfessionID = dr["sEnrollsProfessionID"].ToString();
				epcm.CreateID = dr["CreateID"].ToString();
				epcm.CreateTime = dr["CreateTime"].ToString();
				epcm.UpdateID = dr["UpdateID"].ToString();
				epcm.UpdateTime = dr["UpdateTime"].ToString();
				list.Add(epcm);
			}
			return list;
		}

		public static bool UpdatesEnrollsProfessionChangeStatus(string sEnrollsProfessionChangeID,string status,int userId)
		{
			string where=" and sEnrollsProfessionChangeID=@sEnrollsProfessionChangeID";
			SqlParameter[] paras = new SqlParameter[] {
				new SqlParameter("@sEnrollsProfessionChangeID", sEnrollsProfessionChangeID)
			};
			var list=sEnrollsProfessionChangeBLL.SelectsEnrollsProfessionChangeByWhere(where,paras,"").FirstOrDefault();
			if(list!=null)
			{
				list.Status=status;
				list.UpdateTime=DateTime.Now.ToString();
				list.UpdateID=userId.ToString();
				if(sEnrollsProfessionChangeBLL.UpdatesEnrollsProfessionChange(list).Equals(1))
					return true;
				return false;
			}
			else{
				return false;
			}
		}
	}
}
