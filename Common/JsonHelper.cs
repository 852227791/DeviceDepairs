using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using System.Web.Script.Serialization;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Common
{
    public class JsonHelper
    {
        /// <summary>
        /// DataTable转为Json
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>Json对象</returns>
        public static string DataTableToJson(DataTable dt)
        {
            if (dt == null)
            {
                return "[]";
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    builder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        builder.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString().Replace("\"", "\\\"").Replace("	", "") + "\"");

                        if (j < (dt.Columns.Count - 1))
                        {
                            builder.Append(",");
                        }
                    }
                    builder.Append("}");

                    if (i < (dt.Rows.Count - 1))
                    {
                        builder.Append(",");
                    }
                }
            }
            builder.Append("]");

            return builder.ToString();
        }

        /// <summary>
        /// 类对像转换成Json格式
        /// </summary> 
        /// <returns>Json对象</returns>
        public static string ToJson(object t)
        {
            return new JavaScriptSerializer().Serialize(t);
        }

        /// <summary>
        /// Json换成类对像
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="strJson">Json对象</param>
        /// <returns>json换成类对像转格式</returns>
        public static T FromJson<T>(string strJson) where T : class
        {
            return new JavaScriptSerializer().Deserialize<T>(strJson);
        }
        /// <summary>
        /// 将json转换为DataTable
        /// </summary>
        /// <param name="strJson">得到的json</param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string strJson)
        {
            //转换json格式
            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');

                //创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split('#');

                        if (strCell[0].Substring(0, 1) == "\"")
                        {
                            int a = strCell[0].Length;
                            dc.ColumnName = strCell[0].Substring(1, a - 2);
                        }
                        else
                        {
                            dc.ColumnName = strCell[0];
                        }
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }
    }
}
