using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class ProfessionModel
    {
        public string ProfessionID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public ProfessionModel()
        { }
        public ProfessionModel(string professionid, string status, string deptid, string name, string englishname, string createid, string createtime, string updateid, string updatetime)
        {
            this.ProfessionID = professionid;
            this.Status = status;
            this.DeptID = deptid;
            this.Name = name;
            this.EnglishName = englishname;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
