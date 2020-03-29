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
    public class UserController : BaseController
    {
        //
        // GET: /User/

        public ActionResult UserList()
        {
            ViewBag.Title = "用户管理";
            return View();
        }

        public ActionResult UserEdit()
        {
            ViewBag.Title = "用户编辑";
            return View();
        }

        /// <summary>
        /// 输出用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserList()
        {
            string loginname = Request.Form["txtLoginName"];
            string name = Request.Form["txtName"];
            string status = Request.Form["selStatus"];
            string userType = Request.Form["selUserType"];
            string sex = Request.Form["selSex"];
            string mobile = Request.Form["txtMobile"];
            string loginTimeS = Request.Form["txtLoginTimeS"];
            string loginTimeE = Request.Form["txtLoginTimeE"];
            string where = "";
            if (!string.IsNullOrEmpty(loginname))
            {
                where += " and u.LoginName like '%" + loginname + "%'";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " and u.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "u.Status");
            }
            if (!string.IsNullOrEmpty(userType))
            {
                where += OtherHelper.MultiSelectToSQLWhere(userType, "u.UserType");
            }
            if (!string.IsNullOrEmpty(sex))
            {
                where += OtherHelper.MultiSelectToSQLWhere(sex, "u.Sex");
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                where += " and u.Mobile like '%" + mobile + "%'";
            }
            if (!string.IsNullOrEmpty(loginTimeS))
            {
                where += " and convert(NVARCHAR(10),u.LoginTime,23) >= '" + loginTimeS + "'";
            }
            if (!string.IsNullOrEmpty(loginTimeE))
            {
                where += " and convert(NVARCHAR(10),u.LoginTime,23) <= '" + loginTimeE + "'";
            }
            string cmdText = @"SELECT  u.UserID ,
        u.LoginName ,
        u.Name ,
        u.Status StatusValue ,
        r1.RefeName Status ,
        u.UserType UserTypeValue ,
        r2.RefeName UserType ,
        r3.RefeName Sex ,
        u.Mobile ,
        u.LoginTime
FROM    T_Sys_User u
        LEFT JOIN T_Sys_Refe r1 ON u.Status = r1.Value
                                   AND r1.RefeTypeID = 1
        LEFT JOIN T_Sys_Refe r2 ON u.UserType = r2.Value
                                   AND r2.RefeTypeID = 2
        LEFT JOIN T_Sys_Refe r3 ON u.Sex = r3.Value
                                   AND r3.RefeTypeID = 3
WHERE   u.UserID <> 1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        /// <summary>
        /// 验证登录名是否重复
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckLoginName()
        {
            string loginName = Request.Form["Value"];
            string userId = Request.Form["ID"];

            if (string.IsNullOrEmpty(userId))
            {
                userId = "0";
            }
            if (CheckLoginNameMethod(userId, loginName))
            {
                return AjaxResult.Error("该登录名已经存在，请重新输入");
            }
            else
            {
                return AjaxResult.Success();
            }
        }
        //编辑用户
        public string GetUserEdit(UserModel um)
        {
            if (string.IsNullOrEmpty(um.UserID))
            {
                um.UserID = "0";
            }

            if (um.LoginName.Length < 1 || um.LoginName.Length > 16)
            {
                return "登录名不能为空，并且长度为1-16个字符";
            }
            if (CheckLoginNameMethod(um.UserID, um.LoginName))
            {
                return "该登录名已经存在，请重新输入";
            }
            if (um.Name.Length < 1 || um.Name.Length > 8)
            {
                return "姓名不能为空，并且长度为1-8个字符";
            }
            if (!string.IsNullOrEmpty(um.Mobile))
            {
                if (um.Mobile.Length > 16)
                {
                    return "手机长度不能大于16个字符";
                }
            }
            if (!string.IsNullOrEmpty(um.Remark))
            {
                um.Remark = um.Remark.Replace("\r\n", "<br />");
            }
            string result = "no";
            if (um.UserID != "0")
            {
                string where = " AND UserID=@UserID";
                SqlParameter[] paras = new SqlParameter[] { 
                    new SqlParameter("@UserID", um.UserID)
                };
                UserModel editum = UserBLL.UserModelByWhere(where, paras);

                LogBLL.CreateLog("T_Sys_User", this.UserId.ToString(), editum, um);//写日志

                editum.LoginName = um.LoginName;
                editum.Name = um.Name;
                editum.UserType = um.UserType;
                editum.Sex = um.Sex;
                editum.Mobile = um.Mobile;
                editum.Remark = um.Remark;
                editum.UpdateID = this.UserId.ToString();
                editum.UpdateTime = DateTime.Now.ToString();
                if (UserBLL.UpdateUser(editum) > 0)
                {
                    return "yes";
                }
            }
            else
            {
                um.Status = "1";
                um.Password = Encryption.MD5Encrypt("123456");
                um.LoginTime = DateTime.Now.ToString();
                um.CreateID = this.UserId.ToString();
                um.CreateTime = DateTime.Now.ToString();
                um.UpdateID = this.UserId.ToString();
                um.UpdateTime = DateTime.Now.ToString();
                if (UserBLL.InsertUser(um) > 0)
                {
                    return "yes";
                }
            }
            return result;
        }

        //查询用户
        public AjaxResult SelectUser()
        {
            string userID = Request.Form["ID"];
            string where = " AND UserID = @UserID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@UserID", userID)
            };
            DataTable dt = UserBLL.UserTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        //更改状态
        public AjaxResult GetUpdateStatus()
        {
            string userid = Request.Form["ID"];
            string status = Request.Form["Value"];

            string where = " AND UserID = @UserID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@UserID", userid)
            };
            UserModel um = UserBLL.UserModelByWhere(where, paras);
            um.Status = status;
            if (UserBLL.UpdateUser(um) > 0)
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetResetPassword()
        {
            string userid = Request.Form["ID"];
            string password = Request.Form["Value"];

            string where = " AND UserID = @UserID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@UserID", userid)
            };
            UserModel um = UserBLL.UserModelByWhere(where, paras);
            um.Password = Encryption.MD5Encrypt(password);
            if (UserBLL.UpdateUser(um) > 0)
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public string GetChangePassword()
        {
            string oldPassword = Request.Form["txtOldPassword"];
            string newPassword = Request.Form["txtNewPassword"];
            string okPassword = Request.Form["txtOKPassword"];

            if (string.IsNullOrEmpty(oldPassword))
            {
                return "请输入旧密码";
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return "请输入新密码";
            }
            if (string.IsNullOrEmpty(okPassword))
            {
                return "请输入确认密码";
            }
            if (newPassword != okPassword)
            {
                return "两次密码输入不一致";
            }

            if (!CheckPasswordMethod(oldPassword))
            {
                return "旧密码输入错误";
            }

            string where = " AND UserID = @UserID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@UserID", this.UserId.ToString())
            };
            UserModel um = UserBLL.UserModelByWhere(where, paras);
            um.Password = Encryption.MD5Encrypt(newPassword);
            if (UserBLL.UpdateUser(um) > 0)
            {
                return "yes";
            }
            else
            {
                return "发现系统错误";
            }
        }

        /// <summary>
        /// 验证旧密码是否正确
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckOldPassword()
        {
            string oldPassword = Request.Form["Value"];

            if (CheckPasswordMethod(oldPassword))
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("旧密码输入错误");
            }
        }

        /// <summary>
        /// 判断登录名是否重复的方法
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public bool CheckLoginNameMethod(string UserID, string LoginName)
        {
            string where = " AND UserID <> @UserID AND LoginName = @LoginName";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@UserID", UserID),
                new SqlParameter("@LoginName", LoginName)
            };
            DataTable dt = UserBLL.UserTableByWhere(where, paras, "");
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
        /// 验证密码是否正确
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <returns></returns>
        public bool CheckPasswordMethod(string Password)
        {
            string where = " AND UserID = @UserID AND Password = @Password";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@UserID", this.UserId),
                new SqlParameter("@Password", Encryption.MD5Encrypt(Password))
            };
            DataTable dt = UserBLL.UserTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult UserSet()
        {
            ViewBag.Title = "权限范围设置";
            return View();
        }

        public ActionResult GetUserRoleList()
        {
            string userId = Request.Form["UserID"];
            string where = "";
            if (!string.IsNullOrEmpty(userId))
            {
                where += " and ur.UserID = " + userId;
            }
            string cmdText = @"SELECT  r.RoleID ,
        ur.UserRoleID ,
        r.Name ,
        r2.RefeName RoleType ,
        r.Description
FROM    T_Sys_Role r
        LEFT JOIN T_Sys_UserRole ur ON r.RoleID = ur.RoleID
        LEFT JOIN T_Sys_Refe r2 ON r.RoleType = r2.Value
                                   AND r2.RefeTypeID = 2
WHERE   r.Status = 1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }


        public ActionResult GetPurviewList()
        {
            string userId = Request.Form["UserID"];
            string roleId = Request.Form["RoleID"];
            string where = "";
            if (!string.IsNullOrEmpty(userId))
            {
                where += " and p.UserID = " + userId;
            }
            if (!string.IsNullOrEmpty(roleId))
            {
                where += " and p.RoleID = " + roleId;
            }
            string cmdText = @"SELECT  d.DeptID ,
        p.PurviewID ,
        d.Name ,
        r.RefeName Range
FROM    T_Sys_Dept d
        LEFT JOIN T_Sys_Purview p ON d.DeptID = p.DeptID
        LEFT JOIN T_Sys_Refe r ON p.Range = r.Value
                                   AND r.RefeTypeID = 4
WHERE   d.Status = 1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        public bool CheckUserRoleMethod(UserRoleModel urm)
        {
            string where = " AND UserID = @UserID AND RoleID = @RoleID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@UserID", urm.UserID),
                new SqlParameter("@RoleID", urm.RoleID)
            };
            DataTable dt = UserRoleBLL.UserRoleTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AjaxResult InsertUserRole(UserRoleModel urm)
        {
            if (CheckUserRoleMethod(urm))
            {
                return AjaxResult.Error("不能重复引入角色");
            }
            else
            {
                if (UserRoleBLL.InsertUserRole(urm) > 0)
                {
                    return AjaxResult.Success();
                }
                else
                {
                    return AjaxResult.Error("发现系统错误");
                }
            }
        }

        public AjaxResult DeleteUserRole()
        {
            string userRoleID = Request.Form["UserRoleID"];

            string result = UserRoleBLL.DeleteUserRole(userRoleID);

            if (result == "yes")
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error(result);
            }
        }

        public string InsertPurview(PurviewModel pm)
        {
            if (CheckPurviewMethod(pm))
            {
                return "不能重复引入数据权限";
            }
            else
            {
                if (PurviewBLL.InsertPurview(pm) > 0)
                {
                    return "yes";
                }
                else
                {
                    return "发现系统错误";
                }
            }
        }

        public AjaxResult CheckPurview()
        {
            string deptId = Request.Form["Value"];
            string roleId = Request.Form["ID"];
            PurviewModel pm = new PurviewModel();
            pm.UserID = UserId.ToString();
            pm.RoleID = roleId;
            pm.DeptID = deptId;
            if (CheckPurviewMethod(pm))
            {
                return AjaxResult.Error("不能重复引入数据权限");
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        public bool CheckPurviewMethod(PurviewModel pm)
        {
            string where = " AND UserID = @UserID AND RoleID = @RoleID AND DeptID = @DeptID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@UserID", pm.UserID),
                new SqlParameter("@RoleID", pm.RoleID),
                new SqlParameter("@DeptID", pm.DeptID)
            };
            DataTable dt = PurviewBLL.PurviewTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AjaxResult DeletePurview()
        {
            string purviewID = Request.Form["PurviewID"];

            string result = PurviewBLL.DeletePurview(purviewID);

            if (result == "yes")
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error(result);
            }
        }
    }
}
