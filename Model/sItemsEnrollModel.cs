using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sItemsEnrollModel
    {
        public string sItemsEnrollID { get; set; }
        public string ItemID { get; set; }
        public string sEnrollsProfessionID { get; set; }

        public sItemsEnrollModel()
        { }
        public sItemsEnrollModel(string sitemsenrollid, string itemid, string senrollsprofessionid)
        {
            this.sItemsEnrollID = sitemsenrollid;
            this.ItemID = itemid;
            this.sEnrollsProfessionID = senrollsprofessionid;
        }
    }
}
