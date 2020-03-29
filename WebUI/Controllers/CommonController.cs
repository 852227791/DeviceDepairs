using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Model;
using System.Data;
using BLL;

namespace WebUI.Controllers
{
    public class CommonController : BaseController
    {
        //
        // GET: /Common/

        #region 获取当前日期
        public AjaxResult GetDateNow()
        {
            return AjaxResult.Success(DateTime.Now.ToString("yyyy-MM-dd"));
        }
        #endregion
        /// <summary>
        /// 获取年份下拉菜单
        /// </summary>
        /// <returns></returns>
        public string YearCombobox()
        {
            int year = 2010;
            string yearStr = Request.QueryString["Year"];
            string isSelected = Request.QueryString["IsSelected"];
            string isDefault = Request.QueryString["Is"];

            if (!string.IsNullOrEmpty(yearStr))
            {
                year = Convert.ToInt32(yearStr);
            }
            string temp = "";
            if (isDefault == "Yes")
            {
                temp = "{ \"Id\":\"0\",\"Text\": \"不限制\"},";
            }
            string str = "[" + temp;
            int i = 0;
            int nowYear= DateTime.Now.Year;
            if (DateTime.Now.Month>=11)
            {
                nowYear = nowYear + 1;
            }
            for (int startyear = nowYear; startyear >= year; startyear--)
            {
                string selectedStr = "";
                if (isSelected == "Yes" && i == 0)
                {
                    selectedStr = ",\"selected\":true";
                }
                str += "{ \"Id\":\"" + startyear + "\",\"Text\": \"" + startyear + "年\"" + selectedStr + "},";
                i += 1;
            }
            if (str != "[")
            {
                str = str.Substring(0, str.Length - 1);
            }
            str += "]";
            return str;
        }

        public AjaxResult CheckIdCard()
        {
            string idCard = Request.Form["ID"];
            if (!OtherHelper.CheckIDCard(idCard))
            {
                return AjaxResult.Error("身份证号不规范");
            }
            else
            {
                return AjaxResult.Success();
            }
        }
        /// <summary>
        /// 离校年度
        /// </summary>
        /// <param name="startYear"></param>
        /// <returns></returns>
        public string LeaveYearCombobox(string startYear, bool isSelected)
        {
            if (!OtherHelper.IsInt(startYear))
            {
                return "";
            }
            int year = Convert.ToInt32(startYear);
            List<ComboboxModel> list = new List<ComboboxModel>();
            for (int i = year; i < DateTime.Now.Year + 6; i++)
            {
                ComboboxModel cm = new ComboboxModel();
                cm.text = i.ToString() + "年";
                cm.id = i.ToString();
                if (i == DateTime.Now.Year)
                {
                    if (isSelected)
                        cm.selected = true;
                    else
                        cm.selected = false;
                }
                else
                    cm.selected = false;
                list.Add(cm);
            }
            return OtherHelper.JsonSerializer(list);
        }

        public AjaxResult GetMonthNow()
        {
            int month = DateTime.Now.Month;
            if (month >= 4 && month <= 9)
            {
                return AjaxResult.Success("9", "");
            }
            return AjaxResult.Success("3", "");
        }
        ///// <summary>
        ///// 绵竹天一学院
        ///// </summary>
        ///// <returns></returns>
        //public AjaxResult TianYiUpdateTime()
        //{
        //    string where = " and DeptID=27  AND Year=2016 and Status<>9";
        //    var senrollsProfession = sEnrollsProfessionBLL.SelectsEnrollsProfessionByWhere(where, null, "");
        //    string senrollId = string.Empty;
        //    int successNum = 0;
        //    foreach (var item in senrollsProfession)
        //    {
        //        string where1 = " and sEnrollsProfessionID=" + item.sEnrollsProfessionID + " and Status<>9 ";
        //        var list = sFeeBLL.SelectsFeeByWhere(where1, null, " Order by FeeTime ASC");
        //        if (list.Count > 0)
        //        {
        //            var sfee = list.FirstOrDefault();
        //            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(item.sEnrollsProfessionID);
        //            epm.FirstFeeTime = sfee.FeeTime;
        //            epm.BeforeEnrollTime = "1900-01-01";
        //            sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
        //            successNum++;
        //        }
        //    }
        //    return AjaxResult.Success("", "四川天一学院：2016单招报名" + senrollsProfession.Count + "人,缴费" + successNum + "人，其余人未处理第一次缴费时间");

        //}
        ///// <summary>
        ///// 金堂五月花
        ///// </summary>
        ///// <returns></returns>
        //public AjaxResult WuYueHuaUpdateTime()
        //{
        //    string where = " and DeptID=5  and Status<>9";
        //    var senrollsProfession = sEnrollsProfessionBLL.SelectsEnrollsProfessionByWhere(where, null, "");
        //    string senrollId = string.Empty;
        //    int successNum = 0;
        //    foreach (var item in senrollsProfession)
        //    {
        //        sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(item.sEnrollsProfessionID);
        //        string where1 = " and sEnrollsProfessionID=" + item.sEnrollsProfessionID + " and Status<>9 ";
        //        var list = sFeeBLL.SelectsFeeByWhere(where1, null, " Order by FeeTime ASC");
        //        if (list.Count > 0)
        //        {
        //            var sfee = list.FirstOrDefault();
        //            epm.FirstFeeTime = sfee.FeeTime;
        //        }
        //        else
        //        {
        //            epm.FirstFeeTime = "1900-01-01";
        //        }

        //        if (epm.Status == "2")
        //        {
        //            epm.BeforeEnrollTime = epm.EnrollTime;
        //            epm.EnrollTime = "1900-01-01";
        //            successNum++;
        //        }
        //        else if (epm.Status == "3" || epm.Status == "4")
        //        {
        //            epm.BeforeEnrollTime = "1900-01-01";

        //            string where2 = " and sEnrollsProfessionID=" + item.sEnrollsProfessionID + "";
        //            var list1 = sEnrollsProfessionChangeBLL.SelectsEnrollsProfessionChangeByWhere(where2, null, "");
        //            if (list1.Count > 0)
        //            {
        //                var change = list1.FirstOrDefault();
        //                epm.BeforeEnrollTime = epm.EnrollTime;
        //                epm.EnrollTime = change.CreateTime;
        //            }
        //            successNum++;
        //        }

        //        sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);

        //    }
        //    return AjaxResult.Success("", "五月花专修学院：2016报名" + senrollsProfession.Count + "人,缴费" + successNum + "人，其余人未处理第一次缴费时间");
        //}
    }
}
