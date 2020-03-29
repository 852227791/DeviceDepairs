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
    public class sOrderAddDetailBLL
    {
        public static int InsertsOrderAddDetail(sOrderAddDetailModel oadm)
        {
            string cmdText = @"INSERT INTO T_Stu_sOrderAddDetail
(Status
,sOrderAddID
,DetailID
,Sort
,ShouldMoney
,IsGive
,LimitTime
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@sOrderAddID
,@DetailID
,@Sort
,@ShouldMoney
,@IsGive
,@LimitTime
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", oadm.Status),
new SqlParameter("@sOrderAddID", oadm.sOrderAddID),
new SqlParameter("@DetailID", oadm.DetailID),
new SqlParameter("@Sort", oadm.Sort),
new SqlParameter("@ShouldMoney", oadm.ShouldMoney),
new SqlParameter("@IsGive", oadm.IsGive),
new SqlParameter("@LimitTime", oadm.LimitTime),
new SqlParameter("@CreateID", oadm.CreateID),
new SqlParameter("@CreateTime", oadm.CreateTime),
new SqlParameter("@UpdateID", oadm.UpdateID),
new SqlParameter("@UpdateTime", oadm.UpdateTime)
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

        public static int UpdatesOrderAddDetail(sOrderAddDetailModel oadm)
        {
            string cmdText = @"UPDATE T_Stu_sOrderAddDetail SET
Status=@Status
,sOrderAddID=@sOrderAddID
,DetailID=@DetailID
,Sort=@Sort
,ShouldMoney=@ShouldMoney
,IsGive=@IsGive
,LimitTime=@LimitTime
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sOrderAddDetailID=@sOrderAddDetailID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@sOrderAddDetailID", oadm.sOrderAddDetailID),
new SqlParameter("@Status", oadm.Status),
new SqlParameter("@sOrderAddID", oadm.sOrderAddID),
new SqlParameter("@DetailID", oadm.DetailID),
new SqlParameter("@Sort", oadm.Sort),
new SqlParameter("@ShouldMoney", oadm.ShouldMoney),
new SqlParameter("@IsGive", oadm.IsGive),
new SqlParameter("@LimitTime", oadm.LimitTime),
new SqlParameter("@CreateID", oadm.CreateID),
new SqlParameter("@CreateTime", oadm.CreateTime),
new SqlParameter("@UpdateID", oadm.UpdateID),
new SqlParameter("@UpdateTime", oadm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(oadm.sOrderAddDetailID);
            }
            else
            {
                return -1;
            }
        }

        public static sOrderAddDetailModel sOrderAddDetailModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sOrderAddDetailModel oadm = new sOrderAddDetailModel();
            string cmdText = "SELECT * FROM T_Stu_sOrderAddDetail WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                oadm.sOrderAddDetailID = dt.Rows[0]["sOrderAddDetailID"].ToString();
                oadm.Status = dt.Rows[0]["Status"].ToString();
                oadm.sOrderAddID = dt.Rows[0]["sOrderAddID"].ToString();
                oadm.DetailID = dt.Rows[0]["DetailID"].ToString();
                oadm.Sort = dt.Rows[0]["Sort"].ToString();
                oadm.ShouldMoney = dt.Rows[0]["ShouldMoney"].ToString();
                oadm.IsGive = dt.Rows[0]["IsGive"].ToString();
                oadm.LimitTime = dt.Rows[0]["LimitTime"].ToString();
                oadm.CreateID = dt.Rows[0]["CreateID"].ToString();
                oadm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                oadm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                oadm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return oadm;
        }

        public static DataTable sOrderAddDetailTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sOrderAddDetail WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
