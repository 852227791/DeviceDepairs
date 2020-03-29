using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sOrderAddsProfessionModel
    {
        public string sOrderAddsProfessionID { get; set; }
        public string Status { get; set; }
        public string sOrderAddID { get; set; }
        public string sProfessionID { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sOrderAddsProfessionModel()
        { }
        public sOrderAddsProfessionModel(string sorderaddsprofessionid, string status, string sorderaddid, string sprofessionid, string createid, string createtime, string updateid, string updatetime)
        {
            this.sOrderAddsProfessionID = sorderaddsprofessionid;
            this.Status = status;
            this.sOrderAddID = sorderaddid;
            this.sProfessionID = sprofessionid;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
