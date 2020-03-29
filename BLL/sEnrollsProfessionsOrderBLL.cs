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
    public class sEnrollsProfessionsOrderBLL
    {
        public static int InsertsEnrollsProfessionsOrder(sEnrollsProfessionsOrderModel epom)
        {
            string cmdText = @"INSERT INTO T_Stu_sEnrollsProfessionsOrder
			(sEnrollsProfessionID
			,sOrderID
			,IsNumItem
						)
			VALUES(@sEnrollsProfessionID
			,@sOrderID
			,@IsNumItem
						);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID", epom.sEnrollsProfessionID)
                ,new SqlParameter("@sOrderID", epom.sOrderID)
                ,new SqlParameter("@IsNumItem", epom.IsNumItem)
                            };
            return Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
        }

        public static int UpdatesEnrollsProfessionsOrder(sEnrollsProfessionsOrderModel epom)
        {
            string cmdText = @"UPDATE T_Stu_sEnrollsProfessionsOrder SET  sEnrollsProfessionID=@sEnrollsProfessionID
			,sOrderID=@sOrderID
			,IsNumItem=@IsNumItem
			Where sEnrollsProfessionsOrderID=@sEnrollsProfessionsOrderID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionsOrderID", epom.sEnrollsProfessionsOrderID)
                ,new SqlParameter("@sEnrollsProfessionID", epom.sEnrollsProfessionID)
                ,new SqlParameter("@sOrderID", epom.sOrderID)
                ,new SqlParameter("@IsNumItem", epom.IsNumItem)
                };
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        public static List<sEnrollsProfessionsOrderModel> SelectsEnrollsProfessionsOrderByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            List<sEnrollsProfessionsOrderModel> list = new List<sEnrollsProfessionsOrderModel>();
            sEnrollsProfessionsOrderModel epom = new sEnrollsProfessionsOrderModel();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfessionsOrder WHERE 1 = 1 {0}{1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                epom.sEnrollsProfessionsOrderID = dr["sEnrollsProfessionsOrderID"].ToString();
                epom.sEnrollsProfessionID = dr["sEnrollsProfessionID"].ToString();
                epom.sOrderID = dr["sOrderID"].ToString();
                epom.IsNumItem = dr["IsNumItem"].ToString();
                list.Add(epom);
            }
            return list;
        }

        public static sEnrollsProfessionsOrderModel sEnrollsProfessionsOrderModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sEnrollsProfessionsOrderModel epom = new sEnrollsProfessionsOrderModel();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfessionsOrder WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                epom.sEnrollsProfessionsOrderID = dt.Rows[0]["sEnrollsProfessionsOrderID"].ToString();
                epom.sEnrollsProfessionID = dt.Rows[0]["sEnrollsProfessionID"].ToString();
                epom.sOrderID = dt.Rows[0]["sOrderID"].ToString();
                epom.IsNumItem = dt.Rows[0]["IsNumItem"].ToString();
            }
            return epom;
        }

        public static DataTable sEnrollsProfessionsOrderTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfessionsOrder WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 查询缴费单
        /// </summary>
        /// <param name="senrollsProfessionId"></param>
        /// <returns></returns>
        public static bool ValidatesOrderId(string senrollsProfessionId)
        {

            string where = " and sEnrollsProfessionID=@sEnrollsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {

                new SqlParameter("@sEnrollsProfessionID",senrollsProfessionId)
            };
            DataTable dt = sEnrollsProfessionsOrderBLL.sEnrollsProfessionsOrderTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
