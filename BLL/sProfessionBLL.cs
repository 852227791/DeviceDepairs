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
    public class sProfessionBLL
    {
        public static int InsertsProfession(sProfessionModel pm)
        {
            string cmdText = @"INSERT INTO T_Stu_sProfession
(Status
,DeptID
,Year
,Month
,ProfessionID
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@DeptID
,@Year
,@Month
,@ProfessionID
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", pm.Status),
new SqlParameter("@DeptID", pm.DeptID),
new SqlParameter("@Year", pm.Year),
new SqlParameter("@Month", pm.Month),
new SqlParameter("@ProfessionID", pm.ProfessionID),
new SqlParameter("@CreateID", pm.CreateID),
new SqlParameter("@CreateTime", pm.CreateTime),
new SqlParameter("@UpdateID", pm.UpdateID),
new SqlParameter("@UpdateTime", pm.UpdateTime)
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

        public static int UpdatesProfession(sProfessionModel pm)
        {
            string cmdText = @"UPDATE T_Stu_sProfession SET
Status=@Status
,DeptID=@DeptID
,Year=@Year
,Month=@Month
,ProfessionID=@ProfessionID
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE sProfessionID=@sProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sProfessionID", pm.sProfessionID),
new SqlParameter("@Status", pm.Status),
new SqlParameter("@DeptID", pm.DeptID),
new SqlParameter("@Year", pm.Year),
new SqlParameter("@Month", pm.Month),
new SqlParameter("@ProfessionID", pm.ProfessionID),
new SqlParameter("@CreateID", pm.CreateID),
new SqlParameter("@CreateTime", pm.CreateTime),
new SqlParameter("@UpdateID", pm.UpdateID),
new SqlParameter("@UpdateTime", pm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(pm.sProfessionID);
            }
            else
            {
                return -1;
            }
        }

        public static sProfessionModel sProfessionModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sProfessionModel pm = new sProfessionModel();
            string cmdText = "SELECT * FROM T_Stu_sProfession WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                pm.sProfessionID = dt.Rows[0]["sProfessionID"].ToString();
                pm.Status = dt.Rows[0]["Status"].ToString();
                pm.DeptID = dt.Rows[0]["DeptID"].ToString();
                pm.Year = dt.Rows[0]["Year"].ToString();
                pm.Month = dt.Rows[0]["Month"].ToString();
                pm.ProfessionID = dt.Rows[0]["ProfessionID"].ToString();
                pm.CreateID = dt.Rows[0]["CreateID"].ToString();
                pm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                pm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                pm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return pm;
        }

        public static DataTable sProfessionTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sProfession WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获取招生专业下拉列表数据
        /// </summary>
        /// <param name="depid"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DataTable GetsItemProfessionCombobox(string depid, string year)
        {
            string cmdText = @"SELECT  sp.sProfessionID id ,
        p.EnglishName+'-'+p.Name name
FROM T_Stu_sProfession sp
    LEFT JOIN T_Pro_Profession p ON p.ProfessionID = sp.ProfessionID where sp.DeptID=@DeptID and Year=@Year and sp.Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptID",depid),
                new SqlParameter("@Year",year)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
        /// <summary>
        /// 根据专业名称返回专业招生专业id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetsProfesssionID(string name, UploadsEnrollFormModel uefm)
        {
            string where = " and ProfessionID IN (Select ProfessionID From T_Pro_Profession WHERE Name=@Name and Status=1 ) and  Status=1 and DeptID=@DeptID and Year=@Year and Month=@Month";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Name",name),
                new SqlParameter("@DeptID",uefm.upDeptId),
                new SqlParameter("@Year",uefm.upYear),
                new SqlParameter("@Month",uefm.upMonth)
            };
            return sProfessionBLL.sProfessionModelByWhere(where, paras).sProfessionID;
        }

        public static string GetsProfesssionID(EnrollDataModel edm)
        {
            string where = " and ProfessionID IN (Select ProfessionID From T_Pro_Profession WHERE Name=@Name and Status=1 ) and  Status=1 and DeptID=@DeptID and Year=@Year and Month=@Month";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Name",edm._major),
                new SqlParameter("@DeptID",edm._deptiId),
                new SqlParameter("@Year",edm._year),
                new SqlParameter("@Month",edm._month)
            };
            return sProfessionBLL.sProfessionModelByWhere(where, paras).sProfessionID;
        }

        public static string GetsProfesssionID(string professionName,string deptId,string year,string month)
        {
            string where = " and ProfessionID IN (Select ProfessionID From T_Pro_Profession WHERE Name=@Name and Status=1 ) and  Status=1 and DeptID=@DeptID and Year=@Year and Month=@Month";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Name",professionName),
                new SqlParameter("@DeptID",deptId),
                new SqlParameter("@Year",year),
                new SqlParameter("@Month",month)
            };
            return sProfessionBLL.sProfessionModelByWhere(where, paras).sProfessionID;
        }
        public static string GetsProfesssionID(string professionName, string deptId)
        {
            string where = " and ProfessionID IN (Select ProfessionID From T_Pro_Profession WHERE Name=@Name and Status=1 ) and  Status=1 and DeptID=@DeptID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Name",professionName),
                new SqlParameter("@DeptID",deptId)
            };
            return sProfessionBLL.sProfessionModelByWhere(where, paras).sProfessionID;
        }
    }
}
