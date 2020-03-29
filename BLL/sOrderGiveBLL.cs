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
    public class sOrderGiveBLL
    {
        public static int InsertsOrderGive(sOrderGiveModel ogm)
        {
            string cmdText = @"INSERT INTO T_Stu_sOrderGive
(Status
,DeptID
,sEnrollsProfessionID
,PlanItemID
,Year
,Month
,sGiveID
,Queue
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@DeptID
,@sEnrollsProfessionID
,@PlanItemID
,@Year
,@Month
,@sGiveID
,@Queue
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", ogm.Status),
new SqlParameter("@DeptID", ogm.DeptID),
new SqlParameter("@sEnrollsProfessionID", ogm.sEnrollsProfessionID),
new SqlParameter("@PlanItemID", ogm.PlanItemID),
new SqlParameter("@Year", ogm.Year),
new SqlParameter("@Month", ogm.Month),
new SqlParameter("@sGiveID", ogm.sGiveID),
new SqlParameter("@Queue", ogm.Queue),
new SqlParameter("@CreateID", ogm.CreateID),
new SqlParameter("@CreateTime", ogm.CreateTime),
new SqlParameter("@UpdateID", ogm.UpdateID),
new SqlParameter("@UpdateTime", ogm.UpdateTime)
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

        public static int UpdatesOrderGive(sOrderGiveModel ogm)
        {
            string cmdText = @"UPDATE T_Stu_sOrderGive SET
Status=@Status
,DeptID=@DeptID
,sEnrollsProfessionID=@sEnrollsProfessionID
,PlanItemID=@PlanItemID
,Year=@Year
,Month=@Month
,sGiveID=@sGiveID
,Queue=@Queue
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sOrderGiveID=@sOrderGiveID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sOrderGiveID", ogm.sOrderGiveID),
new SqlParameter("@Status", ogm.Status),
new SqlParameter("@DeptID", ogm.DeptID),
new SqlParameter("@sEnrollsProfessionID", ogm.sEnrollsProfessionID),
new SqlParameter("@PlanItemID", ogm.PlanItemID),
new SqlParameter("@Year", ogm.Year),
new SqlParameter("@Month", ogm.Month),
new SqlParameter("@sGiveID", ogm.sGiveID),
new SqlParameter("@Queue", ogm.Queue),
new SqlParameter("@CreateID", ogm.CreateID),
new SqlParameter("@CreateTime", ogm.CreateTime),
new SqlParameter("@UpdateID", ogm.UpdateID),
new SqlParameter("@UpdateTime", ogm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(ogm.sOrderGiveID);
            }
            else
            {
                return -1;
            }
        }

        public static sOrderGiveModel sOrderGiveModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sOrderGiveModel ogm = new sOrderGiveModel();
            string cmdText = "SELECT * FROM T_Stu_sOrderGive WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ogm.sOrderGiveID = dt.Rows[0]["sOrderGiveID"].ToString();
                ogm.Status = dt.Rows[0]["Status"].ToString();
                ogm.DeptID = dt.Rows[0]["DeptID"].ToString();
                ogm.sEnrollsProfessionID = dt.Rows[0]["sEnrollsProfessionID"].ToString();
                ogm.PlanItemID = dt.Rows[0]["PlanItemID"].ToString();
                ogm.Year = dt.Rows[0]["Year"].ToString();
                ogm.Month = dt.Rows[0]["Month"].ToString();
                ogm.sGiveID = dt.Rows[0]["sGiveID"].ToString();
                ogm.Queue = dt.Rows[0]["Queue"].ToString();
                ogm.CreateID = dt.Rows[0]["CreateID"].ToString();
                ogm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                ogm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                ogm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return ogm;
        }

        public static DataTable sOrderGiveTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sOrderGive WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        /// <summary>
        /// 修改状态，根据sOrderGiveID
        /// </summary>
        /// <param name="sOrderGiveIDs">可能有多个ID（1,2,3）</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static int UpdateStatusBysOrderGiveIDs(string sOrderGiveIDs, string status)
        {
            string cmdText = @"UPDATE T_Stu_sOrderGive SET Status=@Status WHERE sOrderGiveID in (" + sOrderGiveIDs + ")";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", status)
};
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }
      
    }
}
