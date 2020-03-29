using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sFeeModel
    {
        public string sFeeID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string VoucherNum { get; set; }
        public string NoteNum { get; set; }
        public string sEnrollsProfessionID { get; set; }
        public string PlanItemID { get; set; }
        public string NumItemID { get; set; }
        public string FeeTime { get; set; }
        public string FeeMode { get; set; }
        public string ShouldMoney { get; set; }
        public string PaidMoney { get; set; }
        public string PrintNum { get; set; }
        public string AffirmID { get; set; }
        public string AffirmTime { get; set; }
        public string Explain { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sFeeModel()
        { }
        public sFeeModel(string sfeeid, string status, string deptid, string vouchernum, string notenum, string senrollsprofessionid, string planitemid, string numitemid, string feetime, string feemode, string shouldmoney, string paidmoney, string printnum, string affirmid, string affirmtime, string explain, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.sFeeID = sfeeid;
            this.Status = status;
            this.DeptID = deptid;
            this.VoucherNum = vouchernum;
            this.NoteNum = notenum;
            this.sEnrollsProfessionID = senrollsprofessionid;
            this.PlanItemID = planitemid;
            this.NumItemID = numitemid;
            this.FeeTime = feetime;
            this.FeeMode = feemode;
            this.ShouldMoney = shouldmoney;
            this.PaidMoney = paidmoney;
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
    /// <summary>
    /// 获取学费收费表单实体对象
    /// </summary>
    public class GetsFeeFormDataModel
    {
        /// <summary>
        /// 收费编号
        /// </summary>
        public string sFeeID { get; set; }
        /// <summary>
        /// 订单json
        /// </summary>
        public string OrderJsonData { get; set; }
        /// <summary>
        /// 配品json
        /// </summary>
        public string GiveJsonData { get; set; }
        /// <summary>
        /// 报名编号
        /// </summary>
        public string sEnrollsProfessionID { get; set; }
        /// <summary>
        /// 缴费单位
        /// </summary>
        public string DeptID { get; set; }
        /// <summary>
        ///收费时间
        /// </summary>
        public string FeeTime { get; set; }
        /// <summary>
        /// 缴费方式
        /// </summary>
        public string FeeMode { get; set; }

        /// <summary>
        /// 应交金额
        /// </summary>
        public string ShouldMoney { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 缴费次数编号
        /// </summary>
        public string NumItemID { get; set; }
    }
    /// <summary>
    /// 缴费订单列
    /// </summary>
    public class sOrderRowsModel
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string sOrderID { get; set; }
        /// <summary>
        /// 收费类别
        /// </summary>
        public string DetailName { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal ShouldMoney { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public decimal ReceivedMoney { get; set; }
        /// <summary>
        /// 未缴金额
        /// </summary>
        public decimal UnPaid { get; set; }
        /// <summary>
        /// 实缴金额
        /// </summary>
        public decimal PaidMoney { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountMoney { get; set; }
        /// <summary>
        /// 充抵金额
        /// </summary>
        public decimal OffsetMoney { get; set; }
        /// <summary>
        /// 充抵明细
        /// </summary>
        public string OffsetDetail { get; set; }

        /// <summary>
        /// 缴费次数名称
        /// </summary>
        public string NumName { get; set; }
    }


    public class sOrderListModel
    {
        /// <summary>
        /// 缴费次数编号
        /// </summary>
        public string NumItemID { get; set; }
        /// <summary>
        /// 缴费列表json
        /// </summary>
        public List<sOrderRowsModel> rows { get; set; }
    }



    /// <summary>
    /// 配品列
    /// </summary>
    public class sGiveRowsModel
    {
        public string sOrderGiveID { get; set; }

        public string GiveName { get; set; }

        public string NumItemID { get; set; }
    }
    /// <summary>
    /// 配品列表
    /// </summary>
    public class sGiveListModel
    {
        public List<sGiveRowsModel> rows { get; set; }
    }
    /// <summary>
    /// 修改表单实体
    /// </summary>
    public class sFeeEditFormModel
    {
        public string editsFeeID { get; set; }
        public string editGiveJsonData { get; set; }
        public string editFeeTime { get; set; }
        public string editFeeMode { get; set; }
        public string editExplain { get; set; }
        public string editRemark { get; set; }
        public string editOrderJsonData { get; set; }

    }

    public class sFeeOffset
    {

        public string Sort { get; set; }

        public string ID { get; set; }

        public decimal Money { get; set; }
    }

    /// <summary>
    /// 修改收费明细列表实体
    /// </summary>
    public class sFeesOrderRowModel
    {
        /// <summary>
        /// 收费明细编号
        /// </summary>
        public string sFeesOrderID { get; set; }
        /// <summary>
        /// 收费类别
        /// </summary>
        public string DetailName { get; set; }
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
        /// 充抵金额
        /// </summary>
        public decimal OffsetMoney { get; set; }
        /// <summary>
        /// 充抵明细
        /// </summary>
        public string OffsetDetail { get; set; }
    }
    public class sFeesOrderListModel
    {
        public List<sFeesOrderRowModel> rows { get; set; }
    }

    public class ReturnDataModel
    {
        public string sFeeId { get; set; }

        public string type { get; set; }
    }

    public class UploadsFeeCHModel
    {

        public string 姓名 { get; set; }
        public string 身份证号 { get; set; }
        public string 学历层次 { get; set; }
        public string 专业 { get; set; }
        public string 缴费学年 { get; set; }
        public string 费用类别 { get; set; }
        public string 缴费类别 { get; set; }
        public string 供款金额 { get; set; }
        public string 缴费方式 { get; set; }
        public string 打印说明 { get; set; }
        public string 备注 { get; set; }
        public string 系统备注 { get; set; }

    }

    public class UploadsFeeModel
    {
        public string name { get; set; }

        public string IdCard { get; set; }

        public string studyLevel { get; set; }

        public string major { get; set; }

        public string year { get; set; }

        public string detail { get; set; }

        public string type { get; set; }

        public string money { get; set; }

        public string feeMode { get; set; }

        public string printRemark { get; set; }

        public string remark { get; set; }

    }
}
