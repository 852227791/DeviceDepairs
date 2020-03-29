using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DatabaseFactory
    {
        private static string connectionString = SqlHelper.GetConnSting();

        /// <summary>
        /// 执行指定连接字符串,类型的SqlCommand
        /// </summary>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <returns>返回命令影响的行数</returns>
        public static int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText);
        }

        /// <summary>
        /// 执行指定连接字符串,类型的SqlCommand
        /// </summary>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>返回命令影响的行数</returns>
        public static int ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText, commandParameters);
        }

        /// <summary>
        /// 执行指定数据库连接字符串的命令,返回DataSet
        /// </summary>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet ExecuteDataset(string commandText, CommandType commandType)
        {
            return SqlHelper.ExecuteDataset(connectionString, commandType, commandText);
        }

        /// <summary>
        /// 执行指定数据库连接字符串的命令,返回DataSet
        /// </summary>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet ExecuteDataset(string commandText, CommandType commandType, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset(connectionString, commandType, commandText, commandParameters);
        }

        /// <summary>
        /// 执行指定数据库连接对象的数据阅读器
        /// </summary>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            return SqlHelper.ExecuteReader(connectionString, commandType, commandText);
        }

        /// <summary>
        /// 执行指定数据库连接对象的数据阅读器
        /// </summary>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteReader(connectionString, commandType, commandText, commandParameters);
        }

        /// <summary>
        /// 执行指定数据库连接字符串的命令,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(string commandText, CommandType commandType)
        {
            return SqlHelper.ExecuteScalar(connectionString, commandType, commandText);
        }

        /// <summary>
        /// 执行指定数据库连接字符串的命令,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(string commandText, CommandType commandType, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteScalar(connectionString, commandType, commandText, commandParameters);
        }
    }
}
