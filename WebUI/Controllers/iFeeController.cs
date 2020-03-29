using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BLL;
using Common;
using DAL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebUI.Controllers
{
    public class iFeeController : BaseController
    {
        #region 页面
        /// <summary>
        /// 杂费收费主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult iFeeList()
        {
            return View();
        }
        public ActionResult iFeePrint()
        {
            return View();
        }
        /// <summary>
        /// 添加、修改页面
        /// </summary>
        /// <returns></returns>
        public ActionResult iFeeEdit()
        {
            return View();
        }

        /// <summary>
        /// 选择冲抵费用页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ChooseiFeeList()
        {
            return View();
        }

        /// <summary>
        /// 杂费导入页面
        /// </summary>
        /// <returns></returns>
        public ActionResult iFeeUp()
        {
            return View();
        }

        /// <summary>
        /// 选择冲抵费用页面（收多项费用）
        /// </summary>
        /// <returns></returns>
        public ActionResult ChooseiFeeListMore()
        {
            return View();
        }

        /// <summary>
        /// 添加页面（收多项费用）
        /// </summary>
        /// <returns></returns>
        public ActionResult iFeeAddMore()
        {
            return View();
        }

        /// <summary>
        /// 选择充抵项（收多项费用）
        /// </summary>
        /// <returns></returns>
        public ActionResult ChooseOffsetMore()
        {
            return View();
        }

        /// <summary>
        /// 查看页面
        /// </summary>
        /// <returns></returns>
        public ActionResult iFeeDetail()
        {
            return View();
        }
        #endregion

        #region 获取列表、添加、修改、批量导入添加
        /// <summary>
        /// 得到杂费列表信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetiFeeList()
        {
            string menuId = Request.Form["MenuID"];
            string name = Request.Form["txtName"];
            string idCard = Request.Form["txtIDCard"];
            string voucherNum = Request.Form["txtVoucherNum"];
            string noteNum = Request.Form["txtNoteNum"];
            string createName = Request.Form["txtCreateName"];
            string dept = Request.Form["treeDept"];
            string feeMode = Request.Form["selFeeMode"];
            string status = Request.Form["selStatus"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string feeName = Request.Form["txtFeeName"];
            string personSort = Request.Form["selPersonSort"];
            string deptAreaId = Request.Form["selDeptAreaID"];
         
            string where = "";
            string enrollNum = Request.Form["EnrollNum"];
            if (!string.IsNullOrEmpty(enrollNum))
            {
                where += @" AND student.StudentID IN ( SELECT   se.StudentID
                                   FROM T_Stu_sEnroll se
                               WHERE    se.EnrollNum LIKE '%" + enrollNum + "%' )";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND student.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " AND student.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " AND ifee.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " AND ifee.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(createName))
            {
                where += " AND sysuser.Name like '%" + createName + "%'";
            }
            if (!string.IsNullOrEmpty(dept))
            {
                where += "AND ifee.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(feeMode))
            {
                where += OtherHelper.MultiSelectToSQLWhere(feeMode, "ifee.FeeMode");
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "ifee.Status");
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where += " AND convert(NVARCHAR(10),ifee.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where += " AND convert(NVARCHAR(10),ifee.FeeTime,23) <= '" + feeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(feeName))
            {
                feeName = feeName.Substring(feeName.IndexOf(" ") + 1, feeName.Length - feeName.IndexOf(" ") - 1);
                where += " and ( detail.Name='" + feeName + "' or i.Name='" + feeName + "' )";
            }
            if (!string.IsNullOrEmpty(personSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(personSort, "ifee.PersonSort");
            }
            if (!string.IsNullOrEmpty(deptAreaId))
            {
                where += OtherHelper.MultiSelectToSQLWhere(deptAreaId, "ifee.DeptAreaID");
            }
            string cmdText = @"SELECT  ifee.iFeeID ,
        ifee.StudentID ,
        ifee.VoucherNum ,
        dept.Name AS Dept ,
        deptarea.Name AS DeptAreaName ,
        student.Name ,
        student.IDCard ,
        ifee.NoteNum ,
        detail.Name AS FeeContent ,
        CONVERT(NVARCHAR(23), ifee.FeeTime, 23) AS FeeTime ,
        refe1.RefeName AS FeeMode ,
        ifee.ShouldMoney ,
        ifee.PaidMoney ,
        ifee.DiscountMoney ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Inc_iOffset
          WHERE     iFeeID = ifee.iFeeID
                    AND Status = 1
        ) AS OffsetMoney ,
        ( SELECT    ife.VoucherNum + '_' + s.Name + '_'
                    + CONVERT(NVARCHAR(10), iof.Money) + ','
          FROM      T_Inc_iFee ife
                    LEFT JOIN T_Pro_Student s ON ife.StudentID = s.StudentID
                    LEFT JOIN T_Inc_iOffset iof ON ife.iFeeID = iof.ByiFeeID
          WHERE     ife.Status <> 9
                    AND iof.Status = 1
                    AND iof.iFeeID = ifee.iFeeID
        FOR
          XML PATH('')
        ) OffsetStr ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Inc_iOffset
          WHERE     ByiFeeID = ifee.iFeeID
                    AND Status = 1
        ) + ( SELECT    ISNULL(SUM(so.Money), 0)
              FROM      T_Stu_sOffset so
              WHERE     so.BySort = 2
                        AND so.ByRelatedID = ifee.iFeeID
                        AND so.Status = 1
            ) AS BeOffsetMoney ,
        ( SELECT    t + ','
          FROM      ( SELECT    ife.VoucherNum + '_' + s.Name + '_'
                                + CONVERT(NVARCHAR(10), iof.Money) t
                      FROM      T_Inc_iFee ife
                                LEFT JOIN T_Pro_Student s ON ife.StudentID = s.StudentID
                                LEFT JOIN T_Inc_iOffset iof ON ife.iFeeID = iof.iFeeID
                      WHERE     ife.Status <> 9
                                AND iof.Status = 1
                                AND iof.ByiFeeID = ifee.iFeeID
                      UNION ALL
                      SELECT    sf.VoucherNum + '_' + s.Name + '_'
                                + CONVERT(NVARCHAR(10), so.Money)
                      FROM      T_Stu_sOffset so
                                LEFT JOIN T_Stu_sFeesOrder sfo ON so.RelatedID = sfo.sFeesOrderID
                                LEFT JOIN T_Stu_sFee sf ON sfo.sFeeID = sf.sFeeID
                                LEFT JOIN T_Stu_sEnrollsProfession ep ON sf.sEnrollsProfessionID = ep.sEnrollsProfessionID
                                LEFT JOIN T_Stu_sEnroll se ON ep.sEnrollID = se.sEnrollID
                                LEFT JOIN T_Pro_Student s ON se.StudentID = s.StudentID
                      WHERE     so.BySort = 2
                                AND so.Status = 1
                                AND so.ByRelatedID = ifee.iFeeID
                    ) AS byTemp
        FOR
          XML PATH('')
        ) BeOffsetStr ,
        ( SELECT    ISNULL(SUM(RefundMoney), 0)
          FROM      T_Inc_iRefund
          WHERE     iFeeID = ifee.iFeeID
                    AND Status = 1
        ) AS RefundMoney ,
        sysuser.Name AS CreateName ,
        refe2.RefeName AS PersonSort ,
        refe3.RefeName AS Status ,
        ifee.Status AS StatusValue ,
        sysuser2.Name AS Affirm ,
        CASE CONVERT(NVARCHAR(23), ifee.AffirmTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(23), ifee.AffirmTime, 23)
        END AffirmTime ,
        ifee.Explain ,
        ifee.Remark ,
        ( SELECT    CASE im.Name
                      WHEN '杂费收费项' THEN ''
                      ELSE im.Name + '-'
                    END
          FROM      T_Pro_Item im
          WHERE     im.ItemID IN (
                    SELECT  ItemID
                    FROM    dbo.GetParentItemID(( SELECT    ItemID
                                                  FROM      T_Pro_Item
                                                  WHERE     DeptID = ifee.DeptID
                                                            AND ItemID = itemdetail.ItemID
                                                )) )
        FOR
          XML PATH('')
        ) FeeParentContent ,
        ( SELECT    EnrollNum + ','
          FROM      T_Stu_sEnroll
          WHERE     StudentID = student.StudentID
                    AND Status = 1
        FOR
          XML PATH('')
        ) EnrollNum
FROM    T_Inc_iFee AS ifee
        LEFT JOIN T_Pro_Student AS student ON ifee.StudentID = student.StudentID
        LEFT JOIN T_Sys_Dept AS dept ON ifee.DeptID = dept.DeptID
        LEFT JOIN T_Pro_ItemDetail AS itemdetail ON ifee.ItemDetailID = itemdetail.ItemDetailID
        LEFT JOIN T_Pro_Item i ON i.ItemID = itemdetail.ItemID
        LEFT JOIN T_Pro_Detail AS detail ON itemdetail.DetailID = detail.DetailID
        LEFT JOIN T_Sys_Refe AS refe1 ON ifee.FeeMode = refe1.Value
                                         AND refe1.RefeTypeID = 6
        LEFT JOIN T_Sys_User AS sysuser ON ifee.CreateID = sysuser.UserID
        LEFT JOIN T_Sys_Refe AS refe2 ON ifee.PersonSort = refe2.Value
                                         AND refe2.RefeTypeID = 11
        LEFT JOIN T_Sys_Refe AS refe3 ON ifee.Status = refe3.Value
                                         AND refe3.RefeTypeID = 7
        LEFT JOIN T_Sys_User AS sysuser2 ON ifee.AffirmID = sysuser2.UserID
        LEFT JOIN T_Pro_DeptArea AS deptarea ON ifee.DeptAreaID = deptarea.DeptAreaID
WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ifee.DeptID", "ifee.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "ShouldMoney,PaidMoney,DiscountMoney,OffsetMoney,BeOffsetMoney,RefundMoney"));
        }

        /// <summary>
        /// 添加、修改杂费收费
        /// </summary>
        /// <returns></returns>
        public string GetiFeeEdit(iFeeModel ifm)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                string result = "出现未知错误，请联系管理员";

                try
                {
                    #region 后台验证
                    string iFeeID = "0";
                    string offsetMoney = Request.Form["editOffsetMoney"];//充抵金额
                    string editChooseOffSet = Request.Form["editChooseOffSet"];//充抵详细，json格式字符串

                    if (!string.IsNullOrEmpty(ifm.iFeeID))
                    {
                        iFeeID = ifm.iFeeID;
                    }
                    if (string.IsNullOrEmpty(ifm.DeptID))
                    {
                        return "请选择收费单位";
                    }
                    if (string.IsNullOrEmpty(ifm.DeptAreaID))
                    {
                        return "请选择收费校区";
                    }
                    if (string.IsNullOrEmpty(ifm.StudentID))
                    {
                        return "请选择缴费人";
                    }
                    if (string.IsNullOrEmpty(ifm.PersonSort))
                    {
                        return "请选择交款人员";
                    }
                    if (string.IsNullOrEmpty(ifm.FeeTime))
                    {
                        return "收费时间不能为空";
                    }
                    if (string.IsNullOrEmpty(ifm.ItemDetailID))
                    {
                        return "请选择收费项目";
                    }
                    ifm.ItemDetailID = ifm.ItemDetailID.Substring(ifm.ItemDetailID.IndexOf("_") + 1, ifm.ItemDetailID.Length - ifm.ItemDetailID.IndexOf("_") - 1);
                    if (!OtherHelper.IsInt(ifm.ItemDetailID) || !ItemDetailBLL.ValideteItemDetailID(ifm.DeptID, "2", ifm.ItemDetailID))
                    {
                        return "收费项目选择错误";
                    }
                    if (string.IsNullOrEmpty(ifm.FeeMode))
                    {
                        return "请选择缴费方式";
                    }
                    if (string.IsNullOrEmpty(ifm.ShouldMoney))
                    {
                        return "应收金额不能为空";
                    }
                    if (decimal.Parse(ifm.ShouldMoney).Equals(0))
                    {
                        return "应收金额不能为0";
                    }
                    if (string.IsNullOrEmpty(ifm.PaidMoney))
                    {
                        return "实收金额不能为空";
                    }
                    if (string.IsNullOrEmpty(ifm.DiscountMoney))
                    {
                        ifm.DiscountMoney = "0.00";
                    }
                    if (string.IsNullOrEmpty(offsetMoney))
                    {
                        offsetMoney = "0.00";
                    }
                    DataTable dt = ItemDetailBLL.ItemDetailGetMoneyAll(ifm.ItemDetailID);
                    //非固定金额，需判断应收金额是否大于等于非固定金额
                    if (dt.Rows.Count > 0 && dt.Rows[0]["Sort"].ToString() == "2")
                    {
                        if (Convert.ToDecimal(ifm.ShouldMoney) < Convert.ToDecimal(dt.Rows[0]["Money"].ToString()))
                        {
                            return "应收金额必选大于等于" + dt.Rows[0]["Money"].ToString() + "元";
                        }
                    }
                    if (Convert.ToDecimal(ifm.ShouldMoney) != Convert.ToDecimal(ifm.PaidMoney) + Convert.ToDecimal(ifm.DiscountMoney) + Convert.ToDecimal(offsetMoney))
                    {
                        return "应收金额必须等于实收金额+优惠金额+冲抵金额";
                    }
                    if (!string.IsNullOrEmpty(ifm.Explain))
                    {
                        ifm.Explain = ifm.Explain.Replace("\r\n", "<br />");
                    }
                    if (!string.IsNullOrEmpty(ifm.Remark))
                    {
                        ifm.Remark = ifm.Remark.Replace("\r\n", "<br />");
                    }
                    #endregion

                    if (iFeeID.Trim() != "0")
                    {
                        #region 修改
                        string where = " AND iFeeID = @iFeeID";
                        SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@iFeeID", iFeeID) };
                        iFeeModel model = iFeeBLL.iFeeModelByWhere(where, paras);

                        LogBLL.CreateLog("T_Inc_iFee", this.UserId.ToString(), model, ifm);//写日志

                        model.DeptID = ifm.DeptID;
                        model.DeptAreaID = ifm.DeptAreaID;
                        model.StudentID = ifm.StudentID;
                        model.PersonSort = ifm.PersonSort;
                        model.FeeTime = ifm.FeeTime;
                        model.ItemDetailID = ifm.ItemDetailID;
                        model.FeeMode = ifm.FeeMode;
                        model.ShouldMoney = ifm.ShouldMoney;
                        model.PaidMoney = ifm.PaidMoney;
                        model.DiscountMoney = ifm.DiscountMoney;
                        model.CanMoney = (Convert.ToDecimal(ifm.PaidMoney) + Convert.ToDecimal(offsetMoney) - GetRefundMoney(iFeeID) - GetBeOffMoney(iFeeID)).ToString();//可用金额=实收+充抵-核销-被充抵
                        model.Remark = ifm.Remark;
                        model.Explain = ifm.Explain;
                        model.UpdateID = this.UserId.ToString();
                        model.UpdateTime = DateTime.Now.ToString();

                        if (iFeeBLL.UpdateiFee(model) > 0)
                        {
                            result = "yes";
                        }
                        #endregion
                    }
                    else
                    {
                        #region 添加
                        ifm.Status = "1";
                        ifm.VoucherNum = getVoucherNum();//凭证号
                        ifm.NoteNum = string.Empty;//票据号
                        ifm.CanMoney = (Convert.ToDecimal(ifm.PaidMoney) + Convert.ToDecimal(offsetMoney)).ToString();//可用金额=实收+充抵-核销-被充抵
                        ifm.PrintNum = "0";
                        ifm.AffirmID = "0";
                        ifm.AffirmTime = "1900-01-01";
                        ifm.CreateID = this.UserId.ToString();
                        ifm.CreateTime = DateTime.Now.ToString();
                        ifm.UpdateID = this.UserId.ToString();
                        ifm.UpdateTime = DateTime.Now.ToString();
                        int returnID = iFeeBLL.InsertiFee(ifm);
                        if (returnID > 0)
                        {
                            result = "yes";
                        }

                        //写入充抵数据
                        if (!string.IsNullOrEmpty(editChooseOffSet))
                        {
                            //{"total":1,"rows":[{"iFeeID":"2","VoucherNum":"20160126000002","Dept":"四川五月花专修学院","Name":"赖江灵","IDCard":"513701199901180511","NoteNum":"","FeeContent":"电费","FeeTime":"2016-01-27","OffSetMoney":"20.00"}]}
                            JObject job = JObject.Parse(editChooseOffSet);
                            string[] array = job.Properties().Where(x => x.Name == "rows").Select(item => item.Value.ToString()).ToArray();
                            List<Model.iFeeModel.iFeeOffSetModel> offSetList = JsonConvert.DeserializeObject<List<Model.iFeeModel.iFeeOffSetModel>>(array[0]);//将json数据转化为对象类型并赋值给list
                            foreach (var item in offSetList)
                            {
                                //后台验证充抵金额是否大于可用金额
                                if (decimal.Parse(item.OffSetMoney) > GetCanMoney(item.iFeeID))
                                {
                                    result = "充抵金额不能大于可充抵金额";
                                    break;
                                }
                                else
                                {
                                    iOffsetModel model = new iOffsetModel();
                                    model.Status = "1";
                                    model.iFeeID = returnID.ToString();
                                    model.ByiFeeID = item.iFeeID;
                                    model.Money = item.OffSetMoney;
                                    model.CreateID = this.UserId.ToString();
                                    model.CreateTime = DateTime.Now.ToString();
                                    model.UpdateID = this.UserId.ToString();
                                    model.UpdateTime = DateTime.Now.ToString();
                                    iOffsetBLL.InsertiOffset(model);
                                }

                                //修改被充抵项的可用金额
                                iFeeModel fm = iFeeBLL.iFeeModelByWhere(" AND iFeeID=@iFeeID", new SqlParameter[] { new SqlParameter("@iFeeID", item.iFeeID) });
                                fm.CanMoney = GetCanMoney(item.iFeeID).ToString();
                                iFeeBLL.UpdateiFee(fm);
                            }
                        }
                        #endregion
                    }
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
        /// 批量导入添加
        /// </summary>
        /// <param name="ifm"></param>
        /// <returns></returns>
        //public AjaxResult UpLoadiFee(iFeeModel ifm)
        //{
        //    try
        //    {
        //        #region 后台验证
        //        string filePath = Request.Form["filePath"];
        //        if (string.IsNullOrEmpty(ifm.DeptID))
        //        {
        //            return AjaxResult.Error("请选择收费单位");
        //        }
        //        if (string.IsNullOrEmpty(ifm.DeptAreaID))
        //        {
        //            return AjaxResult.Error("请选择收费校区");
        //        }
        //        if (string.IsNullOrEmpty(ifm.PersonSort))
        //        {
        //            return AjaxResult.Error("请选择交款人员");
        //        }
        //        if (string.IsNullOrEmpty(filePath))
        //        {
        //            return AjaxResult.Error("请选择人员文件");
        //        }
        //        if (string.IsNullOrEmpty(ifm.FeeTime))
        //        {
        //            return AjaxResult.Error("收费时间不能为空");
        //        }
        //        if (string.IsNullOrEmpty(ifm.ItemDetailID))
        //        {
        //            return AjaxResult.Error("请选择收费项目");
        //        }
        //        ifm.ItemDetailID = ifm.ItemDetailID.Substring(ifm.ItemDetailID.IndexOf("_") + 1, ifm.ItemDetailID.Length - ifm.ItemDetailID.IndexOf("_") - 1);
        //        if (!OtherHelper.IsInt(ifm.ItemDetailID) || !ItemDetailBLL.ValideteItemDetailID(ifm.DeptID, "2", ifm.ItemDetailID))
        //        {
        //            return AjaxResult.Error("收费项目选择错误");
        //        }
        //        if (string.IsNullOrEmpty(ifm.FeeMode))
        //        {
        //            return AjaxResult.Error("请选择缴费方式");
        //        }
        //        if (string.IsNullOrEmpty(ifm.ShouldMoney))
        //        {
        //            return AjaxResult.Error("应收金额不能为空");
        //        }
        //        if (string.IsNullOrEmpty(ifm.PaidMoney))
        //        {
        //            return AjaxResult.Error("实收金额不能为空");
        //        }
        //        if (string.IsNullOrEmpty(ifm.DiscountMoney))
        //        {
        //            ifm.DiscountMoney = "0.00";
        //        }
        //        DataTable dt = ItemDetailBLL.ItemDetailGetMoneyAll(ifm.ItemDetailID);
        //        //非固定金额，需判断应收金额是否大于等于非固定金额
        //        if (dt.Rows.Count > 0 && dt.Rows[0]["Sort"].ToString() == "2")
        //        {
        //            if (Convert.ToDecimal(ifm.ShouldMoney) < Convert.ToDecimal(dt.Rows[0]["Money"].ToString()))
        //            {
        //                return AjaxResult.Error("应收金额必选大于等于" + dt.Rows[0]["Money"].ToString() + "元");
        //            }
        //        }
        //        if (Convert.ToDecimal(ifm.ShouldMoney) != Convert.ToDecimal(ifm.PaidMoney) + Convert.ToDecimal(ifm.DiscountMoney))
        //        {
        //            return AjaxResult.Error("应收金额必须等于实收金额+优惠金额");
        //        }
        //        if (!string.IsNullOrEmpty(ifm.Explain))
        //        {
        //            ifm.Explain = ifm.Explain.Replace("\r\n", "<br />");
        //        }
        //        if (!string.IsNullOrEmpty(ifm.Remark))
        //        {
        //            ifm.Remark = ifm.Remark.Replace("\r\n", "<br />");
        //        }
        //        #endregion

        //        #region 处理excel数据
        //        DataTable oldDt = new DataTable();
        //        try
        //        {
        //            oldDt = OtherHelper.UploadFile(filePath).Tables[0];
        //        }
        //        catch (Exception)
        //        {

        //            return AjaxResult.Error("模板异常，请联系管理员");
        //        }

        //        DataTable myDt = new DataTable();
        //        if (oldDt != null || oldDt.Rows.Count > 0)
        //        {
        //            int errorNum = 0;
        //            int successNum = 0;
        //            string url = string.Empty;
        //            string[] array = new string[4];
        //            for (int i = 0; i < oldDt.Rows.Count; i++)
        //            {
        //                try
        //                {
        //                    array[0] = oldDt.Rows[i][0].ToString().Trim();//姓名
        //                    array[1] = oldDt.Rows[i][1].ToString().Trim();//身份证号
        //                    array[2] = oldDt.Rows[i][2].ToString().Trim();//性别
        //                    array[3] = oldDt.Rows[i][3].ToString().Trim();//手机
        //                }
        //                catch
        //                {
        //                    return AjaxResult.Error("模板错误");
        //                }
        //                if (i == 0)
        //                {
        //                    if (array[0] != "姓名" || array[1] != "身份证号" || array[2] != "性别" || array[3] != "手机")
        //                    {
        //                        return AjaxResult.Error("模板错误");
        //                    }
        //                    else
        //                    {
        //                        myDt = TableTitle(myDt, array);
        //                    }
        //                }
        //                else
        //                {
        //                    DataTable stuDt = StudentBLL.StudentTableByWhere(" AND Name=@Name AND IDCard=@IDCard", new SqlParameter[] { new SqlParameter("@Name", array[0]), new SqlParameter("@IDCard", array[1]) }, "");
        //                    //查到该人员
        //                    if (stuDt.Rows.Count > 0)
        //                    {
        //                        #region 添加收费
        //                        ifm.Status = "1";
        //                        ifm.VoucherNum = getVoucherNum();//凭证号
        //                        ifm.NoteNum = string.Empty;//票据号
        //                        ifm.StudentID = stuDt.Rows[0]["StudentID"].ToString();
        //                        ifm.CanMoney = ifm.PaidMoney;//此处的可用金额=实收
        //                        ifm.PrintNum = "0";
        //                        ifm.AffirmID = "0";
        //                        ifm.AffirmTime = "1900-01-01";
        //                        ifm.CreateID = this.UserId.ToString();
        //                        ifm.CreateTime = DateTime.Now.ToString();
        //                        ifm.UpdateID = this.UserId.ToString();
        //                        ifm.UpdateTime = DateTime.Now.ToString();
        //                        iFeeBLL.InsertiFee(ifm);
        //                        successNum++;
        //                        #endregion
        //                    }
        //                    //未查到该人员，验证数据
        //                    else
        //                    {
        //                        string errorResult = ValidateExcelWord(array);
        //                        //格式错误，写记录表
        //                        if (errorResult.Trim() != "")
        //                        {
        //                            myDt = TableRows(myDt, array, errorResult);
        //                            errorNum++;
        //                        }
        //                        //格式正确，先添加人员，然后再收费
        //                        else
        //                        {
        //                            #region 添加人员
        //                            StudentModel stuModel = new StudentModel();
        //                            stuModel.Status = "1";
        //                            stuModel.DeptID = ifm.DeptID;
        //                            stuModel.Name = array[0];
        //                            stuModel.IDCard = array[1];
        //                            stuModel.Sex = (array[2] == "男") ? "1" : "2";
        //                            stuModel.Mobile = array[3];
        //                            stuModel.QQ = string.Empty;
        //                            stuModel.WeChat = string.Empty;
        //                            stuModel.Address = string.Empty;
        //                            stuModel.Remark = string.Empty;
        //                            stuModel.CreateID = this.UserId.ToString();
        //                            stuModel.CreateTime = DateTime.Now.ToString();
        //                            stuModel.UpdateID = this.UserId.ToString();
        //                            stuModel.UpdateTime = DateTime.Now.ToString();
        //                            int stuId = StudentBLL.InsertStudent(stuModel);
        //                            #endregion

        //                            #region 添加收费
        //                            ifm.Status = "1";
        //                            ifm.VoucherNum = getVoucherNum();//凭证号
        //                            ifm.NoteNum = string.Empty;//票据号
        //                            ifm.StudentID = stuId.ToString();
        //                            ifm.CanMoney = ifm.PaidMoney;//此处的可用金额=实收
        //                            ifm.PrintNum = "0";
        //                            ifm.AffirmID = "0";
        //                            ifm.AffirmTime = "1900-01-01";
        //                            ifm.CreateID = this.UserId.ToString();
        //                            ifm.CreateTime = DateTime.Now.ToString();
        //                            ifm.UpdateID = this.UserId.ToString();
        //                            ifm.UpdateTime = DateTime.Now.ToString();
        //                            iFeeBLL.InsertiFee(ifm);
        //                            successNum++;
        //                            #endregion
        //                        }
        //                    }
        //                }
        //            }
        //            //保存错误文件
        //            if (myDt.Rows.Count > 0)
        //            {
        //                string FileName = OtherHelper.FilePathAndName();
        //                OtherHelper.DeriveToExcel(myDt, FileName);
        //                url = "../Temp/" + FileName + "";
        //            }
        //            NoteModel nm = new NoteModel();
        //            nm.CreateID = this.UserId.ToString();
        //            nm.CreateTime = DateTime.Now.ToString();
        //            nm.InFile = filePath;
        //            nm.OutFile = url;
        //            nm.Sort = "4";
        //            nm.DeptID = ifm.DeptID;
        //            nm.Status = "1";
        //            nm.SuccessNum = successNum.ToString();
        //            nm.ErrorNum = errorNum.ToString();
        //            NoteBLL.InsertNote(nm);
        //            string susscee = "成功导入" + successNum.ToString() + "条，错误数据" + errorNum.ToString() + "条。";
        //            string json = "{\"Tip\":\"操作成功\",\"Mesg\":\"" + susscee + "\",\"Url\":\"" + url + "\"}";
        //            return AjaxResult.Success(json);
        //        }
        //        else
        //        {
        //            return AjaxResult.Error("操作失败，导入数据不能为空");
        //        }
        //        #endregion
        //    }
        //    catch
        //    {
        //        return AjaxResult.Error("未知错误，请联系管理员");
        //    }
        //}

        /// <summary>
        /// 修改绑定杂费
        /// </summary>
        /// <returns></returns>
        public AjaxResult SelectiFee()
        {
            string ifeeId = Request.Form["ID"];
            DataTable dt = iFeeBLL.SelectiFee(ifeeId);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        /// <summary>
        /// 得到杂费信息选择列表（排除作废的）
        /// </summary>
        public ActionResult GetChooseiFeeList()
        {
            string menuId = Request.Form["MenuID"];
            string deptId = Request.Form["DeptID"];
            string name = Request.Form["txtStudentNameChooseFee"];
            string voucherNum = Request.Form["txtVoucherChooseFee"];
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND student.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " AND ifee.VoucherNum like '%" + voucherNum + "%'";
            }
            //if (!string.IsNullOrEmpty(deptId))
            //{
            //    where += " AND ifee.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            //}
            string cmdText = @"SELECT  ifee.iFeeID ,
        ifee.VoucherNum ,
        dept.Name AS Dept ,
        student.Name ,
        student.IDCard ,
        ifee.NoteNum ,
        detail.Name AS FeeContent ,
        CONVERT(NVARCHAR(23), ifee.FeeTime, 23) AS FeeTime ,
        ifee.CanMoney ,
        '0.00' AS OffSetMoney 
FROM    T_Inc_iFee AS ifee
        LEFT JOIN T_Pro_Student AS student ON ifee.StudentID = student.StudentID
        LEFT JOIN T_Sys_Dept AS dept ON ifee.DeptID = dept.DeptID
        LEFT JOIN T_Pro_ItemDetail AS itemdetail ON ifee.ItemDetailID = itemdetail.ItemDetailID
        LEFT JOIN T_Pro_Detail AS detail ON itemdetail.DetailID = detail.DetailID WHERE  ifee.Status != 9 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ifee.DeptID", "ifee.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, ""));
        }
        #endregion

        #region 添加（多项）
        /// <summary>
        /// 添加杂费收费（多项）
        /// </summary>
        /// <returns></returns>
        public string GetiFeeAdd(iFeeModel ifm)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                string result = "出现未知错误，请联系管理员";

                #region 后台验证
                string tips = string.Empty;
                string itemDetail = Request.Form["AddChooseItemDetail"];//费用项详细
                if (string.IsNullOrEmpty(ifm.DeptID))
                {
                    return "请选择收费单位";
                }
                if (string.IsNullOrEmpty(ifm.DeptAreaID))
                {
                    return "请选择收费校区";
                }
                if (string.IsNullOrEmpty(ifm.StudentID))
                {
                    return "请选择缴费人";
                }
                if (string.IsNullOrEmpty(ifm.PersonSort))
                {
                    return "请选择交款人员";
                }
                if (string.IsNullOrEmpty(ifm.FeeTime))
                {
                    return "收费时间不能为空";
                }
                if (Convert.ToDecimal(Request.Form["AddShouldMoney"]) != Convert.ToDecimal(Request.Form["AddPaidMoney"]) + Convert.ToDecimal(Request.Form["AddDiscountMoney"]) + Convert.ToDecimal(Request.Form["AddOffsetMoney"]))
                {
                    return "应收金额必须等于实收金额+优惠金额+冲抵金额";
                }

                JObject itemObj = JObject.Parse(itemDetail);
                string[] itemArray = itemObj.Properties().Where(x => x.Name == "rows").Select(item => item.Value.ToString()).ToArray();
                List<Model.iFeeModel.iFeeItemDetailModel> itemDetailList = JsonConvert.DeserializeObject<List<Model.iFeeModel.iFeeItemDetailModel>>(itemArray[0]);//将json数据转化为对象类型并赋值给list
                foreach (var item in itemDetailList)
                {
                    item.ItemDetailID = item.ItemDetailID.Substring(item.ItemDetailID.IndexOf("_") + 1, item.ItemDetailID.Length - item.ItemDetailID.IndexOf("_") - 1);
                    DataTable dt = ItemDetailBLL.ItemDetailGetMoneyAll(item.ItemDetailID);
                    //非固定金额，需判断应收金额是否大于等于非固定金额
                    if (dt.Rows.Count > 0 && dt.Rows[0]["Sort"].ToString() == "2")
                    {
                        if (Convert.ToDecimal(item.ShouldMoney) < Convert.ToDecimal(dt.Rows[0]["Money"].ToString()))
                        {
                            tips = item.ItemDetailName + "的应收金额必选大于等于" + dt.Rows[0]["Money"].ToString() + "元";
                            break;
                        }
                    }
                }
                if (tips != "")
                {
                    return tips;
                }
                #endregion

                try
                {
                    #region 添加
                    foreach (var item in itemDetailList)
                    {
                        //写入iFee
                        ifm.Status = "1";
                        ifm.VoucherNum = getVoucherNum();//凭证号
                        ifm.NoteNum = string.Empty;//票据号
                        ifm.ItemDetailID = item.ItemDetailID;
                        ifm.FeeMode = item.FeeMode;
                        ifm.ShouldMoney = item.ShouldMoney;
                        ifm.PaidMoney = item.PaidMoney;
                        ifm.DiscountMoney = item.DiscountMoney;
                        ifm.CanMoney = (Convert.ToDecimal(item.PaidMoney) + Convert.ToDecimal(item.OffSetMoney)).ToString();//可用金额=实收+充抵-核销-被充抵
                        ifm.PrintNum = "0";
                        ifm.AffirmID = "0";
                        ifm.AffirmTime = "1900-01-01";
                        ifm.Explain = item.Explain;
                        ifm.Remark = item.Remark;
                        ifm.CreateID = this.UserId.ToString();
                        ifm.CreateTime = DateTime.Now.ToString();
                        ifm.UpdateID = this.UserId.ToString();
                        ifm.UpdateTime = DateTime.Now.ToString();
                        int returnID = iFeeBLL.InsertiFee(ifm);

                        //写入充抵数据
                        if (!string.IsNullOrEmpty(item.OffsetData) && item.OffsetData != "[]")
                        {
                            //{"total":1,"rows":[{"iFeeID":"2","VoucherNum":"20160126000002","Dept":"四川五月花专修学院","Name":"赖江灵","IDCard":"513701199901180511","NoteNum":"","FeeContent":"电费","FeeTime":"2016-01-27","OffSetMoney":"20.00"}]}
                            JObject job = JObject.Parse(item.OffsetData);
                            string[] array = job.Properties().Where(x => x.Name == "rows").Select(x => x.Value.ToString()).ToArray();
                            List<Model.iFeeModel.iFeeOffSetModel> offSetList = JsonConvert.DeserializeObject<List<Model.iFeeModel.iFeeOffSetModel>>(array[0]);//将json数据转化为对象类型并赋值给list
                            foreach (var offset in offSetList)
                            {
                                //后台验证充抵金额是否大于可用金额
                                if (decimal.Parse(offset.OffSetMoney) > GetCanMoney(offset.iFeeID))
                                {
                                    return item.ItemDetailName + "的充抵金额不能大于可充抵金额";
                                }
                                else
                                {
                                    iOffsetModel model = new iOffsetModel();
                                    model.Status = "1";
                                    model.iFeeID = returnID.ToString();
                                    model.ByiFeeID = offset.iFeeID;
                                    model.Money = offset.OffSetMoney;
                                    model.CreateID = this.UserId.ToString();
                                    model.CreateTime = DateTime.Now.ToString();
                                    model.UpdateID = this.UserId.ToString();
                                    model.UpdateTime = DateTime.Now.ToString();
                                    iOffsetBLL.InsertiOffset(model);
                                }

                                //修改被充抵项的可用金额
                                iFeeModel fm = iFeeBLL.iFeeModelByWhere(" AND iFeeID=@iFeeID", new SqlParameter[] { new SqlParameter("@iFeeID", offset.iFeeID) });
                                fm.CanMoney = GetCanMoney(offset.iFeeID).ToString();
                                iFeeBLL.UpdateiFee(fm);
                            }
                        }
                    }
                    result = "yes";
                    #endregion
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
        #endregion

        #region 生成导出Excel列
        /// <summary>
        /// 生成导出Excel列
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="array"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public DataTable TableRows(DataTable dt, iFeeExcelModel fem, string remark)
        {
            DataRow dr = dt.NewRow();
            dr["姓名"] = fem.姓名;
            dr["身份证号"] = fem.身份证号;
            dr["学号"] = fem.学号;
            dr["性别"] = fem.性别;
            dr["手机"] = fem.手机;
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
        public DataTable TableTitle(DataTable dt)
        {

            dt.Columns.Add("姓名", Type.GetType("System.String"));
            dt.Columns.Add("身份证号", Type.GetType("System.String"));
            dt.Columns.Add("学号", Type.GetType("System.String"));
            dt.Columns.Add("性别", Type.GetType("System.String"));
            dt.Columns.Add("手机", Type.GetType("System.String"));
            dt.Columns.Add("系统备注", Type.GetType("System.String"));
            return dt;
        }
        #endregion

        #region 验证Excel数据
        private static string ValidateExcelWord(string[] array)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(array[0]))
            {
                error += "【姓名不能为空】";
            }
            else if (!string.IsNullOrEmpty(array[0]) && array[0].Length > 8)
            {
                error += "【姓名不能超过8个字符】";
            }

            if (string.IsNullOrEmpty(array[1]))
            {
                error += "【身份证号不能为空】";
            }
            else if (!string.IsNullOrEmpty(array[1]) && array[1].Length > 32)
            {
                error += "【身份证号不能超过32个字符】";
            }
            if (!string.IsNullOrEmpty(array[1]) && !OtherHelper.CheckIDCard(array[1]))
            {
                error += "【身份证号不规范】";
            }

            if (string.IsNullOrEmpty(array[3]))
            {
                error += "【手机号码不能为空】";
            }
            else if (!string.IsNullOrEmpty(array[3]) && array[3].Length > 16)
            {
                error += "【手机号码不能超过16个字符】";
            }
            return error;
        }
        #endregion

        #region 批量变更状态
        public AjaxResult GetUpdatesStatus()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                bool result = true;
                try
                {
                    string feeIdStr = Request.Form["IDStr"];
                    string status = Request.Form["Value"];
                    string[] feeIdArr = feeIdStr.Split(",");
                    for (int i = 0; i < feeIdArr.Length; i++)
                    {
                        iFeeModel fm = iFeeBLL.iFeeModelByWhere(" AND iFeeID=@iFeeID", new SqlParameter[] { new SqlParameter("@iFeeID", feeIdArr[i]) });
                        if (fm.Status.Equals(status))
                        {
                            return AjaxResult.Error("" + fm.VoucherNum + "变更状态和当前状态相同");
                        }
                        switch (status)
                        {
                            case "1":
                                fm.AffirmID = "0";
                                fm.AffirmTime = "1900-01-1";
                                break;
                            case "2":
                                fm.AffirmID = this.UserId.ToString();
                                fm.AffirmTime = DateTime.Now.ToString();
                                break;
                            case "9":
                                break;
                        }
                        fm.Status = status;
                        fm.UpdateID = this.UserId.ToString();
                        fm.UpdateTime = DateTime.Now.ToString();
                        iFeeBLL.UpdateiFee(fm);
                    }
                    ts.Complete();
                }
                catch
                {
                    result = false;
                    Transaction.Current.Rollback();
                }
                finally
                {
                    ts.Dispose();
                }
                if (result)
                {
                    return AjaxResult.Success();
                }
                else
                {
                    return AjaxResult.Error("出现未知错误，请联系管理员");
                }
            }
        }
        #endregion

        #region 重置打印次数
        public AjaxResult GetResetPrintNum()
        {
            string feeIdStr = Request.Form["IDStr"];
            string printNum = Request.Form["Value"];
            string[] feeIdArr = feeIdStr.Split(",");
            for (int i = 0; i < feeIdArr.Length; i++)
            {
                iFeeModel fm = iFeeBLL.iFeeModelByWhere(" AND iFeeID=@iFeeID", new SqlParameter[] { new SqlParameter("@iFeeID", feeIdArr[i]) });
                fm.PrintNum = printNum;
                fm.UpdateID = this.UserId.ToString();
                fm.UpdateTime = DateTime.Now.ToString();
                iFeeBLL.UpdateiFee(fm);
            }
            return AjaxResult.Success();
        }
        #endregion

        #region 导出杂费收费数据
        public AjaxResult DownloadiFee()
        {
            string menuId = Request.Form["MenuID"];
            string name = Request.Form["txtName"];
            string idCard = Request.Form["txtIDCard"];
            string voucherNum = Request.Form["txtVoucherNum"];
            string noteNum = Request.Form["txtNoteNum"];
            string createName = Request.Form["txtCreateName"];
            string dept = Request.Form["treeDept"];
            string feeMode = Request.Form["selFeeMode"];
            string status = Request.Form["selStatus"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string feeName = Request.Form["txtFeeName"];
            string personSort = Request.Form["selPersonSort"];
            string deptAreaId = Request.Form["selDeptAreaID"];
            string where = "";
            string enrollNum = Request.Form["EnrollNum"];
            if (!string.IsNullOrEmpty(enrollNum))
            {
                where += @" AND student.StudentID IN ( SELECT   se.StudentID
                                   FROM T_Stu_sEnroll se
                               WHERE    se.EnrollNum LIKE '%"+ enrollNum + "%' )";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND student.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " AND student.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " AND ifee.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " AND ifee.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(createName))
            {
                where += " AND sysuser.Name like '%" + createName + "%'";
            }
            if (!string.IsNullOrEmpty(dept))
            {
                where += "AND ifee.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(feeMode))
            {
                where += OtherHelper.MultiSelectToSQLWhere(feeMode, "ifee.FeeMode");
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "ifee.Status");
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where += " AND convert(NVARCHAR(10),ifee.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where += " AND convert(NVARCHAR(10),ifee.FeeTime,23) <= '" + feeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(feeName))
            {
                feeName = feeName.Substring(feeName.IndexOf(" ") + 1, feeName.Length - feeName.IndexOf(" ") - 1);
                where += " and ( detail.Name='" + feeName + "' or i.Name='" + feeName + "' )";
            }
            if (!string.IsNullOrEmpty(personSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(personSort, "ifee.PersonSort");
            }
            if (!string.IsNullOrEmpty(deptAreaId))
            {
                where += OtherHelper.MultiSelectToSQLWhere(deptAreaId, "ifee.DeptAreaID");
            }
            string cmdText = @"SELECT  ifee.VoucherNum 凭证号 ,
        dept.Name AS 单位 ,
        deptarea.Name AS 收费校区 ,
        student.Name 缴费人 ,
        student.IDCard 身份证号 ,
        (SELECT EnrollNum+',' FROM T_Stu_sEnroll WHERE StudentID=student.StudentID and Status=1  FOR XML PATH('')) 学号 ,
        ifee.NoteNum 票据号,
        ( SELECT    CASE im.Name
                      WHEN '杂费收费项' THEN ''
                      ELSE im.Name + '-'
                    END
          FROM      T_Pro_Item im
          WHERE     im.ItemID IN (
                    SELECT  ItemID
                    FROM    dbo.GetParentItemID(( SELECT    ItemID
                                                  FROM      T_Pro_Item
                                                  WHERE     DeptID = ifee.DeptID
                                                            AND ItemID = itemdetail.ItemID
                                                )) )
        FOR
          XML PATH('')
        ) 收费项目 ,
        detail.Name AS 收费类别 ,
        CONVERT(NVARCHAR(23), ifee.FeeTime, 23) AS 收费时间 ,
        refe1.RefeName AS 缴费方式 ,
        ifee.ShouldMoney 应缴金额 ,
        ifee.PaidMoney 实缴金额 ,
        ifee.DiscountMoney 优惠金额 ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Inc_iOffset
          WHERE     iFeeID = ifee.iFeeID
                    AND Status = 1
        ) AS 充抵金额 ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Inc_iOffset
          WHERE     ByiFeeID = ifee.iFeeID
                    AND Status = 1
        ) + ( SELECT    ISNULL(SUM(so.Money), 0)
              FROM      T_Stu_sOffset so
              WHERE     so.BySort = 2
                        AND so.ByRelatedID = ifee.iFeeID
                        AND so.Status = 1
            ) AS 被充抵金额 ,
        ( SELECT    ISNULL(SUM(RefundMoney), 0)
          FROM      T_Inc_iRefund
          WHERE     iFeeID = ifee.iFeeID
                    AND Status = 1
        ) AS 核销金额 ,
        sysuser.Name AS 收费人 ,
        refe2.RefeName AS 交款人 ,
        refe3.RefeName AS 状态 ,
        sysuser2.Name AS 结账人 ,
        CASE CONVERT(NVARCHAR(23), ifee.AffirmTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(23), ifee.AffirmTime, 23)
        END 结账时间 ,
        ifee.Explain 打印说明 ,
        ifee.Remark 系统备注
FROM    T_Inc_iFee AS ifee
        LEFT JOIN T_Pro_Student AS student ON ifee.StudentID = student.StudentID
        LEFT JOIN T_Sys_Dept AS dept ON ifee.DeptID = dept.DeptID
        LEFT JOIN T_Pro_ItemDetail AS itemdetail ON ifee.ItemDetailID = itemdetail.ItemDetailID
		LEFT JOIN T_Pro_Item i ON i.ItemID=itemdetail.ItemID
        LEFT JOIN T_Pro_Detail AS detail ON itemdetail.DetailID = detail.DetailID
        LEFT JOIN T_Sys_Refe AS refe1 ON ifee.FeeMode = refe1.Value
                                         AND refe1.RefeTypeID = 6
        LEFT JOIN T_Sys_User AS sysuser ON ifee.CreateID = sysuser.UserID
        LEFT JOIN T_Sys_Refe AS refe2 ON ifee.PersonSort = refe2.Value
                                         AND refe2.RefeTypeID = 11
        LEFT JOIN T_Sys_Refe AS refe3 ON ifee.Status = refe3.Value
                                         AND refe3.RefeTypeID = 7
        LEFT JOIN T_Sys_User AS sysuser2 ON ifee.AffirmID = sysuser2.UserID
        LEFT JOIN T_Pro_DeptArea AS deptarea ON ifee.DeptAreaID = deptarea.DeptAreaID
        WHERE   1 = 1 {0} ORDER BY ifee.iFeeID DESC";
            string filename = "杂费收费信息.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "ifee.DeptID", "ifee.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }
        #endregion

        #region 生成凭证号
        public string getVoucherNum()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string voucherNum = ConfigBLL.UpdateConfigToVoucherNum("2");//2、杂费费用 与T_Pro_Item表Sort字段相同编号
            string zero = "";
            switch (voucherNum.Length)
            {
                case 1:
                    zero = "00000";
                    break;
                case 2:
                    zero = "0000";
                    break;
                case 3:
                    zero = "000";
                    break;
                case 4:
                    zero = "00";
                    break;
                case 5:
                    zero = "0";
                    break;
                case 6:
                    zero = "";
                    break;
            }
            string vouchernum = "Z" + date + zero + voucherNum;
            return vouchernum;
        }
        #endregion

        #region 输出打印信息，验证打印次数
        /// <summary>
        /// 输出打印信息，验证打印次数
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetPrintInfo()
        {
            string iFeeId = Request.Form["ID"];

            string where = " AND iFeeID = @iFeeID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@iFeeID", iFeeId)
            };
            iFeeModel f = iFeeBLL.iFeeModelByWhere(where, paras);

            ConfigModel c = ConfigBLL.ConfigModelByWhere(" AND ConfigID = 2", null);//2、杂费 与T_Pro_Item表Sort字段相同编号

            if (Convert.ToInt32(c.PrintNum) > Convert.ToInt32(f.PrintNum))
            {
                DataTable dt = iFeeBLL.iFeeTablePrintByWhere(" AND f.iFeeID = @iFeeID", paras);
                return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
            }
            else
            {
                return AjaxResult.Error(f.VoucherNum + "打印次数超过 " + c.PrintNum + " 次！");
            }
        }
        #endregion

        #region 输出打印信息，不验证打印次数
        /// <summary>
        /// 输出打印信息，不验证打印次数
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetPrintInfoNoPrintNum()
        {
            string iFeeId = Request.Form["ID"];

            string where = " AND iFeeID = @iFeeID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@iFeeID", iFeeId)
            };
            iFeeModel f = iFeeBLL.iFeeModelByWhere(where, paras);

            DataTable dt = iFeeBLL.iFeeTablePrintByWhere(" AND f.iFeeID = @iFeeID", paras);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }
        #endregion

        #region 更新票据号
        public AjaxResult UpdateNoteNum()
        {
            string iFeeId = Request.Form["iFeeID"];

            iFeeModel ifm = iFeeBLL.GetiFeeModel(iFeeId);

            if (!string.IsNullOrEmpty(ifm.NoteNum))
            {
                iDisableNoteModel idnm = new iDisableNoteModel();
                idnm.Status = "1";
                idnm.iFeeID = ifm.iFeeID;
                idnm.NoteNum = ifm.NoteNum;
                idnm.CreateID = this.UserId.ToString();
                idnm.CreateTime = DateTime.Now.ToString();
                idnm.UpdateID = this.UserId.ToString();
                idnm.UpdateTime = DateTime.Now.ToString();
                iDisableNoteBLL.InsertiDisableNote(idnm);
            }

            string printNumStr = ConfigBLL.getNoteNum("2", "2");
            ifm.NoteNum = printNumStr;
            ifm.PrintNum = (Convert.ToInt32(ifm.PrintNum) + 1).ToString();

            iFeeBLL.UpdateiFee(ifm);

            return AjaxResult.Success(printNumStr);
        }
        #endregion

        #region 得到可用金额
        public static decimal GetCanMoney(string iFeeID)
        {
            string cmdText = @"SELECT  PaidMoney
FROM    T_Inc_iFee
WHERE   iFeeID = @iFeeID";
            SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@iFeeID", iFeeID) };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(dt.Rows[0][0].ToString()) + GetOffMoney(iFeeID) - GetRefundMoney(iFeeID) - GetBeOffMoney(iFeeID);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 得到费用项的冲抵金额
        public static decimal GetOffMoney(string iFeeID)
        {
            string cmdText = @"SELECT  ISNULL(SUM(Money), 0)
FROM    T_Inc_iOffset
WHERE   iFeeID = @iFeeID
        AND Status = 1";
            SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@iFeeID", iFeeID) };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(dt.Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 得到费用项的被冲抵金额
        public static decimal GetBeOffMoney(string iFeeID)
        {
            string cmdText = @"SELECT  ISNULL(SUM(Money), 0)
FROM    T_Inc_iOffset
WHERE   ByiFeeID = @iFeeID
        AND Status = 1";
            SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@iFeeID", iFeeID) };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(dt.Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 得到费用项的核销金额
        public static decimal GetRefundMoney(string iFeeID)
        {
            string cmdText = @"SELECT  ISNULL(SUM(RefundMoney), 0)
FROM    T_Inc_iRefund
WHERE   iFeeID = @iFeeID
        AND Status = 1";
            SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@iFeeID", iFeeID) };
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text, paras).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(dt.Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 修改费用项的可用金额
        public static void UpdateCanMoney(string iFeeID)
        {
            iFeeModel model = iFeeBLL.iFeeModelByWhere(" AND iFeeID = @iFeeID", new SqlParameter[] { new SqlParameter("@iFeeID", iFeeID) });
            model.CanMoney = GetCanMoney(iFeeID).ToString();
            iFeeBLL.UpdateiFee(model);
        }
        #endregion

        /// <summary>
        /// 获取excel
        /// </summary>
        /// <param name="ID">文件名</param>
        /// <returns></returns>
        public AjaxResult GetExcelData(string ID)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = OtherHelper.ExcelToDataTable(ID);
            }
            catch (Exception)
            {

                return AjaxResult.Error("模板错误");
            }

            try
            {
                if (!dt.Columns[0].ColumnName.Trim().Equals("姓名") || !dt.Columns[1].ColumnName.Trim().Equals("身份证号") || !dt.Columns[2].ColumnName.Trim().Equals("学号") || !dt.Columns[3].ColumnName.Trim().Equals("性别") || !dt.Columns[4].ColumnName.Trim().Equals("手机"))
                {
                    return AjaxResult.Error("模板错误");
                }
            }
            catch (Exception)
            {

                return AjaxResult.Error("模板错误");
            }
            return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "success");
        }
        /// <summary>
        /// 导入杂费
        /// </summary>
        /// <param name="ifm"></param>
        /// <returns></returns>
        public AjaxResult UploadiFee(UploadiFeeFormMode ifm)
        {
            try
            {
                if (string.IsNullOrEmpty(ifm.DeptID))
                {
                    return AjaxResult.Error("请选择收费单位");
                }
                if (string.IsNullOrEmpty(ifm.DeptAreaID))
                {
                    return AjaxResult.Error("请选择收费校区");
                }
                if (string.IsNullOrEmpty(ifm.PersonSort))
                {
                    return AjaxResult.Error("请选择交款人员");
                }
                if (string.IsNullOrEmpty(ifm.filePath))
                {
                    return AjaxResult.Error("请选择人员文件");
                }
                if (string.IsNullOrEmpty(ifm.FeeTime))
                {
                    return AjaxResult.Error("收费时间不能为空");
                }
                if (string.IsNullOrEmpty(ifm.ItemDetailID))
                {
                    return AjaxResult.Error("请选择收费项目");
                }
                ifm.ItemDetailID = ifm.ItemDetailID.Substring(ifm.ItemDetailID.IndexOf("_") + 1, ifm.ItemDetailID.Length - ifm.ItemDetailID.IndexOf("_") - 1);
                if (!OtherHelper.IsInt(ifm.ItemDetailID) || !ItemDetailBLL.ValideteItemDetailID(ifm.DeptID, "2", ifm.ItemDetailID))
                {
                    return AjaxResult.Error("收费项目选择错误");
                }
                if (string.IsNullOrEmpty(ifm.FeeMode))
                {
                    return AjaxResult.Error("请选择缴费方式");
                }
                if (ifm.ShouldMoney == 0)
                {
                    return AjaxResult.Error("应收金额不能为0");
                }
                if (ifm.PaidMoney == 0)
                {
                    return AjaxResult.Error("实收金额不能为0");
                }
                DataTable itemData = ItemDetailBLL.ItemDetailGetMoneyAll(ifm.ItemDetailID);
                //非固定金额，需判断应收金额是否大于等于非固定金额
                if (itemData.Rows.Count > 0 && itemData.Rows[0]["Sort"].ToString() == "2")
                {
                    if (ifm.ShouldMoney < Convert.ToDecimal(itemData.Rows[0]["Money"].ToString()))
                    {
                        return AjaxResult.Error("应收金额必选大于等于" + itemData.Rows[0]["Money"].ToString() + "元");
                    }
                }
                if (ifm.ShouldMoney != ifm.PaidMoney + ifm.DiscountMoney)
                {
                    return AjaxResult.Error("应收金额必须等于实收金额+优惠金额");
                }
                if (!string.IsNullOrEmpty(ifm.Explain))
                {
                    ifm.Explain = ifm.Explain.Replace("\r\n", "<br />");
                }
                if (!string.IsNullOrEmpty(ifm.Remark))
                {
                    ifm.Remark = ifm.Remark.Replace("\r\n", "<br />");
                }

                List<iFeeExcelModel> list = JsonConvert.DeserializeObject<List<iFeeExcelModel>>(ifm.Paras);

                DataTable dt = new DataTable();
                dt = TableTitle(dt);
                if (list != null)
                {
                    int errorNum = 0;
                    int successNum = 0;
                    foreach (var item in list)
                    {
                        string studentId = "";
                        string errorString = "";
                        if (!string.IsNullOrEmpty(item.身份证号) || !string.IsNullOrEmpty(item.学号))
                        {
                            if (!string.IsNullOrEmpty(item.身份证号) && string.IsNullOrEmpty(item.学号.Replace(" ", "")))//身份证不为空，学号为空
                            {
                                if (!OtherHelper.CheckIDCard(item.身份证号.Trim()))
                                {
                                    errorString += "身份证号不规范;";
                                }
                                else
                                {
                                    string name = StudentBLL.GetStudentModel(item.身份证号.Trim()).Name;
                                    if (!string.IsNullOrEmpty(name))
                                    {
                                        studentId = StudentBLL.GetStudentModel(item.姓名.Replace(" ", ""), item.身份证号.Trim()).StudentID;
                                        if (string.IsNullOrEmpty(studentId))
                                        {
                                            errorString += "身份证号和姓名不匹配;";
                                        }
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(item.学号.Replace(" ", "")) && string.IsNullOrEmpty(item.身份证号.Replace(" ", "")))
                            {
                                DataTable tempdt = ValidateEnrollNum(item.学号.Replace(" ", ""), ifm.DeptID, item.姓名.Replace(" ", ""));

                                if (tempdt.Rows.Count > 0)
                                {
                                    studentId = tempdt.Rows[0]["StudentID"].ToString();
                                }
                                else
                                {
                                    errorString += "学号和姓名不匹配;";
                                }

                            }
                            if (!string.IsNullOrEmpty(item.身份证号.Replace(" ", "")) && !string.IsNullOrEmpty(item.学号.Replace(" ", "")))
                            {
                                DataTable tempdt = ValidateEnrollNumAndIDCard(item.学号.Replace(" ", ""), ifm.DeptID, item.姓名.Replace(" ", ""), item.身份证号.Trim());
                                if (tempdt.Rows.Count > 0)
                                {
                                    studentId = tempdt.Rows[0]["StudentID"].ToString();
                                }
                                else
                                {
                                    if (!OtherHelper.CheckIDCard(item.身份证号.Trim()))
                                    {
                                        errorString += "身份证号不规范;";
                                    }
                                    string name = StudentBLL.GetStudentModel(item.身份证号.Trim()).Name;

                                    if (!string.IsNullOrEmpty(name) || StudentBLL.ValidateEnrollNum(item.学号, ifm.DeptID))
                                    {
                                        if (string.IsNullOrEmpty(StudentBLL.GetStudentModel(item.姓名.Replace(" ", ""), item.身份证号.Trim()).StudentID))
                                        {
                                            errorString += "身份证号和姓名不匹配;";
                                        }
                                        DataTable tempdt2 = ValidateEnrollNum(item.学号.Replace(" ", ""), ifm.DeptID, item.姓名.Replace(" ", ""));
                                        if (tempdt2.Rows.Count == 0)
                                        {
                                            errorString += "学号和姓名不匹配;";
                                        }
                                    }
                                    if (string.IsNullOrEmpty(errorString))
                                    {
                                        errorString += "姓名/身份证号/学号不匹配;";//同名学号不同
                                    }
                                }
                            }
                        }
                        else
                        {
                            errorString += "身份证号和学号不能同时为空;";
                        }

                        if (string.IsNullOrEmpty(item.姓名))
                        {
                            errorString += "姓名不能为空;";
                        }
                        else
                        {
                            if (item.姓名.Length > 16)
                            {
                                errorString += "姓名不能超过16个字符;";
                            }
                        }
                        if (string.IsNullOrEmpty(item.手机))
                        {
                            errorString += "手机不能为空;";
                        }
                        else
                        {
                            if (item.手机.Length > 16)
                            {
                                errorString += "手机不能超过16个字符;";
                            }
                        }

                        if (RefeBLL.GetRefeValue(item.性别.Trim(), "3").Equals("-1"))
                        {
                            errorString += "性别必须为男/女;";
                        }

                        if (string.IsNullOrEmpty(errorString) && string.IsNullOrEmpty(studentId))
                        {
                            if (!OtherHelper.CheckIDCard(item.身份证号.Trim()))
                            {
                                errorString += "身份证号不规范;";
                            }
                            string name = StudentBLL.GetStudentModel(item.身份证号.Trim()).Name;
                            if (!string.IsNullOrEmpty(name))
                            {
                                errorString += "身份证号不能重复;";
                            }
                        }
                        if (string.IsNullOrEmpty(errorString))
                        {
                            StudentModel sm = new StudentModel();
                            if (string.IsNullOrEmpty(studentId))
                            {
                                sm.QQ = "";
                                sm.IDCard = item.身份证号.Trim();
                                sm.DeptID = ifm.DeptID;
                                sm.CreateID = this.UserId.ToString();
                                sm.CreateTime = DateTime.Now.ToString();
                                sm.Address = "";
                                sm.Mobile = item.手机.Replace(" ", "");
                                sm.Name = item.姓名.Replace(" ", "");
                                sm.Remark = "";
                                sm.Sex = RefeBLL.GetRefeValue(item.性别.Trim(), "3");
                                sm.Status = "1";
                                sm.UpdateID = this.UserId.ToString();
                                sm.UpdateTime = DateTime.Now.ToString();
                                sm.WeChat = "";
                                sm.StudentID = StudentBLL.InsertStudent(sm).ToString();
                                if (!string.IsNullOrEmpty(item.学号.Replace(" ", "")))
                                {
                                    sEnrollModel em = new sEnrollModel();
                                    em.CreateID = UserId.ToString();
                                    em.CreateTime = DateTime.Now.ToString();
                                    em.DeptID = ifm.DeptID;
                                    em.EnrollNum = item.学号.Replace(" ", "");
                                    em.Status = "1";
                                    em.StudentID = sm.StudentID;
                                    em.UpdateID = UserId.ToString();
                                    em.UpdateTime = DateTime.Now.ToString();
                                    sEnrollBLL.InsertsEnroll(em);
                                }

                            }
                            else
                            {
                                sm.StudentID = studentId;
                            }
                            iFeeModel fm = new iFeeModel();
                            fm.AffirmID = "0";
                            fm.AffirmTime = "1900-01-01";
                            fm.CanMoney = ifm.PaidMoney.ToString();
                            fm.CreateID = this.UserId.ToString();
                            fm.CreateTime = DateTime.Now.ToString();
                            fm.DeptAreaID = ifm.DeptAreaID;
                            fm.DeptID = ifm.DeptID;
                            fm.DiscountMoney = ifm.DiscountMoney.ToString();
                            fm.Explain = ifm.Explain;
                            fm.FeeMode = ifm.FeeMode;
                            fm.FeeTime = ifm.FeeTime;
                            fm.ItemDetailID = ifm.ItemDetailID;
                            fm.NoteNum = "";
                            fm.PaidMoney = ifm.PaidMoney.ToString();
                            fm.PersonSort = ifm.PersonSort;
                            fm.PrintNum = "0";
                            fm.Remark = ifm.Remark;
                            fm.ShouldMoney = ifm.ShouldMoney.ToString();
                            fm.Status = "1";
                            fm.StudentID = sm.StudentID;
                            fm.UpdateID = this.UserId.ToString();
                            fm.UpdateTime = DateTime.Now.ToString();
                            fm.VoucherNum = ConfigBLL.getVoucherNum("2", "Z");
                            iFeeBLL.InsertiFee(fm);
                            successNum++;

                        }
                        else
                        {
                            errorNum++;
                            dt = TableRows(dt, item, errorString);
                        }

                    }

                    string url = "";
                    //保存错误文件
                    if (dt.Rows.Count > 0)
                    {
                        string FileName = OtherHelper.FilePathAndName();
                        OtherHelper.DeriveToExcel(dt, FileName);
                        url = "../Temp/" + FileName + "";
                    }
                    NoteModel nm = new NoteModel();
                    nm.CreateID = this.UserId.ToString();
                    nm.CreateTime = DateTime.Now.ToString();
                    nm.InFile = ifm.filePath;
                    nm.OutFile = url;
                    nm.Sort = "4";
                    nm.DeptID = ifm.DeptID;
                    nm.Status = "1";
                    nm.SuccessNum = successNum.ToString();
                    nm.ErrorNum = errorNum.ToString();
                    NoteBLL.InsertNote(nm);
                    return AjaxResult.Success(JsonHelper.DataTableToJson(dt), "操作成功");

                }
                else
                {
                    return AjaxResult.Error("操作失败，数据不能为空");
                }
            }
            catch
            {
                return AjaxResult.Error("未知错误，请联系管理员");
            }
        }

        /// <summary>
        /// 合并打印杂费
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AjaxResult ValidateiFee(string ID)
        {
            string printNumStr = "";
            List<iFeeValidateModel> list = GetiFeeList(ID);
            if (list == null)
            {
                return AjaxResult.Error("数据不能为空");
            }
            else
            {
                int printNum = ConfigBLL.GetConfigPrintNum("2");
                var printNumList = list.FirstOrDefault(O => O.printNum >= printNum);
                if (printNumList != null)
                {
                    return AjaxResult.Error(printNumList.voucherNum + "打印次数超过" + printNum.ToString() + "");
                }
                var firstList = list.FirstOrDefault();
                var otherList = list.FirstOrDefault(O => !O.deptId.Equals(firstList.deptId) || !O.feeTime.Equals(firstList.feeTime) || !O.feeMode.Equals(firstList.feeMode) || !O.itemDetailId.Equals(firstList.itemDetailId) || !O.shouldMoney.Equals(firstList.shouldMoney) || !O.paidMoney.Equals(firstList.paidMoney) || !O.discountMoney.Equals(firstList.discountMoney) || !O.canMoney.Equals(firstList.canMoney));
                if (otherList != null)
                {
                    return AjaxResult.Error(otherList.voucherNum + "数据异常请检查所选的收费数据是否符合合并打印条件。");
                }
                var offsetList = list.FirstOrDefault(O => O.byOffsetNum > 0 || O.offsetNum > 0);
                if (offsetList != null)
                {
                    return AjaxResult.Error(offsetList.voucherNum + "有充抵或被冲抵记录，不能合并打印");
                }
                var refundList = list.FirstOrDefault(O => O.RefundNum > 0);
                if (refundList != null)
                {
                    return AjaxResult.Error(refundList.voucherNum + "已经被核销，不能使用此打印");
                }
                printNumStr = ConfigBLL.getNoteNum("2", "2");

                foreach (var item in list)
                {
                    iFeeModel fm = iFeeBLL.iFeeModelByWhere(" and iFeeID=@iFeeID", new SqlParameter[] { new SqlParameter("@iFeeID", item.iFeeId) });
                    if (!string.IsNullOrEmpty(fm.NoteNum))
                    {
                        iDisableNoteModel dnm = new iDisableNoteModel();
                        dnm.Status = "1";
                        dnm.iFeeID = fm.iFeeID;
                        dnm.NoteNum = fm.NoteNum;
                        dnm.CreateID = this.UserId.ToString();
                        dnm.CreateTime = DateTime.Now.ToString();
                        dnm.UpdateID = this.UserId.ToString();
                        dnm.UpdateTime = DateTime.Now.ToString();
                        iDisableNoteBLL.InsertiDisableNote(dnm);
                    }
                    fm.NoteNum = printNumStr;
                    fm.PrintNum = (Convert.ToInt32(fm.PrintNum) + 1).ToString();
                    iFeeBLL.UpdateiFee(fm);
                }
                DataTable feedt = iFeeBLL.GetiFeePrintContent(ID);
                return AjaxResult.Success(JsonHelper.DataTableToJson(feedt), printNumStr);
            }

        }

        private List<iFeeValidateModel> GetiFeeList(string ifeeString)
        {
            if (string.IsNullOrEmpty(ifeeString))
            {
                return null;
            }
            DataTable dt = iFeeBLL.SelectiFeeByiFeeId(ifeeString);
            List<iFeeValidateModel> list = new List<iFeeValidateModel>();
            foreach (DataRow dr in dt.Rows)
            {
                iFeeValidateModel fm = new iFeeValidateModel();
                fm.byOffsetNum = int.Parse(dr["ByOffsetNum"].ToString());
                fm.canMoney = decimal.Parse(dr["CanMoney"].ToString());
                fm.deptId = dr["DeptID"].ToString();
                fm.discountMoney = decimal.Parse(dr["DiscountMoney"].ToString());
                fm.feeMode = dr["FeeMode"].ToString();
                fm.voucherNum = dr["VoucherNum"].ToString();
                fm.itemDetailId = dr["ItemDetailID"].ToString();
                fm.shouldMoney = decimal.Parse(dr["ShouldMoney"].ToString());
                fm.paidMoney = decimal.Parse(dr["PaidMoney"].ToString());
                fm.offsetNum = int.Parse(dr["OffsetNum"].ToString());
                fm.iFeeId = dr["iFeeID"].ToString();
                fm.feeTime = Convert.ToDateTime(dr["FeeTime"].ToString()).ToString("yyyy-MM-dd");
                fm.printNum = int.Parse(dr["PrintNum"].ToString());
                fm.RefundNum= int.Parse(dr["RefundNum"].ToString());
                list.Add(fm);
            }
            return list;
        }

        private DataTable ValidateEnrollNum(string enrollNum, string deptId, string name)
        {
            string where = " and e.EnrollNum=@EnrollNum and e.DeptID=@DeptID and s.Name=@Name ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@EnrollNum",enrollNum),
                new SqlParameter("@DeptID",deptId),
                new SqlParameter("@Name",name)
            };
            return StudentBLL.StudentTableByWhere(where, paras);
        }

        private DataTable ValidateEnrollNumAndIDCard(string enrollNum, string deptId, string name, string idCard)
        {
            string where = " and e.EnrollNum=@EnrollNum and e.DeptID=@DeptID and s.Name=@Name and s.IDCard=@IDCard ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@EnrollNum",enrollNum),
                new SqlParameter("@DeptID",deptId),
                new SqlParameter("@Name",name),
                new SqlParameter("@IDCard",idCard)
            };
            return StudentBLL.StudentTableByWhere(where, paras);
        }


        public AjaxResult ValidatePrintByStudent(string ID) {
            string printNumStr = "";
            List<iFeeValidateModel> list = GetiFeeList(ID);
            if (list == null)
            {
                return AjaxResult.Error("数据不能为空");
            }
            else
            {
                int printNum = ConfigBLL.GetConfigPrintNum("2");
                var printNumList = list.FirstOrDefault(O => O.printNum >= printNum);
                if (printNumList != null)
                {
                    return AjaxResult.Error(printNumList.voucherNum + "打印次数超过" + printNum.ToString() + "");
                }
                var firstList = list.FirstOrDefault();
                var otherList = list.FirstOrDefault(O => !O.deptId.Equals(firstList.deptId) || !O.feeTime.Equals(firstList.feeTime));
                if (otherList != null)
                {
                    return AjaxResult.Error(otherList.voucherNum + "数据异常请检查所选的收费数据是否符合合并打印条件。");
                }
                var offsetList = list.FirstOrDefault(O => O.byOffsetNum > 0 || O.offsetNum > 0);
                if (offsetList != null)
                {
                    return AjaxResult.Error(offsetList.voucherNum + "有充抵或被冲抵记录，不能使用此打印");
                }
                var refundList= list.FirstOrDefault(O => O.RefundNum > 0 );
                if (refundList!=null)
                {
                    return AjaxResult.Error(refundList.voucherNum + "已经被核销，不能使用此打印");
                }
                printNumStr = ConfigBLL.getNoteNum("2", "2");

                foreach (var item in list)
                {
                    iFeeModel fm = iFeeBLL.iFeeModelByWhere(" and iFeeID=@iFeeID", new SqlParameter[] { new SqlParameter("@iFeeID", item.iFeeId) });
                    if (!string.IsNullOrEmpty(fm.NoteNum))
                    {
                        iDisableNoteModel dnm = new iDisableNoteModel();
                        dnm.Status = "1";
                        dnm.iFeeID = fm.iFeeID;
                        dnm.NoteNum = fm.NoteNum;
                        dnm.CreateID = this.UserId.ToString();
                        dnm.CreateTime = DateTime.Now.ToString();
                        dnm.UpdateID = this.UserId.ToString();
                        dnm.UpdateTime = DateTime.Now.ToString();
                        iDisableNoteBLL.InsertiDisableNote(dnm);
                    }
                    fm.NoteNum = printNumStr;
                    fm.PrintNum = (Convert.ToInt32(fm.PrintNum) + 1).ToString();
                    iFeeBLL.UpdateiFee(fm);
                }
                DataTable feedt = iFeeBLL.GetiFeePrintByStudentContent(ID);
                return AjaxResult.Success(JsonHelper.DataTableToJson(feedt), printNumStr);
            }
        }
    }
}
