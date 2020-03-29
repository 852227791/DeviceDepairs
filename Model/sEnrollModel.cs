using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sEnrollModel
    {
        public string sEnrollID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string StudentID { get; set; }
        public string ExamNum { get; set; }
        public string StudyNum { get; set; }
        public string EnrollNum { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sEnrollModel() { }

        public sEnrollModel(string senrollid, string status, string deptid, string studentid, string examnum, string studynum, string enrollnum, string createid, string createtime, string updateid, string updatetime)
        {
            this.sEnrollID = senrollid;
            this.Status = status;
            this.DeptID = deptid;
            this.StudentID = studentid;
            this.ExamNum = examnum;
            this.StudyNum = studynum;
            this.EnrollNum = enrollnum;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
    public class UploadsExcelEnrollModel
    {
        public string 考生号 { get; set; }
        public string 姓名 { get; set; }
        public string 身份证号 { get; set; }
        public string 性别 { get; set; }
        public string 手机号 { get; set; }
        public string QQ { get; set; }
        public string 微信 { get; set; }
        public string 地址 { get; set; }
        public string 民族 { get; set; }
        public string 生源地_省 { get; set; }
        public string 生源地_市 { get; set; }
        public string 生源地_县 { get; set; }
        public string 邮政编码 { get; set; }
        public string 毕业学校 { get; set; }
        public string 父母姓名 { get; set; }
        public string 父母电话 { get; set; }
        public string 学习层次 { get; set; }
        public string 录取专业 { get; set; }
        public string 招生部门 { get; set; }
        public string 系统备注 { get; set; }
    }
    public class UploadsEnrollFormModel
    {
        public string upDeptId { get; set; }
        public string upSort { get; set; }

        public string upEnrollDeptId { get; set; }

        public string upYear { get; set; }

        public string upMonth { get; set; }

        public string filePath { get; set; }

        public string Paras { get; set; }
    }
    public class UploadsEnrollModel
    {
        public string examNum { get; set; }
        public string name { get; set; }
        public string idCard { get; set; }
        public string sex { get; set; }
        public string mobile { get; set; }
        public string qq { get; set; }
        public string weChat { get; set; }
        public string address { get; set; }
        public string nation { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string zip { get; set; }
        public string school { get; set; }
        public string parentName { get; set; }
        public string parentMobile { get; set; }
        public string enrollLevel { get; set; }
        public string enrollProfession { get; set; }
        public string deptAreaId { get; set; }
    }
    /// <summary>
    /// 审查表单实体
    /// </summary>
    public class sEnrollAuditFormModel
    {
        public string audId { get; set; }

        public string audsEnrollTime { get; set; }

        public string audExplain { get; set; }
    }

    public class StudentLeaveYearCH
    {
        public string 姓名 { get; set; }

        public string 身份证号 { get; set; }

        public string 学历层次 { get; set; }

        public string 专业 { get; set; }

        public string 毕业年份 { get; set; }

        public string 系统备注 { get; set; }
    }
  

    public class StudentLeaveYear
    {
        public string name { get; set; }

        public string idCard { get; set; }

        public string level { get; set; }
        public string major { get; set; }

        public string year { get; set; }
    }
    public class EnrollDeptModelCH
    {
        public string 姓名 { get; set; }

        public string 身份证号 { get; set; }

        public string 学历层次 { get; set; }

        public string 专业 { get; set; }

        public string 校区 { get; set; }

        public string 系统备注 { get; set; }
    }

    public class EnrollDeptModel {
        public string name { get; set; }

        public string idCard { get; set; }

        public string level { get; set; }
        public string major { get; set; }

        public string deptId { get; set; }
    }

    public class StudyModelCH {
        public string 姓名 { get; set; }
        public string 身份证号 { get; set; }
        public string 学号 { get; set; }
        public string 系统备注 { get; set; }
    }

    public class StudyModel {
        public string name { get; set; }

        public string idCard { get; set; }

        public string studyNum { get; set; }
    }
}
