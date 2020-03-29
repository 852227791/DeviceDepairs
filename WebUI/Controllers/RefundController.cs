using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using BLL;
using DAL;

namespace WebUI.Controllers
{
    public class RefundController : BaseController
    {
        //
        // GET: /Refund/

        #region 页面加载
        public ActionResult RefundList()
        {
            return View();
        }

        public ActionResult RefundUpload()
        {
            return View();
        }
        public ActionResult RefundEdit()
        {
            return View();
        }
        public ActionResult RefundView()
        {
            return View();
        }
        #endregion

        #region 核销列表
        public ActionResult GetRefundList()
        {
            string menuId = Request.Form["MenuID"];
            string studentName = Request.Form["studentName"];
            string voucherNum = Request.Form["voucherNum"];
            string idCard = Request.Form["idCard"];
            string selStatus = Request.Form["selStatus"];
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
            if (!string.IsNullOrEmpty(selStatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selStatus, "r.Status");
            }
            if (!string.IsNullOrEmpty(selsort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selsort, "r.Sort");
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " and f.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(refundTimeS))
            {
                where += " and convert(nvarchar(10),r.RefundTime,23) >= '" + refundTimeS + "'";
            }
            if (!string.IsNullOrEmpty(refundTimeE))
            {
                where += " and convert(nvarchar(10),r.RefundTime,23) <= '" + refundTimeE + "'";
            }
            string cmdText = @"SELECT  fd.FeeDetailID ,
        r.RefundID ,
        f.VoucherNum ,
        s.Name ,
        i.Name ItemName ,
        ( SELECT    dl.Name
          FROM      T_Pro_Detail dl
                    LEFT JOIN T_Pro_ItemDetail id ON id.DetailID = dl.DetailID
          WHERE     id.ItemDetailID = fd.ItemDetailID
        ) ItemDetailName ,
        r.RefundMoney ,
        CONVERT(NVARCHAR(10), r.RefundTime, 23) RefundTime ,
        r.PayObject ,
        u.Name Creater ,
        r.Remark ,
        f.NoteNum ,
        s.IDCard ,
        r1.RefeName Sort ,
        r.Status StatusValue ,
        r2.RefeName Status ,
        d.Name DeptName
FROM    T_Pro_Refund r
        LEFT JOIN T_Pro_FeeDetail fd ON r.FeeDetailID = fd.FeeDetailID
        LEFT JOIN T_Pro_Fee f ON f.FeeID = fd.FeeID
        LEFT JOIN T_Pro_Prove p ON f.ProveID = p.ProveID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
        LEFT JOIN T_Sys_User u ON u.UserID = r.CreateID
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = r.Sort
                                      AND r1.RefeTypeID = 10
        LEFT JOIN T_Sys_Refe r2 ON r2.Value = r.Status
                                   AND r2.RefeTypeID = 1
        LEFT JOIN T_Pro_Item i ON i.ItemID = p.ItemID
        LEFT JOIN T_Sys_Dept d ON d.DeptID = f.DeptID
WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "f.DeptID", "f.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "RefundMoney"));
        }
        #endregion

        #region 获取核销实体
        /// <summary>
        /// 获取核销实体
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        public RefundModel GetRefundModel(string refundId)
        {
            string where = " and RefundID=@RefundID";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@RefundID",refundId)
            };
            RefundModel rm = RefundBLL.RefundModelByWhere(where, paras);
            return rm;

        }
        #endregion

        #region 停用启用
        public AjaxResult UpdateRefundStatus()
        {
            string refundId = Request.Form["ID"];
            string statas = Request.Form["Value"];
            RefundModel rm = GetRefundModel(refundId);
            rm.Status = statas;
            rm.UpdateID = this.UserId.ToString();
            rm.UpdateTime = DateTime.Now.ToString();
            if (RefundBLL.UpdateRefund(rm) > 0)
            {
                if (rm.Sort != "3")
                {
                    UpdateProveStatus(rm.FeeDetailID);
                }
                FeeDetailBLL.UpdateFeeDetailCanMoney(rm.FeeDetailID, this.UserId);
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");

            }
        }

