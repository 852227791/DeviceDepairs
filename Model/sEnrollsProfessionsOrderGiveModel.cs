using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sEnrollsProfessionsOrderGiveModel
    {
        public string sEnrollsProfessionsOrderGiveID { get; set; }
        public string sEnrollsProfessionID { get; set; }
        public string sOrderGiveID { get; set; }

        public sEnrollsProfessionsOrderGiveModel()
        { }
        public sEnrollsProfessionsOrderGiveModel(string senrollsprofessionsordergiveid, string senrollsprofessionid, string sordergiveid)
        {
            this.sEnrollsProfessionsOrderGiveID = senrollsprofessionsordergiveid;
            this.sEnrollsProfessionID = senrollsprofessionid;
            this.sOrderGiveID = sordergiveid;
        }
    }
}
