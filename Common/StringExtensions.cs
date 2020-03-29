using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class StringExtensions
    {
        /// <summary>
        /// 不区分大小写的字符串比较
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="to">目标字符串</param>
        /// <returns>true或false</returns>
        public static bool EqualsTo(this string str, string to)
        {
            return str.EqualsTo(to, false);
        }

        /// <summary>
        /// 是否区分大小写的字符串比较
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="to">目标字符串</param>
        /// <param name="ignoreCase">是否区分大小写</param>
        /// <returns>true或false</returns>
        public static bool EqualsTo(this string str, string to, bool ignoreCase)
        {
            return (string.Compare(str, to, ignoreCase) == 0);
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="instance">字符串</param>
        /// <returns>true或false</returns>
        public static bool IsNullOrEmpty(this string instance)
        {
            return string.IsNullOrEmpty(instance);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="splitString">以某个字母为分割项</param>
        /// <returns>字符串数组</returns>
        public static string[] Split(this string str, string splitString)
        {
            return str.Split(new string[] { splitString }, StringSplitOptions.None);
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="instance">原字符串</param>
        /// <param name="args">字符串数组</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string instance, params object[] args)
        {
            return string.Format(instance, args);
        }

        /// <summary>
        /// 替换html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public static string HtmlEncode(this string theString)
        {
            theString = theString.Replace(">", ">");
            theString = theString.Replace("<", "<");
            theString = theString.Replace(" ", " ");
            theString = theString.Replace(" ", " ");
            theString = theString.Replace("\"", "“");
            theString = theString.Replace("\'", "'");
            theString = theString.Replace(" ", "<br/> ");
            return theString;
        }
    }
}
