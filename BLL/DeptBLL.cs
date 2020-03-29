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
    public class DeptBLL
    {
        public static int InsertDept(DeptModel dm)
        {
            string cmdText = @"INSERT INTO T_Sys_Dept
(Status
,ParentID
,Name
,ShortName
,Code
,Queue
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@ParentID
,@Name
,@ShortName
,@Code
,@Queue
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
new SqlParameter("@ShortName", dm.ShortName),
new SqlParameter("@Code", dm.Code),
new SqlParameter("@Queue", dm.Queue),
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

        public static int UpdateDept(DeptModel dm)
        {
            string cmdText = @"UPDATE T_Sys_Dept SET
Status=@Status
,ParentID=@ParentID
,Name=@Name
,ShortName=@ShortName
,Code=@Code
,Queue=@Queue
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE DeptID=@DeptID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@DeptID", dm.DeptID),
new SqlParameter("@Status", dm.Status),
new SqlParameter("@ParentID", dm.ParentID),
new SqlParameter("@Name", dm.Name),
new SqlParameter("@ShortName", dm.ShortName),
new SqlParameter("@Code", dm.Code),
new SqlParameter("@Queue", dm.Queue),
new SqlParameter("@Remark", dm.Remark),
new SqlParameter("@CreateID", dm.CreateID),
new SqlParameter("@CreateTime", dm.CreateTime),
new SqlParameter("@UpdateID", dm.UpdateID),
new SqlParameter("@UpdateTime", dm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(dm.DeptID);
            }
            else
            {
                return -1;
            }
        }

        public static DeptModel DeptModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            DeptModel dm = new DeptModel();
            string cmdText = "SELECT * FROM T_Sys_Dept WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dm.DeptID = dt.Rows[0]["DeptID"].ToString();
                dm.Status = dt.Rows[0]["Status"].ToString();
                dm.ParentID = dt.Rows[0]["ParentID"].ToString();
                dm.Name = dt.Rows[0]["Name"].ToString();
                dm.ShortName = dt.Rows[0]["ShortName"].ToString();
                dm.Code = dt.Rows[0]["Code"].ToString();
                dm.Queue = dt.Rows[0]["Queue"].ToString();
                dm.Remark = dt.Rows[0]["Remark"].ToString();
                dm.CreateID = dt.Rows[0]["CreateID"].ToString();
                dm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                dm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                dm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return dm;
        }

        public static DataTable DeptTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Sys_Dept WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        /// <summary>
        /// 返回传入部门ID的所有子部门ID列表
        /// </summary>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        public static DataTable SelectChildrenDeptID(string DeptID)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT DeptID FROM GetChildrenDeptID(" + DeptID + ")";
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            return dt;
        }

        /// <summary>
        /// 验证传入的部门ID是否是末节点
        /// </summary>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        public static bool CheckIsLastDeptID(string DeptID)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  DeptID
FROM    T_Sys_Dept
WHERE   Status = 1
        AND ParentID = @DeptID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@DeptID", DeptID)
            };
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 输出部门列表用于报表
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectDeptListToReport(string where)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  d.DeptID ,
        d.ParentID ,
        d.Name ,
        ( SELECT    Queue
          FROM      T_Sys_Dept
          WHERE     DeptID = d.ParentID
        ) ParentQueue
FROM    T_Sys_Dept d
WHERE   1 = 1
        AND d.Status = 1
        AND d.DeptID <> 1
        {0}
ORDER BY ParentQueue ASC ,
        Queue ASC ,
        d.DeptID ASC";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            return dt;
        }
    }
}
