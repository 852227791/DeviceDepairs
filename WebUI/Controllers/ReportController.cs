using Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ReportController : BaseController
    {
        //
        // GET: /Report/

        public ActionResult Dept()
        {
            ViewBag.Title = "校区统计报表";
            return View();
        }

        public ActionResult Prove()
        {
            ViewBag.Title = "证书统计报表";
            return View();
        }

        public ActionResult Class()
        {
            ViewBag.Title = "班级统计报表";
            return View();
        }


        public ActionResult GetDeptList()
        {
            string deptId = Request.Form["txtDeptID"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string clear = Request.Form["Clear"];
            string menuId = Request.Form["MenuID"];
            string where1 = "";
            string where2 = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where1 += " AND d.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) <= '" + feeTimeE + "'";
            }
            string cmdText = "";
            if (clear == "No")
            {
                cmdText = @"SELECT  d.DeptID ,
        ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = d.ParentID
        ) ParentName ,
        d.Name ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status IN ( 2, 3, 4, 5 )
                                            AND p.DeptID = d.DeptID )
                    AND f.Status <> 9 {1}
        ) Num ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.DeptID = d.DeptID
                    AND f.Status <> 9 {1}
        ) ShouldMoney ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.DeptID = d.DeptID
                    AND f.Status <> 9 {1}
        ) PaidMoney ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN ( SELECT    f.FeeID
                                      FROM      T_Pro_Fee f
                                      WHERE     f.DeptID = d.DeptID
                                                AND f.Status <> 9 {1} )
        ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) Offset ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) ByOffset ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        ) RefundMoney
FROM    T_Sys_Dept d
WHERE   d.Status = 1
        AND ( SELECT    COUNT(DeptID)
              FROM      T_Sys_Dept
              WHERE     Status = 1
                        AND ParentID = d.DeptID
            ) = 0 {0}";
            }
            else
            {
                cmdText = @"SELECT  d.DeptID ,
        ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = d.ParentID
        ) ParentName ,
        d.Name ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status IN ( 2, 3, 4, 5 )
                                            AND p.DeptID = d.DeptID )
                    AND f.Status <> 9 {1}
        ) Num ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.DeptID = d.DeptID
                    AND f.Status <> 9 {1}
        ) - ( SELECT    ISNULL(SUM(fd.ShouldMoney), 0)
              FROM      T_Pro_FeeDetail fd
              WHERE     fd.Status = 1
                        AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                 FROM   T_Pro_ItemDetail
                                                 WHERE  Status = 1
                                                        AND DetailID = 4 )
                        AND fd.FeeID IN ( SELECT    f.FeeID
                                          FROM      T_Pro_Fee f
                                          WHERE     f.DeptID = d.DeptID
                                                    AND f.Status <> 9 {1} )
            ) ShouldMoney ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.DeptID = d.DeptID
                    AND f.Status <> 9 {1}
        ) - ( SELECT    ISNULL(SUM(fd.PaidMoney), 0)
              FROM      T_Pro_FeeDetail fd
              WHERE     fd.Status = 1
                        AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                 FROM   T_Pro_ItemDetail
                                                 WHERE  Status = 1
                                                        AND DetailID = 4 )
                        AND fd.FeeID IN ( SELECT    f.FeeID
                                          FROM      T_Pro_Fee f
                                          WHERE     f.DeptID = d.DeptID
                                                    AND f.Status <> 9 {1} )
            ) PaidMoney ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN ( SELECT    f.FeeID
                                      FROM      T_Pro_Fee f
                                      WHERE     f.DeptID = d.DeptID
                                                AND f.Status <> 9 {1} )
        ) - ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
              FROM      T_Pro_FeeDetail fd
              WHERE     fd.Status = 1
                        AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                 FROM   T_Pro_ItemDetail
                                                 WHERE  Status = 1
                                                        AND DetailID = 4 )
                        AND fd.FeeID IN ( SELECT    f.FeeID
                                          FROM      T_Pro_Fee f
                                          WHERE     f.DeptID = d.DeptID
                                                    AND f.Status <> 9 {1} )
            ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
          ) Offset ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
          ) ByOffset ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        )
        - ( SELECT  ISNULL(SUM(r.RefundMoney), 0)
            FROM    T_Pro_Refund r
            WHERE   r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
          ) RefundMoney
