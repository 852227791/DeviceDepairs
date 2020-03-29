using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model 
{ 

	public class  sOrderCreateModel
	{ 
		public string sOrderCreateID { get;set;}
		public string DeptID { get;set;}

		public sOrderCreateModel(){ } 

		public  sOrderCreateModel(string sordercreateid,string deptid)
		{
			this.sOrderCreateID=sordercreateid;
			this.DeptID=deptid;
		} 
	} 
} 
