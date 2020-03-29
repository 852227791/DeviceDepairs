using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 将object转为bool
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true或false</returns>
        public static bool ToBool(this object obj)
        {
            return ConvertHelper.ObjToBool(obj);
        }

        /// <summary>
        /// 将object转为DateTime?
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>DateTime</returns>
        public static DateTime? ToDateTimeNull(this object obj)
        {
            return ConvertHelper.ObjToDateTimeNull(obj);
        }

        /// <summary>
        /// 将object转为Decimal
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>decimal</returns>
        public static decimal ToDecimal(this object obj)
        {
            return ConvertHelper.ObjToDecimal(obj);
        }

        /// <summary>
        /// 将object转为Decimal?
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>decimal?</returns>
        public static decimal? ToDecimalNull(this object obj)
        {
            return ConvertHelper.ObjToDecimalNull(obj);
        }

        /// <summary>
        /// 将object转为Int
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>int</returns>
        public static int ToInt(this object obj)
        {
            return ConvertHelper.ObjToInt(obj);
        }

        /// <summary>
        /// 将object转为Int?
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>int?</returns>
        public static int? ToIntNull(this object obj)
        {
            return ConvertHelper.ObjToIntNull(obj);
        }

        /// <summary>
        /// 将object转为string
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>string</returns>
        public static string ToStr(this object obj)
        {
            return ConvertHelper.ObjToStr(obj);
        }
    }
}
