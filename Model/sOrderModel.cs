using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sOrderModel
    {
        public string sOrderID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string sEnrollsProfessionID { get; set; }
        public string PlanItemID { get; set; }
        public string PlanName { get; set; }
        public string PlanSort { get; set; }
        public string PlanLevel { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string NumItemID { get; set; }
        public string NumName { get; set; }
        public string LimitTime { get; set; }
        public string ItemDetailID { get; set; }
        public string DetailID { get; set; }
        public string Sort { get; set; }
        public string ShouldMoney { get; set; }
        public string PaidMoney { get; set; }
        public string IsGive { get; set; }
        public string ItemQueue { get; set; }
        public string ItemDetailQueue { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sOrderModel()
        { }
        public sOrderModel(string sorderid, string status, string deptid, string senrollsprofessionid, string planitemid, string planname, string plansort, string planlevel, string year, string month, string numitemid, string numname, string limittime, string itemdetailid, string detailid, string sort, string shouldmoney, string paidmoney, string isgive, string itemqueue, string itemdetailqueue, string createid, string createtime, string updateid, string updatetime)
        {
            this.sOrderID = sorderid;
            this.Status = status;
            this.DeptID = deptid;
            this.sEnrollsProfessionID = senrollsprofessionid;
            this.PlanItemID = planitemid;
            this.PlanName = planname;
            this.PlanSort = plansort;
            this.PlanLevel = planlevel;
            this.Year = year;
            this.Month = month;
            this.NumItemID = numitemid;
            this.NumName = numname;
            this.LimitTime = limittime;
            this.ItemDetailID = itemdetailid;
            this.DetailID = detailid;
            this.Sort = sort;
            this.ShouldMoney = shouldmoney;
            this.PaidMoney = paidmoney;
            this.IsGive = isgive;
            this.ItemQueue = itemqueue;
            this.ItemDetailQueue = itemdetailqueue;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }

        public class sOrderDetailModel
        {
            public string DetailID { get; set; }
            public string DetailName { get; set; }
            public string Sort { get; set; }
            public string IsGive { get; set; }
            public string ShouldMoney { get; set; }
            public string LimitTime { get; set; }
        }
    }
}