        //public AjaxResult TempUpdateRefundStatus()
        //{
        //    for (int i = 18236; i <= 19102; i++)
        //    {
        //        RefundModel rm = GetRefundModel(i.ToString());
        //        rm.Status = "2";
        //        rm.UpdateID = this.UserId.ToString();
        //        rm.UpdateTime = DateTime.Now.ToString();
        //        if (RefundBLL.UpdateRefund(rm) > 0)
        //        {
        //            FeeDetailBLL.UpdateFeeDetailCanMoney(rm.FeeDetailID, this.UserId);
        //        }
        //    }
        //    return AjaxResult.Success("修改成功");
           
        //}
        #endregion

        #region 编辑核销
        public string GetRefundEdit(RefundModel rm)
        {
            if (string.IsNullOrEmpty(rm.RefundID))
            {
                rm.RefundID = "0";
            }
            if (string.IsNullOrEmpty(rm.FeeDetailID))
            {
                return "请选择收费项";
            }
            if (string.IsNullOrEmpty(rm.Sort))
            {
                return "请选择核销类别";
            }
            if (string.IsNullOrEmpty(rm.RefundMoney))
            {
                return "核销金额不能为空";
            }
            decimal tempMoney = FeeDetailBLL.GetCanMoney(rm.FeeDetailID, rm.RefundID);
            if (Convert.ToDecimal(rm.RefundMoney) > tempMoney)
            {
                return "核销金额不能大于" + tempMoney.ToString();
            }
            if (string.IsNullOrEmpty(rm.RefundTime))
            {
                return "核销时间不能为空";
            }
            if (string.IsNullOrEmpty(rm.PayObject))
            {
                return "支付对象不能为空";
            }
            else
            {
                if (rm.PayObject.Length > 32)
                {
                    return "支付对象不能超过32个字符";
                }
            }
            if (!string.IsNullOrEmpty(rm.Remark))
            {
                rm.Remark = rm.Remark.Replace("\r\n", "<br />");
            }
            bool flag = false;
            if (rm.RefundID.Equals("0"))
            {
                rm.Status = "1";
                rm.CreateID = this.UserId.ToString();
                rm.CreateTime = DateTime.Now.ToString();
                rm.UpdateID = this.UserId.ToString();
                rm.UpdateTime = DateTime.Now.ToString();
                if (RefundBLL.InsertRefund(rm) > 0)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                if (rm.Sort != "3")
                {
                    UpdateProveStatus(rm.FeeDetailID);
                }
                FeeDetailBLL.UpdateFeeDetailCanMoney(rm.FeeDetailID, this.UserId);
                return "yes";
            }
            else
            {
                return "出现未知错误，请联系管理员";
            }
        }
        #endregion

        #region 修改表单赋值
        public AjaxResult SelectRefund()
        {
            string refundId = Request.Form["ID"];
            DataTable dt = RefundBLL.SelectRefund(refundId);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }
        #endregion

