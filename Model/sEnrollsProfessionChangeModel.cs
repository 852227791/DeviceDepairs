using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model 
{ 

	public class  sEnrollsProfessionChangeModel
	{ 
		public string sEnrollsProfessionChangeID { get;set;}
		public string Status { get;set;}
		public string sEnrollsProfessionID { get;set;}
		public string CreateID { get;set;}
		public string CreateTime { get;set;}
		public string UpdateID { get;set;}
		public string UpdateTime { get;set;}

		public sEnrollsProfessionChangeModel(){ } 

		public  sEnrollsProfessionChangeModel(string senrollsprofessionchangeid,string status,string senrollsprofessionid,string createid,string createtime,string updateid,string updatetime)
		{
			this.sEnrollsProfessionChangeID=senrollsprofessionchangeid;
			this.Status=status;
			this.sEnrollsProfessionID=senrollsprofessionid;
			this.CreateID=createid;
			this.CreateTime=createtime;
			this.UpdateID=updateid;
			this.UpdateTime=updatetime;
		} 
	} 
} 
