using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class sOrderGiveModel
    {
        public string sOrderGiveID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string sEnrollsProfessionID { get; set; }
        public string PlanItemID { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string sGiveID { get; set; }
        public string Queue { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sOrderGiveModel()
        { }
        public sOrderGiveModel(string sordergiveid, string status, string deptid, string senrollsprofessionid, string planitemid, string year, string month, string sgiveid, string queue, string createid, string createtime, string updateid, string updatetime)
        {
            this.sOrderGiveID = sordergiveid;
            this.Status = status;
            this.DeptID = deptid;
            this.sEnrollsProfessionID = senrollsprofessionid;
            this.PlanItemID = planitemid;
            this.Year = year;
            this.Month = month;
            this.sGiveID = sgiveid;
            this.Queue = queue;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
