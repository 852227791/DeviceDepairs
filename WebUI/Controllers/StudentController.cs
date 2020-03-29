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
    public class StudentController : BaseController
    {
        //
        // GET: /Student/
        #region 页面加载
        public ActionResult StudentView()
        {
            return View();
        }
        public ActionResult StudentModify()
        {
            return View();
        }
        public ActionResult ChooseStudent()
        {
            return View();
        }
        public ActionResult StudentList()
        {
            return View();
        }
        public ActionResult StudentEdit()
        {
            return View();
        }
        public ActionResult StudentEditNo()
        {
            return View();
        }
        public ActionResult StudentEditIDCard()
        {
            return View();
        }

        public ActionResult StudentPicture()
        {
            return View();
        }
        #endregion


        public ActionResult ChooseStudentAdd()
        {
            return View();
        }
        #region 学生信息列表
        public ActionResult GetStudentList()
        {
            string menuId = Request.Form["MenuID"];
            string name = Request.Form["txtName"];
            string idCard = Request.Form["txtIDCard"];
            string mobile = Request.Form["txtMobile"];
            string deptId = Request.Form["selDept"];
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += " and s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                where += " and s.Mobile like '%" + mobile + "%'";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND s.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            string cmdText = @"SELECT  s.StudentID ,
        d.Name DeptName ,
        s.Name ,
        s.IDCard ,
        r2.RefeName Sex ,
        s.Mobile ,
        s.QQ ,
        s.WeChat ,
        s.Address ,
        r1.RefeName Status ,
        s.Status StatusValue ,
        ( SELECT    EnrollNum+','
          FROM      T_Stu_sEnroll
          WHERE     StudentID = s.StudentID FOR XML PATH ('')
        ) EnrollNum
FROM    T_Pro_Student AS s
        LEFT JOIN T_Sys_Dept d ON d.DeptID = s.DeptID
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = s.Status
                                      AND r1.RefeTypeID = 1
        LEFT JOIN T_Sys_Refe AS r2 ON r2.Value = s.Sex
                                      AND r2.RefeTypeID = 3
							
WHERE   1 = 1 {0}";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "s.DeptID", "s.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        #endregion

        #region  导出学生信息
        public AjaxResult DownloadStudent()
        {
            string menuId = Request.Form["MenuID"];
            string name = Request.Form["txtName"];
            string idCard = Request.Form["txtIDCard"];
            string mobile = Request.Form["txtMobile"];
            string deptId = Request.Form["selDept"];
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += " and s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                where += " and s.Mobile like '%" + mobile + "%'";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                where += "AND s.DeptID in (SELECT DeptID FROM GetChildrenDeptID(" + deptId + "))";
            }
            string cmdText = @"SELECT  d.Name 校区 ,
        s.Name 姓名 ,
        ( SELECT    EnrollNum+','
          FROM      T_Stu_sEnroll
          WHERE     StudentID = s.StudentID FOR XML PATH ('')
        ) 学号 ,
        s.IDCard 身份证号 ,
        r2.RefeName 性别 ,
        s.Mobile 联系电话 ,
        s.QQ QQ号 ,
        s.WeChat 微信号 ,
        s.Address 地址 ,
        r1.RefeName 状态
FROM    T_Pro_Student AS s
        LEFT JOIN T_Sys_Dept d ON d.DeptID = s.DeptID
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = s.Status
                                      AND r1.RefeTypeID = 1
        LEFT JOIN T_Sys_Refe AS r2 ON r2.Value = s.Sex
                                      AND r2.RefeTypeID = 3
							
WHERE   1 = 1 {0}";
            string filename = "交费人信息.xls";
            string powerStr = PurviewToList(menuId);
            powerStr = string.Format(powerStr, "s.DeptID", "s.CreateID");
            cmdText = string.Format(cmdText, where + powerStr);
            DataTable dt = DatabaseFactory.ExecuteDataset(cmdText, CommandType.Text).Tables[0];
            OtherHelper.DeriveToExcel(dt, filename);
            return AjaxResult.Success(filename);
        }
        #endregion

        #region 编辑学生信息
        /// <summary>
        /// 编辑学生信息
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public string GetStudentEdit(StudentModel sm)
        {
            if (string.IsNullOrEmpty(sm.StudentID))
            {
                sm.StudentID = "0";
            }
            if (string.IsNullOrEmpty(sm.DeptID))
            {
                return "请选择校区 ";
            }
            if (string.IsNullOrEmpty(sm.Name))
            {
                return "姓名不能为空";
            }
            else
            {
                if (sm.Name.Length > 16)
                {
                    return "姓名长度不能超过16个字符";
                }
            }
            if (!OtherHelper.CheckIDCard(sm.IDCard))
            {
                return "身份证号不规范 ";
            }
            if (CheckIDCardIsRepeatMethod(sm.IDCard, sm.StudentID))
            {
                return "身份证号已经存在 ";
            }
            if (string.IsNullOrEmpty(sm.Sex))
            {
                return "请选择性别 ";
            }
            if (string.IsNullOrEmpty(sm.Mobile))
            {
                return "联系电话不能为空";
            }
            else
            {
                if (sm.Mobile.Length > 16)
                {
                    return "联系电话长度不能超过16个字符";
                }
            }
            if (!string.IsNullOrEmpty(sm.QQ))
            {
                if (sm.QQ.Length > 16)
                {
                    return "qq号长度不能超过16个字符";
                }
            }
            if (!string.IsNullOrEmpty(sm.WeChat))
            {
                if (sm.WeChat.Length > 32)
                {
                    return "微信号长度不能超过32个字符";
                }
            }

            if (!string.IsNullOrEmpty(sm.Address))
            {
                if (sm.Address.Length > 128)
                {
                    return "家庭住址能超过128个字符 ";
                }
            }
            if (!string.IsNullOrEmpty(sm.Remark))
            {
                sm.Remark = sm.Remark.Replace("\r\n", "<br />");
            }
            bool flag = false;
            if (sm.StudentID == "0")
            {
                sm.Status = "1";
                sm.CreateID = this.UserId.ToString();
                sm.CreateTime = DateTime.Now.ToString();
                sm.UpdateID = this.UserId.ToString();
                sm.UpdateTime = DateTime.Now.ToString();
                if (StudentBLL.InsertStudent(sm) > 0)
                {
                    flag = true;
                }
            }
            else
            {
                StudentModel editsm = GetStudentModel(sm.StudentID);
                LogBLL.CreateLog("T_Pro_Student", this.UserId.ToString(), editsm, sm);
                editsm.DeptID = sm.DeptID;
                editsm.Name = sm.Name;
                editsm.IDCard = sm.IDCard;
                editsm.Sex = sm.Sex;
                editsm.Mobile = sm.Mobile;
                editsm.QQ = sm.QQ;
                editsm.WeChat = sm.WeChat;
                editsm.Address = sm.Address;
                editsm.Remark = sm.Remark;
                editsm.UpdateID = this.UserId.ToString();
                editsm.UpdateTime = DateTime.Now.ToString();
                if (StudentBLL.UpdateStudent(editsm) > 0)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                return "yes";
            }
            else
            {
                return "出现未知错误，请联系管理员";
            }
        }
        #endregion

        #region 编辑学生信息(无身份证)
        /// <summary>
        /// 编辑学生信息（无身份证）
        /// </summary>
        /// <param name="noStudentID"></param>
        /// <param name="noDeptID"></param>
        /// <param name="noName"></param>
        /// <param name="noIDCard"></param>
        /// <param name="noSex"></param>
        /// <param name="noMobile"></param>
        /// <param name="noQQ"></param>
        /// <param name="noWeChat"></param>
        /// <param name="noAddress"></param>
        /// <param name="noRemark"></param>
        /// <returns></returns>
        public string GetStudentEditNo(string noStudentID, string noDeptID, string noName, string noIDCard, string noSex, string noMobile, string noQQ, string noWeChat, string noAddress, string noRemark)
        {
            StudentModel sm = new StudentModel();
            sm.StudentID = noStudentID;
            sm.DeptID = noDeptID;
            sm.Name = noName;
            sm.IDCard = noIDCard;
            sm.Sex = noSex;
            sm.Mobile = noMobile;
            sm.QQ = noQQ;
            sm.WeChat = noWeChat;
            sm.Address = noAddress;
            sm.Remark = noRemark;
            if (string.IsNullOrEmpty(sm.StudentID))
            {
                sm.StudentID = "0";
            }
            if (string.IsNullOrEmpty(sm.DeptID))
            {
                return "请选择校区 ";
            }
            if (string.IsNullOrEmpty(sm.Name))
            {
                return "姓名不能为空";
            }
            else
            {
                if (sm.Name.Length > 16)
                {
                    return "姓名长度不能超过16个字符";
                }
            }
            if (string.IsNullOrEmpty(sm.Sex))
            {
                return "请选择性别 ";
            }
            if (string.IsNullOrEmpty(sm.Mobile))
            {
                return "联系电话不能为空";
            }
            else
            {
                if (sm.Mobile.Length > 16)
                {
                    return "联系电话长度不能超过16个字符";
                }
            }
            if (!string.IsNullOrEmpty(sm.QQ))
            {
                if (sm.QQ.Length > 16)
                {
                    return "qq号长度不能超过16个字符";
                }
            }
            if (!string.IsNullOrEmpty(sm.WeChat))
            {
                if (sm.WeChat.Length > 32)
                {
                    return "微信号长度不能超过32个字符";
                }
            }

            if (!string.IsNullOrEmpty(sm.Address))
            {
                if (sm.Address.Length > 128)
                {
                    return "家庭住址能超过128个字符 ";
                }
            }
            if (!string.IsNullOrEmpty(sm.Remark))
            {
                sm.Remark = sm.Remark.Replace("\r\n", "<br />");
            }
            bool flag = false;
            if (sm.StudentID == "0")
            {
                sm.Status = "1";
                sm.CreateID = this.UserId.ToString();
                sm.CreateTime = DateTime.Now.ToString();
                sm.UpdateID = this.UserId.ToString();
                sm.UpdateTime = DateTime.Now.ToString();
                if (StudentBLL.InsertStudent(sm) > 0)
                {
                    flag = true;
                }
            }
            else
            {
                StudentModel editsm = GetStudentModel(sm.StudentID);
                LogBLL.CreateLog("T_Pro_Student", this.UserId.ToString(), editsm, sm);
                editsm.DeptID = sm.DeptID;
                editsm.Name = sm.Name;
                editsm.IDCard = sm.IDCard;
                editsm.Sex = sm.Sex;
                editsm.Mobile = sm.Mobile;
                editsm.QQ = sm.QQ;
                editsm.WeChat = sm.WeChat;
                editsm.Address = sm.Address;
                editsm.Remark = sm.Remark;
                editsm.UpdateID = this.UserId.ToString();
                editsm.UpdateTime = DateTime.Now.ToString();
                if (StudentBLL.UpdateStudent(editsm) > 0)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                return "yes";
            }
            else
            {
                return "出现未知错误，请联系管理员";
            }
        }
        #endregion

        #region 补录身份证
        public string GetStudentEditIDCard(string idStudentID, string idIDCard)
        {
            StudentModel sm = new StudentModel();
            sm.StudentID = idStudentID;
            sm.IDCard = idIDCard;
            if (!string.IsNullOrEmpty(sm.StudentID))
            {
                if (!OtherHelper.CheckIDCard(sm.IDCard))
                {
                    return "身份证号不规范 ";
                }
                if (CheckIDCardIsRepeatMethod(sm.IDCard, sm.StudentID))
                {
                    return "身份证号已经存在 ";
                }
                bool flag = false;

                StudentModel editsm = GetStudentModel(sm.StudentID);
                LogBLL.CreateLog("T_Pro_Student", this.UserId.ToString(), editsm, sm);
                editsm.IDCard = sm.IDCard;
                editsm.UpdateID = this.UserId.ToString();
                editsm.UpdateTime = DateTime.Now.ToString();
                if (StudentBLL.UpdateStudent(editsm) > 0)
                {
                    flag = true;
                }
                if (flag)
                {
                    return "yes";
                }
                else
                {
                    return "出现未知错误，请联系管理员";
                }
            }
            else
            {
                return "出现未知错误，请联系管理员";
            }
        }
        #endregion

        #region 验证身份证号重复
        public AjaxResult CheckIDCardIsRepeat()
        {
            string studentId = Request.Form["ID"];
            string idCard = Request.Form["Value"];
            if (CheckIDCardIsRepeatMethod(idCard, studentId))
            {
                return AjaxResult.Error("身份证号已被使用");
            }
            else
            {
                return AjaxResult.Success();
            }
        }
        #endregion

        #region 验证身份证号是否规范
        public AjaxResult CheckIDCardIsStandard()
        {
            string idCard = Request.Form["Value"];
            if (!OtherHelper.CheckIDCard(idCard))
            {
                return AjaxResult.Error("身份证号不规范");
            }
            else
            {
                return AjaxResult.Success();
            }
        }
        #endregion

        #region 验证身份证号是否重复的方法
        public bool CheckIDCardIsRepeatMethod(string idCard, string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                studentId = "0";
            }
            string where = " and StudentID<>@StudentID and IDCard=@IDCard ";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter ("@StudentID",studentId),
            new SqlParameter("@IDCard",idCard)
            };
            DataTable dt = StudentBLL.StudentTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 获取学生信息Table
        private static StudentModel GetStudentModel(string studentId)
        {
            string where = " and StudentID=@StudentID ";
            SqlParameter[] paras = new SqlParameter[] {
            new SqlParameter("@StudentID",studentId)
            };
            StudentModel sm = StudentBLL.StudentModelByWhere(where, paras);
            return sm;
        }
        #endregion

        #region 修改时赋值
        public AjaxResult SelectStudent()
        {
            string studentId = Request.Form["ID"];
            string where = " and s.StudentID=@StudentID";
            SqlParameter[] paras = new SqlParameter[] {
         new SqlParameter("@StudentID",studentId)
        };
            DataTable dt = StudentBLL.StudentClassTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }
        #endregion

        #region 查看
        public AjaxResult SelectViewStudent()
        {
            string studentId = Request.Form["ID"];
            string cmdText = @"SELECT  s.StudentID ,
        d.Name DeptName ,
        s.Name ,
        s.IDCard ,
        r.RefeName Sex ,
        s.Mobile ,
        s.QQ ,
        s.WeChat ,
        s.Address ,
        s.Remark
FROM    T_Pro_Student s
        LEFT JOIN T_Sys_Dept d ON s.DeptID = d.DeptID
        LEFT JOIN T_Sys_Refe r ON s.Sex = r.Value
                                  AND r.RefeTypeID = 3
WHERE   s.StudentID = {0}";
            cmdText = string.Format(cmdText, studentId);
            return AjaxResult.Success(JsonData.GetArray(cmdText));
        }
        #endregion

        #region 变更状态
        public AjaxResult GetUpdateStudentStatus()
        {
            string studentId = Request.Form["ID"];
            string status = Request.Form["Value"];
            StudentModel sm = GetStudentModel(studentId);
            sm.Status = status;
            sm.UpdateID = this.UserId.ToString();
            sm.UpdateTime = DateTime.Now.ToString();
            if (StudentBLL.UpdateStudent(sm) > 0)
            {
                return AjaxResult.Success();

            }
            else
            {
                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
        }
        #endregion

        #region 获取选择学生列表
        public ActionResult GetChooseStudentList()
        {
            string name = Request.Form["txtName"];
            string idCard = Request.Form["txtIDCard"];
            string enrollNum = Request.Form["txtEnrollNum"];
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += " and s.Name like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                where += " and s.IDCard like '%" + idCard + "%'";
            }
            if (!string.IsNullOrEmpty(enrollNum))
            {
                where += " and e.EnrollNum like '%" + enrollNum + "%'";
            }


            string cmdText = @"SELECT  s.StudentID ,
        d.Name DeptName ,
        s.Name ,
        s.IDCard ,
        r2.RefeName Sex ,
        s.Mobile ,
        s.QQ ,
        s.WeChat ,
        s.Address ,
        r1.RefeName Status,
		e.EnrollNum,
		si.Photo
FROM    T_Pro_Student AS s
        LEFT JOIN T_Sys_Dept d ON d.DeptID = s.DeptID
		LEFT JOIN T_Stu_sEnroll e ON e.StudentID=s.StudentID
        LEFT JOIN T_Sys_Refe AS r1 ON r1.Value = s.Status
                                      AND r1.RefeTypeID = 1
        LEFT JOIN T_Sys_Refe AS r2 ON r2.Value = s.Sex
                                      AND r2.RefeTypeID = 3
		LEFT JOIN T_Pro_StudentInfo si ON si.StudentID=s.StudentID
WHERE   s.Status = 1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }
        #endregion

        /// <summary>
        /// 导入学生信息
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentUpload()
        {
            return View();
        }

        /// <summary>
        /// 验证模板并返回数据
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
                if (!dt.Columns[0].ColumnName.Trim().Equals("学号") || !dt.Columns[1].ColumnName.Trim().Equals("姓名") || !dt.Columns[2].ColumnName.Trim().Equals("身份证号") || !dt.Columns[3].ColumnName.Trim().Equals("性别") || !dt.Columns[4].ColumnName.Trim().Equals("手机号") || !dt.Columns[5].ColumnName.Trim().Equals("QQ") || !dt.Columns[6].ColumnName.Trim().Equals("微信") || !dt.Columns[7].ColumnName.Trim().Equals("地址") || !dt.Columns[8].ColumnName.Trim().Equals("备注"))
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

        public AjaxResult UploadStdent(string filePath, string Dept, string Paras)
        {
            if (string.IsNullOrEmpty(filePath))
                return AjaxResult.Error("请选择文件");
            if (string.IsNullOrEmpty(Dept))
                return AjaxResult.Error("请选择校区");

            List<ImportStudentModel> stu = JsonConvert.DeserializeObject<List<ImportStudentModel>>(Paras);

            if (stu != null && stu.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = TableTitle(dt);
                decimal successNum = 0;
                decimal errorNum = 0;
                foreach (var item in stu)
                {
                    string errorString = ValidateWords(item, Dept);
                    if (string.IsNullOrEmpty(errorString))
                    {
                        string tempId = StudentBLL.getStudentId(item.身份证号.Trim());
                        StudentModel sm = new StudentModel();
                        if (tempId.Equals("0"))
                        {
                            sm.IDCard = item.身份证号.Replace(" ", "");
                            sm.DeptID = Dept.Replace(" ", "");
                            sm.Address = item.地址.Replace(" ", "");
                            sm.CreateID = UserId.ToString();
                            sm.CreateTime = DateTime.Now.ToString();
                            sm.Mobile = item.手机号.Replace(" ", "");
                            sm.Name = item.姓名.Replace(" ", "");
                            sm.QQ = item.QQ.Replace(" ", "");
                            sm.Remark = item.备注;
                            sm.Sex = RefeBLL.GetRefeValue(item.性别.Trim(), "3");
                            sm.Status = "1";
                            sm.UpdateID = UserId.ToString();
                            sm.UpdateTime = DateTime.Now.ToString();
                            sm.WeChat = item.微信.Replace(" ", "");
                            sm.StudentID = StudentBLL.InsertStudent(sm).ToString();
                            InsetsEnroll(sm.StudentID, Dept, item.学号.Replace(" ", ""));

                        }
                        else
                        {
                            DataTable sEnrollDT = sEnrollBLL.GetStudentEnroll(tempId);
                            if (sEnrollDT.Rows.Count == 0)
                            {
                                InsetsEnroll(tempId, Dept, item.学号.Replace(" ", ""));
                            }
                            else if (sEnrollDT.Rows.Count == 1)
                            {
                                sEnrollModel sem = sEnrollBLL.GetEnrollModel(sEnrollDT.Rows[0]["sEnrollID"].ToString());
                                sem.EnrollNum = item.学号.Replace(" ", "");
                                sem.UpdateID = this.UserId.ToString();
                                sem.UpdateTime = DateTime.Now.ToString();
                                sEnrollBLL.UpdatesEnroll(sem);
                            }
                        }
                        successNum++;
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
                nm.Sort = "7";
                nm.DeptID = Dept;
                nm.Status = "1";
                nm.SuccessNum = successNum.ToString();
                nm.ErrorNum = errorNum.ToString();
                NoteBLL.InsertNote(nm);
                return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "操作成功");
            }
            else
            {
                return AjaxResult.Error("操作失败，数据不能为空");
            }

        }

        private void InsetsEnroll(string studentId, string deptId, string enrollNum)
        {
            sEnrollModel em = new sEnrollModel();
            em.CreateID = UserId.ToString();
            em.CreateTime = DateTime.Now.ToString();
            em.DeptID = deptId;
            em.EnrollNum = enrollNum;
            em.Status = "1";
            em.StudentID = studentId;
            em.UpdateID = UserId.ToString();
            em.UpdateTime = DateTime.Now.ToString();
            sEnrollBLL.InsertsEnroll(em);
        }
        private string ValidateWords(ImportStudentModel item, string deptId)
        {
            string errorString = "";
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
            if (string.IsNullOrEmpty(item.学号))
            {
                errorString += "学号不能为空;";
            }
            else
            {
                if (item.学号.Length > 16)
                {
                    errorString += "学号不能超过16个字符;";
                }
            }

            if (string.IsNullOrEmpty(item.手机号))
            {
                errorString += "手机号不能为空;";
            }
            else
            {
                if (item.手机号.Length > 16)
                {
                    errorString += "手机号不能超过16个字符;";
                }
            }
            if (string.IsNullOrEmpty(item.身份证号))
            {
                errorString += "身份证号不能为空;";
            }
            else
            {
                if (!OtherHelper.CheckIDCard(item.身份证号.Trim()))
                    errorString += "身份证号不规范;";
                else
                {
                    StudentModel sm = StudentBLL.GetStudentModel(item.身份证号.Trim());
                    if (!string.IsNullOrEmpty(sm.Name))
                    {
                        if (!sm.Name.Equals(item.姓名.Replace(" ", "")))
                        {
                            errorString += "身份证号和姓名不匹配;";
                        }

                    }
                }
            }
            if (string.IsNullOrEmpty(item.性别))
            {
                errorString += "性别不能为空;";
            }
            else
            {
                if (RefeBLL.GetRefeValue(item.性别.Trim(), "3") == "-1")
                {
                    errorString += "性别必须为男/女;";
                }
            }
            if (!string.IsNullOrEmpty(item.QQ))
            {
                if (item.QQ.Length > 16)
                    errorString += "QQ不能超过16个字符;";
            }
            if (!string.IsNullOrEmpty(item.微信))
            {
                if (item.微信.Length > 32)
                    errorString += "微信不能超过32个字符;";
            }
            if (!string.IsNullOrEmpty(item.地址))
            {
                if (item.地址.Length > 128)
                    errorString += "地址不能超过128个字符;";
            }
            string tempId = StudentBLL.getStudentId(item.身份证号.Trim());
            if (!tempId.Equals("0"))
            {
                if (sEnrollBLL.GetStudentEnroll(tempId).Rows.Count > 1)
                {
                    errorString += "已存在多条报名信息;";
                }
            }
            if (StudentBLL.ValidateEnrollNum(item.学号, deptId))
            {
                errorString += "此学号已存在;";
            }
            return errorString;
        }


        public DataTable TableTitle(DataTable dt)
        {
            dt.Columns.Add("学号", Type.GetType("System.String"));
            dt.Columns.Add("姓名", Type.GetType("System.String"));
            dt.Columns.Add("身份证号", Type.GetType("System.String"));
            dt.Columns.Add("性别", Type.GetType("System.String"));
            dt.Columns.Add("手机号", Type.GetType("System.String"));
            dt.Columns.Add("QQ", Type.GetType("System.String"));
            dt.Columns.Add("微信", Type.GetType("System.String"));
            dt.Columns.Add("地址", Type.GetType("System.String"));
            dt.Columns.Add("备注", Type.GetType("System.String"));
            dt.Columns.Add("系统备注", Type.GetType("System.String"));
            return dt;
        }

        public DataTable TableRow(DataTable dt, ImportStudentModel em, string remark)
        {
            DataRow dr = dt.NewRow();
            dr["学号"] = em.学号;
            dr["姓名"] = em.姓名;
            dr["身份证号"] = em.身份证号;
            dr["性别"] = em.性别;
            dr["手机号"] = em.手机号;
            dr["QQ"] = em.QQ;
            dr["微信"] = em.微信;
            dr["地址"] = em.地址;
            dr["备注"] = em.备注;
            dr["系统备注"] = remark;
            dt.Rows.Add(dr);
            return dt;
        }
        /// <summary>
        /// 缴费人选择器，快速添加
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public AjaxResult AddStudent(AddStudentModel asm)
        {

            if (string.IsNullOrEmpty(asm.deptId))
            {
                return AjaxResult.Error("请选择收费单位");

            }
            if (string.IsNullOrEmpty(asm.addName))
            {
                return AjaxResult.Error("姓名不能为空");
            }
            else
            {
                if (asm.addName.Length > 8)
                {
                    return AjaxResult.Error("姓名不能超过八个字符");
                }

            }
            if (string.IsNullOrEmpty(asm.addMobile))
            {
                return AjaxResult.Error("电话不能为空");
            }
            else
            {
                if (asm.addMobile.Length > 16)
                {
                    return AjaxResult.Error("电话不能超过16个字符");
                }

            }
            if (string.IsNullOrEmpty(asm.addSex))
            {
                return AjaxResult.Error("请选择性别");
            }
            if (!string.IsNullOrEmpty(asm.addAddress))
            {
                if (asm.addAddress.Length > 128)
                {
                    return AjaxResult.Error("地址不能超过128个字符");
                }
            }
            if (!string.IsNullOrEmpty(asm.addIDCard))
            {
                if (!OtherHelper.CheckIDCard(asm.addIDCard))
                {
                    return AjaxResult.Error("身份证号不规范");
                }
                StudentModel vsm = StudentBLL.GetStudentModel(asm.addIDCard);
                if (!string.IsNullOrEmpty(vsm.StudentID))
                {
                    return AjaxResult.Error("身份证号已存在");
                }
            }
            StudentModel sm = new StudentModel();
            sm.Address = asm.addAddress;
            sm.CreateID = UserId.ToString();
            sm.CreateTime = DateTime.Now.ToString();
            sm.DeptID = asm.deptId;
            sm.IDCard = asm.addIDCard;
            sm.Mobile = asm.addMobile;
            sm.Name = asm.addName;
            sm.QQ = "";
            sm.Remark = "";
            sm.Sex = asm.addSex;
            sm.Status = "1";
            sm.WeChat = "";
            sm.UpdateID = UserId.ToString();
            sm.UpdateTime = DateTime.Now.ToString();
            try
            {
                sm.StudentID = StudentBLL.InsertStudent(sm).ToString();
            }
            catch (Exception)
            {

                return AjaxResult.Error("出现未知错误，请联系管理员");
            }
            return AjaxResult.Success(OtherHelper.JsonSerializer(sm), "success");
        }
        /// <summary>
        /// 根据姓名和身份证号返回学生信息
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IdCard"></param>
        /// <returns></returns>
        public AjaxResult GetStudent(string Name, string IdCard)
        {
            StudentModel sm1 = StudentBLL.GetStudentModel(IdCard);
            if (string.IsNullOrEmpty(sm1.StudentID))
            {
                return AjaxResult.Success("yes");
            }
            else
            {
                DataTable dt = StudentBLL.StudentTableByWhere(Name, IdCard);
                if (dt.Rows.Count == 0)
                {
                    return AjaxResult.Error("身份证和姓名不匹配");
                }
                else
                {
                    return AjaxResult.Success(JsonHelper.DataTableToJson(dt), "success");
                }
            }
        }

        public AjaxResult SelectViewStudentInfo(string ID)
        {
            return AjaxResult.Success(JsonHelper.DataTableToJson(StudentInfoBLL.GetStudentInfoView(ID)), "");
        }
        /// <summary>
        /// 保存上传照片
        /// </summary>
        /// <param name="StudentIDpic"></param>
        /// <param name="filePathpic"></param>
        /// <returns></returns>
        public AjaxResult UploadStudentPicture(string StudentIDpic, string filePathpic)
        {
            if (string.IsNullOrEmpty(StudentIDpic))
            {
                return AjaxResult.Error("请选择缴费人信息");
            }
            if (string.IsNullOrEmpty(filePathpic))
            {
                return AjaxResult.Error("上传文件不能为空！");
            }
            try
            {

                StudentInfoModel sm = StudentInfoBLL.GetStudentInfoModel(StudentIDpic);
                if (!string.IsNullOrEmpty(sm.StudentInfoID))
                {
                    sm.Photo = filePathpic;
                    sm.UpdateID = UserId.ToString();
                    sm.UpdateTime = DateTime.Now.ToString();
                    StudentInfoBLL.UpdateStudentInfo(sm);
                    return AjaxResult.Success("", "保存成功");

                }
                else
                {
                    sm.StudentID = StudentIDpic;
                    sm.CreateID = UserId.ToString();
                    sm.CreateTime= DateTime.Now.ToString();
                    sm.UpdateID = UserId.ToString();
                    sm.UpdateTime = DateTime.Now.ToString();
                    sm.Photo = filePathpic;
                    StudentInfoBLL.InsertStudentInfo(sm);
                    return AjaxResult.Success("", "保存成功");
                }
            }
            catch (Exception ex)
            {
                return AjaxResult.Error(ex.Message);
            }


        }
    }
}
