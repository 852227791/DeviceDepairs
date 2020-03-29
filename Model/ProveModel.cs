using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProveModel
    {
        public string ProveID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string ClassID { get; set; }
        public string EnrollTime { get; set; }
        public string StudentID { get; set; }
        public string ItemID { get; set; }
        public string IsForce { get; set; }
        public string ProveNum { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public ProveModel() { }

        public ProveModel(string proveid, string status, string deptid, string classid, string enrolltime, string studentid, string itemid, string isforce, string provenum, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.ProveID = proveid;
            this.Status = status;
            this.DeptID = deptid;
            this.ClassID = classid;
            this.EnrollTime = enrolltime;
            this.StudentID = studentid;
            this.ItemID = itemid;
            this.IsForce = isforce;
            this.ProveNum = provenum;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
    /// <summary>
    /// 老证书导入实体类
    /// </summary>
    public class OldProveModel
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public string idCard { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 证书名称
        /// </summary>
        public string proveName { get; set; }
        /// <summary>
        /// 教师
        /// </summary>
        public string teacher { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 报名时间
        /// </summary>
        public string enrollTime { get; set; }

    }


    public class ProveNumExcelModel
    {
        public string 姓名 { get; set; }

        public string 身份证号 { get; set; }

        public string 证书名称 { get; set; }

        public string 证书编号 { get; set; }
    }

    public class ProveNumModel
    {
        public string name { get; set; }
        public string idCard { get; set; }
        public string proveName { get; set; }
        public string proveNum { get; set; }
    }
}
