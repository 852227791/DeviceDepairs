using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DAL;
using Model;
using System.Transactions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BLL
{
    public class sEnrollBLL
    {
        public static int InsertsEnroll(sEnrollModel em)
        {
            string cmdText = @"INSERT INTO T_Stu_sEnroll
			(Status
			,DeptID
			,StudentID
			,ExamNum
			,StudyNum
			,EnrollNum
			,CreateID
			,CreateTime
			,UpdateID
			,UpdateTime
						)
			VALUES(@Status
			,@DeptID
			,@StudentID
			,@ExamNum
			,@StudyNum
			,@EnrollNum
			,@CreateID
			,@CreateTime
			,@UpdateID
			,@UpdateTime
						);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Status", em.Status)
                ,new SqlParameter("@DeptID", em.DeptID)
                ,new SqlParameter("@StudentID", em.StudentID)
                ,new SqlParameter("@ExamNum", em.ExamNum)
                ,new SqlParameter("@StudyNum", em.StudyNum)
                ,new SqlParameter("@EnrollNum", em.EnrollNum)
                ,new SqlParameter("@CreateID", em.CreateID)
                ,new SqlParameter("@CreateTime", em.CreateTime)
                ,new SqlParameter("@UpdateID", em.UpdateID)
                ,new SqlParameter("@UpdateTime", em.UpdateTime)
                            };
            return Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
        }

        public static int UpdatesEnroll(sEnrollModel em)
        {
            string cmdText = @"UPDATE T_Stu_sEnroll SET  Status=@Status
			,DeptID=@DeptID
			,StudentID=@StudentID
			,ExamNum=@ExamNum
			,StudyNum=@StudyNum
			,EnrollNum=@EnrollNum
			,CreateID=@CreateID
			,CreateTime=@CreateTime
			,UpdateID=@UpdateID
			,UpdateTime=@UpdateTime
			Where sEnrollID=@sEnrollID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollID", em.sEnrollID)
                ,new SqlParameter("@Status", em.Status)
                ,new SqlParameter("@DeptID", em.DeptID)
                ,new SqlParameter("@StudentID", em.StudentID)
                ,new SqlParameter("@ExamNum", em.ExamNum)
                ,new SqlParameter("@StudyNum", em.StudyNum)
                ,new SqlParameter("@EnrollNum", em.EnrollNum)
                ,new SqlParameter("@CreateID", em.CreateID)
                ,new SqlParameter("@CreateTime", em.CreateTime)
                ,new SqlParameter("@UpdateID", em.UpdateID)
                ,new SqlParameter("@UpdateTime", em.UpdateTime)
                };
            return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        public static List<sEnrollModel> SelectsEnrollByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            List<sEnrollModel> list = new List<sEnrollModel>();
            string cmdText = "SELECT * FROM T_Stu_sEnroll WHERE 1 = 1 {0}{1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                sEnrollModel em = new sEnrollModel();
                em.sEnrollID = dr["sEnrollID"].ToString();
                em.Status = dr["Status"].ToString();
                em.DeptID = dr["DeptID"].ToString();
                em.StudentID = dr["StudentID"].ToString();
                em.ExamNum = dr["ExamNum"].ToString();
                em.StudyNum = dr["StudyNum"].ToString();
                em.EnrollNum = dr["EnrollNum"].ToString();
                em.CreateID = dr["CreateID"].ToString();
                em.CreateTime = dr["CreateTime"].ToString();
                em.UpdateID = dr["UpdateID"].ToString();
                em.UpdateTime = dr["UpdateTime"].ToString();
                list.Add(em);
            }
            return list;
        }

        public static sEnrollModel GetsEnrollModel(string sEnrollID)
        {
            string where = " and sEnrollID=@sEnrollID";
            SqlParameter[] paras = new SqlParameter[] {
             new SqlParameter("@sEnrollID", sEnrollID)
             };
            return sEnrollBLL.SelectsEnrollByWhere(where, paras, "").FirstOrDefault();
        }


        public static sEnrollModel sEnrollModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            sEnrollModel em = new sEnrollModel();
            string cmdText = "SELECT * FROM T_Stu_sEnroll WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                em.sEnrollID = dt.Rows[0]["sEnrollID"].ToString();
                em.Status = dt.Rows[0]["Status"].ToString();
                em.DeptID = dt.Rows[0]["DeptID"].ToString();
                em.StudentID = dt.Rows[0]["StudentID"].ToString();
                em.ExamNum = dt.Rows[0]["ExamNum"].ToString();
                em.StudyNum = dt.Rows[0]["StudyNum"].ToString();
                em.EnrollNum = dt.Rows[0]["EnrollNum"].ToString();
                em.CreateID = dt.Rows[0]["CreateID"].ToString();
                em.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                em.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                em.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return em;
        }

        public static DataTable sEnrollTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Stu_sEnroll WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        /// <summary>
        /// 根据主体、年、月获取专业（排除停用的）
        /// </summary>
        /// <param name="dept"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DataTable GetMajor(string dept, string year, string month)
        {
            string cmdText = @"SELECT  spro.sProfessionID AS Id ,
        pro.EnglishName + ' ' + pro.Name AS Text
FROM    T_Stu_sProfession AS spro
        LEFT JOIN T_Pro_Profession AS pro ON spro.ProfessionID = pro.ProfessionID
WHERE   spro.DeptID = @DeptID
        AND spro.Year = @Year
        AND spro.Month = @Month
        AND spro.Status = 1
ORDER BY pro.EnglishName ASC";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@DeptID", dept),
new SqlParameter("@Year", year),
new SqlParameter("@Month", month)
};
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        /// <summary>
        /// 根据sEnrollsProfessionID获取报名信息
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <returns></returns>
        public static DataTable SelectsEnroll(string sEnrollsProfessionID)
        {
            string cmdText = @"SELECT    senrollpro.sEnrollsProfessionID ,
            
            senrollpro.DeptID ,
            dept.Name AS DeptName ,
            senroll.EnrollNum ,
            senroll.StudentID ,
            student.Name AS StuName ,
            student.IDCard ,
            senrollpro.Year ,
            CAST(senrollpro.Year AS NVARCHAR) + '年' AS YearName ,
            senrollpro.Month ,
            refe_month.RefeName AS MonthName ,
            senrollpro.EnrollLevel ,
            refe_level.RefeName AS LevelName ,
            senrollpro.sProfessionID ,
            senrollpro.ClassID ,
            pro.Name AS MajorName ,
            CONVERT(NVARCHAR(23), senrollpro.EnrollTime, 23) AS EnrollTime ,
            senrollpro.Remark ,
            ( SELECT    item.Name + '，'
              FROM      T_Stu_sItemsEnroll AS sienroll
                        LEFT JOIN T_Pro_Item AS item ON sienroll.ItemID = item.ItemID
              WHERE     sienroll.sEnrollsProfessionID = @sEnrollsProfessionID
            FOR
              XML PATH('')
            ) AS SchemeName,
           senrollpro.DeptAreaID
  FROM      T_Stu_sEnrollsProfession AS senrollpro
            LEFT JOIN T_Sys_Dept AS dept ON senrollpro.DeptID = dept.DeptID
            LEFT JOIN T_Stu_sEnroll AS senroll ON senrollpro.sEnrollID = senroll.sEnrollID
            LEFT JOIN T_Pro_Student AS student ON senroll.StudentID = student.StudentID
            LEFT JOIN T_Sys_Refe AS refe_month ON senrollpro.Month = refe_month.Value
                                                  AND refe_month.RefeTypeID = 18
            LEFT JOIN T_Sys_Refe AS refe_level ON senrollpro.EnrollLevel = refe_level.Value
                                                  AND refe_level.RefeTypeID = 17
            LEFT JOIN T_Stu_sProfession AS spro ON senrollpro.sProfessionID = spro.sProfessionID
            LEFT JOIN T_Pro_Profession AS pro ON spro.ProfessionID = pro.ProfessionID
  WHERE     senrollpro.sEnrollsProfessionID = @sEnrollsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionID",sEnrollsProfessionID)
};
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        /// <summary>
        /// 根据sEnrollsProfessionID获取缴费方案
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <returns></returns>
        public static DataTable GetEditShceme(string sEnrollsProfessionID)
        {
            string cmdText = @"SELECT  sienroll.ItemID ,
        item.Year ,
        refe_month.RefeName AS Month ,
        item.Name
FROM    T_Stu_sItemsEnroll AS sienroll
        LEFT JOIN T_Pro_Item AS item ON sienroll.ItemID = item.ItemID
        LEFT JOIN T_Sys_Refe AS refe_month ON item.Month = refe_month.Value
                                              AND refe_month.RefeTypeID = 16
WHERE   sienroll.sEnrollsProfessionID = @sEnrollsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionID",sEnrollsProfessionID)
};
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        /// <summary>
        /// 判断学生是否重复报名
        /// </summary>
        /// <param name="DeptID">主体</param>
        /// <param name="studentID">学生</param>
        /// <param name="Year">年份</param>
        /// <param name="Month">月份</param>
        /// <param name="EnrollLevel">学习层次</param>
        /// <param name="sProfessionID">专业</param>
        /// <param name="sEnrollsProfessionID">修改的ID，判断重复时是否排除自身</param>
        /// <returns></returns>
        public static bool CheckIsRepeat(string DeptID, string studentID, string Year, string Month, string EnrollLevel, string sProfessionID, string sEnrollsProfessionID)
        {
            string cmdText = @"SELECT  senrollpro.*
FROM    T_Stu_sEnrollsProfession AS senrollpro
        LEFT JOIN T_Stu_sEnroll AS senroll ON senrollpro.sEnrollID = senroll.sEnrollID
WHERE   senrollpro.DeptID = @DeptID
        AND senroll.StudentID = @StudentID
        AND senrollpro.Year = @Year
        AND senrollpro.Month = @Month
        AND senrollpro.EnrollLevel = @EnrollLevel
        AND senrollpro.sProfessionID = @sProfessionID
        AND senrollpro.Status != 9";
            if (sEnrollsProfessionID.Trim() != "")
            {
                //修改判断重复，排除自身
                cmdText += " AND senrollpro.sEnrollsProfessionID != " + sEnrollsProfessionID;
            }
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@DeptID",DeptID),
new SqlParameter("@StudentID",studentID),
new SqlParameter("@Year",Year),
new SqlParameter("@Month",Month),
new SqlParameter("@EnrollLevel",EnrollLevel),
new SqlParameter("@sProfessionID",sProfessionID)
};
            if (DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除T_Stu_sItemsEnroll
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        public static void DeleteItemsEnroll(string sEnrollsProfessionID)
        {
            string cmdText = @"DELETE FROM T_Stu_sItemsEnroll WHERE sEnrollsProfessionID = @sEnrollsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID)
};
            DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 验证报名专业是否已经缴费
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <returns></returns>
        public static bool CheckIsGiveFee(string sEnrollsProfessionID)
        {
            string cmdText = @"SELECT  ISNULL(SUM(PaidMoney), 0) AS PaidMoney
FROM    T_Stu_sOrder
WHERE   sEnrollsProfessionID = @sEnrollsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionID",sEnrollsProfessionID)
};
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (Convert.ToDecimal(dt.Rows[0][0]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据报名专业ID判断是否生成缴费单
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <returns></returns>
        public static bool CheckIsShouldMoney(string sEnrollsProfessionID)
        {
            string cmdText = @"SELECT  ISNULL(SUM(ShouldMoney), 0) AS ShouldMoney
FROM    T_Stu_sOrder
WHERE   sEnrollsProfessionID = @sEnrollsProfessionID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionID",sEnrollsProfessionID)
};
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (Convert.ToDecimal(dt.Rows[0][0]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据学生ID从T_Stu_sEnroll获取数据
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        public static DataTable GetStudentEnroll(string studentID)
        {
            string cmdText = @"SELECT * from T_Stu_sEnroll WHERE StudentID=@StudentID ORDER BY sEnrollID desc";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@StudentID",studentID)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
        public static DataTable GetStudentEnroll(string studentid, string enrollNum)
        {
            string where = " and StudentID=@StudentID and EnrollNum=@EnrollNum";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",studentid),
                new SqlParameter("@EnrollNum",enrollNum)
            };
            return sEnrollBLL.sEnrollTableByWhere(where, paras, "");
        }
        /// <summary>
        /// 验证选择的报名信息中是否有不能生成的数据（只有在报名状态下才能生成缴费单，2）
        /// </summary>
        /// <param name="sEnrollsProfessionIDs"></param>
        /// <returns></returns>
        public static bool CheckIsBuildsFee(string sEnrollsProfessionIDs)
        {
            string cmdText = @"SELECT sEnrollsProfessionID FROM T_Stu_sEnrollsProfession WHERE sEnrollsProfessionID IN (" + sEnrollsProfessionIDs + ") AND Status NOT IN (1,2,3,4)";
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到报名专业对应的学费缴费方案（多条），排除作废的
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <returns></returns>
        public static DataTable GetsFeeScheme(string sEnrollsProfessionID)
        {
            string cmdText = @"SELECT  item.*
FROM    T_Stu_sItemsEnroll AS itemenroll
        LEFT JOIN T_Pro_Item AS item ON item.ItemID = itemenroll.ItemID
        LEFT JOIN T_Stu_sEnrollsProfession sen ON itemenroll.sEnrollsProfessionID = sen.sEnrollsProfessionID
        LEFT JOIN T_Stu_sProfession spr ON sen.sProfessionID = spr.sProfessionID
WHERE   itemenroll.sEnrollsProfessionID = @sEnrollsProfessionID
        AND item.Status = 1
        AND spr.Status = 1
ORDER BY item.Queue ASC";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionID",sEnrollsProfessionID)
};
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        /// <summary>
        /// 得到缴费方案下面的缴费期数（多条），T_Pro_Item，排除作废的
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns></returns>
        public static DataTable GetSemester(string ItemID)
        {
            string cmdText = @"SELECT  *
FROM    T_Pro_Item
WHERE   ParentID = @ItemID
        AND Status = 1 ORDER BY Queue ASC";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ItemID",ItemID)
};
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        /// <summary>
        /// 得到每期下面的详细费用项，T_Pro_ItemDetail，排除作废的
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns></returns>
        public static DataTable GetItemDetail(string ItemID)
        {
            string cmdText = @"SELECT  *
FROM    T_Pro_ItemDetail
WHERE   ItemID = @ItemID
        AND Status = 1 ORDER BY Queue ASC";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ItemID",ItemID)
};
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        /// <summary>
        /// 得到缴费方案下面的配品（多条），T_Stu_sItemsGive
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns></returns>
        public static DataTable GetsGive(string ItemID)
        {
            string cmdText = @"SELECT  sitem.*
FROM    T_Stu_sItemsGive sitem
        LEFT JOIN T_Stu_sGive sgive ON sgive.sGiveID = sitem.sGiveID
WHERE   sitem.ItemID = @ItemID
        AND sgive.Status = 1
ORDER BY Queue ASC";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ItemID",ItemID)
};
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        /// <summary>
        /// 根据报名专业ID和缴费方案ID，停掉缴费单
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <param name="PlanItemID"></param>
        public static void StopsOrder(string sEnrollsProfessionID, string PlanItemID)
        {
            string cmdText = @"UPDATE  T_Stu_sOrder
SET     Status = 9
WHERE   sEnrollsProfessionID = @sEnrollsProfessionID
        AND PlanItemID = @PlanItemID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID),
new SqlParameter("@PlanItemID", PlanItemID)
};
            DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 根据报名专业ID和缴费方案ID和期数ID，停掉缴费单
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <param name="PlanItemID"></param>
        /// <param name="NumItemID"></param>
        public static void StopsOrder(string sEnrollsProfessionID, string PlanItemID, string NumItemID)
        {
            string cmdText = @"UPDATE  T_Stu_sOrder
SET     Status = 9
WHERE   sEnrollsProfessionID = @sEnrollsProfessionID
        AND PlanItemID = @PlanItemID
        AND NumItemID = @NumItemID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID),
new SqlParameter("@PlanItemID", PlanItemID),
new SqlParameter("@NumItemID", NumItemID)
};
            DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 把缴费单中该期的数据的ItemQueue给修改了
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <param name="PlanItemID"></param>
        /// <param name="NumItemID"></param>
        /// <param name="ItemQueue"></param>
        public static void UpdatesOrderItemQueue(string sEnrollsProfessionID, string PlanItemID, string NumItemID, string ItemQueue)
        {
            string cmdText = @"UPDATE  T_Stu_sOrder
SET     ItemQueue = @ItemQueue
WHERE   sEnrollsProfessionID = @sEnrollsProfessionID
        AND PlanItemID = @PlanItemID
        AND NumItemID = @NumItemID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ItemQueue", ItemQueue),
new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID),
new SqlParameter("@PlanItemID", PlanItemID),
new SqlParameter("@NumItemID", NumItemID)
};
            DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 判断缴费方案的配品是否领取（排除停用的），2已领取
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <param name="PlanItemID"></param>
        /// <returns></returns>
        public static bool IsTakeGive(string sEnrollsProfessionID, string PlanItemID)
        {
            string cmdText = @"SELECT  *
FROM    T_Stu_sOrderGive
WHERE   sEnrollsProfessionID = @sEnrollsProfessionID
        AND PlanItemID = @PlanItemID
        AND Status = 2";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionID",sEnrollsProfessionID),
