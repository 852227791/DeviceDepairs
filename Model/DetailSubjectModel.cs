using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DetailSubjectModel
    {
        public string DetailSubjectID { get; set; }
        public string DetailID { get; set; }
        public string SubjectID { get; set; }

        public DetailSubjectModel()
        { }
        public DetailSubjectModel(string detailsubjectid, string detailid, string subjectid)
        {
            this.DetailSubjectID = detailsubjectid;
            this.DetailID = detailid;
            this.SubjectID = subjectid;
        }
    }
}
