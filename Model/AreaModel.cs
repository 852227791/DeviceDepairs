using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class AreaModel
    {
        public string AreaID { get; set; }
        public string Status { get; set; }
        public string ParentID { get; set; }
        public string Name { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public AreaModel()
        { }
        public AreaModel(string areaid, string status, string parentid, string name, string createid, string createtime, string updateid, string updatetime)
        {
            this.AreaID = areaid;
            this.Status = status;
            this.ParentID = parentid;
            this.Name = name;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
