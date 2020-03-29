using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class iReportConfigModel
    {
        public string iReportConfigID { get; set; }
        public string Sort { get; set; }
        public string ID { get; set; }
        public string DetailID { get; set; }

        public iReportConfigModel()
        { }
        public iReportConfigModel(string ireportconfigid, string sort, string id, string detailid)
        {
            this.iReportConfigID = ireportconfigid;
            this.Sort = sort;
            this.ID = id;
            this.DetailID = detailid;
        }
    }
}
