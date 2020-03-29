using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class PowerModel
    {
        public string PowerID { get; set; }
        public string RoleID { get; set; }
        public string MenuID { get; set; }
        public string ButtonID { get; set; }

        public PowerModel()
        { }
        public PowerModel(string powerid, string roleid, string menuid, string buttonid)
        {
            this.PowerID = powerid;
            this.RoleID = roleid;
            this.MenuID = menuid;
            this.ButtonID = buttonid;
        }
    }
}
