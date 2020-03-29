using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    internal class ConvertHelper
    {
        /// <summary>
        /// 将object转为bool
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true或false</returns>
        public static bool ObjToBool(object obj)
        {
            bool flag;

            if (obj == null)
            {
                return false;
            }

            if (obj.Equals(DBNull.Value))
            {
                return false;
            }

            return (bool.TryParse(obj.ToString(), out flag) && flag);
        }

        /// <summary>
        /// 将object转为DateTime?
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>DateTime</returns>
        public static DateTime? ObjToDateTimeNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            try
            {
                return new DateTime?(Convert.ToDateTime(obj));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将object转为Decimal
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>decimal</returns>
        public static decimal ObjToDecimal(object obj)
        {
            if (obj == null)
            {
                return 0M;
            }

            if (obj.Equals(DBNull.Value))
            {
                return 0M;
            }

            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return 0M;
            }
        }
        
        /// <summary>
        /// 将object转为Decimal?
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>decimal?</returns>
        public static decimal? ObjToDecimalNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj.Equals(DBNull.Value))
            {
                return null;
            }

            return new decimal?(ObjToDecimal(obj));
        }

        /// <summary>
        /// 将object转为Int
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>int</returns>
        public static int ObjToInt(object obj)
        {
            if (obj != null)
            {
                int num;

                if (obj.Equals(DBNull.Value))
                {
                    return 0;
                }

                if (int.TryParse(obj.ToString(), out num))
                {
                    return num;
                }
            }

            return 0;
        }

        /// <summary>
        /// 将object转为Int?
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>int?</returns>
        public static int? ObjToIntNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj.Equals(DBNull.Value))
            {
                return null;
            }

            return new int?(ObjToInt(obj));
        }

        /// <summary>
        /// 将object转为string
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>string</returns>
        public static string ObjToStr(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            if (obj.Equals(DBNull.Value))
            {
                return "";
            }

            return Convert.ToString(obj);
        }
    }
}
