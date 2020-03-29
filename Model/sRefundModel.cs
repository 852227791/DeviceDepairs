using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class sRefundModel
    {
        public string sRefundID { get; set; }
        public string Status { get; set; }
        public string sFeesOrderID { get; set; }
        public string Sort { get; set; }
        public string RefundMoney { get; set; }
        public string RefundTime { get; set; }
        public string PayObject { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sRefundModel()
        { }
        public sRefundModel(string srefundid, string status, string sfeesorderid, string sort, string refundmoney, string refundtime, string payobject, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.sRefundID = srefundid;
            this.Status = status;
            this.sFeesOrderID = sfeesorderid;
            this.Sort = sort;
            this.RefundMoney = refundmoney;
            this.RefundTime = refundtime;
            this.PayObject = payobject;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