        /// <summary>
        /// 根据核销feeDetailid，修改对应证书状态
        /// </summary>
        /// <param name="feeId"></param>
        public void UpdateProveStatus(string feeDetailId)
        {
            FeeDetailModel fdm = FeeDetailBLL.GetFeeDetailModel(feeDetailId);

            string isAll = "Yes";
            string where1 = " AND Status <> 9 AND FeeID = @FeeID";
            SqlParameter[] paras1 = new SqlParameter[] {
                new SqlParameter ("@FeeID", fdm.FeeID)
            };
            FeeModel fm = FeeBLL.FeeModelByWhere(where1, paras1);

            string where2 = " AND Status <> 9 AND ProveID = @ProveID";
            SqlParameter[] paras2 = new SqlParameter[] {
                new SqlParameter ("@ProveID", fm.ProveID)
            };
            DataTable dt = FeeBLL.FeeTableByWhere(where2, paras2, "");
            foreach (DataRow dr in dt.Rows)
            {
                string where4 = " AND Status = 1 AND FeeID = @FeeID";
                SqlParameter[] paras4 = new SqlParameter[] {
                    new SqlParameter ("@FeeID", dr["FeeID"].ToString())
                };
                DataTable dt2 = FeeDetailBLL.FeeDetailTableByWhere(where4, paras4, "");
                foreach (DataRow dr2 in dt2.Rows)
                {
                    if (FeeDetailBLL.GetCanMoney(dr2["FeeDetailID"].ToString(), "0") > 0)
                    {
                        isAll = "No";
                        break;
                    }
                }
            }

            string where3 = " AND Status <> 9 AND ProveID = @ProveID";
            SqlParameter[] paras3 = new SqlParameter[] {
                new SqlParameter("@ProveID", fm.ProveID)
            };
            ProveModel pm = ProveBLL.ProveModelByWhere(where3, paras3);
            if (isAll == "Yes")
            {
                pm.Status = "1";
            }
            else
            {
                pm.Status = "2";
            }
            pm.UpdateID = this.UserId.ToString();
            pm.UpdateTime = DateTime.Now.ToString();
            ProveBLL.UpdateProve(pm);
        }
        /// <summary>
        /// 导入费用核销
        /// </summary>
        /// <returns></returns>
        public AjaxResult UpLoadRefund()
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
                        FeeModel fm = FeeBLL.GetFeeModelByVoucherNum(array[0]);
                        if (fm.FeeID == null)
                        {
                            errorString += "凭证号不存在；";
                        }
                        string feeDetailId = FeeDetailBLL.GetFeeModelByItemNum(fm.FeeID, array[1]);
                        if (feeDetailId == "0")
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
                        decimal tempUseableMoney = 0;
                        decimal useableMoney = 0;
                        if (array[3] != "")
                        {
                            tempUseableMoney = decimal.Parse(array[3]);
                            useableMoney = FeeDetailBLL.GetCanMoney(feeDetailId, "0");
                            if (tempUseableMoney > useableMoney)
                            {
                                errorString += "可核销金额不足；";
                            }
                        }
                        if (string.IsNullOrEmpty(errorString))
                        {
                            InsertFeeRefund(feeDetailId, array, sort);
                            FeeDetailBLL.UpdateFeeDetailCanMoney(feeDetailId, this.UserId);
                            if (sort != "3")
                            {
                                UpdateProveStatus(feeDetailId);
                            }
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
                nm.Sort = "3";
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
        /// 新增核销
        /// </summary>
        /// <param name="feeId"></param>
        /// <param name="array"></param>
        public void InsertFeeRefund(string feedetailId, string[] array, string sort)
        {

            RefundModel rm = new RefundModel();
            rm.CreateTime = DateTime.Now.ToString();
            rm.Remark = array[6];
            rm.PayObject = array[5];
            rm.RefundTime = array[4];
            rm.RefundMoney = array[3];
            rm.FeeDetailID = feedetailId;
            rm.Sort = sort;
            rm.Status = "1";
            rm.UpdateTime = DateTime.Now.ToString();
            rm.CreateID = this.UserId.ToString();
            rm.UpdateID = this.UserId.ToString();
            RefundBLL.InsertRefund(rm);

        }

        public DataTable TableTitle(DataTable dt, string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                dt.Columns.Add(array[i], Type.GetType("System.String"));
            }
            dt.Columns.Add("系统备注", Type.GetType("System.String"));
            return dt;
        }
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
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>

