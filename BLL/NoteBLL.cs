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
    public class NoteBLL
    {
        public static int InsertNote(NoteModel nm)
        {
            string cmdText = @"INSERT INTO T_Pro_Note
(Status
,DeptID
,Sort
,InFile
,OutFile
,SuccessNum
,ErrorNum
,CreateID
,CreateTime
)
VALUES (@Status
,@DeptID
,@Sort
,@InFile
,@OutFile
,@SuccessNum
,@ErrorNum
,@CreateID
,@CreateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", nm.Status),
new SqlParameter("@DeptID", nm.DeptID),
new SqlParameter("@Sort", nm.Sort),
new SqlParameter("@InFile", nm.InFile),
new SqlParameter("@OutFile", nm.OutFile),
new SqlParameter("@SuccessNum", nm.SuccessNum),
new SqlParameter("@ErrorNum", nm.ErrorNum),
new SqlParameter("@CreateID", nm.CreateID),
new SqlParameter("@CreateTime", nm.CreateTime)
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

        public static int UpdateNote(NoteModel nm)
        {
            string cmdText = @"UPDATE T_Pro_Note SET
Status=@Status
,DeptID=@DeptID
,Sort=@Sort
,InFile=@InFile
,OutFile=@OutFile
,SuccessNum=@SuccessNum
,ErrorNum=@ErrorNum
,CreateID=@CreateID
,CreateTime=@CreateTime
WHERE NoteID=@NoteID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@NoteID", nm.NoteID),
new SqlParameter("@Status", nm.Status),
new SqlParameter("@DeptID", nm.DeptID),
new SqlParameter("@Sort", nm.Sort),
new SqlParameter("@InFile", nm.InFile),
new SqlParameter("@OutFile", nm.OutFile),
new SqlParameter("@SuccessNum", nm.SuccessNum),
new SqlParameter("@ErrorNum", nm.ErrorNum),
new SqlParameter("@CreateID", nm.CreateID),
new SqlParameter("@CreateTime", nm.CreateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(nm.NoteID);
            }
            else
            {
                return -1;
            }
        }

        public static NoteModel NoteModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            NoteModel nm = new NoteModel();
            string cmdText = "SELECT * FROM T_Pro_Note WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                nm.NoteID = dt.Rows[0]["NoteID"].ToString();
                nm.Status = dt.Rows[0]["Status"].ToString();
                nm.DeptID = dt.Rows[0]["DeptID"].ToString();
                nm.Sort = dt.Rows[0]["Sort"].ToString();
                nm.InFile = dt.Rows[0]["InFile"].ToString();
                nm.OutFile = dt.Rows[0]["OutFile"].ToString();
                nm.SuccessNum = dt.Rows[0]["SuccessNum"].ToString();
                nm.ErrorNum = dt.Rows[0]["ErrorNum"].ToString();
                nm.CreateID = dt.Rows[0]["CreateID"].ToString();
                nm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
            }
            return nm;
        }

        public static DataTable NoteTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Note WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