FROM    T_Sys_Dept d
WHERE   d.Status = 1
        AND ( SELECT    COUNT(DeptID)
              FROM      T_Sys_Dept
              WHERE     Status = 1
                        AND ParentID = d.DeptID
            ) = 0 {0}";
            }
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "d.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where1 + powerStr, where2);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "Num,ShouldMoney,PaidMoney,DiscountMoney,Offset,ByOffset,RefundMoney"));
        }

        public AjaxResult DeptDownload()
        {
            string deptId = Request.Form["txtDeptID"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string clear = Request.Form["Clear"];
            string menuId = Request.Form["MenuID"];
            string where1 = "";
            string where2 = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where1 += " AND d.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) <= '" + feeTimeE + "'";
            }
            string cmdText = "";
            if (clear == "No")
            {
                cmdText = @"SELECT  ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = d.ParentID
        ) 学校 ,
        d.Name 校区 ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status IN ( 2, 3, 4, 5 )
                                            AND p.DeptID = d.DeptID )
                    AND f.Status <> 9 {1}
        ) 报考人次 ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.DeptID = d.DeptID
                    AND f.Status <> 9 {1}
        ) 应交金额 ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.DeptID = d.DeptID
                    AND f.Status <> 9 {1}
        ) 实交金额 ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN ( SELECT    f.FeeID
                                      FROM      T_Pro_Fee f
                                      WHERE     f.DeptID = d.DeptID
                                                AND f.Status <> 9 {1} )
        ) 优惠金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) 充抵金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) 被充抵金额 ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        ) 核销金额
FROM    T_Sys_Dept d
WHERE   d.Status = 1
        AND ( SELECT    COUNT(DeptID)
              FROM      T_Sys_Dept
              WHERE     Status = 1
                        AND ParentID = d.DeptID
            ) = 0 {0}";
            }
            else
            {
                cmdText = @"SELECT  d.DeptID ,
        ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = d.ParentID
        ) 学校 ,
        d.Name 校区 ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status IN ( 2, 3, 4, 5 )
                                            AND p.DeptID = d.DeptID )
                    AND f.Status <> 9 {1}
        ) 报考人次 ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.DeptID = d.DeptID
                    AND f.Status <> 9 {1}
        ) - ( SELECT    ISNULL(SUM(fd.ShouldMoney), 0)
              FROM      T_Pro_FeeDetail fd
              WHERE     fd.Status = 1
                        AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                 FROM   T_Pro_ItemDetail
                                                 WHERE  Status = 1
                                                        AND DetailID = 4 )
                        AND fd.FeeID IN ( SELECT    f.FeeID
                                          FROM      T_Pro_Fee f
                                          WHERE     f.DeptID = d.DeptID
                                                    AND f.Status <> 9 {1} )
            ) 应收金额 ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.DeptID = d.DeptID
                    AND f.Status <> 9 {1}
        ) - ( SELECT    ISNULL(SUM(fd.PaidMoney), 0)
              FROM      T_Pro_FeeDetail fd
              WHERE     fd.Status = 1
                        AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                 FROM   T_Pro_ItemDetail
                                                 WHERE  Status = 1
                                                        AND DetailID = 4 )
                        AND fd.FeeID IN ( SELECT    f.FeeID
                                          FROM      T_Pro_Fee f
                                          WHERE     f.DeptID = d.DeptID
                                                    AND f.Status <> 9 {1} )
            ) 实收金额 ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN ( SELECT    f.FeeID
                                      FROM      T_Pro_Fee f
                                      WHERE     f.DeptID = d.DeptID
                                                AND f.Status <> 9 {1} )
        ) - ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
              FROM      T_Pro_FeeDetail fd
              WHERE     fd.Status = 1
                        AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                 FROM   T_Pro_ItemDetail
                                                 WHERE  Status = 1
                                                        AND DetailID = 4 )
                        AND fd.FeeID IN ( SELECT    f.FeeID
                                          FROM      T_Pro_Fee f
                                          WHERE     f.DeptID = d.DeptID
                                                    AND f.Status <> 9 {1} )
            ) 优惠金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
          ) 充抵金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
          ) 被充抵金额 ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        )
        - ( SELECT  ISNULL(SUM(r.RefundMoney), 0)
            FROM    T_Pro_Refund r
            WHERE   r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN ( SELECT    f.FeeID
                                              FROM      T_Pro_Fee f
                                              WHERE     f.DeptID = d.DeptID
                                                        AND f.Status <> 9 {1} ) )
          ) 核销金额
