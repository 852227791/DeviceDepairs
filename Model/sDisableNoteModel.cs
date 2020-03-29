using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model 
{ 

	public class  sDisableNoteModel
	{ 
		public string sDisableNoteID { get;set;}
		public string Status { get;set;}
		public string sFeeID { get;set;}
		public string NoteNum { get;set;}
		public string CreateID { get;set;}
		public string CreateTime { get;set;}
		public string UpdateID { get;set;}
		public string UpdateTime { get;set;}

		public sDisableNoteModel(){ } 

		public  sDisableNoteModel(string sdisablenoteid,string status,string sfeeid,string notenum,string createid,string createtime,string updateid,string updatetime)
		{
			this.sDisableNoteID=sdisablenoteid;
			this.Status=status;
			this.sFeeID=sfeeid;
			this.NoteNum=notenum;
			this.CreateID=createid;
			this.CreateTime=createtime;
			this.UpdateID=updateid;
			this.UpdateTime=updatetime;
		} 
	} 
} 
