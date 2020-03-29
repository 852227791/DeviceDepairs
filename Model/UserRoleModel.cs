using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class UserRoleModel
    {
        public string UserRoleID { get; set; }
        public string UserID { get; set; }
        public string RoleID { get; set; }

        public UserRoleModel()
        { }
        public UserRoleModel(string userroleid, string userid, string roleid)
        {
            this.UserRoleID = userroleid;
            this.UserID = userid;
            this.RoleID = roleid;
        }
    }
}
