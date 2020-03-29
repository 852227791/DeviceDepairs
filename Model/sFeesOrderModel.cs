using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class sFeesOrderModel
    {

        public string sFeesOrderID { get; set; }
        public string Status { get; set; }
        public string sFeeID { get; set; }
        public string sOrderID { get; set; }
        public string ShouldMoney { get; set; }
        public string PaidMoney { get; set; }
        public string DiscountMoney { get; set; }
        public string CanMoney { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sFeesOrderModel()
        { }
        public sFeesOrderModel(string sfeesorderid, string status, string sfeeid, string sorderid, string shouldmoney, string paidmoney, string discountmoney, string canmoney, string createid, string createtime, string updateid, string updatetime)
        {
            this.sFeesOrderID = sfeesorderid;
            this.Status = status;
            this.sFeeID = sfeeid;
            this.sOrderID = sorderid;
            this.ShouldMoney = shouldmoney;
            this.PaidMoney = paidmoney;
            this.DiscountMoney = discountmoney;
            this.CanMoney = canmoney;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
