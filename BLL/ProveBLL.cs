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
    public class ProveBLL
    {
        public static int InsertProve(ProveModel pm)
        {
            string cmdText = @"INSERT INTO T_Pro_Prove
(Status
,DeptID
,ClassID
,EnrollTime
,StudentID
,ItemID
,IsForce	
,ProveNum
,Remark
,CreateID
,CreateTime
,UpdateID
,UpdateTime
)
VALUES (@Status
,@DeptID
,@ClassID
,@EnrollTime
,@StudentID
,@ItemID
,@IsForce	
,@ProveNum
,@Remark
,@CreateID
,@CreateTime
,@UpdateID
,@UpdateTime
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@Status", pm.Status),
new SqlParameter("@DeptID", pm.DeptID),
new SqlParameter("@ClassID", pm.ClassID),
new SqlParameter("@EnrollTime", pm.EnrollTime),
new SqlParameter("@StudentID", pm.StudentID),
new SqlParameter("@ItemID", pm.ItemID),
new SqlParameter("@IsForce", pm.IsForce),
new SqlParameter("@ProveNum", pm.ProveNum),
new SqlParameter("@Remark", pm.Remark),
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

        public static int UpdateProve(ProveModel pm)
        {
            string cmdText = @"UPDATE T_Pro_Prove SET
Status=@Status
,DeptID=@DeptID
,ClassID=@ClassID
,EnrollTime=@EnrollTime
,StudentID=@StudentID
,ItemID=@ItemID
,IsForce=@IsForce
,ProveNum=@ProveNum
,Remark=@Remark
,CreateID=@CreateID
,CreateTime=@CreateTime
,UpdateID=@UpdateID
,UpdateTime=@UpdateTime
WHERE ProveID=@ProveID";
            SqlParameter[] paras = new SqlParameter[] { 
new SqlParameter("@ProveID", pm.ProveID),
new SqlParameter("@Status", pm.Status),
new SqlParameter("@DeptID", pm.DeptID),
new SqlParameter("@ClassID", pm.ClassID),
new SqlParameter("@EnrollTime", pm.EnrollTime),
new SqlParameter("@StudentID", pm.StudentID),
new SqlParameter("@ItemID", pm.ItemID),
new SqlParameter("@IsForce", pm.IsForce),
new SqlParameter("@ProveNum", pm.ProveNum),
new SqlParameter("@Remark", pm.Remark),
new SqlParameter("@CreateID", pm.CreateID),
new SqlParameter("@CreateTime", pm.CreateTime),
new SqlParameter("@UpdateID", pm.UpdateID),
new SqlParameter("@UpdateTime", pm.UpdateTime)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(pm.ProveID);
            }
            else
            {
                return -1;
            }
        }

        public static ProveModel ProveModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            ProveModel pm = new ProveModel();
            string cmdText = "SELECT * FROM T_Pro_Prove WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                pm.ProveID = dt.Rows[0]["ProveID"].ToString();
                pm.Status = dt.Rows[0]["Status"].ToString();
                pm.DeptID = dt.Rows[0]["DeptID"].ToString();
                pm.ClassID = dt.Rows[0]["ClassID"].ToString();
                pm.EnrollTime = dt.Rows[0]["EnrollTime"].ToString();
                pm.StudentID = dt.Rows[0]["StudentID"].ToString();
                pm.ItemID = dt.Rows[0]["ItemID"].ToString();
                pm.IsForce = dt.Rows[0]["IsForce"].ToString();
                pm.ProveNum = dt.Rows[0]["ProveNum"].ToString();
                pm.Remark = dt.Rows[0]["Remark"].ToString();
                pm.CreateID = dt.Rows[0]["CreateID"].ToString();
                pm.CreateTime = dt.Rows[0]["CreateTime"].ToString();
                pm.UpdateID = dt.Rows[0]["UpdateID"].ToString();
                pm.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
            }
            return pm;
        }

        public static DataTable ProveTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Prove WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }


        public static DataTable ProveStudentTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT  p.ProveID ,
        p.Status ,
        p.DeptID ,
        ISNULL(c.ProfessionID, 0) ProfessionID ,
        p.ClassID ,
        CONVERT(NVARCHAR(23), p.EnrollTime, 23) EnrollTime ,
        p.StudentID ,
        p.ItemID ,
        p.IsForce ,
        p.Remark ,
        s.Name + '_' + s.IDCard Name,
		si.Photo
