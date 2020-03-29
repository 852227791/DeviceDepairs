using BLL;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class sOrderCreateController : BaseController
    {
        //
        // GET: /sOrderCreate/
        public ActionResult sOrderCreateEdit()
        {
            return View();
        }
        public ActionResult sOrderCreateList()
        {
            return View();
        }
        public ActionResult GetsOrderCreateList() {
            string cmdText= @"SELECT  oc.sOrderCreateID ,
        d.Name
FROM  T_Stu_sOrderCreate oc
        LEFT JOIN T_Sys_Dept d ON d.DeptID = oc.DeptID";
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        public string GetsOrderCreateEdit(sOrderCreateModel ocm) {
            if (string.IsNullOrEmpty(ocm.DeptID))
            {
                return "请选择主体！";
            }
            string where1 = " and DeptID=@DeptID";
            SqlParameter[] paras1 = new SqlParameter[] {
                  new SqlParameter("@DeptID",ocm.DeptID)

                };
            sOrderCreateModel socm = sOrderCreateBLL.SelectsOrderCreateByWhere(where1, paras1, "").FirstOrDefault();
            if (socm!=null)
            {
                return "该主体已经加入，不能重复添加！";
            }

            sOrderCreateBLL.InsertsOrderCreate(ocm);
            return "yes";
        }

        public AjaxResult GetsOrderCreateDel(string ID) {
            if (string.IsNullOrEmpty(ID))
            {
                return AjaxResult.Error("请选择要删除的信息！");
            }
            sOrderCreateBLL.DeletesOrderCreate(ID);
            return AjaxResult.Success("删除成功！");

        }
    }
}
