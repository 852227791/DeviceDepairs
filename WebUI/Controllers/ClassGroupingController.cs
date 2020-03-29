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
    public class ClassGroupingController : BaseController
    {
        //
        // GET: /ClassGrouping/

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
                    || !dt.Columns[4].ColumnName.Trim().Equals("班级"))
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


        public AjaxResult SaveClassGrouping()
        {
            string filePath = Request.Form["groupPath"];
            string deptId = Request.Form["groupDeptId"];
            string Paras = Request.Form["Paras"];
            if (string.IsNullOrEmpty(deptId))
            {
                return AjaxResult.Error("校区不能为空");
            }
            if (string.IsNullOrEmpty(filePath))
            {
                return AjaxResult.Error("上传文件不能为空");
            }
            List<ClassGroupingCHModel> list = JsonConvert.DeserializeObject<List<ClassGroupingCHModel>>(Paras);
            if (list == null || list.Count == 0)
            {
                return AjaxResult.Error("不能导入空数据");
            }
            DataTable dt = TableTitle(new DataTable());
            decimal successNum = 0;
            decimal errorNum = 0;
            foreach (var item in list)
            {
                string classId = string.Empty;
                string senrollsProfessionId = string.Empty;
                ClassGroupingModel cm = GetClassGroupingModel(item);
                string errorString = Validate(cm, ref classId, deptId, ref senrollsProfessionId);
                if (string.IsNullOrEmpty(errorString))
                {
                    sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(senrollsProfessionId);
                    epm.ClassID = classId;
                    epm.UpdateID = UserId.ToString();
                    epm.UpdateTime = DateTime.Now.ToString();
                    sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                    successNum++;
                }
                else
                {
                    errorNum++;
                    item.系统备注 = errorString;
                    dt = TableRow(dt, item);
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
            nm.Sort = "11";
            nm.DeptID = deptId;
            nm.Status = "1";
            nm.SuccessNum = successNum.ToString();
            nm.ErrorNum = errorNum.ToString();
            NoteBLL.InsertNote(nm);
            return AjaxResult.Success(JsonGridData.GetGridJSON(dt), "操作成功");
        }
        /// <summary>
        /// 错误文件table
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable TableTitle(DataTable dt)
        {
            dt.Columns.Add("姓名", Type.GetType("System.String"));
            dt.Columns.Add("身份证号", Type.GetType("System.String"));
            dt.Columns.Add("学历层次", Type.GetType("System.String"));
            dt.Columns.Add("专业", Type.GetType("System.String"));
            dt.Columns.Add("班级", Type.GetType("System.String"));
            dt.Columns.Add("系统备注", Type.GetType("System.String"));
            return dt;
        }
        /// <summary>
        /// 错误文件rows
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="em"></param>
        /// <returns></returns>
        public DataTable TableRow(DataTable dt, ClassGroupingCHModel em)
        {
            DataRow dr = dt.NewRow();
            dr["姓名"] = em.姓名;
            dr["身份证号"] = em.身份证号;
            dr["学历层次"] = em.学历层次;
            dr["专业"] = em.专业;
            dr["班级"] = em.班级;
            dr["系统备注"] = em.系统备注;
            dt.Rows.Add(dr);
            return dt;
        }
        /// <summary>
        /// 实体类转换
        /// </summary>
        /// <param name="cgchm"></param>
        /// <returns></returns>
        public ClassGroupingModel GetClassGroupingModel(ClassGroupingCHModel cgchm)
        {
            ClassGroupingModel cgm = new ClassGroupingModel();
            cgm.classId = cgchm.班级.Replace(" ", "");
            cgm.IdCard = cgchm.身份证号.Replace(" ", "");
            cgm.major = cgchm.专业.Replace(" ", "");
            cgm.name = cgchm.姓名.Replace(" ", "");
            cgm.studyLevel = cgchm.学历层次.Replace(" ", "");
            return cgm;
        }

        /// <summary>
        /// 验证字段
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="classId"></param>
        /// <param name="deptId"></param>
        /// <param name="senrollProfessionId"></param>
        /// <returns></returns>
        public string Validate(ClassGroupingModel cm, ref string classId, string deptId, ref string senrollProfessionId)
        {
            string errorString = string.Empty;
            string studentId = string.Empty;
            if (string.IsNullOrEmpty(cm.name))
            {
                errorString += "姓名不能为空;";
            }

            if (!OtherHelper.CheckIDCard(cm.IdCard))
            {
                errorString += "身份证号不规范;";
            }
            else
            {
                StudentModel sm = StudentBLL.GetStudentModel(cm.IdCard);
                if (!string.IsNullOrEmpty(sm.Name))
                {
                    if (!cm.name.Equals(sm.Name))
                    {
                        errorString += "姓名和身份证号不匹配;";
                    }
                }
                studentId = sm.StudentID;
            }
            string studyLevel = RefeBLL.GetRefeValue(cm.studyLevel, "17");
            if (studyLevel.Equals("-1"))
            {
                errorString += "学习层次不存在;";
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
                    errorString += "此学生多次报名，请手动编辑班级信息;";
                }
                else if (enrollTab.Rows.Count == 0)
                {
                    errorString += "此学生未报名;";
                }
                else {
                    DataTable senrollPro = sEnrollsProfessionBLL.GetsEnrollsProfessionTable(studyLevel, enrollTab.Rows[0]["sEnrollID"].ToString(), cm.major, deptId);
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
                        classId = ClassBLL.GetClassID(senrollPro.Rows[0]["sProfessionID"].ToString(), cm.classId);
                        if (string.IsNullOrEmpty(classId))
                        {
                            errorString += "班级不存在;";
                        }
                        senrollProfessionId = senrollPro.Rows[0]["sEnrollsProfessionID"].ToString();
                    }

                }

            }
            return errorString;
        }

    }
}
