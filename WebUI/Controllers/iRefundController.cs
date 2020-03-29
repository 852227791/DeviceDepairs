using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using BLL;
using Model;
using System.Transactions;
using System.Data.SqlClient;
using DAL;

namespace WebUI.Controllers
{
    public class iRefundController : BaseController
    {
        #region 页面
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult iRefundList()
        {
            return View();
        }

        /// <summary>
        /// 添加、修改页面
        /// </summary>
        /// <returns></returns>
        public ActionResult iRefundEdit()
        {
            return View();
        }

        /// <summary>
        /// 上传页面
        /// </summary>
        /// <returns></returns>
        public ActionResult iRefundUp()
        {
            return View();
        }

        /// <summary>
        /// 选择收费项页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ChooseiFeeList()
        {
            return View();
        }
        #endregion

        #region 获取列表、添加、修改、批量导入添加
        /// <summary>
        /// 得到杂费核销列表信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetiRefundList()
        {
            string menuId = Request.Form["MenuID"];
            string name = Request.Form["txtName"];
            string voucherNum = Request.Form["txtVoucherNum"];
            string idCard = Request.Form["txtIDCard"];
            string selSort = Request.Form["selSort"];
            string noteNum = Request.Form["txtNoteNum"];
            string refundTimeS = Request.Form["txtRefundTimeS"];
            string refundTimeE = Request.Form["txtRefundTimeE"];
            string deptId = Request.Form["txtDeptID"];
            string where = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND ifee.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND student.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " AND ifee.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " AND student.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(selSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selSort, "irefund.Sort");
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " AND ifee.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(refundTimeS))
            {
                where += " AND convert(NVARCHAR(10),irefund.RefundTime,23) >= '" + refundTimeS + "'";
            }
            if (!string.IsNullOrEmpty(refundTimeE))
            {
                where += " AND convert(NVARCHAR(10),irefund.RefundTime,23) <= '" + refundTimeE + "'";
            }
            string cmdText = @"SELECT  irefund.iRefundID ,
        ifee.VoucherNum ,
        ifee.NoteNum ,
        student.Name ,
        student.IDCard ,
        detail.Name AS FeeContent ,
        refe.RefeName AS Sort ,
        irefund.RefundMoney ,
        CONVERT(NVARCHAR(23), irefund.RefundTime, 23) AS RefundTime ,
        irefund.PayObject ,
        sysuser.Name AS CreateName ,
        irefund.Remark ,
        irefund.Status,
		d.Name DeptName
FROM    T_Inc_iRefund AS irefund
        LEFT JOIN T_Inc_iFee AS ifee ON irefund.iFeeID = ifee.iFeeID
        LEFT JOIN T_Pro_Student AS student ON ifee.StudentID = student.StudentID
        LEFT JOIN T_Pro_ItemDetail AS itemdetail ON ifee.ItemDetailID = itemdetail.ItemDetailID
        LEFT JOIN T_Pro_Detail AS detail ON itemdetail.DetailID = detail.DetailID
        LEFT JOIN T_Sys_Refe AS refe ON irefund.Sort = refe.Value
                                        AND refe.RefeTypeID = 10
        LEFT JOIN T_Sys_User AS sysuser ON irefund.CreateID = sysuser.UserID
		LEFT JOIN T_Sys_Dept d ON ifee.DeptID=d.DeptID   WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ifee.DeptID", "irefund.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "RefundMoney"));
        }

        public ActionResult GetViewiRefundList()
        {
            string ifeeId = Request.Form["iFeeID"];
            string where = string.Empty;
            if (!string.IsNullOrEmpty(ifeeId))
            {
                where += " AND ifee.iFeeID= " + ifeeId + "";
            }

            string cmdText = @"SELECT  irefund.iRefundID ,
        ifee.VoucherNum ,
        ifee.NoteNum ,
        student.Name ,
        student.IDCard ,
        detail.Name AS FeeContent ,
        refe.RefeName AS Sort ,
        irefund.RefundMoney ,
        CONVERT(NVARCHAR(23), irefund.RefundTime, 23) AS RefundTime ,
        irefund.PayObject ,
        sysuser.Name AS CreateName ,
        irefund.Remark ,
        irefund.Status,
		d.Name DeptName
FROM    T_Inc_iRefund AS irefund
        LEFT JOIN T_Inc_iFee AS ifee ON irefund.iFeeID = ifee.iFeeID
        LEFT JOIN T_Pro_Student AS student ON ifee.StudentID = student.StudentID
        LEFT JOIN T_Pro_ItemDetail AS itemdetail ON ifee.ItemDetailID = itemdetail.ItemDetailID
        LEFT JOIN T_Pro_Detail AS detail ON itemdetail.DetailID = detail.DetailID
        LEFT JOIN T_Sys_Refe AS refe ON irefund.Sort = refe.Value
                                        AND refe.RefeTypeID = 10
        LEFT JOIN T_Sys_User AS sysuser ON irefund.CreateID = sysuser.UserID
		LEFT JOIN T_Sys_Dept d ON ifee.DeptID=d.DeptID   WHERE   1 = 1 {0}";
            cmdText = string.Format(cmdText, where );
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "RefundMoney"));
        }

        /// <summary>
        /// 添加、修改杂费核销
        /// </summary>
        /// <returns></returns>
        public string GetiRefundEdit(iRefundModel ifm)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                string result = "出现未知错误，请联系管理员";

                #region 后台验证
                string iRefundID = "0";
                string FeeName = Request.Form["FeeName"];
                if (!string.IsNullOrEmpty(ifm.iRefundID))
                {
                    iRefundID = ifm.iRefundID;
                }
                if (string.IsNullOrEmpty(ifm.Sort))
                {
                    return "请选择核销类别";
                }
                if (string.IsNullOrEmpty(FeeName))
                {
                    return "请选择收费项";
                }
                if (string.IsNullOrEmpty(ifm.RefundMoney))
                {
                    return "核销金额不能为空";
                }
                if (!string.IsNullOrEmpty(ifm.RefundMoney) && Convert.ToDecimal(ifm.RefundMoney) == 0)
                {
                    return "核销金额不能为0";
                }
                if (string.IsNullOrEmpty(ifm.RefundTime))
                {
                    return "请选择核销时间";
                }
                if (string.IsNullOrEmpty(ifm.PayObject))
                {
                    return "支付对象不能为空";
                }
                if (!string.IsNullOrEmpty(ifm.PayObject) && ifm.PayObject.Length > 32)
                {
                    return "支付对象不能超过32个字符";
                }
                if (Convert.ToDecimal(ifm.RefundMoney) > iFeeController.GetCanMoney(ifm.iFeeID))
                {
                    return "核销金额不能大于费用项的可核销金额";
                }
                if (!string.IsNullOrEmpty(ifm.Remark))
                {
                    ifm.Remark = ifm.Remark.Replace("\r\n", "<br />");
                }
                #endregion

                try
                {
                    if (iRefundID.Trim() != "0")
                    {
                        #region 修改
                        iRefundModel model = iRefundBLL.iRefundModelByWhere(" AND iRefundID = @iRefundID", new SqlParameter[] { new SqlParameter("@iRefundID", iRefundID) });
                        string tempID = model.iFeeID.Trim();

                        LogBLL.CreateLog("T_Inc_iRefund", this.UserId.ToString(), model, ifm);//写日志

                        model.Sort = ifm.Sort;
                        model.iFeeID = ifm.iFeeID;
                        model.RefundMoney = ifm.RefundMoney;
                        model.RefundTime = ifm.RefundTime;
                        model.PayObject = ifm.PayObject;
                        model.Remark = ifm.Remark;
                        model.UpdateID = this.UserId.ToString();
                        model.UpdateTime = DateTime.Now.ToString();

                        if (iRefundBLL.UpdateiRefund(model) > 0)
                        {
                            //表示修改了核销项
                            if (tempID != ifm.iFeeID.Trim())
                            {
                                //修改费用项的可用金额
                                iFeeController.UpdateCanMoney(tempID);
                            }
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
                        int returnID = iRefundBLL.InsertiRefund(ifm);
                        if (returnID > 0)
                        {
                            result = "yes";
                        }
                        #endregion
                    }

                    //修改费用项的可用金额
                    iFeeController.UpdateCanMoney(ifm.iFeeID);

                    if (result == "yes")
                    {
                        ts.Complete();
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                    }
                }
                catch
                {
                    result = "出现未知错误，请联系管理员";
                    Transaction.Current.Rollback();
                }
                finally
                {
                    ts.Dispose();
                }
                return result;
            }
        }

        /// <summary>
        /// 修改绑定杂费核销
        /// </summary>
        /// <returns></returns>
        public AjaxResult SelectiRefund()
        {
            string iRefundId = Request.Form["ID"];
            DataTable dt = iRefundBLL.SelectiRefund(iRefundId);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        /// <summary>
        /// 批量导入添加
        /// </summary>
        /// <param name="ifm"></param>
        /// <returns></returns>
        public AjaxResult UpLoadiRefund()
        {
            try
            {
                #region 后台验证
                string deptId = Request.Form["DeptID"];
                string filePath = Request.Form["filePath"];
                if (string.IsNullOrEmpty(deptId))
                {
                    return AjaxResult.Error("请选择校区");
                }
                if (string.IsNullOrEmpty(filePath))
                {
                    return AjaxResult.Error("请选择文件");
                }
                #endregion

                #region 处理excel数据
                DataTable oldDt = new DataTable();
                try
                {
                    oldDt = OtherHelper.UploadFile(filePath).Tables[0];
                }
                catch (Exception)
                {
                    return AjaxResult.Error("模板异常，请联系管理员");
                }
                DataTable myDt = new DataTable();
                if (oldDt != null || oldDt.Rows.Count > 0)
                {
                    int errorNum = 0;
                    int successNum = 0;
                    string url = string.Empty;
                    string[] array = new string[6];
                    for (int i = 0; i < oldDt.Rows.Count; i++)
                    {
                        try
                        {
                            array[0] = oldDt.Rows[i][0].ToString().Trim();//凭证号
                            array[1] = oldDt.Rows[i][1].ToString().Trim();//核销类别
                            array[2] = oldDt.Rows[i][2].ToString().Trim();//核销金额
                            array[3] = oldDt.Rows[i][3].ToString().Trim();//核销时间
                            array[4] = oldDt.Rows[i][4].ToString().Trim();//支付对象
                            array[5] = oldDt.Rows[i][5].ToString().Trim();//备注
                        }
                        catch
                        {
                            return AjaxResult.Error("模板错误");
                        }
                        if (i == 0)
                        {
                            if (array[0] != "凭证号" || array[1] != "核销类别" || array[2] != "核销金额" || array[3] != "核销时间" || array[4] != "支付对象" || array[5] != "备注")
                            {
                                return AjaxResult.Error("模板错误");
                            }
                            else
                            {
                                myDt = TableTitle(myDt, array);
                            }
                        }
                        else
                        {
                            string errorResult = ValidateExcelWord(array);
                            //格式错误，写记录表
                            if (errorResult.Trim() != "")
                            {
                                myDt = TableRows(myDt, array, errorResult);
                                errorNum++;
                            }
                            //格式正确，添加核销
                            else
                            {
                                #region 添加
                                string sort = RefeBLL.GetRefeValue(array[1], "10");
                                iFeeModel ifeeModel = iFeeBLL.iFeeModelByWhere(" AND VoucherNum=@VoucherNum", new SqlParameter[] { new SqlParameter("@VoucherNum", array[0]) });
                                iRefundModel model = new iRefundModel();
                                model.Status = "1";
                                model.iFeeID = ifeeModel.iFeeID;
                                model.Sort = sort;
                                model.RefundMoney = array[2];
                                model.RefundTime = array[3];
                                model.PayObject = array[4];
                                model.Remark = array[5];
                                model.CreateID = this.UserId.ToString();
                                model.CreateTime = DateTime.Now.ToString();
                                model.UpdateID = this.UserId.ToString();
                                model.UpdateTime = DateTime.Now.ToString();
                                iRefundBLL.InsertiRefund(model);
                                successNum++;
                                #endregion

                                //修改费用项的可用金额
                                iFeeController.UpdateCanMoney(ifeeModel.iFeeID);
                            }
                        }
                    }
                    //保存错误文件
                    if (myDt.Rows.Count > 0)
                    {
                        string FileName = OtherHelper.FilePathAndName();
                        OtherHelper.DeriveToExcel(myDt, FileName);
                        url = "../Temp/" + FileName + "";
                    }
                    NoteModel nm = new NoteModel();
                    nm.CreateID = this.UserId.ToString();
                    nm.CreateTime = DateTime.Now.ToString();
                    nm.InFile = filePath;
                    nm.OutFile = url;
                    nm.Sort = "13";
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
                    return AjaxResult.Error("操作失败，导入数据不能为空");
                }
                #endregion
            }
            catch
            {
                return AjaxResult.Error("未知错误，请联系管理员");
            }
        }
        #endregion

        #region 生成导出Excel列
        /// <summary>
        /// 生成导出Excel列
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="array"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public DataTable TableRows(DataTable dt, string[] array, string remark)
        {
            DataRow dr = dt.NewRow();
            dr["凭证号"] = array[0];
            dr["核销类别"] = array[1];
            dr["核销金额"] = array[2];
            dr["核销时间"] = array[3];
            dr["支付对象"] = array[4];
            dr["备注"] = array[5];
            dr["系统备注"] = remark;
            dt.Rows.Add(dr);
            return dt;
        }
        #endregion

        #region   生成Excel表头
        /// <summary>
        /// 生成Excel表头
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
        #endregion

        #region 验证Excel数据
        private static string ValidateExcelWord(string[] array)
        {
            string error = string.Empty;
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(array[0]))
            {
                error += "【凭证号不能为空】";
            }
            else if (!string.IsNullOrEmpty(array[0]))
            {
                dt = iFeeBLL.iFeeTableByWhere(" AND VoucherNum=@VoucherNum", new SqlParameter[] { new SqlParameter("@VoucherNum", array[0]) }, "");
                if (dt.Rows.Count <= 0)
                {
                    error += "【凭证号不存在】";
                }
            }

            if (string.IsNullOrEmpty(array[1]))
            {
                error += "【核销类别不能为空】";
            }
            else if (!string.IsNullOrEmpty(array[1]))
            {
                string sort = RefeBLL.GetRefeValue(array[1], "10");
                if (sort == "-1")
                {
                    error += "【核销类别不存在】";
                }
            }

            if (string.IsNullOrEmpty(array[2]))
            {
                error += "【核销金额不能为空】";
            }
            else if (!string.IsNullOrEmpty(array[2]))
            {
                if (!OtherHelper.IsDecimal(array[2]))
                {
                    error += "【核销金额必须是数字】";
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        //核销金额
                        decimal iRefundMoney = Convert.ToDecimal(array[2]);
                        if (iRefundMoney > Convert.ToDecimal(dt.Rows[0]["CanMoney"]))
                        {
                            error += "【核销金额不能大于可核销金额】";
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(array[3]))
            {
                error += "【核销时间不能为空】";
            }
            else if (!string.IsNullOrEmpty(array[3]))
            {
                if (!OtherHelper.IsDateTime(array[3]))
                {
                    error += "【核销时间必须是时间格式】";
                }
            }

            if (string.IsNullOrEmpty(array[4]))
            {
                error += "【支付对象不能为空】";
            }
            else if (!string.IsNullOrEmpty(array[4]))
            {
                if (array[4].Length > 32)
                {
                    error += "【支付对象不能超过32个字符】";
                }
            }

            return error;
        }
        #endregion

        #region 修改状态（启用、停用）
        public AjaxResult UpdateiRefundStatus()
        {
            try
            {
                string iRefundId = Request.Form["ID"];
                string status = Request.Form["Value"];
                iRefundModel fm = iRefundBLL.iRefundModelByWhere(" AND iRefundID=@iRefundID", new SqlParameter[] { new SqlParameter("@iRefundID", iRefundId) });
                fm.Status = status;
                fm.UpdateID = this.UserId.ToString();
                fm.UpdateTime = DateTime.Now.ToString();
                iRefundBLL.UpdateiRefund(fm);

                //修改费用项的可用金额
                iFeeController.UpdateCanMoney(fm.iFeeID);

                return AjaxResult.Success();
            }
            catch
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
        }
        #endregion
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public AjaxResult Download()
        {
            string menuId = Request.Form["MenuID"];
            string name = Request.Form["txtName"];
            string voucherNum = Request.Form["txtVoucherNum"];
            string idCard = Request.Form["txtIDCard"];
            string selSort = Request.Form["selSort"];
            string noteNum = Request.Form["txtNoteNum"];
            string refundTimeS = Request.Form["txtRefundTimeS"];
            string refundTimeE = Request.Form["txtRefundTimeE"];
            string deptId = Request.Form["txtDeptID"];
            string where = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND ifee.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND student.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " AND ifee.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " AND student.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(selSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selSort, "irefund.Sort");
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " AND ifee.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(refundTimeS))
            {
                where += " AND convert(NVARCHAR(10),irefund.RefundTime,23) >= '" + refundTimeS + "'";
            }
            if (!string.IsNullOrEmpty(refundTimeE))
            {
                where += " AND convert(NVARCHAR(10),irefund.RefundTime,23) <= '" + refundTimeE + "'";
            }
            string cmdText = @"SELECT  
        d.Name 收费单位,
        ifee.VoucherNum 凭证号,
        ifee.NoteNum 票据号,
        student.Name 姓名,
        student.IDCard 身份证号,
        detail.Name AS 收费项 ,
        refe.RefeName AS 核销类别 ,
        irefund.RefundMoney 核销金额 ,
        CONVERT(NVARCHAR(23), irefund.RefundTime, 23) AS 核销时间,
        irefund.PayObject 支付对象 ,
        sysuser.Name AS 核销员 ,
        irefund.Remark  核销备注
FROM    T_Inc_iRefund AS irefund
        LEFT JOIN T_Inc_iFee AS ifee ON irefund.iFeeID = ifee.iFeeID
        LEFT JOIN T_Pro_Student AS student ON ifee.StudentID = student.StudentID
        LEFT JOIN T_Pro_ItemDetail AS itemdetail ON ifee.ItemDetailID = itemdetail.ItemDetailID
        LEFT JOIN T_Pro_Detail AS detail ON itemdetail.DetailID = detail.DetailID
        LEFT JOIN T_Sys_Refe AS refe ON irefund.Sort = refe.Value
                                        AND refe.RefeTypeID = 10
        LEFT JOIN T_Sys_User AS sysuser ON irefund.CreateID = sysuser.UserID 
        LEFT JOIN T_Sys_Dept d ON ifee.DeptID=d.DeptID  WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ifee.DeptID", "irefund.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            string filename = "../Temp/杂费费用核销信息.xls";
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename, "success");
        }

    }
}
