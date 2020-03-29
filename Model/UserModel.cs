using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class UserModel
    {
        public string UserID { get; set; }
        public string Status { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string UserType { get; set; }
        public string Sex { get; set; }
        public string Mobile { get; set; }
        public string Remark { get; set; }
        public string LoginTime { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public UserModel()
        { }
        public UserModel(string userid, string status, string loginname, string password, string name, string usertype, string sex, string mobile, string remark, string logintime, string createid, string createtime, string updateid, string updatetime)
        {
            this.UserID = userid;
            this.Status = status;
            this.LoginName = loginname;
            this.Password = password;
            this.Name = name;
            this.UserType = usertype;
            this.Sex = sex;
            this.Mobile = mobile;
            this.Remark = remark;
            this.LoginTime = logintime;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
}
