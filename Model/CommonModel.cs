using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ComboboxModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public bool selected { get; set; }
    }
}
