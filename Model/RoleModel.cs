using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class RoleModel
    {
        public string RoleID { get; set; }
        public string Status { get; set; }
        public string RoleType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public RoleModel()
        { }
        public RoleModel(string roleid, string status, string roletype, string name, string description, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.RoleID = roleid;
            this.Status = status;
            this.RoleType = roletype;
            this.Name = name;
            this.Description = description;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
