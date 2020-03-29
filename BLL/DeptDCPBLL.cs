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
  public  class DeptDCPBLL
    {
        public static int InsertDeptDCP(DeptDCPModel ddcpm)
        {
            string cmdText = @"INSERT INTO T_Stu_DeptDCP
(DeptID
,DCPID
)
VALUES (@DeptID
,@DCPID
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@DeptID", ddcpm.DeptID),
new SqlParameter("@DCPID", ddcpm.DCPID)
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

        public static int UpdateDeptDCP(DeptDCPModel ddcpm)
        {
            string cmdText = @"UPDATE T_Stu_DeptDCP SET
DeptID=@DeptID
,DCPID=@DCPID
WHERE DeptDCPID=@DeptDCPID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@DeptDCPID", ddcpm.DeptDCPID),
new SqlParameter("@DeptID", ddcpm.DeptID),
new SqlParameter("@DCPID", ddcpm.DCPID)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(ddcpm.DeptDCPID);
            }
            else
            {
                return -1;
            }
        }

        public static DeptDCPModel DeptDCPModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            DeptDCPModel ddcpm = new DeptDCPModel();
            string cmdText = "SELECT * FROM T_Stu_DeptDCP WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ddcpm.DeptDCPID = dt.Rows[0]["DeptDCPID"].ToString();
                ddcpm.DeptID = dt.Rows[0]["DeptID"].ToString();
                ddcpm.DCPID = dt.Rows[0]["DCPID"].ToString();
            }
            return ddcpm;
        }

        public static DataTable DeptDCPTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_DeptDCP WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static long GetDeptID(string dcpDetpId)
        {
            string where = " and DCPID=@DCPID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DCPID",dcpDetpId)
            };
            string dept= DeptDCPBLL.DeptDCPModelByWhere(where, paras).DeptID;
            if (string.IsNullOrEmpty(dept))
            {
                return 0;
            }
            else
            {
                return long.Parse(dept);
            }
        }


        public static long GetDcpDeptID(string DetpId)
        {
            string where = " and DeptID=@DeptID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptID",DetpId)
            };
            string dept = DeptDCPBLL.DeptDCPModelByWhere(where, paras).DCPID;
            if (string.IsNullOrEmpty(dept))
            {
                return 0;
            }
            else
            {
                return long.Parse(dept);
            }
        }
    }
}
