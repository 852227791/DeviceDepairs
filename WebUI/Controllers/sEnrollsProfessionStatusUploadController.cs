using BLL;
using Common;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class sEnrollsProfessionStatusUploadController : BaseController
    {
        //
        // GET: /sEnrollsProfessionStatusUpload/
        /// <summary>
        /// 验证模板并返回数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AjaxResult CheckExcelModel(string ID)
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
                    || !dt.Columns[2].ColumnName.Trim().Equals("学历层次") || !dt.Columns[3].ColumnName.Trim().Equals("专业")
                    || !dt.Columns[4].ColumnName.Trim().Equals("在校状态") || !dt.Columns[5].ColumnName.Trim().Equals("变更理由"))
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
        /// 保存导入变更状态
        /// </summary>
        /// <returns></returns>
        public AjaxResult UploadsChangesEnrollsProfessionStatus()
        {
            string deptId = Request.Form["upDept"];
            string filePath = Request.Form["Path"];
            string Paras = Request.Form["Paras"];
            if (string.IsNullOrEmpty(deptId))
            {
                return AjaxResult.Error("校区不能为空");
            }
            if (string.IsNullOrEmpty(filePath))
            {
                return AjaxResult.Error("上传文件不能为空");
            }
            List<UploadChangeEnrollsProfession> list = JsonConvert.DeserializeObject<List<UploadChangeEnrollsProfession>>(Paras);
            if (list == null || list.Count == 0)
            {
                return AjaxResult.Error("不能导入空数据");
            }
            DataTable dt = TableTitle(new DataTable());
            decimal successNum = 0;
            decimal errorNum = 0;
            string senrollsProfession = string.Empty;
            string status = string.Empty;
            foreach (var item in list)
            {
                ChangeEnrollsProfessionModel uem = ReturnChangeEnrollsProfessionModel(item);
                string errorString = string.Empty;
                errorString = Validate(uem, deptId, ref senrollsProfession, ref status);
                sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(senrollsProfession);
                if (!string.IsNullOrEmpty(senrollsProfession) && !status.Equals("-1"))
                {
                    if (!status.Equals("4") && !status.Equals("5") && !status.Equals("6") && !status.Equals("7") && !status.Equals("8"))
                    {
                        errorString += "不能改为此状态";
                    }
                    if (!epm.Status.Equals("4") && !epm.Status.Equals("5") && !epm.Status.Equals("6") && !epm.Status.Equals("7") && !epm.Status.Equals("8"))
                    {
                        errorString += "该报名信息状态不能修改";
                    }
                    if (epm.Status.Equals(status))
                    {
                        errorString += "变更状态与当前状态相同";
                    }

                }

                if (string.IsNullOrEmpty(errorString) && !string.IsNullOrEmpty(senrollsProfession))
                {

                    epm.Status = status;
                    epm.UpdateID = UserId.ToString();
                    epm.UpdateTime = DateTime.Now.ToString();
                    sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                    sEnrollsProfessionStatusModel epsm = new sEnrollsProfessionStatusModel();
                    epsm.Status = "1";
                    epsm.sEnrollsProfessionID = senrollsProfession;
                    epsm.Explain = uem.reson;
                    epsm.StatusValue = status;
                    epsm.UpdateID = UserId.ToString();
                    epsm.UpdateTime = DateTime.Now.ToString();
                    epsm.CreateID = UserId.ToString();
                    epsm.CreateTime = DateTime.Now.ToString();
                    sEnrollsProfessionStatusBLL.InsertsEnrollsProfessionStatus(epsm);
                    successNum++;
                }
                else
                {
                    item.系统备注 = errorString;
                    dt = TableRow(dt, item);
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
            nm.Sort = "9";
            nm.DeptID = deptId;
            nm.Status = "1";
            nm.SuccessNum = successNum.ToString();
            nm.ErrorNum = errorNum.ToString();
            NoteBLL.InsertNote(nm);
            return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "操作成功");
        }
        /// <summary>
        /// 错误文件Table
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable TableTitle(DataTable dt)
        {
            dt.Columns.Add("姓名", Type.GetType("System.String"));
            dt.Columns.Add("身份证号", Type.GetType("System.String"));
            dt.Columns.Add("学历层次", Type.GetType("System.String"));
            dt.Columns.Add("专业", Type.GetType("System.String"));
            dt.Columns.Add("在校状态", Type.GetType("System.String"));
            dt.Columns.Add("变更理由", Type.GetType("System.String"));
            dt.Columns.Add("系统备注", Type.GetType("System.String"));
            return dt;
        }
        /// <summary>
        /// 错误文件Rows
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="em"></param>
        /// <returns></returns>
        public DataTable TableRow(DataTable dt, UploadChangeEnrollsProfession em)
        {
            DataRow dr = dt.NewRow();
            dr["姓名"] = em.姓名;
            dr["身份证号"] = em.身份证号;
            dr["学历层次"] = em.学历层次;
            dr["专业"] = em.专业;
            dr["在校状态"] = em.在校状态;
            dr["变更理由"] = em.变更理由;
            dr["系统备注"] = em.系统备注;
            dt.Rows.Add(dr);
            return dt;
        }
        /// <summary>
        /// 实体类转换
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public ChangeEnrollsProfessionModel ReturnChangeEnrollsProfessionModel(UploadChangeEnrollsProfession em)
        {
            ChangeEnrollsProfessionModel cpm = new ChangeEnrollsProfessionModel();
            cpm.idCard = em.身份证号.Replace(" ", "");
            cpm.name = em.姓名.Replace(" ", "");
            cpm.studyLevel = em.学历层次.Replace(" ", "");
            cpm.major = em.专业.Replace(" ", "");
            cpm.reson = em.变更理由.Replace(" ", "");
            cpm.status = em.在校状态.Replace(" ", "");
            return cpm;

        }
        /// <summary>
        /// 验证字段
        /// </summary>
        /// <param name="uem"></param>
        /// <returns></returns>
        public string Validate(ChangeEnrollsProfessionModel uem, string deptId, ref string senrollsProfession, ref string status)
        {
            string errorString = string.Empty;
            string studentId = string.Empty;
            string sprofessionId = string.Empty;
            if (string.IsNullOrEmpty(uem.name))
            {
                errorString += "姓名不能为空;";
            }
            else
            {
                if (uem.name.Length > 16)
                {
                    errorString += "姓名不能超过16个字符;";
                }
            }

            if (!OtherHelper.CheckIDCard(uem.idCard))
            {
                errorString += "身份证号不规范;";
            }
            else
            {
                StudentModel sm = StudentBLL.GetStudentModel(uem.idCard);
                if (!string.IsNullOrEmpty(sm.Name))
                {
                    if (!uem.name.Equals(sm.Name))
                    {
                        errorString += "姓名和身份证号不匹配;";
                    }
                }
                studentId = sm.StudentID;

            }

            if (string.IsNullOrEmpty(uem.reson))
            {
                errorString += "变更理由不能为空;";
            }
            string studyLevel = RefeBLL.GetRefeValue(uem.studyLevel, "17");
            if (studyLevel.Equals("-1"))
            {
                errorString += "学习层次不存在;";
            }
            status = RefeBLL.GetRefeValue(uem.status, "21");
            if (status.Equals("-1"))
            {
                errorString += "变更状态不存在;";
            }

            if (!status.Equals("4") && !status.Equals("5") && !status.Equals("6") && !status.Equals("7"))
            {
                errorString += "不能改为此状态";
            }
            if (string.IsNullOrEmpty(studentId))
            {
                errorString += "此学生不存在;";
            }
            if (!string.IsNullOrEmpty(studentId) && string.IsNullOrEmpty(errorString))
            {
                DataTable enrollTab = sEnrollBLL.GetsEnrollTable(studentId);
                if (enrollTab.Rows.Count > 1)
                {
                    errorString += "此学生多次报名，请手动变更在校状态;";
                }
                else if (enrollTab.Rows.Count == 0)
                {
                    errorString += "此学生未报名;";
                }
                else
                {

                    if (string.IsNullOrEmpty(errorString))
                    {
                        DataTable senrollPro = sEnrollsProfessionBLL.GetsEnrollsProfessionTable(studyLevel, enrollTab.Rows[0]["sEnrollID"].ToString(), uem.major, deptId);
                        if (senrollPro.Rows.Count > 1)
                        {
                            errorString += "此学生已经多次报名此专业，请手修改状态;";
                        }
                        else if (senrollPro.Rows.Count == 0)
                        {
                            errorString += "此学生没有报过任何专业;";
                        }
                        else
                        {
                            if (!status.Equals(senrollPro.Rows[0]["Status"].ToString()))
                            {
                                senrollsProfession = senrollPro.Rows[0]["sEnrollsProfessionID"].ToString();
                            }
                            else
                            {
                                errorString += "变更状态和当前状态相同;";
                            }
                        }
                    }

                }
            }

            return errorString;
        }
    }
}
