using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sEnrollsProfessionNoteModel
    {
        public string sEnrollsProfessionNoteID { get; set; }
        public string Status { get; set; }
        public string sEnrollsProfessionID { get; set; }
        public string NewsEnrollsProfessionID { get; set; }
        public string Sort { get; set; }
        public string NoteTime { get; set; }
        public string Explain { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sEnrollsProfessionNoteModel()
        { }
        public sEnrollsProfessionNoteModel(string senrollsprofessionnoteid, string status, string senrollsprofessionid, string newsenrollsprofessionid, string sort, string notetime, string explain, string createid, string createtime, string updateid, string updatetime)
        {
            this.sEnrollsProfessionNoteID = senrollsprofessionnoteid;
            this.Status = status;
            this.sEnrollsProfessionID = senrollsprofessionid;
            this.NewsEnrollsProfessionID = newsenrollsprofessionid;
            this.Sort = sort;
            this.NoteTime = notetime;
            this.Explain = explain;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
