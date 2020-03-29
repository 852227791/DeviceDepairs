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
    public class AuthorizeController : Controller
    {
        //
        // GET: /Authorize/

        public ActionResult Login()
        {
            ViewBag.Title = "收费管理系统用户登录";
            return View();
        }

        public ActionResult TestPrint()
        {
            ViewBag.Title = "测试打印控件";
            return View();
        }

        public ActionResult TestNewPrint()
        {
            ViewBag.Title = "测试新版打印控件";
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel um, string Code)
        {
            ViewBag.Title = "收费管理系统用户登录";
            string validateCode = Session["ValidateCode"] == null ? "0123456789" : Session["ValidateCode"].ToString();

            if (Code != validateCode)
            {
                ViewBag.Message = "验证码输入错误！";
                return View();
            }
            if (string.IsNullOrEmpty(um.LoginName))
            {
                ViewBag.Message = "用户名不能为空！";
                return View();
            }
            if (string.IsNullOrEmpty(um.Password))
            {
                ViewBag.Message = "密码不能为空！";
                return View();
            }

            string where = " AND Status = 1 AND LoginName = @LoginName AND Password = @Password";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@LoginName", um.LoginName), 
                new SqlParameter("@Password", Encryption.MD5Encrypt(um.Password)) 
            };
            UserModel loginum = UserBLL.UserModelByWhere(where, paras);
            int userId = -1;
            if (loginum.UserID != null)
            {
                userId = Convert.ToInt32(loginum.UserID);
                loginum.LoginTime = DateTime.Now.ToString();
                UserBLL.UpdateUser(loginum);
            }

            if (userId == -1)
            {
                ViewBag.Message = "用户名或密码错误！";
                return View();
            }
            else
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie(userId.ToString(), true);

                string returnurl = @Url.Content("~/");
                return new RedirectResult(returnurl);
            }
        }

        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();

            string returnurl = @Url.Content("~/");

            return new RedirectResult(returnurl);
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

    }
}
