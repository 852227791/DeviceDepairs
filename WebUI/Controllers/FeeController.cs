using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Model;
using BLL;
using Common;
using DAL;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Transactions;

namespace WebUI.Controllers
{
    public class FeeController : BaseController
    {
        //
        // GET: /Fee/
        #region 页面加载
        public ActionResult FeeList()
        {
            return View();
        }


        public ActionResult PrintFee()
        {
            return View();
        }
        public ActionResult FeeDiscountEdit()
        {
            return View();
        }
        public ActionResult FeeEdit()
        {
            return View();
        }
        public ActionResult FeeView()
        {
            return View();
        }
        public ActionResult ChooseFeeList()
        {
            return View();
        }
        public ActionResult ChooseFee()
        {
            return View();
        }
        public ActionResult FeeUp()
        {
            return View();
        }
        #endregion

        #region  收费信息列表
        public ActionResult GetFeeList()
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
            string proveName = Request.Form["txtProveName"];
            string proveId = Request.Form["txtProveID"];
            string personSort = Request.Form["selPersonSort"];
            string className = Request.Form["txtClass"];
            string where = "";
            if (!string.IsNullOrEmpty(className))
            {
                where += " AND c.Name like '%" + className + "%'";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " AND s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " AND f.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " AND f.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(createName))
            {
                where += " AND u2.Name like '%" + createName + "%'";
            }
            if (!string.IsNullOrEmpty(dept))
            {
                where += "AND f.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(feeMode))
            {
                where += OtherHelper.MultiSelectToSQLWhere(feeMode, "f.FeeMode");
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "f.Status");
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where += " AND convert(NVARCHAR(10),f.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where += " AND convert(NVARCHAR(10),f.FeeTime,23) <= '" + feeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(proveName))
            {
                where += " AND i.Name like '%" + proveName + "%'";
            }
            if (!string.IsNullOrEmpty(proveId))
            {
                where += " AND f.ProveID = " + proveId + "";
            }
            if (!string.IsNullOrEmpty(personSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(personSort, "f.PersonSort");
            }
            string filed = "FeeID";
            string cmdText = @"SELECT  f.FeeID ,
        f.VoucherNum ,
        d.Name Dept ,
        s.Name ,
        s.IDCard ,
        f.NoteNum ,
        r1.RefeName Status ,
        f.Status StatusValue ,
        i.Name ItemName ,
        ( SELECT    dl.Name + ','
          FROM      T_Pro_FeeDetail fd
                    LEFT JOIN T_Pro_ItemDetail id ON fd.ItemDetailID = id.ItemDetailID
                    LEFT JOIN T_Pro_Detail dl ON id.DetailID = dl.DetailID
          WHERE     fd.Status = 1
                    AND fd.FeeID = f.FeeID
        FOR
          XML PATH('')
        ) FeeContent ,
        r2.RefeName FeeMode ,
        f.ShouldMoney ,
        f.PaidMoney ,
        ( SELECT    ISNULL(SUM(DiscountMoney), 0)
          FROM      T_Pro_FeeDetail
          WHERE     Status = 1
                    AND FeeID = f.FeeID
        ) DiscountMoney ,
        u1.Name Affirm ,
        CONVERT(NVARCHAR(23), f.FeeTime, 23) FeeTime ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = o.ByFeeDetailID
          WHERE     fd.FeeID = f.FeeID
                    AND fd.Status = 1
                    AND o.Status = 1
        )
        + ( SELECT  ISNULL(SUM(so.Money), 0)
            FROM    T_Stu_sOffset so
                    LEFT JOIN T_Pro_FeeDetail fd ON so.ByRelatedID = fd.FeeDetailID
            WHERE   so.BySort = 3
                    AND fd.FeeID = f.FeeID
                    AND fd.Status = 1
                    AND so.Status = 1
          ) BeOffsetMoney ,
        ( SELECT    t + ','
          FROM      ( SELECT    fee.VoucherNum + '_' + s.Name + '_'
                                + CONVERT(NVARCHAR(10), offs.Money) t
                      FROM      T_Pro_FeeDetail feed
                                LEFT JOIN T_Pro_Offset offs ON feed.FeeDetailID = offs.FeeDetailID
                                LEFT JOIN T_Pro_Fee fee ON feed.FeeID = fee.FeeID
                                LEFT JOIN T_Pro_Prove p ON fee.ProveID = p.ProveID
                                LEFT JOIN T_Pro_Student s ON p.StudentID = s.StudentID
                      WHERE     feed.Status = 1
                                AND offs.Status = 1
                                AND offs.ByFeeDetailID IN (
                                SELECT  FeeDetailID
                                FROM    T_Pro_FeeDetail
                                WHERE   Status = 1
                                        AND FeeID = f.FeeID )
                      UNION ALL
                      SELECT    sf.VoucherNum + '_' + s.Name + '_'
                                + CONVERT(NVARCHAR(10), so.Money)
                      FROM      T_Stu_sOffset so
                                LEFT JOIN T_Stu_sFeesOrder sfo ON so.RelatedID = sfo.sFeesOrderID
                                LEFT JOIN T_Stu_sFee sf ON sfo.sFeeID = sf.sFeeID
                                LEFT JOIN T_Stu_sEnrollsProfession ep ON sf.sEnrollsProfessionID = ep.sEnrollsProfessionID
                                LEFT JOIN T_Stu_sEnroll se ON ep.sEnrollID = se.sEnrollID
                                LEFT JOIN T_Pro_Student s ON se.StudentID = s.StudentID
                      WHERE     so.BySort = 3
                                AND so.Status = 1
                                AND so.ByRelatedID IN (
                                SELECT  FeeDetailID
                                FROM    T_Pro_FeeDetail
                                WHERE   Status = 1
                                        AND FeeID = f.FeeID )
                    ) AS byTemp
        FOR
          XML PATH('')
        ) BeOffsetString ,
        ( SELECT    fee.VoucherNum + '_' + s.Name + '_'
                    + CONVERT(NVARCHAR(10), offs.Money) + ','
          FROM      T_Pro_FeeDetail feed
                    LEFT JOIN T_Pro_Offset offs ON feed.FeeDetailID = offs.ByFeeDetailID
                    LEFT JOIN T_Pro_Fee fee ON feed.FeeID = fee.FeeID
                    LEFT JOIN T_Pro_Prove p ON fee.ProveID = p.ProveID
                    LEFT JOIN T_Pro_Student s ON p.StudentID = s.StudentID
          WHERE     feed.Status = 1
                    AND offs.FeeDetailID IN ( SELECT    FeeDetailID
                                              FROM      T_Pro_FeeDetail
                                              WHERE     Status = 1
                                                        AND FeeID = f.FeeID )
        FOR
          XML PATH('')
        ) OffsetString ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = o.FeeDetailID
          WHERE     fd.FeeID = f.FeeID
                    AND fd.Status = 1
                    AND o.Status = 1
        ) OffsetMoney ,
        ( SELECT    ISNULL(SUM(RefundMoney), 0)
          FROM      T_Pro_Refund r
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = r.FeeDetailID
          WHERE     r.Status = 1
                    AND fd.Status = 1
                    AND fd.FeeID = f.FeeID
        ) RefundMoney ,
        CASE CONVERT(NVARCHAR(23), f.AffirmTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(23), f.AffirmTime, 23)
        END AffirmTime ,
        u2.Name CreateName ,
        c.Name ClassName ,
        pro.Name ProName ,
        r3.RefeName PersonSort ,
        f.Teacher ,
        ( SELECT    EnrollNum + ','
          FROM      T_Stu_sEnroll
          WHERE     StudentID = s.StudentID
                    AND Status = 1
        FOR
          XML PATH('')
        ) EnrollNum
FROM    T_Pro_Fee AS f
        LEFT JOIN T_Pro_Prove AS p ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
        LEFT JOIN T_Pro_Class c ON c.ClassID = p.ClassID
        LEFT JOIN T_Pro_Profession pro ON pro.ProfessionID = c.ProfessionID
        LEFT JOIN T_Sys_Dept AS d ON d.DeptID = f.DeptID
        LEFT JOIN T_Pro_Item AS i ON i.ItemID = p.ItemID
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = f.Status
                                      AND r1.RefeTypeID = 7
        LEFT JOIN T_Sys_Refe AS r2 ON r2.Value = f.FeeMode
                                      AND r2.RefeTypeID = 6
        LEFT JOIN T_Sys_User u1 ON u1.UserID = f.AffirmID
        LEFT JOIN T_Sys_User u2 ON u2.UserID = f.CreateID
        LEFT JOIN T_Sys_Refe r3 ON f.PersonSort = r3.Value
                                   AND r3.RefeTypeID = 11
WHERE   1 = 1  {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "f.DeptID", "f.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, filed, Request.Form, "ShouldMoney,PaidMoney,DiscountMoney,OffsetMoney,BeOffsetMoney,RefundMoney"));
        }
        #endregion

        #region 变更状态
        //public AjaxResult GetUpdateStatus()
        //{
        //    string feeId = Request.Form["ID"];
        //    string status = Request.Form["Value"];
        //    FeeModel fm = FeeBLL.GetFeeModel(feeId);
        //    switch (status)
        //    {
        //        case "1":
        //            fm.AffirmID = "0";
        //            fm.AffirmTime = "1900-01-1";
        //            break;
        //        case "2":
        //            fm.AffirmID = this.UserId.ToString();
        //            fm.AffirmTime = DateTime.Now.ToString();
        //            break;
        //        case "9":
        //            break;
        //    }
        //    fm.Status = status;
        //    fm.UpdateID = this.UserId.ToString();
        //    fm.UpdateTime = DateTime.Now.ToString();
        //    if (FeeBLL.UpdateFee(fm) > 0)
        //    {
        //        return AjaxResult.Success();
        //    }
        //    else
        //    {
        //        return AjaxResult.Error("出现未知错误，请联系管理员");
        //    }
        //}
        #endregion

        #region 批量变更状态`
        public AjaxResult GetUpdatesStatus()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    string feeIdStr = Request.Form["IDStr"];
                    string status = Request.Form["Value"];
                    string[] feeIdArr = feeIdStr.Split(",");
                    for (int i = 0; i < feeIdArr.Length; i++)
                    {
                        FeeModel fm = FeeBLL.GetFeeModel(feeIdArr[i]);
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
                        FeeBLL.UpdateFee(fm);
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    return AjaxResult.Error(ex.Message);
                }
                finally
                {
                    ts.Dispose();
                }

            }
            return AjaxResult.Success();
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
                FeeModel fm = FeeBLL.GetFeeModel(feeIdArr[i]);
                fm.PrintNum = printNum;
                fm.UpdateID = this.UserId.ToString();
                fm.UpdateTime = DateTime.Now.ToString();
                FeeBLL.UpdateFee(fm);
            }
            return AjaxResult.Success();
        }


        #endregion

        #region  导出收费
        public AjaxResult DownloadFee()
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
            string proveName = Request.Form["txtProveName"];
            string proveId = Request.Form["txtProveID"];
            string personSort = Request.Form["selPersonSort"];

            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " AND s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " AND f.VoucherNum like '%" + voucherNum + "%'";
            }
            if (!string.IsNullOrEmpty(noteNum))
            {
                where += " AND f.NoteNum like '%" + noteNum + "%'";
            }
            if (!string.IsNullOrEmpty(createName))
            {
                where += " AND u2.Name like '%" + createName + "%'";
            }
            if (!string.IsNullOrEmpty(dept))
            {
                where += "AND f.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(feeMode))
            {
                where += OtherHelper.MultiSelectToSQLWhere(feeMode, "f.FeeMode");
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "f.Status");
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where += " AND convert(NVARCHAR(10),f.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where += " AND convert(NVARCHAR(10),f.FeeTime,23) <= '" + feeTimeE + "'";
            }
            if (!string.IsNullOrEmpty(proveName))
            {
                where += " AND i.Name like '%" + proveName + "%'";
            }
            if (!string.IsNullOrEmpty(proveId))
            {
                where += " AND f.ProveID = " + proveId + "";
            }
            if (!string.IsNullOrEmpty(personSort))
            {
                where += OtherHelper.MultiSelectToSQLWhere(personSort, "f.PersonSort");
            }
            string cmdText = @"SELECT  f.VoucherNum 凭证号 ,
        d.Name 校区 ,
        s.Name 学生姓名 ,
        s.IDCard 身份证号 ,
        (SELECT EnrollNum+',' FROM T_Stu_sEnroll WHERE StudentID=s.StudentID  and Status=1  FOR XML PATH('')) 学号,
        s.Mobile 联系电话 ,
        s.QQ QQ ,
        s.WeChat 微信 ,
        f.NoteNum 票据号 ,
        i.Name 证书名称 ,
        CONVERT(NVARCHAR(23), f.FeeTime, 23) 收费时间 ,
        f.CreateTime 具体收费时间 ,
        r2.RefeName 交费方式 ,
        f.ShouldMoney 应交金额 ,
        f.PaidMoney 实交金额 ,
           ( SELECT    ISNULL(SUM(DiscountMoney), 0)
          FROM      T_Pro_FeeDetail
          WHERE     Status = 1
                    AND FeeID = f.FeeID
        ) 优惠金额 ,
         ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Pro_Offset o
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = o.FeeDetailID
          WHERE     fd.FeeID = f.FeeID
                    AND o.Status = 1
        ) 充抵金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = o.ByFeeDetailID
          WHERE     fd.FeeID = f.FeeID
                    AND fd.Status = 1
                    AND o.Status = 1
        )
        + ( SELECT  ISNULL(SUM(so.Money), 0)
            FROM    T_Stu_sOffset so
                    LEFT JOIN T_Pro_FeeDetail fd ON so.ByRelatedID = fd.FeeDetailID
            WHERE   so.BySort = 3
                    AND fd.FeeID = f.FeeID
                    AND fd.Status = 1
                    AND so.Status = 1
          ) 被充抵金额 ,
       ( SELECT    ISNULL(SUM(RefundMoney), 0)
          FROM      T_Pro_Refund r
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = r.FeeDetailID
          WHERE     r.Status = 1
                    AND fd.Status = 1
                    AND fd.FeeID = f.FeeID
        ) 核销金额 ,
        u2.Name 收费人 ,
        pro.Name 专业 ,
        c.Name 班级 ,
        r3.RefeName 收款人员 ,
        f.Teacher  教师,
        r1.RefeName 状态 ,
        u1.Name 结账人 ,
        CASE CONVERT(NVARCHAR(23), f.AffirmTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(23), f.AffirmTime, 23)
        END 结账时间
