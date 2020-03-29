using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ItemDetailModel
    {
        public string ItemDetailID { get; set; }
        public string Status { get; set; }
        public string ItemID { get; set; }
        public string DetailID { get; set; }
        public string Sort { get; set; }
        public string Money { get; set; }
        public string IsGive { get; set; }
        public string IsReport { get; set; }
        public string IsShow { get; set; }
        public string Queue { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public ItemDetailModel()
        { }
        public ItemDetailModel(string itemdetailid, string status, string itemid, string detailid, string sort, string money, string isgive, string isreport, string isshow, string queue, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.ItemDetailID = itemdetailid;
            this.Status = status;
            this.ItemID = itemid;
            this.DetailID = detailid;
            this.Sort = sort;
            this.Money = money;
            this.IsGive = isgive;
            this.IsReport = isreport;
            this.IsShow = isshow;
            this.Queue = queue;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }


    }
    public class ItemDetailFormModel
    {
        public string ItemDetailID { get; set; }

        public string ItemID1 { get; set; }

        public string DetailID { get; set; }

        public string SortID { get; set; }

        public string IsGive { get; set; }

        public string QueueID { get; set; }

        public string Money { get; set; }

        public string Remark1 { get; set; }

        public string IsReport1 { get; set; }

        public string IsShow1 { get; set; }
    }

    public class ItemDetailModels
    {
        public string ItemDetailID { get; set; }

        public string DetailID { get; set; }
        public string Name { get; set; }
        public string DeptName { get; set; }
        public string IsReport { get; set; }
        public string ParentName { get; set; }
        /// <summary>
        /// 类型：1多，2少
        /// </summary>
        public string Type { get; set; }

    }

    public class ItemDetailUploadModel
    {

        public string Id { get; set; }

        public string Pid { get; set; }

        public string Text { get; set; }

        public string Sort { get; set; }

        public string IsGive { get; set; }
        public string Money { get; set; }
        public string IsShow { get; set; }

        public string IsReport { get; set; }
        public string Queue { get; set; }

        public string Remark { get; set; }
    }

}
