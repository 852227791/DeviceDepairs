using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sOrderAddModel
    {
        public string sOrderAddID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Name { get; set; }
        public string PlanItemID { get; set; }
        public string NumItemID { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sOrderAddModel()
        { }
        public sOrderAddModel(string sorderaddid, string status, string deptid, string year, string month, string name, string planitemid, string numitemid, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.sOrderAddID = sorderaddid;
            this.Status = status;
            this.DeptID = deptid;
            this.Year = year;
            this.Month = month;
            this.Name = name;
            this.PlanItemID = planitemid;
            this.NumItemID = numitemid;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
