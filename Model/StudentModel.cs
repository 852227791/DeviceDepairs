using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class StudentModel
    {
        public string StudentID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string Name { get; set; }
        public string IDCard { get; set; }
        public string Sex { get; set; }
        public string Mobile { get; set; }
        public string QQ { get; set; }
        public string WeChat { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public StudentModel()
        { }
        public StudentModel(string studentid, string status, string deptid, string name, string idcard, string sex, string mobile, string qq, string wechat, string address, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.StudentID = studentid;
            this.Status = status;
            this.DeptID = deptid;
            this.Name = name;
            this.IDCard = idcard;
            this.Sex = sex;
            this.Mobile = mobile;
            this.QQ = qq;
            this.WeChat = wechat;
            this.Address = address;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }


    }
    /// <summary>
    /// 学生信息导入模板
    /// </summary>
    public class ImportStudentModel
    {
        public string 学号 { set; get; }

        public string 姓名 { get; set; }

        public string 身份证号 { get; set; }

        public string 性别 { get; set; }

        public string 手机号 { get; set; }

        public string QQ { get; set; }

        public string 微信 { get; set; }

        public string 地址 { get; set; }

        public string 备注 { get; set; }

        public string 系统备注 { get; set; }
    }

    /// <summary>
    /// 缴费人选择器，快速添加
    /// </summary>
    public class AddStudentModel {
        public string deptId { get; set; }
        public string addName{ get; set; }

        public string addIDCard { get; set; }

        public string addSex { get; set; }

        public string addMobile { get; set; }

        public string addAddress { get; set; }
    }
}
