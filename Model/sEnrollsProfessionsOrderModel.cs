using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model 
{ 

	public class  sEnrollsProfessionsOrderModel
	{ 
		public string sEnrollsProfessionsOrderID { get;set;}
		public string sEnrollsProfessionID { get;set;}
		public string sOrderID { get;set;}
		public string IsNumItem { get;set;}

		public sEnrollsProfessionsOrderModel(){ } 

		public  sEnrollsProfessionsOrderModel(string senrollsprofessionsorderid,string senrollsprofessionid,string sorderid,string isnumitem)
		{
			this.sEnrollsProfessionsOrderID=senrollsprofessionsorderid;
			this.sEnrollsProfessionID=senrollsprofessionid;
			this.sOrderID=sorderid;
			this.IsNumItem=isnumitem;
		} 
	} 
} 
