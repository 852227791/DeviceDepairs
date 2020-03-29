using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BLL;
using Model;

namespace WebUI.Controllers
{
    public class AreaController : BaseController
    {
        //
        // GET: /Area/AreaList

        public ActionResult AreaList() {
            return View();
        }
        public ActionResult AreaEdit()
        {
            return View();
        }
        public AjaxResult GetProvinceFile()
        {
            string json1 = getAreaTable("1");
            OtherHelper.CreateJsonFile(json1, "province.json");
            string json2 = getAreaTable("2");
            OtherHelper.CreateJsonFile(json2, "city.json");
            string json3 = getAreaTable("3");
            OtherHelper.CreateJsonFile(json3, "district.json");
            string json4 = getAreaTable("4");
            OtherHelper.CreateJsonFile(json4, "area.json");
            return AjaxResult.Success("生成成功");

        }

        private string getAreaTable(string type)
        {
            string where = string.Empty;
            if (type.Equals("1"))
            {
                where = " and Status=1 and ParentID=0";
            }
            if (type.Equals("2"))
            {
                where = " and Status=1 and ParentID IN(SELECT AreaID FROM T_Pro_Area WHERE ParentID=0)";
            }
            if (type.Equals("3"))
            {
                where = " and Status=1 and ParentID IN(SELECT AreaID FROM T_Pro_Area WHERE ParentID IN (SELECT AreaID FROM T_Pro_Area WHERE ParentID=0))";
            }
            if (type.Equals("4"))
            {
                string cmdText = "SELECT AreaID id,Name text,ParentID pid FROM T_Pro_Area  ";
                return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "pid", CommandType.Text);
            }
            return JsonHelper.DataTableToJson(AreaBLL.AreaTableByWhere(where, null));
        }

        public ActionResult GetAreaList(string Name)
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(Name))
            {
                where += " and a.Name like '%" + Name + "%'";
            }
            string cmdText = @"SELECT  a.Name ,
        a1.Name ParentName ,
        r1.RefeName Status,
		a.AreaID,
		a.ParentID
FROM    T_Pro_Area a
        LEFT JOIN T_Pro_Area a1 ON a1.AreaID = a.ParentID
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = a.Status
                                   AND r1.RefeTypeID = 1 
        Where 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        public string GetAreaEdit(AreaModel am)
        {
            if (string.IsNullOrEmpty(am.Name))
            {
                return "名称不能为空";
            }
            else
            {
                if (am.Name.Length>16)
                {
                    return "名称不能超过16个字符";
                }
            }

            if (string.IsNullOrEmpty(am.AreaID))
            {
                if (string.IsNullOrEmpty(am.ParentID))
                {
                    am.ParentID = "0";
                }
                am.Status = "1";
                am.UpdateID = this.UserId.ToString();
                am.UpdateTime = DateTime.Now.ToString();
                am.CreateID = UserId.ToString();
                am.CreateTime = DateTime.Now.ToString();
                AreaBLL.InsertArea(am);
                return "yes";
            }
            else
            {
                if (string.IsNullOrEmpty(am.ParentID))
                {
                    am.ParentID = "0";
                }
                AreaModel editam = AreaBLL.GetAreaModelByAreaID(am.AreaID);
                editam.Name = am.Name;
                editam.ParentID = am.ParentID;
                editam.UpdateID = this.UserId.ToString();
                editam.UpdateTime = DateTime.Now.ToString();
                AreaBLL.UpdateArea(editam);
                return "yes";
            }
        }
        public AjaxResult GetUpdateStatus(string ID, string Value)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return AjaxResult.Error("区域信息");
            }
            if (AreaBLL.UpdateAreaStatus(ID, Value, UserId))
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("出现未知错误，请联系管理员！");
            }
        }

    }
}