FROM    T_Sys_Dept d
WHERE   d.Status = 1
        AND ( SELECT    COUNT(DeptID)
              FROM      T_Sys_Dept
              WHERE     Status = 1
                        AND ParentID = d.DeptID
            ) = 0 {0}";
            }
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "d.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where1 + powerStr, where2);
            string filename = "证书收费校区报表.xls";
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }


        public ActionResult GetProveList()
        {
            string deptId = Request.Form["txtDeptID"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string clear = Request.Form["Clear"];
            string menuId = Request.Form["MenuID"];
            string where1 = "";
            string where2 = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where1 += " AND i.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) <= '" + feeTimeE + "'";
            }
            string cmdText = "";
            if (clear == "No")
            {
                cmdText = @"SELECT  i.ItemID ,
        ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = i.DeptID
        ) DeptName ,
        ( SELECT    i1.Name
          FROM      T_Pro_Item i1
          WHERE     i1.Status = 1
                    AND i1.ItemID = i.ParentID
        ) ParentName ,
        i.Name ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status IN ( 2, 3, 4, 5 )
                                            AND p.ItemID = i.ItemID )
                    AND f.Status <> 9 {1}
        ) Num ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                    AND f.Status <> 9 {1}
        ) ShouldMoney ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                    AND f.Status <> 9 {1}
        ) PaidMoney ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ItemID = i.ItemID )
                            AND f.Status <> 9 {1} )
        ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) Offset ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) ByOffset ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        ) RefundMoney
FROM    T_Pro_Item i
WHERE   i.Status = 1
        AND ( SELECT    COUNT(DeptID)
              FROM      T_Pro_Item
              WHERE     Status = 1
                        AND ParentID = i.ItemID
            ) = 0 {0}";
            }
            else
            {
                cmdText = @"SELECT  i.ItemID ,
        ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = i.DeptID
        ) DeptName ,
        ( SELECT    i1.Name
          FROM      T_Pro_Item i1
          WHERE     i1.Status = 1
                    AND i1.ItemID = i.ParentID
        ) ParentName ,
        i.Name ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status IN ( 2, 3, 4, 5 )
                                            AND p.ItemID = i.ItemID )
                    AND f.Status <> 9 {1}
        ) Num ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                    AND f.Status <> 9
        )
        - ( SELECT  ISNULL(SUM(fd.ShouldMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ItemID = i.ItemID )
                            AND f.Status <> 9 {1} )
          ) ShouldMoney ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                    AND f.Status <> 9 {1}
        )
        - ( SELECT  ISNULL(SUM(fd.PaidMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ItemID = i.ItemID )
                            AND f.Status <> 9 {1} )
          ) PaidMoney ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ItemID = i.ItemID )
                            AND f.Status <> 9 {1} )
        )
        - ( SELECT  ISNULL(SUM(fd.DiscountMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ItemID = i.ItemID )
                            AND f.Status <> 9 {1} )
          ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
          ) Offset ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
          ) ByOffset ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        )
        - ( SELECT  ISNULL(SUM(r.RefundMoney), 0)
            FROM    T_Pro_Refund r
            WHERE   r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
          ) RefundMoney
