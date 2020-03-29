using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DetailModel
    {
        public string DetailID { get; set; }
        public string Status { get; set; }
        public string ParentID { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public DetailModel()
        { }
        public DetailModel(string detailid, string status, string parentid, string name, string englishname, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.DetailID = detailid;
            this.Status = status;
            this.ParentID = parentid;
            this.Name = name;
            this.EnglishName = englishname;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
    public class DetailFormModel : DetailModel
    {
        public string SubjectID { get; set; }
    }
}
