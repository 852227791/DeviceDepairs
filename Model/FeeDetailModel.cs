using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class FeeDetailModel
    {
        public string FeeDetailID { get; set; }
        public string Status { get; set; }
        public string FeeID { get; set; }
        public string ItemDetailID { get; set; }
        public string ShouldMoney { get; set; }
        public string PaidMoney { get; set; }
        public string DiscountMoney { get; set; }
        public string CanMoney { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public FeeDetailModel()
        { }
        public FeeDetailModel(string feedetailid, string status, string feeid, string itemdetailid, string shouldmoney, string paidmoney, string discountmoney, string canmoney, string createid, string createtime, string updateid, string updatetime)
        {
            this.FeeDetailID = feedetailid;
            this.Status = status;
            this.FeeID = feeid;
            this.ItemDetailID = itemdetailid;
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