FROM    T_Pro_Item i
WHERE   i.Status = 1
        AND i.Sort = 1
        AND ( SELECT    COUNT(DeptID)
              FROM      T_Pro_Item
              WHERE     Status = 1
                        AND ParentID = i.ItemID
            ) = 0 {0}";
            }
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "i.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where1 + powerStr, where2);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "Num,ShouldMoney,PaidMoney,DiscountMoney,Offset,ByOffset,RefundMoney"));
        }

        public AjaxResult ProveDownload()
        {
            string deptId = Request.Form["txtDeptID"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string clear = Request.Form["Clear"];
            string menuId = Request.Form["MenuID"];
            string where1 = "";
            string where2 = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where1 += " AND i.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) <= '" + feeTimeE + "'";
            }
            string cmdText = "";
            if (clear == "No")
            {
                cmdText = @"SELECT  
        ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = i.DeptID
        ) 校区 ,
        ( SELECT    i1.Name
          FROM      T_Pro_Item i1
          WHERE     i1.Status = 1
                    AND i1.ItemID = i.ParentID
        ) 证书分类 ,
        i.Name 证书名称 ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID in ( SELECT    p.ProveID
                                  FROM      T_Pro_Prove p
                                  WHERE     p.Status IN ( 2, 3, 4, 5 )
                                            AND p.ItemID = i.ItemID
                                )
                    AND f.Status <> 9 {1}
        ) 报考人次 ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID in ( SELECT    p.ProveID
                                  FROM      T_Pro_Prove p
                                  WHERE     p.Status <> 9
                                            AND p.ItemID = i.ItemID
                                )
                    AND f.Status <> 9 {1}
        ) 应交金额 ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID in ( SELECT    p.ProveID
                                  FROM      T_Pro_Prove p
                                  WHERE     p.Status <> 9
                                            AND p.ItemID = i.ItemID
                                )
                    AND f.Status <> 9 {1}
        ) 实交金额 ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ItemID = i.ItemID )
                            AND f.Status <> 9 {1} )
        ) 优惠金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) 充抵金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) 被充抵金额 ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        ) 核销金额
FROM    T_Pro_Item i
WHERE   i.Status = 1
        AND ( SELECT    COUNT(DeptID)
              FROM      T_Pro_Item
              WHERE     Status = 1
                        AND ParentID = i.ItemID
            ) = 0 {0}";
            }
            else
            {
                cmdText = @"SELECT  i.ItemID ,
        ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = i.DeptID
        ) 校区 ,
        ( SELECT    i1.Name
          FROM      T_Pro_Item i1
          WHERE     i1.Status = 1
                    AND i1.ItemID = i.ParentID
        ) 证书分类 ,
        i.Name 证书名称 ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status IN ( 2, 3, 4, 5 )
                                            AND p.ItemID = i.ItemID )
                    AND f.Status <> 9 {1}
        ) 报考人次 ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                    AND f.Status <> 9
        )
        - ( SELECT  ISNULL(SUM(fd.ShouldMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ItemID = i.ItemID )
                            AND f.Status <> 9 {1} )
          ) 应收金额 ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                    AND f.Status <> 9 {1}
        )
        - ( SELECT  ISNULL(SUM(fd.PaidMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ItemID = i.ItemID )
                            AND f.Status <> 9 {1} )
          ) 实收金额 ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ItemID = i.ItemID )
                            AND f.Status <> 9 {1} )
        )
        - ( SELECT  ISNULL(SUM(fd.DiscountMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ItemID = i.ItemID )
                            AND f.Status <> 9 {1} )
          ) 优惠金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
          ) 充抵金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
          ) 被充抵金额 ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        )
        - ( SELECT  ISNULL(SUM(r.RefundMoney), 0)
            FROM    T_Pro_Refund r
            WHERE   r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ItemID = i.ItemID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
          ) 核销金额
