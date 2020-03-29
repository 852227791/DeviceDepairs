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

	public class sOrderCreateBLL
	{
		public static int InsertsOrderCreate(sOrderCreateModel ocm)
		{
 			string cmdText = @"INSERT INTO T_Stu_sOrderCreate
			(DeptID
						)
			VALUES(@DeptID
						);SELECT CAST(scope_identity() AS int)";
			SqlParameter[] paras = new SqlParameter[] {
				new SqlParameter("@DeptID", ocm.DeptID)
							};
			return Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
		}

		public static int UpdatesOrderCreate(sOrderCreateModel ocm)
		{
			string cmdText = @"UPDATE T_Stu_sOrderCreate SET  DeptID=@DeptID
			Where sOrderCreateID=@sOrderCreateID " ;
			SqlParameter[] paras = new SqlParameter[] {
				new SqlParameter("@sOrderCreateID", ocm.sOrderCreateID)
				,new SqlParameter("@DeptID", ocm.DeptID)
				};
			return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
		}
        public static int DeletesOrderCreate(string id)
        {
            string cmdText = @"DELETE FROM dbo.T_Stu_sOrderCreate WHERE sOrderCreateID=@sOrderCreateID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sOrderCreateID", id)
                };
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }
        public static List<sOrderCreateModel> SelectsOrderCreateByWhere(string where, SqlParameter[] paras, string queue)
		{
			DataTable dt = new DataTable();
			List<sOrderCreateModel> list=new List<sOrderCreateModel>();
			string cmdText = "SELECT * FROM T_Stu_sOrderCreate WHERE 1 = 1 {0}{1}";
			cmdText = string.Format(cmdText, where, queue);
			dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				sOrderCreateModel ocm=new sOrderCreateModel();
				ocm.sOrderCreateID = dr["sOrderCreateID"].ToString();
				ocm.DeptID = dr["DeptID"].ToString();
				list.Add(ocm);
			}
			return list;
		}

		public static sOrderCreateModel GetsOrderCreateModel(string sOrderCreateID)
		 {
			string where = " and sOrderCreateID=@sOrderCreateID";
			 SqlParameter[] paras = new SqlParameter[] {
			 new SqlParameter("@sOrderCreateID", sOrderCreateID)
			 };
			return sOrderCreateBLL.SelectsOrderCreateByWhere(where, paras, "").FirstOrDefault();
		 }
			}
}
