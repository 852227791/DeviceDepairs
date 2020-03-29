using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{


    public class sEnrollsProfessionModel
    {
        public string sEnrollsProfessionID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string DeptAreaID { get; set; }
        public string sEnrollID { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string LeaveYear { get; set; }
        public string BeforeEnrollTime { get; set; }
        public string EnrollTime { get; set; }
        public string FirstFeeTime { get; set; }
        public string EnrollLevel { get; set; }
        public string sProfessionID { get; set; }
        public string ClassID { get; set; }
        public string Auditor { get; set; }
        public string AuditTime { get; set; }
        public string AuditView { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sEnrollsProfessionModel() { }

        public sEnrollsProfessionModel(string senrollsprofessionid, string status, string deptid, string deptareaid, string senrollid, string year, string month, string leaveyear, string beforeenrolltime, string enrolltime, string firstfeetime, string enrolllevel, string sprofessionid, string classid, string auditor, string audittime, string auditview, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.sEnrollsProfessionID = senrollsprofessionid;
            this.Status = status;
            this.DeptID = deptid;
            this.DeptAreaID = deptareaid;
            this.sEnrollID = senrollid;
            this.Year = year;
            this.Month = month;
            this.LeaveYear = leaveyear;
            this.BeforeEnrollTime = beforeenrolltime;
            this.EnrollTime = enrolltime;
            this.FirstFeeTime = firstfeetime;
            this.EnrollLevel = enrolllevel;
            this.sProfessionID = sprofessionid;
            this.ClassID = classid;
            this.Auditor = auditor;
            this.AuditTime = audittime;
            this.AuditView = auditview;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
    public class sEnrollSchemeModel
    {
        public string ItemID { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Name { get; set; }
    }
    public class sEnrollProfessionViewModel
    {
        /// <summary>
        /// 报名专业id
        /// </summary>
        public string enrollsProfessionId { get; set; }
        /// <summary>
        /// 招生方案
        /// </summary>
        public string scheme { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string idCard { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 学生联系电话
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string qq { get; set; }
        /// <summary>
        /// 毕业学校
        /// </summary>
        public string school { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string weChat { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string nation { get; set; }
        /// <summary>
        /// 家庭地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string zip { get; set; }
        /// <summary>
        /// 生源地 省
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 生源地 市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string district { get; set; }
        /// <summary>
        /// 校区
        /// </summary>
        public string deptId { get; set; }
        /// <summary>
        /// 入学年份
        /// </summary>
        public string year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public string month { get; set; }
        /// <summary>
        /// 学习层次
        /// </summary>
        public string enrollLevel { set; get; }
        /// <summary>
        /// 报名专业
        /// </summary>
        public string professionId { get; set; }

        /// <summary>
        /// 班级
        /// </summary>
        public string classId { get; set; }
        /// <summary>
        ///报名时间
        /// </summary>
        public string enrollTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 报名类型：1.正式报名;2.预报名;3.续报名;4.重新报名
        /// </summary>
        public string isRepeat { get; set; }
        /// <summary>
        /// 报名校区
        /// </summary>
        public string senrollDeptId { get; set; }
        /// <summary>
        /// 父母姓名
        /// </summary>
        public string parentName { get; set; }
        /// <summary>
        /// 父母联系电话
        /// </summary>
        public string tel { get; set; }
        /// <summary>
        /// 学生id
        /// </summary>
        public string studentId { get; set; }
        /// <summary>
        /// 学生联系人id
        /// </summary>
        public string studentContactId { get; set; }
        /// <summary>
        /// 学生信息id
        /// </summary>
        public string studentInfoId { get; set; }
    }


    public class ClassGroupingCHModel
    {
        public string 姓名 { get; set; }

        public string 身份证号 { get; set; }

        public string 学历层次 { get; set; }

        public string 专业 { get; set; }

        public string 班级 { get; set; }

        public string 系统备注 { get; set; }
    }

    public class ClassGroupingModel
    {
        public string name { get; set; }

        public string IdCard { get; set; }

        public string studyLevel { get; set; }

        public string major { get; set; }

        public string classId { get; set; }

        public string remark { get; set; }
    }
}
