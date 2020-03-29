using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sEnrollsProfessionStatusModel
    {
        public string sEnrollsProfessionStatusID { get; set; }
        public string Status { get; set; }
        public string sEnrollsProfessionID { get; set; }
        public string StatusValue { get; set; }
        public string Explain { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sEnrollsProfessionStatusModel()
        { }
        public sEnrollsProfessionStatusModel(string senrollsprofessionstatusid, string status, string senrollsprofessionid, string statusvalue, string explain, string createid, string createtime, string updateid, string updatetime)
        {
            this.sEnrollsProfessionStatusID = senrollsprofessionstatusid;
            this.Status = status;
            this.sEnrollsProfessionID = senrollsprofessionid;
            this.StatusValue = statusvalue;
            this.Explain = explain;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }

      
    }
    public class UploadChangeEnrollsProfession
    {
        public string 系统备注 { get; set; }

        public string 姓名 { get; set; }

        public string 身份证号 { get; set; }
        public string 学历层次 { get; set; }
        public string 专业 { get; set; }
        public string 在校状态 { get; set; }
        public string 变更理由 { get; set; }
    }

    public class ChangeEnrollsProfessionModel
    {

        public string name { get; set; }

        public string idCard { get; set; }
        public string studyLevel { get; set; }
        public string major { get; set; }
        public string status { get; set; }
        public string reson { get; set; }
    }
}
