using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class FeeManageModel
    {
        public string FeeManageID { get; set; }
        public string Status { get; set; }
        public string IsForce { get; set; }
        public string FeeID { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public FeeManageModel()
        { }
        public FeeManageModel(string feemanageid, string status, string isforce, string feeid, string createid, string createtime, string updateid, string updatetime)
        {
            this.FeeManageID = feemanageid;
            this.Status = status;
            this.IsForce = isforce;
            this.FeeID = feeid;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
