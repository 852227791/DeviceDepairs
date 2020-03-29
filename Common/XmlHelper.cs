using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Common
{
    public class XmlHelper
    {
        /// <summary>
        /// 将Json对象序列化为XML字符串
        /// </summary>
        /// <param name="json">要序列化的对象</param>
        /// <returns>序列化产生的XML</returns>
        public static XElement JsonToXml(string json)
        {
            XElement details = new XElement("Details");

            if (!string.IsNullOrEmpty(json))
            {
                var list = JsonHelper.FromJson<List<Dictionary<string, string>>>(json);
                foreach (var items in list)
                {
                    XElement detail = new XElement("Detail");
                    detail.Add(items.Select(item => new XElement(item.Key, string.IsNullOrEmpty(item.Value) ? null : item.Value)));
                    details.Add(detail);
                }
            }

            return details;
        }

        /// <summary>
        /// 将Json对象序列化为XML字符串
        /// </summary>
        /// <param name="json">要序列化的对象</param>
        /// <returns>序列化产生的XML</returns>
        public static XElement JsonUploadifyToXml(string json)
        {
            XElement uploadifies = new XElement("Uploadifies");

            if (!string.IsNullOrEmpty(json))
            {
                var list = JsonHelper.FromJson<List<Dictionary<string, string>>>(json);
                foreach (var items in list)
                {
                    XElement uploadify = new XElement("Uploadify");
                    uploadify.Add(items.Select(item => new XElement(item.Key, string.IsNullOrEmpty(item.Value) ? null : item.Value)));
                    uploadifies.Add(uploadify);
                }
            }

            return uploadifies;
        }
    }
}