using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class iOffsetModel
    {
        public string iOffsetID { get; set; }
        public string Status { get; set; }
        public string iFeeID { get; set; }
        public string ByiFeeID { get; set; }
        public string Money { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public iOffsetModel()
        { }
        public iOffsetModel(string ioffsetid, string status, string ifeeid, string byifeeid, string money, string createid, string createtime, string updateid, string updatetime)
        {
            this.iOffsetID = ioffsetid;
            this.Status = status;
            this.iFeeID = ifeeid;
            this.ByiFeeID = byifeeid;
            this.Money = money;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
