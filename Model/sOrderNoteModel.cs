using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class sOrderNoteFormModel
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string sOrderID { get; set; }
        /// <summary>
        /// 改变后的应交金额
        /// </summary>
        public string ChangeShouldMoney { get; set; }
        /// <summary>
        /// 变更原因
        /// </summary>
        public string ChangeShouldMoneyRemark { get; set; }
    }
    public class sOrderNoteModel
    {
        public string sOrderNoteID { get; set; }
        public string Status { get; set; }
        public string sOrderID { get; set; }
        public string FieldName { get; set; }
        public string ValueOld { get; set; }
        public string ValueNew { get; set; }
        public string Explain { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public sOrderNoteModel()
        { }
        public sOrderNoteModel(string sordernoteid, string status, string sorderid, string fieldname, string valueold, string valuenew, string explain, string createid, string createtime, string updateid, string updatetime)
        {
            this.sOrderNoteID = sordernoteid;
            this.Status = status;
            this.sOrderID = sorderid;
            this.FieldName = fieldname;
            this.ValueOld = valueold;
            this.ValueNew = valuenew;
            this.Explain = explain;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }

      
    }
}
