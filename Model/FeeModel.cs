using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class FeeModel
    {

        public string FeeID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string VoucherNum { get; set; }
        public string NoteNum { get; set; }
        public string FeeTime { get; set; }
        public string ProveID { get; set; }
        public string PersonSort { get; set; }
        public string FeeMode { get; set; }
        public string ShouldMoney { get; set; }
        public string PaidMoney { get; set; }
        public string Teacher { get; set; }
        public string PrintNum { get; set; }
        public string AffirmID { get; set; }
        public string AffirmTime { get; set; }
        public string Explain { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public FeeModel()
        { }
        public FeeModel(string feeid, string status, string deptid, string vouchernum, string notenum, string feetime, string proveid, string personsort, string feemode, string shouldmoney, string paidmoney, string teacher, string printnum, string affirmid, string affirmtime, string explain, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.FeeID = feeid;
            this.Status = status;
            this.DeptID = deptid;
            this.VoucherNum = vouchernum;
            this.NoteNum = notenum;
            this.FeeTime = feetime;
            this.ProveID = proveid;
            this.PersonSort = personsort;
            this.FeeMode = feemode;
            this.ShouldMoney = shouldmoney;
            this.PaidMoney = paidmoney;
            this.Teacher = teacher;
            this.PrintNum = printnum;
            this.AffirmID = affirmid;
            this.AffirmTime = affirmtime;
            this.Explain = explain;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }

    }

    public class FeeOffsetListModel
    {
        /// <summary>
        /// 收费项编号
        /// </summary>
        public string FeeDetailID { get; set; }
        /// <summary>
        /// 充抵到ID
        /// </summary>
        public string ItemDetailID { get; set; }
        /// <summary>
        /// 凭证号
        /// </summary>
        public string VoucherNum { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 收费单位
        /// </summary>
        public string Dept { get; set; }
        /// <summary>
        /// 票据号
        /// </summary>
        public string NoteNum { get; set; }
        /// <summary>
        /// 收费项
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 收费时间
        /// </summary>
        public string FeeTime { get; set; }
        /// <summary>
        /// 充抵金额
        /// </summary>
        public decimal Offset { get; set; }
        /// <summary>
        /// 充抵到收费项（名称）
        /// </summary>
        public string OffsetItem { get; set; }

    }
    /// <summary>
    /// 收费充抵列表
    /// </summary>
    public class FeeOffsetList
    {
        public List<FeeOffsetListModel> rows { get; set; }
    }

    public class FeeDiscountList
    {
        public List<FeeDiscount> rows { get; set; }
    }

    public class FeeDiscount
    {

        public string ItemDetailID { get; set; }
        /// <summary>
        /// 优惠到项（id）
        /// </summary>
        public string OffsetItem { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountMoney { get; set; }
    }
    public class ExcelModel
    {
        public string 专业
        {
            get;

            set;
        }

        public string 班级
        {
            get;

            set;
        }

        public string 姓名
        {
            get;

            set;
        }

        public string 身份证号
        {
            get;

            set;
        }

        public string 性别
        {
            get;

            set;
        }

        public string 手机
        {
            get;

            set;
        }

        public string QQ
        {
            get;

            set;
        }
        public string 微信
        {
            get;

            set;
        }

        public string 地址
        {
            get;

            set;
        }

        public string 系统备注
        {
            get;

            set;
        }


    }
    /// <summary>
    /// 上传表单
    /// </summary>
    public class UplodFormModel
    {
        /// <summary>
        /// 校区
        /// </summary>
        public string Dept { get; set; }
        /// <summary>
        /// 证书
        /// </summary>
        public string[] ItemID { get; set; }

        /// <summary>
        /// 交款人员
        /// </summary>
        public string PersonSort { get; set; }
        /// <summary>
        /// 收费项目
        /// </summary>
        public string[] ItemDetailID { get; set; }
        /// <summary>
        /// 交费金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 报名时间
        /// </summary>
        public string EnrollTime { get; set; }
        /// <summary>
        /// 收费时间
        /// </summary>
        public string FeeTime { get; set; }
        /// <summary>
        /// 缴费方式
        /// </summary>
        public string FeeMode { get; set; }
        /// <summary>
        /// 教师
        /// </summary>
        public string Teacher { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string filePath { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 表单外围参数
        /// </summary>
        public string Paras { get; set; }
    }


    public class FeeValidateModel
    {
        public string proveId { get; set; }
        public string feeId { get; set; }
        public string voucherNum { get; set; }
        public string deptId { get; set; }

        public string feeTime { get; set; }

        public string feeMode { get; set; }

        public string shouldMoney { get; set; }

        public string paidMoney { get; set; }

        public int printNum { get; set; }

        public string createrId { get; set; }
    }

    public class FeeDetailValidateModel
    {
        public string itemDetailId { get; set; }

        public decimal shouldMoney { get; set; }

        public decimal paidMoney { get; set; }

        public decimal discountMoney { get; set; }

        public decimal canMoney { get; set; }

        public int offsetNum { get; set; }

        public int byOffsetNum { get; set; }

        public string voucherNum { get; set; }

        public int RefundNum { get; set; }
    }
}
