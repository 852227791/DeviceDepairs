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
    public class ProveController : BaseController
    {
        //
        // GET: /Prove/
        public ActionResult ProveNumUpload()
        {
            return View();
        }
        public ActionResult ProveNumEdit()
        {
            return View();
        }
        public ActionResult ProveChoose()
        {
            ViewBag.Title = "选择证书";
            return View();
        }
        public ActionResult OldProveList()
        {
            ViewBag.Title = "老系统证书";
            return View();
        }
        public ActionResult OldProveFileList()
        {
            ViewBag.Title = "下载老证书信息";
            return View();
        }
        public ActionResult ProveList()
        {
            ViewBag.Title = "证书管理";
            return View();
        }
        public ActionResult ProveUp()
        {
            ViewBag.Title = "证书状态变更";
            return View();
        }
        public ActionResult ProveStatusEdit()
        {
            ViewBag.Title = "修改证书状态";
            return View();
        }

        public ActionResult ProveEdit()
        {
            ViewBag.Title = "编辑证书";
            return View();
        }

        public ActionResult ProveView()
        {
            ViewBag.Title = "查看证书";
            return View();
        }

        public ActionResult ProveOldUp()
        {
            ViewBag.Title = "导入老证书";
            return View();
        }

        #region 证书列表
        /// <summary>
        /// 证书列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProveList()
        {
            string menuId = Request.Form["MenuID"];
            string proveName = Request.Form["txtProveName"];
            string name = Request.Form["txtName"];
            string idCard = Request.Form["txtIDCard"];
            string mobile = Request.Form["txtMobile"];
            string status = Request.Form["selStatus"];
            string dept = Request.Form["treeDept"];
            string enrollTimeS = Request.Form["txtEnrollTimeS"];
            string enrollTimeE = Request.Form["txtEnrollTimeE"];

            string where = "";
            if (!string.IsNullOrEmpty(proveName))
            {
                where += " AND i.Name like '%" + proveName + "%'";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " AND s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                where += " AND s.Mobile like '%" + mobile + "%'";
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "p1.Status");
            }
            if (!string.IsNullOrEmpty(dept))
            {
                where += "AND p1.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(enrollTimeS))
            {
                where += " AND convert(NVARCHAR(10),p1.EnrollTime,23) >= '" + enrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(enrollTimeE))
            {
                where += " AND convert(NVARCHAR(10),p1.EnrollTime,23) <= '" + enrollTimeE + "'";
            }
            string cmdText = @"SELECT  p1.ProveID ,
        r.RefeName Status ,
        d.Name DeptName ,
        pr.Name ProfessionName ,
        c.Name ClassName ,
        s.Name StudentName ,
        s.IDCard ,
        s.Mobile ,
        i.Name ItemName ,
        p1.ProveNum,
        p1.EnrollTime ,
        p1.Status StatusValue ,
        p1.ItemID ,
        ( SELECT  ISNULL(SUM(fd.ShouldMoney),0)  
          FROM      T_Pro_FeeDetail fd
                    LEFT JOIN T_Pro_Fee f ON f.FeeID = fd.FeeID
          WHERE     f.ProveID = p1.ProveID
        ) ShouldMoney ,
        ( SELECT     ISNULL(SUM(rd.RefundMoney),0)
          FROM      T_Pro_FeeDetail fd
                    LEFT JOIN T_Pro_Refund rd ON rd.FeeDetailID = fd.FeeDetailID
                    LEFT JOIN T_Pro_Fee f ON f.FeeID = fd.FeeID
          WHERE     f.ProveID = p1.ProveID
        ) RefundMoney
FROM    T_Pro_Prove p1
        LEFT JOIN T_Sys_Refe r ON p1.Status = r.Value
                                  AND r.RefeTypeID = 9
        LEFT JOIN T_Sys_Dept d ON p1.DeptID = d.DeptID
        LEFT JOIN T_Pro_Student s ON p1.StudentID = s.StudentID
        LEFT JOIN T_Pro_Item i ON p1.ItemID = i.ItemID
        LEFT JOIN T_Pro_Class c ON c.ClassID = p1.ClassID
        LEFT JOIN T_Pro_Profession pr ON pr.ProfessionID = c.ProfessionID
WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "p1.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        #endregion

        #region  导出证书
        public AjaxResult DownloadProve()
        {
            string menuId = Request.Form["MenuID"];
            string proveName = Request.Form["txtProveName"];
            string name = Request.Form["txtName"];
            string idCard = Request.Form["txtIDCard"];
            string status = Request.Form["selStatus"];
            string dept = Request.Form["treeDept"];
            string enrollTimeS = Request.Form["txtEnrollTimeS"];
            string enrollTimeE = Request.Form["txtEnrollTimeE"];
            string where = "";
            if (!string.IsNullOrEmpty(proveName))
            {
                where += " AND i.Name like '%" + proveName + "%'";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " AND s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "p1.Status");
            }
            if (!string.IsNullOrEmpty(dept))
            {
                where += "AND p1.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(enrollTimeS))
            {
                where += " AND convert(NVARCHAR(10),p1.EnrollTime,23) >= '" + enrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(enrollTimeE))
            {
                where += " AND convert(NVARCHAR(10),p1.EnrollTime,23) <= '" + enrollTimeE + "'";
            }
            string cmdText = @"SELECT  d.Name 校区 ,
        pr.Name 专业 ,
        c.Name 班级 ,
        s.Name 学生姓名 ,
        s.IDCard 身份证号 ,
        s.Mobile 手机号 ,
        r2.RefeName 性别 ,
        i.Name 证书名称 ,
        p1.ProveNum 证书编号 ,
        p1.EnrollTime 报名时间 ,
        r.RefeName 状态,
        ( SELECT  ISNULL(SUM(fd.ShouldMoney),0)  
          FROM      T_Pro_FeeDetail fd
                    LEFT JOIN T_Pro_Fee f ON f.FeeID = fd.FeeID
          WHERE     f.ProveID = p1.ProveID
        ) 实收金额 ,
        ( SELECT     ISNULL(SUM(rd.RefundMoney),0)
          FROM      T_Pro_FeeDetail fd
                    LEFT JOIN T_Pro_Refund rd ON rd.FeeDetailID = fd.FeeDetailID
                    LEFT JOIN T_Pro_Fee f ON f.FeeID = fd.FeeID
          WHERE     f.ProveID = p1.ProveID
        ) 核销金额
FROM    T_Pro_Prove p1
        LEFT JOIN T_Sys_Refe r ON p1.Status = r.Value
                                  AND r.RefeTypeID = 9
        LEFT JOIN T_Sys_Dept d ON p1.DeptID = d.DeptID
        LEFT JOIN T_Pro_Student s ON p1.StudentID = s.StudentID
        LEFT JOIN T_Pro_Item i ON p1.ItemID = i.ItemID
        LEFT JOIN T_Pro_Class c ON c.ClassID = p1.ClassID
        LEFT JOIN T_Pro_Profession pr ON pr.ProfessionID = c.ProfessionID
        LEFT JOIN T_Sys_Refe r2 ON s.Sex = r2.Value
                                   AND r2.RefeTypeID = 3
WHERE   1 = 1 {0}
ORDER BY p1.ProveID DESC";
            string filename = "证书信息.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "p1.DeptID", "p1.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }
        #endregion

        #region 查询证书
        /// <summary>
        /// 查询证书
        /// </summary>
        /// <returns></returns>
        public AjaxResult SelectProve()
        {
            string proveID = Request.Form["ID"];
            string where = " AND ProveID = @ProveID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ProveID", proveID)
            };
            DataTable dt = ProveBLL.ProveStudentTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }
        #endregion

        #region 查看证书
        /// <summary>
        /// 查看证书
        /// </summary>
        /// <returns></returns>
        public AjaxResult SelectViewProve()
        {
            string proveId = Request.Form["ID"];
            string cmdText = @"
SELECT  p1.ProveID ,
        pr.Name ProfessionName ,
        c.Name ClassName ,
        i.Name ,
        CONVERT(NVARCHAR(23), p1.EnrollTime, 23) EnrollTime ,
        r.RefeName Status ,
        CASE p1.IsForce
          WHEN 1 THEN '否'
          WHEN 2 THEN '是'
        END Force ,
        p1.Remark ,
        p1.StudentID,
		d.Name DeptName,
        p1.ProveNum
FROM    T_Pro_Prove p1
        LEFT JOIN T_Pro_Class c ON p1.ClassID = c.ClassID
        LEFT JOIN T_Pro_Profession pr ON c.ProfessionID = pr.ProfessionID
        LEFT JOIN T_Pro_Item i ON p1.ItemID = i.ItemID
        LEFT JOIN T_Sys_Refe r ON p1.Status = r.Value
                                  AND r.RefeTypeID = 9	
		LEFT JOIN T_Sys_Dept d ON d.DeptID=p1.DeptID
WHERE   p1.ProveID = {0}";
            cmdText = string.Format(cmdText, proveId);
            return AjaxResult.Success(JsonData.GetArray(cmdText));
        }
        #endregion

        #region 返回证书条数
        public AjaxResult SelectItemNum()
        {
            string proveId = Request.Form["ProveID"];
            string studentId = Request.Form["StudentID"];
            string itemId = Request.Form["ItemID"];
            if (string.IsNullOrEmpty(studentId))
            {
                studentId = "0";
            }
            if (string.IsNullOrEmpty(itemId))
            {
                itemId = "0";
            }
            return AjaxResult.Success(ProveBLL.SelectProveNum(proveId, studentId, itemId));
        }
        #endregion

        #region 更改状态
        /// <summary>
        /// 更改状态
        /// </summary>
        /// <returns></returns>
        public string GetUpdateStatus(string sProveID, string Status)
        {
            string result = "no";
            string where = " AND ProveID = @ProveID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ProveID", sProveID)
            };
            ProveModel pm = ProveBLL.ProveModelByWhere(where, paras);
            pm.Status = Status;
            pm.UpdateID = this.UserId.ToString();
            pm.UpdateTime = DateTime.Now.ToString();
            if (ProveBLL.UpdateProve(pm) > 0)
            {
                result = "yes";
            }
            return result;
        }
        #endregion

        #region 编辑证书
        /// <summary>
        /// 编辑证书
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        public string GetProveEdit(ProveModel pm)
        {
            string filePath = Request.Form["filePathpic"];

            if (string.IsNullOrEmpty(pm.ProveID))
            {
                pm.ProveID = "0";
            }
            if (string.IsNullOrEmpty(pm.DeptID))
            {
                return "请选择校区";
            }
            if (string.IsNullOrEmpty(pm.EnrollTime))
            {
                return "报名时间不能为空";
            }
            if (string.IsNullOrEmpty(pm.StudentID))
            {
                return "请选择学生";
            }
            if (string.IsNullOrEmpty(pm.ItemID))
            {
                return "请选择证书";
            }
            if (!string.IsNullOrEmpty(pm.Remark))
            {
                pm.Remark = pm.Remark.Replace("\r\n", "<br />");
            }
            else
            {
                pm.Remark = "";
            }

            if (!ValidateIdCard(pm.StudentID))
            {
                return "该学生缺少身份证号，不能报考证书。";
            }
            if (string.IsNullOrEmpty(pm.ClassID))
            {
                pm.ClassID = "0";
            }
            string result = "no";
            StudentInfoModel sm = StudentInfoBLL.GetStudentInfoModel(pm.StudentID);
            if (!string.IsNullOrEmpty(sm.StudentInfoID))
            {
                sm.Photo = filePath;
                sm.UpdateID = UserId.ToString();
                sm.UpdateTime = DateTime.Now.ToString();
                StudentInfoBLL.UpdateStudentInfo(sm);

            }
            else
            {
                sm.StudentID = pm.StudentID;
                sm.CreateID = UserId.ToString();
                sm.CreateTime = DateTime.Now.ToString();
                sm.UpdateID = UserId.ToString();
                sm.UpdateTime = DateTime.Now.ToString();
                sm.Photo = filePath;
                StudentInfoBLL.InsertStudentInfo(sm);
            }
            if (pm.ProveID != "0")
            {
                string where = " AND ProveID = @ProveID";
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@ProveID", pm.ProveID)
                };
                ProveModel editpm = ProveBLL.ProveModelByWhere(where, paras);

                LogBLL.CreateLog("T_Pro_Prove", this.UserId.ToString(), editpm, pm);//写日志

                editpm.DeptID = pm.DeptID;
                editpm.ClassID = pm.ClassID;
                editpm.EnrollTime = pm.EnrollTime;
                editpm.StudentID = pm.StudentID;
                editpm.ItemID = pm.ItemID;
                editpm.IsForce = pm.IsForce;
                editpm.Remark = pm.Remark;
                editpm.UpdateID = this.UserId.ToString();
                editpm.UpdateTime = DateTime.Now.ToString();
                if (ProveBLL.UpdateProve(editpm) > 0)
                {
                    return "yes";
                }
            }
            else
            {
                pm.Status = "1";
                pm.CreateID = this.UserId.ToString();
                pm.CreateTime = DateTime.Now.ToString();
                pm.UpdateID = this.UserId.ToString();
                pm.UpdateTime = DateTime.Now.ToString();
                if (ProveBLL.InsertProve(pm) > 0)
                {
                    return "yes";
                }
            }



            return result;
        }
        #endregion

        #region  导入证书状态
        public AjaxResult UploadProve()
        {
            string filePath = Request.Form["filePath"];
            string deptId = Request.Form["Dept"];
            if (string.IsNullOrEmpty(filePath))
            {
                return AjaxResult.Error("请选择上传的文件");
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
                string[] array = new string[4];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string errorString = "";
                    try
                    {
                        array[0] = ds.Tables[0].Rows[i][0].ToString().Trim();//姓名
                        array[1] = ds.Tables[0].Rows[i][1].ToString().Trim();//身份证号
                        array[2] = ds.Tables[0].Rows[i][2].ToString().Trim();//证书名称
                        array[3] = ds.Tables[0].Rows[i][3].ToString().Trim();//状态
                    }
                    catch (Exception)
                    {
                        return AjaxResult.Error("模板错误");
                    }
                    if (i == 0)
                    {
                        if (array[0] != "姓名" || array[1] != "身份证号" || array[2] != "证书名称" || array[3] != "状态")
                        {
                            return AjaxResult.Error("模板错误");
                        }
                        else
                        {
                            dt = TableTitle(dt, array);
                        }
                    }
                    else
                    {
                        StudentModel sm = StudentBLL.GetStudentModel(array[0], array[1]);
                        if (string.IsNullOrEmpty(sm.StudentID))
                        {
                            errorString += "此学生不存在;";
                        }
                        ItemModel im = ItemBLL.GetItemModel(deptId, array[2]);
                        if (string.IsNullOrEmpty(im.Name))
                        {
                            errorString += "系统内不存在此证书名称;";
                        }
                        DataTable pdt = GetProveTable(sm.StudentID, im.ItemID);
                        if (pdt.Rows.Count == 0)
                        {
                            errorString += "此学生没有报此证书;";
                        }
                        if (pdt.Rows.Count > 1)
                        {
                            errorString += "此学生多次报考此证书，请手动修改状态";
                        }
                        string status = RefeBLL.GetRefeValue(array[3], "9");
                        if (status.Equals("-1"))
                        {
                            errorString += "此状态不存在;";
                        }
                        if (string.IsNullOrEmpty(errorString))
                        {
                            ProveModel pm = ProveBLL.GetProveModel(sm.StudentID, im.ItemID);
                            pm.Status = status;
                            pm.UpdateTime = DateTime.Now.ToString();
                            pm.UpdateID = this.UserId.ToString();
                            if (ProveBLL.UpdateProve(pm) > 0)
                            {
                                successNum += 1;
                            }
                        }
                        else
                        {
                            errorNum += 1;
                            dt = TableRows(dt, array, errorString);
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
                nm.Sort = "2";
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
                return AjaxResult.Error("操作失败，数据不能为空");
            }

        }
        #endregion

        #region   生成Excel表头
        /// <summary>
        /// 生成ExcelTableTitle
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="opm"></param>
        /// <returns></returns>
        public DataTable TableTitle(DataTable dt, OldProveModel opm)
        {

            dt.Columns.Add(opm.name, Type.GetType("System.String"));
            dt.Columns.Add(opm.idCard, Type.GetType("System.String"));
            dt.Columns.Add(opm.sex, Type.GetType("System.String"));
            dt.Columns.Add(opm.mobile, Type.GetType("System.String"));
            dt.Columns.Add(opm.proveName, Type.GetType("System.String"));
            dt.Columns.Add(opm.enrollTime, Type.GetType("System.String"));
            dt.Columns.Add(opm.teacher, Type.GetType("System.String"));
            dt.Columns.Add(opm.remark, Type.GetType("System.String"));
            dt.Columns.Add(opm.address, Type.GetType("System.String"));
            dt.Columns.Add("系统备注", Type.GetType("System.String"));
            return dt;
        }
        /// <summary>
        /// 生成ExcelTableTitle
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
            dr["姓名"] = array[0];
            dr["身份证号"] = "'" + array[1];
            dr["证书名称"] = "'" + array[2];
            dr["状态"] = array[3];
            dr["系统备注"] = remark;
            dt.Rows.Add(dr);
            return dt;
        }
        /// <summary>
        /// 老证书excel行
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="opm"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public DataTable TableRowsOldProve(DataTable dt, OldProveModel opm, string remark)
        {
            DataRow dr = dt.NewRow();
            dr["姓名"] = opm.name;
            dr["身份证号"] = opm.idCard;
            dr["性别"] = opm.sex;
            dr["手机"] = opm.mobile;
            dr["地址"] = opm.address;
            dr["证书名称"] = opm.proveName;
            dr["报名时间"] = opm.enrollTime;
            dr["教师"] = opm.teacher;
            dr["备注"] = opm.remark;
            dr["系统备注"] = remark;
            dt.Rows.Add(dr);
            return dt;
        }

        #endregion

        #region 获取证书Table
        public DataTable GetProveTable(string studentId, string itemId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                studentId = "0";
            }
            if (string.IsNullOrEmpty(itemId))
            {
                itemId = "0";
            }
            string where = " and StudentID=@StudentID and ItemID=@ItemID";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@StudentID",studentId),
            new SqlParameter("@ItemID",itemId)
            };
            DataTable dt = ProveBLL.ProveTableByWhere(where, paras, "");
            return dt;
        }
        #endregion


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

        public ActionResult GetProveOldList()
        {
            string menuId = Request.Form["MenuID"];
            string proveName = Request.Form["txtProveName"];
            string name = Request.Form["txtName"];
            string idCard = Request.Form["txtIDCard"];
            string mobile = Request.Form["txtMobile"];
            string status = Request.Form["selStatus"];
            string dept = Request.Form["treeDept"];
            string enrollTimeS = Request.Form["txtEnrollTimeS"];
            string enrollTimeE = Request.Form["txtEnrollTimeE"];

            string where = "";
            if (!string.IsNullOrEmpty(proveName))
            {
                where += " AND i.Name like '%" + proveName + "%'";
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " AND s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                where += " AND s.Mobile like '%" + mobile + "%'";
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += OtherHelper.MultiSelectToSQLWhere(status, "p1.Status");
            }
            if (!string.IsNullOrEmpty(dept))
            {
                where += "AND p1.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            if (!string.IsNullOrEmpty(enrollTimeS))
            {
                where += " AND convert(NVARCHAR(10),p1.EnrollTime,23) >= '" + enrollTimeS + "'";
            }
            if (!string.IsNullOrEmpty(enrollTimeE))
            {
                where += " AND convert(NVARCHAR(10),p1.EnrollTime,23) <= '" + enrollTimeE + "'";
            }
            string cmdText = @"SELECT  p1.ProveID ,
        r.RefeName Status ,
        d.Name DeptName ,
        pr.Name ProfessionName ,
        c.Name ClassName ,
        s.Name StudentName ,
        s.IDCard ,
        s.Mobile ,
        i.Name ItemName ,
        p1.EnrollTime ,
        p1.Status StatusValue ,
        p1.ItemID
FROM   T_Pro_ProveOld po
        LEFT JOIN T_Pro_Prove p1 ON p1.ProveID = po.OldID
        LEFT JOIN T_Sys_Refe r ON p1.Status = r.Value
                                  AND r.RefeTypeID = 9
        LEFT JOIN T_Sys_Dept d ON p1.DeptID = d.DeptID
        LEFT JOIN T_Pro_Student s ON p1.StudentID = s.StudentID
        LEFT JOIN T_Pro_Item i ON p1.ItemID = i.ItemID
        LEFT JOIN T_Pro_Class c ON c.ClassID = p1.ClassID
        LEFT JOIN T_Pro_Profession pr ON pr.ProfessionID = c.ProfessionID
WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "p1.DeptID", "p1.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }


        public AjaxResult UploadOldProve()
        {
            string deptId = Request.Form["Dept"];
            string filePath = Request.Form["filePath"];
            if (string.IsNullOrEmpty(deptId))
            {
                return AjaxResult.Error("请选择校区");
            }
            if (string.IsNullOrEmpty(filePath))
            {
                return AjaxResult.Error("请选择上传的文件");
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

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string errorString = string.Empty;
                    DataRow dr = ds.Tables[0].Rows[i];
                    OldProveModel opm = new OldProveModel();
                    try
                    {
                        opm.name = dr[0].ToString().Replace(" ", "").Replace("\n", "").Replace("\r", "");
                        opm.idCard = dr[1].ToString().Replace(" ", "").Replace("\n", "").Replace("\r", "");
                        opm.sex = dr[2].ToString().Replace(" ", "").Replace("\n", "").Replace("\r", "");
                        opm.mobile = dr[3].ToString().Replace(" ", "").Replace("\n", "").Replace("\r", "");
                        opm.address = dr[4].ToString().Replace(" ", "").Replace("\n", "").Replace("\r", "");
                        opm.proveName = dr[5].ToString().Trim().Replace("\n", "").Replace("\r", "");
                        opm.enrollTime = dr[6].ToString().Replace(" ", "").Replace("\n", "").Replace("\r", "");
                        opm.teacher = dr[7].ToString().Replace(" ", "").Replace("\n", "").Replace("\r", "");
                        opm.remark = dr[8].ToString();
                    }
                    catch (Exception)
                    {
                        return AjaxResult.Error("模板错误");
                    }
                    if (i == 0)
                    {
                        if (opm.name != "姓名" || opm.idCard != "身份证号" || opm.sex != "性别" || opm.mobile != "手机" || opm.address != "地址" || opm.proveName != "证书名称" || opm.enrollTime != "报名时间" || opm.teacher != "教师" || opm.remark != "备注")
                            return AjaxResult.Error("模板错误");
                        else
                            dt = TableTitle(dt, opm);

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(opm.name))
                            errorString += "【姓名不能为空】";
                        else
                        {
                            if (opm.name.Length > 16)
                                errorString += "【姓名不能超过16个字符】";
                        }
                        StudentModel sm = new StudentModel();
                        if (!OtherHelper.CheckIDCard(opm.idCard))
                            errorString += "【身份证号码不规范】";
                        else
                        {
                            sm = StudentBLL.GetStudentModel(opm.idCard);
                            if (!string.IsNullOrEmpty(sm.Name))
                            {
                                if (string.IsNullOrEmpty(StudentBLL.GetStudentModel(opm.name, opm.idCard).StudentID))
                                {
                                    errorString += "【身份证号和姓名不匹配】";
                                }
                            }
                        }
                        string itemId = SelectProve(deptId, opm.proveName);
                        if (string.IsNullOrEmpty(itemId))
                            errorString += "【证书不存在，请先录入该证书】";

                        if (!OtherHelper.IsDateTime(opm.enrollTime))
                            errorString += "【报名时间必须是时间格式】";

                        if (!string.IsNullOrEmpty(opm.remark))
                        {
                            if (opm.remark.Length > 128)
                                errorString += "【备注不能超过128个字符】";
                        }
                        if (string.IsNullOrEmpty(opm.mobile))
                            errorString += "【电话号码不能为空】";
                        else
                        {
                            if (opm.mobile.Length > 16)
                            {
                                errorString += "【电话号码不能超过16个字符】";
                            }
                        }
                        if (RefeBLL.GetRefeValue(opm.sex, "3").Equals("-1"))
                        {
                            errorString += "【性别必须为男/女】";
                        }
                        if (!string.IsNullOrEmpty(sm.StudentID))
                        {
                            DataTable old = ProveBLL.GetProveTablenew(sm.StudentID, itemId);
                            if (old.Rows.Count > 0)
                                errorString += "【该证书已存在，不能重复导入】";

                        }
                        if (errorString.Equals(""))
                        {

                            if (string.IsNullOrEmpty(sm.StudentID))
                            {
                                sm.StudentID = InsertStudent(opm, deptId);
                            }
                            ProveModel pm = new ProveModel();
                            pm.ClassID = "0";
                            pm.CreateID = UserId.ToString();
                            pm.CreateTime = DateTime.Now.ToString();
                            pm.DeptID = deptId;
                            pm.EnrollTime = opm.enrollTime;
                            pm.IsForce = "1";
                            pm.ItemID = itemId;
                            pm.Remark = opm.remark;
                            pm.Status = "2";
                            pm.StudentID = sm.StudentID;
                            pm.UpdateID = UserId.ToString();
                            pm.UpdateTime = DateTime.Now.ToString();
                            string oldPoveId = ProveBLL.InsertProve(pm).ToString();
                            ProveOldModel pom = new ProveOldModel();
                            pom.OldID = oldPoveId;
                            ProveOldBLL.InsertProveOld(pom);
                            successNum++;
                        }
                        else
                        {
                            dt = TableRowsOldProve(dt, opm, errorString);
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
                nm.Sort = "5";
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
                return AjaxResult.Error("操作失败，数据不能为空");
            }
        }

        /// <summary>
        /// 添加学生
        /// </summary>
        /// <param name="opm"></param>
        /// <returns></returns>
        private string InsertStudent(OldProveModel opm, string deptId)
        {
            StudentModel sm = new StudentModel();
            sm.DeptID = deptId;
            sm.QQ = "";
            sm.Name = opm.name;
            sm.IDCard = opm.idCard;
            sm.Sex = RefeBLL.GetRefeValue(opm.sex, "3");
            sm.Mobile = opm.mobile;
            sm.Address = opm.address;
            sm.WeChat = "";
            sm.CreateID = UserId.ToString();
            sm.CreateTime = DateTime.Now.ToString();
            sm.UpdateID = UserId.ToString();
            sm.UpdateTime = DateTime.Now.ToString();
            sm.Status = "1";
            return StudentBLL.InsertStudent(sm).ToString();
        }
        /// <summary>
        /// 获取证书编号
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="proveName"></param>
        /// <returns></returns>
        private string SelectProve(string deptId, string proveName)
        {
            string where = " and Status=1 and Name=@Name and DeptID=@DeptID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@Name",proveName),
                new SqlParameter("@DeptID",deptId)
            };
            return ItemBLL.ItemModelByWhere(where, paras).ItemID;
        }

        /// <summary>
        /// 导出老证书系统数据
        /// </summary>
        /// <returns></returns>
        public AjaxResult DownloadProveFile()
        {
            string cmdText = @"SELECT  si.Stu_Name 姓名 ,
        si.Stu_Sex 性别 ,
        si.Stu_Sid 身份证号 ,
        si.Stu_Telephone 联系电话 ,
        si.Stu_Address 地址 ,
        ssi.CertificateNO 凭证号 ,
        sui.Name 证书名称 ,
        ssi.Money 金额 ,
        ssi.CreateTime 报名时间 ,
        ssi.teacher 教师 ,
        sci.Name 校区
FROM    tb_StuSubjectInfo ssi
        LEFT JOIN tb_StudentInfo si ON ssi.Stu_ID = si.ID
        LEFT JOIN tb_SubjectInfo sui ON ssi.Subject_Code = sui.Code
        LEFT JOIN tb_SchoolInfo sci ON ssi.School_Code = sci.Code";
            string filename = "老证书信息.xls";
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }

        /// <summary>
        /// 老证书系统下载列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOldProveFileList()
        {
            string menuId = Request.Form["MenuID"];
            string dept = Request.Form["treeDept"];

            string where = "";
            if (!string.IsNullOrEmpty(dept))
            {
                where += "AND d.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + dept + "))";
            }
            string cmdText = @"SELECT  d.DeptID ,
        d.Name ,
        pof.FileText,
        ( SELECT    Queue
          FROM      T_Sys_Dept
          WHERE     DeptID = d.ParentID
        ) ParentQueue
