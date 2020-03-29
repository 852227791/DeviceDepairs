using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sProfessionModel
    {
        public string sProfessionID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string ProfessionID { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sProfessionModel()
        { }
        public sProfessionModel(string sprofessionid, string status, string deptid, string year, string month, string professionid, string createid, string createtime, string updateid, string updatetime)
        {
            this.sProfessionID = sprofessionid;
            this.Status = status;
            this.DeptID = deptid;
            this.Year = year;
            this.Month = month;
            this.ProfessionID = professionid;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }

    }

    public class sProfessionFormModel : sProfessionModel
    {
        public new string[] ProfessionID { get; set; }
    }
}
