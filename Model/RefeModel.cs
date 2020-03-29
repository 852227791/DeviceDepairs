using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class RefeModel
    {
        public string RefeID { get; set; }
        public string Status { get; set; }
        public string RefeTypeID { get; set; }
        public string RefeName { get; set; }
        public string Value { get; set; }
        public string Queue { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public RefeModel()
        { }
        public RefeModel(string refeid, string status, string refetypeid, string refename, string value, string queue, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.RefeID = refeid;
            this.Status = status;
            this.RefeTypeID = refetypeid;
            this.RefeName = refename;
            this.Value = value;
            this.Queue = queue;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
