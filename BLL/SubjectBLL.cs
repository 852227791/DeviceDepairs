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
    public class SubjectBLL
    {
        public static int InsertSubject(SubjectModel sm)
        {
            string cmdText = @"INSERT INTO T_Pro_Subject
(Status
,ParentID
,Name
,EnglishName
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@ParentID
,@Name
,@EnglishName
,@Remark
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", sm.Status),
new SqlParameter("@ParentID", sm.ParentID),
new SqlParameter("@Name", sm.Name),
new SqlParameter("@EnglishName", sm.EnglishName),
new SqlParameter("@Remark", sm.Remark),
new SqlParameter("@CreateID", sm.CreateID),
new SqlParameter("@CreateTime", sm.CreateTime),
new SqlParameter("@UpdateID", sm.UpdateID),
new SqlParameter("@UpdateTime", sm.UpdateTime)
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

        public static int UpdateSubject(SubjectModel sm)
        {
            string cmdText = @"UPDATE T_Pro_Subject SET
Status=@Status
,ParentID=@ParentID
,Name=@Name
,EnglishName=@EnglishName
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE SubjectID=@SubjectID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@SubjectID", sm.SubjectID),
new SqlParameter("@Status", sm.Status),
new SqlParameter("@ParentID", sm.ParentID),
new SqlParameter("@Name", sm.Name),
new SqlParameter("@EnglishName", sm.EnglishName),
new SqlParameter("@Remark", sm.Remark),
new SqlParameter("@CreateID", sm.CreateID),
new SqlParameter("@CreateTime", sm.CreateTime),
new SqlParameter("@UpdateID", sm.UpdateID),
new SqlParameter("@UpdateTime", sm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(sm.SubjectID);
            }
            else
            {
                return -1;
            }
        }

        public static SubjectModel SubjectModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            SubjectModel sm = new SubjectModel();
            string cmdText = "SELECT * FROM T_Pro_Subject WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                sm.SubjectID = dt.Rows[0]["SubjectID"].ToString();
                sm.Status = dt.Rows[0]["Status"].ToString();
                sm.ParentID = dt.Rows[0]["ParentID"].ToString();
                sm.Name = dt.Rows[0]["Name"].ToString();
                sm.EnglishName = dt.Rows[0]["EnglishName"].ToString();
                sm.Remark = dt.Rows[0]["Remark"].ToString();
                sm.CreateID = dt.Rows[0]["CreateID"].ToString();
                sm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                sm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                sm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return sm;
        }

        public static DataTable SubjectTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Subject WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获取会计科目实体
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public static SubjectModel SubjectModelByWhere(string subjectId)
        {
            string where = " and SubjectID=@SubjectID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@SubjectID",subjectId)
            };
            return SubjectBLL.SubjectModelByWhere(where, paras);
        }
        /// <summary>
        /// 获取会计科目的所有子节点
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public static DataTable SelectChildSubject(string subjectId)
        {
            string cmdText = "SELECT SubjectID FROM GetChildrenSubjectID(" + subjectId + ")";
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
        }
        /// <summary>
        /// 验证会计科目子父级
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static bool CheckSubject(string subjectId, string parentId)
        {
            string itemString = ",";
            DataTable dt = SubjectBLL.SelectChildSubject(subjectId);
            foreach (DataRow dr in dt.Rows)
            {
                itemString += dr["SubjectID"].ToString() + ",";
            }

            if (itemString.IndexOf("," + parentId + ",") > -1)
                return true;
            else
                return false;
        }
    }
}
