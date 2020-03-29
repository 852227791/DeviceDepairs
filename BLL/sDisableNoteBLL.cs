using System;
using Model;
using Common;
using DAL;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{

	public class sDisableNoteBLL
	{
		public static int InsertsDisableNote(sDisableNoteModel dnm)		{
 			string cmdText = @"INSERT INTO T_Stu_sDisableNote
			(Status
			,sFeeID
			,NoteNum
			,CreateID
			,CreateTime
			,UpdateID
			,UpdateTime
						)
			VALUES(@Status
			,@sFeeID
			,@NoteNum
			,@CreateID
			,@CreateTime
			,@UpdateID
			,@UpdateTime
						);SELECT CAST(scope_identity() AS int)";
			SqlParameter[] paras = new SqlParameter[] {
				new SqlParameter("@Status", dnm.Status)
				,new SqlParameter("@sFeeID", dnm.sFeeID)
				,new SqlParameter("@NoteNum", dnm.NoteNum)
				,new SqlParameter("@CreateID", dnm.CreateID)
				,new SqlParameter("@CreateTime", dnm.CreateTime)
				,new SqlParameter("@UpdateID", dnm.UpdateID)
				,new SqlParameter("@UpdateTime", dnm.UpdateTime)
							};
			return Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
		}

		public static int UpdatesDisableNote(sDisableNoteModel dnm)
		{
			string cmdText = @"UPDATE T_Stu_sDisableNote SET  Status=@Status
			,sFeeID=@sFeeID
			,NoteNum=@NoteNum
			,CreateID=@CreateID
			,CreateTime=@CreateTime
			,UpdateID=@UpdateID
			,UpdateTime=@UpdateTime
			Where sDisableNoteID=@sDisableNoteID " ;
			SqlParameter[] paras = new SqlParameter[] {
				new SqlParameter("@sDisableNoteID", dnm.sDisableNoteID)
				,new SqlParameter("@Status", dnm.Status)
				,new SqlParameter("@sFeeID", dnm.sFeeID)
				,new SqlParameter("@NoteNum", dnm.NoteNum)
				,new SqlParameter("@CreateID", dnm.CreateID)
				,new SqlParameter("@CreateTime", dnm.CreateTime)
				,new SqlParameter("@UpdateID", dnm.UpdateID)
				,new SqlParameter("@UpdateTime", dnm.UpdateTime)
				};
			return DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
		}

		public static List<sDisableNoteModel> SelectsDisableNoteByWhere(string where, SqlParameter[] paras, string queue)
		{
			DataTable dt = new DataTable();
			List<sDisableNoteModel> list=new List<sDisableNoteModel>();
			string cmdText = "SELECT * FROM T_Stu_sDisableNote WHERE 1 = 1 {0}{1}";
			cmdText = string.Format(cmdText, where, queue);
			dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				sDisableNoteModel dnm=new sDisableNoteModel();
				dnm.sDisableNoteID = dr["sDisableNoteID"].ToString();
				dnm.Status = dr["Status"].ToString();
				dnm.sFeeID = dr["sFeeID"].ToString();
				dnm.NoteNum = dr["NoteNum"].ToString();
				dnm.CreateID = dr["CreateID"].ToString();
				dnm.CreateTime = dr["CreateTime"].ToString();
				dnm.UpdateID = dr["UpdateID"].ToString();
				dnm.UpdateTime = dr["UpdateTime"].ToString();
				list.Add(dnm);
			}
			return list;
		}

		public static bool UpdatesDisableNoteStatus(string sDisableNoteID,string status,int userId)
		{
			string where=" and sDisableNoteID=@sDisableNoteID";
			SqlParameter[] paras = new SqlParameter[] {
				new SqlParameter("@sDisableNoteID", sDisableNoteID)
			};
			var list=sDisableNoteBLL.SelectsDisableNoteByWhere(where,paras,"").FirstOrDefault();
			if(list!=null)
			{
				list.Status=status;
				list.UpdateTime=DateTime.Now.ToString();
				list.UpdateID=userId.ToString();
				if(sDisableNoteBLL.UpdatesDisableNote(list).Equals(1))
					return true;
				return false;
			}
			else{
				return false;
			}
		}
	}
}
