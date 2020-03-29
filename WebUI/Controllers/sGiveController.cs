using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common;
using Model;

namespace WebUI.Controllers
{
    public class sGiveController : BaseController
    {
        #region 页面
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sGiveList()
        {
            return View();
        }

        /// <summary>
        /// 添加、修改页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sGiveEdit()
        {
            return View();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 得到配品列表信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetsGiveList()
        {
            string menuId = Request.Form["MenuID"];
            string deptId = Request.Form["DeptID"];
            string where = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND DeptID=" + deptId + "";
            }
            string cmdText = @"SELECT    *
  FROM      T_Stu_sGive WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "DeptID", "CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, ""));
        }

        /// <summary>
        /// 添加、修改配品
        /// </summary>
        /// <returns></returns>
        public string GetsGiveEdit(sGiveModel ifm)
        {
            string result = "出现未知错误，请联系管理员";

            #region 后台验证
            string sGiveID = "0";
            if (!string.IsNullOrEmpty(ifm.sGiveID))
            {
                sGiveID = ifm.sGiveID;
            }
            if (string.IsNullOrEmpty(ifm.Name))
            {
                return "配品名称不能为空";
            }
            if (!string.IsNullOrEmpty(ifm.Name) && ifm.Name.Length > 32)
            {
                return "配品名称不能超过32个字符";
            }
            if (string.IsNullOrEmpty(ifm.Money))
            {
                return "配品金额不能为空";
            }
            if (!string.IsNullOrEmpty(ifm.Remark))
            {
                ifm.Remark = ifm.Remark.Replace("\r\n", "<br />");
            }
            #endregion

            try
            {
                if (sGiveID.Trim() != "0")
                {
                    #region 修改
                    string where = " AND sGiveID = @sGiveID";
                    SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@sGiveID", sGiveID) };
                    sGiveModel model = sGiveBLL.sGiveModelByWhere(where, paras);

                    LogBLL.CreateLog("T_Stu_sGive", this.UserId.ToString(), model, ifm);//写日志

                    model.Name = ifm.Name;
                    model.Money = ifm.Money;
                    model.Remark = ifm.Remark;
                    model.UpdateID = this.UserId.ToString();
                    model.UpdateTime = DateTime.Now.ToString();

                    if (sGiveBLL.UpdatesGive(model) > 0)
                    {
                        result = "yes";
                    }
                    #endregion
                }
                else
                {
                    #region 添加
                    ifm.Status = "1";
                    ifm.CreateID = this.UserId.ToString();
                    ifm.CreateTime = DateTime.Now.ToString();
                    ifm.UpdateID = this.UserId.ToString();
                    ifm.UpdateTime = DateTime.Now.ToString();
                    int returnID = sGiveBLL.InsertsGive(ifm);
                    if (returnID > 0)
                    {
                        result = "yes";
                    }
                    #endregion
                }
            }
            catch
            {
                result = "出现未知错误，请联系管理员";
            }
            return result;
        }

        /// <summary>
        /// 修改绑定配品
        /// </summary>
        /// <returns></returns>
        public AjaxResult SelectsGive()
        {
            string sgiveId = Request.Form["ID"];
            string where = " AND sGiveID = @sGiveID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@sGiveID", sgiveId)
            };
            DataTable dt = sGiveBLL.sGiveTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        /// <summary>
        /// 验证配品名称在当前主体下是否重复
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckNameRepeat()
        {
            string name = Request.Form["Value"];
            string giveId = Request.Form["ID"];
            string deptId = Request.Form["TypeID"];

            if (CheckNameRepeatMethod(name, giveId, deptId))
            {
                return AjaxResult.Error("配品名称不能重复");
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        /// <summary>
        /// 验证配品名称是否重复的方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="giveId"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        private bool CheckNameRepeatMethod(string name, string giveId, string deptId)
        {
            string where = " AND sGiveID <> @sGiveID AND Name = @Name AND DeptID = @DeptID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@sGiveID", giveId),
                new SqlParameter("@Name", name),
                new SqlParameter("@DeptID", deptId)
            };
            DataTable dt = sGiveBLL.sGiveTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改状态，启用、停用
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetUpdatesStatus()
        {
            string sGiveID = Request.Form["ID"];
            string Status = Request.Form["Value"];

            string where = " AND sGiveID = @sGiveID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@sGiveID", sGiveID)
            };
            sGiveModel dm = sGiveBLL.sGiveModelByWhere(where, paras);
            dm.Status = Status;
            if (sGiveBLL.UpdatesGive(dm) > 0)
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
        }
        #endregion

        /// <summary>
        /// 获取配品下拉
        /// </summary>
        /// <returns></returns>
        public string GetsItemsGiveCombobox() {
            string deptId = Request.QueryString["DeptID"];
            return JsonHelper.DataTableToJson(sGiveBLL.GetsGiveCombobox(deptId));
        }
    }
}
