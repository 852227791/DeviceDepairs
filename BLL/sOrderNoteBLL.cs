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
    public class sOrderNoteBLL
    {
        public static int InsertsOrderNote(sOrderNoteModel onm)
        {
            string cmdText = @"INSERT INTO T_Stu_sOrderNote
(Status
,sOrderID
,FieldName
,ValueOld
,ValueNew
,Explain
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@sOrderID
,@FieldName
,@ValueOld
,@ValueNew
,@Explain
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", onm.Status),
new SqlParameter("@sOrderID", onm.sOrderID),
new SqlParameter("@FieldName", onm.FieldName),
new SqlParameter("@ValueOld", onm.ValueOld),
new SqlParameter("@ValueNew", onm.ValueNew),
new SqlParameter("@Explain", onm.Explain),
new SqlParameter("@CreateID", onm.CreateID),
new SqlParameter("@CreateTime", onm.CreateTime),
new SqlParameter("@UpdateID", onm.UpdateID),
new SqlParameter("@UpdateTime", onm.UpdateTime)
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

        public static int UpdatesOrderNote(sOrderNoteModel onm)
        {
            string cmdText = @"UPDATE T_Stu_sOrderNote SET
Status=@Status
,sOrderID=@sOrderID
,FieldName=@FieldName
,ValueOld=@ValueOld
,ValueNew=@ValueNew
,Explain=@Explain
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sOrderNoteID=@sOrderNoteID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sOrderNoteID", onm.sOrderNoteID),
new SqlParameter("@Status", onm.Status),
new SqlParameter("@sOrderID", onm.sOrderID),
new SqlParameter("@FieldName", onm.FieldName),
new SqlParameter("@ValueOld", onm.ValueOld),
new SqlParameter("@ValueNew", onm.ValueNew),
new SqlParameter("@Explain", onm.Explain),
new SqlParameter("@CreateID", onm.CreateID),
new SqlParameter("@CreateTime", onm.CreateTime),
new SqlParameter("@UpdateID", onm.UpdateID),
new SqlParameter("@UpdateTime", onm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(onm.sOrderNoteID);
            }
            else
            {
                return -1;
            }
        }

        public static sOrderNoteModel sOrderNoteModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sOrderNoteModel onm = new sOrderNoteModel();
            string cmdText = "SELECT * FROM T_Stu_sOrderNote WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                onm.sOrderNoteID = dt.Rows[0]["sOrderNoteID"].ToString();
                onm.Status = dt.Rows[0]["Status"].ToString();
                onm.sOrderID = dt.Rows[0]["sOrderID"].ToString();
                onm.FieldName = dt.Rows[0]["FieldName"].ToString();
                onm.ValueOld = dt.Rows[0]["ValueOld"].ToString();
                onm.ValueNew = dt.Rows[0]["ValueNew"].ToString();
                onm.Explain = dt.Rows[0]["Explain"].ToString();
                onm.CreateID = dt.Rows[0]["CreateID"].ToString();
                onm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                onm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                onm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return onm;
        }

        public static DataTable sOrderNoteTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sOrderNote WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