FROM    T_Sys_Dept d
        LEFT JOIN T_Pro_ProveOldFile pof ON d.DeptID = pof.DeptID
WHERE   1 = 1
        AND d.Status = 1
        AND ( SELECT    COUNT(DeptID)
              FROM      T_Sys_Dept
              WHERE     Status = 1
                        AND ParentID = d.DeptID
            ) = 0 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "d.DeptID", this.UserId.ToString());
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        private bool ValidateIdCard(string studentId)
        {
            string where = " and StudentID=@StudentID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@StudentID",studentId)            };
            string idcard = StudentBLL.StudentModelByWhere(where, paras).IDCard;
            if (string.IsNullOrEmpty(idcard))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
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
                if (!dt.Columns[0].ColumnName.Trim().Equals("姓名") || !dt.Columns[1].ColumnName.Trim().Equals("身份证号")
                    || !dt.Columns[2].ColumnName.Trim().Equals("证书名称") || !dt.Columns[3].ColumnName.Trim().Equals("证书编号"))
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

        public AjaxResult UploadProveNum(string filePath, string Dept, string Paras)
        {
            if (string.IsNullOrEmpty(filePath))
                return AjaxResult.Error("请选择文件");
            if (string.IsNullOrEmpty(Dept))
                return AjaxResult.Error("请选择校区");
            List<ProveNumExcelModel> prove = JsonConvert.DeserializeObject<List<ProveNumExcelModel>>(Paras);
            if (prove.Count == 0 || prove == null)
            {
                return AjaxResult.Error("不能导入空数据");
            }
            DataTable dt = new DataTable();
            dt = TableTitle(dt);
            decimal successNum = 0;
            decimal errorNum = 0;
            foreach (var item in prove)
            {
                string studentId = string.Empty;
                string itemId = string.Empty;
                ProveNumModel pnm = ReturnProveNumModel(item);
                string errorString = ValidateProveNum(pnm, Dept, ref studentId, ref itemId);

                if (string.IsNullOrEmpty(errorString))
                {
                    ProveModel pm = ProveBLL.GetProveModel(studentId, itemId);
                    pm.ProveNum = pnm.proveNum;
                    pm.UpdateTime = DateTime.Now.ToString();
                    pm.UpdateID = this.UserId.ToString();
                    if (ProveBLL.UpdateProve(pm) > 0)
                    {
                        successNum += 1;
                    }
                }
                else
                {
                    dt = TableRow(dt, item, errorString);
                    errorNum++;
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
            nm.Sort = "14";
            nm.DeptID = Dept;
            nm.Status = "1";
            nm.SuccessNum = successNum.ToString();
            nm.ErrorNum = errorNum.ToString();
            NoteBLL.InsertNote(nm);
            return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "操作成功");
        }
        public string ValidateProveNum(ProveNumModel pnm, string deptId, ref string studentId, ref string itemId)
        {
            string errorString = string.Empty;
            StudentModel sm = StudentBLL.GetStudentModel(pnm.idCard);
            if (string.IsNullOrEmpty(sm.StudentID))
            {
                errorString += "此学生不存在;";
            }
            else
            {
                if (!sm.Name.Equals(pnm.name))
                {
                    errorString += "姓名和身份证号不匹配;";
                }
                else
                {
                    studentId = sm.StudentID;
                }
            }
            ItemModel im = ItemBLL.GetItemModel(deptId, pnm.proveName);
            if (string.IsNullOrEmpty(im.Name))
            {
                errorString += "系统内不存在此证书名称;";
            }
            else
            {
                itemId = im.ItemID;
            }
            DataTable pdt = GetProveTable(sm.StudentID, im.ItemID);
            if (pdt.Rows.Count == 0)
            {
                errorString += "此学生没有报此证书;";
            }
            if (pdt.Rows.Count > 1)
            {
                errorString += "此学生多次报考此证书，请手动添加证书编号";
            }
            if (string.IsNullOrEmpty(pnm.proveNum))
            {
                errorString += "证书编号不能为空;";
            }
            else
            {
                if (pnm.proveNum.Length > 36)
                {
                    errorString += "证书编号不能超过36个字符;";
                }
            }
            return errorString;
        }

        private ProveNumModel ReturnProveNumModel(ProveNumExcelModel pnem)
        {
            ProveNumModel pum = new ProveNumModel();
            pum.name = pnem.姓名.Replace(" ", "");
            pum.idCard = pnem.身份证号.Replace(" ", "");
            pum.proveName = pnem.证书名称.Replace(" ", "");
            pum.proveNum = pnem.证书编号.Replace(" ", "");
            return pum;
        }
        public DataTable TableTitle(DataTable dt)
        {
            dt.Columns.Add("姓名", Type.GetType("System.String"));
            dt.Columns.Add("身份证号", Type.GetType("System.String"));
            dt.Columns.Add("证书名称", Type.GetType("System.String"));
            dt.Columns.Add("证书编号", Type.GetType("System.String"));
            dt.Columns.Add("系统备注", Type.GetType("System.String"));
            return dt;
        }

        public DataTable TableRow(DataTable dt, ProveNumExcelModel em, string remark)
        {
            DataRow dr = dt.NewRow();
            dr["姓名"] = em.姓名;
            dr["身份证号"] = em.身份证号;
            dr["证书名称"] = em.证书名称;
            dr["证书编号"] = em.证书编号;
            dr["系统备注"] = remark;
            dt.Rows.Add(dr);
            return dt;
        }

        public string GetProveNumEdit(string ProveID, string ProveNum)
        {
            if (string.IsNullOrEmpty(ProveNum))
            {
                return "证书编号不能为空";
            }
            else
            {
                if (ProveNum.Length > 36)
                {
                    return "证书编号不能超过36个字符";

                }
            }
            if (string.IsNullOrEmpty(ProveID))
            {
                return "请选择证书信息;";
            }
            ProveModel pm = ProveBLL.GetProveModel(ProveID);
            pm.ProveNum = ProveNum;
            pm.UpdateID = UserId.ToString();
            pm.UpdateTime = DateTime.Now.ToString();
            ProveBLL.UpdateProve(pm);
            return "yes";
        }
    }
}