FROM    T_Pro_Fee AS f
        LEFT JOIN T_Pro_Prove AS p ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
        LEFT JOIN T_Pro_Class c ON c.ClassID = p.ClassID
        LEFT JOIN T_Pro_Profession pro ON pro.ProfessionID = c.ProfessionID
        LEFT JOIN T_Sys_Dept AS d ON d.DeptID = f.DeptID
        LEFT JOIN T_Pro_Item AS i ON i.ItemID = p.ItemID
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = f.Status
                                      AND r1.RefeTypeID = 7
        LEFT JOIN T_Sys_Refe AS r2 ON r2.Value = f.FeeMode
                                      AND r2.RefeTypeID = 6
        LEFT JOIN T_Sys_User u1 ON u1.UserID = f.AffirmID
        LEFT JOIN T_Sys_User u2 ON u2.UserID = f.CreateID
        LEFT JOIN T_Sys_Refe r3 ON f.PersonSort = r3.Value
                                   AND r3.RefeTypeID = 11
            where 1=1 {0}
ORDER BY f.FeeID DESC";
            string filename = "证书收费信息.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "f.DeptID", "f.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }
        #endregion

        #region 输出打印信息，验证打印次数
        /// <summary>
        /// 输出打印信息，验证打印次数
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetPrintInfo()
        {
            string feeId = Request.Form["ID"];

            string where = " AND FeeID = @FeeID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FeeID", feeId)
            };
            FeeModel f = FeeBLL.FeeModelByWhere(where, paras);

            ConfigModel c = ConfigBLL.ConfigModelByWhere(" AND ConfigID = 1", null);//1、证书费用 与T_Pro_Item表Sort字段相同编号

            if (Convert.ToInt32(c.PrintNum) > Convert.ToInt32(f.PrintNum))
            {
                DataTable dt = FeeBLL.FeeTablePrintByWhere(" AND f.FeeID = @FeeID", paras);
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
            string feeId = Request.Form["ID"];

            string where = " AND FeeID = @FeeID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FeeID", feeId)
            };
            FeeModel f = FeeBLL.FeeModelByWhere(where, paras);

            DataTable dt = FeeBLL.FeeTablePrintByWhere(" AND f.FeeID = @FeeID", paras);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }
        #endregion

        #region 更新票据号

        public AjaxResult UpdateNoteNum()
        {
            string feeId = Request.Form["FeeID"];

            FeeModel fm = FeeBLL.GetFeeModel(feeId);

            string printNumStr = ConfigBLL.getNoteNum("1", "1");//1、证书费用 与T_Pro_Item表Sort字段相同编号

            if (!string.IsNullOrEmpty(fm.NoteNum))
            {
                DisableNoteModel dnm = new DisableNoteModel();
                dnm.Status = "1";
                dnm.FeeID = fm.FeeID;
                dnm.NoteNum = fm.NoteNum;
                dnm.CreateID = this.UserId.ToString();
                dnm.CreateTime = DateTime.Now.ToString();
                dnm.UpdateID = this.UserId.ToString();
                dnm.UpdateTime = DateTime.Now.ToString();
                DisableNoteBLL.InsertDisableNote(dnm);
            }

            fm.NoteNum = printNumStr;
            fm.PrintNum = (Convert.ToInt32(fm.PrintNum) + 1).ToString();

            FeeBLL.UpdateFee(fm);

            return AjaxResult.Success(printNumStr);
        }
        #endregion

        #region  收费信息列表
        public ActionResult GetChooseFeeList()
        {
            string deptId = Request.Form["DeptID"];
            string menuId = Request.Form["MenuID"];
            string studentName = Request.Form["studentName"];
            string voucherNum = Request.Form["voucherNum"];

            string where = "";
            if (!string.IsNullOrEmpty(studentName))
            {
                where += " and s.Name like '%" + studentName + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and f.VoucherNum like '%" + voucherNum + "%'";
            }
            //if (!string.IsNullOrEmpty(deptId))
            //{
            //    where += "AND f.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            //}
            //else
            //{
            //    where += "and 1=2 ";
            //}
            string cmdText = @"SELECT  fd.FeeDetailID ,
        f.VoucherNum ,
        d.Name Dept ,
        s.Name ,
        s.IDCard ,
        f.NoteNum ,
        i.Name ItemName ,
        dl.Name DetailName ,
        CONVERT(NVARCHAR(23), f.FeeTime, 23) FeeTime ,
        r2.RefeName FeeMode ,
        fd.ShouldMoney ,
        fd.PaidMoney ,
        fd.DiscountMoney ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID = fd.FeeDetailID
                    AND o.Status = 1
        ) OffsetMoney ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID = fd.FeeDetailID
                    AND o.Status = 1
        ) BeOffsetMoney ,
        ( SELECT    ISNULL(SUM(RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.Status = 1
                    AND r.FeeDetailID = fd.FeeDetailID
        ) RefundMoney ,
        r1.RefeName Status ,
        u.Name Affirm ,
        CASE CONVERT(NVARCHAR(10), f.AffirmTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), f.AffirmTime, 23)
        END AffirmTime ,
        fd.CanMoney ,
        0.00 Offset
FROM    T_Pro_FeeDetail AS fd
        LEFT JOIN T_Pro_Fee f ON f.FeeID = fd.FeeID
        LEFT JOIN T_Pro_Prove AS p ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
        LEFT JOIN T_Sys_Dept AS d ON d.DeptID = f.DeptID
        LEFT JOIN T_Pro_Item AS i ON i.ItemID = p.ItemID
        LEFT JOIN T_Pro_ItemDetail id ON id.ItemDetailID = fd.ItemDetailID
        LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = f.Status
                                      AND r1.RefeTypeID = 7
        LEFT JOIN T_Sys_Refe AS r2 ON r2.Value = f.FeeMode
                                      AND r2.RefeTypeID = 6
        LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
WHERE   f.Status IN ( 1, 2 ) and fd.Status=1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "f.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        #endregion

        #region 生成凭证号

        #endregion

        #region 导入(弃用)
        ///// <summary>
        /////导入收费信息
        ///// </summary>
        ///// <returns></returns>
        //public AjaxResult UpLoadBFee()
        //{
        //    string deptId = Request.Form["Dept"];
        //    string filePath = Request.Form["filePath"];
        //    string enrollTime = Request.Form["EnrollTime"];
        //    string feeTime = Request.Form["FeeTime"];
        //    string itemId = Request.Form["ItemID"];
        //    string itemDetailId = Request.Form["ItemDetailID"];
        //    string feeMode = Request.Form["FeeMode"];
        //    string teacher = Request.Form["Teacher"];
        //    string explain = Request.Form["Explain"];
        //    string remark = Request.Form["Remark"];
        //    string money = Request.Form["Money"];
        //    string personSort = Request.Form["PersonSort"];
        //    if (string.IsNullOrEmpty(deptId))
        //    {
        //        return AjaxResult.Error("请选择校区");
        //    }
        //    if (string.IsNullOrEmpty(filePath))
        //    {
        //        return AjaxResult.Error("请选择上传的文件");
        //    }
        //    if (string.IsNullOrEmpty(personSort))
        //    {
        //        return AjaxResult.Error("请选择交款人员");
        //    }
        //    if (string.IsNullOrEmpty(enrollTime))
        //    {
        //        return AjaxResult.Error("请选择报名时间");
        //    }
        //    if (string.IsNullOrEmpty(feeTime))
        //    {
        //        return AjaxResult.Error("请选择收费时间");
        //    }
        //    if (string.IsNullOrEmpty(itemId))
        //    {
        //        return AjaxResult.Error("请选收费项");
        //    }
        //    if (string.IsNullOrEmpty(itemDetailId))
        //    {
        //        return AjaxResult.Error("请选收费项明细");
        //    }
        //    string[] itemIdArray = itemId.Split(',');
        //    for (int j = 0; j < itemIdArray.Length; j++)
        //    {
        //        DataTable tempdt2 = ItemDetailBLL.GetItemDetailTable(itemIdArray[j], itemDetailId);
        //        if (tempdt2.Rows.Count == 0)
        //        {
        //            string where = " AND ItemID = @ItemID";
        //            SqlParameter[] paras = new SqlParameter[] {
        //                new SqlParameter("@ItemID", itemIdArray[j])
        //            };
        //            return AjaxResult.Error("请勾选【" + ItemBLL.ItemModelByWhere(where, paras).Name + "】收费项目;");
        //        }
        //    }
        //    if (string.IsNullOrEmpty(feeMode))
        //    {
        //        return AjaxResult.Error("请选交费方式");
        //    }
        //    if (!string.IsNullOrEmpty(teacher))
        //    {
        //        if (teacher.Length > 32)
        //        {
        //            return AjaxResult.Error("教师不能超过32个字符");
        //        }
        //    }
        //    if (string.IsNullOrEmpty(money))
        //    {
        //        return AjaxResult.Error("交费金额不能为空");
        //    }
        //    if (!string.IsNullOrEmpty(explain))
        //    {
        //        explain = explain.Replace("\r\n", "<br />");
        //    }
        //    if (!string.IsNullOrEmpty(remark))
        //    {
        //        remark = remark.Replace("\r\n", "<br />");
        //    }

        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        ds = OtherHelper.UploadFile(filePath);
        //    }
        //    catch (Exception)
        //    {
        //        return AjaxResult.Error("模板异常，请联系管理员");
        //    }
        //    DataTable dt = new DataTable();
        //    if (ds != null || ds.Tables[0].Rows.Count > 0)
        //    {
        //        int errorNum = 0;
        //        int successNum = 0;
        //        string[] array = new string[9];
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            string errorString = "";
        //            StudentModel sm = new StudentModel();
        //            string[] ProveID = new string[] { };
        //            string tempProveId = "";
        //            try
        //            {

        //                array[0] = ds.Tables[0].Rows[i][0].ToString().Trim();//专业
        //                array[1] = ds.Tables[0].Rows[i][1].ToString().Trim();//班级
        //                array[2] = ds.Tables[0].Rows[i][2].ToString().Trim();//姓名
        //                array[3] = ds.Tables[0].Rows[i][3].ToString().Trim();//身份证号
        //                array[4] = ds.Tables[0].Rows[i][4].ToString().Trim();//性别
        //                array[5] = ds.Tables[0].Rows[i][5].ToString().Trim();//手机
        //                array[6] = ds.Tables[0].Rows[i][6].ToString().Trim();//QQ
        //                array[7] = ds.Tables[0].Rows[i][7].ToString().Trim();//微信
        //                array[8] = ds.Tables[0].Rows[i][8].ToString().Trim();//地址

        //            }
        //            catch (Exception)
        //            {
        //                return AjaxResult.Error("模板错误");
        //            }
        //            if (i == 0)
        //            {
        //                if (array[0] != "专业" || array[1] != "班级" || array[2] != "姓名" || array[3] != "身份证号" || array[4] != "性别" || array[5] != "手机" || array[6] != "QQ" || array[7] != "微信" || array[8] != "地址")
        //                {
        //                    return AjaxResult.Error("模板错误");
        //                }
        //                else
        //                {
        //                    dt = TableTitle(dt, array);
        //                }
        //            }
        //            else
        //            {
        //                string classId = ValidateExcelWord(array, itemIdArray, itemDetailId, ref errorString, ref sm, ref ProveID);
        //                if (errorString == "")
        //                {
        //                    for (int k = 0; k < itemIdArray.Length; k++)
        //                    {

        //                        if (string.IsNullOrEmpty(sm.StudentID))
        //                        {
        //                            sm.StudentID = InsertStudent(deptId, array);
        //                        }
        //                        decimal tempMoney = ItemDetailBLL.ItemDetailGetMoney(itemDetailId, itemIdArray[k]);
        //                        for (int l = 0; l < ProveID.Length; l++)
        //                        {
        //                            string[] tempId = ProveID[l].Split(',');
        //                            if (itemIdArray[k] == tempId[0])
        //                            {
        //                                if (tempId[1] == "0")
        //                                {
        //                                    tempProveId = InsertProve(deptId, enrollTime, itemIdArray[k], sm.StudentID, classId);
        //                                    string FeeID = InsertFee(deptId, feeTime, personSort, feeMode, teacher, explain, remark, tempMoney.ToString(), tempProveId);
        //                                    DataTable tempdt = ItemDetailBLL.GetItemDetailTable(itemIdArray[k], itemDetailId);
        //                                    for (int j = 0; j < tempdt.Rows.Count; j++)
        //                                    {
        //                                        InsertFeeDetail(FeeID, tempdt.Rows[j]["ItemDetailID"].ToString(), 0, 0);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    string where = " AND ProveID = @ProveID";
        //                                    SqlParameter[] paras = new SqlParameter[] {
        //                                        new SqlParameter("@ProveID", tempId[1])
        //                                    };
        //                                    ProveModel pm = ProveBLL.ProveModelByWhere(where, paras);
        //                                    pm.Status = "2";
        //                                    ProveBLL.UpdateProve(pm);

        //                                    string FeeID = InsertFee(deptId, feeTime, personSort, feeMode, teacher, explain, remark, tempMoney.ToString(), tempId[1]);
        //                                    DataTable tempdt = ItemDetailBLL.GetItemDetailTable(itemIdArray[k], itemDetailId);
        //                                    for (int j = 0; j < tempdt.Rows.Count; j++)
        //                                    {
        //                                        InsertFeeDetail(FeeID, tempdt.Rows[j]["ItemDetailID"].ToString(), 0, 0);
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        successNum += 1;
        //                    }
        //                }
        //                else
        //                {
        //                    errorNum += 1;
        //                    dt = TableRows(dt, array, errorString);
        //                }
        //            }
        //        }
        //        string url = "";
        //        if (dt.Rows.Count > 0)
        //        {
        //            string FileName = OtherHelper.FilePathAndName();
        //            OtherHelper.DeriveToExcel(dt, FileName);
        //            url = "../Temp/" + FileName + "";
        //        }
        //        NoteModel nm = new NoteModel();
        //        nm.CreateID = this.UserId.ToString();
        //        nm.CreateTime = DateTime.Now.ToString();
        //        nm.InFile = filePath;
        //        nm.OutFile = url;
        //        nm.Sort = "1";
        //        nm.DeptID = deptId;
        //        nm.Status = "1";
        //        nm.SuccessNum = successNum.ToString();
        //        nm.ErrorNum = errorNum.ToString();
        //        NoteBLL.InsertNote(nm);
        //        string susscee = "成功导入" + successNum.ToString() + "条，错误数据" + errorNum.ToString() + "条。";
        //        string json = "{\"Tip\":\"操作成功\",\"Mesg\":\"" + susscee + "\",\"Url\":\"" + url + "\"}";
        //        return AjaxResult.Success(json);
        //    }
        //    else
        //    {
        //        return AjaxResult.Error("操作失败，数据不能为空");
        //    }

        //}
        #endregion

        #region 生成导出Excel列（弃用）
        /// <summary>
        /// 生成导出Excel列
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="array"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        //public DataTable TableRows(DataTable dt, string[] array, string remark)
        //{
        //    DataRow dr = dt.NewRow();
        //    dr["专业"] = array[0];
        //    dr["班级"] = array[1];
        //    dr["姓名"] = array[2];
        //    dr["身份证号"] = "'" + array[3];
        //    dr["性别"] = "'" + array[4];
        //    dr["手机"] = array[5];
        //    dr["QQ"] = array[6];
        //    dr["微信"] = array[7];
        //    dr["地址"] = array[8];
        //    dr["系统备注"] = remark;
        //    dt.Rows.Add(dr);
        //    return dt;
        //}
        #endregion

        #region 验证字段长度（弃用）
        //private static string ValidateExcelWord(string[] array, string[] itemId, string itemDetailId, ref string errorString, ref StudentModel sm, ref string[] ProveID)
        //{
        //    string classId = "0";
        //    if (!string.IsNullOrEmpty(array[0]) && !string.IsNullOrEmpty(array[1]))
        //    {
        //        classId = ClassBLL.GetClassID(array[0], array[1]);
        //        if (classId.Equals("-1"))
        //        {
        //            errorString += "班级不存在;";
        //        }
        //    }
        //    if (string.IsNullOrEmpty(array[2]))
        //    {

        //    }
        //    else
        //    {
        //        if (array[2].Length > 8)
        //        {
        //            errorString += "姓名不能超过8个字符;";
        //        }
        //    }
        //    if (string.IsNullOrEmpty(array[3]))
        //    {
        //        errorString += "身份证号不能为空;";
        //    }
        //    else
        //    {
        //        if (!OtherHelper.CheckIDCard(array[3]))
        //        {
        //            errorString += "身份证号不规范;";
        //        }
        //    }
        //    if (string.IsNullOrEmpty(array[5]))
        //    {
        //        errorString += "联系电话不能为空;";
        //    }
        //    else
        //    {
        //        if (array[5].Length > 16)
        //        {
        //            errorString += "联系电话长度不能超过16个字符;";
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(array[6]))
        //    {
        //        if (array[6].Length > 16)
        //        {
        //            errorString += "QQ长度不能超过16个字符;";
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(array[7]))
        //    {
        //        if (array[7].Length > 32)
        //        {
        //            errorString += "微信长度不能超过32个字符;";
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(array[8]))
        //    {
        //        if (array[8].Length > 128)
        //        {
        //            errorString += "地址长度不能超过128个字符;";
        //        }
        //    }
        //    sm = StudentBLL.GetStudentModel(array[2], array[3]);
        //    string proveString = "";
        //    for (int j = 0; j < itemId.Length; j++)
        //    {
        //        if (!string.IsNullOrEmpty(sm.StudentID))
        //        {
        //            DataTable dt = ProveBLL.GetProveTable(sm.StudentID, itemId[j]);
        //            if (dt.Rows.Count > 1)
        //            {
        //                proveString += itemId[j] + "," + "0;";
        //                for (int i = 0; i < dt.Rows.Count; i++)
        //                {
        //                    errorString += "该生已多次报考" + dt.Rows[i]["Name"] + "证书，请确认之前报考证书状态已更新;";
        //                }
        //            }
        //            else if (dt.Rows.Count == 1)
        //            {
        //                proveString += itemId[j] + "," + dt.Rows[0]["ProveID"].ToString() + ";";
        //                DataTable fdtable = GetFeeDetailTable(dt.Rows[0]["ProveID"].ToString(), itemDetailId);
        //                string tempString = "";
        //                if (fdtable.Rows.Count > 0)
        //                {
        //                    tempString += "不能重复交";
        //                    string str1 = "";
        //                    for (int k = 0; k < fdtable.Rows.Count; k++)
        //                    {
        //                        string str2 = ItemBLL.GetItemName(fdtable.Rows[k]["ItemDetailID"].ToString());

        //                        if (str1 != str2)
        //                        {
        //                            str1 = str2;
        //                            tempString += str1 + "【" + ItemDetailBLL.GetItemDetailName(fdtable.Rows[k]["ItemDetailID"].ToString()) + "】";
        //                        }
        //                        else
        //                        {
        //                            tempString += "【" + ItemDetailBLL.GetItemDetailName(fdtable.Rows[k]["ItemDetailID"].ToString()) + "】";
        //                        }

        //                    }
        //                    tempString += "如是重新报考请确认之前报考证书状态已更新;";
        //                }
        //                errorString += tempString;
        //            }
        //            else
        //            {
        //                proveString += itemId[j] + "," + "0;";
        //            }
        //        }
        //        else
        //        {
        //            proveString += itemId[j] + "," + "0;";
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(proveString))
        //    {
        //        proveString = proveString.Substring(0, proveString.Length - 1);
        //        ProveID = proveString.Split(';');
        //    }
        //    return classId;
        //}
        #endregion

        #region 添加收费明细
        /// <summary>
        /// 添加收费明细
        /// </summary>
        /// <param name="feeId"></param>
        /// <param name="itemdetailId"></param>
        /// <param name="discountMoneyDetail"></param>
        /// <param name="offsetMoneyDetail"></param>
        /// <returns></returns>
        private int InsertFeeDetail(string feeId, string itemdetailId, decimal discountMoneyDetail, decimal offsetMoneyDetail)
        {
            FeeDetailModel fdm = new FeeDetailModel();
            fdm.FeeID = feeId;
            fdm.Status = "1";
            fdm.UpdateID = this.UserId.ToString();
            fdm.UpdateTime = DateTime.Now.ToString();
            fdm.CreateID = this.UserId.ToString();
            fdm.CreateTime = DateTime.Now.ToString();
            fdm.ItemDetailID = itemdetailId;
            fdm.ShouldMoney = GetItemDetailMoney(itemdetailId);
            fdm.PaidMoney = (Convert.ToDecimal(fdm.ShouldMoney) - discountMoneyDetail - offsetMoneyDetail).ToString();//实收金额=应收金额-优惠金额-充抵金额
            fdm.DiscountMoney = discountMoneyDetail.ToString();
            fdm.CanMoney = (Convert.ToDecimal(fdm.PaidMoney) + offsetMoneyDetail).ToString();//可用金额=实收金额+充抵金额
            return FeeDetailBLL.InsertFeeDetail(fdm);
        }
        #endregion

        #region 添加收费
        /// <summary>
        /// 添加收费
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="feeTime"></param>
        /// <param name="personSort"></param>
        /// <param name="feeMode"></param>
        /// <param name="teacher"></param>
        /// <param name="explain"></param>
        /// <param name="remark"></param>
        /// <param name="money"></param>
        /// <param name="proveId"></param>
        /// <returns></returns>
        private string InsertFee(string deptId, string feeTime, string personSort, string feeMode, string teacher, string explain, string remark, string money, string proveId)
        {
            FeeModel fm = new FeeModel();
            fm.AffirmID = "0";
            fm.AffirmTime = "1900-01-01";
            fm.CreateID = this.UserId.ToString();
            fm.CreateTime = DateTime.Now.ToString();
            fm.DeptID = deptId;
            fm.Explain = explain;
            fm.PersonSort = personSort;
            fm.FeeMode = feeMode;
            fm.FeeTime = feeTime;
            fm.NoteNum = "";
            fm.PaidMoney = money;
            fm.PrintNum = "0";
            fm.ProveID = proveId;
            fm.Remark = remark;
            fm.ShouldMoney = money;
            fm.Status = "1";
            fm.Teacher = teacher;
            fm.UpdateID = this.UserId.ToString();
            fm.UpdateTime = DateTime.Now.ToString();
            fm.VoucherNum = ConfigBLL.getVoucherNum("1", "");
            fm.FeeID = FeeBLL.InsertFee(fm).ToString();
            return fm.FeeID;
        }
        #endregion

        #region 添加学生信息（弃用）
        /// <summary>
        /// 添加学生信息
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="array"></param>
        /// <param name="sm"></param>
        /// <returns></returns>
        //private string InsertStudent(string deptId, string[] array)
        //{
        //    StudentModel sm = new StudentModel();
        //    sm.DeptID = deptId;
        //    sm.Name = array[2];
        //    sm.IDCard = array[3];
        //    sm.Sex = RefeBLL.GetRefeValue(array[4], "3");
        //    if (sm.Sex.Equals("-1"))
        //    {
        //        sm.Sex = "3";
        //    }
        //    sm.Mobile = array[5];
        //    sm.QQ = array[6];
        //    sm.WeChat = array[7];
        //    sm.Address = array[8];
        //    sm.Status = "1";
        //    sm.CreateID = this.UserId.ToString();
        //    sm.CreateTime = DateTime.Now.ToString();
        //    sm.UpdateID = this.UserId.ToString();
        //    sm.UpdateTime = DateTime.Now.ToString();
        //    sm.Remark = "";
        //    sm.StudentID = StudentBLL.InsertStudent(sm).ToString();
        //    return sm.StudentID;
        //}
        #endregion

        #region   生成Excel表头（弃用）
        /// <summary>
        /// 生成Excel表头
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        //public DataTable TableTitle(DataTable dt, string[] array)
        //{
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        dt.Columns.Add(array[i], Type.GetType("System.String"));
        //    }
        //    dt.Columns.Add("系统备注", Type.GetType("System.String"));
        //    return dt;
        //}
        #endregion

        #region 添加证书
        /// <summary>
        /// 添加证书
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="enrollTime"></param>
        /// <param name="itemId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        private string InsertProve(string deptId, string enrollTime, string itemId, string studentId, string classId)
        {
            ProveModel pm = new ProveModel();
            pm.CreateID = this.UserId.ToString();
            pm.CreateTime = DateTime.Now.ToString();
            pm.DeptID = deptId;
            pm.ClassID = classId;
            pm.EnrollTime = enrollTime;
            pm.IsForce = "1";
            pm.ItemID = itemId;
            pm.Remark = "";
            pm.Status = "2";
            pm.StudentID = studentId;
            pm.UpdateID = this.UserId.ToString();
            pm.UpdateTime = DateTime.Now.ToString();
            pm.ProveID = ProveBLL.InsertProve(pm).ToString();
            return pm.ProveID;
        }

        #endregion

        #region 获取收费明细实体
        /// <summary>
        /// 获取收费明细实体
        /// </summary>
        /// <param name="feeId"></param>
        /// <returns></returns>
        public FeeDetailModel GetFeeDetailModel(string feeId)
        {
            string where = " and FeeID=@FeeID";
            SqlParameter[] paras = new SqlParameter[] {
                 new SqlParameter("@FeeID",feeId)
            };
            FeeDetailModel fdm = FeeDetailBLL.FeeDetailModelByWhere(where, paras);
            return fdm;
        }
        #endregion

        #region 获取项目明细金额
        public string GetItemDetailMoney(string itemDetailId)
        {
            DataTable dt = ItemDetailBLL.ItemDetailGetMoney(itemDetailId);
            return dt.Rows[0]["Money"].ToString();
        }
        #endregion

        #region 获取收费明细Table
        /// <summary>
        /// 获取收费明细Table
        /// </summary>
        /// <param name="feeId"></param>
        /// <param name="itemDetailId"></param>
        /// <returns></returns>
        private static DataTable GetFeeDetailTable(string proveId, string itemDetailId)
        {
            string where = @" AND f.ProveID = @ProveID
        AND fd.ItemDetailID IN (" + itemDetailId + ")";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ProveID",proveId)
            };
            DataTable dt = FeeDetailBLL.FeeDetailFeeTableByWhere(where, paras, " Order By f.ProveID ASC");
            return dt;
        }
        #endregion

        #region 编辑收费
        /// <summary>
        /// 编辑收费
        /// </summary>
        /// <returns></returns>
        public string GetFeeEdit()
        {

            string feeId = Request.Form["editFeeID"];
            string offsetId = Request.Form["editOffsetID"];
            string deptId = Request.Form["editDeptID"];
            string proveId = Request.Form["editProveID"];
            string itemDetailId = Request.Form["editItemDetailID"];
            string personSort = Request.Form["editPersonSort"];
            string feeMode = Request.Form["editFeeMode"];
            string shouldMoney = Request.Form["editShouldMoney"];
            string paidMoney = Request.Form["editPaidMoney"];
            string discountMoney = Request.Form["editDiscountMoney"];
            string offsetMoney = Request.Form["editOffsetMoney"];
            string teacher = Request.Form["editTeacher"];
            string feeTime = Request.Form["editFeeTime"];
            string explain = Request.Form["editExplain"];
            string remark = Request.Form["editRemark"];
            string discountListString = Request.Form["editFeeDiscountJson"];
            if (string.IsNullOrEmpty(offsetId))
                offsetId = "";
            if (string.IsNullOrEmpty(discountListString))
                discountListString = "";
            if (offsetId.Equals("[]"))
                offsetId = "";
            if (discountListString.Equals("[]"))
                discountListString = "";


            FeeOffsetList offsetList = JsonConvert.DeserializeObject<FeeOffsetList>(offsetId);
            FeeDiscountList discount = JsonConvert.DeserializeObject<FeeDiscountList>(discountListString);
            if (itemDetailId.IndexOf(",", 0, 1) > -1)
            {
                itemDetailId = itemDetailId.Substring(1, itemDetailId.Length - 1);
            }
            if (string.IsNullOrEmpty(feeId))
            {
                feeId = "0";
            }
            if (string.IsNullOrEmpty(deptId))
            {
                return "请选择收费单位";
            }

            if (string.IsNullOrEmpty(proveId))
            {
                return "请选择证书";
            }

            if (string.IsNullOrEmpty(personSort))
            {
                return "请选择交款人员";
            }

            if (string.IsNullOrEmpty(itemDetailId))
            {
                return "请选择项目明细";
            }

            if (string.IsNullOrEmpty(feeMode))
            {
                return "请选择交费方式";
            }
            if (string.IsNullOrEmpty(shouldMoney))
            {
                return "应交金额不能为空";
            }
            if (string.IsNullOrEmpty(paidMoney))
            {
                return "实交金额能为空";
            }
            if (string.IsNullOrEmpty(feeTime))
            {
                return "交费时间不能为空";
            }

            if (string.IsNullOrEmpty(discountMoney))
            {
                discountMoney = "0";
            }
            if (string.IsNullOrEmpty(shouldMoney))
            {
                shouldMoney = "0";
            }
            if (string.IsNullOrEmpty(offsetMoney))
            {
                offsetMoney = "0";
            }
            if (Convert.ToDecimal(shouldMoney) != Convert.ToDecimal(paidMoney) + Convert.ToDecimal(discountMoney) + Convert.ToDecimal(offsetMoney))
            {
                return "应收金额必须等于实收金额+优惠金额+冲抵金额";
            }

            if (!string.IsNullOrEmpty(explain))
            {
                explain = explain.Replace("\r\n", "<br />");
            }
            if (!string.IsNullOrEmpty(remark))
            {
                remark = remark.Replace("\r\n", "<br />");
            }


            DataTable fdtable = SelectFeeDetailByWhere(feeId, proveId, itemDetailId);
            string tempString = "";
            if (fdtable.Rows.Count > 0)
            {
                for (int k = 0; k < fdtable.Rows.Count; k++)
                {
                    tempString += "" + ItemDetailBLL.GetItemDetailName(fdtable.Rows[k]["ItemDetailID"].ToString()) + "";
                }
                tempString += " 已交费";
                return tempString;
            }
            string errstring = "";
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                bool flag = false;
                if (!feeId.Equals("0"))
                {
                    DataTable fd = FeeDetailBLL.FeeDetailTableByWhere(feeId);//返回修改前的收费明细编号
                    for (int i = 0; i < fd.Rows.Count; i++)//遍历修改前的收费明细
                    {
                        DataTable offset = OffsetBLL.GetOffsetMoney(fd.Rows[i]["FeeDetailID"].ToString());//根据以前的收费明细返回被冲抵的收费金额和被充抵的收费明细编号
                        decimal byoffsetMoney = 0;
                        for (int j = 0; j < offset.Rows.Count; j++)
                        {
                            byoffsetMoney = decimal.Parse(offset.Rows[j]["Money"].ToString());
                            FeeDetailBLL.UpdateFeeDetailCanMoney(-byoffsetMoney, this.UserId, offset.Rows[j]["ByFeeDetailID"].ToString());
                        }
                    }
                }
                if (offsetList != null)
                {
                    foreach (var item in offsetList.rows)
                    {
                        if (item.Offset > FeeDetailBLL.GetuseableMoney(item.FeeDetailID))
                        {
                            Transaction.Current.Rollback();
                            return "可充抵金额不足";
                        }
                    }
                }

                try
                {
                    if (discount != null)//判断优惠金额
                    {
                        foreach (var item in discount.rows)
                        {
                            if (item.DiscountMoney > ItemDetailBLL.GetItemDetialMoney(item.ItemDetailID))
                            {
                                Transaction.Current.Rollback();
                                return "优惠金额不能大于应收金额";
                            }
                        }
                    }
                    string where = " and FeeID=@FeeID";
                    SqlParameter[] paras = new SqlParameter[] {
                       new SqlParameter ("@FeeID",feeId)
                      };

                    FeeModel fm = FeeBLL.FeeModelByWhere(where, paras);
                    string tempProveId = fm.ProveID;
                    fm.DeptID = deptId;
                    fm.ProveID = proveId;
                    fm.PersonSort = personSort;
                    fm.PaidMoney = paidMoney;
                    fm.ShouldMoney = shouldMoney;
                    fm.Explain = explain;
                    fm.FeeMode = feeMode;
                    fm.FeeTime = feeTime;
                    fm.Remark = remark;
                    fm.Teacher = teacher;
                    fm.UpdateID = this.UserId.ToString();
                    fm.UpdateTime = DateTime.Now.ToString();
                    fm.FeeID = feeId;
                    if (fm.FeeID.Equals("0"))
                    {

                        fm.Status = "1";
                        fm.PrintNum = "0";
                        fm.AffirmID = "0";
                        fm.AffirmTime = "1900-01-01";
                        fm.VoucherNum = ConfigBLL.getVoucherNum("1", "");
                        fm.NoteNum = "";
                        fm.CreateID = this.UserId.ToString();
                        fm.CreateTime = DateTime.Now.ToString();
                        fm.FeeID = FeeBLL.InsertFee(fm).ToString();
                        string[] itemDetailIdArray = itemDetailId.Split(',');

                        for (int i = 0; i < itemDetailIdArray.Length; i++)
                        {
                            decimal discountMoneyDetail = 0;
                            decimal offsetMoneyDetail = 0;
                            if (offsetList != null)
                            {
                                var offsetlist = offsetList.rows.Where(O => O.ItemDetailID == itemDetailIdArray[i]).ToList();
                                if (offsetlist != null)
                                {
                                    offsetMoneyDetail = offsetlist.Sum(O => O.Offset);
                                }
                            }

                            if (discount != null)
                            {
                                if (discount.rows != null)
                                {
                                    discountMoneyDetail = discount.rows.FirstOrDefault(D => D.ItemDetailID == itemDetailIdArray[i]).DiscountMoney;
                                }
                            }

                            string feedetialId = InsertFeeDetail(fm.FeeID, itemDetailIdArray[i], discountMoneyDetail, offsetMoneyDetail).ToString();
                            if (offsetList != null)
                            {
                                var offsetlist = offsetList.rows.Where(O => O.ItemDetailID == itemDetailIdArray[i]).ToList();
                                foreach (var item in offsetlist)
                                {
                                    OffsetModel om = new OffsetModel();
                                    om.ByFeeDetailID = item.FeeDetailID;
                                    om.CreateID = this.UserId.ToString();
                                    om.UpdateID = this.UserId.ToString();
                                    om.UpdateTime = DateTime.Now.ToString();
                                    om.CreateTime = DateTime.Now.ToString();
                                    om.FeeDetailID = feedetialId;
                                    om.Money = item.Offset.ToString();
                                    om.Status = "1";
                                    OffsetBLL.InsertOffset(om);//新增充抵信息
                                    FeeDetailBLL.UpdateFeeDetailCanMoney(item.Offset, this.UserId, item.FeeDetailID);//修改可用金额
                                    UpdateProveStatus(ProveBLL.ProveIDByFeeDetailID(item.FeeDetailID));
                                }
                            }
                        }
                        UpdateProveStatus(proveId);
                        flag = true;

                    }
                    else
                    {

                        fm.FeeID = FeeBLL.UpdateFee(fm).ToString();//修改收费信息
                        string[] itemDetailIdArray = itemDetailId.Split(',');

                        UpdateFeeDetailStatus(fm.FeeID, "2");//停用收费明细

                        UpdateOffsetStatus("2", fm.FeeID);//停用充抵
                        DataTable dt = ProveBLL.ProveIDByFeeID(fm.FeeID);
                        foreach (DataRow dr in dt.Rows)
                        {
                            UpdateProveStatus(dr["ProveID"].ToString());
                        }

                        for (int i = 0; i < itemDetailIdArray.Length; i++)
                        {
                            decimal discountMoneyDetail = 0;
                            decimal offsetMoneyDetail = 0;
                            if (offsetList != null)
                            {
                                var offsetlist = offsetList.rows.Where(O => O.ItemDetailID == itemDetailIdArray[i]).ToList();
                                if (offsetlist != null)
                                {
                                    offsetMoneyDetail = offsetlist.Sum(O => O.Offset);
                                }
                            }
                            if (discount != null)
                            {
                                if (discount.rows != null)
                                {
                                    discountMoneyDetail = discount.rows.FirstOrDefault(D => D.ItemDetailID == itemDetailIdArray[i]).DiscountMoney;
                                }
                            }
                            string feedetialId = InsertFeeDetail(fm.FeeID, itemDetailIdArray[i], discountMoneyDetail, offsetMoneyDetail).ToString();//添加新收费明细
                            if (offsetList != null)
                            {
                                var offsetlists = offsetList.rows.Where(O => O.ItemDetailID == itemDetailIdArray[i]);
                                foreach (var item in offsetlists)
                                {
                                    OffsetModel om = new OffsetModel();

                                    om.CreateID = this.UserId.ToString();
                                    om.UpdateID = this.UserId.ToString();
                                    om.UpdateTime = DateTime.Now.ToString();
                                    om.CreateTime = DateTime.Now.ToString();
                                    om.ByFeeDetailID = item.FeeDetailID;
                                    om.FeeDetailID = feedetialId;
                                    om.Money = item.Offset.ToString();
                                    om.Status = "1";
                                    om.Money = item.Offset.ToString();
                                    OffsetBLL.InsertOffset(om);//新增充抵信息
                                    FeeDetailBLL.UpdateFeeDetailCanMoney(item.Offset, this.UserId, item.FeeDetailID);//修改可用金额
                                    UpdateProveStatus(ProveBLL.ProveIDByFeeDetailID(item.FeeDetailID));
                                }
                            }
                        }
                        if (tempProveId != proveId)
                        {
                            UpdateProveStatus(tempProveId);
                        }
                        UpdateProveStatus(proveId);
                        flag = true;

                    }
                    if (flag)
                    {
                        ts.Complete();
                        errstring = "yes";
                    }
                    else
                    {
                        errstring = "出现未知错误，请联系管理员";
                    }

                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    return ex.Message;
                }
                finally
                {
                    ts.Dispose();
                }
                return errstring;
            }
        }
        #endregion

        #region 变更收费明细状态
        public bool UpdateFeeDetailStatus(string feeId, string status)
        {
            SqlParameter[] paras = new SqlParameter[] {
             new SqlParameter("@Status",status),
             new SqlParameter("@FeeID",feeId),
             new SqlParameter("@UpdateID",this.UserId.ToString()),
             new SqlParameter("@UpdateTime",DateTime.Now.ToString()),
            };
            if (FeeDetailBLL.UpdateFeeDetailStatus(paras) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 修改表单赋值
        public AjaxResult SelectFee()
        {
            string feeId = Request.Form["ID"];
            DataTable dt = FeeBLL.SelectFee(feeId);
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }
        #endregion

        #region 查看
        public AjaxResult SelectViewFee()
        {
            string feeId = Request.Form["ID"];
            string cmdText = @"SELECT  d.Name DeptName ,
        f.VoucherNum ,
        f.NoteNum ,
        p.StudentID ,
        s.Name StudentName ,
        s.IDCard ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        r1.RefeName FeeMode ,
        f.ShouldMoney ,
        f.PaidMoney ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = o.FeeDetailID
          WHERE     o.Status = 1
                    AND fd.FeeID = f.FeeID
        ) OffsetMoney ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = o.ByFeeDetailID
          WHERE     o.Status = 1
                    AND fd.FeeID = f.FeeID
        ) ByOffsetMoney ,
        ( SELECT    ISNULL(SUM(DiscountMoney), 0)
          FROM      T_Pro_FeeDetail
          WHERE     Status = 1
                    AND FeeID = f.FeeID
        ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(re.RefundMoney), 0)
          FROM      T_Pro_Refund re
                    LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeDetailID = re.FeeDetailID
          WHERE     re.Status = 1
                    AND fd.FeeID = f.FeeID
        ) RefundMoney ,
        u.Name AffirmName ,
        CASE CONVERT(NVARCHAR(10), f.AffirmTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), f.AffirmTime, 23)
        END AffirmTime ,
        f.Teacher ,
        r2.RefeName Status ,
        i.Name ItemName ,
        c.Name ClassName ,
        ( SELECT    dl.Name + ' ' + CONVERT(NVARCHAR(10), fd.ShouldMoney) + '元,'
          FROM      T_Pro_FeeDetail fd
                    LEFT JOIN T_Pro_ItemDetail id ON fd.ItemDetailID = id.ItemDetailID
                    LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
          WHERE     fd.Status = 1
                    AND fd.FeeID = f.FeeID
        FOR
          XML PATH('')
        ) FeeContent ,
        f.Explain ,
        f.Remark,
		r3.RefeName PersonSort
