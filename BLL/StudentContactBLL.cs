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
    public class StudentContactBLL
    {
        public static int InsertStudentContact(StudentContactModel scm)
        {
            string cmdText = @"INSERT INTO T_Pro_StudentContact
(Status
,StudentID
,Name
,Tel
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@StudentID
,@Name
,@Tel
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", scm.Status),
new SqlParameter("@StudentID", scm.StudentID),
new SqlParameter("@Name", scm.Name),
new SqlParameter("@Tel", scm.Tel),
new SqlParameter("@CreateID", scm.CreateID),
new SqlParameter("@CreateTime", scm.CreateTime),
new SqlParameter("@UpdateID", scm.UpdateID),
new SqlParameter("@UpdateTime", scm.UpdateTime)
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

        public static int UpdateStudentContact(StudentContactModel scm)
        {
            string cmdText = @"UPDATE T_Pro_StudentContact SET
Status=@Status
,StudentID=@StudentID
,Name=@Name
,Tel=@Tel
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE StudentContactID=@StudentContactID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@StudentContactID", scm.StudentContactID),
new SqlParameter("@Status", scm.Status),
new SqlParameter("@StudentID", scm.StudentID),
new SqlParameter("@Name", scm.Name),
new SqlParameter("@Tel", scm.Tel),
new SqlParameter("@CreateID", scm.CreateID),
new SqlParameter("@CreateTime", scm.CreateTime),
new SqlParameter("@UpdateID", scm.UpdateID),
new SqlParameter("@UpdateTime", scm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(scm.StudentContactID);
            }
            else
            {
                return -1;
            }
        }

        public static StudentContactModel StudentContactModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            StudentContactModel scm = new StudentContactModel();
            string cmdText = "SELECT * FROM T_Pro_StudentContact WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                scm.StudentContactID = dt.Rows[0]["StudentContactID"].ToString();
                scm.Status = dt.Rows[0]["Status"].ToString();
                scm.StudentID = dt.Rows[0]["StudentID"].ToString();
                scm.Name = dt.Rows[0]["Name"].ToString();
                scm.Tel = dt.Rows[0]["Tel"].ToString();
                scm.CreateID = dt.Rows[0]["CreateID"].ToString();
                scm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                scm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                scm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return scm;
        }

        public static DataTable StudentContactTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_StudentContact WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static StudentContactModel GetStudentContactModel(string studentId)
        {
            string where = " and Status=1  and StudentID=@StudentID Order By StudentContactID ASC  ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",studentId)
            };
            return StudentContactBLL.StudentContactModelByWhere(where, paras);
        }
    }
}
