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
    public class DetailSubjectBLL
    {
        public static int InsertDetailSubject(DetailSubjectModel dsm)
        {
            string cmdText = @"INSERT INTO T_Pro_DetailSubject
(DetailID
,SubjectID
)
VALUES (@DetailID
,@SubjectID
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@DetailID", dsm.DetailID),
new SqlParameter("@SubjectID", dsm.SubjectID)
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

        public static int UpdateDetailSubject(DetailSubjectModel dsm)
        {
            string cmdText = @"UPDATE T_Pro_DetailSubject SET
DetailID=@DetailID
,SubjectID=@SubjectID
WHERE DetailSubjectID=@DetailSubjectID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@DetailSubjectID", dsm.DetailSubjectID),
new SqlParameter("@DetailID", dsm.DetailID),
new SqlParameter("@SubjectID", dsm.SubjectID)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(dsm.DetailSubjectID);
            }
            else
            {
                return -1;
            }
        }

        public static DetailSubjectModel DetailSubjectModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            DetailSubjectModel dsm = new DetailSubjectModel();
            string cmdText = "SELECT * FROM T_Pro_DetailSubject WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dsm.DetailSubjectID = dt.Rows[0]["DetailSubjectID"].ToString();
                dsm.DetailID = dt.Rows[0]["DetailID"].ToString();
                dsm.SubjectID = dt.Rows[0]["SubjectID"].ToString();
            }
            return dsm;
        }

        public static DataTable DetailSubjectTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_DetailSubject WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
       /// <summary>
       /// 删除
       /// </summary>
       /// <param name="dsm"></param>
       /// <returns></returns>
        public static int DeleteDetailSubject(string detailId)
        {
            string cmdText = @"DELETE FROM T_Pro_DetailSubject WHERE DetailID=@DetailID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@DetailID", detailId)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }
    }
}
