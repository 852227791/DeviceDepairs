using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model 
{ 

	public class  StudentInfoModel
	{ 
		public string StudentInfoID { get;set;}
		public string Status { get;set;}
		public string StudentID { get;set;}
		public string Nation { get;set;}
		public string ProvinceID { get;set;}
		public string CityID { get;set;}
		public string DistrictID { get;set;}
		public string Zip { get;set;}
		public string School { get;set;}
		public string Photo { get;set;}
		public string CreateID { get;set;}
		public string CreateTime { get;set;}
		public string UpdateID { get;set;}
		public string UpdateTime { get;set;}

		public StudentInfoModel(){ } 

		public  StudentInfoModel(string studentinfoid,string status,string studentid,string nation,string provinceid,string cityid,string districtid,string zip,string school,string photo,string createid,string createtime,string updateid,string updatetime)
		{
			this.StudentInfoID=studentinfoid;
			this.Status=status;
			this.StudentID=studentid;
			this.Nation=nation;
			this.ProvinceID=provinceid;
			this.CityID=cityid;
			this.DistrictID=districtid;
			this.Zip=zip;
			this.School=school;
			this.Photo=photo;
			this.CreateID=createid;
			this.CreateTime=createtime;
			this.UpdateID=updateid;
			this.UpdateTime=updatetime;
		} 
	} 
} 
