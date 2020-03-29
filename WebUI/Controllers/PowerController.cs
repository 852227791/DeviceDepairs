using BLL;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class PowerController : BaseController
    {
        //
        // GET: /Power/

        public ActionResult PowerSet()
        {
            ViewBag.Title = "权限设置";
            return View();
        }

        public string GetPowerTree()
        {
            string roleId = Request.Form["RoleID"];
            string type = Request.Form["Type"];
            string cmdText = @"SELECT  CONVERT(NVARCHAR(100), m.MenuID) id ,
        m.ParentID ,
        m.Name text ,
        '' checked ,
        Queue
FROM    T_Sys_Menu m
WHERE   m.Status = 1
        AND m.MenuType = @Type
UNION ALL
SELECT  CONVERT(NVARCHAR(100), b.MenuID) + '-'
        + CONVERT(NVARCHAR(100), b.ButtonID) ,
        b.MenuID ,
        b.Name + ' (' + CONVERT(NVARCHAR(100),b.Num) + ':' + b.Code + ')' ,
        CASE WHEN ( SELECT  COUNT(PowerID)
                    FROM    T_Sys_Power
                    WHERE   RoleID = @RoleID
                            AND ButtonID = b.ButtonID
                  ) > 0 THEN 'true'
             ELSE ''
        END ,
        Queue
FROM    T_Sys_Button b
WHERE   b.Status = 1
        AND b.ButtonType = @Type
ORDER BY Queue ASC";
            SqlParameter[] paras = new SqlParameter[] 
            { 
                new SqlParameter("@RoleID", roleId),
                new SqlParameter("@Type", type)
            };
            return JsonMenuTreeData.GetArrayJSON("id", "ParentID", cmdText, CommandType.Text, paras);
        }

        public AjaxResult SavePower()
        {
            string roleId = Request.Form["RoleID"];
            string powerData = Request.Form["PowerData"];

            if (string.IsNullOrEmpty(roleId))
            {
                return AjaxResult.Error("请选择角色！");
            }
            if (string.IsNullOrEmpty(powerData))
            {
                return AjaxResult.Error("请选择权限！");
            }

            string result = PowerBLL.SavePower(roleId, powerData);

            if (result == "yes")
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }

        public AjaxResult DelPower()
        {
            string roleId = Request.Form["RoleID"];

            if (string.IsNullOrEmpty(roleId))
            {
                return AjaxResult.Error("请选择角色！");
            }

            string result = PowerBLL.DelPower(roleId);

            if (result == "yes")
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }

        public AjaxResult GetValidatePower()
        {
            string menuId = Request.Form["MenuID"];
            string num = Request.Form["Num"];
            string buttonCode = "%" + Request.Form["ButtonCode"] + "%";
            string validateStr = PowerBLL.ValidatePower(menuId, num, buttonCode, this.UserId.ToString());
            if (validateStr != "yes")
            {
                return AjaxResult.Error(validateStr);
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        public AjaxResult GetValidatePagePower()
        {
            string menuId = Request.Form["MenuID"];
            string validateStr = PowerBLL.ValidatePagePower(menuId, this.UserId);
            if (validateStr != "yes")
            {
                return AjaxResult.Error(validateStr);
            }
            else
            {
                return AjaxResult.Success();
            }
        }
    }
}
