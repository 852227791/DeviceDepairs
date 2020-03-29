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
    public class sEnrollsProfessionNoteBLL
    {
        public static int InsertsEnrollsProfessionNote(sEnrollsProfessionNoteModel epnm)
        {
            string cmdText = @"INSERT INTO T_Stu_sEnrollsProfessionNote
(Status
,sEnrollsProfessionID
,NewsEnrollsProfessionID
,Sort
,NoteTime
,Explain
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@sEnrollsProfessionID
,@NewsEnrollsProfessionID
,@Sort
,@NoteTime
,@Explain
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", epnm.Status),
new SqlParameter("@sEnrollsProfessionID", epnm.sEnrollsProfessionID),
new SqlParameter("@NewsEnrollsProfessionID", epnm.NewsEnrollsProfessionID),
new SqlParameter("@Sort", epnm.Sort),
new SqlParameter("@NoteTime", epnm.NoteTime),
new SqlParameter("@Explain", epnm.Explain),
new SqlParameter("@CreateID", epnm.CreateID),
new SqlParameter("@CreateTime", epnm.CreateTime),
new SqlParameter("@UpdateID", epnm.UpdateID),
new SqlParameter("@UpdateTime", epnm.UpdateTime)
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

        public static int UpdatesEnrollsProfessionNote(sEnrollsProfessionNoteModel epnm)
        {
            string cmdText = @"UPDATE T_Stu_sEnrollsProfessionNote SET
Status=@Status
,sEnrollsProfessionID=@sEnrollsProfessionID
,NewsEnrollsProfessionID=@NewsEnrollsProfessionID
,Sort=@Sort
,NoteTime=@NoteTime
,Explain=@Explain
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sEnrollsProfessionNoteID=@sEnrollsProfessionNoteID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@sEnrollsProfessionNoteID", epnm.sEnrollsProfessionNoteID),
new SqlParameter("@Status", epnm.Status),
new SqlParameter("@sEnrollsProfessionID", epnm.sEnrollsProfessionID),
new SqlParameter("@NewsEnrollsProfessionID", epnm.NewsEnrollsProfessionID),
new SqlParameter("@Sort", epnm.Sort),
new SqlParameter("@NoteTime", epnm.NoteTime),
new SqlParameter("@Explain", epnm.Explain),
new SqlParameter("@CreateID", epnm.CreateID),
new SqlParameter("@CreateTime", epnm.CreateTime),
new SqlParameter("@UpdateID", epnm.UpdateID),
new SqlParameter("@UpdateTime", epnm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(epnm.sEnrollsProfessionNoteID);
            }
            else
            {
                return -1;
            }
        }

        public static sEnrollsProfessionNoteModel sEnrollsProfessionNoteModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sEnrollsProfessionNoteModel epnm = new sEnrollsProfessionNoteModel();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfessionNote WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                epnm.sEnrollsProfessionNoteID = dt.Rows[0]["sEnrollsProfessionNoteID"].ToString();
                epnm.Status = dt.Rows[0]["Status"].ToString();
                epnm.sEnrollsProfessionID = dt.Rows[0]["sEnrollsProfessionID"].ToString();
                epnm.NewsEnrollsProfessionID = dt.Rows[0]["NewsEnrollsProfessionID"].ToString();
                epnm.Sort = dt.Rows[0]["Sort"].ToString();
                epnm.NoteTime = dt.Rows[0]["NoteTime"].ToString();
                epnm.Explain = dt.Rows[0]["Explain"].ToString();
                epnm.CreateID = dt.Rows[0]["CreateID"].ToString();
                epnm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                epnm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                epnm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return epnm;
        }

        public static DataTable sEnrollsProfessionNoteTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfessionNote WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        public static sEnrollsProfessionNoteModel sEnrollsProfessionNoteModelByWhere(string newid) {
            string where = " and  NewsEnrollsProfessionID=@NewsEnrollsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@NewsEnrollsProfessionID",newid)
            };

            return sEnrollsProfessionNoteModelByWhere(where, paras);
        }


    }
}
