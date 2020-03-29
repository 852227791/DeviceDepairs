using BLL;
using Common;
using DAL;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class sRefundDiscountController : BaseController
    {
        //
        // GET: /sRefundDiscount/

        public ActionResult sRefundDiscountList()
        {
            return View();
        }
        public ActionResult sRefundDiscountUp()
        {
            return View();
        }
        public AjaxResult GetsRefundDiscountData(string ID)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = OtherHelper.ExcelToDataTable(ID);
            }
            catch (Exception ex)
            {

                return AjaxResult.Error(ex.Message);
            }
            try
            {
                if (!dt.Columns[0].ColumnName.Trim().Equals("凭证号") ||
                    !dt.Columns[1].ColumnName.Trim().Equals("收费项目") ||
                    !dt.Columns[2].ColumnName.Trim().Equals("核销类别") ||
                    !dt.Columns[3].ColumnName.Trim().Equals("核销金额") ||
                    !dt.Columns[4].ColumnName.Trim().Equals("核销时间") ||
                    !dt.Columns[5].ColumnName.Trim().Equals("支付对象") ||
                    !dt.Columns[6].ColumnName.Trim().Equals("备注"))
                {
                    return AjaxResult.Error("模板错误");
                }
            }
            catch (Exception ex)
            {

                return AjaxResult.Error(ex.Message);
            }
            return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "success");
        }


        public ActionResult sRefundDiscountEdit()
        {
            return View();
        }

        public AjaxResult UplaodStatus(string id, string value)
        {
            return AjaxResult.Success();
        }
        /// <summary>
        /// 编辑核销
        /// </summary>
        /// <param name="rdm"></param>
        /// <returns></returns>
        public string GetsRefundDiscountEdit(sRefundDiscountModel rdm)
        {

            if (string.IsNullOrEmpty(rdm.sFeesOrderID))
            {
                return "请选择核销项";
            }
            if (!string.IsNullOrEmpty(rdm.Remark))
            {
                rdm.Remark = rdm.Remark.Replace("\r\n", "<br />");
            }
            if (!string.IsNullOrEmpty(rdm.PayObject))
            {
                if (rdm.PayObject.Length > 32)
                {
                    return "支付对象不能超过32个字符";
                }
            }
            decimal canMoney = sRefundDiscountBLL.SelectsCanMoney(rdm.sFeesOrderID);
            if (canMoney < decimal.Parse(rdm.RefundMoney))
            {
                return "可核销金额不足";
            }
            rdm.CreateID = UserId.ToString();
            rdm.CreateTime = DateTime.Now.ToString();
            rdm.Status = "1";
            rdm.UpdateID = UserId.ToString();
            rdm.UpdateTime = DateTime.Now.ToString();
            sRefundDiscountBLL.InsertsRefundDiscount(rdm);
            if (!rdm.Sort.Equals("3"))
            {
                sFeesOrderModel som = sFeesOrderBLL.GetsFeeOrderModel(rdm.sFeesOrderID);
                sOrderBLL.UpdatesOderPaidMoney(-decimal.Parse(rdm.RefundMoney), som.sOrderID, UserId);
                sFeesOrderModel sfom = sFeesOrderBLL.GetsFeeOrderModel(rdm.sFeesOrderID);
                sFeeBLL.UpdatePaidMoney(sfom.sFeeID, -decimal.Parse(rdm.RefundMoney), UserId);
            }
            return "yes";
        }
        /// <summary>
        /// 变更状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public AjaxResult UpdateStatus(string ID, string Value)
        {
            sRefundDiscountModel rm = sRefundDiscountBLL.GetsRefundDiscountModel(ID);
            if (rm.Status.Equals(Value))
                return AjaxResult.Error("变更状态有误");
            if (sRefundDiscountBLL.UpdatesRefundDiscountStatus(ID, Value, UserId.ToString()))
            {
                decimal tempMoney = decimal.Parse(rm.RefundMoney);
                if (Value.Equals("2"))
                {
                    tempMoney = -tempMoney;
                }
                if (!rm.Sort.Equals("3"))
                {
                    sFeesOrderModel som = sFeesOrderBLL.GetsFeeOrderModel(rm.sFeesOrderID);//
                    sOrderBLL.UpdatesOderPaidMoney(-tempMoney, som.sOrderID, UserId);
                    sFeesOrderModel sfom = sFeesOrderBLL.GetsFeeOrderModel(rm.sFeesOrderID);
                    sFeeBLL.UpdatePaidMoney(sfom.sFeeID, -tempMoney, UserId);
                }
            }
            return AjaxResult.Success();
        }



        /// <summary>
        /// 获取学费核销列表信息
        /// </summary>
        /// <param name="MenuID">菜单ID</param>
        /// <returns></returns>
        public ActionResult GetsRefundDiscountList(string MenuID)
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
            string cmdText = @"SELECT  rd.sRefundDiscountID ,
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
FROM     dbo.T_Stu_sRefundDiscount rd
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
        /// 导出学费核销
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public AjaxResult Download(string MenuID)
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
FROM     T_Stu_sRefundDiscount rd
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
            string filename = "../Temp/学费优惠核销信息.xls";
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename, "success");
        }
        /// <summary>
        /// 对象转换
        /// </summary>
        /// <param name="rmch"></param>
        /// <returns></returns>
        private sRefundDiscountUploadModel GetRefundDoscountModel(sRefundDiscountUploadCH rmch)
        {
            sRefundDiscountUploadModel rm = new sRefundDiscountUploadModel();
            rm.money = rmch.核销金额.Replace(" ", "");
            rm.feeOrder = rmch.收费项目.Replace(" ", "");
            rm.payObj = rmch.支付对象.Replace(" ", "");
            rm.refundType = rmch.核销类别.Replace(" ", "");
            rm.remark = rmch.备注.Replace(" ", "");
            rm.time = rmch.核销时间;
            rm.voucherNum = rmch.凭证号.Replace(" ", "");
            return rm;

        }

        public AjaxResult UploadsRefundDiscount()
        {
            string deptId = Request.Form["DeptID"];
            string filePath = Request.Form["filePath"];
            string Paras = Request.Form["Paras"];
            if (string.IsNullOrEmpty(deptId))
            {
                return AjaxResult.Error("校区不能为空");
            }
            if (string.IsNullOrEmpty(filePath))
            {
                return AjaxResult.Error("上传文件不能为空");
            }
            List<sRefundDiscountUploadCH> list = JsonConvert.DeserializeObject<List<sRefundDiscountUploadCH>>(Paras);
            if (list == null || list.Count == 0)
            {
                return AjaxResult.Error("不能导入空数据");
            }
            decimal successNum = 0;
            decimal errorNum = 0;
            string senrollsProfession = string.Empty;
            List<sRefundDiscountUploadCH> edm = new List<sRefundDiscountUploadCH>();
            foreach (var item in list)
            {
                sRefundDiscountUploadModel uem = GetRefundDoscountModel(item);
                string sfeeOrderId = string.Empty;
                string refundType = string.Empty;
                string errorString = validate(uem, ref sfeeOrderId, ref refundType);
                if (string.IsNullOrEmpty(errorString))
                {
                    sRefundDiscountModel rdm = new sRefundDiscountModel();
                    rdm.CreateID = UserId.ToString();
                    rdm.CreateTime = DateTime.Now.ToString();
                    rdm.UpdateTime = DateTime.Now.ToString();
                    rdm.UpdateID = UserId.ToString();
                    rdm.Status = "1";
                    rdm.Remark = uem.remark;
                    rdm.sFeesOrderID = sfeeOrderId;
                    rdm.PayObject = uem.payObj;
                    rdm.RefundMoney = uem.money;
                    rdm.RefundTime = uem.time;
                    rdm.Sort = refundType;
                    sRefundDiscountBLL.InsertsRefundDiscount(rdm);
                    
                    sFeesOrderModel som = sFeesOrderBLL.GetsFeeOrderModel(sfeeOrderId);
                    if (!refundType.Equals("3"))
                    {
                        sOrderBLL.UpdatesOderPaidMoney(-decimal.Parse(uem.money), som.sOrderID, UserId);
                    }
                    sFeeBLL.UpdatePaidMoney(som.sFeeID, -decimal.Parse(uem.money), UserId);
                    successNum += 1;
                }
                else
                {
                    item.系统备注 = errorString;
                    edm.Add(item);
                    errorNum += 1;
                }
            }

            string url = "";
            string errorData = string.Empty;
            if (edm.Count > 0)
            {
                string FileName = OtherHelper.FilePathAndName();
                OtherHelper.DeriveToExcel(edm, FileName);
                url = "../Temp/" + FileName + "";
                errorData = OtherHelper.JsonSerializer(edm);
            }
            NoteModel nm = new NoteModel();
            nm.CreateID = this.UserId.ToString();
            nm.CreateTime = DateTime.Now.ToString();
            nm.InFile = filePath;
            nm.OutFile = url;
            nm.Sort = "16";
            nm.DeptID = deptId;
            nm.Status = "1";
            nm.SuccessNum = successNum.ToString();
            nm.ErrorNum = errorNum.ToString();
            NoteBLL.InsertNote(nm);
            return AjaxResult.Success(errorData, "操作成功");
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
        private string validate(sRefundDiscountUploadModel uem, ref string sfeeOrderId, ref string refundType)
        {
            string errorString = string.Empty;
            sFeeModel fm = GetsFeeModel(uem.voucherNum);
            if (fm.sFeeID == null)
            {
                errorString += "凭证号不存在；";
            }
             sfeeOrderId = sFeesOrderBLL.GetsFeesOrderID(fm.sFeeID, uem.feeOrder);
            if (sfeeOrderId == "")
            {
                errorString += "收费项目不存在；";
            }
             refundType = RefeBLL.GetRefeValue(uem.refundType, "10");
            if (refundType == "-1")
            {
                errorString += "核销类别不存在；";
            }
            if (!OtherHelper.IsDateTime(uem.time))
            {
                errorString += "核销时间必须是时间格式；";
            }
            if (string.IsNullOrEmpty(uem.payObj))
            {
                errorString += "支付对象不能为空；";
            }
            else
            {
                if (uem.payObj.Length>32)
                {
                    errorString += "支付对象不能超过32个字符；";
                }
            }
            if (!OtherHelper.IsDecimal(uem.money))
            {
                errorString += "核销金额必须是数字；";
            }
            else
            {
                if (decimal.Parse(uem.money)<=0)
                {
                    errorString += "核销金额不能小于等于0；";
                }
                decimal tempUseableMoney = decimal.Parse(uem.money);
                var useableMoney = sFeesOrderBLL.GetsFeesOrderCanMoney(sfeeOrderId);
                if (tempUseableMoney > useableMoney)
                {
                    errorString += "可核销金额不足；";
                }
            }

            return errorString;
        }
        
    }
}
