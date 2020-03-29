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
    public class ClassBLL
    {
        public static int InsertClass(ClassModel cm)
        {
            string cmdText = @"INSERT INTO T_Pro_Class
(Status
,ProfessionID
,Name
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@ProfessionID
,@Name
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", cm.Status),
new SqlParameter("@ProfessionID", cm.ProfessionID),
new SqlParameter("@Name", cm.Name),
new SqlParameter("@CreateID", cm.CreateID),
new SqlParameter("@CreateTime", cm.CreateTime),
new SqlParameter("@UpdateID", cm.UpdateID),
new SqlParameter("@UpdateTime", cm.UpdateTime)
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

        public static int UpdateClass(ClassModel cm)
        {
            string cmdText = @"UPDATE T_Pro_Class SET
Status=@Status
,ProfessionID=@ProfessionID
,Name=@Name
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE ClassID=@ClassID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ClassID", cm.ClassID),
new SqlParameter("@Status", cm.Status),
new SqlParameter("@ProfessionID", cm.ProfessionID),
new SqlParameter("@Name", cm.Name),
new SqlParameter("@CreateID", cm.CreateID),
new SqlParameter("@CreateTime", cm.CreateTime),
new SqlParameter("@UpdateID", cm.UpdateID),
new SqlParameter("@UpdateTime", cm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(cm.ClassID);
            }
            else
            {
                return -1;
            }
        }

        public static ClassModel ClassModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            ClassModel cm = new ClassModel();
            string cmdText = "SELECT * FROM T_Pro_Class WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                cm.ClassID = dt.Rows[0]["ClassID"].ToString();
                cm.Status = dt.Rows[0]["Status"].ToString();
                cm.ProfessionID = dt.Rows[0]["ProfessionID"].ToString();
                cm.Name = dt.Rows[0]["Name"].ToString();
                cm.CreateID = dt.Rows[0]["CreateID"].ToString();
                cm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                cm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                cm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return cm;
        }

        public static DataTable ClassTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Class WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        /// <summary>
        /// 返回班级ID
        /// </summary>
        /// <param name="professionName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static string GetClassID(string professionName, string className, string detptId)
        {
            string cmdText = @"SELECT c.ClassID
FROM    T_Pro_Class AS c
        LEFT JOIN T_Pro_Profession AS p ON p.ProfessionID = c.ProfessionID
        Where  p.Name=@ProfessionName and c.Name=@ClassName and  p.DeptID=@DeptID ";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ProfessionName",professionName),
            new SqlParameter("@ClassName",className),
            new SqlParameter ("@DeptID",detptId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ClassID"].ToString();
            }
            else
            {
                return "-1";
            }

        }



        public static DataTable ClassCombobox(string professionId)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT ClassID Value,Name Text  FROM T_Pro_Class WHERE   Status=1 and ProfessionID=@ProfessionID order by ClassID ASC";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ProfessionID",professionId)
            };
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        ///根据招生专业返回班级id
        /// </summary>
        /// <param name="sprofessionId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetClassID(string sprofessionId, string name)
        {
            string where = " and ProfessionID=(SELECT ProfessionID FROM T_Stu_sProfession WHERE sProfessionID=@sProfessionID) and Name=@Name";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sProfessionID",sprofessionId),
                 new SqlParameter("@Name",name)
            };
            return ClassBLL.ClassModelByWhere(where, paras).ClassID;
        }

        public static DataTable ClassComboboxBysProfessionID(string sProfessionId)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  ClassID Value ,
        Name Text
FROM T_Pro_Class
WHERE Status = 1
        AND ProfessionID IN(SELECT    ProfessionID
                              FROM      T_Stu_sProfession
                              WHERE     Status = 1
                                        AND sProfessionID = @sProfessionID)
ORDER BY ClassID ASC";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@sProfessionID",sProfessionId)
            };
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
    }
}
