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
    public class DisableNoteBLL
    {
        public static int InsertDisableNote(DisableNoteModel dnm)
        {
            string cmdText = @"INSERT INTO T_Pro_DisableNote
(Status
,FeeID
,NoteNum
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@FeeID
,@NoteNum
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", dnm.Status),
new SqlParameter("@FeeID", dnm.FeeID),
new SqlParameter("@NoteNum", dnm.NoteNum),
new SqlParameter("@CreateID", dnm.CreateID),
new SqlParameter("@CreateTime", dnm.CreateTime),
new SqlParameter("@UpdateID", dnm.UpdateID),
new SqlParameter("@UpdateTime", dnm.UpdateTime)
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

        public static int UpdateDisableNote(DisableNoteModel dnm)
        {
            string cmdText = @"UPDATE T_Pro_DisableNote SET
Status=@Status
,FeeID=@FeeID
,NoteNum=@NoteNum
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE DisableNoteID=@DisableNoteID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@DisableNoteID", dnm.DisableNoteID),
new SqlParameter("@Status", dnm.Status),
new SqlParameter("@FeeID", dnm.FeeID),
new SqlParameter("@NoteNum", dnm.NoteNum),
new SqlParameter("@CreateID", dnm.CreateID),
new SqlParameter("@CreateTime", dnm.CreateTime),
new SqlParameter("@UpdateID", dnm.UpdateID),
new SqlParameter("@UpdateTime", dnm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(dnm.DisableNoteID);
            }
            else
            {
                return -1;
            }
        }

        public static DisableNoteModel DisableNoteModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            DisableNoteModel dnm = new DisableNoteModel();
            string cmdText = "SELECT * FROM T_Pro_DisableNote WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dnm.DisableNoteID = dt.Rows[0]["DisableNoteID"].ToString();
                dnm.Status = dt.Rows[0]["Status"].ToString();
                dnm.FeeID = dt.Rows[0]["FeeID"].ToString();
                dnm.NoteNum = dt.Rows[0]["NoteNum"].ToString();
                dnm.CreateID = dt.Rows[0]["CreateID"].ToString();
                dnm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                dnm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                dnm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return dnm;
        }

        public static DataTable DisableNoteTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_DisableNote WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
