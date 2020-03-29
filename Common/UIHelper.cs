using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;
using DAL;
using System.Web;
using System.Collections.Specialized;
using System.Collections;
using System.Data.SqlClient;

namespace Common
{
    public class JsonGridData
    {
        /// <summary>
        /// 将Datable转换成JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetGridJSON(DataTable dt)
        {
            string result = JsonHelper.DataTableToJson(dt);
            string json = @"{""rows"":" + result + @",""total"":""" + dt.Rows.Count + @"""}";
            return json;
        }
        /// <summary>
        /// 根据条件和排序,检索出全部前M行数据的json对象
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="request">request信息</param>
        /// <returns>json</returns>
        public static string GetGridJSON(string sql, NameValueCollection request)
        {
            GridData data = GetGridData(sql, request);

            string result = JsonHelper.DataTableToJson(data.Rows);

            string json = @"{""rows"":" + result + @",""total"":""" + data.Total + @"""}";
            return json;
        }
        /// <summary>
        /// 根据条件和排序,检索出全部前M行数据的json对象,含合计数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="request">request信息</param>
        /// <param name="fieldArray">需合计字段</param>
        /// <returns>json</returns>
        public static string GetGridJSON(string sql, NameValueCollection request, string fieldArray)
        {
            GridData data = GetGridData(sql, request);

            string result = JsonHelper.DataTableToJson(data.Rows);
            string footerStr = "";
            if (fieldArray != "")
            {
                footerStr = GetGridSum(sql, fieldArray);
            }
            else
            {
                footerStr = "[]";
            }
            string json = @"{""rows"":" + result + @",""total"":""" + data.Total + @""",""footer"":" + footerStr + "}";
            return json;
        }

        /// <summary>
        /// 根据条件和排序,检索出全部前M行数据的json对象,含合计数据(not in分页)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filed"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetGridJSON(string sql, string filed, NameValueCollection request)
        {
            GridData data = GetGridData(sql, filed, request);

            string result = JsonHelper.DataTableToJson(data.Rows);

            string json = @"{""rows"":" + result + @",""total"":""" + data.Total + @"""}";
            return json;
        }

        /// <summary>
        /// 根据条件和排序,检索出全部前M行数据的json对象,含合计数据(not in分页)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="filed">not in 字段</param>
        /// <param name="request">request信息</param>
        /// <param name="fieldArray">需合计字段</param>
        /// <returns>json</returns>
        public static string GetGridJSON(string sql, string filed, NameValueCollection request, string fieldArray)
        {
            GridData data = GetGridData(sql, filed, request);

            string result = JsonHelper.DataTableToJson(data.Rows);

            string footerStr = "";
            if (fieldArray != "")
            {
                footerStr = GetGridSum(sql, fieldArray);
            }
            else
            {
                footerStr = "[]";
            }

            string json = @"{""rows"":" + result + @",""total"":""" + data.Total + @""",""footer"":" + footerStr + "}";
            return json;
        }

        /// <summary>
        /// 根据条件、页脚sql和排序,检索出全部前M行数据的json对象(not in分页)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filed"></param>
        /// <param name="footSql"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetGridJSON(string sql, string filed, string footSql, NameValueCollection request)
        {
            GridData data = GetGridData(sql, filed, request);

            string result = JsonHelper.DataTableToJson(data.Rows);

            string footerStr = "";
            footerStr = JsonHelper.DataTableToJson(DatabaseFactory.ExecuteDataset(footSql, CommandType.Text).Tables[0]);

            string json = @"{""rows"":" + result + @",""total"":""" + data.Total + @""",""footer"":" + footerStr + "}";
            return json;
        }

        /// <summary>
        /// 获取合计数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetGridSum(string sql, string field)
        {
            string[] fieldArray = field.Split(',');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fieldArray.Length; i++)
            {
                sb.Append(" '合计:'+ convert(nvarchar(20), IsNull(SUM(" + fieldArray[i] + "),0)) " + fieldArray[i] + ",");
            }
            string temp = sb.ToString().Substring(0, sb.ToString().Length - 1);
            string commandText = "SELECT " + temp + " FROM (" + sql + ")p";
            DataTable dt = DatabaseFactory.ExecuteDataset(commandText, CommandType.Text).Tables[0];
            return JsonHelper.DataTableToJson(dt);

        }

        /// <summary>
        /// 根据条件和排序,检索出全部前M行数据的json对象
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="commandParameters">存储过程参数</param>
        /// <returns>json</returns>
        public static string GetGridJSON(string spName, params SqlParameter[] commandParameters)
        {
            GridData data = GetGridData(spName, commandParameters);
            string result = JsonHelper.DataTableToJson(data.Rows);
            string json = @"{""rows"":" + result + @",""total"":""" + data.Total + @"""}";

            return json;
        }

        /// <summary>
        /// 根据存储过程得到GridData对象
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="commandParameters">存储过程参数</param>
        /// <returns>GridData对象</returns>
        private static GridData GetGridData(string spName, params SqlParameter[] commandParameters)
        {
            GridData data = new GridData();
            DataSet ds = DatabaseFactory.ExecuteDataset(spName, CommandType.StoredProcedure, commandParameters);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            data.Rows = ds.Tables[0];
            data.Total = ds.Tables[0].Rows.Count;
            return data;
        }

        /// <summary>
        /// 根据sql得到GridData对象
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="request">request信息</param>
        /// <returns>GridData对象</returns>
        private static GridData GetGridData(string sql, NameValueCollection request)
        {
            string sortname = request["sort"];
            string sortorder = request["order"];
            string _pagenumber = request["page"];
            string _pagesize = request["rows"];

            //if (sortname.IsNullOrEmpty())
            //{
            //    throw new Exception("必须指定一个排序条件");
            //}

            int? pagenumber = null;
            int? pagesize = null;

            //可分页
            if (!_pagenumber.IsNullOrEmpty() && !_pagesize.IsNullOrEmpty())
            {
                pagenumber = _pagenumber.ToInt();
                pagesize = _pagesize.ToInt();

                if (pagesize == 0)
                {
                    pagesize = 20;
                }
            }

            //可排序
            if (!sortname.IsNullOrEmpty())
            {
                sortorder = sortorder.IsNullOrEmpty() || sortorder.EqualsTo("asc") ? "ASC" : "DESC";
            }

            return GetGridData(sql, sortname, sortorder, pagenumber, pagesize);
        }

        /// <summary>
        /// 根据sql得到GridData对象(not in分页)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="filed">not in 字段</param>
        /// <param name="request">request信息</param>
        /// <returns>GridData对象</returns>
        private static GridData GetGridData(string sql, string filed, NameValueCollection request)
        {
            string sortname = request["sort"];
            string sortorder = request["order"];
            string _pagenumber = request["page"];
            string _pagesize = request["rows"];

            //if (sortname.IsNullOrEmpty())
            //{
            //    throw new Exception("必须指定一个排序条件");
            //}

            int? pagenumber = null;
            int? pagesize = null;

            //可分页
            if (!_pagenumber.IsNullOrEmpty() && !_pagesize.IsNullOrEmpty())
            {
                pagenumber = _pagenumber.ToInt();
                pagesize = _pagesize.ToInt();

                if (pagesize == 0)
                {
                    pagesize = 20;
                }
            }

            //可排序
            if (!sortname.IsNullOrEmpty())
            {
                sortorder = sortorder.IsNullOrEmpty() || sortorder.EqualsTo("asc") ? "ASC" : "DESC";
            }

            return GetGridData(sql, filed, sortname, sortorder, pagenumber, pagesize);
        }

        /// <summary>
        /// 根据条件和排序,检索出全部前M行的GridData对象
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sortname">排序名称</param>
        /// <param name="sortorder">升降序</param>
        /// <param name="pagenumber">页码</param>
        /// <param name="pagesize">页数</param>
        /// <returns>GridData对象</returns>
        private static GridData GetGridData(string sql, string sortname, string sortorder, int? pagenumber, int? pagesize)
        {
            var rows = GetGridRows(sql, sortname, sortorder, pagenumber, pagesize);
            var total = GetGridTotal(sql);
            return new GridData()
            {
                Rows = rows,
                Total = total
            };
        }

        /// <summary>
        /// 根据条件和排序,检索出全部前M行的GridData对象(not in分页)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="filed">not in 字段</param>
        /// <param name="sortname">排序名称</param>
        /// <param name="sortorder">升降序</param>
        /// <param name="pagenumber">页码</param>
        /// <param name="pagesize">页数</param>
        /// <returns>GridData对象</returns>
        private static GridData GetGridData(string sql, string filed, string sortname, string sortorder, int? pagenumber, int? pagesize)
        {
            var rows = GetGridRows(sql, filed, sortname, sortorder, pagenumber, pagesize);
            var total = GetGridTotal(sql);
            return new GridData()
            {
                Rows = rows,
                Total = total
            };
        }

        /// <summary>
        /// 根据条件和排序,检索出全部前M行数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sortname">排序名称</param>
        /// <param name="sortorder">升降序</param>
        /// <param name="pagenumber">页码</param>
        /// <param name="pagesize">页数</param>
        /// <returns>DataTable</returns>
        private static DataTable GetGridRows(string sql, string sortname, string sortorder, int? pagenumber, int? pagesize)
        {
            string commandText = "";
            bool pagable = pagenumber.HasValue && pagesize.HasValue;
            bool sortable = !sortname.IsNullOrEmpty() && !sortorder.IsNullOrEmpty();

            if (pagable)
            {
                if (sql.IsNullOrEmpty() || sql.Trim().Length <= 0)
                {
                    return null;
                }

                string rowNumberOrderBy = "SELECT ROW_NUMBER() OVER (ORDER BY " + sortname + @" " + sortorder + @") AS RowNumber, * FROM ";

                commandText = "SELECT TOP " + pagesize + @" *
FROM 
      (" + rowNumberOrderBy + "(" + sql + ")p" + @" 
      ) q
WHERE RowNumber BETWEEN " + ((pagenumber - 1) * pagesize + 1) + " AND " + (pagenumber * pagesize);

            }
            else
            {
                if (sortable)
                {
                    commandText = "SELECT * FROM (" + sql + ")p";
                    commandText += string.Concat(" ORDER BY ", sortname, " ", sortorder);
                }
                else
                {
                    commandText = sql;
                }
            }

            try
            {
                DataSet ds = DatabaseFactory.ExecuteDataset(commandText, CommandType.Text);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return null;
                }

                return ds.Tables[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据条件和排序,检索出全部前M行数据(not in)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="filed">not in 字段</param>
        /// <param name="sortname">排序名称</param>
        /// <param name="sortorder">升降序</param>
        /// <param name="pagenumber">页码</param>
        /// <param name="pagesize">页数</param>
        /// <returns>DataTable</returns>
        private static DataTable GetGridRows(string sql, string filed, string sortname, string sortorder, int? pagenumber, int? pagesize)
        {
            string commandText = "";
            bool pagable = pagenumber.HasValue && pagesize.HasValue;
            bool sortable = !sortname.IsNullOrEmpty() && !sortorder.IsNullOrEmpty();

            if (pagable)
            {
                if (sql.IsNullOrEmpty() || sql.Trim().Length <= 0 || sql.Substring(0, 6).ToLower() != "select")
                {
                    return null;
                }

                string orderBy = " ORDER BY " + sortname + @" " + sortorder + @"";

                commandText = "SELECT TOP " + pagesize + sql.Substring(6) + " " + @"
AND " + filed + " NOT IN (" + @"
SELECT TOP " + ((pagenumber - 1) * pagesize) + filed + " FROM (" + sql + ") AS ryftemptest " + @"
" + orderBy + @"
) " + @"
" + orderBy;

            }
            else
            {
                if (sortable)
                {
                    commandText = "SELECT * FROM (" + sql + ")p";
                    commandText += string.Concat(" ORDER BY ", sortname, " ", sortorder);
                }
                else
                {
                    commandText = sql;
                }
            }

            try
            {
                DataSet ds = DatabaseFactory.ExecuteDataset(commandText, CommandType.Text);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return null;
                }

                return ds.Tables[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取总行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>总行数</returns>
        private static int GetGridTotal(string sql)
        {
            string commandText = "SELECT COUNT(0) FROM (" + sql + ")p";
            return (int)DatabaseFactory.ExecuteScalar(commandText, CommandType.Text);
        }
    }

    public class JsonTreeGridData
    {
        /// <summary>
        /// 获取树格式对象的JSON(不分页)
        /// </summary>
        /// <param name="commandText">commandText</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        public static string GetArrayJSON(string commandText, string id, string pid)
        {
            var obj = ArrayToTreeData(commandText, id, pid);
            string json = @"{""rows"":" + JsonHelper.ToJson(obj) + "}";
            return json;
            //return JsonHelper.ToJson(obj);
        }

        /// <summary>
        /// 获取树格式对象的JSON
        /// </summary>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="commandParameters">存储过程参数</param>
        /// <returns>JSON</returns>
        public static string GetArrayJSON(string id, string pid, string spName, params SqlParameter[] commandParameters)
        {
            var obj = ArrayToTreeData(id, pid, spName, commandParameters);
            return JsonHelper.ToJson(obj);
        }

        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="commandText">sql</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param> 
        /// <returns></returns>
        private static object ArrayToTreeData(string commandText, string id, string pid)
        {
            using (var reader = DatabaseFactory.ExecuteReader(commandText, CommandType.Text))
            {
                List<Hashtable> list = DbReaderToHash(reader);
                return ArrayToTreeData(list, id, pid);
            }
        }

        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="commandParameters">存储过程参数</param>
        /// <returns>JSON</returns>
        private static object ArrayToTreeData(string id, string pid, string spName, params SqlParameter[] commandParameters)
        {
            using (var reader = DatabaseFactory.ExecuteReader(spName, CommandType.StoredProcedure, commandParameters))
            {
                List<Hashtable> list = DbReaderToHash(reader);
                return ArrayToTreeData(list, id, pid);
            }
        }

        /// <summary>
        /// 将db reader转换为Hashtable列表
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <returns>Hashtable</returns>
        private static List<Hashtable> DbReaderToHash(IDataReader reader)
        {
            var list = new List<Hashtable>();
            while (reader.Read())
            {
                var item = new Hashtable();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    var value = reader[i];
                    item[name] = value;
                }
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="list">线性数据</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        private static object ArrayToTreeData(IList<Hashtable> list, string id, string pid)
        {
            var hashtable = new Hashtable(); //数据索引 
            var returnList = new List<Hashtable>(); //数据池,要返回的 
            foreach (var item in list)
            {
                if (!item.ContainsKey(id)) continue;
                hashtable[item[id].ToString()] = item;
            }
            foreach (var item in list)
            {
                if (!item.ContainsKey(id)) continue;
                if (!item.ContainsKey(pid) || item[pid] == null || !hashtable.ContainsKey(item[pid].ToString()))
                {
                    returnList.Add(item);
                }
                else
                {
                    var pitem = hashtable[item[pid].ToString()] as Hashtable;
                    if (!pitem.ContainsKey("children"))
                        pitem["children"] = new List<Hashtable>();
                    var children = pitem["children"] as List<Hashtable>;
                    children.Add(item);
                }
            }
            return returnList;
        }
    }

    public class JsonMenuTreeData
    {
        /// <summary>
        /// 获取树格式对象的JSON
        /// </summary>
        /// <param name="commandText">commandText</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        public static string GetArrayJSON(string commandText, string id, string pid, CommandType commandType)
        {
            var obj = ArrayToTreeData(commandText, id, pid, commandType);
            return JsonHelper.ToJson(obj);
        }

        /// <summary>
        /// 获取树格式对象的JSON
        /// </summary>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="commandParameters">存储过程参数</param>
        /// <returns>JSON</returns>
        public static string GetArrayJSON(string id, string pid, string spName, CommandType commandType, params SqlParameter[] commandParameters)
        {
            var obj = ArrayToTreeData(id, pid, spName, commandType, commandParameters);
            return JsonHelper.ToJson(obj);
        }

        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="commandText">sql</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param> 
        /// <returns></returns>
        private static object ArrayToTreeData(string commandText, string id, string pid, CommandType commandType)
        {
            using (var reader = DatabaseFactory.ExecuteReader(commandText, commandType))
            {
                List<Hashtable> list = DbReaderToHash(reader);
                return ArrayToTreeData(list, id, pid);
            }
        }

        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="commandParameters">存储过程参数</param>
        /// <returns>JSON</returns>
        private static object ArrayToTreeData(string id, string pid, string spName, CommandType commandType, params SqlParameter[] commandParameters)
        {
            using (var reader = DatabaseFactory.ExecuteReader(spName, commandType, commandParameters))
            {
                List<Hashtable> list = DbReaderToHash(reader);
                return ArrayToTreeData(list, id, pid);
            }
        }

        /// <summary>
        /// 将db reader转换为Hashtable列表
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <returns>Hashtable</returns>
        private static List<Hashtable> DbReaderToHash(IDataReader reader)
        {
            var list = new List<Hashtable>();
            while (reader.Read())
            {
                var item = new Hashtable();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    var value = reader[i];
                    item[name] = value.ToString();
                }
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="list">线性数据</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        public static object ArrayToTreeData(IList<Hashtable> list, string id, string pid)
        {
            var hashtable = new Hashtable(); //数据索引 
            var returnList = new List<Hashtable>(); //数据池,要返回的 
            foreach (var item in list)
            {
                if (!item.ContainsKey(id)) continue;
                hashtable[item[id].ToString()] = item;
            }
            foreach (var item in list)
            {
                if (!item.ContainsKey(id)) continue;
                if (!item.ContainsKey(pid) || item[pid] == null || !hashtable.ContainsKey(item[pid].ToString()))
                {
                    returnList.Add(item);
                }
                else
                {
                    var pitem = hashtable[item[pid].ToString()] as Hashtable;
                    if (!pitem.ContainsKey("children"))
                        pitem["children"] = new List<Hashtable>();
                    var children = pitem["children"] as List<Hashtable>;
                    children.Add(item);
                }
            }
            return returnList;
        }
    }

    public class JsonData
    {
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="commandText">commandText</param>
        /// <returns>Hashtable</returns>
        public static List<Hashtable> GetArray(string commandText)
        {
            return ArrayToData(commandText);
        }

        /// <summary>
        /// 获取树格式对象的JSON
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="commandParameters">存储过程参数</param>
        /// <returns>Hashtable</returns>
        public static List<Hashtable> GetArray(string spName, params SqlParameter[] commandParameters)
        {
            return ArrayToData(spName, commandParameters);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="commandText">sql</param>
        /// <returns>Hashtable</returns>
        private static List<Hashtable> ArrayToData(string commandText)
        {
            using (var reader = DatabaseFactory.ExecuteReader(commandText, CommandType.Text))
            {
                return DbReaderToHash(reader);
            }
        }

        /// <summary>
        /// 获取格式对象
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="commandParameters">存储过程参数</param>
        /// <returns>JSON</returns>
        private static List<Hashtable> ArrayToData(string spName, params SqlParameter[] commandParameters)
        {
            using (var reader = DatabaseFactory.ExecuteReader(spName, CommandType.StoredProcedure, commandParameters))
            {
                return DbReaderToHash(reader);
            }
        }

        /// <summary>
        /// 将db reader转换为Hashtable列表
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <returns>Hashtable</returns>
        private static List<Hashtable> DbReaderToHash(IDataReader reader)
        {
            var list = new List<Hashtable>();
            while (reader.Read())
            {
                var item = new Hashtable();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    var value = reader[i];
                    item[name] = value;
                }
                list.Add(item);
            }
            return list;
        }
    }

    public class GridData
    {
        public DataTable Rows
        {
            get;
            set;
        }

        public int Total
        {
            get;
            set;
        }
    }
}
