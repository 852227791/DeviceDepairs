using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model 
{ 

	public class  DiscountDetailModel
	{ 
		public string DiscountDetailID { get;set;}
		public string Status { get;set;}
		public string DiscountPlanID { get;set;}
		public string NumItemID { get;set;}
		public string ItemDetaiID { get;set;}
		public string Money { get;set;}
		public string CreateID { get;set;}
		public string CreateTime { get;set;}
		public string UpdateID { get;set;}
		public string UpdateTime { get;set;}

		public DiscountDetailModel(){ } 

		public  DiscountDetailModel(string discountdetailid,string status,string discountplanid,string numitemid,string itemdetaiid,string money,string createid,string createtime,string updateid,string updatetime)
		{
			this.DiscountDetailID=discountdetailid;
			this.Status=status;
			this.DiscountPlanID=discountplanid;
			this.NumItemID=numitemid;
			this.ItemDetaiID=itemdetaiid;
			this.Money=money;
			this.CreateID=createid;
			this.CreateTime=createtime;
			this.UpdateID=updateid;
			this.UpdateTime=updatetime;
		} 

        
	}

    public class DiscountDetail {
        public string DiscountDetailID { get; set; }
        public string ItemDetailID { get; set; }
        public string ItemID { get; set; }
        public string NumName { get; set; }
        public string Name { get; set; }
        public string ShouldMoney { get; set; }
        public string DiscountMoney { get; set; }

    }
} 
