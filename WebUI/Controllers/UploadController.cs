using BLL;
using Common;
using Model;
using Newtonsoft.Json.Linq;
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
    public class UploadController : BaseController
    {
        //
        // GET: /Upload/

        public ActionResult UploadInfo()
        {
            ViewBag.Title = "上传附件";
            return View();
        }

        public ActionResult GetUploadList()
        {
            string sort = Request.Form["txtSort"];
            string relatedId = Request.Form["RelatedID"];
            string where = "";
            if (!string.IsNullOrEmpty(sort))
            {
                where += " and Sort = " + sort + "";
            }
            if (!string.IsNullOrEmpty(relatedId))
            {
                where += " and RelatedID = " + relatedId + "";
            }
            string cmdText = @"SELECT  UploadID ,
        Name ,
        Path
FROM    T_Pro_Upload
WHERE   Status = 1 {0}";
            cmdText = string.Format(cmdText, where);
            return ResponseWriteResult(JsonGridData.GetGridJSON(cmdText, Request.Form));
        }

        //public AjaxResult SelectUploadInfo()
        //{
        //    string relatedId = Request.Form["RelatedID"];
        //    string sort = Request.Form["Sort"];
        //    string where = " AND Sort = @Sort AND RelatedID = @RelatedID AND Status=1";
        //    SqlParameter[] paras = new SqlParameter[] { 
        //        new SqlParameter("@Sort", sort),
        //        new SqlParameter("@RelatedID", relatedId)
        //    };
        //    DataTable dt = UploadBLL.UploadTableByWhere(where, paras, "");
        //    return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        //}

        //public AjaxResult ListUpload()
        //{
        //    string sort = Request.Form["Sort"];
        //    string relatedId = Request.Form["RelatedID"];
        //    string data = Request.Form["Data"];
        //    string where = " AND Sort = @Sort AND RelatedID = @RelatedID AND Status = 1";
        //    SqlParameter[] param = new SqlParameter[] { 
        //        new SqlParameter("@Sort", sort),
        //        new SqlParameter("@RelatedID", relatedId)
        //    };
        //    UploadBLL.UpdateUploadStatus(where, param);

        //    JObject json = JObject.Parse(data);
        //    int total = Convert.ToInt32(json["total"].ToString());
        //    JArray jar = JArray.Parse(json["rows"].ToString());
        //    UploadModel um = new UploadModel();
        //    for (int i = 0; i < total; i++)
        //    {
        //        JObject j = JObject.Parse(jar[i].ToString());
        //        um.Status = "1";
        //        um.Sort = sort;
        //        um.RelatedID = relatedId;
        //        um.Name = j["Name"].ToString();
        //        um.Path = j["Path"].ToString();
        //        um.CreateID = this.UserId.ToString();
        //        um.UpdateID = this.UserId.ToString();
        //        um.CreateTime = DateTime.Now.ToString();
        //        um.UpdateTime = DateTime.Now.ToString();
        //        UploadBLL.InsertUpload(um);
        //    }
        //    return AjaxResult.Success();
        //}

        public string UploadList()
        {
            //FileStream fileStream = null;
            //Stream stream = null;

            //接收上传后的文件
            HttpPostedFileBase file = Request.Files["Filedata"];
            //获取文件的保存路径
            string Path = Request["folder"] + "/";
            //string uploadPath = HttpContext.Current.Server.MapPath(@context.Request["folder"]) + "\\";
            string uploadPath = Server.MapPath(Path) + "\\";
            //判断上传的文件是否为空
            if (file != null)
            {
                Random R = new Random();//创建产生随机数
                int val = 100 + R.Next(899);//产生随机数为99以内任意
                string sj = val.ToString();//产生随机数
                string FileTime = DateTime.Now.ToString("yyyyMMddHHmmss") + sj;//得到系统时间(格式化)并加上随机数以便生成上传图片名称
                string filename = FileTime;//产生上传图片的名称
                string fileload = DateTime.Now.ToString("yyyyMM");//每月产生一个文件夹名
                string fileloaddate = DateTime.Now.ToString("dd");//每天一个文件夹
                //没有文件夹就创建
                if (!Directory.Exists(uploadPath + "/" + fileload + "/" + fileloaddate))
                {
                    Directory.CreateDirectory(uploadPath + "/" + fileload + "/" + fileloaddate);
                }
                string FileLastName = file.FileName.Substring(file.FileName.LastIndexOf("."));//后缀名
                //保存文件
                try
                {
                    string fileurl = uploadPath + "/" + fileload + "/" + fileloaddate + "/" + FileTime + FileLastName;
                    file.SaveAs(fileurl);
                    //fileStream = new FileStream(fileurl, FileMode.Open);
                    //stream = fileStream as Stream;
                    return Path + fileload + "/" + fileloaddate + "/" + FileTime + FileLastName;
                }
                catch
                {
                    return "";
                }
                finally
                {
                    //fileStream.Dispose();
                    //stream.Dispose();
                }
            }
            else
            {
                return "";
            }
        }
    }
}
