using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model 
{ 

	public class  DiscountPlanModel
	{ 
		public string DiscountPlanID { get;set;}
		public string Status { get;set;}
		public string Name { get;set;}
		public string PlanItemID { get;set;}
		public string CreateID { get;set;}
		public string CreateTime { get;set;}
		public string UpdateID { get;set;}
		public string UpdateTime { get;set;}

		public DiscountPlanModel(){ } 

		public  DiscountPlanModel(string discountplanid,string status,string name,string planitemid,string createid,string createtime,string updateid,string updatetime)
		{
			this.DiscountPlanID=discountplanid;
			this.Status=status;
			this.Name=name;
			this.PlanItemID=planitemid;
			this.CreateID=createid;
			this.CreateTime=createtime;
			this.UpdateID=updateid;
			this.UpdateTime=updatetime;
		} 
	} 
} 