FROM    T_Pro_Item i
WHERE   i.Status = 1
        AND i.Sort = 1
        AND ( SELECT    COUNT(DeptID)
              FROM      T_Pro_Item
              WHERE     Status = 1
                        AND ParentID = i.ItemID
            ) = 0 {0}";
            }
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "i.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where1 + powerStr, where2);
            string filename = "证书收费证书报表.xls";
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }


        public ActionResult GetClassList()
        {
            string deptId = Request.Form["txtDeptID"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string clear = Request.Form["Clear"];
            string menuId = Request.Form["MenuID"];
            string where1 = "";
            string where2 = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where1 += " AND p.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) <= '" + feeTimeE + "'";
            }
            string cmdText = "";
            if (clear == "No")
            {
                cmdText = @"SELECT  c.ClassID ,
        ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = p.DeptID
        ) DeptName ,
        ( SELECT    p.Name
          FROM      T_Pro_Profession p
          WHERE     p.Status = 1
                    AND p.ProfessionID = c.ProfessionID
        ) ParentName ,
        c.Name ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status IN ( 2, 3, 4, 5 )
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        ) Num ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        ) ShouldMoney ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        ) PaidMoney ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ClassID = c.ClassID )
                            AND f.Status <> 9 {1} )
        ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) Offset ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) ByOffset ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        ) RefundMoney
FROM    T_Pro_Class c
        LEFT JOIN T_Pro_Profession p ON c.ProfessionID = p.ProfessionID
WHERE   c.Status = 1 {0}";
            }
            else
            {
                cmdText = @"SELECT  c.ClassID ,
        ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = p.DeptID
        ) DeptName ,
        ( SELECT    p.Name
          FROM      T_Pro_Profession p
          WHERE     p.Status = 1
                    AND p.ProfessionID = c.ProfessionID
        ) ParentName ,
        c.Name ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status IN ( 2, 3, 4, 5 )
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        ) Num ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        )
        - ( SELECT  ISNULL(SUM(fd.ShouldMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ClassID = c.ClassID )
                            AND f.Status <> 9 {1} )
          ) ShouldMoney ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        )
        - ( SELECT  ISNULL(SUM(fd.PaidMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ClassID = c.ClassID )
                            AND f.Status <> 9 {1} )
          ) PaidMoney ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ClassID = c.ClassID )
                            AND f.Status <> 9 {1} )
        )
        - ( SELECT  ISNULL(SUM(fd.DiscountMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ClassID = c.ClassID )
                            AND f.Status <> 9 {1} )
          ) DiscountMoney ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
          ) Offset ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
          ) ByOffset ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        )
        - ( SELECT  ISNULL(SUM(r.RefundMoney), 0)
            FROM    T_Pro_Refund r
            WHERE   r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
          ) RefundMoney
FROM    T_Pro_Class c
        LEFT JOIN T_Pro_Profession p ON c.ProfessionID = p.ProfessionID
WHERE   c.Status = 1 {0}";
            }
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "p.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where1 + powerStr, where2);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, "Num,ShouldMoney,PaidMoney,DiscountMoney,Offset,ByOffset,RefundMoney"));
        }

        public AjaxResult ClassDownload()
        {
            string deptId = Request.Form["txtDeptID"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string clear = Request.Form["Clear"];
            string menuId = Request.Form["MenuID"];
            string where1 = "";
            string where2 = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                where1 += " AND p.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                where2 += " AND convert(NVARCHAR(10),f.FeeTime,23) <= '" + feeTimeE + "'";
            }
            string cmdText = "";
            if (clear == "No")
            {
                cmdText = @"SELECT  
        ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = p.DeptID
        ) 校区 ,
        ( SELECT    p.Name
          FROM      T_Pro_Profession p
          WHERE     p.Status = 1
                    AND p.ProfessionID = c.ProfessionID
        ) 专业 ,
        c.Name 班级 ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status IN ( 2, 3, 4, 5 )
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        ) 报考人次 ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        ) 应交金额 ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        ) 实交金额 ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ClassID = c.ClassID )
                            AND f.Status <> 9 {1} )
        ) 优惠金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) 充抵金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        ) 被充抵金额 ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        ) 核销金额
FROM    T_Pro_Class c
        LEFT JOIN T_Pro_Profession p ON c.ProfessionID = p.ProfessionID
