using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class sFeesOrderGiveModel
    {
        public string sFeesOrderGiveID { get; set; }
        public string Status { get; set; }
        public string sFeeID { get; set; }
        public string sOrderGiveID { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sFeesOrderGiveModel()
        { }
        public sFeesOrderGiveModel(string sfeesordergiveid, string status, string sfeeid, string sordergiveid, string createid, string createtime, string updateid, string updatetime)
        {
            this.sFeesOrderGiveID = sfeesordergiveid;
            this.Status = status;
            this.sFeeID = sfeeid;
            this.sOrderGiveID = sordergiveid;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
