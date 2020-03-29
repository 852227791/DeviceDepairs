using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sEnrollNumModel
    {
        public string sEnrollNumID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string Year { get; set; }
        public string Num { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sEnrollNumModel()
        { }
        public sEnrollNumModel(string senrollnumid, string status, string deptid, string year, string num, string createid, string createtime, string updateid, string updatetime)
        {
            this.sEnrollNumID = senrollnumid;
            this.Status = status;
            this.DeptID = deptid;
            this.Year = year;
            this.Num = num;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
