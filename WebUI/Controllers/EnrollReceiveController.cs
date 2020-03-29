using BLL;
using Common;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class EnrollReceiveController : Controller
    {
        //
        // GET: /EnrollReceive/
        [HttpPost]
        public void Post()
        {
            Stream postData = Request.InputStream;
            string message = string.Empty;

            string data = string.Empty;
            try
            {
                data = OtherHelper.RequestStream(postData);
            }
            catch (Exception)
            {

                message += "数据错误";
            }

            EnrollApiModel eam = new EnrollApiModel();
            string dcpDeptId = "";
            if (string.IsNullOrEmpty(message))
            {


                try
                {
                    #region
                    eam = JsonConvert.DeserializeObject<EnrollApiModel>(data);
                    if (string.IsNullOrEmpty(eam.idCard))
                    {
                        message += "身份证号不能为空;";
                    }
                    if (string.IsNullOrEmpty(eam.major))
                    {
                        message += "专业不能为空;";
                    }
                    if (string.IsNullOrEmpty(eam.deptId))
                    {
                        message += "主体不能为空;";
                    }
                    else
                    {
                        dcpDeptId = eam.deptId;
                        eam.deptId = DeptDCPBLL.GetDeptID(eam.deptId).ToString();
                        if (string.IsNullOrEmpty(eam.deptId))
                        {
                            message += "系统内没有找到对应的主体;";
                        }
                    }
                    string temp = dcpDeptId + eam.idCard + eam.major + OtherHelper.GetAppSettingsValue("Key");
                    string key = Encryption.MD5Encrypt(temp).ToLower();
                    if (!key.Equals(eam.key))
                    {
                        message += "签名错误;";
                    }
                    #endregion
                    if (string.IsNullOrEmpty(message))
                    {
                        StudentModel sm = StudentBLL.GetStudentModel(eam.idCard);//获取学生信息
                        if (string.IsNullOrEmpty(sm.StudentID))
                        {
                            message += "学生不存在;";
                        }
                        else
                        {
                            DataTable enrollTab = sEnrollBLL.GetEnrollTable(sm.StudentID);//获取学生报名信息
                            if (enrollTab.Rows.Count == 1)
                            {
                                DataTable senrollsProfessionTab = sEnrollsProfessionBLL.GetsEnrollProfessionTable(enrollTab.Rows[0]["sEnrollID"].ToString());//获取报名专业信息
                                sEnrollModel em = sEnrollBLL.GetEnrollModel(enrollTab.Rows[0]["sEnrollID"].ToString());
                                if (senrollsProfessionTab.Rows.Count == 1)
                                {
                                    if (senrollsProfessionTab.Rows[0]["Status"].ToString().Equals("1"))//报名状态为录取更新报名时间
                                    {
                                        if (string.IsNullOrEmpty(enrollTab.Rows[0]["EnrollNum"].ToString()))
                                        {
                                            em.EnrollNum = sEnrollBLL.getEnrollNum(senrollsProfessionTab.Rows[0]["DeptID"].ToString(), senrollsProfessionTab.Rows[0]["Year"].ToString(), "0");
                                            em.UpdateID = "0";
                                            em.UpdateTime = DateTime.Now.ToString();
                                            sEnrollBLL.UpdatesEnroll(em);//更新报名
                                        }
                                        sEnrollsProfessionBLL.UpdatesEnrollTime(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString());//更新报名时间
                                    }

                                    DataTable feeOrder = sOrderBLL.GetsOrderTable(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString(), true);//获取已缴费的订单
                                    #region  未交费
                                    string tempsEnrollProfessionId = string.Empty;
                                    if (feeOrder.Rows.Count == 0)
                                    {

                                        string sprofessionId = sProfessionBLL.GetsProfesssionID(eam.major, senrollsProfessionTab.Rows[0]["DeptID"].ToString(), senrollsProfessionTab.Rows[0]["Year"].ToString(), senrollsProfessionTab.Rows[0]["Month"].ToString());
                                        if (string.IsNullOrEmpty(sprofessionId))
                                        {
                                            message += "报名专业不存在;";
                                        }
                                        else
                                        {

                                            if (sprofessionId.Equals(senrollsProfessionTab.Rows[0]["sProfessionID"].ToString()))//专业相同
                                            {
                                                DataTable nofeeOrder = sOrderBLL.GetsOrderTable(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString(), false);//获取未缴费的订单
                                                if (nofeeOrder.Rows.Count == 0)
                                                {
                                                    sOrderBLL.BuildsOrder(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString(), "0");
                                                }
                                                sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString());
                                                epm.AuditView = "报名审查";
                                                epm.AuditTime = DateTime.Now.ToString();
                                                epm.Auditor = "0";
                                                sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                                                tempsEnrollProfessionId = senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString();
                                            }
                                            else
                                            {
                                                sEnrollsProfessionModel epm = new sEnrollsProfessionModel();
                                                epm.sEnrollID = senrollsProfessionTab.Rows[0]["sEnrollID"].ToString();
                                                epm.DeptID = senrollsProfessionTab.Rows[0]["DeptID"].ToString();
                                                epm.EnrollTime = senrollsProfessionTab.Rows[0]["EnrollTime"].ToString();
                                                epm.Year = senrollsProfessionTab.Rows[0]["Year"].ToString();
                                                epm.Month = senrollsProfessionTab.Rows[0]["Month"].ToString();
                                                epm.EnrollLevel = senrollsProfessionTab.Rows[0]["EnrollLevel"].ToString();
                                                epm.sProfessionID = sprofessionId;
                                                DataTable itemProfessionTab = sItemsProfessionBLL.GetItemsProfessionTable(sprofessionId);
                                                string MajorScheme = "";
                                                if (itemProfessionTab.Rows.Count == 1)
                                                {
                                                    MajorScheme = "{\"total\":1,\"rows\":[{\"ItemID\":\"" + itemProfessionTab.Rows[0]["ItemID"].ToString() + "\"}]}";
                                                }
                                                else
                                                {
                                                    MajorScheme = "{\"total\":1,\"rows\":[]}";
                                                }
                                                string[] array = { "0" };
                                                string tempId = sEnrollBLL.ChangesEnrollProfession(epm, "0", MajorScheme, senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString(), DateTime.Now.ToString(), "报名时要求转专业", array);
                                                sEnrollsProfessionBLL.UpdatesEnrollTime(tempId);//更新报名时间
                                                if (itemProfessionTab.Rows.Count == 1)
                                                {
                                                    sOrderBLL.BuildsOrder(tempId, "0");
                                                }
                                                tempsEnrollProfessionId = tempId;
                                            }
                                        }
                                    }
                                    #endregion
                                    #region 已缴费
                                    else
                                    {
                                        if (senrollsProfessionTab.Rows[0]["Status"].ToString().Equals("1"))//报名状态为录取更新报名时间
                                        {
                                            sEnrollsProfessionModel epm = sEnrollsProfessionBLL.GetsEnrollsProfessionModel(senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString());
                                            epm.AuditView = "报名审查";
                                            epm.AuditTime = DateTime.Now.ToString();
                                            epm.Auditor = "0";
                                            sEnrollsProfessionBLL.UpdatesEnrollsProfession(epm);
                                        }
                                        tempsEnrollProfessionId = senrollsProfessionTab.Rows[0]["sEnrollsProfessionID"].ToString();
                                        message += "已缴费;";
                                    }
                                    #endregion
                                    sEnrollsProfessionBLL.UpdatesEnrollsProfessionStatus(tempsEnrollProfessionId, "0");
                                }
                                else if (senrollsProfessionTab.Rows.Count == 0)
                                {
                                    message += "该学生报名专业为空;";
                                }
                                else
                                {
                                    message += "该学生报多个专业;";
                                }
                            }
                            else if (enrollTab.Rows.Count > 1)//多次报名不处理
                            {
                                message += "该学生已经报过名;";
                            }
                            else
                            {//没有报名信息  不处理
                                message += "没有此学生的报名信息;";
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    message += ex.Message;
                }

                if (!string.IsNullOrEmpty(message))
                {
                    message = "【"+eam.idCard+"】"+ message+ "\n";
                    OtherHelper.CreateFile(message, OtherHelper.CreateTxtFileName());
                }

            }

            if (!string.IsNullOrEmpty(eam.returnUrl))
            {
                OtherHelper.HostingReturn(eam.returnUrl, message);
            }

        }

    }
}
