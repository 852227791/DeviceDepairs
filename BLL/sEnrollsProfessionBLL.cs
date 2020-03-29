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
    public class sEnrollsProfessionBLL
    {
        public static int InsertsEnrollsProfession(sEnrollsProfessionModel epm)
        {
            string cmdText = @"INSERT INTO T_Stu_sEnrollsProfession
			(Status
			,DeptID
			,DeptAreaID
			,sEnrollID
			,Year
			,Month
			,LeaveYear
			,BeforeEnrollTime
			,EnrollTime
			,FirstFeeTime
			,EnrollLevel
			,sProfessionID
			,ClassID
			,Auditor
			,AuditTime
			,AuditView
			,Remark
			,CreateID
			,CreateTime
			,UpdateID
			,UpdateTime
						)
			VALUES(@Status
			,@DeptID
			,@DeptAreaID
			,@sEnrollID
			,@Year
			,@Month
			,@LeaveYear
			,@BeforeEnrollTime
			,@EnrollTime
			,@FirstFeeTime
			,@EnrollLevel
			,@sProfessionID
			,@ClassID
			,@Auditor
			,@AuditTime
			,@AuditView
			,@Remark
			,@CreateID
			,@CreateTime
			,@UpdateID
			,@UpdateTime
						);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Status", epm.Status)
                ,new SqlParameter("@DeptID", epm.DeptID)
                ,new SqlParameter("@DeptAreaID", epm.DeptAreaID)
                ,new SqlParameter("@sEnrollID", epm.sEnrollID)
                ,new SqlParameter("@Year", epm.Year)
                ,new SqlParameter("@Month", epm.Month)
                ,new SqlParameter("@LeaveYear", epm.LeaveYear)
                ,new SqlParameter("@BeforeEnrollTime", epm.BeforeEnrollTime)
                ,new SqlParameter("@EnrollTime", epm.EnrollTime)
                ,new SqlParameter("@FirstFeeTime", epm.FirstFeeTime)
                ,new SqlParameter("@EnrollLevel", epm.EnrollLevel)
                ,new SqlParameter("@sProfessionID", epm.sProfessionID)
                ,new SqlParameter("@ClassID", epm.ClassID)
                ,new SqlParameter("@Auditor", epm.Auditor)
                ,new SqlParameter("@AuditTime", epm.AuditTime)
                ,new SqlParameter("@AuditView", epm.AuditView)
                ,new SqlParameter("@Remark", epm.Remark)
                ,new SqlParameter("@CreateID", epm.CreateID)
                ,new SqlParameter("@CreateTime", epm.CreateTime)
                ,new SqlParameter("@UpdateID", epm.UpdateID)
                ,new SqlParameter("@UpdateTime", epm.UpdateTime)
                            };
            return Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
        }

        public static int UpdatesEnrollsProfession(sEnrollsProfessionModel epm)
        {
            string cmdText = @"UPDATE T_Stu_sEnrollsProfession SET  Status=@Status
			,DeptID=@DeptID
			,DeptAreaID=@DeptAreaID
			,sEnrollID=@sEnrollID
			,Year=@Year
			,Month=@Month
			,LeaveYear=@LeaveYear
			,BeforeEnrollTime=@BeforeEnrollTime
			,EnrollTime=@EnrollTime
			,FirstFeeTime=@FirstFeeTime
			,EnrollLevel=@EnrollLevel
			,sProfessionID=@sProfessionID
			,ClassID=@ClassID
			,Auditor=@Auditor
			,AuditTime=@AuditTime
			,AuditView=@AuditView
			,Remark=@Remark
			,CreateID=@CreateID
			,CreateTime=@CreateTime
			,UpdateID=@UpdateID
			,UpdateTime=@UpdateTime
			Where sEnrollsProfessionID=@sEnrollsProfessionID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID", epm.sEnrollsProfessionID)
                ,new SqlParameter("@Status", epm.Status)
                ,new SqlParameter("@DeptID", epm.DeptID)
                ,new SqlParameter("@DeptAreaID", epm.DeptAreaID)
                ,new SqlParameter("@sEnrollID", epm.sEnrollID)
                ,new SqlParameter("@Year", epm.Year)
                ,new SqlParameter("@Month", epm.Month)
                ,new SqlParameter("@LeaveYear", epm.LeaveYear)
                ,new SqlParameter("@BeforeEnrollTime", epm.BeforeEnrollTime)
                ,new SqlParameter("@EnrollTime", epm.EnrollTime)
                ,new SqlParameter("@FirstFeeTime", epm.FirstFeeTime)
                ,new SqlParameter("@EnrollLevel", epm.EnrollLevel)
                ,new SqlParameter("@sProfessionID", epm.sProfessionID)
                ,new SqlParameter("@ClassID", epm.ClassID)
                ,new SqlParameter("@Auditor", epm.Auditor)
                ,new SqlParameter("@AuditTime", epm.AuditTime)
                ,new SqlParameter("@AuditView", epm.AuditView)
                ,new SqlParameter("@Remark", epm.Remark)
                ,new SqlParameter("@CreateID", epm.CreateID)
                ,new SqlParameter("@CreateTime", epm.CreateTime)
                ,new SqlParameter("@UpdateID", epm.UpdateID)
                ,new SqlParameter("@UpdateTime", epm.UpdateTime)
                };
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        public static List<sEnrollsProfessionModel> SelectsEnrollsProfessionByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            List<sEnrollsProfessionModel> list = new List<sEnrollsProfessionModel>();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfession WHERE 1 = 1 {0}{1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                sEnrollsProfessionModel epm = new sEnrollsProfessionModel();
                epm.sEnrollsProfessionID = dr["sEnrollsProfessionID"].ToString();
                epm.Status = dr["Status"].ToString();
                epm.DeptID = dr["DeptID"].ToString();
                epm.DeptAreaID = dr["DeptAreaID"].ToString();
                epm.sEnrollID = dr["sEnrollID"].ToString();
                epm.Year = dr["Year"].ToString();
                epm.Month = dr["Month"].ToString();
                epm.LeaveYear = dr["LeaveYear"].ToString();
                epm.BeforeEnrollTime = dr["BeforeEnrollTime"].ToString();
                epm.EnrollTime = dr["EnrollTime"].ToString();
                epm.FirstFeeTime = dr["FirstFeeTime"].ToString();
                epm.EnrollLevel = dr["EnrollLevel"].ToString();
                epm.sProfessionID = dr["sProfessionID"].ToString();
                epm.ClassID = dr["ClassID"].ToString();
                epm.Auditor = dr["Auditor"].ToString();
                epm.AuditTime = dr["AuditTime"].ToString();
                epm.AuditView = dr["AuditView"].ToString();
                epm.Remark = dr["Remark"].ToString();
                epm.CreateID = dr["CreateID"].ToString();
                epm.CreateTime = dr["CreateTime"].ToString();
                epm.UpdateID = dr["UpdateID"].ToString();
                epm.UpdateTime = dr["UpdateTime"].ToString();
                list.Add(epm);
            }
            return list;
        }

        public static bool UpdatesEnrollsProfessionStatus(string sEnrollsProfessionID, string status, int userId)
        {
            string where = " and sEnrollsProfessionID=@sEnrollsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID)
            };
            var list = sEnrollsProfessionBLL.SelectsEnrollsProfessionByWhere(where, paras, "").FirstOrDefault();
            if (list != null)
            {
                list.Status = status;
                list.UpdateTime = DateTime.Now.ToString();
                list.UpdateID = userId.ToString();
                if (sEnrollsProfessionBLL.UpdatesEnrollsProfession(list).Equals(1))
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }

        public static sEnrollsProfessionModel sEnrollsProfessionModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sEnrollsProfessionModel epm = new sEnrollsProfessionModel();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfession WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                epm.sEnrollsProfessionID = dt.Rows[0]["sEnrollsProfessionID"].ToString();
                epm.Status = dt.Rows[0]["Status"].ToString();
                epm.DeptID = dt.Rows[0]["DeptID"].ToString();
                epm.DeptAreaID = dt.Rows[0]["DeptAreaID"].ToString();
                epm.sEnrollID = dt.Rows[0]["sEnrollID"].ToString();
                epm.Year = dt.Rows[0]["Year"].ToString();
                epm.Month = dt.Rows[0]["Month"].ToString();
                epm.LeaveYear = dt.Rows[0]["LeaveYear"].ToString();
                epm.BeforeEnrollTime = dt.Rows[0]["BeforeEnrollTime"].ToString();
                epm.EnrollTime = dt.Rows[0]["EnrollTime"].ToString();
                epm.FirstFeeTime = dt.Rows[0]["FirstFeeTime"].ToString();
                epm.EnrollLevel = dt.Rows[0]["EnrollLevel"].ToString();
                epm.sProfessionID = dt.Rows[0]["sProfessionID"].ToString();
                epm.ClassID = dt.Rows[0]["ClassID"].ToString();
                epm.Auditor = dt.Rows[0]["Auditor"].ToString();
                epm.AuditTime = dt.Rows[0]["AuditTime"].ToString();
                epm.AuditView = dt.Rows[0]["AuditView"].ToString();
                epm.Remark = dt.Rows[0]["Remark"].ToString();
                epm.CreateID = dt.Rows[0]["CreateID"].ToString();
                epm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                epm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                epm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return epm;
        }

        public static DataTable sEnrollsProfessionTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sEnrollsProfession WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// 根据报名专业id返回方案名称，报名id
        /// </summary>
        /// <param name="senrollProfessionId"></param>
        /// <returns></returns>
        public static DataTable GetPlanName(string senrollProfessionId)
        {
            string cmdText = @"SELECT DISTINCT
        s.Name + '_' + p.Name + '_' + o.PlanName PlanName,e.sEnrollID,e.StudyNum
FROM    T_Stu_sEnrollsProfession ep
        LEFT JOIN T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN T_Stu_sOrder o ON o.sEnrollsProfessionID = ep.sEnrollsProfessionID
        LEFT JOIN T_Stu_sProfession ip ON ip.sProfessionID = ep.sProfessionID
        LEFT JOIN T_Pro_Profession p ON p.ProfessionID = ip.ProfessionID
WHERE   ep.sEnrollsProfessionID = @sEnrollsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",senrollProfessionId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
        /// <summary>
        /// 根据报名id返回报名专业
        /// </summary>
        /// <param name="senrollId"></param>
        /// <returns></returns>
        public static DataTable GetsEnrollProfessionTable(string senrollId)
        {
            string where = " and Status IN(1,2,3,4) and sEnrollID=@sEnrollID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollID",senrollId)
            };
            return sEnrollsProfessionBLL.sEnrollsProfessionTableByWhere(where, paras, "");
        }

        /// <summary>
        /// 更新报名时间和状态
        /// </summary>
        /// <param name="senrollProfessionId"></param>
        public static void UpdatesEnrollTime(string senrollProfessionId)
        {
            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(senrollProfessionId);
            epm.EnrollTime = DateTime.Now.ToString("yyyy-MM-dd");
            epm.Status = "3";
            epm.UpdateID = "0";
            epm.UpdateTime = DateTime.Now.ToString();
            sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
        }
        /// <summary>
        /// 根据id返回报名专业信息
        /// </summary>
        /// <param name="senrollsProfessionId"></param>
        /// <returns></returns>
        public static sEnrollsProfessionModel GetsEnrollsProfessionModel(string senrollsProfessionId)
        {
            string where = " and sEnrollsProfessionID=@sEnrollsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",senrollsProfessionId)
            };
            return sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(where, paras);
        }
        /// <summary>
        /// 根据订单缴费情况变更报名状态
        /// </summary>
        /// <param name="sEnrollProfessionId"></param>
        /// <param name="userId"></param>
        public static void UpdatesEnrollsProfessionStatus(string sEnrollProfessionId, string userId)
        {
            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(sEnrollProfessionId);
            if (epm.Status.Equals("3"))
            {
                DataTable dt = sOrderBLL.GetsOrderTable(sEnrollProfessionId, true);
                if (dt.Rows.Count > 0)
                {
                    epm.Status = "4";
                    epm.UpdateID = userId;
                    epm.UpdateTime = DateTime.Now.ToString();
                    sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                }
            }
        }

        /// <summary>
        /// 根据报名专业id返回学生身份证号
        /// </summary>
        /// <param name="senrollsProfessionId"></param>
        /// <returns></returns>
        public static string GetStudentIdCard(string senrollsProfessionId)
        {

            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(senrollsProfessionId);
            sEnrollModel em = sEnrollBLL.GetEnrollModel(epm.sEnrollID);
            string where = "and StudentID=@StudentID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",em.StudentID)
            };
            return StudentBLL.StudentModelByWhere(where, paras).IDCard;
        }
        /// <summary>
        /// 根据专业获取报名信息
        /// </summary>
        /// <param name="sprofessionId"></param>
        /// <returns></returns>
        public static DataTable GetsEnrollsProfessionTable(string enrollLevel, string senrollId, string majorName, string deptId)
        {
            string where = " and sProfessionID IN (Select sProfessionID from T_Stu_sProfession Where ProfessionID=(Select ProfessionID From T_Pro_Profession WHERE Name=@Name and Status=1 and DeptID=@DeptID) and  Status=1 and DeptID=@DeptID)  and EnrollLevel=@EnrollLevel and sEnrollID=@sEnrollID and Status<>9";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollID",senrollId),
                 new SqlParameter("@DeptID",deptId),
                  new SqlParameter("@Name",majorName),
                new SqlParameter("@EnrollLevel",enrollLevel)
            };
            return sEnrollsProfessionBLL.sEnrollsProfessionTableByWhere(where, paras, "");
        }
    }
}
