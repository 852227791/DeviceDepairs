using BLL;
using Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class iReportController : BaseController
    {
        //
        // GET: /iReport/

        public ActionResult Detail()
        {
            ViewBag.Title = "类别统计报表";
            return View();
        }

        public AjaxResult GetColumns()
        {
            //string deptId = Request.Form["txtDeptID"];
            //string menuId = Request.Form["MenuID"];
            //string whereDept = "";
            //if (!string.IsNullOrEmpty(deptId))
            //{
            //    whereDept += " AND d.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            //}
            //string powerStr = PurviewToList(menuId);
            //powerStr = string.Format(powerStr, "d.DeptID", this.UserId.ToString());
            //DataTable dt = DeptBLL.SelectDeptListToReport(whereDept + powerStr);
            string tempDeptID = "77";//dt.Rows[0]["DeptID"].ToString()
            string cmdText = @"SELECT  'DeptName1' ItemID ,
        'd_1' ParentID ,
        '单位' Name ,
        0 Queue
UNION ALL
SELECT  'i_' + CONVERT(NVARCHAR(10), i.ItemID) ,
        'i_' + CONVERT(NVARCHAR(10), i.ParentID) ,
        i.Name ,
        i.Queue
FROM    T_Pro_Item i
WHERE   Status = 1
        AND i.ParentID <> -1
        AND i.DeptID = {0}
        AND i.Sort = 2
        AND i.IsReport = 1
        AND ( ( SELECT  COUNT(ItemDetailID)
                FROM    T_Pro_ItemDetail
                WHERE   Status = 1
                        AND ItemID = i.ItemID
              ) > 0
              OR ( SELECT   COUNT(ItemDetailID)
                   FROM     T_Pro_ItemDetail
                   WHERE    Status = 1
                            AND ItemID IN (
                            SELECT  ItemID
                            FROM    dbo.GetChildrenItemID(i.ItemID) )
                 ) > 0
            )
UNION ALL
SELECT  'id_' + CONVERT(NVARCHAR(10), id.ItemDetailID) ,
        'i_' + CONVERT(NVARCHAR(10), id.ItemID) ,
        d.Name ,
        id.Queue
FROM    T_Pro_ItemDetail id
        LEFT JOIN T_Pro_Detail d ON id.DetailID = d.DetailID
WHERE   id.Status = 1
        AND id.IsReport = 1
        AND id.ItemID IN ( SELECT   ItemID
                           FROM     T_Pro_Item
                           WHERE    Status = 1
                                    AND ParentID <> -1
                                    AND DeptID = {0}
                                    AND Sort = 2 )
UNION ALL
SELECT  'DeptSum' ItemID ,
        'ds_1' ParentID ,
        '合计' Name ,
        1000 Queue
ORDER BY Queue ASC";
                        
            cmdText = string.Format(cmdText, tempDeptID);
            string json = JsonMenuTreeData.GetArrayJSON(cmdText, "ItemID", "ParentID", CommandType.Text);
            int rowSum = JsonRowSum(json) + 1;
            JArray newArr = HadleJson(json, 1, rowSum);
            //string result = newArr.ToString();
            string result = "[[";
            string jArr = "[]";
            for (int num = 0; num < rowSum; num++)
            {
                JArray tempArr = JArray.Parse(jArr);
                if (num != 0)
                {
                    result += "],[";
                }
                result += StringToJson(newArr, rowSum, ref tempArr);
                newArr = tempArr;
            }
            result += "]]";

            return AjaxResult.Success(result);
        }

        public int JsonRowSum(string initJson)
        {
            int resultSum = 0;
            JArray jsonArr = JArray.Parse(initJson);
            for (int i = 0; i < jsonArr.Count; i++)
            {
                int sum = 0;
                JObject jsonObj = JObject.Parse(jsonArr[i].ToString());
                if (jsonObj["children"] != null)
                {
                    JArray jsonChildrenArr = JArray.Parse(jsonObj["children"].ToString());
                    if (jsonChildrenArr.Count > 0)
                    {
                        sum++;
                        sum += JsonRowSum(jsonObj["children"].ToString());
                    }
                }
                if (resultSum < sum)
                {
                    resultSum = sum;
                }
            }
            return resultSum;
        }

        public int JsonColSum(string initJson)
        {
            int resultSum = 0;
            JArray jsonArr = JArray.Parse(initJson);
            for (int i = 0; i < jsonArr.Count; i++)
            {
                int sum = 0;
                JObject jsonObj = JObject.Parse(jsonArr[i].ToString());
                if (jsonObj["children"] != null)
                {
                    JArray jsonChildrenArr = JArray.Parse(jsonObj["children"].ToString());
                    if (jsonChildrenArr.Count > 0)
                    {
                        sum += JsonColSum(jsonObj["children"].ToString());
                    }
                }
                if (sum == 0)
                {
                    sum = 1;
                }
                resultSum += sum;
            }
            return resultSum;
        }

        public JArray HadleJson(string initJson, int level, int rowSum)
        {
            JArray jsonArr = JArray.Parse(initJson);
            for (int i = 0; i < jsonArr.Count; i++)
            {
                int selfRowspan = 0;
                int selfColspan = 1;
                JObject jsonObj = JObject.Parse(jsonArr[i].ToString());
                if (jsonObj["children"] != null)
                {
                    JArray jsonChildrenArr = JArray.Parse(jsonObj["children"].ToString());
                    if (jsonChildrenArr.Count > 0)
                    {
                        selfRowspan = 1;
                        jsonObj["children"] = HadleJson(jsonObj["children"].ToString(), level + 1, rowSum);
                        selfColspan = JsonColSum(jsonObj["children"].ToString());
                    }
                }
                else
                {
                    selfRowspan = rowSum - level + 1;
                }
                jsonObj.Add("rowspan", selfRowspan);
                jsonObj.Add("colspan", selfColspan);
                jsonArr[i] = jsonObj;
            }
            return jsonArr;
        }


        public string StringToJson(JArray json, int sum, ref JArray tempArr)
        {
            string result = "";
            for (int i = 0; i < json.Count; i++)
            {
                JObject jsonObj = JObject.Parse(json[i].ToString());
                string itemId = jsonObj["ItemID"].ToString();
                string name = jsonObj["Name"].ToString();
                string rowspan = jsonObj["rowspan"].ToString();
                string colspan = jsonObj["colspan"].ToString();
                string alignStr = ",halign:'center',align:'right'";
                if (itemId == "DeptName")
                {
                    alignStr = ",halign:'left',align:'left'";
                }
                if (itemId == "DeptName1")
                {
                    alignStr = ",halign:'left',align:'left', hidden: true";
                }
                result += "{field:'" + itemId + "',title:'" + name + "',rowspan:" + rowspan + ",colspan:" + colspan + alignStr + ",formatter: function(value,row,index){if (value===undefined){return '0.00';} else {return value;}}},";
                if (jsonObj["children"] != null)
                {
                    JArray jsonChildrenArr = JArray.Parse(jsonObj["children"].ToString());
                    for (int j = 0; j < jsonChildrenArr.Count; j++)
                    {
                        tempArr.Add(jsonObj["children"][j]);
                    }
                }
            }
            if (result != "")
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        //        public ActionResult GetDetailList()
        //        {
        //            string deptId = Request.Form["txtDeptID"];
        //            string feeTimeS = Request.Form["txtFeeTimeS"];
        //            string feeTimeE = Request.Form["txtFeeTimeE"];
        //            string menuId = Request.Form["MenuID"];
        //            string whereDept = "";
        //            string whereDept2 = "";
        //            string whereFee = "";
        //            if (!string.IsNullOrEmpty(deptId))
        //            {
        //                whereDept += " AND d.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
        //                whereDept2 += " AND DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
        //            }
        //            if (!string.IsNullOrEmpty(feeTimeS))
        //            {
        //                whereFee += " AND convert(NVARCHAR(10),FeeTime,23) >= '" + feeTimeS + "'";
        //            }
        //            if (!string.IsNullOrEmpty(feeTimeE))
        //            {
        //                whereFee += " AND convert(NVARCHAR(10),FeeTime,23) <= '" + feeTimeE + "'";
        //            }
        //            string powerStr1 = PurviewToList(menuId);
        //            powerStr1 = string.Format(powerStr1, "d.DeptID", this.UserId.ToString());
        //            DataTable dt1 = DeptBLL.SelectDeptListToReport(whereDept + powerStr1);
        //            if (dt1 == null)
        //            {
        //                return ResponseWriteResult("[]");
        //            }
        //            else
        //            {
        //                StringBuilder builder = new StringBuilder();
        //                builder.Append("[");
        //                string footerStr = "[{\"DeptName\":\"合计：\",";
        //                for (int i = 0; i < dt1.Rows.Count; i++)
        //                {
        //                    builder.Append("{");
        //                    builder.Append("\"DeptName\":\"" + dt1.Rows[i]["Name"].ToString().Replace("\"", "\\\"") + "\"");
        //                    string cmdText = @"SELECT  'i_' Sort ,
        //        i.ItemID ID
        //FROM    T_Pro_Item i
        //WHERE   Status = 1
        //        AND i.ParentID <> -1
        //        AND i.DeptID = {0}
        //        AND i.Sort = 2
        //        AND i.IsReport = 1
        //        AND ( ( SELECT  COUNT(ItemDetailID)
        //                FROM    T_Pro_ItemDetail
        //                WHERE   Status = 1
        //                        AND ItemID = i.ItemID
        //              ) > 0
        //              OR ( SELECT   COUNT(ItemDetailID)
        //                   FROM     T_Pro_ItemDetail
        //                   WHERE    Status = 1
        //                            AND ItemID IN (
        //                            SELECT  ItemID
        //                            FROM    dbo.GetChildrenItemID(i.ItemID) )
        //                 ) > 0
        //            )
        //UNION ALL
        //SELECT  'id_' ,
        //        id.ItemDetailID
        //FROM    T_Pro_ItemDetail id
        //WHERE   id.Status = 1
        //        AND id.IsReport = 1
        //        AND id.ItemID IN ( SELECT   ItemID
        //                           FROM     T_Pro_Item
        //                           WHERE    Status = 1
        //                                    AND ParentID <> -1
        //                                    AND DeptID = {0}
        //                                    AND Sort = 2 )";
        //                    string cmd1 = string.Format(cmdText, dt1.Rows[0]["DeptID"].ToString());
        //                    //DataTable dt2 = ItemDetailBLL.ItemDetailTableByWhere(where2, null, " ORDER BY Queue ASC");
        //                    List<Hashtable> list0 = JsonData.GetArray(cmd1);
        //                    for (int j = 0; j < list0.Count; j++)
        //                    {
        //                        builder.Append(",");
        //                        string tempDetailID = "";
        //                        if (list0[j]["Sort"].ToString() == "id_")
        //                        {
        //                            tempDetailID = @"SELECT  DetailID
        //FROM    T_Pro_ItemDetail
        //WHERE   Status = 1
        //        AND ItemDetailID = {0}";
        //                        }
        //                        else if (list0[j]["Sort"].ToString() == "i_")
        //                        {
        //                            tempDetailID = @"SELECT  DetailID
        //FROM    T_Pro_ItemDetail
        //WHERE   Status = 1
        //        AND ItemID IN ( SELECT  ItemID
        //                        FROM    dbo.GetChildrenItemID({0}) )";
        //                        }
        //                        tempDetailID = string.Format(tempDetailID, list0[j]["ID"].ToString());
        //                        string cmdText1 = @"SELECT  ISNULL(SUM(PaidMoney), 0) Sum
        //FROM    T_Inc_iFee
        //WHERE   Status IN ( 1, 2 )
        //        AND DeptID = {0}
        //        AND ItemDetailID IN (
        //        SELECT  id.ItemDetailID
        //        FROM    T_Pro_ItemDetail id
        //                LEFT JOIN T_Pro_Item i ON id.ItemID = i.ItemID
        //        WHERE   i.Sort = 2
        //                AND id.DetailID IN ( {1} ) )
        //        {2}";
        //                        cmdText1 = string.Format(cmdText1, dt1.Rows[i]["DeptID"].ToString(), tempDetailID, whereFee);
        //                        List<Hashtable> list1 = JsonData.GetArray(cmdText1);
        //                        builder.Append("\"" + list0[j]["Sort"].ToString() + list0[j]["ID"].ToString() + "\":\"" + list1[0]["Sum"].ToString().Replace("\"", "\\\"") + "\"");

        //                        if (i == 0)
        //                        {
        //                            string cmdText2 = @"SELECT  ISNULL(SUM(PaidMoney), 0) Sum
        //FROM    T_Inc_iFee
        //WHERE   Status IN ( 1, 2 )
        //        AND ItemDetailID IN (
        //        SELECT  id.ItemDetailID
        //        FROM    T_Pro_ItemDetail id
        //                LEFT JOIN T_Pro_Item i ON id.ItemID = i.ItemID
        //        WHERE   i.Sort = 2
        //                AND id.DetailID IN ( {0} ) )
        //        {1}";
        //                            string powerStr2 = PurviewToList(menuId);
        //                            powerStr2 = string.Format(powerStr2, "DeptID", this.UserId.ToString());
        //                            cmdText2 = string.Format(cmdText2, tempDetailID, whereDept2 + whereFee + powerStr2);
        //                            List<Hashtable> list2 = JsonData.GetArray(cmdText2);
        //                            footerStr += "\"" + list0[j]["Sort"].ToString() + list0[j]["ID"].ToString() + "\":\"" + list2[0]["Sum"].ToString().Replace("\"", "\\\"") + "\",";
        //                            footerStr = footerStr.Substring(0, footerStr.Length - 1);
        //                            if (j < (list0.Count - 1))
        //                            {
        //                                footerStr += ",";
        //                            }
        //                        }
        //                    }
        //                    builder.Append("}");

        //                    if (i < (dt1.Rows.Count - 1))
        //                    {
        //                        builder.Append(",");
        //                    }
        //                }
        //                builder.Append("]");
        //                footerStr += "}]";

        //                string json = @"{""rows"":" + builder + @",""total"":""" + dt1.Rows.Count + @""",""footer"":" + footerStr + "}";
        //                return ResponseWriteResult(json);
        //            }
        //        }

        public ActionResult GetDetailList()
        {
            string deptId = Request.Form["txtDeptID"];
            string feeTimeS = Request.Form["txtFeeTimeS"];
            string feeTimeE = Request.Form["txtFeeTimeE"];
            string menuId = Request.Form["MenuID"];
            string isparent = Request.Form["isParent"];
            string whereDept = "";
            string whereDept2 = "";
            string whereFee = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                whereDept += " AND d.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
                whereDept2 += " AND DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            if (!string.IsNullOrEmpty(feeTimeS))
            {
                whereFee += " AND convert(NVARCHAR(10),FeeTime,23) >= '" + feeTimeS + "'";
            }
            if (!string.IsNullOrEmpty(feeTimeE))
            {
                whereFee += " AND convert(NVARCHAR(10),FeeTime,23) <= '" + feeTimeE + "'";
            }
            if (isparent == "Yes")
            {
                whereDept += " AND d.ParentID = 1";
            }
            string powerStr1 = PurviewToList(menuId);
            powerStr1 = string.Format(powerStr1, "d.DeptID", this.UserId.ToString());
            DataTable dt1 = DeptBLL.SelectDeptListToReport(whereDept + powerStr1);
            if (dt1.Rows.Count == 0)
            {
                return ResponseWriteResult(@"{""rows"":[],""total"":""0"",""footer"":[]}");
            }
            else
            {
                IList<Hashtable> listSum = new List<Hashtable>();
                string footerStr = "[{\"DeptName\":\"合计：\",\"DeptName1\":\"合计：\",";

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    Hashtable h = new Hashtable();
                    h.Add("DeptID", dt1.Rows[i]["DeptID"].ToString());
                    h.Add("ParentID", dt1.Rows[i]["ParentID"].ToString());
                    h.Add("DeptName", dt1.Rows[i]["Name"].ToString().Replace("\"", "\\\""));
                    h.Add("DeptName1", dt1.Rows[i]["Name"].ToString().Replace("\"", "\\\""));

                    string cmdText = @"SELECT  a.Sort Sort ,
        CONVERT(NVARCHAR(10), a.ID) ID ,
        ( SELECT    ISNULL(SUM(PaidMoney), 0) Sum
          FROM      T_Inc_iFee
          WHERE     Status IN ( 1, 2 )
                    AND DeptID IN ( SELECT  DeptID
                                    FROM    dbo.GetChildrenDeptID({1}) )
                    AND ItemDetailID IN (
                    SELECT  id.ItemDetailID
                    FROM    T_Pro_ItemDetail id
                            LEFT JOIN T_Pro_Item i ON id.ItemID = i.ItemID
                    WHERE   i.Sort = 2
                            AND id.DetailID IN ( ( SELECT   rc.DetailID
                                                   FROM     T_Inc_iReportConfig rc
                                                   WHERE    rc.Sort = a.Sort
                                                            AND rc.ID = a.ID
                                                 )
                                            ) )
                    {2}
        ) Sum
FROM    ( SELECT    'i_' Sort ,
                    i.ItemID ID
          FROM      T_Pro_Item i
          WHERE     Status = 1
                    AND i.ParentID <> -1
                    AND i.DeptID IN ( SELECT    DeptID
                                      FROM      dbo.GetChildrenDeptID({0}) )
                    AND i.Sort = 2
                    AND i.IsReport = 1
                    AND ( ( SELECT  COUNT(ItemDetailID)
                            FROM    T_Pro_ItemDetail
                            WHERE   Status = 1
                                    AND ItemID = i.ItemID
                          ) > 0
                          OR ( SELECT   COUNT(ItemDetailID)
                               FROM     T_Pro_ItemDetail
                               WHERE    Status = 1
                                        AND ItemID IN (
                                        SELECT  ItemID
                                        FROM    dbo.GetChildrenItemID(i.ItemID) )
                             ) > 0
                        )
          UNION ALL
          SELECT    'id_' ,
                    id.ItemDetailID
          FROM      T_Pro_ItemDetail id
          WHERE     id.Status = 1
                    AND id.IsReport = 1
                    AND id.ItemID IN (
                    SELECT  ItemID
                    FROM    T_Pro_Item
                    WHERE   Status = 1
                            AND ParentID <> -1
                            AND DeptID IN ( SELECT  DeptID
                                            FROM    dbo.GetChildrenDeptID({0}) )
                            AND Sort = 2 )
        ) AS a
UNION ALL
SELECT  'Dept' ,
        'Sum' ,
        ISNULL(SUM(PaidMoney), 0)
FROM    T_Inc_iFee
WHERE   Status IN ( 1, 2 )
        AND DeptID IN ( SELECT  DeptID
                        FROM    dbo.GetChildrenDeptID({1}) )
        {2}";
                    cmdText = string.Format(cmdText, "77", dt1.Rows[i]["DeptID"].ToString(), whereFee);
                    List<Hashtable> list = JsonData.GetArray(cmdText);
                    for (int j = 0; j < list.Count; j++)
                    {
                        h.Add(list[j]["Sort"].ToString() + list[j]["ID"].ToString(), list[j]["Sum"].ToString().Replace("\"", "\\\""));
                    }

                    if (i == 0)
                    {
                        string cmdText2 = @"SELECT  a.Sort Sort ,
        CONVERT(NVARCHAR(10), a.ID) ID ,
        ( SELECT    ISNULL(SUM(PaidMoney), 0) Sum
          FROM      T_Inc_iFee
          WHERE     Status IN ( 1, 2 )
                    AND ItemDetailID IN (
                    SELECT  id.ItemDetailID
                    FROM    T_Pro_ItemDetail id
                            LEFT JOIN T_Pro_Item i ON id.ItemID = i.ItemID
                    WHERE   i.Sort = 2
                            AND id.DetailID IN ( ( SELECT   rc.DetailID
                                                   FROM     T_Inc_iReportConfig rc
                                                   WHERE    rc.Sort = a.Sort
                                                            AND rc.ID = a.ID
                                                 )
                                            ) )
                    {1}
        ) Sum
FROM    ( SELECT    'i_' Sort ,
                    i.ItemID ID
          FROM      T_Pro_Item i
          WHERE     Status = 1
                    AND i.ParentID <> -1
                    AND i.DeptID IN ( SELECT    DeptID
                                      FROM      dbo.GetChildrenDeptID({0}) )
                    AND i.Sort = 2
                    AND i.IsReport = 1
                    AND ( ( SELECT  COUNT(ItemDetailID)
                            FROM    T_Pro_ItemDetail
                            WHERE   Status = 1
                                    AND ItemID = i.ItemID
                          ) > 0
                          OR ( SELECT   COUNT(ItemDetailID)
                               FROM     T_Pro_ItemDetail
                               WHERE    Status = 1
                                        AND ItemID IN (
                                        SELECT  ItemID
                                        FROM    dbo.GetChildrenItemID(i.ItemID) )
                             ) > 0
                        )
          UNION ALL
          SELECT    'id_' ,
                    id.ItemDetailID
          FROM      T_Pro_ItemDetail id
          WHERE     id.Status = 1
                    AND id.IsReport = 1
                    AND id.ItemID IN (
                    SELECT  ItemID
                    FROM    T_Pro_Item
                    WHERE   Status = 1
                            AND ParentID <> -1
                            AND DeptID IN ( SELECT  DeptID
                                            FROM    dbo.GetChildrenDeptID({0}) )
                            AND Sort = 2 )
        ) AS a
UNION ALL
SELECT  'Dept' ,
        'Sum' ,
        ISNULL(SUM(PaidMoney), 0)
FROM    T_Inc_iFee
WHERE   Status IN ( 1, 2 )
        {1}";
                        string powerStr2 = PurviewToList(menuId);
                        powerStr2 = string.Format(powerStr2, "DeptID", this.UserId.ToString());
                        cmdText2 = string.Format(cmdText2, "77", whereDept2 + whereFee + powerStr2);
                        List<Hashtable> list2 = JsonData.GetArray(cmdText2);
                        for (int k = 0; k < list2.Count; k++)
                        {
                            footerStr += "\"" + list2[k]["Sort"].ToString() + list2[k]["ID"].ToString() + "\":\"" + list2[k]["Sum"].ToString().Replace("\"", "\\\"") + "\",";
                        }
                        footerStr = footerStr.Substring(0, footerStr.Length - 1);
                    }
                    listSum.Add(h);
                }
                var obj = JsonMenuTreeData.ArrayToTreeData(listSum, "DeptID", "ParentID");
                string rows = JsonHelper.ToJson(obj);
                footerStr += "}]";

                string json = @"{""rows"":" + rows + @",""total"":""" + dt1.Rows.Count + @""",""footer"":" + footerStr + "}";
                return ResponseWriteResult(json);
            }
        }
    }
}
