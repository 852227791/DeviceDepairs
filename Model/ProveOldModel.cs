using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
 public   class ProveOldModel
    {
        public string ProveOldID { get; set; }
        public string OldID { get; set; }

        public ProveOldModel()
        { }
        public ProveOldModel(string proveoldid, string oldid)
        {
            this.ProveOldID = proveoldid;
            this.OldID = oldid;
        }
    }
}
