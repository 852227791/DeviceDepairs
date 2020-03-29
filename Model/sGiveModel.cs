using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sGiveModel
    {
        public string sGiveID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string Name { get; set; }
        public string Money { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sGiveModel()
        { }
        public sGiveModel(string sgiveid, string status, string deptid, string name, string money, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.sGiveID = sgiveid;
            this.Status = status;
            this.DeptID = deptid;
            this.Name = name;
            this.Money = money;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
