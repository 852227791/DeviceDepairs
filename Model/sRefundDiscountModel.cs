using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model 
{ 

	public class  sRefundDiscountModel
	{ 
		public string sRefundDiscountID { get;set;}
		public string Status { get;set;}
		public string sFeesOrderID { get;set;}
		public string Sort { get;set;}
		public string RefundMoney { get;set;}
		public string RefundTime { get;set;}
		public string PayObject { get;set;}
		public string Remark { get;set;}
		public string CreateID { get;set;}
		public string CreateTime { get;set;}
		public string UpdateID { get;set;}
		public string UpdateTime { get;set;}

		public sRefundDiscountModel(){ } 

		public  sRefundDiscountModel(string srefunddiscountid,string status,string sfeesorderid,string sort,string refundmoney,string refundtime,string payobject,string remark,string createid,string createtime,string updateid,string updatetime)
		{
			this.sRefundDiscountID=srefunddiscountid;
			this.Status=status;
			this.sFeesOrderID=sfeesorderid;
			this.Sort=sort;
			this.RefundMoney=refundmoney;
			this.RefundTime=refundtime;
			this.PayObject=payobject;
			this.Remark=remark;
			this.CreateID=createid;
			this.CreateTime=createtime;
			this.UpdateID=updateid;
			this.UpdateTime=updatetime;
		} 


	}

    public class sRefundDiscountUploadCH
    {
        public string 凭证号 { get; set; }
        public string 收费项目 { get; set; }
        public string 系统备注 { get; set; }
        public string 核销类别 { get; set; }
        public string 核销金额 { get; set; }
        public string 核销时间 { get; set; }
        public string 支付对象 { get; set; }
        public string 备注 { get; set; }
    }

    public class sRefundDiscountUploadModel
    {
        public string voucherNum { get; set; }
        public string feeOrder { get; set; }
        public string refundType { get; set; }
        public string money { get; set; }
        public string time { get; set; }
        public string payObj { get; set; }
        public string remark { get; set; }
    }

    public class FeeRefundSearchModel
    {

        public string name { get; set; }

        public string vouchernum { get; set; }
        public string idCard { get; set; }
        public string refundType { get; set; }
        public string noteNum { get; set; }
        public string refundTimeE { get; set; }

        public string refundTimeS { get; set; }

        public string deptId { get; set; }

        public string refundName { get; set; }

    }
} 
