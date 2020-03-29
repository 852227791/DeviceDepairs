using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SearchModel
    {
        /// <summary>
        /// 就读学校
        /// </summary>
        public string treeDeptID { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string txtEnrollNum { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string txtStudentName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string txtIDCard { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public string selYear { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public string selMonth { get; set; }
        /// <summary>
        /// 预报名日期
        /// </summary>
        public string txtBeforeEnrollTimeS { get; set; }
        /// <summary>
        /// 预报名日期
        /// </summary>
        public string txtBeforeEnrollTimeE { get; set; }
        /// <summary>
        /// 正式报名日期
        /// </summary>
        public string txtEnrollTimeS { get; set; }
        /// <summary>
        /// 正式报名日期
        /// </summary>
        public string txtEnrollTimeE { get; set; }
        /// <summary>
        /// 首次缴费日期
        /// </summary>
        public string txtFirstFeeTimeS { get; set; }
        /// <summary>
        /// 首次缴费日期
        /// </summary>
        public string txtFirstFeeTimeE { get; set; }
        /// <summary>
        /// 在校状态
        /// </summary>
        public string selStatus { get; set; }
        /// <summary>
        /// 报读专业
        /// </summary>
        public string txtProfessionName { get; set; }
        /// <summary>
        /// 学历层次
        /// </summary>
        public string selEnrollLevel { get; set; }
        /// <summary>
        /// 学历类别
        /// </summary>
        public string selSort { get; set; }
        /// <summary>
        /// 费用类别
        /// </summary>
        public string treeDetail { get; set; }
        /// <summary>
        /// 欠费金额
        /// </summary>
        public string txtArrearsMoneyS { get; set; }
        /// <summary>
        /// 欠费金额
        /// </summary>
        public string txtArrearsMoneyE { get; set; }
        /// <summary>
        /// 已供贷金额
        /// </summary>
        public string txtPaidMoneyS { get; set; }
        /// <summary>
        /// 已供贷金额
        /// </summary>
        public string txtPaidMoneyE { get; set; }
        /// <summary>
        /// 供款日期
        /// </summary>
        public string txtLimitTimeS { get; set; }
        /// <summary>
        /// 供款日期
        /// </summary>
        public string txtLimitTimeE { get; set; }
        /// <summary>
        /// 逾期状态
        /// </summary>
        public string selArrearsStatus { get; set; }
    }
}
