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
    public class sGiveBLL
    {
        public static int InsertsGive(sGiveModel gm)
        {
            string cmdText = @"INSERT INTO T_Stu_sGive
(Status
,DeptID
,Name
,Money
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@DeptID
,@Name
,@Money
,@Remark
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", gm.Status),
new SqlParameter("@DeptID", gm.DeptID),
new SqlParameter("@Name", gm.Name),
new SqlParameter("@Money", gm.Money),
new SqlParameter("@Remark", gm.Remark),
new SqlParameter("@CreateID", gm.CreateID),
new SqlParameter("@CreateTime", gm.CreateTime),
new SqlParameter("@UpdateID", gm.UpdateID),
new SqlParameter("@UpdateTime", gm.UpdateTime)
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

        public static int UpdatesGive(sGiveModel gm)
        {
            string cmdText = @"UPDATE T_Stu_sGive SET
Status=@Status
,DeptID=@DeptID
,Name=@Name
,Money=@Money
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sGiveID=@sGiveID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sGiveID", gm.sGiveID),
new SqlParameter("@Status", gm.Status),
new SqlParameter("@DeptID", gm.DeptID),
new SqlParameter("@Name", gm.Name),
new SqlParameter("@Money", gm.Money),
new SqlParameter("@Remark", gm.Remark),
new SqlParameter("@CreateID", gm.CreateID),
new SqlParameter("@CreateTime", gm.CreateTime),
new SqlParameter("@UpdateID", gm.UpdateID),
new SqlParameter("@UpdateTime", gm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(gm.sGiveID);
            }
            else
            {
                return -1;
            }
        }

        public static sGiveModel sGiveModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sGiveModel gm = new sGiveModel();
            string cmdText = "SELECT * FROM T_Stu_sGive WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                gm.sGiveID = dt.Rows[0]["sGiveID"].ToString();
                gm.Status = dt.Rows[0]["Status"].ToString();
                gm.DeptID = dt.Rows[0]["DeptID"].ToString();
                gm.Name = dt.Rows[0]["Name"].ToString();
                gm.Money = dt.Rows[0]["Money"].ToString();
                gm.Remark = dt.Rows[0]["Remark"].ToString();
                gm.CreateID = dt.Rows[0]["CreateID"].ToString();
                gm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                gm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                gm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return gm;
        }

        public static DataTable sGiveTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sGive WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获取配品下拉
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static DataTable GetsGiveCombobox(string deptId)
        {
            string cmdText = @"SELECT  Name + '-' + convert(nvarchar(18),Money)  text ,
        sGiveID id
FROM    T_Stu_sGive where DeptID=@DeptID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptID",deptId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

    }
}
