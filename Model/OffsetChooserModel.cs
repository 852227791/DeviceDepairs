using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 获取充抵选择器的表单对象类
    /// </summary>
    public class OffsetChooserModel
    {
        public OffsetChooserModel() { }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string idCard { get; set; }
        /// <summary>
        /// 凭证号
        /// </summary>
        public string voucherNum { get; set; }
        /// <summary>
        //票据号
        /// </summary>
        public string noteNum { get; set; }
        /// <summary>
        /// 菜单编号
        /// </summary>
        public string MenuID { get; set; }
    }
}
