using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ButtonController : BaseController
    {
        //
        // GET: /Botton/

        public AjaxResult GetMyButton()
        {
            string menuId = Request.Form["MenuID"];
            string num = Request.Form["Num"];
            string cmdText = @"SELECT  Code ,
        Name ,
        IconPath
FROM    T_Sys_Button
WHERE   Status = 1
        AND ShowType = 1
        AND MenuID = {0}
        AND Num = {1}
        AND ButtonID IN ( SELECT    ButtonID
                          FROM      T_Sys_Power
                          WHERE     MenuID = {0}
                                    AND RoleID IN ( SELECT  RoleID
                                                    FROM    T_Sys_UserRole
                                                    WHERE   UserID = {2} ) )
ORDER BY Queue ASC";
            cmdText = string.Format(cmdText, menuId, num, this.UserId);

            return AjaxResult.Success(JsonData.GetArray(cmdText));
        }

    }
}
