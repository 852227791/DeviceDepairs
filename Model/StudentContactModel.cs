using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class StudentContactModel
    {
        public string StudentContactID { get; set; }
        public string Status { get; set; }
        public string StudentID { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public StudentContactModel()
        { }
        public StudentContactModel(string studentcontactid, string status, string studentid, string name, string tel, string createid, string createtime, string updateid, string updatetime)
        {
            this.StudentContactID = studentcontactid;
            this.Status = status;
            this.StudentID = studentid;
            this.Name = name;
            this.Tel = tel;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
