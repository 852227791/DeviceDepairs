using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
 public   class ConfigModel
    {
        public string ConfigID { get; set; }
        public string VoucherNum { get; set; }
        public string NoteNum { get; set; }
        public string PrintNum { get; set; }

        public ConfigModel()
        { }
        public ConfigModel(string configid, string vouchernum, string notenum, string printnum)
        {
            this.ConfigID = configid;
            this.VoucherNum = vouchernum;
            this.NoteNum = notenum;
            this.PrintNum = printnum;
        }
    }
}
