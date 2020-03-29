
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Common
{
    public class OtherHelper
    {
        /// <summary>
        /// 下拉多选框转为sql条件
        /// </summary>
        /// <param name="option"></param>
        /// <param name="Field"></param>
        /// <returns></returns>
        public static string MultiSelectToSQLWhere(string option, string Field)
        {
            string where = "";
            string[] optionArr = option.Split(',');
            string bracketS = "(";
            string bracketE = ")";
            if (optionArr.Length < 2)
            {
                bracketS = "";
                bracketE = "";
            }
            for (int optionI = 0; optionI < optionArr.Length; optionI++)
            {
                if (optionI == 0)
                {
                    where += " and " + bracketS;
                }
                else
                {
                    where += " or ";
                }
                where += Field + " = " + (optionArr[optionI] == "" ? "0" : optionArr[optionI]) + "";
            }
            where += bracketE;
            return where;
        }

        /// <summary>
        /// 需要将字符串截断到合适的长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len">字符串长度，以汉字个数计算，英文算半个</param>
        /// <param name="endstr"></param>
        /// <returns></returns>
        public static string SubString(string str, int len, string endstr)
        {
            bool isjie = false;
            len = len * 2;
            StringBuilder sb = new StringBuilder();
            int counter = 0;
            char[] chars = str.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                if (c < 255)
                {
                    counter++;
                }
                else
                {
                    counter = counter + 2;
                }
                if (counter > len)
                {
                    isjie = true;
                    break;
                }
                sb.Append(c);
            }
            if (endstr != "" && isjie)
            {
                return sb.ToString() + endstr;
            }
            else
            {
                return sb.ToString();
            }
        }

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(string str)
        {
            int n;
            if (int.TryParse(str, out n))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 截取指定的字符串
        /// </summary>
        /// <param name="ContentHtml">源代码</param>
        /// <param name="BeginStr">截取字符串开头代码</param>
        /// <param name="EndStr">截取字符串结束代码</param>
        /// <param name="BeginNum">开始位置</param>
        /// <param name="EndNum">结束位置</param>
        /// <returns></returns>
        public static string ContentStrByMore(string ContentHtml, string BeginStr, string EndStr, int BeginNum, int EndNum)
        {
            BeginNum = ContentHtml.IndexOf(BeginStr, EndNum, ContentHtml.Length - EndNum);
            EndNum = ContentHtml.IndexOf(EndStr, BeginNum + BeginStr.Length, ContentHtml.Length - BeginNum - BeginStr.Length);
            string ContentStr = ContentHtml.Substring(BeginNum + BeginStr.Length, EndNum - BeginNum - BeginStr.Length);
            return ContentStr;
        }

        /// <summary>
        /// 获得开始代码位置
        /// </summary>
        /// <param name="ContentHtml">源代码</param>
        /// <param name="BeginStr">截取字符串开头代码</param>
        /// <param name="EndStr">截取字符串结束代码</param>
        /// <param name="BeginNum">开始位置</param>
        /// <param name="EndNum">结束位置</param>
        /// <returns></returns>
        public static int BeginNumByMore(string ContentHtml, string BeginStr, string EndStr, int BeginNum, int EndNum)
        {
            BeginNum = ContentHtml.IndexOf(BeginStr, EndNum, ContentHtml.Length - EndNum);
            return BeginNum;
        }

        /// <summary>
        /// 获得结束代码位置
        /// </summary>
        /// <param name="ContentHtml">源代码</param>
        /// <param name="BeginStr">截取字符串开头代码</param>
        /// <param name="EndStr">截取字符串结束代码</param>
        /// <param name="BeginNum">开始位置</param>
        /// <param name="EndNum">结束位置</param>
        /// <returns></returns>
        public static int EndNumByMore(string ContentHtml, string BeginStr, string EndStr, int BeginNum, int EndNum)
        {
            EndNum = ContentHtml.IndexOf(EndStr, BeginNum + BeginStr.Length, ContentHtml.Length - BeginNum - BeginStr.Length);
            return EndNum;
        }
        /// <summary>  
        /// 验证身份证合理性  
        /// </summary>  
        /// <param name="Id"></param>  
        /// <returns></returns>  
        public static bool CheckIDCard(string idNumber)
        {
            try
            {
                if (idNumber.Length == 18)
                {
                    bool check = CheckIDCard18(idNumber);
                    return check;
                }
                else if (idNumber.Length == 15)
                {
                    bool check = CheckIDCard15(idNumber);
                    return check;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        /// <summary>  
        /// 15位身份证号码验证  
        /// </summary>  
        private static bool CheckIDCard15(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            return true;
        }

        /// <summary>  
        /// 18位身份证号码验证  
        /// </summary>  
        private static bool CheckIDCard18(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber.Remove(17), out n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = idNumber.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
            {
                return false;//校验码验证  
            }
            return true;//符合GB11643-1999标准  
        }

   

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt">要导出的DataTable</param>
        /// <param name="filePath"></param>
        public static void DeriveToExcel(DataTable dt, string filePath)
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            DataGrid excel = new DataGrid();
            //System.Web.UI.WebControls.TableItemStyle AlternatingStyle = new TableItemStyle();
            //System.Web.UI.WebControls.TableItemStyle headerStyle = new TableItemStyle();
            //System.Web.UI.WebControls.TableItemStyle itemStyle = new TableItemStyle();

            //AlternatingStyle.BackColor = System.Drawing.Color.LightGray;
            //headerStyle.BackColor = System.Drawing.Color.LightGray;
            //headerStyle.Font.Bold = true;
            //headerStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            //itemStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            //itemStyle.ToString();

            //excel.AlternatingItemStyle.MergeWith(AlternatingStyle);
            //excel.HeaderStyle.MergeWith(headerStyle);
            //excel.ItemStyle.MergeWith(itemStyle);
            excel.Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            excel.GridLines = GridLines.Both;
            excel.HeaderStyle.Font.Bold = true;
            excel.DataSource = dt.DefaultView;//输出DataTable的内容

            excel.DataBind();
            excel.RenderControl(htmlWriter);

            string filestr = HttpContext.Current.Server.MapPath("~/Temp/" + filePath);  //filePath是文件的路径
            int pos = filestr.LastIndexOf("\\");
            string file = filestr.Substring(0, pos);
            if (!Directory.Exists(file))
            {
                Directory.CreateDirectory(file);
            }
            StreamWriter sw = new StreamWriter(filestr, false, Encoding.UTF8);
            sw.Write(stringWriter.ToString());
            sw.Close();
        }


        public static void DeriveToExcelByHtml(string Html, string filePath)
        {
            string file = System.Web.HttpContext.Current.Server.MapPath("~/Temp/" + filePath);
            if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Temp/")))
            {
                Directory.CreateDirectory("~/Temp/");
            }
            File.WriteAllText(file, Html, Encoding.UTF8);
        }

        public static DataSet UploadFile(string fileName)
        {
            string oleDBConnString = String.Empty;
            oleDBConnString = "Provider=Microsoft.Jet.OLEDB.4.0;";
            oleDBConnString += "Data Source=";
            oleDBConnString += System.Web.HttpContext.Current.Server.MapPath(fileName);
            oleDBConnString += ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1'";
            OleDbConnection oleDBConn = null;
            OleDbDataAdapter oleAdMaster = null;
            DataTable m_tableName = new DataTable();
            DataSet ds = new DataSet();

            oleDBConn = new OleDbConnection(oleDBConnString);
            oleDBConn.Open();
            m_tableName = oleDBConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (m_tableName != null || m_tableName.Rows.Count > 0)
            {
                m_tableName.TableName = m_tableName.Rows[0]["TABLE_NAME"].ToString();
            }
            string sqlMaster;
            sqlMaster = " SELECT *  FROM [" + m_tableName.TableName + "]";
            oleAdMaster = new OleDbDataAdapter(sqlMaster, oleDBConn);
            oleAdMaster.Fill(ds, "m_tableName");
            oleAdMaster.Dispose();
            oleDBConn.Close();
            oleDBConn.Dispose();
            return ds;
        }
        public static List<DataTable> UploadExeclFile(string fileName)
        {
            List<DataTable> list = new List<DataTable>();
            try
            {
                string oleDBConnString = String.Empty;
                oleDBConnString = "Provider=Microsoft.Jet.OLEDB.4.0;";
                oleDBConnString += "Data Source=";
                oleDBConnString += System.Web.HttpContext.Current.Server.MapPath(fileName);
                oleDBConnString += ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                OleDbConnection oleDBConn = null;
                OleDbDataAdapter oleAdMaster = null;
                DataTable m_tableName = new DataTable();


                oleDBConn = new OleDbConnection(oleDBConnString);
                oleDBConn.Open();
                m_tableName = oleDBConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                for (int i = 0; i < m_tableName.Rows.Count; i++)
                {
                    DataSet ds = new DataSet();
                    m_tableName.TableName = m_tableName.Rows[i]["TABLE_NAME"].ToString();
                    string sqlMaster;
                    sqlMaster = " SELECT *  FROM [" + m_tableName.TableName + "]";
                    oleAdMaster = new OleDbDataAdapter(sqlMaster, oleDBConn);
                    oleAdMaster.Fill(ds, "m_tableName");
                    list.Add(ds.Tables[0]);
                }
                oleAdMaster.Dispose();
                oleDBConn.Close();
                oleDBConn.Dispose();
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 生成文件路径和文件名
        /// </summary>
        /// <returns></returns>
        public static string FilePathAndName()
        {
            Random R = new Random();//创建产生随机数
            int val = 100 + R.Next(899);//产生随机数为99以内任意
            string sj = val.ToString();//产生随机数
            string FileTime = DateTime.Now.ToString("yyyyMMddHHmmss") + sj;//得到系统时间(格式化)并加上随机数以便生成上传图片名称
            //产生上传图片的名称
            string fileload = DateTime.Now.ToString("yyyyMM");//每月产生一个文件夹名
            string fileloaddate = DateTime.Now.ToString("dd");//每天一个文件夹
            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string filename = year + "/" + month + "/" + day + "/" + FileTime + ".xls";
            return filename;
        }

        /// <summary>
        /// 验证字符串是否是时间格式
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsDateTime(string dateTime)
        {
            try
            {
                Convert.ToDateTime(dateTime);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 验证是否是数字
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsDecimal(string num)
        {
            try
            {
                Convert.ToDecimal(num);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 将excel转换成DataTable
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string fileName)
        {
            //   oleDBConnString += "Provider=Microsoft.ACE.OLEDB.12.0;";
            try
            {
                string oleDBConnString = String.Empty;
                oleDBConnString = "Provider=Microsoft.Jet.OLEDB.4.0;";
                oleDBConnString += "Data Source=";
                oleDBConnString += System.Web.HttpContext.Current.Server.MapPath(fileName);
                oleDBConnString += ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                OleDbConnection oleDBConn = null;
                OleDbDataAdapter oleAdMaster = null;
                DataTable m_tableName = new DataTable();
                DataSet ds = new DataSet();

                oleDBConn = new OleDbConnection(oleDBConnString);
                oleDBConn.Open();
                m_tableName = oleDBConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (m_tableName != null || m_tableName.Rows.Count > 0)
                {
                    m_tableName.TableName = m_tableName.Rows[0]["TABLE_NAME"].ToString();
                }
                string sqlMaster;
                sqlMaster = " SELECT *  FROM [" + m_tableName.TableName + "]";
                oleAdMaster = new OleDbDataAdapter(sqlMaster, oleDBConn);
                oleAdMaster.Fill(ds, "m_tableName");
                oleAdMaster.Dispose();
                oleDBConn.Close();
                oleDBConn.Dispose();
                return ds.Tables[0];
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 将object 序列化成json
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string JsonSerializer(object list)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(list.GetType());
            string jsonString = "";
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, list);
                jsonString = Encoding.UTF8.GetString(stream.ToArray());
            }
            return jsonString;
        }
        /// <summary>
        /// 将json数据生成json文件
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <param name="fileName">文件名  </param>
        /// <returns></returns>
        public static bool CreateJsonFile(string json, string fileName)
        {
            try
            {
                string filestr = System.Web.HttpContext.Current.Server.MapPath("~/Content/Json/" + fileName + "");  //filePath是文件的路径
                int pos = filestr.LastIndexOf("\\");
                string file = filestr.Substring(0, pos);
                if (!Directory.Exists(file))
                {
                    Directory.CreateDirectory(file);
                }
                StreamWriter sw = new StreamWriter(filestr, false, Encoding.UTF8);
                sw.Write(json);
                sw.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CreateFile(string str, string fileName)
        {
            try
            {
                string filestr = System.Web.HttpContext.Current.Server.MapPath(fileName);  //filePath是文件的路径
                int pos = filestr.LastIndexOf("\\");
                string file = filestr.Substring(0, pos);
                if (!Directory.Exists(file))
                {
                    Directory.CreateDirectory(file);
                }
                StreamWriter sw = new StreamWriter(filestr, false, Encoding.UTF8);
                sw.Write(str);
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 将字符编码
        /// </summary>
        /// <param name="code_type">编码方式</param>
        /// <param name="code">字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code); //将一组字符编码为一个字节序列. 
            try
            {
                encode = Convert.ToBase64String(bytes); //将8位无符号整数数组的子集转换为其等效的,以64为基的数字编码的字符串形式. 
            }
            catch
            {
                encode = code;
            }
            return encode;
        }

        public static string HostingReturn(string url, string xml)
        {
            string postData = EncodeBase64("UTF-8", xml);
            string strUrl = url;
            // 准备请求... 
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(strUrl);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/json";
            myRequest.ContentLength = postData.Length;
            Stream newStream = myRequest.GetRequestStream();
            byte[] data = Encoding.UTF8.GetBytes(postData);
            // 发送数据 
            newStream.Write(data, 0, data.Length);
            HttpWebResponse res = myRequest.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            String ret = sr.ReadToEnd();
            newStream.Close();
            return ret;
        }

        /// <summary> 
        /// 2013-11-05 新增
        /// 将字符串使用base64算法解密 
        /// </summary> 
        /// <param name="code_type">编码类型</param> 
        /// <param name="code">已用base64算法加密的字符串</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code); //将2进制编码转换为8位无符号整数数组. 
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes); //将指定字节数组中的一个字节序列解码为一个字符串。 
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
        /// <summary>
        /// 读取文件流返回解密后的字符串
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string RequestStream(Stream postData)
        {
            try
            {
                StreamReader sRead = new StreamReader(postData);
                string postContent = sRead.ReadToEnd();
                sRead.Close();
                return OtherHelper.DecodeBase64("UTF-8", postContent);
            }
            catch (Exception)
            {

                throw;
            }


        }
        /// <summary>
        /// 获取配置文件值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettingsValue(string key)
        {
            ConfigurationManager.RefreshSection("appSettings");
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }


        public static string CreateTxtFileName()
        {

            Random R = new Random();//创建产生随机数
            int val = 100 + R.Next(899);//产生随机数为99以内任意
            string sj = val.ToString();//产生随机数
            string FileTime = DateTime.Now.ToString("yyyyMMddHHmmss") + sj;//得到系统时间(格式化)并加上随机数以便生成上传图片名称
            //产生上传图片的名称
            string fileload = DateTime.Now.ToString("yyyyMM");//每月产生一个文件夹名
            string fileloaddate = DateTime.Now.ToString("dd");//每天一个文件夹
            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string filename = "/EnrollInterface/" + year + "/" + month + "/" + day + "/" + FileTime + ".txt";
            return filename;

        }

        public static string ImportToExcel(List<DataTable> ds)
        {

            StringBuilder sb = new StringBuilder();
            //   Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            sb.Append("<?xml version='1.0'?><?mso-application progid='Excel.Sheet'?>");
            sb.Append(@"<Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet'    xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel'   xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet' xmlns:html='http://www.w3.org/TR/REC-html40'>");
            sb.Append(@"<DocumentProperties xmlns='urn:schemas-microsoft-com:office:office'>");
            sb.Append(@"<Author></Author><LastAuthor></LastAuthor>   <Created>2010-09-08T14:07:11Z</Created><Company>mxh</Company><Version>1990</Version>");
            sb.Append("</DocumentProperties>");
            sb.Append(@"<Styles><Style ss:ID='Default' ss:Name='Normal'><Alignment ss:Vertical='Center'/>  <Borders/><Font ss:FontName='宋体' x:CharSet='134' ss:Size='12'/><Interior/><NumberFormat/><Protection/></Style>");
            //定义标题样式      
            sb.Append(@"<Style ss:ID='Header'><Borders><Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'/>    <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'/>   <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'/>  <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'/></Borders>    <Font ss:FontName='宋体' x:CharSet='134' ss:Size='18' ss:Color='#FF0000' ss:Bold='1'/></Style>");

            //定义边框  
            sb.Append(@"<Style ss:ID='border'><NumberFormat ss:Format='@'/><Borders>  
                      <Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'/>  
                      <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'/>  
                      <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'/>  
                      <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'/></Borders></Style>");

            sb.Append("</Styles>");

            //SheetCount代表生成的 Sheet 数目。  
            for (int i = 0; i < ds.Count; i++)
            {
                //计算该 Sheet 中的数据起始行和结束行。  

                sb.Append("<Worksheet ss:Name='Sheet" + (i + 1) + "'>");
                sb.Append("<Table x:FullColumns='1' x:FullRows='1'>");

                //输出标题  
                sb.Append("\r\n<Row ss:AutoFitHeight='1'>");
                foreach (DataColumn dc in ds[i].Columns)
                {
                    sb.Append("<Cell ss:StyleID='Header'><Data ss:Type='String'>" + dc.ColumnName + "</Data></Cell>");
                }
                sb.Append("\r\n</Row>");

                for (int j = 0; j < ds[i].Columns.Count; j++)
                {

                    sb.Append("<Row>");
                    for (int k = 0; k < ds[i].Rows.Count; k++)
                    {
                        DataTable dt = ds[i];
                        sb.Append("<Cell ss:StyleID='border'><Data ss:Type='String'>" + dt.Rows[k][j].ToString() + "</Data></Cell>");

                    }

                    sb.Append("</Row>");
                }

            }
            sb.Append("</Table>");
            sb.Append("</Worksheet>");
            sb.Append("</Workbook>");
            return sb.ToString();
        }

        public static void DeriveToExcel(Object obj, string filePath)
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            DataGrid excel = new DataGrid();
            excel.Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            excel.GridLines = GridLines.Both;
            excel.HeaderStyle.Font.Bold = true;
            excel.DataSource = obj;//输出DataTable的内容

            excel.DataBind();
            excel.RenderControl(htmlWriter);

            string filestr = HttpContext.Current.Server.MapPath("~/Temp/" + filePath);  //filePath是文件的路径
            int pos = filestr.LastIndexOf("\\");
            string file = filestr.Substring(0, pos);
            if (!Directory.Exists(file))
            {
                Directory.CreateDirectory(file);
            }
            StreamWriter sw = new StreamWriter(filestr, false, Encoding.UTF8);
            sw.Write(stringWriter.ToString());
            sw.Close();
        }
    }

}
