using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DisableNoteModel
    {
        public string DisableNoteID { get; set; }
        public string Status { get; set; }
        public string FeeID { get; set; }
        public string NoteNum { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public DisableNoteModel()
        { }
        public DisableNoteModel(string disablenoteid, string status, string feeid, string notenum, string createid, string createtime, string updateid, string updatetime)
        {
            this.DisableNoteID = disablenoteid;
            this.Status = status;
            this.FeeID = feeid;
            this.NoteNum = notenum;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
