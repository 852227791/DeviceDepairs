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
    public class iDisableNoteBLL
    {
        public static int InsertiDisableNote(iDisableNoteModel dnm)
        {
            string cmdText = @"INSERT INTO T_Inc_iDisableNote
(Status
,iFeeID
,NoteNum
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@iFeeID
,@NoteNum
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", dnm.Status),
new SqlParameter("@iFeeID", dnm.iFeeID),
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

        public static int UpdateiDisableNote(iDisableNoteModel dnm)
        {
            string cmdText = @"UPDATE T_Inc_iDisableNote SET
Status=@Status
,iFeeID=@iFeeID
,NoteNum=@NoteNum
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE iDisableNoteID=@iDisableNoteID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@iDisableNoteID", dnm.iDisableNoteID),
new SqlParameter("@Status", dnm.Status),
new SqlParameter("@iFeeID", dnm.iFeeID),
new SqlParameter("@NoteNum", dnm.NoteNum),
new SqlParameter("@CreateID", dnm.CreateID),
new SqlParameter("@CreateTime", dnm.CreateTime),
new SqlParameter("@UpdateID", dnm.UpdateID),
new SqlParameter("@UpdateTime", dnm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(dnm.iDisableNoteID);
            }
            else
            {
                return -1;
            }
        }

        public static iDisableNoteModel iDisableNoteModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            iDisableNoteModel dnm = new iDisableNoteModel();
            string cmdText = "SELECT * FROM T_Inc_iDisableNote WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dnm.iDisableNoteID = dt.Rows[0]["iDisableNoteID"].ToString();
                dnm.Status = dt.Rows[0]["Status"].ToString();
                dnm.iFeeID = dt.Rows[0]["iFeeID"].ToString();
                dnm.NoteNum = dt.Rows[0]["NoteNum"].ToString();
                dnm.CreateID = dt.Rows[0]["CreateID"].ToString();
                dnm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                dnm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                dnm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return dnm;
        }

        public static DataTable iDisableNoteTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Inc_iDisableNote WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
