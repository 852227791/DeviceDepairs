using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebUI.Controllers
{
    public class sOrderAddController : BaseController
    {
        #region 页面
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sOrderAddList()
        {
            return View();
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sOrderAdd()
        {
            return View();
        }

        /// <summary>
        /// 查看页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sOrderAddView()
        {
            return View();
        }

        /// <summary>
        /// 批量添加结果页面
        /// </summary>
        /// <returns></returns>
        public ActionResult sOrderAddResult()
        {
            return View();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 得到批量缴费单列表信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetsOrderAddInfo()
        {
            string menuId = Request.Form["MenuID"];
            string name = Request.Form["txtName"];
            string dept = Request.Form["treeDept"];
            string year = Request.Form["selYear"];
            string month = Request.Form["selMonth"];
            //string major = Request.Form["txtMajor"];
            string planname = Request.Form["txtPlanName"];
            string numname = Request.Form["txtNumName"];
            string createname = Request.Form["txtCreateName"];
            string createTimeS = Request.Form["txtCreateTimeS"];
            string createTimeE = Request.Form["txtCreateTimeE"];
            string selStatus = Request.Form["selStatus"];
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND sorderadd.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(dept))
            {
                where += " AND sorderadd.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(year))
            {
                where += OtherHelper.MultiSelectToSQLWhere(year, "sorderadd.Year");
            }
            if (!string.IsNullOrEmpty(month))
            {
                where += OtherHelper.MultiSelectToSQLWhere(month, "sorderadd.Month");
            }
            //if (!string.IsNullOrEmpty(major))
            //{
            //    where += " AND pro.Name like '%" + major + "%'";
            //}
            if (!string.IsNullOrEmpty(planname))
            {
                where += " AND item.Name like '%" + planname + "%'";
            }
            if (!string.IsNullOrEmpty(numname))
            {
                where += " AND item_num.Name like '%" + numname + "%'";
            }
            if (!string.IsNullOrEmpty(createname))
            {
                where += " AND sysuser.Name like '%" + createname + "%'";
            }
            if (!string.IsNullOrEmpty(createTimeS))
            {
                where += " AND convert(NVARCHAR(10),sorderadd.CreateTime,23) >= '" + createTimeS + "'";
            }
            if (!string.IsNullOrEmpty(createTimeE))
            {
                where += " AND convert(NVARCHAR(10),sorderadd.CreateTime,23) <= '" + createTimeE + "'";
            }
            if (!string.IsNullOrEmpty(selStatus))
            {
                where += OtherHelper.MultiSelectToSQLWhere(selStatus, "sorderadd.Status");
            }
            string cmdText = @"SELECT    sorderadd.sOrderAddID ,
            sorderadd.Status ,
            refe.RefeName AS StatusName ,
            sorderadd.Name ,
            dept.Name AS DeptName ,
            sorderadd.Year ,
            refe_month.RefeName AS Month ,
            ( SELECT    pro.Name + ','
              FROM      T_Stu_sOrderAddsProfession AS sorderpro
                        LEFT JOIN T_Stu_sProfession AS spro ON sorderpro.sProfessionID = spro.sProfessionID
                        LEFT JOIN T_Pro_Profession AS pro ON pro.ProfessionID = spro.ProfessionID
              WHERE     sorderpro.sOrderAddID = sorderadd.sOrderAddID
            FOR
              XML PATH('')
            ) AS Major ,
            item.Name AS PlanName ,
            item_num.Name AS NumName ,
            sorderadd.Remark ,
            sysuser.Name AS CreateName ,
            CONVERT(NVARCHAR(23), sorderadd.CreateTime, 23) AS CreateTime
  FROM      T_Stu_sOrderAdd AS sorderadd
            LEFT JOIN T_Sys_Refe AS refe ON refe.Value = sorderadd.Status
                                                AND refe.RefeTypeID = 23
            LEFT JOIN T_Sys_Dept AS dept ON dept.DeptID = sorderadd.DeptID
            LEFT JOIN T_Sys_Refe AS refe_month ON refe_month.Value = sorderadd.Month
                                                      AND refe_month.RefeTypeID = 18
            LEFT JOIN T_Pro_Item AS item ON item.ItemID = sorderadd.PlanItemID
            LEFT JOIN T_Pro_Item AS item_num ON item_num.ItemID = sorderadd.NumItemID
            LEFT JOIN T_Sys_User AS sysuser ON sysuser.UserID = sorderadd.CreateID 
WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "sorderadd.DeptID", "sorderadd.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, ""));
        }

        /// <summary>
        /// 批量添加缴费单
        /// </summary>
        /// <returns></returns>
        public string GetsOrderAdd()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                string str = "出现未知错误，请联系管理员";
                try
                {
                    #region 获取数据并验证
                    string sOrderContent = Request.Form["sOrderContent"];
                    string Name = Request.Form["Name"];
                    string DeptID = Request.Form["DeptID"];
                    string Year = Request.Form["Year"];
                    string Month = Request.Form["Month"];
                    string sProfessionID = Request.Form["sProfessionID"];
                    string Scheme = Request.Form["Scheme"];
                    string Semester = Request.Form["Semester"];
                    string Remark = Request.Form["Remark"];
                    if (string.IsNullOrEmpty(Name))
                    {
                        return "名称不能为空";
                    }
                    if (Name.Length > 32)
                    {
                        return "名称长度不能超过32个字符";
                    }
                    if (string.IsNullOrEmpty(DeptID))
                    {
                        return "请选择校区";
                    }
                    if (string.IsNullOrEmpty(Year))
                    {
                        return "请选择年份";
                    }
                    if (string.IsNullOrEmpty(Month))
                    {
                        return "请选择月份";
                    }
                    if (string.IsNullOrEmpty(sProfessionID))
                    {
                        return "请选择专业";
                    }
                    if (string.IsNullOrEmpty(Scheme))
                    {
                        return "请选择缴费方案";
                    }
                    if (string.IsNullOrEmpty(Semester))
                    {
                        return "请选择缴费次数";
                    }
                    if (string.IsNullOrEmpty(sOrderContent))
                    {
                        return "请设置缴费项";
                    }
                    if (!string.IsNullOrEmpty(Remark))
                    {
                        Remark = Remark.Replace("\n", "<br />");
                    }
                    #endregion

                    #region 数据写入
                    //写入T_Stu_sOrderAdd
                    sOrderAddModel sorderadd_model = new sOrderAddModel();
                    sorderadd_model.Status = "1";
                    sorderadd_model.DeptID = DeptID;
                    sorderadd_model.Year = Year;
                    sorderadd_model.Month = Month;
                    sorderadd_model.Name = Name;
                    sorderadd_model.PlanItemID = Scheme;
                    sorderadd_model.NumItemID = Semester;
                    sorderadd_model.Remark = Remark;
                    sorderadd_model.CreateID = this.UserId.ToString();
                    sorderadd_model.CreateTime = DateTime.Now.ToString();
                    sorderadd_model.UpdateID = this.UserId.ToString();
                    sorderadd_model.UpdateTime = DateTime.Now.ToString();
                    int sOrderAddID = sOrderAddBLL.InsertsOrderAdd(sorderadd_model);

                    //写入T_Stu_sOrderAddDetail
                    if (!string.IsNullOrEmpty(sOrderContent))
                    {
                        //{"total":1,"rows":[{"DetailID":"2","DetailName":"报名费","Sort":"2","IsGive":"1","ShouldMoney":"200","LimitTime":"2015-02-15"}]}
                        JObject job = JObject.Parse(sOrderContent);
                        string[] array = job.Properties().Where(x => x.Name == "rows").Select(x => x.Value.ToString()).ToArray();
                        List<sOrderModel.sOrderDetailModel> sOrderList = JsonConvert.DeserializeObject<List<sOrderModel.sOrderDetailModel>>(array[0]);//将json数据转化为对象类型并赋值给list
                        foreach (var item in sOrderList)
                        {
                            sOrderAddDetailModel sorderadddetail_model = new sOrderAddDetailModel();
                            sorderadddetail_model.Status = "1";
                            sorderadddetail_model.sOrderAddID = sOrderAddID.ToString();
                            sorderadddetail_model.DetailID = item.DetailID;
                            sorderadddetail_model.Sort = item.Sort;
                            sorderadddetail_model.ShouldMoney = item.ShouldMoney;
                            sorderadddetail_model.IsGive = item.IsGive;
                            sorderadddetail_model.LimitTime = item.LimitTime;
                            sorderadddetail_model.CreateID = this.UserId.ToString();
                            sorderadddetail_model.CreateTime = DateTime.Now.ToString();
                            sorderadddetail_model.UpdateID = this.UserId.ToString();
                            sorderadddetail_model.UpdateTime = DateTime.Now.ToString();
                            sOrderAddDetailBLL.InsertsOrderAddDetail(sorderadddetail_model);
                        }
                    }

                    //写入T_Stu_sOrderAddsProfession
                    string[] sProIDArray = sProfessionID.Split(',');
                    foreach (var item in sProIDArray)
                    {
                        sOrderAddsProfessionModel sorderaddpro_model = new sOrderAddsProfessionModel();
                        sorderaddpro_model.Status = "1";
                        sorderaddpro_model.sOrderAddID = sOrderAddID.ToString();
                        sorderaddpro_model.sProfessionID = item;
                        sorderaddpro_model.CreateID = this.UserId.ToString();
                        sorderaddpro_model.CreateTime = DateTime.Now.ToString();
                        sorderaddpro_model.UpdateID = this.UserId.ToString();
                        sorderaddpro_model.UpdateTime = DateTime.Now.ToString();
                        sOrderAddsProfessionBLL.InsertsOrderAddsProfession(sorderaddpro_model);
                    }

                    str = "yes";
                    #endregion

                    ts.Complete();
                }
                catch
                {
                    str = "出现未知错误，请联系管理员";
                    Transaction.Current.Rollback();
                }
                finally
                {
                    ts.Dispose();
                }
                return str;
            }
        }

        /// <summary>
        /// 根据专业ID取缴费方案（排除停用的），可能有多个sProfessionID
        /// </summary>
        /// <returns></returns>
        public string SelScheme()
        {
            string majorIDs = Request.Form["MajorID"];
            DataTable dt = sOrderAddBLL.GetScheme(majorIDs);
            return JsonHelper.DataTableToJson(dt);
        }

        /// <summary>
        /// 根据缴费方案ID得到缴费期数（排除停用的）
        /// </summary>
        /// <returns></returns>
        public string SelSemester()
        {
            string schemeID = Request.Form["SchemeID"];
            DataTable dt = sOrderAddBLL.GetSemester(schemeID);
            return JsonHelper.DataTableToJson(dt);
        }

        /// <summary>
        /// 批量添加缴费单，返回结果
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetsOrderAddResult()
        {
            #region 获取数据
            StringBuilder result = new StringBuilder();
            string sOrderContent = Request.Form["sOrderContent"];
            string DeptID = Request.Form["DeptID"];
            string Year = Request.Form["Year"];
            string Month = Request.Form["Month"];
            string sProfessionID = Request.Form["sProfessionID"];
            string Scheme = Request.Form["Scheme"];
            string Semester = Request.Form["Semester"];
            #endregion

            #region 写入数据
            DataTable dt = sOrderAddBLL.GetInfo(DeptID, sProfessionID, Scheme, Semester, Year, Month);
            if (dt.Rows.Count > 0)
            {
                result.Append("[");
                //sOrderContent  {"total":1,"rows":[{"DetailID":"2","DetailName":"报名费","Sort":"2","IsGive":"1","ShouldMoney":"200","LimitTime":"2015-02-15"}]}
                JObject job = JObject.Parse(sOrderContent);
                string[] array = job.Properties().Where(x => x.Name == "rows").Select(x => x.Value.ToString()).ToArray();
                List<sOrderModel.sOrderDetailModel> sOrderList = JsonConvert.DeserializeObject<List<sOrderModel.sOrderDetailModel>>(array[0]);//将json数据转化为对象类型并赋值给list
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region MyRegion
                    sOrderModel sorderModel = sOrderBLL.sOrderModelByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID AND PlanItemID=@PlanItemID AND NumItemID=@NumItemID AND Status!=9", new SqlParameter[] { 
                                new SqlParameter("@sEnrollsProfessionID", dt.Rows[i]["sEnrollsProfessionID"].ToString()),
                                new SqlParameter("@PlanItemID", dt.Rows[i]["PlanItemID"].ToString()),
                                new SqlParameter("@NumItemID", dt.Rows[i]["NumItemID"].ToString())
                    });
                    foreach (var item in sOrderList)
                    {
                        result.Append("{");
                        using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                        {
                            #region MyRegion
                            StringBuilder tempStr = new StringBuilder();
                            try
                            {
                                //判断收费项是否重复（在报名专业、缴费方案、缴费期数下）
                                DataTable repeatDt = sOrderBLL.sOrderTableByWhere(" AND sEnrollsProfessionID=@sEnrollsProfessionID AND PlanItemID=@PlanItemID AND NumItemID=@NumItemID AND DetailID=@DetailID AND Status!=9", new SqlParameter[] { 
                                new SqlParameter("@sEnrollsProfessionID", dt.Rows[i]["sEnrollsProfessionID"].ToString()),
                                new SqlParameter("@PlanItemID", dt.Rows[i]["PlanItemID"].ToString()),
                                new SqlParameter("@NumItemID", dt.Rows[i]["NumItemID"].ToString()),
                                new SqlParameter("@DetailID", item.DetailID) 
                                }, "");
                                if (repeatDt.Rows.Count > 0)
                                {
                                    //存在重复的
                                    tempStr.Append("\"Result\":\"失败\",");
                                    tempStr.Append("\"Remark\":\"缴费项已存在\",");
                                }
                                else
                                {
                                    #region 写入sOrder
                                    sOrderModel sOrder = new sOrderModel();
                                    sOrder.Status = "1";
                                    sOrder.DeptID = sorderModel.DeptID;
                                    sOrder.sEnrollsProfessionID = dt.Rows[i]["sEnrollsProfessionID"].ToString();
                                    sOrder.PlanItemID = dt.Rows[i]["PlanItemID"].ToString();
                                    sOrder.PlanLevel = dt.Rows[i]["PlanLevel"].ToString();
                                    sOrder.PlanName = sorderModel.PlanName;
                                    sOrder.PlanSort = sorderModel.PlanSort;
                                    sOrder.Year = sorderModel.Year;
                                    sOrder.Month = sorderModel.Month;
                                    sOrder.NumItemID = dt.Rows[i]["NumItemID"].ToString();
                                    sOrder.NumName = sorderModel.NumName;
                                    sOrder.LimitTime = item.LimitTime;
                                    sOrder.ItemDetailID = "0";
                                    sOrder.DetailID = item.DetailID;
                                    sOrder.Sort = item.Sort;
                                    sOrder.ShouldMoney = item.ShouldMoney;
                                    sOrder.PaidMoney = "0";
                                    sOrder.IsGive = item.IsGive;
                                    sOrder.ItemQueue = sorderModel.ItemQueue;
                                    sOrder.ItemDetailQueue = sOrderBLL.GetItemDetailQueue(dt.Rows[i]["sEnrollsProfessionID"].ToString(), dt.Rows[i]["PlanItemID"].ToString(), dt.Rows[i]["NumItemID"].ToString());
                                    sOrder.CreateID = this.UserId.ToString();
                                    sOrder.CreateTime = DateTime.Now.ToString();
                                    sOrder.UpdateID = this.UserId.ToString();
                                    sOrder.UpdateTime = DateTime.Now.ToString();
                                    sOrderBLL.InsertsOrder(sOrder);
                                    #endregion

                                    tempStr.Append("\"Result\":\"成功\",");
                                    tempStr.Append("\"Remark\":\"\",");
                                }
                                ts.Complete();
                            }
                            catch
                            {
                                tempStr.Clear();
                                tempStr.Append("\"Result\":\"失败\",");
                                tempStr.Append("\"Remark\":\"系统错误\",");
                                Transaction.Current.Rollback();
                            }
                            finally
                            {
                                ts.Dispose();
                            }
                            result.Append(tempStr.ToString());
                            #endregion
                        }
                        result.Append("\"StuName\":\"" + dt.Rows[i]["StuName"].ToString() + "\",");
                        result.Append("\"IDCard\":\"" + dt.Rows[i]["IDCard"].ToString() + "\",");
                        result.Append("\"Scheme\":\"" + dt.Rows[i]["PlanName"].ToString() + "\",");
                        result.Append("\"Semester\":\"" + dt.Rows[i]["NumName"].ToString() + "\",");
                        result.Append("\"DetailName\":\"" + item.DetailName + "\"");
                        result.Append("}");
                        result.Append(",");
                    }
                    #endregion
                }
                string str = result.ToString().TrimEnd(',');
                result.Clear();
                result.Append(str);
                result.Append("]");
            }
            else
            {
                result.Append("[{");
                result.Append("\"Result\":\"失败\",");
                result.Append("\"Remark\":\"没有可用的缴费单信息\",");
                result.Append("\"StuName\":\"\",");
                result.Append("\"IDCard\":\"\",");
                result.Append("\"Scheme\":\"\",");
                result.Append("\"Semester\":\"\",");
                result.Append("\"DetailName\":\"\"");
                result.Append("}]");
            }
            #endregion

            return AjaxResult.Success(result.ToString());
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <returns></returns>
        public string GetReback()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    string sOrderAddID = Request.Form["ID"];
                    string str = sOrderAddBLL.RebacksOrderAdd(sOrderAddID);
                    ts.Complete();
                    return str;
                }
                catch
                {
                    Transaction.Current.Rollback();
                    return "出现未知错误，请联系管理员";
                }
                finally
                {
                    ts.Dispose();
                }
            }
        }

        /// <summary>
        /// 得到查看缴费项列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetsOrderView()
        {
            string sOrderAddID = Request.Form["ID"];
            string cmdText = @"SELECT   sorderdetail.sOrderAddDetailID ,detail.Name AS DetailName ,
            refe.RefeName AS Sort ,
            refe_give.RefeName AS IsGive ,
            sorderdetail.ShouldMoney ,
            CONVERT(NVARCHAR(23), sorderdetail.LimitTime, 23) AS LimitTime
  FROM      T_Stu_sOrderAddDetail AS sorderdetail
            LEFT JOIN T_Pro_Detail AS detail ON detail.DetailID = sorderdetail.DetailID
            LEFT JOIN T_Sys_Refe AS refe ON refe.Value = sorderdetail.Sort
                                                AND refe.RefeTypeID = 5
            LEFT JOIN T_Sys_Refe AS refe_give ON refe_give.Value = sorderdetail.IsGive
                                                     AND refe_give.RefeTypeID = 15
  WHERE     sorderdetail.sOrderAddID = {0}";
            cmdText = string.Format(cmdText, sOrderAddID);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form, ""));
        }
        #endregion
    }
}
