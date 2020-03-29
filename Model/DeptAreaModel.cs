using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class DeptAreaModel
    {
        public string DeptAreaID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string Name { get; set; }
        public string Queue { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public DeptAreaModel()
        { }
        public DeptAreaModel(string deptareaid, string status, string deptid, string name, string queue, string createid, string createtime, string updateid, string updatetime)
        {
            this.DeptAreaID = deptareaid;
            this.Status = status;
            this.DeptID = deptid;
            this.Name = name;
            this.Queue = queue;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
