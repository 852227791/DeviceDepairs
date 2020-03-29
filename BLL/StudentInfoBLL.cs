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
    public class StudentInfoBLL
    {
        public static int InsertStudentInfo(StudentInfoModel sim)
        {
            string cmdText = @"INSERT INTO T_Pro_StudentInfo
			(Status
			,StudentID
			,Nation
			,ProvinceID
			,CityID
			,DistrictID
			,Zip
			,School
			,Photo
			,CreateID
			,CreateTime
			,UpdateID
			,UpdateTime
						)
			VALUES(@Status
			,@StudentID
			,@Nation
			,@ProvinceID
			,@CityID
			,@DistrictID
			,@Zip
			,@School
			,@Photo
			,@CreateID
			,@CreateTime
			,@UpdateID
			,@UpdateTime
						);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Status", sim.Status)
                ,new SqlParameter("@StudentID", sim.StudentID)
                ,new SqlParameter("@Nation", sim.Nation)
                ,new SqlParameter("@ProvinceID", sim.ProvinceID)
                ,new SqlParameter("@CityID", sim.CityID)
                ,new SqlParameter("@DistrictID", sim.DistrictID)
                ,new SqlParameter("@Zip", sim.Zip)
                ,new SqlParameter("@School", sim.School)
                ,new SqlParameter("@Photo", sim.Photo)
                ,new SqlParameter("@CreateID", sim.CreateID)
                ,new SqlParameter("@CreateTime", sim.CreateTime)
                ,new SqlParameter("@UpdateID", sim.UpdateID)
                ,new SqlParameter("@UpdateTime", sim.UpdateTime)
                            };
            return Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
        }

        public static int UpdateStudentInfo(StudentInfoModel sim)
        {
            string cmdText = @"UPDATE T_Pro_StudentInfo SET  Status=@Status
			,StudentID=@StudentID
			,Nation=@Nation
			,ProvinceID=@ProvinceID
			,CityID=@CityID
			,DistrictID=@DistrictID
			,Zip=@Zip
			,School=@School
			,Photo=@Photo
			,CreateID=@CreateID
			,CreateTime=@CreateTime
			,UpdateID=@UpdateID
			,UpdateTime=@UpdateTime
			Where StudentInfoID=@StudentInfoID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentInfoID", sim.StudentInfoID)
                ,new SqlParameter("@Status", sim.Status)
                ,new SqlParameter("@StudentID", sim.StudentID)
                ,new SqlParameter("@Nation", sim.Nation)
                ,new SqlParameter("@ProvinceID", sim.ProvinceID)
                ,new SqlParameter("@CityID", sim.CityID)
                ,new SqlParameter("@DistrictID", sim.DistrictID)
                ,new SqlParameter("@Zip", sim.Zip)
                ,new SqlParameter("@School", sim.School)
                ,new SqlParameter("@Photo", sim.Photo)
                ,new SqlParameter("@CreateID", sim.CreateID)
                ,new SqlParameter("@CreateTime", sim.CreateTime)
                ,new SqlParameter("@UpdateID", sim.UpdateID)
                ,new SqlParameter("@UpdateTime", sim.UpdateTime)
                };
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        public static List<StudentInfoModel> SelectStudentInfoByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            List<StudentInfoModel> list = new List<StudentInfoModel>();
            string cmdText = "SELECT * FROM T_Pro_StudentInfo WHERE 1 = 1 {0}{1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                StudentInfoModel sim = new StudentInfoModel();
                sim.StudentInfoID = dr["StudentInfoID"].ToString();
                sim.Status = dr["Status"].ToString();
                sim.StudentID = dr["StudentID"].ToString();
                sim.Nation = dr["Nation"].ToString();
                sim.ProvinceID = dr["ProvinceID"].ToString();
                sim.CityID = dr["CityID"].ToString();
                sim.DistrictID = dr["DistrictID"].ToString();
                sim.Zip = dr["Zip"].ToString();
                sim.School = dr["School"].ToString();
                sim.Photo = dr["Photo"].ToString();
                sim.CreateID = dr["CreateID"].ToString();
                sim.CreateTime = dr["CreateTime"].ToString();
                sim.UpdateID = dr["UpdateID"].ToString();
                sim.UpdateTime = dr["UpdateTime"].ToString();
                list.Add(sim);
            }
            return list;
        }

        public static bool UpdateStudentInfoStatus(string StudentInfoID, string status, int userId)
        {
            string where = " and StudentInfoID=@StudentInfoID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentInfoID", StudentInfoID)
            };
            var list = StudentInfoBLL.SelectStudentInfoByWhere(where, paras, "").FirstOrDefault();
            if (list != null)
            {
                list.Status = status;
                list.UpdateTime = DateTime.Now.ToString();
                list.UpdateID = userId.ToString();
                if (StudentInfoBLL.UpdateStudentInfo(list).Equals(1))
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }

        public static StudentInfoModel StudentInfoModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            StudentInfoModel sim = new StudentInfoModel();
            string cmdText = "SELECT * FROM T_Pro_StudentInfo WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                sim.StudentInfoID = dt.Rows[0]["StudentInfoID"].ToString();
                sim.Status = dt.Rows[0]["Status"].ToString();
                sim.StudentID = dt.Rows[0]["StudentID"].ToString();
                sim.Nation = dt.Rows[0]["Nation"].ToString();
                sim.ProvinceID = dt.Rows[0]["ProvinceID"].ToString();
                sim.CityID = dt.Rows[0]["CityID"].ToString();
                sim.DistrictID = dt.Rows[0]["DistrictID"].ToString();
                sim.Zip = dt.Rows[0]["Zip"].ToString();
                sim.School = dt.Rows[0]["School"].ToString();
                sim.CreateID = dt.Rows[0]["CreateID"].ToString();
                sim.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                sim.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                sim.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return sim;
        }

        public static DataTable StudentInfoTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_StudentInfo WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static StudentInfoModel GetStudentInfoModel(string studentId)
        {
            string where = " and StudentID=@StudentID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",studentId)
            };
            return StudentInfoBLL.StudentInfoModelByWhere(where, paras);
        }

        /// <summary>
        /// 查看学生信息
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static DataTable GetStudentInfoView(string studentId)
        {
            string cmdText = @" SELECT  a.Name + ISNULL(c.Name,'')+ISNULL(d.Name,'') PC ,
        si.Zip ,
        si.School ,
        r1.RefeName Nation
FROM    T_Pro_StudentInfo si
        LEFT JOIN T_Pro_Area a ON a.AreaID = si.ProvinceID
        LEFT JOIN T_Pro_Area c ON c.AreaID = si.CityID
		LEFT JOIN T_Pro_Area d ON d.AreaID=si.DistrictID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = si.Nation
                                   AND r1.RefeTypeID = 24
        where si.StudentID=@StudentID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",studentId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
    }
}