FROM    T_Pro_Prove p
        LEFT JOIN T_Pro_Student s ON p.StudentID = s.StudentID
        LEFT JOIN T_Pro_Class c ON p.ClassID = c.ClassID
		LEFT JOIN T_Pro_StudentInfo si ON si.StudentID=s.StudentID
WHERE   1 = 1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }


        public static string SelectProveNum(string proveId, string studentId, string itemId)
        {
            string cmdText = @"SELECT  COUNT(ProveID) Num
FROM    T_Pro_Prove
WHERE   Status <> 9
        AND ProveID <> @ProveID
        AND StudentID = @StudentID
        AND ItemID = @ItemID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@ProveID", proveId),
                new SqlParameter("@StudentID", studentId),
                new SqlParameter("@ItemID", itemId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt.Rows[0]["Num"].ToString();
        }

        #region 获取证书实体类
        /// <summary>
        /// 获取证书实体类
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static ProveModel GetProveModel(string studentId, string itemId)
        {
            string where = " and StudentID=@StudentID and ItemID=@ItemID";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@StudentID",studentId),
            new SqlParameter("@ItemID",itemId)
            };
            ProveModel pm = ProveBLL.ProveModelByWhere(where, paras);
            return pm;
        }
        #endregion


        public static DataTable ProveTable(string studentId, string itemId)
        {
            string where = " and Status in (1,2) and StudentID=@StudentID and ItemID IN(" + itemId + ")";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@StudentID",studentId)
            };
            DataTable dt = ProveBLL.ProveTableByWhere(where, paras, "");
            return dt;
        }

        public static DataTable GetProveTable(string studentId, string itemId)
        {
            string cmdText = @"SELECT  pf.ProveID,i.Name
FROM    T_Pro_Prove pf
        LEFT JOIN T_Pro_Item i ON pf.ItemID = i.ItemID where  pf.Status in (1,2) and pf.StudentID=@StudentID and pf.ItemID=@ItemID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@StudentID",studentId),
                new SqlParameter("@ItemID",itemId)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }
        /// <summary>
        /// /获取证书，不加状态
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static DataTable GetProveTablenew(string studentId, string itemId)
        {
            string where = " and StudentID=@StudentID and ItemID=@ItemID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",studentId),
                new SqlParameter("@ItemID",itemId)
            };
            DataTable dt = ProveBLL.ProveTableByWhere(where, paras, "");
            return dt;
        }

        /// <summary>
        /// 根据收费明细ID输出证书ID
        /// </summary>
        /// <param name="FeeDetailID"></param>
        /// <returns></returns>
        public static string ProveIDByFeeDetailID(string FeeDetailID)
        {
            string cmdText = @"SELECT  p.ProveID
FROM    T_Pro_Prove p
        LEFT JOIN T_Pro_Fee f ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_FeeDetail fd ON f.FeeID = fd.FeeID
WHERE   fd.FeeDetailID = @FeeDetailID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@FeeDetailID", FeeDetailID)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt.Rows[0]["ProveID"].ToString();
        }


        public static DataTable ProveIDByFeeID(string FeeID)
        {
            string cmdText = @"SELECT  p.ProveID
FROM    T_Pro_Prove p
        LEFT JOIN T_Pro_Fee f ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_FeeDetail fd1 ON f.FeeID = fd1.FeeID
        LEFT JOIN T_Pro_Offset o ON fd1.FeeDetailID = o.ByFeeDetailID
        LEFT JOIN T_Pro_FeeDetail fd2 ON o.FeeDetailID = fd2.FeeDetailID
WHERE   fd2.FeeID = @FeeID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@FeeID", FeeID)
            };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static ProveModel GetProveModel(string proveId) {
            string where = " and ProveID=@ProveID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ProveID",proveId)
            };
            return ProveBLL.ProveModelByWhere(where, paras);
        }
    }
}
