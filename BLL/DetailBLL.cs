using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Model;
using DAL;
using System.Data;
using Common;

namespace BLL
{
    public class DetailBLL
    {
        public static int InsertDetail(DetailModel dm)
        {
            string cmdText = @"INSERT INTO T_Pro_Detail
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
new SqlParameter("@Status", dm.Status),
new SqlParameter("@ParentID", dm.ParentID),
new SqlParameter("@Name", dm.Name),
new SqlParameter("@EnglishName", dm.EnglishName),
new SqlParameter("@Remark", dm.Remark),
new SqlParameter("@CreateID", dm.CreateID),
new SqlParameter("@CreateTime", dm.CreateTime),
new SqlParameter("@UpdateID", dm.UpdateID),
new SqlParameter("@UpdateTime", dm.UpdateTime)
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

        public static int UpdateDetail(DetailModel dm)
        {
            string cmdText = @"UPDATE T_Pro_Detail SET
Status=@Status
,ParentID=@ParentID
,Name=@Name
,EnglishName=@EnglishName
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE DetailID=@DetailID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@DetailID", dm.DetailID),
new SqlParameter("@Status", dm.Status),
new SqlParameter("@ParentID", dm.ParentID),
new SqlParameter("@Name", dm.Name),
new SqlParameter("@EnglishName", dm.EnglishName),
new SqlParameter("@Remark", dm.Remark),
new SqlParameter("@CreateID", dm.CreateID),
new SqlParameter("@CreateTime", dm.CreateTime),
new SqlParameter("@UpdateID", dm.UpdateID),
new SqlParameter("@UpdateTime", dm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(dm.DetailID);
            }
            else
            {
                return -1;
            }
        }

        public static DetailModel DetailModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            DetailModel dm = new DetailModel();
            string cmdText = "SELECT * FROM T_Pro_Detail WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dm.DetailID = dt.Rows[0]["DetailID"].ToString();
                dm.Status = dt.Rows[0]["Status"].ToString();
                dm.ParentID = dt.Rows[0]["ParentID"].ToString();
                dm.Name = dt.Rows[0]["Name"].ToString();
                dm.EnglishName = dt.Rows[0]["EnglishName"].ToString();
                dm.Remark = dt.Rows[0]["Remark"].ToString();
                dm.CreateID = dt.Rows[0]["CreateID"].ToString();
                dm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                dm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                dm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return dm;
        }

        public static DataTable DetailTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Detail WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        /// <summary>
        /// 返回传入费用类别ID的所有子类别ID列表
        /// </summary>
        /// <param name="DetailID"></param>
        /// <returns></returns>
        public static DataTable SelectChildrenDetailID(string DetailID)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT DetailID FROM GetChildrenDetailID(" + DetailID + ")";
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            return dt;
        }

        /// <summary>
        /// 生成下拉菜单
        /// </summary>
        /// <returns></returns>
        public static string GetDetailCommbox()
        {
            string cmdText = @" SELECT  d1.DetailID id ,
        d1.EnglishName+' '+ d1.Name text,
		d1.ParentID
FROM     T_Pro_Detail d1
WHERE   d1.Status = 1 ORDER BY  d1.EnglishName ASC ";
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }

    }
}
