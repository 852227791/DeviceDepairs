using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 收费系统传递的数据
    /// </summary>
    public class PaymentModel
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }
        /// <summary>
        /// 主体
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal ReceivableAmount { get; set; }

        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal PaidInAmount { get; set; }
        
    }
}
