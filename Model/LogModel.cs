using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class LogModel
    {
        public string LogID { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string ValueOld { get; set; }
        public string ValueNew { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }

        public LogModel()
        { }
        public LogModel(string logid, string tablename, string fieldname, string valueold, string valuenew, string createid, string createtime)
        {
            this.LogID = logid;
            this.TableName = tablename;
            this.FieldName = fieldname;
            this.ValueOld = valueold;
            this.ValueNew = valuenew;
            this.CreateID = createid;
            this.CreateTime = createtime;
        }
    }
}
