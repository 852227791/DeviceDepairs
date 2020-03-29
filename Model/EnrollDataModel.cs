using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    ///报名信息实体
    /// </summary>
    public class EnrollDataModel
    {
        private string examNum;
        private string name;
        private string idCard;
        private int sex;
        private string mobile;
        private string qq;
        private string wechat;
        private string address;
        private string nation;
        private string province;
        private string city;
        private string zip;
        private string schoolName;
        private string parentName;
        private string parentMobile;
        private int studyLevel;
        private string major;
        private long deptiId;
        private int year;
        private int month;
        /// <summary>
        /// 考生号
        /// </summary>
        public string _examNum
        {
            get
            {
                return examNum;
            }

            set
            {
                examNum = value;
            }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string _name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string _idCard
        {
            get
            {
                return idCard;
            }

            set
            {
                idCard = value;
            }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public int _sex
        {
            get
            {
                return sex;
            }

            set
            {
                sex = value;
            }
        }
        /// <summary>
        ///手机号
        /// </summary>
        public string _mobile
        {
            get
            {
                return mobile;
            }

            set
            {
                mobile = value;
            }
        }
        /// <summary>
        /// qq
        /// </summary>
        public string _qq
        {
            get
            {
                return qq;
            }

            set
            {
                qq = value;
            }
        }
        /// <summary>
        /// 微信
        /// </summary>
        public string _wechat
        {
            get
            {
                return wechat;
            }

            set
            {
                wechat = value;
            }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public string _address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
            }
        }
        /// <summary>
        /// 民族
        /// </summary>
        public string _nation
        {
            get
            {
                return nation;
            }

            set
            {
                nation = value;
            }
        }
        /// <summary>
        /// 生源地 省
        /// </summary>
        public string _province
        {
            get
            {
                return province;
            }

            set
            {
                province = value;
            }
        }
        /// <summary>
        /// 生源地 市
        /// </summary>
        public string _city
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
            }
        }
        /// <summary>
        /// 邮编
        /// </summary>
        public string _zip
        {
            get
            {
                return zip;
            }

            set
            {
                zip = value;
            }
        }
        /// <summary>
        /// 毕业学校
        /// </summary>
        public string _schoolName
        {
            get
            {
                return schoolName;
            }

            set
            {
                schoolName = value;
            }
        }
        /// <summary>
        /// 家长姓名
        /// </summary>
        public string _parentName
        {
            get
            {
                return parentName;
            }

            set
            {
                parentName = value;
            }
        }
        /// <summary>
        /// 家长电话
        /// </summary>
        public string _parentMobile
        {
            get
            {
                return parentMobile;
            }

            set
            {
                parentMobile = value;
            }
        }
        /// <summary>
        /// 学习层次
        /// </summary>
        public int _studyLevel
        {
            get
            {
                return studyLevel;
            }

            set
            {
                studyLevel = value;
            }
        }
        /// <summary>
        /// 录取专业
        /// </summary>
        public string _major
        {
            get
            {
                return major;
            }

            set
            {
                major = value;
            }
        }
        /// <summary>
        /// 主体编号
        /// </summary>
        public long _deptiId
        {
            get
            {
                return deptiId;
            }

            set
            {
                deptiId = value;
            }
        }
        /// <summary>
        /// 年份
        /// </summary>
        public int _year
        {
            get
            {
                return year;
            }

            set
            {
                year = value;
            }
        }
        /// <summary>
        /// 月份
        /// </summary>
        public int _month
        {
            get
            {
                return month;
            }

            set
            {
                month = value;
            }
        }
    }

    public class EnrollPostModel
    {
        /// <summary>
        /// 返回地址
        /// </summary>
        public string bankUrl { get; set; }
        /// <summary>
        /// 报名数据
        /// </summary>
        public EnrollDataModel enrollData { get; set; }
        /// <summary>
        /// 安全认证密钥
        /// </summary>
        public string key { get; set; }
    }

    public class ErrorCodeModel
    {
        public List<string> code { get; set; }
    }
}
