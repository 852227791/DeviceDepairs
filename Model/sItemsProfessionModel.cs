using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class sItemsProfessionModel
    {
        public string sItemsProfessionID { get; set; }
        public string ItemID { get; set; }
        public string sProfessionID { get; set; }

        public sItemsProfessionModel()
        { }
        public sItemsProfessionModel(string sitemsprofessionid, string itemid, string sprofessionid)
        {
            this.sItemsProfessionID = sitemsprofessionid;
            this.ItemID = itemid;
            this.sProfessionID = sprofessionid;
        }
    }

    public class sItemsProfessionFormModel {
        public string sItemsProfessionID { get; set; }
        public string ItemID { get; set; }
        public string [] sProfessionID { get; set; }

        public sItemsProfessionFormModel()
        { }
        public sItemsProfessionFormModel(string sitemsprofessionid, string itemid, string[] sprofessionid)
        {
            this.sItemsProfessionID = sitemsprofessionid;
            this.ItemID = itemid;
            this.sProfessionID = sprofessionid;
        }
    }
}
