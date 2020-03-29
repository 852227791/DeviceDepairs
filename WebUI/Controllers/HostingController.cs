using BLL;
using Common;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class HostingController : Controller
    {
        //
        // GET: /Hosting/
        [HttpPost]
        public void Post()
        {
            ErrorCodeModel ecm = new ErrorCodeModel();
            Stream postData = Request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            EnrollPostModel epm = new EnrollPostModel();
            string data = OtherHelper.DecodeBase64("UTF-8", postContent);

            try
            {
                epm = JsonConvert.DeserializeObject<EnrollPostModel>(data);
                long decDeptId = epm.enrollData._deptiId;
                epm.enrollData._deptiId = DeptDCPBLL.GetDeptID(epm.enrollData._deptiId.ToString());
                ecm.code = validate(epm, decDeptId);
                if (ecm.code.Count == 0)
                {
                    var m = epm.enrollData;
                    StudentModel sm = StudentBLL.GetStudentModel(m._idCard);
                    if (string.IsNullOrEmpty(sm.StudentID))
                    {
                        sm.Address = m._address;
                        sm.CreateID = "0";
                        sm.CreateTime = DateTime.Now.ToString();
                        sm.UpdateID = "0";
                        sm.UpdateTime = DateTime.Now.ToString();
                        sm.WeChat = m._wechat;
                        sm.DeptID = m._deptiId.ToString();
                        sm.IDCard = m._idCard;
                        sm.Mobile = m._mobile;
                        sm.Name = m._name;
                        sm.QQ = m._qq;
                        sm.Remark = "";
                        sm.Sex = m._sex.ToString();
                        sm.Status = "1";
                        sm.StudentID = StudentBLL.InsertStudent(sm).ToString();//添加学生
                    }
                    //添加详细信息

                    StudentInfoModel sim = StudentInfoBLL.GetStudentInfoModel(sm.StudentID);
                    if (string.IsNullOrEmpty(sim.StudentInfoID))
                    {
                        sim.CreateID = "0";
                        sim.CreateTime = DateTime.Now.ToString();
                        sim.Status = "1";
                        sim.ProvinceID = AreaBLL.GetAreaModel(m._province).AreaID;
                        sim.CityID = AreaBLL.GetAreaModel(m._city).AreaID;
                        sim.Nation = RefeBLL.GetRefeValue(m._province, "24");
                        sim.School = m._schoolName;
                        sim.Zip = m._zip;
                        sim.UpdateID = "0";
                        sim.UpdateTime = DateTime.Now.ToString();
                        StudentInfoBLL.InsertStudentInfo(sim);
                    }
                    if (!string.IsNullOrEmpty(m._parentMobile) || !string.IsNullOrEmpty(m._parentName))
                    {
                        StudentContactModel scm = StudentContactBLL.GetStudentContactModel(sm.StudentID);
                        if (string.IsNullOrEmpty(scm.StudentContactID))
                        {
                            scm.Status = "1";
                            scm.StudentID = sm.StudentID;
                            scm.CreateID = "0";
                            scm.CreateTime = DateTime.Now.ToString();
                            scm.Name = m._parentName;
                            scm.Tel = m._parentMobile;
                            scm.UpdateID = "0";
                            scm.UpdateTime = DateTime.Now.ToString();
                            StudentContactBLL.InsertStudentContact(scm);
                        }

                    }
                    //添加联系人

                    //添加报名
                    sEnrollModel em = new sEnrollModel();
                    em.CreateID = "0";
                    em.CreateTime = DateTime.Now.ToString();
                    em.UpdateID = "0";
                    em.UpdateTime = DateTime.Now.ToString();
                    em.DeptID = m._deptiId.ToString();
                    em.EnrollNum = sEnrollBLL.getEnrollNum(m._deptiId.ToString(), m._year.ToString(), "0"); ;
                    em.Status = "1";
                    em.StudentID = sm.StudentID;
                    em.sEnrollID = sEnrollBLL.InsertsEnroll(em).ToString();
                    //添加报名专业
                    sEnrollsProfessionModel sepm = new sEnrollsProfessionModel();
                    sepm.CreateID = "0";
                    sepm.CreateTime = DateTime.Now.ToString();
                    sepm.DeptAreaID = DeptAreaBLL.GetFirstDeptArea(m._deptiId.ToString());
                    sepm.DeptID = m._deptiId.ToString();
                    sepm.EnrollLevel = m._studyLevel.ToString();
                    sepm.Month = m._month.ToString();
                    sepm.Year = m._year.ToString();
                    sepm.Remark = "";
                    sepm.Status = "3";
                    sepm.EnrollTime = DateTime.Now.ToString();
                    sepm.Auditor = "0";
                    sepm.AuditTime = DateTime.Now.ToString();
                    sepm.AuditView = "";
                    sepm.sProfessionID = sProfessionBLL.GetsProfesssionID(m); ;
                    sepm.sEnrollID = em.sEnrollID;
                    sepm.UpdateID = "0";
                    sepm.UpdateTime = DateTime.Now.ToString();
                    sepm.sEnrollsProfessionID = sEnrollsProfessionBLL.InsertsEnrollsProfession(sepm).ToString();//添加报名专业
                    DataTable scheme = sItemsProfessionBLL.GetItemsProfession(m._year.ToString(), m._month.ToString(), sepm.sProfessionID,"");
                    if (scheme.Rows.Count == 1)
                    {
                        sItemsEnrollModel iem = new sItemsEnrollModel();
                        iem.ItemID = scheme.Rows[0]["ItemID"].ToString();
                        iem.sEnrollsProfessionID = sepm.sEnrollsProfessionID;
                        sItemsEnrollBLL.InsertsItemsEnroll(iem);//添加方案
                        sOrderBLL.BuildsOrder(sepm.sEnrollsProfessionID, "0");//生成缴费单
                    }
                }

            }
            catch (Exception)
            {
                ecm.code.Add("0x0026");
            }

            OtherHelper.HostingReturn(epm.bankUrl, OtherHelper.JsonSerializer(ecm));
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="epm"></param>
        /// <returns></returns>
        private List<string> validate(EnrollPostModel epm, long dcpDeptId)
        {
            List<string> lsit = new List<string>();
            if (string.IsNullOrEmpty(epm.bankUrl))
            {
                lsit.Add("0x0001");
            }
            if (string.IsNullOrEmpty(epm.key))
            {
                lsit.Add("0x0002");
            }
            if (epm != null)
            {
                var m = epm.enrollData;
                string temp = m._examNum + m._name + m._idCard + m._sex.ToString() + m._mobile + m._qq + m._wechat +
                m._address + m._nation + m._province + m._city + m._zip + m._schoolName + m._parentName + m._parentMobile + m._studyLevel.ToString()
                + m._major + dcpDeptId.ToString() + m._year.ToString() + m._month.ToString();
                string key = Encryption.MD5Encrypt(temp + "524d9ecd75db17039b22a063274c9dbc").ToLower();
                if (!key.Equals(epm.key))
                {
                    lsit.Add("0x0004");
                }
                if (string.IsNullOrEmpty(m._name))
                {
                    lsit.Add("0x0005");
                }
                else
                {
                    if (m._name.Length > 16)
                    {
                        lsit.Add("0x0006");
                    }
                }
                if (!OtherHelper.CheckIDCard(m._idCard))
                {
                    lsit.Add("0x0007");
                }
                else
                {
                    StudentModel sm = StudentBLL.GetStudentModel(m._idCard);
                    if (!string.IsNullOrEmpty(sm.Name))
                    {
                        if (!sm.Name.Equals(m._name))
                        {
                            lsit.Add("0x0008");
                        }
                        else
                        {
                            DataTable enrollTable = sEnrollBLL.GetStudentEnroll(sm.StudentID);
                            if (enrollTable.Rows.Count > 0)
                            {
                                lsit.Add("0x0009");
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(m._examNum))
                {
                    if (m._examNum.Length > 16)
                    {
                        lsit.Add("0x0010");
                    }

                }
                if (!m._sex.Equals(1) && !m._sex.Equals(2))
                {
                    lsit.Add("0x0011");
                }

                if (string.IsNullOrEmpty(m._mobile))
                {
                    lsit.Add("0x0012");
                }
                else
                {
                    if (m._mobile.Length > 16)
                    {
                        lsit.Add("0x0013");
                    }
                }
                string nationId = RefeBLL.GetRefeValue(m._nation, "24");
                if (nationId.Equals("-1"))
                {
                    lsit.Add("0x0014");

                }

                if (string.IsNullOrEmpty(m._city) || string.IsNullOrEmpty(m._province))
                {
                    if (string.IsNullOrEmpty(m._province))
                    {
                        lsit.Add("0x0015");
                    }
                    if (string.IsNullOrEmpty(m._city))
                    {
                        lsit.Add("0x0016");
                    }
                }
                else
                {
                    AreaModel province = AreaBLL.GetAreaModel(m._province);
                    AreaModel city = AreaBLL.GetAreaModel(m._city);
                    if (string.IsNullOrEmpty(province.AreaID))
                    {
                        lsit.Add("0x0017");
                    }
                    if (string.IsNullOrEmpty(city.AreaID))
                    {
                        lsit.Add("0x0018");
                    }
                    if (!string.IsNullOrEmpty(province.AreaID) && !string.IsNullOrEmpty(city.AreaID))
                    {
                        if (!city.ParentID.Equals(province.AreaID))
                        {
                            lsit.Add("0x0019");
                        }
                    }
                }
                if (!string.IsNullOrEmpty(m._zip))
                {
                    if (m._zip.Length > 8)
                    {
                        lsit.Add("0x0020");
                    }
                }
                if (!string.IsNullOrEmpty(m._schoolName))
                {
                    if (m._schoolName.Length > 64)
                    {
                        lsit.Add("0x0021");
                    }
                }
                if (!string.IsNullOrEmpty(m._parentMobile))
                {
                    if (m._parentMobile.Length > 32)
                    {
                        lsit.Add("0x0022");
                    }
                }
                if (!string.IsNullOrEmpty(m._parentName))
                {
                    if (m._parentName.Length > 32)
                    {
                        lsit.Add("0x0023");
                    }
                }
                if (!m._studyLevel.Equals(1) && !m._studyLevel.Equals(2) && !m._studyLevel.Equals(3))
                {
                    lsit.Add("0x0024");
                }
                string sprfessionId = sProfessionBLL.GetsProfesssionID(m);
                if (string.IsNullOrEmpty(sprfessionId))
                {
                    lsit.Add("0x0025");
                }
                if (m._deptiId.Equals(0))
                {
                    lsit.Add("0x0028");
                }
            }
            else
            {
                lsit.Add("0x0003");
            }


            return lsit;
        }



    }
}
