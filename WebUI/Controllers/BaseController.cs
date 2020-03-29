using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [AuthorizeAttribute]
    public class BaseController : Controller
    {
        public int UserId
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                    return Convert.ToInt32(User.Identity.Name);
                else
                    return -1;
            }
        }

        public string UserName
        {
            get
            {
                string where = " AND UserID = @UserID";
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@UserID", UserId)
                };
                DataTable dt = UserBLL.UserTableByWhere(where, paras, "");
                string userName = "系统错误";
                if (dt.Rows.Count > 0)
                {
                    userName = dt.Rows[0]["Name"].ToString();
                }
                return userName;
            }
        }

        public ActionResult ResponseWriteResult(string str)
        {
            Response.ContentType = "text/plain";
            Response.Write(str);
            Response.End();
            return new EmptyResult();
        }

        /// <summary>
        /// 数据权限sql语句用于列表
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public string PurviewToList(string MenuID)
        {
            string str = "";
            string bracketS = "(";
            string bracketE = ")";

            string where = @" AND UserID = @UserID
                AND RoleID IN ( SELECT  RoleID
                                FROM    T_Sys_Power
                                WHERE   MenuID = @MenuID
                                        AND RoleID IN ( SELECT  RoleID
                                                        FROM    T_Sys_UserRole
                                                        WHERE   UserID = @UserID ) )";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@MenuID", MenuID),
                new SqlParameter("@UserID", UserId)
            };
            DataTable dt = PurviewBLL.PurviewTableByWhere(where, paras, "");

            if (dt.Rows.Count < 2)
            {
                bracketS = "";
                bracketE = "";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    str += " AND " + bracketS;
                }
                else
                {
                    str += " OR ";
                }
                if (dt.Rows[i]["Range"].ToString() == "1")
                {
                    str += "{0} in (SELECT DeptID FROM GetChildrenDeptID(" + dt.Rows[i]["DeptID"] + "))";
                }
                else if (dt.Rows[i]["Range"].ToString() == "2")
                {
                    str += "({0} in (SELECT DeptID FROM GetChildrenDeptID(" + dt.Rows[i]["DeptID"] + ")) AND {1} = " + UserId + ")";
                }
            }

            str += bracketE;
            if (str == "")
            {
                str = " AND 1 = 2";
            }
            return str;
        }



        /// <summary>
        /// 数据权限sql语句用于树
        /// </summary>
        /// <param name="MenuID"></param>
        /// <param name="IsEdit"></param>
        /// <returns></returns>
        public string PurviewToTree(string MenuID, string IsEdit)
        {
            string str = "";
            string bracketS = "(";
            string bracketE = ")";

            string isEditStr = "1 = 1";
            if (IsEdit == "Yes")
            {
                isEditStr = "(Code LIKE '%add%' OR Code LIKE '%edit%' OR Code LIKE '%upload%' OR Code LIKE '%trunMajor%')";
            }

            string where = @" AND UserID = @UserID
                AND RoleID IN ( SELECT  RoleID
                                FROM    T_Sys_Power
                                WHERE   MenuID = @MenuID
                                        AND RoleID IN ( SELECT  RoleID
                                                        FROM    T_Sys_UserRole
                                                        WHERE   UserID = @UserID )
                                        AND ButtonID IN ( SELECT    ButtonID
                                                          FROM      T_Sys_Button
                                                          WHERE     Status = 1
                                                                    AND MenuID = @MenuID
                                                                    AND " + isEditStr + " ) )";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@MenuID", MenuID),
                new SqlParameter("@UserID", UserId)
            };
            DataTable dt = PurviewBLL.PurviewTableByWhere(where, paras, "");

            if (dt.Rows.Count < 2)
            {
                bracketS = "";
                bracketE = "";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    str += " AND " + bracketS;
                }
                else
                {
                    str += " OR ";
                }
                str += "{0} in (SELECT DeptID FROM GetChildrenDeptID(" + dt.Rows[i]["DeptID"] + "))";// OR {0} in (SELECT DeptID FROM GetParentDeptID(" + dt.Rows[i]["DeptID"] + "))
            }

            str += bracketE;
            if (str == "")
            {
                str = " AND 1 = 2";
            }
            return str;
        }

        /// <summary>
        /// 通用的编辑返回提示
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public string ReturnMessage(bool flag)
        {
            if (flag)
            {
                return "yes";
            }
            else {
                return "出现未知错误，请联系管理员";
            }
        }
    }
    public class TestMemberAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var isAuthorized = false;
            if (httpContext != null && httpContext.Session != null)
            {
                if (HttpContext.Current.Session["UserName"] != null)
                {
                    isAuthorized = true;
                }
            }

            return isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            string path = filterContext.HttpContext.Request.Path;
            const string strUrl = "/Home/Index?ReturnUrl={0}";
            filterContext.HttpContext.Response.Redirect(string.Format(strUrl, HttpUtility.UrlEncode(path)), true);
        }
    }
}
