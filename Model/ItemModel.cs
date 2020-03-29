using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ItemModel
    {
        public string ItemID { get; set; }
        public string Status { get; set; }
        public string DeptID { get; set; }
        public string ParentID { get; set; }
        public string Sort { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string IsPlan { get; set; }
        public string PlanLevel { get; set; }
        public string IsReport { get; set; }
        public string IsShow { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string LimitTime { get; set; }
        public string Queue { get; set; }
        public string Remark { get; set; }
        public string CreateID { get; set; }
        public string CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateTime { get; set; }

        public ItemModel() { }

        public ItemModel(string itemid, string status, string deptid, string parentid, string sort, string year, string month, string name, string englishname, string isplan, string planlevel, string isreport, string isshow, string starttime, string endtime, string limittime, string queue, string remark, string createid, string createtime, string updateid, string updatetime)
        {
            this.ItemID = itemid;
            this.Status = status;
            this.DeptID = deptid;
            this.ParentID = parentid;
            this.Sort = sort;
            this.Year = year;
            this.Month = month;
            this.Name = name;
            this.EnglishName = englishname;
            this.IsPlan = isplan;
            this.PlanLevel = planlevel;
            this.IsReport = isreport;
            this.IsShow = isshow;
            this.StartTime = starttime;
            this.EndTime = endtime;
            this.LimitTime = limittime;
            this.Queue = queue;
            this.Remark = remark;
            this.CreateID = createid;
            this.CreateTime = createtime;
            this.UpdateID = updateid;
            this.UpdateTime = updatetime;
        }
    }
    public class CopyDetailModel
    {
        public string fromDeptId { get; set; }

        public string[] fromItemDetailId { get; set; }

        public string toDeptId { get; set; }

        public string toItemId { get; set; }
    }
    public class ItemModels
    {
        public string ItemID { get; set; }

        public string IsReport { get; set; }

        public string Name { get; set; }

        public string DeptName { get; set; }

        public string ParentName { get; set; }
        /// <summary>
        /// 类型：1 多,2 少
        /// </summary>
        public string Type { get; set; }
    }

    public class ItemUploadModel
    {
        public string Id { get; set; }

        public string Pid { get; set; }

        public string Text { get; set; }

        public string EnglishName { get; set; }

        public string Sort { get; set; }

        public string Year { get; set; }

        public string Month { get; set; }

        public string IsPlan { get; set; }

        public string PlanLevel { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string LimitTime { get; set; }

        public string Queue { get; set; }

        public string IsReport { get; set; }

        public string IsShow { get; set; }

        public string Remark { get; set; }
    }

}