        public AjaxResult Download()
        {
            string menuId = Request.Form["MenuID"];
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
                where += OtherHelper.MultiSelectToSQLWhere(selsort, "r.Sort");
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " and f.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(refundTimeS))
            {
                where += " and convert(nvarchar(10),r.RefundTime,23) >= '" + refundTimeS + "'";
            }
            if (!string.IsNullOrEmpty(refundTimeE))
            {
                where += " and convert(nvarchar(10),r.RefundTime,23) <= '" + refundTimeE + "'";
            }
            string cmdText = @"SELECT 
        f.VoucherNum 凭证号 ,
        f.NoteNum 票据号 ,
        s.Name 姓名 ,
        s.IDCard 身份证号 ,
        i.Name 证书名称 ,
        ( SELECT    dl.Name
          FROM      T_Pro_Detail dl
                    LEFT JOIN T_Pro_ItemDetail id ON id.DetailID = dl.DetailID
          WHERE     id.ItemDetailID = fd.ItemDetailID
        ) 收费项 ,
        r1.RefeName 核销类别 ,
        r.RefundMoney 核销金额 ,
        CONVERT(NVARCHAR(10), r.RefundTime, 23) 核销时间 ,
        r.PayObject 支付对象 ,
        u.Name 核销员 ,
        r2.RefeName 状态 ,
        r.Remark 核销备注
FROM    T_Pro_Refund r
        LEFT JOIN T_Pro_FeeDetail fd ON r.FeeDetailID = fd.FeeDetailID
        LEFT JOIN T_Pro_Fee f ON f.FeeID = fd.FeeID
        LEFT JOIN T_Pro_Prove p ON f.ProveID = p.ProveID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
        LEFT JOIN T_Sys_User u ON u.UserID = r.CreateID
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = r.Sort
                                      AND r1.RefeTypeID = 10
        LEFT JOIN T_Sys_Refe r2 ON r2.Value = r.Status
                                   AND r2.RefeTypeID = 1
        LEFT JOIN T_Pro_Item i ON i.ItemID = p.ItemID
WHERE   1 = 1 {0}";

            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "f.DeptID", "f.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            string filename = "../Temp/证书费用核销信息.xls";
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename, "success");
        }
        public ActionResult GetViewRefundList()
        {
            string feeId = Request.Form["FeeID"];
            string where = "";
            if (!string.IsNullOrEmpty(feeId))
            {
                where += "and r.FeeDetailID IN (SELECT FeeDetailID FROM  T_Pro_FeeDetail WHERE FeeID=" + feeId + ") and r.Status=1";
            }

            string cmdText = @"SELECT DISTINCT
        fd.FeeDetailID ,
        r.RefundID ,
        f.VoucherNum ,
        s.Name ,
        i.Name ItemName ,
        ( SELECT    dl.Name
          FROM      T_Pro_Detail dl
                    LEFT JOIN T_Pro_ItemDetail id ON id.DetailID = dl.DetailID
          WHERE     id.ItemDetailID = fd.ItemDetailID
        ) ItemDetailName ,
        r.RefundMoney ,
        CONVERT(NVARCHAR(10), r.RefundTime, 23) RefundTime ,
        r.PayObject ,
        u.Name Creater ,
        r.Remark ,
        f.NoteNum ,
        s.IDCard ,
        r1.RefeName Sort ,
        r.Status StatusValue,
        r2.RefeName Status,
		d.Name DeptName
FROM    T_Pro_Refund r
        LEFT JOIN T_Pro_FeeDetail fd ON r.FeeDetailID = fd.FeeDetailID
        LEFT JOIN T_Pro_Fee f ON f.FeeID = fd.FeeID
        LEFT JOIN T_Pro_Prove p ON f.ProveID = p.ProveID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
        LEFT JOIN T_Sys_User u ON u.UserID = r.CreateID
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = r.Sort
                                      AND r1.RefeTypeID = 10
        LEFT JOIN T_Sys_Refe r2 ON r2.Value = r.Status
                                   AND r2.RefeTypeID = 1
        LEFT JOIN T_Pro_Item i ON i.ItemID = p.ItemID
		LEFT JOIN T_Sys_Dept d ON d.DeptID=f.DeptID
WHERE   1 = 1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "RefundMoney"));
        }
    }
}
