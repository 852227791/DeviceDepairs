using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class EnrollApiModel
    {
        /// <summary>
        /// 返回地址
        /// </summary>
        public string returnUrl { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string idCard { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string major { get; set; }
        /// <summary>
        /// 主体
        /// </summary>
        public string deptId { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string key { get; set; }
    }

    public class EnrollSendModel : EnrollApiModel {
        /// <summary>
        /// 是否缴费
        /// </summary>
        public string payed { get; set; }
    }
}
