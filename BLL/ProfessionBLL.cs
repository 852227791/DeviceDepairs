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
    public class ProfessionBLL
    {
        public static int InsertProfession(ProfessionModel pm)
        {
            string cmdText = @"INSERT INTO T_Pro_Profession
(Status
,DeptID
,Name
,EnglishName
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@DeptID
,@Name
,@EnglishName
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", pm.Status),
new SqlParameter("@DeptID", pm.DeptID),
new SqlParameter("@Name", pm.Name),
new SqlParameter("@EnglishName", pm.EnglishName),
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

        public static int UpdateProfession(ProfessionModel pm)
        {
            string cmdText = @"UPDATE T_Pro_Profession SET
Status=@Status
,DeptID=@DeptID
,Name=@Name
,EnglishName=@EnglishName
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE ProfessionID=@ProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ProfessionID", pm.ProfessionID),
new SqlParameter("@Status", pm.Status),
new SqlParameter("@DeptID", pm.DeptID),
new SqlParameter("@Name", pm.Name),
new SqlParameter("@EnglishName", pm.EnglishName),
new SqlParameter("@CreateID", pm.CreateID),
new SqlParameter("@CreateTime", pm.CreateTime),
new SqlParameter("@UpdateID", pm.UpdateID),
new SqlParameter("@UpdateTime", pm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(pm.ProfessionID);
            }
            else
            {
                return -1;
            }
        }

        public static ProfessionModel ProfessionModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            ProfessionModel pm = new ProfessionModel();
            string cmdText = "SELECT * FROM T_Pro_Profession WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                pm.ProfessionID = dt.Rows[0]["ProfessionID"].ToString();
                pm.Status = dt.Rows[0]["Status"].ToString();
                pm.DeptID = dt.Rows[0]["DeptID"].ToString();
                pm.Name = dt.Rows[0]["Name"].ToString();
                pm.EnglishName = dt.Rows[0]["EnglishName"].ToString();
                pm.CreateID = dt.Rows[0]["CreateID"].ToString();
                pm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                pm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                pm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return pm;
        }

        public static DataTable ProfessionTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Profession WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        public static DataTable ProfessionCombobox(string deptId)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT ProfessionID Value,EnglishName+'-'+Name Text  FROM T_Pro_Profession WHERE   Status=1 and DeptID=@DeptID order by EnglishName ASC";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@DeptID",deptId)
            };
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 根据招生专业id返回专业名称
        /// </summary>
        /// <param name="sprofessionId"></param>
        /// <returns></returns>
        public static string GetProfesionName(string sprofessionId)
        {
            string cmdText = @"SELECT  p.Name
FROM    T_Pro_Profession p
        LEFT JOIN T_Stu_sProfession sp ON sp.ProfessionID = p.ProfessionID
        LEFT JOIN T_Stu_sEnrollsProfession ep ON ep.sProfessionID = sp.sProfessionID
WHERE   ep.sEnrollsProfessionID = @sEnrollsProfessionID";

            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",sprofessionId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Name"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 根据专业名称返回专业实体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ProfessionModel GetProfessionModel(string name)
        {
            string where = " and Name=@Name and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Name",name)            };
            return ProfessionBLL.ProfessionModelByWhere(where, paras);
        }
    }
}
