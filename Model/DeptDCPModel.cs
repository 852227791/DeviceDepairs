using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public  class DeptDCPModel
    {
        public string DeptDCPID { get; set; }
        public string DeptID { get; set; }
        public string DCPID { get; set; }

        public DeptDCPModel()
        { }
        public DeptDCPModel(string deptdcpid, string deptid, string dcpid)
        {
            this.DeptDCPID = deptdcpid;
            this.DeptID = deptid;
            this.DCPID = dcpid;
        }
    }
}