new SqlParameter("@PlanItemID",PlanItemID)
};
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据报名专业ID和缴费方案ID，停掉配品
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <param name="PlanItemID"></param>
        public static void StopsOrderGive(string sEnrollsProfessionID, string PlanItemID)
        {
            string cmdText = @"UPDATE  T_Stu_sOrderGive
SET     Status = 9
WHERE   sEnrollsProfessionID = @sEnrollsProfessionID
        AND PlanItemID = @PlanItemID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID),
new SqlParameter("@PlanItemID", PlanItemID)
};
            DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 根据报名专业ID和缴费方案ID和配品ID，停掉配品
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <param name="PlanItemID"></param>
        /// <param name="sGiveID"></param>
        public static void StopsOrderGive(string sEnrollsProfessionID, string PlanItemID, string sGiveID)
        {
            string cmdText = @"UPDATE  T_Stu_sOrderGive
SET     Status = 9
WHERE   sEnrollsProfessionID = @sEnrollsProfessionID
        AND PlanItemID = @PlanItemID
        AND sGiveID = @sGiveID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID),
new SqlParameter("@PlanItemID", PlanItemID),
new SqlParameter("@sGiveID", sGiveID)
};
            DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 修改配品的Queue
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <param name="PlanItemID"></param>
        /// <param name="sGiveID"></param>
        /// <param name="Queue"></param>
        public static void UpdatesOrderGiveQueue(string sEnrollsProfessionID, string PlanItemID, string sGiveID, string Queue)
        {
            string cmdText = @"UPDATE  T_Stu_sOrderGive
SET     Queue = @Queue
WHERE   sEnrollsProfessionID = @sEnrollsProfessionID
        AND PlanItemID = @PlanItemID
        AND sGiveID = @sGiveID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@Queue", Queue),
new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID),
new SqlParameter("@PlanItemID", PlanItemID),
new SqlParameter("@sGiveID", sGiveID)
};
            DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 根据主体、年、月获取缴费方案（排除停用的）
        /// </summary>
        /// <param name="dept"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DataTable GetScheme(string dept, string year, string month)
        {
            string cmdText = @"SELECT  ItemID AS Id ,
        EnglishName + ' ' + Name AS Text
FROM    T_Pro_Item
WHERE   DeptID = @DeptID
        AND Year = @Year
        AND Month = @Month
        AND Status = 1
        AND IsPlan = 1
ORDER BY EnglishName ASC";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@DeptID", dept),
new SqlParameter("@Year", year),
new SqlParameter("@Month", month)
};
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        /// <summary>
        /// 批量生成缴费单时，得到报名专业ID
        /// </summary>
        /// <param name="level"></param>
        /// <param name="sProfessionIDs"></param>
        /// <param name="schemes"></param>
        /// <returns></returns>
        public static string GetBuildsProfessionIDs(string level, string sProfessionIDs, string schemes)
        {
            string result = string.Empty;
            string cmdText = @"SELECT  sEnrollsProfessionID 
FROM    T_Stu_sEnrollsProfession
WHERE   Status IN (1,2,3,4)";
            if (!string.IsNullOrEmpty(level))
            {
                cmdText += " AND EnrollLevel = " + level;
            }
            if (!string.IsNullOrEmpty(sProfessionIDs))
            {
                cmdText += " AND sProfessionID in (" + sProfessionIDs + ")";
            }
            if (!string.IsNullOrEmpty(schemes))
            {
                cmdText += " AND sProfessionID in (SELECT DISTINCT sProfessionID FROM T_Stu_sItemsProfession WHERE ItemID IN (" + schemes + "))";
            }
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            if (dt.Rows.Count > 0)
            {
                result = string.Join(",", dt.AsEnumerable().Select(x => x.Field<int>("sEnrollsProfessionID")).ToArray());
            }
            return result;
        }

        /// <summary>
        /// 得到缴费方案
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataTable(string cmdText)
        {
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
        }

        /// <summary>
        /// 得到报名专业的缴费单状态
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <returns></returns>
        public static string GetsOrderStatus(string sEnrollsProfessionID)
        {
            string returnstr = string.Empty;
            DataTable sOrderDt = sOrderBLL.sOrderTableByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID AND Status!=9", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", sEnrollsProfessionID) }, "");
            if (sOrderDt.Rows.Count > 0)
            {
                returnstr = "未更新";
                //得到报名专业的缴费方案ID，和方案对应的缴费期数ID字符串（按Queue正序排列）,从T_Stu_sItemsEnroll表取方案
                string cmdText = @"SELECT  item.ItemID ,
        ( SELECT TOP ( 1000 )
                    CAST(ItemID AS NVARCHAR) + ','
          FROM      T_Pro_Item
          WHERE     ParentID = item.ItemID
                    AND Status = 1
          ORDER BY  Queue ASC
        FOR
          XML PATH('')
        ) AS ItemNumID
FROM    T_Pro_Item AS item
WHERE   item.ItemID IN ( SELECT ItemID
                         FROM   T_Stu_sItemsEnroll
                         WHERE  sEnrollsProfessionID = {0} )
        AND item.Status = 1
ORDER BY item.ItemID ASC";
                cmdText = string.Format(cmdText, sEnrollsProfessionID);
                DataTable schemeDt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
                if (schemeDt.Rows.Count > 0)
                {
                    int[] array = sOrderDt.AsEnumerable().OrderBy(x => x.Field<int>("PlanItemID")).Select(x => x.Field<int>("PlanItemID")).Distinct().ToArray();
                    if (schemeDt.Rows.Count == array.Length)
                    {
                        int temp = 0;
                        for (var i = 0; i < schemeDt.Rows.Count; i++)
                        {
                            if (schemeDt.Rows[i]["ItemID"].ToString().Equals(array[i].ToString()))
                            {
                                //拼接缴费单中方案的缴费期数字符串，按ItemQueue正序排列
                                string buildItemIDs = string.Join(",", sOrderDt.AsEnumerable().Where(x => x.Field<int>("PlanItemID").ToString() == schemeDt.Rows[i]["ItemID"].ToString()).OrderBy(x => x.Field<int>("ItemQueue")).Select(x => x.Field<int>("NumItemID")).Distinct().ToArray());
                                string oldItemIDs = schemeDt.Rows[i]["ItemNumID"] == null ? "" : schemeDt.Rows[i]["ItemNumID"].ToString().TrimEnd(',');
                                if (buildItemIDs.Equals(oldItemIDs))
                                {
                                    temp++;
                                }
                            }
                        }
                        if (temp == schemeDt.Rows.Count)
                        {
                            returnstr = "已生成";
                        }
                    }
                }
            }
            else
            {
                returnstr = "未生成";
            }
            return returnstr;
        }

        /// <summary>
        /// 根据报名专业ID判断该学生是否有身份证号码
        /// </summary>
        /// <param name="sEnrollsProfessionID"></param>
        /// <returns></returns>
        public static bool CheckIsHaveIDCard(string sEnrollsProfessionID)
        {
            string cmdText = @"SELECT  ISNULL(student.IDCard, '') AS IDCard
FROM    T_Stu_sEnrollsProfession AS spro
        LEFT JOIN T_Stu_sEnroll AS senroll ON senroll.sEnrollID = spro.sEnrollID
        LEFT JOIN T_Pro_Student AS student ON student.StudentID = senroll.StudentID
WHERE   spro.sEnrollsProfessionID = {0}";
            cmdText = string.Format(cmdText, sEnrollsProfessionID);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 生成报名号（学号）
        /// </summary>
        /// <param name="dept"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string getEnrollNum(string dept, string year, string userId)
        {
            string s = "";
            string num = string.Empty;
            string zero = string.Empty;
            DeptModel dm = DeptBLL.DeptModelByWhere(" AND DeptID=@DeptID", new SqlParameter[] { new SqlParameter("@DeptID", dept) });
            string parentDeptID = dm.ParentID;
            sEnrollNumModel model = sEnrollNumBLL.sEnrollNumModelByWhere(" AND DeptID=@DeptID AND Year=@Year", new SqlParameter[] { new SqlParameter("@DeptID", parentDeptID), new SqlParameter("@Year", year) });
            if (!string.IsNullOrEmpty(model.sEnrollNumID))
            {
                s = sEnrollNumBLL.UpdateEnrollNumToNum(model.sEnrollNumID);
            }
            else
            {
                model.Status = "1";
                model.DeptID = parentDeptID;
                model.Year = year;
                model.Num = "1";
                model.CreateID = userId;
                model.CreateTime = DateTime.Now.ToString();
                model.UpdateID = userId;
                model.UpdateTime = DateTime.Now.ToString();
                sEnrollNumBLL.InsertsEnrollNum(model);
                s = "1";
            }

            DeptModel deptmodel = DeptBLL.DeptModelByWhere(" AND DeptID=@DeptID", new SqlParameter[] { new SqlParameter("@DeptID", parentDeptID) });
            switch (s.Length)
            {
                case 1:
                    zero = "0000";
                    break;
                case 2:
                    zero = "000";
                    break;
                case 3:
                    zero = "00";
                    break;
                case 4:
                    zero = "0";
                    break;
            }
            return deptmodel.Code + year.Substring(2, 2) + zero + s;
        }

        /// <summary>
        /// 根据学生考生号返回学生缴费方案
        /// </summary>
        /// <param name="examNum"></param>
        /// <returns></returns>
        public static DataTable getEnrollData(string examNum, string deptId)
        {
            string cmdText = @"SELECT  ItemID ,
        sEnrollsProfessionID
FROM    T_Stu_sItemsEnroll
WHERE   sEnrollsProfessionID IN (
        SELECT  sEnrollsProfessionID
        FROM    T_Stu_sEnrollsProfession
        WHERE   Status <> 9
                AND sEnrollID IN (
                SELECT  sEnrollID
                FROM    T_Stu_sEnroll
                WHERE   Status = 1
                        AND DeptID = @DeptID
                        AND ( ExamNum = @ExamNum
                              OR StudentID IN (
                              SELECT    s.StudentID
                              FROM      T_Pro_Student s
                              WHERE     s.Status = 1
                                        AND s.IDCard = @ExamNum )
                            ) ) )";

            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ExamNum",examNum),
                 new SqlParameter("@DeptID",deptId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }
        /// <summary>
        /// 根据学生id返回学生报名信息
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static DataTable GetEnrollTable(string studentId)
        {
            string where = " and StudentID=@StudentID and Status=1 ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",studentId)
            };
            return sEnrollBLL.sEnrollTableByWhere(where, paras, "");
        }
        /// <summary>
        /// 根据报名id返回报名信息
        /// </summary>
        /// <param name="senrollId"></param>
        /// <returns></returns>
        public static sEnrollModel GetEnrollModel(string senrollId)
        {
            string where = " and sEnrollID=@sEnrollID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollID",senrollId)
            };
            return sEnrollBLL.sEnrollModelByWhere(where, paras);
        }

        /// <summary>
        /// 转专业
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="MajorScheme"></param>
        /// <param name="OldID"></param>
        /// <param name="MajorChangeTime"></param>
        /// <param name="MajorExplain"></param>
        /// <returns></returns>
        public static string ChangesEnrollProfession(sEnrollsProfessionModel model, string userId, string MajorScheme, string OldID, string MajorChangeTime, string MajorExplain, string[] readYear)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                string resultID = "";
                try
                {
                    sEnrollsProfessionModel spm = sEnrollsProfessionBLL.sEnrollsProfessionModelByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", OldID) });
                    //插入T_Stu_sEnrollsProfession（报名专业表）
                    model.Status = (spm.Status == "1" ? "3" : spm.Status);
                    if (model.DeptID == spm.DeptID)//同校区转专业，招生部门不变，跨校区转专业，招生部门变为新主体主校区
                    {
                        model.DeptAreaID = spm.DeptAreaID;
                    }
                    else
                    {
                        model.DeptAreaID = DeptAreaBLL.GetFirstDeptArea(model.DeptID);
                    }
                    if (!string.IsNullOrEmpty(model.Remark))
                    {
                        model.Remark = model.Remark.Replace("\r\n", "<br />");
                    }
                    model.EnrollTime = spm.EnrollTime;
                    model.BeforeEnrollTime = spm.BeforeEnrollTime;
                    model.FirstFeeTime = spm.FirstFeeTime;
                    model.Auditor = userId;
                    model.AuditTime = DateTime.Now.ToString();
                    model.AuditView = "";
                    model.CreateTime = DateTime.Now.ToString();
                    model.UpdateID = userId;
                    model.CreateID = userId;
                    model.UpdateTime = DateTime.Now.ToString();
                    int NewID = sEnrollsProfessionBLL.InsertsEnrollsProfession(model);
                    resultID = NewID.ToString();
                    //插入T_Stu_sItemsEnroll（报名专业缴费方案表）
                    //{"total":1,"rows":[{"ItemID":"2","Year":"2016","Month":"3月（春季）","Name":"缴费方案1"}]}
                    JObject job = JObject.Parse(MajorScheme);
                    string[] array = job.Properties().Where(x => x.Name == "rows").Select(item => item.Value.ToString()).ToArray();
                    List<sEnrollSchemeModel> schemeList = JsonConvert.DeserializeObject<List<sEnrollSchemeModel>>(array[0]);//将json数据转化为对象类型并赋值给list
                    string tempSchemeId = string.Empty;
                    foreach (var item in schemeList)
                    {
                        sItemsEnrollModel siemmodel = new sItemsEnrollModel();
                        siemmodel.ItemID = item.ItemID;
                        siemmodel.sEnrollsProfessionID = NewID.ToString();
                        sItemsEnrollBLL.InsertsItemsEnroll(siemmodel);
                        tempSchemeId += item.ItemID + ",";
                    }
                    tempSchemeId = tempSchemeId.Substring(0, tempSchemeId.Length - 1);
                    DataTable itemTable = ItemBLL.GetItemChildModel(tempSchemeId);
                    if (itemTable.Rows.Count > 0)
                    {
                        DateTime date = Convert.ToDateTime(itemTable.Rows[0]["LimitTime"].ToString());
                        string leaveYear = (date.Year + 1).ToString();
                        sEnrollsProfessionModel newepm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(NewID.ToString());
                        newepm.LeaveYear = leaveYear;
                        newepm.UpdateID = userId;
                        newepm.UpdateTime = DateTime.Now.ToString();
                        sEnrollsProfessionBLL.UpdatesEnrollsProfession(newepm);

                    }
                    //插入T_Stu_sEnrollsProfessionNote（停用记录表）
                    sEnrollsProfessionNoteModel pm = new sEnrollsProfessionNoteModel();
                    pm.Status = "1";
                    pm.sEnrollsProfessionID = OldID;
                    pm.NewsEnrollsProfessionID = NewID.ToString();
                    pm.Sort = "1";//类型：转专业
                    pm.NoteTime = MajorChangeTime;
                    pm.Explain = MajorExplain.Replace("\r\n", "<br />");
                    pm.CreateID = userId;
                    pm.CreateTime = DateTime.Now.ToString();
                    pm.UpdateID = userId;
                    pm.UpdateTime = DateTime.Now.ToString();
                    sEnrollsProfessionNoteBLL.InsertsEnrollsProfessionNote(pm);

                    //插入T_Stu_sEnrollsProfessionsOrder（停用报名专业缴费单表）
                    DataTable sOrderDt = sOrderBLL.sOrderTableByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID AND Status!=9", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", OldID) }, "");
                    if (sOrderDt.Rows.Count > 0)
                    {
                        for (var i = 0; i < sOrderDt.Rows.Count; i++)
                        {
                            sEnrollsProfessionsOrderModel pom = new sEnrollsProfessionsOrderModel();
                            pom.sEnrollsProfessionID = OldID;
                            pom.sOrderID = sOrderDt.Rows[i]["sOrderID"].ToString();
                            string isNumItem = "2";
                            for (int j = 0; j < readYear.Length; j++)
                            {
                                if (sOrderDt.Rows[i]["NumItemID"].ToString().Equals(readYear[j]))
                                {
                                    isNumItem = "1";
                                }
                            }
                            pom.IsNumItem = isNumItem;
                            sEnrollsProfessionsOrderBLL.InsertsEnrollsProfessionsOrder(pom);
                        }
                    }
                    //插入T_Stu_sEnrollsProfessionsOrderGive（停用报名专业缴费单配品表）
                    DataTable sOrderGiveDt = sOrderGiveBLL.sOrderGiveTableByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID AND Status!=9", new SqlParameter[] { new SqlParameter("@sEnrollsProfessionID", OldID) }, "");
                    if (sOrderGiveDt.Rows.Count > 0)
                    {
                        for (var i = 0; i < sOrderGiveDt.Rows.Count; i++)
                        {
                            sEnrollsProfessionsOrderGiveModel pgm = new sEnrollsProfessionsOrderGiveModel();
                            pgm.sEnrollsProfessionID = OldID;
                            pgm.sOrderGiveID = sOrderGiveDt.Rows[i]["sOrderGiveID"].ToString();
                            sEnrollsProfessionsOrderGiveBLL.InsertsEnrollsProfessionsOrderGive(pgm);
                        }
                    }
                    //停用要停掉已生成的缴费单数据，停掉已生成的配品数据
                    spm.Status = "9";
                    spm.UpdateID = userId;
                    spm.UpdateTime = DateTime.Now.ToString();
                    sEnrollsProfessionBLL.UpdatesEnrollsProfession(spm);
                    sOrderBLL.UpdateStatus(OldID, "9");
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    throw ex;

                }
                finally
                {
                    ts.Dispose();
                }

                return resultID;
            }

        }

        public static DataTable GetsEnrollTable(string studentId)
        {
            string where = " and StudentID=@StudentID and Status=1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter ("@StudentID",studentId)
            };
            return sEnrollBLL.sEnrollTableByWhere(where, paras, "");
        }



        public static DataTable ChangeMajorYearCombobox(string senrollProfessionId)
        {
            string cmdText = @"SELECT  *
FROM    ( SELECT  DISTINCT
                    r1.RefeName + '_' + o.NumName text ,
                    o.NumItemID id ,
                    o.ItemQueue
          FROM      T_Stu_sOrder o
                    LEFT JOIN T_Sys_Refe r1 ON r1.Value = o.PlanLevel
                                                   AND r1.RefeTypeID = 17
          WHERE     sEnrollsProfessionID = @sEnrollsProfessionID  AND o.Status<>9
          UNION ALL
          SELECT    '统招新生报到/成教' ,
                    0 ,
                    0
        ) d
ORDER BY ItemQueue ASC ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",senrollProfessionId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        public static DataTable GetsFeeInfo(string senrollsProfessionId)
        {

            string where = string.Empty;

            if (sEnrollsProfessionsOrderBLL.ValidatesOrderId(senrollsProfessionId))
            {
                where = @" SELECT    sOrderID
                                  FROM      T_Stu_sEnrollsProfessionsOrder
                                  WHERE     sEnrollsProfessionID =@sEnrollsProfessionID";
            }
            else
            {
                where = @"SELECT sOrderID FROM T_Stu_sOrder WHERE sEnrollsProfessionID=@sEnrollsProfessionID  AND Status<>9";
            }

            string cmdText = @"SELECT  ( SELECT    ISNULL(SUM(ShouldMoney), 0)
          FROM      T_Stu_sOrder
          WHERE     sOrderID IN ( {0} )
        ) ShouldMoney ,
        ( SELECT    ISNULL(SUM(PaidMoney), 0)
          FROM      T_Stu_sFeesOrder
          WHERE     sOrderID IN ( {0} )
        ) PaidMoney ,
        ( SELECT    ISNULL(SUM(DiscountMoney), 0)
          FROM      T_Stu_sFeesOrder
          WHERE     sOrderID IN ({0} )
        ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Stu_sOffset
          WHERE     RelatedID IN (
                    SELECT  sFeesOrderID
                    FROM    T_Stu_sFeesOrder fo
                    WHERE   fo.sOrderID IN (
                            {0} )
                            AND fo.Status = 1 )
                    AND Status = 1
        ) OffsetMoney ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Stu_sOffset
          WHERE     ByRelatedID IN (
                    SELECT  sFeesOrderID
                    FROM    T_Stu_sFeesOrder fo
                    WHERE   fo.sOrderID IN (
                            {0} )
                            AND fo.Status = 1 )
                    AND Status = 1
                    AND BySort = 1
        ) ByOffsetMoney";
            cmdText = string.Format(cmdText, where);
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sEnrollsProfessionID",senrollsProfessionId)
            };
            return DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
        }

        /// <summary>
        /// 报名审查
        /// </summary>
        /// <param name="message"></param>
        public static string EnrollReview(string data)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(data))
            {
                return "数据不能为空";
            }
            try
            {
                VerifyModel eam = JsonConvert.DeserializeObject<VerifyModel>(data);
                if (string.IsNullOrEmpty(eam.IdCard))
                {
                    message += "身份证号不能为空;";
                }
                if (string.IsNullOrEmpty(eam.Major))
                {
                    message += "专业不能为空;";
                }
                if (string.IsNullOrEmpty(eam.OrgId))
                {
                    message += "主体不能为空;";
                }
                else
                {
                    eam.OrgId = DeptDCPBLL.GetDeptID(eam.OrgId).ToString();
                    if (eam.OrgId.Equals("0"))
                    {
                        message += "系统内没有找到对应的主体;";
                    }
                }
                StudentModel sm = StudentBLL.GetStudentModel(eam.IdCard);//获取学生信息
                if (string.IsNullOrEmpty(sm.StudentID))
                {
                    message += "学生不存在;";
                }
                else
                {
                    if (string.IsNullOrEmpty(eam.Name))
                    {
                        message += "姓名不能为空;";
                    }
                    else
                    {
                        if (!sm.Name.Replace(" ", "").Equals(eam.Name.Replace(" ", "")))
                        {
                            message += "姓名和身份证不匹配;";
                        }
                    }
                }
                if (string.IsNullOrEmpty(message))
                {

                    DataTable enrollTab = sEnrollBLL.GetEnrollTable(sm.StudentID);//获取学生报名信息
                    if (enrollTab.Rows.Count == 1)
                    {
                        DataTable senrollsProfessionTab = sEnrollsProfessionBLL.GetsEnrollProfessionTable(enrollTab.Rows[0]["sEnrollID"].ToString());//获取报名专业信息
                        sEnrollModel em = sEnrollBLL.GetEnrollModel(enrollTab.Rows[0]["sEnrollID"].ToString());
                        if (senrollsProfessionTab.Rows.Count == 1)
                        {
                            if (senrollsProfessionTab.Rows[0]["Status"].ToString().Equals("1"))//报名状态为录取更新报名时间
                            {
                                if (string.IsNullOrEmpty(enrollTab.Rows[0]["EnrollNum"].ToString()))
                                {
                                    em.EnrollNum = sEnrollBLL.getEnrollNum(senrollsProfessionTab.Rows[0]["DeptID"].ToString(), senrollsProfessionTab.Rows[0]["Year"].ToString(), "0");
                                    em.UpdateID = "0";
                                    em.UpdateTime = DateTime.Now.ToString();
                                    sEnrollBLL.UpdatesEnroll(em);//更新报名
                                }
                                sEnrollsProfessionBLL.UpdatesEnrollTime(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString());//更新报名时间
                            }

                            DataTable feeOrder = sOrderBLL.GetsOrderTable(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString(), true);//获取已缴费的订单
                            #region  未交费
                            string tempsEnrollProfessionId = string.Empty;
                            if (feeOrder.Rows.Count == 0)
                            {
                                string sprofessionId = sProfessionBLL.GetsProfesssionID(eam.Major, senrollsProfessionTab.Rows[0]["DeptID"].ToString(), senrollsProfessionTab.Rows[0]["Year"].ToString(), senrollsProfessionTab.Rows[0]["Month"].ToString());
                                if (string.IsNullOrEmpty(sprofessionId))
                                {
                                    message += "报名专业不存在;";
                                }
                                else
                                {
                                    if (sprofessionId.Equals(senrollsProfessionTab.Rows[0]["sProfessionID"].ToString()))//专业相同
                                    {
                                        DataTable nofeeOrder = sOrderBLL.GetsOrderTable(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString(), false);//获取未缴费的订单
                                        if (nofeeOrder.Rows.Count == 0)
                                        {
                                            sOrderBLL.BuildsOrder(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString(), "0");
                                        }
                                        sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString());
                                        epm.AuditView = "报名审查";
                                        epm.AuditTime = DateTime.Now.ToString();
                                        epm.Auditor = "0";
                                        sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                                        tempsEnrollProfessionId = senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString();
                                    }
                                    else
                                    {
                                        sEnrollsProfessionModel epm = new sEnrollsProfessionModel();
                                        epm.sEnrollID = senrollsProfessionTab.Rows[0]["sEnrollID"].ToString();
                                        epm.DeptID = senrollsProfessionTab.Rows[0]["DeptID"].ToString();
                                        epm.EnrollTime = senrollsProfessionTab.Rows[0]["EnrollTime"].ToString();
                                        epm.Year = senrollsProfessionTab.Rows[0]["Year"].ToString();
                                        epm.Month = senrollsProfessionTab.Rows[0]["Month"].ToString();
                                        epm.EnrollLevel = senrollsProfessionTab.Rows[0]["EnrollLevel"].ToString();
                                        epm.sProfessionID = sprofessionId;
                                        string type = "3";//2016写死，默认统招
                                        string level = "";
                                        DataTable itemProfessionTab = sItemsProfessionBLL.GetItemsProfessionTable(sprofessionId, type, level);
                                        string MajorScheme = "";
                                        if (itemProfessionTab.Rows.Count == 1)
                                        {
                                            MajorScheme = "{\"total\":1,\"rows\":[{\"ItemID\":\"" + itemProfessionTab.Rows[0]["ItemID"].ToString() + "\"}]}";
                                        }
                                        else
                                        {
                                            MajorScheme = "{\"total\":1,\"rows\":[]}";
                                        }
                                        string[] array = { "0" };
                                        string tempId = sEnrollBLL.ChangesEnrollProfession(epm, "0", MajorScheme, senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString(), DateTime.Now.ToString(), "报名时要求转专业", array);
                                        //sEnrollsProfessionBLL.UpdatesEnrollTime(tempId);//更新报名时间
                                        if (itemProfessionTab.Rows.Count == 1)
                                        {
                                            sOrderBLL.BuildsOrder(tempId, "0");
                                        }
                                        tempsEnrollProfessionId = tempId;
                                    }
                                }
                            }
                            #endregion
                            #region 已缴费
                            else
                            {
                                sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString());
                                if (string.IsNullOrEmpty(epm.EnrollTime))
                                {
                                    epm.EnrollTime = DateTime.Now.ToString();
                                }
                                else
                                {
                                    if (Convert.ToDateTime(epm.EnrollTime).ToString("yyyy-MM-dd").Equals("1900-01-01"))
                                    {
                                        epm.EnrollTime = DateTime.Now.ToString();
                                    }
                                }
                                epm.AuditView = "报名审查";
                                epm.AuditTime = DateTime.Now.ToString();
                                epm.Auditor = "0";
                                sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                                tempsEnrollProfessionId = senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString();
                                message += "已缴费;";
                            }
                            #endregion
                            sEnrollsProfessionBLL.UpdatesEnrollsProfessionStatus(tempsEnrollProfessionId, "0");
                        }
                        else if (senrollsProfessionTab.Rows.Count == 0)
                        {
                            message += "该学生报名专业为空;";
                        }
                        else
                        {
                            message += "该学生报多个专业;";
                        }
                    }
                    else if (enrollTab.Rows.Count > 1)//多次报名不处理
                    {
                        message += "该学生已经报过名;";
                    }
                    else
                    {//没有报名信息  不处理
                        message += "没有此学生的报名信息;";
                    }
                }
            }
            catch (Exception ex)
            {
                message += ex.Message;
                throw ex;
            }
            return message;
        }
    }
}
