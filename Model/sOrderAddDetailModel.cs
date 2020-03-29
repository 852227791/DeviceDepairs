using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sOrderAddDetailModel
    {
        public string sOrderAddDetailID { get; set; }
        public string Status { get; set; }
        public string sOrderAddID { get; set; }
        public string DetailID { get; set; }
        public string Sort { get; set; }
        public string ShouldMoney { get; set; }
        public string IsGive { get; set; }
        public string LimitTime { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sOrderAddDetailModel()
        { }
        public sOrderAddDetailModel(string sorderadddetailid, string status, string sorderaddid, string detailid, string sort, string shouldmoney, string isgive, string limittime, string createid, string createtime, string updateid, string updatetime)
        {
            this.sOrderAddDetailID = sorderadddetailid;
            this.Status = status;
            this.sOrderAddID = sorderaddid;
            this.DetailID = detailid;
            this.Sort = sort;
            this.ShouldMoney = shouldmoney;
            this.IsGive = isgive;
            this.LimitTime = limittime;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
