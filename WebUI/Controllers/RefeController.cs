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
    public class RefeController : BaseController
    {
        //
        // GET: /Refe/

        public ActionResult RefeInfo()
        {
            ViewBag.Title = "基础信息管理";
            return View();
        }

        public ActionResult RefeTypeEdit()
        {
            ViewBag.Title = "基础分类编辑";
            return View();
        }

        public ActionResult RefeEdit()
        {
            ViewBag.Title = "基础明细编辑";
            return View();
        }

        public string SelRefeTypeList()
        {
            string where = " AND Status = 1";
            SqlParameter[] paras = null;
            string queue = " ORDER BY RefeTypeID ASC";
            DataTable dt = RefeTypeBLL.RefeTypeTableByWhere(where, paras, queue);
            return JsonHelper.DataTableToJson(dt);
        }

        public string SelList()
        {
            string refetypeid = Request.QueryString["RefeTypeID"];
            string where = " AND Status = 1 AND RefeTypeID = @RefeTypeID";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@RefeTypeID", refetypeid)
            };
            string queue = " ORDER BY Queue ASC";
            DataTable dt = RefeBLL.RefeTableByWhere(where, paras, queue);
            return JsonHelper.DataTableToJson(dt);
        }

        public ActionResult GetRefeTypeList()
        {
            string cmdText = @"SELECT  rt.RefeTypeID ,
                rt.ModuleName ,
                rt.TypeName ,
                rt.Status StatusValue ,
                r.RefeName Status
        FROM    T_Sys_RefeType rt
                LEFT JOIN T_Sys_Refe r ON rt.Status = r.Value
                                          AND r.RefeTypeID = 1";
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        public string GetRefeTypeEdit(RefeTypeModel rtm, string RefeTypeRemark)
        {
            rtm.Remark = RefeTypeRemark;
            if (string.IsNullOrEmpty(rtm.RefeTypeID))
            {
                rtm.RefeTypeID = "0";
            }

            if (rtm.ModuleName.Length < 1 || rtm.ModuleName.Length > 16)
            {
                return "模块名称不能为空，并且长度为1-16个字符！";
            }
            if (rtm.TypeName.Length < 1 || rtm.TypeName.Length > 16)
            {
                return "分类名称不能为空，并且长度为1-16个字符！";
            }
            if (!string.IsNullOrEmpty(rtm.Remark))
            {
                rtm.Remark = rtm.Remark.Replace("\r\n", "<br />");
            }

            string result = "no";
            if (rtm.RefeTypeID != "0")
            {
                string where = " AND RefeTypeID=@RefeTypeID";
                SqlParameter[] paras = new SqlParameter[] { 
                    new SqlParameter("@RefeTypeID", rtm.RefeTypeID)
                };
                RefeTypeModel editrtm = RefeTypeBLL.RefeTypeModelByWhere(where, paras);

                LogBLL.CreateLog("T_Sys_RefeType", this.UserId.ToString(), editrtm, rtm);//写日志

                editrtm.ModuleName = rtm.ModuleName;
                editrtm.TypeName = rtm.TypeName;
                editrtm.Remark = rtm.Remark;
                editrtm.UpdateID = this.UserId.ToString();
                editrtm.UpdateTime = DateTime.Now.ToString();
                if (RefeTypeBLL.UpdateRefeType(editrtm) > 0)
                {
                    return "yes";
                }
            }
            else
            {
                rtm.Status = "1";
                rtm.Remark = rtm.Remark;
                rtm.CreateID = this.UserId.ToString();
                rtm.CreateTime = DateTime.Now.ToString();
                rtm.UpdateID = this.UserId.ToString();
                rtm.UpdateTime = DateTime.Now.ToString();
                if (RefeTypeBLL.InsertRefeType(rtm) > 0)
                {
                    return "yes";
                }
            }
            return result;
        }

        public AjaxResult SelectRefeType()
        {
            string refeTypeId = Request.Form["ID"];
            string where = " AND RefeTypeID = @RefeTypeID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@RefeTypeID", refeTypeId)
            };
            DataTable dt = RefeTypeBLL.RefeTypeTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        public AjaxResult GetUpdateRefeTypeStatus()
        {
            string refeTypeId = Request.Form["ID"];
            string status = Request.Form["Value"];

            string where = " AND RefeTypeID = @RefeTypeID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@RefeTypeID", refeTypeId)
            };
            RefeTypeModel rtm = RefeTypeBLL.RefeTypeModelByWhere(where, paras);
            rtm.Status = status;
            if (RefeTypeBLL.UpdateRefeType(rtm) > 0)
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }

        public ActionResult GetRefeList()
        {
            string refetypeid = Request.QueryString["RefeTypeID"];
            string cmdText = @"SELECT  r1.RefeID ,
                r1.RefeName ,
                r1.Value ,
                r1.Queue ,
                r1.Status StatusValue ,
                r2.RefeName Status
        FROM    T_Sys_Refe r1
                LEFT JOIN T_Sys_Refe r2 ON r1.Status = r2.Value
                                           AND r2.RefeTypeID = 1
        WHERE   r1.RefeTypeID = {0}";
            cmdText = string.Format(cmdText, refetypeid);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        public string GetRefeEdit(RefeModel rm, string RefeType, string RefeRemark)
        {
            rm.RefeTypeID = RefeType;
            rm.Remark = RefeRemark;
            if (string.IsNullOrEmpty(rm.RefeID))
            {
                rm.RefeID = "0";
            }

            if (rm.RefeName.Length < 1 || rm.RefeName.Length > 16)
            {
                return "名称不能为空，并且长度为1-16个字符！";
            }
            if (string.IsNullOrEmpty(rm.RefeTypeID))
            {
                return "请选择基础信息分类！";
            }
            if (string.IsNullOrEmpty(rm.Queue))
            {
                return "排序不能为空！";
            }
            if (!string.IsNullOrEmpty(rm.Remark))
            {
                rm.Remark = rm.Remark.Replace("\r\n", "<br />");
            }
            string result = "no";
            if (rm.RefeID != "0")
            {
                string where = " AND RefeID=@RefeID";
                SqlParameter[] paras = new SqlParameter[] { 
                    new SqlParameter("@RefeID", rm.RefeID)
                };
                RefeModel editrm = RefeBLL.RefeModelByWhere(where, paras);

                LogBLL.CreateLog("T_Sys_Refe", this.UserId.ToString(), editrm, rm);//写日志

                editrm.RefeName = rm.RefeName;
                editrm.RefeTypeID = rm.RefeTypeID;
                editrm.Value = rm.Value;
                editrm.Queue = rm.Queue;
                editrm.Remark = rm.Remark;
                editrm.UpdateID = this.UserId.ToString();
                editrm.UpdateTime = DateTime.Now.ToString();
                if (RefeBLL.UpdateRefe(editrm) > 0)
                {
                    return "yes";
                }
            }
            else
            {
                rm.Status = "1";
                rm.RefeTypeID = rm.RefeTypeID;
                rm.Remark = rm.Remark;
                rm.CreateID = this.UserId.ToString();
                rm.CreateTime = DateTime.Now.ToString();
                rm.UpdateID = this.UserId.ToString();
                rm.UpdateTime = DateTime.Now.ToString();
                if (RefeBLL.InsertRefe(rm) > 0)
                {
                    return "yes";
                }
            }
            return result;
        }

        public AjaxResult SelectRefe()
        {
            string refeID = Request.Form["ID"];
            string where = " AND RefeID = @RefeID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@RefeID", refeID)
            };
            DataTable dt = RefeBLL.RefeTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        public AjaxResult GetUpdateRefeStatus()
        {
            string refeId = Request.Form["ID"];
            string status = Request.Form["Value"];

            string where = " AND RefeID = @RefeID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@RefeID", refeId)
            };
            RefeModel rm = RefeBLL.RefeModelByWhere(where, paras);
            rm.Status = status;
            if (RefeBLL.UpdateRefe(rm) > 0)
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }

        public AjaxResult CheckRefeTypeName()
        {
            string refeTypeName = Request.Form["Value"];
            string refeTypeId = Request.Form["ID"];

            if (CheckRefeTypeNameMethod(refeTypeId, refeTypeName))
            {
                return AjaxResult.Error("该基础分类名已经存在，请重新输入");
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        /// <summary>
        /// 验证基础分类名是否重复的方法
        /// </summary>
        /// <param name="RefeTypeID"></param>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public bool CheckRefeTypeNameMethod(string RefeTypeID, string TypeName)
        {
            string where = " AND RefeTypeID <> @RefeTypeID AND TypeName = @TypeName";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@RefeTypeID", RefeTypeID),
                new SqlParameter("@TypeName", TypeName)
            };
            DataTable dt = RefeTypeBLL.RefeTypeTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AjaxResult CheckRefeName()
        {
            string refeName = Request.Form["Value"];
            string refeId = Request.Form["ID"];
            string typeId = Request.Form["TypeId"];

            if (CheckRefeNameMethod(refeId, typeId, refeName))
            {
                return AjaxResult.Error("该基础信息名已经存在，请重新输入");
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        /// <summary>
        /// 验证基础信息名是否重复的方法
        /// </summary>
        /// <param name="RefeID"></param>
        /// <param name="RefeName"></param>
        /// <returns></returns>
        public bool CheckRefeNameMethod(string RefeID, string RefeTypeID, string RefeName)
        {
            string where = " AND RefeID <> @RefeID AND RefeTypeID=@RefeTypeID AND RefeName = @RefeName";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@RefeID", RefeID),
                new SqlParameter("@RefeTypeID", RefeTypeID),
                new SqlParameter("@RefeName", RefeName)
            };
            DataTable dt = RefeBLL.RefeTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AjaxResult CheckRefeValue()
        {
            string value = Request.Form["Value"];
            string refeId = Request.Form["ID"];
            string typeId = Request.Form["TypeId"];

            if (CheckRefeValueMethod(refeId, typeId, value))
            {
                return AjaxResult.Error("该基础信息值已经存在，请重新输入");
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        /// <summary>
        /// 验证基础信息值是否重复的方法
        /// </summary>
        /// <param name="RefeID"></param>
        /// <param name="RefeTypeID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool CheckRefeValueMethod(string RefeID, string RefeTypeID, string Value)
        {
            string where = " AND RefeID <> @RefeID AND RefeTypeID=@RefeTypeID AND Value = @Value";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@RefeID", RefeID),
                new SqlParameter("@RefeTypeID", RefeTypeID),
                new SqlParameter("@Value", Value)
            };
            DataTable dt = RefeBLL.RefeTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
