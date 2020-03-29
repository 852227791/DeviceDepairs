using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class ClassModel
    {
        public string ClassID { get; set; }
        public string Status { get; set; }
        public string ProfessionID { get; set; }
        public string Name { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public ClassModel()
        { }
        public ClassModel(string classid, string status, string professionid, string name, string createid, string createtime, string updateid, string updatetime)
        {
            this.ClassID = classid;
            this.Status = status;
            this.ProfessionID = professionid;
            this.Name = name;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
