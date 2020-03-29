using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class NoteModel
    {
        public string NoteID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string Sort { get; set; }
        public string InFile { get; set; }
        public string OutFile { get; set; }
        public string SuccessNum { get; set; }
        public string ErrorNum { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }

        public NoteModel()
        { }
        public NoteModel(string noteid, string status, string deptid, string sort, string infile, string outfile, string successnum, string errornum, string createid, string createtime)
        {
            this.NoteID = noteid;
            this.Status = status;
            this.DeptID = deptid;
            this.Sort = sort;
            this.InFile = infile;
            this.OutFile = outfile;
            this.SuccessNum = successnum;
            this.ErrorNum = errornum;
            this.CreateID = createid;
            this.CreateTime = createtime;
        }
    }
}
