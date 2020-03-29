using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class ConfigBLL
    {
        public static int InsertConfig(ConfigModel cm)
        {
            string cmdText = @"INSERT INTO T_Pro_Config
(VoucherNum
,NoteNum
,PrintNum
)
VALUES (@VoucherNum
,@NoteNum
,@PrintNum
);SELECT CAST(scope_identity() AS int)";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@VoucherNum", cm.VoucherNum),
new SqlParameter("@NoteNum", cm.NoteNum),
new SqlParameter("@PrintNum", cm.PrintNum)
};
            int result = Convert.ToInt32(DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras));
            if (result > 0)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }

        public static int UpdateConfig(ConfigModel cm)
        {
            string cmdText = @"UPDATE T_Pro_Config SET
VoucherNum=@VoucherNum
,NoteNum=@NoteNum
,PrintNum=@PrintNum
WHERE ConfigID=@ConfigID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ConfigID", cm.ConfigID),
new SqlParameter("@VoucherNum", cm.VoucherNum),
new SqlParameter("@NoteNum", cm.NoteNum),
new SqlParameter("@PrintNum", cm.PrintNum)
};
            int result = DatabaseFactory.ExecuteNonQuery(cmdText, CommandType.Text, paras);
            if (result > 0)
            {
                return Convert.ToInt32(cm.ConfigID);
            }
            else
            {
                return -1;
            }
        }

        public static ConfigModel ConfigModelByWhere(string where, SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            ConfigModel cm = new ConfigModel();
            string cmdText = "SELECT * FROM T_Pro_Config WHERE 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                cm.ConfigID = dt.Rows[0]["ConfigID"].ToString();
                cm.VoucherNum = dt.Rows[0]["VoucherNum"].ToString();
                cm.NoteNum = dt.Rows[0]["NoteNum"].ToString();
                cm.PrintNum = dt.Rows[0]["PrintNum"].ToString();
            }
            return cm;
        }

        public static DataTable ConfigTableByWhere(string where, SqlParameter[] paras, string queue)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT * FROM T_Pro_Config WHERE 1=1 {0} {1}";
            cmdText = string.Format(cmdText, where, queue);
            dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            return dt;
        }

        public static ConfigModel GetConfigModel(string ConfigID)
        {
            string where = " and ConfigID=@ConfigID";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ConfigID",ConfigID)
            };
            ConfigModel cm = ConfigBLL.ConfigModelByWhere(where, paras);
            return cm;
        }
        /// <summary>
        /// 获取系统的打印次数
        /// </summary>
        /// <param name="ConfigID"></param>
        /// <returns></returns>
        public static int GetConfigPrintNum(string ConfigID)
        {
            string where = " and ConfigID=@ConfigID";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ConfigID",ConfigID)
            };
            ConfigModel cm = ConfigBLL.ConfigModelByWhere(where, paras);
            return int.Parse(cm.PrintNum);
        }

        /// <summary>
        /// 生成凭证号
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public static string getVoucherNum(string configId, string word)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string voucherNum = UpdateConfigToVoucherNum(configId);//1、证书费用 与T_Pro_Item表Sort字段相同编号
            string zero = "";
            switch (voucherNum.Length)
            {
                case 1:
                    zero = "00000";
                    break;
                case 2:
                    zero = "0000";
                    break;
                case 3:
                    zero = "000";
                    break;
                case 4:
                    zero = "00";
                    break;
                case 5:
                    zero = "0";
                    break;
                case 6:
                    zero = "";
                    break;
            }
            return word + date + zero + voucherNum;
        }

        public static string UpdateConfigToVoucherNum(string ConfigID)
        {
            string cmdText = @"UPDATE  T_Pro_Config
SET     VoucherNum = VoucherNum + 1
OUTPUT  Inserted.VoucherNum
WHERE   ConfigID = @ConfigID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ConfigID", ConfigID)
};
            string result = DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras).ToString();
            return result;
        }

        public static string getNoteNum(string configId, string word)
        {
            string noteNum = UpdateConfigToNoteNum(configId);//1、证书费用 与T_Pro_Item表Sort字段相同编号
            string zero = "";
            switch (noteNum.Length)
            {
                case 1:
                    zero = "00000";
                    break;
                case 2:
                    zero = "0000";
                    break;
                case 3:
                    zero = "000";
                    break;
                case 4:
                    zero = "00";
                    break;
                case 5:
                    zero = "0";
                    break;
                case 6:
                    zero = "";
                    break;
            }
            return word + zero + noteNum;
        }

        public static string UpdateConfigToNoteNum(string ConfigID)
        {
            string cmdText = @"UPDATE  T_Pro_Config
SET     NoteNum = NoteNum + 1
OUTPUT  Inserted.NoteNum
WHERE   ConfigID = @ConfigID";
            SqlParameter[] paras = new SqlParameter[] {
new SqlParameter("@ConfigID", ConfigID)
};
            string result = DatabaseFactory.ExecuteScalar(cmdText, CommandType.Text, paras).ToString();
            return result;
        }
    }
}
