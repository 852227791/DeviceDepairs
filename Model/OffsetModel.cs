using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OffsetModel
    {
        public string OffsetID { get; set; }
        public string Status { get; set; }
        public string FeeDetailID { get; set; }
        public string ByFeeDetailID { get; set; }
        public string Money { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public OffsetModel()
        { }
        public OffsetModel(string offsetid, string status, string feedetailid, string byfeedetailid, string money, string createid, string createtime, string updateid, string updatetime)
        {
            this.OffsetID = offsetid;
            this.Status = status;
            this.FeeDetailID = feedetailid;
            this.ByFeeDetailID = byfeedetailid;
            this.Money = money;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
