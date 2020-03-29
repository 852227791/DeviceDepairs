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
    public class DeptAreaBLL
    {
        public static int InsertDeptArea(DeptAreaModel dam)
        {
            string cmdText = @"INSERT INTO T_Pro_DeptArea
(Status
,DeptID
,Name
,Queue
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@DeptID
,@Name
,@Queue
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", dam.Status),
new SqlParameter("@DeptID", dam.DeptID),
new SqlParameter("@Name", dam.Name),
new SqlParameter("@Queue", dam.Queue),
new SqlParameter("@CreateID", dam.CreateID),
new SqlParameter("@CreateTime", dam.CreateTime),
new SqlParameter("@UpdateID", dam.UpdateID),
new SqlParameter("@UpdateTime", dam.UpdateTime)
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

        public static int UpdateDeptArea(DeptAreaModel dam)
        {
            string cmdText = @"UPDATE T_Pro_DeptArea SET
Status=@Status
,DeptID=@DeptID
,Name=@Name
,Queue=@Queue
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE DeptAreaID=@DeptAreaID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@DeptAreaID", dam.DeptAreaID),
new SqlParameter("@Status", dam.Status),
new SqlParameter("@DeptID", dam.DeptID),
new SqlParameter("@Name", dam.Name),
new SqlParameter("@Queue", dam.Queue),
new SqlParameter("@CreateID", dam.CreateID),
new SqlParameter("@CreateTime", dam.CreateTime),
new SqlParameter("@UpdateID", dam.UpdateID),
new SqlParameter("@UpdateTime", dam.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(dam.DeptAreaID);
            }
            else
            {
                return -1;
            }
        }

        public static DeptAreaModel DeptAreaModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            DeptAreaModel dam = new DeptAreaModel();
            string cmdText = "SELECT * FROM T_Pro_DeptArea WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dam.DeptAreaID = dt.Rows[0]["DeptAreaID"].ToString();
                dam.Status = dt.Rows[0]["Status"].ToString();
                dam.DeptID = dt.Rows[0]["DeptID"].ToString();
                dam.Name = dt.Rows[0]["Name"].ToString();
                dam.Queue = dt.Rows[0]["Queue"].ToString();
                dam.CreateID = dt.Rows[0]["CreateID"].ToString();
                dam.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                dam.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                dam.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return dam;
        }

        public static DataTable DeptAreaTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_DeptArea WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 根据id获取校区信息
        /// </summary>
        /// <param name="deptAreaId"></param>
        /// <returns></returns>
        public static DeptAreaModel DeptAreaModelByWhere(string deptAreaId)
        {
            string where = " and DeptAreaID=@DeptAreaID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptAreaID",deptAreaId)
            };
            return DeptAreaBLL.DeptAreaModelByWhere(where, paras);
        }
        /// <summary>
        /// 获取校区下拉列表
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static DataTable GetDeptAreaCombobox(string deptId)
        {
            if (string.IsNullOrEmpty(deptId))
                return null;
            string cmdText = "SELECT DeptAreaID id ,Name name,Queue FROM T_Pro_DeptArea  WHERE Status=1 AND DeptID=@DeptID ORDER BY Queue ASC ";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@DeptID",deptId)};
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        public static string GetFirstDeptArea(string deptId)
        {
            string where = " and DeptID=@DeptID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptID",deptId)
            };
            DataTable dt = DeptAreaBLL.DeptAreaTableByWhere(where, paras, " Order BY Queue ASC");
            return dt.Rows[0]["DeptAreaID"].ToString();
        }

        public static string GetFirstDeptArea(string deptId, string deptAreaName)
        {
            string where = " and DeptID=@DeptID and Name=@Name ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptID",deptId),
                  new SqlParameter("@Name",deptAreaName)
            };
            DataTable dt = DeptAreaBLL.DeptAreaTableByWhere(where, paras, " Order BY Queue ASC");
            if (dt.Rows.Count>0)
            {
                return dt.Rows[0]["DeptAreaID"].ToString();
            }
            else
            {
                return string.Empty;
            }
           
        }
    }

}
