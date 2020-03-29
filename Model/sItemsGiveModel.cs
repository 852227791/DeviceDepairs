using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class sItemsGiveModel
    {
        public string sItemsGiveID { get; set; }
        public string ItemID { get; set; }
        public string sGiveID { get; set; }
        public string Queue { get; set; }

        public sItemsGiveModel()
        { }
        public sItemsGiveModel(string sitemsgiveid, string itemid, string sgiveid, string queue)
        {
            this.sItemsGiveID = sitemsgiveid;
            this.ItemID = itemid;
            this.sGiveID = sgiveid;
            this.Queue = queue;
        }
    }
}