FROM    T_Pro_Fee f
        LEFT JOIN T_Pro_FeeDetail fd ON fd.FeeID = f.FeeID
        LEFT JOIN T_Sys_Dept d ON f.DeptID = d.DeptID
        LEFT JOIN T_Pro_Prove p ON f.ProveID = p.ProveID
        LEFT JOIN T_Pro_Student s ON p.StudentID = s.StudentID
        LEFT JOIN T_Sys_Refe r1 ON f.FeeMode = r1.Value
                                   AND r1.RefeTypeID = 6
        LEFT JOIN T_Sys_User u ON f.AffirmID = u.UserID
        LEFT JOIN T_Sys_Refe r2 ON f.Status = r2.Value
                                   AND r2.RefeTypeID = 7
        LEFT JOIN T_Pro_Item i ON p.ItemID = i.ItemID
        LEFT JOIN T_Pro_Class c ON p.ClassID = c.ClassID
		LEFT JOIN T_Sys_Refe r3 ON r3.Value=f.PersonSort AND r3.RefeTypeID=11
WHERE   f.FeeID = {0}";
            cmdText = string.Format(cmdText, feeId);
            return AjaxResult.Success(JsonData.GetArray(cmdText));
        }
        #endregion

        #region 返回收费明细Table
        public DataTable SelectFeeDetailByWhere(string feeId, string proveId, string itemDetailId)
        {
            string where = " and Status=1 and FeeID<>@FeeID and FeeID in(select FeeID from T_Pro_Fee where Status IN(1,2) and  ProveID=@ProveID) and ItemDetailID in(" + itemDetailId + ") and CanMoney>0";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@ProveID",proveId),
            new SqlParameter("@FeeID",feeId)
            };
            DataTable dt = FeeDetailBLL.FeeDetailTableByWhere(where, paras, "");
            return dt;
        }
        #endregion


        /// <summary>
        /// 修改充抵状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="feeId"></param>
        /// <returns></returns>
        public bool UpdateOffsetStatus(string status, string feeId)
        {
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@Status",status),
            new SqlParameter("@FeeID",feeId),
            new SqlParameter("@UpdateID",this.UserId.ToString()),
            new SqlParameter("@UpdateTime",DateTime.Now.ToString())
            };
            if (OffsetBLL.UpdateOffsetStatus(paras) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #region 选择收费项
        public ActionResult GetChooseFee()
        {
            string studentName = Request.Form["studentName"];
            string voucherNum = Request.Form["voucherNum"];

            string where = "";
            if (!string.IsNullOrEmpty(studentName))
            {
                where += " and s.Name like '%" + studentName + "%'";
            }
            if (!string.IsNullOrEmpty(voucherNum))
            {
                where += " and f.VoucherNum like '%" + voucherNum + "%'";
            }
            string cmdText = @"SELECT  fd.FeeDetailID ,
        f.VoucherNum ,
        d.Name Dept ,
        s.Name ,
        s.IDCard ,
        f.NoteNum ,
        i.Name ItemName ,
        dl.Name DetailName ,
        CONVERT(NVARCHAR(10), f.FeeTime, 23) FeeTime ,
        r2.RefeName FeeMode ,
        fd.ShouldMoney ,
        fd.PaidMoney ,
        fd.DiscountMoney ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID = fd.FeeDetailID
                    AND o.Status = 1
        ) OffsetMoney ,
        ( SELECT    ISNULL(SUM(Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID = fd.FeeDetailID
                    AND o.Status = 1
        ) BeOffsetMoney ,
        ( SELECT    ISNULL(SUM(RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.Status = 1
                    AND r.FeeDetailID = fd.FeeDetailID
        ) RefundMoney ,
        r1.RefeName Status ,
        u.Name Affirm ,
        CASE CONVERT(NVARCHAR(10), f.AffirmTime, 23)
          WHEN '1900-01-01' THEN ''
          ELSE CONVERT(NVARCHAR(10), f.AffirmTime, 23)
        END AffirmTime ,
        fd.CanMoney
FROM    T_Pro_FeeDetail AS fd
        LEFT JOIN T_Pro_Fee f ON f.FeeID = fd.FeeID
        LEFT JOIN T_Pro_Prove AS p ON p.ProveID = f.ProveID
        LEFT JOIN T_Pro_Student s ON s.StudentID = p.StudentID
        LEFT JOIN T_Sys_Dept AS d ON d.DeptID = f.DeptID
        LEFT JOIN T_Pro_Item AS i ON i.ItemID = p.ItemID
        LEFT JOIN T_Pro_ItemDetail id ON id.ItemDetailID = fd.ItemDetailID
        LEFT JOIN T_Pro_Detail dl ON dl.DetailID = id.DetailID
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = f.Status
                                      AND r1.RefeTypeID = 7
        LEFT JOIN T_Sys_Refe AS r2 ON r2.Value = f.FeeMode
                                      AND r2.RefeTypeID = 6
        LEFT JOIN T_Sys_User u ON u.UserID = f.AffirmID
WHERE   f.Status <> 9  and fd.Status=1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        #endregion

        public string GetFeeDetailDataGrid()
        {
            string feeId = Request.Form["ID"];
            DataTable dt = OffsetBLL.SelectOffsetByFeeId(feeId);
            string content = "[";
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                content += "{\"FeeDetailID\":\"" + dt.Rows[i]["FeeDetailID"] + "\",\"VoucherNum\":\"" + dt.Rows[i]["VoucherNum"] + "\",\"Name\":\"" + dt.Rows[i]["Name"] + "\",\"Dept\":\"" + dt.Rows[i]["Dept"] + "\",\"ItemName\":\"" + dt.Rows[i]["ItemName"] + "\",\"FeeTime\":\"" + dt.Rows[i]["FeeTime"] + "\",\"OffsetItem\":\"" + dt.Rows[i]["OffsetItem"] + "\",\"Offset\":\"" + dt.Rows[i]["Offset"] + "\",\"ItemDetailID\":\"" + dt.Rows[i]["ItemDetailID"] + "\"},";
            }
            if (!content.Equals("["))
            {
                content = content.Substring(0, content.Length - 1);
            }
            content += "]";
            string str = "{\"rows\":" + content + ",\"total\":" + i + "}";
            return str;
        }

        /// <summary>
        /// 修改证书状态
        /// </summary>
        /// <param name="proveId"></param>
        private void UpdateProveStatus(string proveId)
        {
            string isAll = "Yes";

            string where2 = " AND Status <> 9 AND ProveID = @ProveID";
            SqlParameter[] paras2 = new SqlParameter[] {
                new SqlParameter ("@ProveID", proveId)
            };
            DataTable dt = FeeBLL.FeeTableByWhere(where2, paras2, "");
            foreach (DataRow dr in dt.Rows)
            {
                if (FeeDetailBLL.GetuseableMoneyByFeeID(dr["FeeID"].ToString()) > 0)
                {
                    isAll = "No";
                    break;
                }
            }

            string where3 = " AND Status <> 9 AND ProveID = @ProveID";
            SqlParameter[] paras3 = new SqlParameter[] {
                new SqlParameter("@ProveID", proveId)
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
        /// 根据收费信息修改证书状态
        /// </summary>
        /// <param name="feeId"></param>
        private void UpdateProveStatusByFeeID(string feeId)
        {

            string where = " AND FeeID = @FeeID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FeeID",feeId)
            };
            UpdateProveStatus(FeeBLL.FeeModelByWhere(where, paras).ProveID);
        }

        /// <summary>
        /// 修改时查询充抵
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AjaxResult GetSelectItemDetailOffset(string ID)
        {
            return AjaxResult.Success(JsonGridData.GetGridJSON(OffsetBLL.GetSelectoffset(ID)), "");
        }
        /// <summary>
        /// 修改查询优惠
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AjaxResult GetSelectDiscount(string ID)
        {
            return AjaxResult.Success(JsonGridData.GetGridJSON(OffsetBLL.GetSelectdiscount(ID)), "");
        }

        private string InsertStudent2(string deptId, ExcelModel em)
        {
            StudentModel sm = new StudentModel();
            sm.DeptID = deptId;
            sm.Name = em.姓名.Replace(" ", "");
            sm.IDCard = em.身份证号.Replace(" ", "");
            sm.Sex = RefeBLL.GetRefeValue(em.性别.Trim(), "3");
            sm.Mobile = em.手机.Replace(" ", "");
            sm.QQ = em.QQ.Replace(" ", "");
            sm.WeChat = em.微信.Replace(" ", "");
            sm.Address = em.地址.Replace(" ", "");
            sm.Status = "1";
            sm.CreateID = this.UserId.ToString();
            sm.CreateTime = DateTime.Now.ToString();
            sm.UpdateID = this.UserId.ToString();
            sm.UpdateTime = DateTime.Now.ToString();
            sm.Remark = "";
            sm.StudentID = StudentBLL.InsertStudent(sm).ToString();
            return sm.StudentID;
        }

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
            catch (Exception ex)
            {

                return AjaxResult.Error(ex.Message);
            }
            try
            {
                if (!dt.Columns[0].ColumnName.Trim().Equals("专业") || !dt.Columns[1].ColumnName.Trim().Equals("班级") || !dt.Columns[2].ColumnName.Trim().Equals("姓名") || !dt.Columns[3].ColumnName.Trim().Equals("身份证号") || !dt.Columns[4].ColumnName.Trim().Equals("性别") || !dt.Columns[5].ColumnName.Trim().Equals("手机") || !dt.Columns[6].ColumnName.Trim().Equals("QQ") || !dt.Columns[7].ColumnName.Trim().Equals("微信") || !dt.Columns[8].ColumnName.Trim().Equals("地址"))
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
        /// 错误Data
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable TableTitle2(DataTable dt)
        {
            dt.Columns.Add("专业", Type.GetType("System.String"));
            dt.Columns.Add("班级", Type.GetType("System.String"));
            dt.Columns.Add("姓名", Type.GetType("System.String"));
            dt.Columns.Add("身份证号", Type.GetType("System.String"));
            dt.Columns.Add("性别", Type.GetType("System.String"));
            dt.Columns.Add("手机", Type.GetType("System.String"));
            dt.Columns.Add("QQ", Type.GetType("System.String"));
            dt.Columns.Add("微信", Type.GetType("System.String"));
            dt.Columns.Add("地址", Type.GetType("System.String"));
            dt.Columns.Add("系统备注", Type.GetType("System.String"));
            return dt;
        }
        /// <summary>
        /// 错误data行
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="em"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public DataTable TableRow2(DataTable dt, ExcelModel em, string remark)
        {
            DataRow dr = dt.NewRow();
            dr["专业"] = em.专业;
            dr["班级"] = em.班级;
            dr["姓名"] = em.姓名;
            dr["身份证号"] = em.身份证号;
            dr["性别"] = em.性别;
            dr["手机"] = em.手机;
            dr["QQ"] = em.QQ;
            dr["微信"] = em.微信;
            dr["地址"] = em.地址;
            dr["系统备注"] = remark;
            dt.Rows.Add(dr);
            return dt;
        }
        /// <summary>
        /// 验证字段
        /// </summary>
        /// <param name="em">excel</param>
        /// <param name="itemId">证书</param>
        /// <param name="itemDetailId">证书收费项</param>
        /// <param name="errorString">错误信息</param>
        /// <param name="sm">学生信息</param>
        /// <param name="ProveID">证书编号</param>
        /// <returns></returns>
        private static string ValidateExcelWord2(ExcelModel em, string[] itemId, string itemDetailId, ref string errorString, ref StudentModel sm, ref string[] ProveID, string deptId)
        {
            string classId = "0";
            if (!string.IsNullOrEmpty(em.专业) && !string.IsNullOrEmpty(em.班级))
            {
                classId = ClassBLL.GetClassID(em.专业, em.班级, deptId);
                if (classId.Equals("-1"))
                {
                    errorString += "班级不存在;";
                }
            }
            if (string.IsNullOrEmpty(em.姓名))
            {
                errorString += "姓名不能为空;";
            }
            else
            {
                if (em.姓名.Length > 16)
                {
                    errorString += "姓名不能超过16个字符;";
                }
            }
            if (string.IsNullOrEmpty(em.身份证号))
            {
                errorString += "身份证号不能为空;";
            }
            else
            {
                if (!OtherHelper.CheckIDCard(em.身份证号.Trim()))
                {
                    errorString += "身份证号不规范;";
                }
                else
                {
                    string name = StudentBLL.GetStudentModel(em.身份证号.Trim()).Name;
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (string.IsNullOrEmpty(StudentBLL.GetStudentModel(em.姓名.Replace(" ", ""), em.身份证号.Trim()).StudentID))
                        {
                            errorString += "身份证号和姓名不匹配;";
                        }
                    }
                }


            }
            if (string.IsNullOrEmpty(em.手机))
            {
                errorString += "手机不能为空;";
            }
            else
            {
                if (em.手机.Length > 16)
                {
                    errorString += "手机长度不能超过16个字符;";
                }
            }
            if (!string.IsNullOrEmpty(em.QQ))
            {
                if (em.QQ.Length > 16)
                {
                    errorString += "QQ长度不能超过16个字符;";
                }
            }
            if (!string.IsNullOrEmpty(em.微信))
            {
                if (em.微信.Length > 32)
                {
                    errorString += "微信长度不能超过32个字符;";
                }
            }
            if (!string.IsNullOrEmpty(em.地址))
            {
                if (em.地址.Length > 128)
                {
                    errorString += "地址长度不能超过128个字符;";
                }
            }

            if (RefeBLL.GetRefeValue(em.性别.Trim(), "3") == "-1")
            {
                errorString += "性别只能是男/女";
            }
            sm = StudentBLL.GetStudentModel(em.姓名.Replace(" ", ""), em.身份证号.Trim());
            string proveString = "";
            for (int j = 0; j < itemId.Length; j++)
            {
                if (!string.IsNullOrEmpty(sm.StudentID))
                {
                    DataTable dt = ProveBLL.GetProveTable(sm.StudentID, itemId[j]);
                    if (dt.Rows.Count > 1)
                    {
                        proveString += itemId[j] + "," + "0;";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            errorString += "该生已多次报考" + dt.Rows[i]["Name"] + "证书，请确认之前报考证书状态已更新;";
                        }
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        proveString += itemId[j] + "," + dt.Rows[0]["ProveID"].ToString() + ";";
                        DataTable fdtable = GetFeeDetailTable(dt.Rows[0]["ProveID"].ToString(), itemDetailId);
                        string tempString = "";
                        if (fdtable.Rows.Count > 0)
                        {
                            tempString += "不能重复交";
                            string str1 = "";
                            for (int k = 0; k < fdtable.Rows.Count; k++)
                            {
                                string str2 = ItemBLL.GetItemName(fdtable.Rows[k]["ItemDetailID"].ToString());

                                if (str1 != str2)
                                {
                                    str1 = str2;
                                    tempString += str1 + "【" + ItemDetailBLL.GetItemDetailName(fdtable.Rows[k]["ItemDetailID"].ToString()) + "】";
                                }
                                else
                                {
                                    tempString += "【" + ItemDetailBLL.GetItemDetailName(fdtable.Rows[k]["ItemDetailID"].ToString()) + "】";
                                }

                            }
                            tempString += "如是重新报考请确认之前报考证书状态已更新;";
                        }
                        errorString += tempString;
                    }
                    else
                    {
                        proveString += itemId[j] + "," + "0;";
                    }
                }
                else
                {
                    proveString += itemId[j] + "," + "0;";
                }
            }
            if (!string.IsNullOrEmpty(proveString))
            {
                proveString = proveString.Substring(0, proveString.Length - 1);
                ProveID = proveString.Split(';');
            }
            return classId;
        }


        public AjaxResult GetUpdateProveFee(UplodFormModel ufm)
        {
            List<ExcelModel> stuList = JsonConvert.DeserializeObject<List<ExcelModel>>(ufm.Paras);
            if (string.IsNullOrEmpty(ufm.Dept))
            {
                return AjaxResult.Error("请选择校区");
            }
            if (string.IsNullOrEmpty(ufm.filePath))
            {
                return AjaxResult.Error("请选择上传的文件");
            }
            if (string.IsNullOrEmpty(ufm.PersonSort))
            {
                return AjaxResult.Error("请选择交款人员");
            }
            if (string.IsNullOrEmpty(ufm.EnrollTime))
            {
                return AjaxResult.Error("请选择报名时间");
            }
            if (string.IsNullOrEmpty(ufm.FeeTime))
            {
                return AjaxResult.Error("请选择收费时间");
            }
            if (ufm.ItemID.Length == 0)
            {
                return AjaxResult.Error("请选收费项");
            }
            if (ufm.ItemDetailID.Length == 0)
            {
                return AjaxResult.Error("请选收费项明细");
            }
            string itemDetailIdString = "";
            for (int i = 0; i < ufm.ItemDetailID.Length; i++)
            {
                itemDetailIdString += ufm.ItemDetailID[i] + ",";
            }

            itemDetailIdString = itemDetailIdString.Substring(0, itemDetailIdString.Length - 1);
            for (int j = 0; j < ufm.ItemID.Length; j++)
            {
                DataTable tempdt2 = ItemDetailBLL.GetItemDetailTable(ufm.ItemID[j], itemDetailIdString);
                if (tempdt2.Rows.Count == 0)
                {
                    string where = " AND ItemID = @ItemID";
                    SqlParameter[] paras = new SqlParameter[] {
                        new SqlParameter("@ItemID", ufm.ItemID[j])
                    };
                    return AjaxResult.Error("请勾选【" + ItemBLL.ItemModelByWhere(where, paras).Name + "】收费项目;");
                }
            }
            if (string.IsNullOrEmpty(ufm.FeeMode))
            {
                return AjaxResult.Error("请选交费方式");
            }
            if (!string.IsNullOrEmpty(ufm.Teacher))
            {
                if (ufm.Teacher.Length > 32)
                {
                    return AjaxResult.Error("教师不能超过32个字符");
                }
            }
            if (ufm.Money.Equals(0))
            {
                return AjaxResult.Error("交费金额不能为空");
            }
            if (!string.IsNullOrEmpty(ufm.Explain))
            {
                ufm.Explain = ufm.Explain.Replace("\r\n", "<br />");
            }
            if (!string.IsNullOrEmpty(ufm.Remark))
            {
                ufm.Remark = ufm.Remark.Replace("\r\n", "<br />");
            }

            if (stuList.Count == 0)
            {
                return AjaxResult.Error("操作失败，数据不能为空");
            }
            else
            {

                StudentModel sm = new StudentModel();
                string[] ProveID = new string[] { };
                string tempProveId = "";
                int successNum = 0;
                int errorNum = 0;
                DataTable dt = new DataTable();
                dt = TableTitle2(dt);
                foreach (var item in stuList)
                {
                    string errorString = "";
                    string classId = ValidateExcelWord2(item, ufm.ItemID, itemDetailIdString, ref errorString, ref sm, ref ProveID, ufm.Dept);
                    if (errorString == "")
                    {
                        for (int k = 0; k < ufm.ItemID.Length; k++)
                        {

                            if (string.IsNullOrEmpty(sm.StudentID))
                            {
                                sm.StudentID = InsertStudent2(ufm.Dept, item);
                            }
                            decimal tempMoney = ItemDetailBLL.ItemDetailGetMoney(itemDetailIdString, ufm.ItemID[k]);
                            for (int l = 0; l < ProveID.Length; l++)
                            {
                                string[] tempId = ProveID[l].Split(',');
                                if (ufm.ItemID[k] == tempId[0])
                                {
                                    if (tempId[1] == "0")
                                    {
                                        tempProveId = InsertProve(ufm.Dept, ufm.EnrollTime.Replace(" ", ""), ufm.ItemID[k], sm.StudentID, classId);
                                        string FeeID = InsertFee(ufm.Dept, ufm.FeeTime, ufm.PersonSort, ufm.FeeMode, ufm.Teacher, ufm.Explain, ufm.Remark, tempMoney.ToString(), tempProveId);
                                        DataTable tempdt = ItemDetailBLL.GetItemDetailTable(ufm.ItemID[k], itemDetailIdString);
                                        for (int j = 0; j < tempdt.Rows.Count; j++)
                                        {
                                            InsertFeeDetail(FeeID, tempdt.Rows[j]["ItemDetailID"].ToString(), 0, 0);
                                        }
                                    }
                                    else
                                    {
                                        string where = " AND ProveID = @ProveID";
                                        SqlParameter[] paras = new SqlParameter[] {
                                                new SqlParameter("@ProveID", tempId[1])
                                            };
                                        ProveModel pm = ProveBLL.ProveModelByWhere(where, paras);
                                        pm.Status = "2";
                                        ProveBLL.UpdateProve(pm);

                                        string FeeID = InsertFee(ufm.Dept, ufm.FeeTime, ufm.PersonSort, ufm.FeeMode, ufm.Teacher, ufm.Explain, ufm.Remark, tempMoney.ToString(), tempId[1]);
                                        DataTable tempdt = ItemDetailBLL.GetItemDetailTable(ufm.ItemID[k], itemDetailIdString);
                                        for (int j = 0; j < tempdt.Rows.Count; j++)
                                        {
                                            InsertFeeDetail(FeeID, tempdt.Rows[j]["ItemDetailID"].ToString(), 0, 0);
                                        }
                                    }
                                }
                            }

                            successNum += 1;
                        }
                    }
                    else
                    {
                        errorNum += 1;
                        dt = TableRow2(dt, item, errorString);
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
                nm.InFile = ufm.filePath;
                nm.OutFile = url;
                nm.Sort = "1";
                nm.DeptID = ufm.Dept;
                nm.Status = "1";
                nm.SuccessNum = successNum.ToString();
                nm.ErrorNum = errorNum.ToString();
                NoteBLL.InsertNote(nm);
                return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "操作成功");

            }
        }

        public AjaxResult ValidateFee(string ID)
        {
            string printNumStr = "";
            List<FeeValidateModel> list = GetFeeList(ID);
            if (list == null)
            {
                return AjaxResult.Error("数据不能为空。");
            }
            else
            {
                int printNum = ConfigBLL.GetConfigPrintNum("1");
                var firstList = list.FirstOrDefault();
                var otherList = list.FirstOrDefault(O => !O.deptId.Equals(firstList.deptId) || !O.feeTime.Equals(firstList.feeTime) || !O.feeMode.Equals(firstList.feeMode) || !O.shouldMoney.Equals(firstList.shouldMoney) || !O.paidMoney.Equals(firstList.paidMoney) || !O.createrId.Equals(firstList.createrId));
                if (otherList != null)
                {
                    return AjaxResult.Error(otherList.voucherNum + "数据异常请检查所选的收费数据是否符合合并打印条件。");
                }
                else
                {
                    var printNumList = list.FirstOrDefault(O => O.printNum >= printNum);
                    if (printNumList != null)
                    {
                        return AjaxResult.Error("" + printNumList.voucherNum + "打印次数超过" + printNum.ToString() + "次");
                    }

                    List<ProveModel> pmlList = GetProveList(list);
                    var pmfirstList = pmlList.FirstOrDefault();
                    var pmotherList = pmlList.FirstOrDefault(O => !O.DeptID.Equals(pmfirstList.DeptID) || !O.ClassID.Equals(pmfirstList.ClassID) || !O.ItemID.Equals(pmfirstList.ItemID));
                    if (pmotherList != null)
                    {
                        var feelist = list.FirstOrDefault(O => O.proveId.Equals(pmotherList.ProveID));
                        return AjaxResult.Error(feelist.voucherNum + "数据异常请检查所选的收费数据是否符合合并打印条件。");
                    }
                    List<FeeDetailValidateModel> feedList = GetDetailList(ID);
                    var feedFirstList = feedList.FirstOrDefault();
                    var feedotherList = feedList.FirstOrDefault(O => !O.itemDetailId.Equals(feedFirstList.itemDetailId) || !O.canMoney.Equals(feedFirstList.canMoney) || !O.discountMoney.Equals(feedFirstList.discountMoney) || !O.shouldMoney.Equals(feedFirstList.shouldMoney) || !O.paidMoney.Equals(feedFirstList.paidMoney));
                    if (feedotherList != null)
                    {
                        return AjaxResult.Error(feedotherList.voucherNum + "数据异常请检查所选的收费数据是否符合合并打印条件。");
                    }
                    var offsetList = feedList.FirstOrDefault(O => O.offsetNum > 0 || O.byOffsetNum > 0);
                    if (feedotherList != null)
                    {
                        return AjaxResult.Error(feedotherList.voucherNum + "有充抵或被冲抵记录，不能合并打印");
                    }
                    var refundList = feedList.FirstOrDefault(O => O.RefundNum > 0);
                    if (refundList != null)
                    {
                        return AjaxResult.Error(refundList.voucherNum + "已经被核销，不能合并打印");
                    }
                    printNumStr = ConfigBLL.getNoteNum("1", "1");

                    foreach (var item in list)
                    {
                        FeeModel fm = FeeBLL.FeeModelByWhere(" and FeeID=@FeeID", new SqlParameter[] { new SqlParameter("@FeeID", item.feeId) });
                        if (!string.IsNullOrEmpty(fm.NoteNum))
                        {
                            DisableNoteModel dnm = new DisableNoteModel();
                            dnm.Status = "1";
                            dnm.FeeID = fm.FeeID;
                            dnm.NoteNum = fm.NoteNum;
                            dnm.CreateID = this.UserId.ToString();
                            dnm.CreateTime = DateTime.Now.ToString();
                            dnm.UpdateID = this.UserId.ToString();
                            dnm.UpdateTime = DateTime.Now.ToString();
                            DisableNoteBLL.InsertDisableNote(dnm);
                        }
                        fm.NoteNum = printNumStr;
                        fm.PrintNum = (Convert.ToInt32(fm.PrintNum) + 1).ToString();
                        FeeBLL.UpdateFee(fm);
                    }

                }
                DataTable feedt = FeeBLL.GetPrintMoreContent(ID);
                return AjaxResult.Success(JsonHelper.DataTableToJson(feedt), printNumStr);
            }


        }
        /// <summary>
        /// 将证书收费数据转换成List<FeeValidateModel>泛型
        /// </summary>
        /// <param name="feeIdString"></param>
        /// <returns></returns>
        private List<FeeValidateModel> GetFeeList(string feeIdString)
        {
            if (string.IsNullOrEmpty(feeIdString))
            {
                return null;
            }
            List<FeeValidateModel> list = new List<FeeValidateModel>();
            string where = " and FeeID IN(" + feeIdString + ")";
            DataTable dt = FeeBLL.FeeTableByWhere(where, null, "");
            foreach (DataRow dr in dt.Rows)
            {
                FeeValidateModel fm = new FeeValidateModel();
                fm.feeId = dr["FeeID"].ToString();
                fm.proveId = dr["ProveID"].ToString();
                fm.deptId = dr["DeptID"].ToString();
                fm.feeTime = Convert.ToDateTime(dr["FeeTime"].ToString()).ToString("yyyy-MM-dd");
                fm.feeMode = dr["FeeMode"].ToString();
                fm.shouldMoney = dr["ShouldMoney"].ToString();
                fm.paidMoney = dr["PaidMoney"].ToString();
                fm.printNum = int.Parse(dr["PrintNum"].ToString());
                fm.voucherNum = dr["VoucherNum"].ToString();
                fm.createrId = dr["CreateID"].ToString();
                list.Add(fm);
            }
            return list;
        }
        /// <summary>
        /// 获取证书List<ProveModel>泛型
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<ProveModel> GetProveList(List<FeeValidateModel> list)
        {
            List<ProveModel> pmlist = new List<ProveModel>();
            foreach (var item in list)
            {
                string where = " and ProveID=@ProveID";
                SqlParameter[] paras = new SqlParameter[] {
                      new SqlParameter("@ProveID",item.proveId)
                       };
                ProveModel pm = ProveBLL.ProveModelByWhere(where, paras);
                pmlist.Add(pm);

            }
            return pmlist;
        }

        public List<FeeDetailValidateModel> GetDetailList(string feeIdString)
        {
            List<FeeDetailValidateModel> list = new List<FeeDetailValidateModel>();
            DataTable dt = FeeBLL.SelectFeeByFeeID(feeIdString);
            foreach (DataRow dr in dt.Rows)
            {
                FeeDetailValidateModel fm = new FeeDetailValidateModel();
                fm.byOffsetNum = int.Parse(dr["ByOffsetNum"].ToString());
                fm.offsetNum = int.Parse(dr["OffsetNum"].ToString());
                fm.itemDetailId = dr["ItemDetailID"].ToString();
                fm.shouldMoney = decimal.Parse(dr["ShouldMoney"].ToString());
                fm.paidMoney = decimal.Parse(dr["PaidMoney"].ToString());
                fm.discountMoney = decimal.Parse(dr["DiscountMoney"].ToString());
                fm.canMoney = decimal.Parse(dr["CanMoney"].ToString());
                fm.voucherNum = dr["VoucherNum"].ToString();
                fm.RefundNum =  int.Parse(dr["RefundNum"].ToString());
                list.Add(fm);
            }
            return list;
        }
    }
}
