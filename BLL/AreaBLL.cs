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
    public class AreaBLL
    {
        public static int InsertArea(AreaModel am)
        {
            string cmdText = @"INSERT INTO T_Pro_Area
(Status
,ParentID
,Name
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@ParentID
,@Name
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", am.Status),
new SqlParameter("@ParentID", am.ParentID),
new SqlParameter("@Name", am.Name),
new SqlParameter("@CreateID", am.CreateID),
new SqlParameter("@CreateTime", am.CreateTime),
new SqlParameter("@UpdateID", am.UpdateID),
new SqlParameter("@UpdateTime", am.UpdateTime)
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

        public static int UpdateArea(AreaModel am)
        {
            string cmdText = @"UPDATE T_Pro_Area SET
Status=@Status
,ParentID=@ParentID
,Name=@Name
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE AreaID=@AreaID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@AreaID", am.AreaID),
new SqlParameter("@Status", am.Status),
new SqlParameter("@ParentID", am.ParentID),
new SqlParameter("@Name", am.Name),
new SqlParameter("@CreateID", am.CreateID),
new SqlParameter("@CreateTime", am.CreateTime),
new SqlParameter("@UpdateID", am.UpdateID),
new SqlParameter("@UpdateTime", am.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(am.AreaID);
            }
            else
            {
                return -1;
            }
        }

        public static AreaModel AreaModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            AreaModel am = new AreaModel();
            string cmdText = "SELECT * FROM T_Pro_Area WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                am.AreaID = dt.Rows[0]["AreaID"].ToString();
                am.Status = dt.Rows[0]["Status"].ToString();
                am.ParentID = dt.Rows[0]["ParentID"].ToString();
                am.Name = dt.Rows[0]["Name"].ToString();
                am.CreateID = dt.Rows[0]["CreateID"].ToString();
                am.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                am.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                am.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return am;
        }

        public static DataTable AreaTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Area WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        public static DataTable AreaTableByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT AreaID id,Name text,ParentID pid FROM T_Pro_Area WHERE 1=1 {0} ";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AreaModel GetAreaModel(string name)
        {
            string where = " and Name=@Name";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Name",name)
            };
            return AreaBLL.AreaModelByWhere(where, paras);
        }
        public static AreaModel GetAreaModel(string name,string parentId)
        {
            string where = " and Name=@Name and ParentID=@PrarentID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Name",name),
                 new SqlParameter("@PrarentID",parentId)
            };
            return AreaBLL.AreaModelByWhere(where, paras);
        }


        public static List<AreaModel> SelectAreaByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            List<AreaModel> list = new List<AreaModel>();
            string cmdText = "SELECT * FROM T_Pro_Area WHERE 1 = 1 {0}{1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                AreaModel am = new AreaModel();
                am.AreaID = dr["AreaID"].ToString();
                am.Status = dr["Status"].ToString();
                am.ParentID = dr["ParentID"].ToString();
                am.Name = dr["Name"].ToString();
                am.CreateID = dr["CreateID"].ToString();
                am.CreateTime = dr["CreateTime"].ToString();
                am.UpdateID = dr["UpdateID"].ToString();
                am.UpdateTime = dr["UpdateTime"].ToString();
                list.Add(am);
            }
            return list;
        }

        public static AreaModel GetAreaModelByAreaID(string AreaID)
        {
            string where = " and AreaID=@AreaID";
            SqlParameter[] paras = new SqlParameter[] {
             new SqlParameter("@AreaID", AreaID)
             };
            return AreaBLL.SelectAreaByWhere(where, paras, "").FirstOrDefault();
        }

        public static bool UpdateAreaStatus(string AreaID, string status, int userId)
        {
            var list = AreaBLL.GetAreaModelByAreaID(AreaID);
            if (list != null)
            {
                list.Status = status;
                list.UpdateTime = DateTime.Now.ToString();
                list.UpdateID = userId.ToString();
                if (AreaBLL.UpdateArea(list)>0)
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
