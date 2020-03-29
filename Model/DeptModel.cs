using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DeptModel
    {
        public string DeptID { get; set; }
        public string Status { get; set; }
        public string ParentID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Code { get; set; }
        public string Queue { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public DeptModel()
        { }
        public DeptModel(string deptid, string status, string parentid, string name, string shortname, string code, string queue, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.DeptID = deptid;
            this.Status = status;
            this.ParentID = parentid;
            this.Name = name;
            this.ShortName = shortname;
            this.Code = code;
            this.Queue = queue;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