WHERE   c.Status = 1 {0}";
            }
            else
            {
                cmdText = @"SELECT  c.ClassID ,
        ( SELECT    d1.Name
          FROM      T_Sys_Dept d1
          WHERE     d1.Status = 1
                    AND d1.DeptID = p.DeptID
        ) 校区 ,
        ( SELECT    p.Name
          FROM      T_Pro_Profession p
          WHERE     p.Status = 1
                    AND p.ProfessionID = c.ProfessionID
        ) 专业 ,
        c.Name 班级 ,
        ( SELECT    COUNT(DISTINCT f.ProveID)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status IN ( 2, 3, 4, 5 )
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        ) 报考人次 ,
        ( SELECT    ISNULL(SUM(f.ShouldMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        )
        - ( SELECT  ISNULL(SUM(fd.ShouldMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ClassID = c.ClassID )
                            AND f.Status <> 9 {1} )
          ) 应收金额 ,
        ( SELECT    ISNULL(SUM(f.PaidMoney), 0)
          FROM      T_Pro_Fee f
          WHERE     f.ProveID IN ( SELECT   p.ProveID
                                   FROM     T_Pro_Prove p
                                   WHERE    p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                    AND f.Status <> 9 {1}
        )
        - ( SELECT  ISNULL(SUM(fd.PaidMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ClassID = c.ClassID )
                            AND f.Status <> 9 {1} )
          ) 实收金额 ,
        ( SELECT    ISNULL(SUM(fd.DiscountMoney), 0)
          FROM      T_Pro_FeeDetail fd
          WHERE     fd.Status = 1
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ClassID = c.ClassID )
                            AND f.Status <> 9 {1} )
        )
        - ( SELECT  ISNULL(SUM(fd.DiscountMoney), 0)
            FROM    T_Pro_FeeDetail fd
            WHERE   fd.Status = 1
                    AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                             FROM   T_Pro_ItemDetail
                                             WHERE  Status = 1
                                                    AND DetailID = 4 )
                    AND fd.FeeID IN (
                    SELECT  f.FeeID
                    FROM    T_Pro_Fee f
                    WHERE   f.ProveID IN ( SELECT   p.ProveID
                                           FROM     T_Pro_Prove p
                                           WHERE    p.Status <> 9
                                                    AND p.ClassID = c.ClassID )
                            AND f.Status <> 9 {1} )
          ) 优惠金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
          ) 充抵金额 ,
        ( SELECT    ISNULL(SUM(o.Money), 0)
          FROM      T_Pro_Offset o
          WHERE     o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
        )
        - ( SELECT  ISNULL(SUM(o.Money), 0)
            FROM    T_Pro_Offset o
            WHERE   o.ByFeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND o.Status = 1
          ) 被充抵金额 ,
        ( SELECT    ISNULL(SUM(r.RefundMoney), 0)
          FROM      T_Pro_Refund r
          WHERE     r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
        )
        - ( SELECT  ISNULL(SUM(r.RefundMoney), 0)
            FROM    T_Pro_Refund r
            WHERE   r.FeeDetailID IN (
                    SELECT  fd.FeeDetailID
                    FROM    T_Pro_FeeDetail fd
                    WHERE   fd.Status = 1
                            AND fd.ItemDetailID IN ( SELECT ItemDetailID
                                                     FROM   T_Pro_ItemDetail
                                                     WHERE  Status = 1
                                                            AND DetailID = 4 )
                            AND fd.FeeID IN (
                            SELECT  f.FeeID
                            FROM    T_Pro_Fee f
                            WHERE   f.ProveID IN (
                                    SELECT  p.ProveID
                                    FROM    T_Pro_Prove p
                                    WHERE   p.Status <> 9
                                            AND p.ClassID = c.ClassID )
                                    AND f.Status <> 9 {1} ) )
                    AND r.Status = 1
          ) 核销金额
FROM    T_Pro_Class c
        LEFT JOIN T_Pro_Profession p ON c.ProfessionID = p.ProfessionID
WHERE   c.Status = 1 {0}";
            }
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "p.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where1 + powerStr, where2);
            string filename = "证书收费班级报表.xls";
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }
    }
}
