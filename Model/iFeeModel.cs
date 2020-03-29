using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class iFeeModel
    {
        public string iFeeID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string DeptAreaID { get; set; }
        public string VoucherNum { get; set; }
        public string NoteNum { get; set; }
        public string StudentID { get; set; }
        public string FeeTime { get; set; }
        public string ItemDetailID { get; set; }
        public string PersonSort { get; set; }
        public string FeeMode { get; set; }
        public string ShouldMoney { get; set; }
        public string PaidMoney { get; set; }
        public string DiscountMoney { get; set; }
        public string CanMoney { get; set; }
        public string PrintNum { get; set; }
        public string AffirmID { get; set; }
        public string AffirmTime { get; set; }
        public string Explain { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public iFeeModel()
        { }
        public iFeeModel(string ifeeid, string status, string deptid, string deptareaid, string vouchernum, string notenum, string studentid, string feetime, string itemdetailid, string personsort, string feemode, string shouldmoney, string paidmoney, string discountmoney, string canmoney, string printnum, string affirmid, string affirmtime, string explain, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.iFeeID = ifeeid;
            this.Status = status;
            this.DeptID = deptid;
            this.DeptAreaID = deptareaid;
            this.VoucherNum = vouchernum;
            this.NoteNum = notenum;
            this.StudentID = studentid;
            this.FeeTime = feetime;
            this.ItemDetailID = itemdetailid;
            this.PersonSort = personsort;
            this.FeeMode = feemode;
            this.ShouldMoney = shouldmoney;
            this.PaidMoney = paidmoney;
            this.DiscountMoney = discountmoney;
            this.CanMoney = canmoney;
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

        public class iFeeOffSetModel
        {
            public string iFeeID { get; set; }
            public string VoucherNum { get; set; }
            public string Dept { get; set; }
            public string Name { get; set; }
            public string IDCard { get; set; }
            public string NoteNum { get; set; }
            public string FeeContent { get; set; }
            public string FeeTime { get; set; }
            public string OffSetMoney { get; set; }
        }

        public class iFeeItemDetailModel
        {
            public string ItemDetailID { get; set; }
            public string IsFixed { get; set; }
            public string ItemDetailName { get; set; }
            public string FeeMode { get; set; }
            public string ShouldMoney { get; set; }
            public string PaidMoney { get; set; }
            public string DiscountMoney { get; set; }
            public string OffSetMoney { get; set; }
            public string Explain { get; set; }
            public string Remark { get; set; }
            public string OffsetData { get; set; }
        }


    }
    public class UploadiFeeFormMode
    {
        /// <summary>
        /// 收费单位
        /// </summary>
        public string DeptID { get; set; }
        /// <summary>
        /// 收费校区
        /// </summary>
        public string DeptAreaID { get; set; }
        /// <summary>
        /// 人员类别
        /// </summary>
        public string PersonSort { get; set; }
        /// <summary>
        /// 收费项
        /// </summary>
        public string ItemDetailID { get; set; }
        /// <summary>
        /// 缴费方式
        /// </summary>
        public string FeeMode { get; set; }

        /// <summary>
        /// 应缴金额
        /// </summary>
        public decimal ShouldMoney { get; set; }
        /// <summary>
        /// 实缴金额
        /// </summary>
        public decimal PaidMoney { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountMoney { get; set; }
        /// <summary>
        /// 收费时间
        /// </summary>
        public string FeeTime { get; set; }
        /// <summary>
        /// 打印说明
        /// </summary>
        public string Explain { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string filePath { get; set; }

        /// <summary>
        /// 表单外部参数
        /// </summary>
        public string Paras { get; set; }
    }

    public class iFeeExcelModel
    {
        public string 姓名 { get; set; }

        public string 身份证号 { get; set; }
        public string 学号 { get; set; }
        public string 性别 { get; set; }

        public string 手机 { get; set; }
    }

    public class iFeeValidateModel
    {
        public string deptId { get; set; }

        public string feeTime { get; set; }

        public string feeMode { get; set; }

        public string itemDetailId { get; set; }

        public decimal shouldMoney { get; set; }

        public decimal paidMoney { get; set; }

        public decimal discountMoney { get; set; }

        public decimal canMoney { get; set; }

        public int offsetNum { get; set; }

        public int byOffsetNum { get; set; }

        public int printNum { get; set; }

        public string voucherNum { get; set; }

        public string iFeeId { get; set; }

        public int RefundNum { get; set; }
    }
}
