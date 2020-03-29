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
    public class StudentBLL
    {
        /// <summary>
        /// 根据身份证号返回学生id
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static string getStudentId(string idCard)
        {
            string where = " and IDCard=@IDCard ";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@IDCard",idCard)
            };
            DataTable dt = StudentBLL.StudentTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["StudentID"].ToString();
            }
            else
            {
                return "0";
            }
        }

        public static int InsertStudent(StudentModel sm)
        {
            string cmdText = @"INSERT INTO T_Pro_Student
(Status
,DeptID
,Name
,IDCard
,Sex
,Mobile
,QQ
,WeChat
,Address
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@DeptID
,@Name
,@IDCard
,@Sex
,@Mobile
,@QQ
,@WeChat
,@Address
,@Remark
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Status", sm.Status),
new SqlParameter("@DeptID", sm.DeptID),
new SqlParameter("@Name", sm.Name),
new SqlParameter("@IDCard", sm.IDCard),
new SqlParameter("@Sex", sm.Sex),
new SqlParameter("@Mobile", sm.Mobile),
new SqlParameter("@QQ", sm.QQ),
new SqlParameter("@WeChat", sm.WeChat),
new SqlParameter("@Address", sm.Address),
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

        public static int UpdateStudent(StudentModel sm)
        {
            string cmdText = @"UPDATE T_Pro_Student SET
Status=@Status
,DeptID=@DeptID
,Name=@Name
,IDCard=@IDCard
,Sex=@Sex
,Mobile=@Mobile
,QQ=@QQ
,WeChat=@WeChat
,Address=@Address
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE StudentID=@StudentID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@StudentID", sm.StudentID),
new SqlParameter("@Status", sm.Status),
new SqlParameter("@DeptID", sm.DeptID),
new SqlParameter("@Name", sm.Name),
new SqlParameter("@IDCard", sm.IDCard),
new SqlParameter("@Sex", sm.Sex),
new SqlParameter("@Mobile", sm.Mobile),
new SqlParameter("@QQ", sm.QQ),
new SqlParameter("@WeChat", sm.WeChat),
new SqlParameter("@Address", sm.Address),
new SqlParameter("@Remark", sm.Remark),
new SqlParameter("@CreateID", sm.CreateID),
new SqlParameter("@CreateTime", sm.CreateTime),
new SqlParameter("@UpdateID", sm.UpdateID),
new SqlParameter("@UpdateTime", sm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(sm.StudentID);
            }
            else
            {
                return -1;
            }
        }

        public static StudentModel StudentModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            StudentModel sm = new StudentModel();
            string cmdText = "SELECT * FROM T_Pro_Student WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                sm.StudentID = dt.Rows[0]["StudentID"].ToString();
                sm.Status = dt.Rows[0]["Status"].ToString();
                sm.DeptID = dt.Rows[0]["DeptID"].ToString();
                sm.Name = dt.Rows[0]["Name"].ToString();
                sm.IDCard = dt.Rows[0]["IDCard"].ToString();
                sm.Sex = dt.Rows[0]["Sex"].ToString();
                sm.Mobile = dt.Rows[0]["Mobile"].ToString();
                sm.QQ = dt.Rows[0]["QQ"].ToString();
                sm.WeChat = dt.Rows[0]["WeChat"].ToString();
                sm.Address = dt.Rows[0]["Address"].ToString();
                sm.Remark = dt.Rows[0]["Remark"].ToString();
                sm.CreateID = dt.Rows[0]["CreateID"].ToString();
                sm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                sm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                sm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return sm;
        }

        public static DataTable StudentTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Student WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }


        public static DataTable StudentClassTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  s.StudentID ,
        s.DeptID ,
        s.Name ,
        s.IDCard ,
        s.Sex ,
        s.Mobile ,
        s.QQ ,
        s.WeChat ,
        s.Address ,
        s.Remark
FROM    T_Pro_Student s
WHERE   1 = 1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        #region 获取学生Model
        public static StudentModel GetStudentModel(string name, string idCard)
        {
            string where = " and Name=@Name and IDCard=@IDCard";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@Name",name),
            new SqlParameter("@IDCard",idCard)
            };
            StudentModel sm = StudentBLL.StudentModelByWhere(where, paras);
            return sm;
        }
        #endregion

        public static StudentModel GetStudentModel(string idCard)
        {
            string where = " and  IDCard=@IDCard";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@IDCard",idCard)
            };
            StudentModel sm = StudentBLL.StudentModelByWhere(where, paras);
            return sm;
        }

        /// <summary>
        /// 验证学号是否重复
        /// </summary>
        /// <param name="enrollNum"></param>
        /// <returns></returns>
        public static bool ValidateEnrollNum(string enrollNum, string deptId)
        {
            string where = " and EnrollNum=@EnrollNum and DeptID=@DeptID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@EnrollNum",enrollNum),
                new SqlParameter("@DeptID",deptId)
            };
            if (string.IsNullOrEmpty(sEnrollBLL.sEnrollModelByWhere(where, paras).sEnrollID))
            {
                return false;
            }
            else
                return true;
        }

        public static DataTable StudentTableByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  s.StudentID ,
        s.Status ,
        s.DeptID ,
        s.Name ,
        s.IDCard
FROM    T_Pro_Student s
        LEFT JOIN T_Stu_sEnroll e ON e.StudentID = s.StudentID WHERE 1=1 {0} ";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static DataTable StudentTableByWhere(string name, string IdCard)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  s.StudentID ,
        s.Name studentName ,
        s.IDCard ,
        s.Sex ,
        s.Mobile ,
        si.Nation ,
        s.Address ,
        ( SELECT TOP 1
                    Name
          FROM      T_Pro_StudentContact
          WHERE     StudentID = s.StudentID
                    AND Status = 1
          ORDER BY  StudentContactID ASC
        ) parentName ,
        ( SELECT TOP 1
                    Tel
          FROM      T_Pro_StudentContact
          WHERE     StudentID = s.StudentID
                    AND Status = 1
          ORDER BY  StudentContactID ASC
        ) parentTel ,   ( SELECT TOP 1
                    StudentContactID
          FROM      T_Pro_StudentContact
          WHERE     StudentID = s.StudentID
                    AND Status = 1
          ORDER BY  StudentContactID ASC
        ) studentContactId,
        si.ProvinceID ,
        si.CityID,
        si.School ,
        si.Zip ,
        s.QQ ,
		si.StudentInfoID studentInfoId,
        s.WeChat
FROM    T_Pro_Student s
        LEFT JOIN T_Pro_StudentInfo si ON si.StudentID = s.StudentID
        WHERE s.Name=@Name and s.IDCard=@IDCard";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Name",name),
                new SqlParameter("@IDCard",IdCard)
            };
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 验证身份证重复
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool ValidateIDCard(string studentId, string idCard)
        {
            string where = " and StudentID<>@StudentID and IDCard=@IDCard";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",studentId),
                new SqlParameter("@IDCard",idCard)
            };
            DataTable dt = StudentBLL.StudentTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;

            }
            return false;
        }
    }
}
