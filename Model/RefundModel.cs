using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class RefundModel
    {
        public string RefundID { get; set; }
        public string Status { get; set; }
        public string FeeDetailID { get; set; }
        public string Sort { get; set; }
        public string RefundMoney { get; set; }
        public string RefundTime { get; set; }
        public string PayObject { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public RefundModel()
        { }
        public RefundModel(string refundid, string status, string feedetailid, string sort, string refundmoney, string refundtime, string payobject, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.RefundID = refundid;
            this.Status = status;
            this.FeeDetailID = feedetailid;
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
