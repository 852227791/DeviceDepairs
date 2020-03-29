using System;

namespace Model
{
   
    public class VerifyModel
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }
        /// <summary>
        /// 主体
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// 审查时间
        /// </summary>
        public DateTime VerifyDateTime { get; set; }
    }
}