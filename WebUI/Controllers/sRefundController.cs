using BLL;
using Common;
using DAL;
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
    public class sRefundController : BaseController
    {
        //
        // GET: /sRedund/

        public ActionResult sRefundUp()
        {
            return View();
        }
        public ActionResult sRefundEdit()
        {
            return View();
        }
        public ActionResult sRefundList()
        {
            return View();
        }
        /// <summary>
        /// 获取学费核销列表信息
        /// </summary>
        /// <param name="MenuID">菜单ID</param>
        /// <returns></returns>
        public ActionResult GetsRefundList(string MenuID)
        {
            string studentName = Request.Form["studentName"];
            string voucherNum = Request.Form["voucherNum"];
            string idCard = Request.Form["idCard"];
            string selsort = Request.Form["selsort"];
            string noteNum = Request.Form["noteNum"];
            string refundTimeS = Request.Form["refundTimeS"];
            string refundTimeE = Request.Form["refundTimeE"];
            string deptId = Request.Form["txtDeptID"];
            string where = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND f.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(studentName))
            {
                where += " and s.Name like '%" + studentName + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and f.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(selsort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selsort, "rd.Sort");
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " and f.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(refundTimeS))
            {
                where += " and convert(nvarchar(10),rd.RefundTime,23) >= '" + refundTimeS + "'";
            }
            if (!string.IsNullOrEmpty(refundTimeE))
            {
                where += " and convert(nvarchar(10),rd.RefundTime,23) <= '" + refundTimeE + "'";
            }
            string cmdText = @"SELECT  rd.sRefundID ,
        f.VoucherNum ,
        f.NoteNum ,
        s.Name ,
        s.IDCard ,
        so.PlanName + ' ' + so.NumName Content ,
        d.Name DetailName ,
        r1.RefeName Sort ,
        rd.RefundMoney ,
        CONVERT(NVARCHAR(10), rd.RefundTime, 23) RefundTime ,
        rd.PayObject ,
        u.Name Creater ,
        rd.Remark,
		r2.RefeName Status,
		rd.Status StatusValue,
        d1.Name DeptName
FROM     T_Stu_sRefund rd
        LEFT JOIN  T_Stu_sFeesOrder fo ON fo.sFeesOrderID = rd.sFeesOrderID
        LEFT JOIN  T_Stu_sFee f ON f.sFeeID = fo.sFeeID
        LEFT JOIN  T_Stu_sOrder so ON so.sOrderID = fo.sOrderID
        LEFT JOIN  T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = so.sEnrollsProfessionID
        LEFT JOIN  T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN  T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN  T_Pro_Detail d ON d.DetailID = so.DetailID
        LEFT JOIN  T_Sys_Refe r1 ON r1.Value = rd.Sort
                                       AND r1.RefeTypeID = 10
        LEFT JOIN  T_Sys_User u ON u.UserID = rd.CreateID
		LEFT JOIN  T_Sys_Refe r2 ON r2.Value=rd.Status  AND r2.RefeTypeID=1
        LEFT JOIN T_Sys_Dept d1 ON f.DeptID=d1.DeptID 
        Where 1=1 {0}";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "f.DeptID", "rd.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "RefundMoney"));
        }
        /// <summary>
        /// 编辑学费核销信息
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        public string GetsRefundEdit(sRefundModel rm)
        {
            if (string.IsNullOrEmpty(rm.sRefundID))
                rm.sRefundID = "0";

            if (!string.IsNullOrEmpty(rm.Remark))
                rm.Remark = rm.Remark.Replace("\r\n", "<br />");
            else
                rm.Remark = "";


            if (string.IsNullOrEmpty(rm.sFeesOrderID))
                return "收费项不能为空";
            if (string.IsNullOrEmpty(rm.RefundMoney))
                return "核销金额不能为空";
            if (string.IsNullOrEmpty(rm.Sort))
                return "核销类别不能为空";
            if (string.IsNullOrEmpty(rm.PayObject))
                return "支付对象不能为空";

            decimal refundMoney = sRefundBLL.GetRefundMoney(rm.sFeesOrderID, rm.sRefundID);//已核销金额
            decimal tempMoney = sFeesOrderBLL.GetsFeesOrderCanMoney(rm.sFeesOrderID);//可核销金额


            bool flag = false;
            if (rm.sRefundID.Equals("0"))
            {
                if (decimal.Parse(rm.RefundMoney) > tempMoney)
                    return "可核销金额不足";


                rm.CreateID = UserId.ToString();
                rm.CreateTime = DateTime.Now.ToString();
                rm.UpdateID = UserId.ToString();
                rm.UpdateTime = DateTime.Now.ToString();
                rm.Status = "1";
                if (sRefundBLL.InsertsRefund(rm) > 0)
                {
                    flag = true;
                    sFeesOrderBLL.UpdatasFeesOrderCanMoney(decimal.Parse(rm.RefundMoney), rm.sFeesOrderID, UserId);
                    if (!rm.Sort.Equals("3"))
                    {
                        sFeesOrderModel som = sFeesOrderBLL.GetsFeeOrderModel(rm.sFeesOrderID);
                        sOrderBLL.UpdatesOderPaidMoney(-decimal.Parse(rm.RefundMoney), som.sOrderID, UserId);
                        sFeesOrderModel sfom = sFeesOrderBLL.GetsFeeOrderModel(rm.sFeesOrderID);
                        sFeeBLL.UpdatePaidMoney(sfom.sFeeID, -decimal.Parse(rm.RefundMoney), UserId);
                    }
                }
            }
            else
            {
                //sRefundModel editrm = GetRefundModel(rm.sRefundID);


                //LogBLL.CreateLog("T_Stu_sRefund", this.UserId.ToString(), editrm, rm);//写日志
                //decimal temprefundMoney = decimal.Parse(editrm.RefundMoney);
                //if (tempMoney + temprefundMoney - decimal.Parse(rm.RefundMoney) < 0)
                //{
                //    return "可核销金额不足";
                //}
                //editrm.sFeesOrderID = rm.sFeesOrderID;
                //editrm.PayObject = rm.PayObject;
                //editrm.RefundMoney = rm.RefundMoney;
                //editrm.RefundTime = rm.RefundTime;
                //editrm.Remark = rm.Remark;
                //editrm.Sort = rm.Sort;
                //editrm.UpdateID = this.UserId.ToString();
                //editrm.UpdateTime = DateTime.Now.ToString();
                //if (sRefundBLL.UpdatesRefund(editrm) > 0)
                //{
                //    flag = true;
                //    sFeesOrderBLL.UpdatasFeesOrderCanMoney(decimal.Parse(rm.RefundMoney) + temprefundMoney, rm.sFeesOrderID, UserId);
                //    sFeesOrderModel som = sFeesOrderBLL.GetsFeeOrderModel(rm.sFeesOrderID);
                //    if (!rm.Sort.Equals("3"))
                //    {
                //        sOrderBLL.UpdatesOderPaidMoney(-tempMoney + temprefundMoney, som.sOrderID, UserId);
                //    }
                //    sFeeBLL.UpdatePaidMoney(som.sFeeID, -tempMoney + temprefundMoney, UserId);
                //}
            }

            if (flag)
            {
                try
                {

                    return "yes";
                }
                catch (Exception)
                {
                    return "系统错误，请联系管理员";
                    throw;
                }


            }
            else
                return "出现未知错误，请联系管理员";
        }
        /// <summary>
        /// 获取学费核销实体
        /// </summary>
        /// <param name="srefundId"></param>
        /// <returns></returns>
        private sRefundModel GetRefundModel(string srefundId)
        {
            string where = " and  sRefundID=@sRefundID";
            SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@sRefundID",srefundId)
                };
            return sRefundBLL.sRefundModelByWhere(where, paras);
        }
        /// <summary>
        /// 修改学费核销信息状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public AjaxResult UpdateStatus(string ID, string Value)
        {
            sRefundModel rm = GetRefundModel(ID);
            if (rm.Status.Equals(Value))
                return AjaxResult.Error("变更状态有误");
            rm.Status = Value;
            rm.UpdateID = this.UserId.ToString();
            rm.UpdateTime = DateTime.Now.ToString();
            if (sRefundBLL.UpdatesRefund(rm) > 0)
            {
                try
                {
                    decimal tempMoney = decimal.Parse(rm.RefundMoney);
                    if (Value.Equals("2"))
                    {
                        tempMoney = -tempMoney;
                    }

                    sFeesOrderBLL.UpdatasFeesOrderCanMoney(tempMoney, rm.sFeesOrderID, UserId);//修改收费项可用金额
                    if (!rm.Sort.Equals("3"))
                    {
                        sFeesOrderModel som = sFeesOrderBLL.GetsFeeOrderModel(rm.sFeesOrderID);//
                        sOrderBLL.UpdatesOderPaidMoney(-tempMoney, som.sOrderID, UserId);
                        sFeesOrderModel sfom = sFeesOrderBLL.GetsFeeOrderModel(rm.sFeesOrderID);
                        sFeeBLL.UpdatePaidMoney(sfom.sFeeID,-tempMoney, UserId);

                    }
                    return AjaxResult.Success();
                }
                catch (Exception)
                {
                    return AjaxResult.Error("系统错误，请联系管理员");
                }

            }
            else
                return AjaxResult.Error();
        }
        /// <summary>
        /// 修改时表单赋值
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AjaxResult SelectsRefund(string ID)
        {
            string where = " and  sRefundID=@sRefundID";
            SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@sRefundID",ID)
                };
            return AjaxResult.Success(JsonHelper.DataTableToJson(sRefundBLL.sRefundTableByWhere(where, paras, "")), "");
        }

        public AjaxResult UpLoadsRefund()
        {
            string filePath = Request.Form["filePath"];
            string deptId = Request.Form["DeptID"];
            if (string.IsNullOrEmpty(filePath))
            {
                return AjaxResult.Error("请上传文件");
            }

            DataSet ds = new DataSet();
            try
            {
                ds = OtherHelper.UploadFile(filePath);
            }
            catch (Exception)
            {
                return AjaxResult.Error("模板异常，请联系管理员");
            }
            DataTable dt = new DataTable();
            if (ds != null || ds.Tables[0].Rows.Count > 0)
            {
                int errorNum = 0;
                int successNum = 0;
                string[] array = new string[7];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string errorString = "";
                    try
                    {
                        array[0] = ds.Tables[0].Rows[i][0].ToString().Trim();//凭证号
                        array[1] = ds.Tables[0].Rows[i][1].ToString().Trim();//收费项目
                        array[2] = ds.Tables[0].Rows[i][2].ToString().Trim();//核销类别
                        array[3] = ds.Tables[0].Rows[i][3].ToString().Trim();//核销金额
                        array[4] = ds.Tables[0].Rows[i][4].ToString().Trim();//核销时间
                        array[5] = ds.Tables[0].Rows[i][5].ToString().Trim();//支付对象
                        array[6] = ds.Tables[0].Rows[i][6].ToString().Trim();//备注
                    }
                    catch (Exception)
                    {
                        return AjaxResult.Error("模板错误,请选用下载的模板上传数据1");
                    }

                    if (i == 0)
                    {
                        if (array[0] != "凭证号" || array[1] != "收费项目" || array[2] != "核销类别" || array[3] != "核销金额" || array[4] != "核销时间" || array[5] != "支付对象" || array[6] != "备注")
                        {
                            return AjaxResult.Error("模板错误,请选用下载的模板上传数据");
                        }
                        dt = TableTitle(dt, array);
                    }
                    else
                    {
                        sFeeModel fm = GetsFeeModel(array[0]);
                        if (fm.sFeeID == null)
                        {
                            errorString += "凭证号不存在；";
                        }
                        string sfeeOrderId = sFeesOrderBLL.GetsFeesOrderID(fm.sFeeID, array[1]);
                        if (sfeeOrderId == "")
                        {
                            errorString += "收费项目不存在；";
                        }
                        string sort = RefeBLL.GetRefeValue(array[2], "10");
                        if (sort == "-1")
                        {
                            errorString += "核销类别不存在；";
                        }
                        if (!OtherHelper.IsDateTime(array[4]))
                        {
                            errorString += "核销时间必须是时间格式；";
                        }
                        if (string.IsNullOrEmpty(array[5]))
                        {
                            errorString += "支付对象不能为空；";
                        }
                        if (!OtherHelper.IsDecimal(array[3]))
                        {
                            errorString += "核销金额必须是数字；";
                        }
                        if (array[3] != "")
                        {

                            decimal tempUseableMoney = decimal.Parse(array[3]);
                            var useableMoney = sFeesOrderBLL.GetsFeesOrderCanMoney(sfeeOrderId);
                            if (tempUseableMoney > useableMoney)
                            {
                                errorString += "可核销金额不足；";
                            }
                        }
                        if (string.IsNullOrEmpty(errorString))
                        {
                            InsertsFeeRefund(sfeeOrderId, array, sort);
                            sFeesOrderBLL.UpdatasFeesOrderCanMoney(decimal.Parse(array[3]), sfeeOrderId, UserId);
                            sFeesOrderModel som = sFeesOrderBLL.GetsFeeOrderModel(sfeeOrderId);
                            if (!sort.Equals("3"))
                            {
                                sOrderBLL.UpdatesOderPaidMoney(-decimal.Parse(array[3]), som.sOrderID, UserId);
                            }
                            sFeeBLL.UpdatePaidMoney(som.sFeeID, -decimal.Parse(array[3]), UserId);
                            successNum++;
                        }
                        else
                        {
                            dt = TableRows(dt, array, errorString);
                            errorNum++;
                        }
                    }
                }
                string url = "";
                if (dt.Rows.Count > 0)
                {
                    string FileName = OtherHelper.FilePathAndName();
                    OtherHelper.DeriveToExcel(dt, FileName);
                    url = "../Temp/" + FileName + "";
                }
                NoteModel nm = new NoteModel();
                nm.CreateID = this.UserId.ToString();
                nm.CreateTime = DateTime.Now.ToString();
                nm.InFile = filePath;
                nm.OutFile = url;
                nm.Sort = "6";
                nm.DeptID = deptId;
                nm.Status = "1";
                nm.SuccessNum = successNum.ToString();
                nm.ErrorNum = errorNum.ToString();
                NoteBLL.InsertNote(nm);
                string susscee = "成功导入" + successNum.ToString() + "条，错误数据" + errorNum.ToString() + "条。";
                string json = "{\"Tip\":\"操作成功\",\"Mesg\":\"" + susscee + "\",\"Url\":\"" + url + "\"}";
                return AjaxResult.Success(json);
            }
            else
            {
                return AjaxResult.Error("操作失败,数据不能为空");
            }
        }
        /// <summary>
        /// 生成错误文件dt表头
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public DataTable TableTitle(DataTable dt, string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                dt.Columns.Add(array[i], Type.GetType("System.String"));
            }
            dt.Columns.Add("系统备注", Type.GetType("System.String"));
            return dt;
        }
        /// <summary>
        /// 在错误文件添加列
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="array"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public DataTable TableRows(DataTable dt, string[] array, string remark)
        {
            DataRow dr = dt.NewRow();
            dr["凭证号"] = array[0];
            dr["收费项目"] = array[1];
            dr["核销类别"] = array[2];
            dr["核销金额"] = array[3];
            dr["核销时间"] = array[4];
            dr["支付对象"] = array[5];
            dr["备注"] = array[6];
            dr["系统备注"] = remark;
            dt.Rows.Add(dr);
            return dt;
        }

        public void InsertsFeeRefund(string feeorderId, string[] array, string sort)
        {

            sRefundModel rm = new sRefundModel();
            rm.CreateTime = DateTime.Now.ToString();
            rm.Remark = array[6];
            rm.PayObject = array[5];
            rm.RefundTime = array[4];
            rm.RefundMoney = array[3];
            rm.sFeesOrderID = feeorderId;
            rm.Sort = sort;
            rm.Status = "1";
            rm.UpdateTime = DateTime.Now.ToString();
            rm.CreateID = this.UserId.ToString();
            rm.UpdateID = this.UserId.ToString();
            sRefundBLL.InsertsRefund(rm);

        }
        /// <summary>
        /// 根据凭证号获取收费实体
        /// </summary>
        /// <param name="voucherNum"></param>
        /// <returns></returns>
        private sFeeModel GetsFeeModel(string voucherNum)
        {
            string where = " and VoucherNum=@VoucherNum";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@VoucherNum",voucherNum)
            };
            return sFeeBLL.sFeeModelByWhere(where, paras);
        }

        /// <summary>
        /// 根据收费信息id和收费类别返回收费明细
        /// </summary>
        /// <param name="sfeeId"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        private sFeesOrderModel GetsFeesOrderModel(string sfeeId, string name)
        {
            string where = @"and sFeeID=@sFeeID  and Status=1  and sOrderID=(SELECT o.sOrderID FROM T_Stu_sOrder o 
                              LEFT JOIN T_Pro_Detail d ON d.DetailID=o.DetailID Where d.Name=@Name and o.sOrderID=)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@sFeeID",sfeeId),
                new SqlParameter("@Name",name)
            };
            return sFeesOrderBLL.sFeesOrderModelByWhere(where, paras);
        }
        /// <summary>
        /// 导出学费核销
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public AjaxResult Download(string MenuID) {
            string studentName = Request.Form["studentName"];
            string voucherNum = Request.Form["voucherNum"];
            string idCard = Request.Form["idCard"];
            string selsort = Request.Form["selsort"];
            string noteNum = Request.Form["noteNum"];
            string refundTimeS = Request.Form["refundTimeS"];
            string refundTimeE = Request.Form["refundTimeE"];
            string deptId = Request.Form["txtDeptID"];

            string where = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND f.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(studentName))
            {
                where += " and s.Name like '%" + studentName + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and f.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(selsort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selsort, "rd.Sort");
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " and f.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(refundTimeS))
            {
                where += " and convert(nvarchar(10),rd.RefundTime,23) >= '" + refundTimeS + "'";
            }
            if (!string.IsNullOrEmpty(refundTimeE))
            {
                where += " and convert(nvarchar(10),rd.RefundTime,23) <= '" + refundTimeE + "'";
            }
            string cmdText = @"SELECT  
        d1.Name 校区,
        f.VoucherNum 凭证号,
        f.NoteNum 票据号 ,
        s.Name 姓名,
        s.IDCard 身份证号,
        so.PlanName + ' ' + so.NumName 缴费方案 ,
        d.Name  收费项,
        r1.RefeName  核销类别 ,
        rd.RefundMoney  核销金额,
        CONVERT(NVARCHAR(10), rd.RefundTime, 23) 核销时间 ,
        rd.PayObject 支付对象 ,
        u.Name 核销员  ,
        rd.Remark 核销备注,
		r2.RefeName 状态
FROM     T_Stu_sRefund rd
        LEFT JOIN  T_Stu_sFeesOrder fo ON fo.sFeesOrderID = rd.sFeesOrderID
        LEFT JOIN  T_Stu_sFee f ON f.sFeeID = fo.sFeeID
        LEFT JOIN  T_Stu_sOrder so ON so.sOrderID = fo.sOrderID
        LEFT JOIN  T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = so.sEnrollsProfessionID
        LEFT JOIN  T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN  T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN  T_Pro_Detail d ON d.DetailID = so.DetailID
        LEFT JOIN  T_Sys_Refe r1 ON r1.Value = rd.Sort
                                       AND r1.RefeTypeID = 10
        LEFT JOIN  T_Sys_User u ON u.UserID = rd.CreateID
		LEFT JOIN  T_Sys_Refe r2 ON r2.Value=rd.Status  AND r2.RefeTypeID=1
        LEFT JOIN T_Sys_Dept d1 ON f.DeptID=d1.DeptID 
        Where 1=1 {0}";
            string powerStr = PurviewToList(MenuID);
            powerStr = string.Format(powerStr, "f.DeptID", "rd.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            string filename = "../Temp/证书费用核销信息.xls";
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename, "success");
        }
        public ActionResult GetViewsRefundList()
        {
            string sfeeId = Request.Form["sFeeID"];
            string where = "";
            if (!string.IsNullOrEmpty(sfeeId))
            {
                where += "and rd.sFeesOrderID IN (SELECT sFeesOrderID FROM  T_Stu_sFeesOrder WHERE sFeeID=" + sfeeId + ") and rd.Status=1";
            }
            string cmdText = @"SELECT  rd.sRefundID ,
        f.VoucherNum ,
        f.NoteNum ,
        s.Name ,
        s.IDCard ,
        so.PlanName + ' ' + so.NumName Content ,
        d.Name DetailName ,
        r1.RefeName Sort ,
        rd.RefundMoney ,
        CONVERT(NVARCHAR(10), rd.RefundTime, 23) RefundTime ,
        rd.PayObject ,
        u.Name Creater ,
        rd.Remark,
		r2.RefeName Status,
		rd.Status StatusValue,
        d1.Name DeptName
FROM     T_Stu_sRefund rd
        LEFT JOIN  T_Stu_sFeesOrder fo ON fo.sFeesOrderID = rd.sFeesOrderID
        LEFT JOIN  T_Stu_sFee f ON f.sFeeID = fo.sFeeID
        LEFT JOIN  T_Stu_sOrder so ON so.sOrderID = fo.sOrderID
        LEFT JOIN  T_Stu_sEnrollsProfession ep ON ep.sEnrollsProfessionID = so.sEnrollsProfessionID
        LEFT JOIN  T_Stu_sEnroll e ON e.sEnrollID = ep.sEnrollID
        LEFT JOIN  T_Pro_Student s ON s.StudentID = e.StudentID
        LEFT JOIN  T_Pro_Detail d ON d.DetailID = so.DetailID
        LEFT JOIN  T_Sys_Refe r1 ON r1.Value = rd.Sort
                                       AND r1.RefeTypeID = 10
        LEFT JOIN  T_Sys_User u ON u.UserID = rd.CreateID
		LEFT JOIN  T_Sys_Refe r2 ON r2.Value=rd.Status  AND r2.RefeTypeID=1
        LEFT JOIN T_Sys_Dept d1 ON f.DeptID=d1.DeptID 
        Where 1=1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "RefundMoney"));
        }
    }
}
