using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class sOffsetModel
    {
        public string sOffsetID { get; set; }
        public string Status { get; set; }
        public string BySort { get; set; }
        public string RelatedID { get; set; }
        public string ByRelatedID { get; set; }
        public string Money { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sOffsetModel()
        { }
        public sOffsetModel(string soffsetid, string status, string bysort, string relatedid, string byrelatedid, string money, string createid, string createtime, string updateid, string updatetime)
        {
            this.sOffsetID = soffsetid;
            this.Status = status;
            this.BySort = bysort;
            this.RelatedID = relatedid;
            this.ByRelatedID = byrelatedid;
            this.Money = money;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
