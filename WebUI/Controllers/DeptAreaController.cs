using BLL;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class DeptAreaController : BaseController
    {
        //
        // GET: /DeptArea/

        public ActionResult DeptAreaList()
        {
            return View();
        }
        public ActionResult DeptAreaEdit()
        {
            return View();
        }
        /// <summary>
        /// 获取收费校区下拉菜单
        /// </summary>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        public string GetDeptAreaCombobox(string DeptID)
        {
            return JsonHelper.DataTableToJson(DeptAreaBLL.GetDeptAreaCombobox(DeptID));
        }
        /// <summary>
        /// 获取收费校区列表信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDeptAreaList()
        {
            string deptId = Request.Form["DeptID"];
            if (string.IsNullOrEmpty(deptId))
                deptId = "0";
            string cmdText = @"SELECT  Name ,
        da.Queue ,
        DeptAreaID ,
        r1.RefeName Status,
        da.Status StatusValue,
        da.DeptID
        FROM T_Pro_DeptArea da
        LEFT JOIN T_Sys_Refe r1 ON r1.Value = da.Status
                                   AND r1.RefeTypeID = 1
        WHERE da.DeptID={0}  ";
            cmdText = string.Format(cmdText, deptId);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        /// <summary>
        /// 编辑收费校区
        /// </summary>
        /// <param name="dam"></param>
        /// <returns></returns>
        public string GetDeptAreaEdit(DeptAreaModel dam)
        {
            if (string.IsNullOrEmpty(dam.DeptAreaID))
                dam.DeptAreaID = "0";
            if (string.IsNullOrEmpty(dam.Name))
                return "名称不能为空";

            if (ValidateDeptAreaName(dam.Name, dam.DeptAreaID,dam.DeptID))
                return "名称不能重复";

            bool flag = false;
            if (dam.DeptAreaID.Equals("0"))
            {
                dam.Status = "1";
                dam.CreateID = this.UserId.ToString();
                dam.CreateTime = DateTime.Now.ToString();
                dam.UpdateID = UserId.ToString();
                dam.UpdateTime = DateTime.Now.ToString();
                if (DeptAreaBLL.InsertDeptArea(dam) > 0)
                    flag = true;
            }
            else
            {
                DeptAreaModel editdam = DeptAreaBLL.DeptAreaModelByWhere(dam.DeptAreaID);
                LogBLL.CreateLog("T_Stu_DeptArea", this.UserId.ToString(), editdam, dam);//写日志
                editdam.Name = dam.Name;
                editdam.Queue = dam.Queue;
                editdam.UpdateID = UserId.ToString();
                editdam.UpdateTime = DateTime.Now.ToString();
                if (DeptAreaBLL.UpdateDeptArea(editdam) > 0)
                    flag = true;
            }

            return ReturnMessage(flag);
        }
        /// <summary>
        /// 验证收费校区名称是否重复
        /// </summary>
        /// <param name="name"></param>
        /// <param name="deptAreaId"></param>
        /// <returns></returns>
        private bool ValidateDeptAreaName(string name, string deptAreaId,string deptId)
        {
            string where = " and DeptAreaID<>@DeptAreaID and Name=@Name and DeptID=@DeptID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DeptAreaID",deptAreaId),
                 new SqlParameter("@Name",name),
                    new SqlParameter("@DeptID",deptId)
            };
            DataTable dt = DeptAreaBLL.DeptAreaTableByWhere(where, paras, "");
            return dt.Rows.Count > 0;
        }
        /// <summary>
        /// 表单验证名称是否重复
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public AjaxResult FormValidateDeptAreaName(string ID, string Value)
        {
            string deptId = Request.Form["TypeId"];
            if (ValidateDeptAreaName(Value, ID, deptId))
                return AjaxResult.Error();
            return AjaxResult.Success();
        }
        /// <summary>
        /// 变更收费校区状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public AjaxResult GetUpdateStatus(string ID, string Value)
        {
            DeptAreaModel dam = DeptAreaBLL.DeptAreaModelByWhere(ID);
            dam.Status = Value;
            dam.UpdateID = UserId.ToString();
            dam.UpdateTime = DateTime.Now.ToString();
            if (DeptAreaBLL.UpdateDeptArea(dam) > 0)
                return AjaxResult.Success();
            return AjaxResult.Error();
        }
    }
}
