using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class RefeTypeModel
    {
        public string RefeTypeID { get; set; }
        public string Status { get; set; }
        public string ModuleName { get; set; }
        public string TypeName { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public RefeTypeModel()
        { }
        public RefeTypeModel(string refetypeid, string status, string modulename, string typename, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.RefeTypeID = refetypeid;
            this.Status = status;
            this.ModuleName = modulename;
            this.TypeName = typename;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
