using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class PurviewModel
    {
        public string PurviewID { get; set; }
        public string UserID { get; set; }
        public string RoleID { get; set; }
        public string DeptID { get; set; }
        public string Range { get; set; }

        public PurviewModel()
        { }
        public PurviewModel(string purviewid, string userid, string roleid, string deptid, string range)
        {
            this.PurviewID = purviewid;
            this.UserID = userid;
            this.RoleID = roleid;
            this.DeptID = deptid;
            this.Range = range;
        }
    }
}
