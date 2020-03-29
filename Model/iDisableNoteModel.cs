using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class iDisableNoteModel
    {
        public string iDisableNoteID { get; set; }
        public string Status { get; set; }
        public string iFeeID { get; set; }
        public string NoteNum { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public iDisableNoteModel()
        { }
        public iDisableNoteModel(string idisablenoteid, string status, string ifeeid, string notenum, string createid, string createtime, string updateid, string updatetime)
        {
            this.iDisableNoteID = idisablenoteid;
            this.Status = status;
            this.iFeeID = ifeeid;
            this.NoteNum = notenum;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
