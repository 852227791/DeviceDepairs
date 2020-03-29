using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common;
using Model;


namespace WebUI.Controllers
{
    public class DetailController : BaseController
    {
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailList()
        {
            return View();
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailEdit()
        {
            return View();
        }

        /// <summary>
        /// 得到收费类别树的数据
        /// </summary>
        /// <returns></returns>
        public string GetDetailTree()
        {
            string menuId = Request.Form["MenuID"];
            string status = Request.Form["Status"];
            string cmdText = @"SELECT  d.DetailID id ,
        d.ParentID ,
        CASE d.Status
          WHEN 1 THEN d.EnglishName + ' ' + d.Name
          WHEN 2
          THEN '<span style=color:#ff0000>' + d.EnglishName + ' ' + d.Name
               + '(停用)' + '</span>'
        END text ,
        ds.SubjectID ,
        s.Name SubName ,
        d.EnglishName ,
        ISNULL(d.Remark, '') Remark,
        d.Name 
FROM    T_Pro_Detail d
        LEFT JOIN T_Pro_DetailSubject ds ON ds.DetailID = d.DetailID
        LEFT JOIN T_Pro_Subject s ON s.SubjectID = ds.SubjectID
WHERE   d.Status IN ( {0} )
ORDER BY d.EnglishName ASC";
            cmdText = String.Format(cmdText, status);
            return JsonMenuTreeData.GetArrayJSON(cmdText, "id", "ParentID", CommandType.Text);
        }

        /// <summary>
        /// 查看费用类别
        /// </summary>
        /// <returns></returns>
        public AjaxResult SelectViewDetail()
        {
            string detailID = Request.Form["ID"];
            string cmdText = @"SELECT  detail1.Name ,
        detail1.EnglishName ,
        detail2.Name AS ParentName ,
        detail1.Remark
FROM    T_Pro_Detail AS detail1
        LEFT JOIN T_Pro_Detail AS detail2 ON detail1.ParentID = detail2.DetailID
WHERE   detail1.DetailID = {0}";
            cmdText = string.Format(cmdText, detailID);
            return AjaxResult.Success(JsonData.GetArray(cmdText));
        }


        /// <summary>
        /// 得到费用类别
        /// </summary>
        /// <returns></returns>
        public AjaxResult SelectDetail()
        {
            string detailID = Request.Form["ID"];
            string where = " AND DetailID = @DetailID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DetailID", detailID)
            };
            DataTable dt = DetailBLL.DetailTableByWhere(where, paras, "");
            return AjaxResult.Success(JsonHelper.DataTableToJson(dt));
        }

        /// <summary>
        /// 验证费用类别是否重复
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckDetailName()
        {
            string name = Request.Form["Value"];
            string detailId = Request.Form["ID"];

            if (CheckDetailNameMethod(detailId, name))
            {
                return AjaxResult.Error("该费用类别已经存在，请重新输入");
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        /// <summary>
        /// 验证费用类别是否重复的方法
        /// </summary>
        /// <param name="DetailID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool CheckDetailNameMethod(string DetailID, string Name)
        {
            string where = " AND DetailID <> @DetailID AND Name = @Name";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DetailID", DetailID),
                new SqlParameter("@Name", Name)
            };
            DataTable dt = DetailBLL.DetailTableByWhere(where, paras, "");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证费用类别ID是否与父级ID相同
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckDetailIDIsParentID()
        {
            string detailId = Request.Form["ID"];
            string parentId = Request.Form["TypeId"];

            if (CheckDetailIDIsParentIDMethod(detailId, parentId))
            {
                return AjaxResult.Error("不能选择自己或自己的下级作为上级类别");
            }
            else
            {
                return AjaxResult.Success();
            }
        }

        /// <summary>
        /// 验证费用类别ID是否与父级ID相同的方法
        /// </summary>
        /// <param name="DetailID"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public bool CheckDetailIDIsParentIDMethod(string DetailID, string ParentID)
        {
            string DetailIDStr = ",";
            DataTable dt = DetailBLL.SelectChildrenDetailID(DetailID);
            foreach (DataRow dr in dt.Rows)
            {
                DetailIDStr += dr["DetailID"].ToString() + ",";
            }

            if (DetailIDStr.IndexOf("," + ParentID + ",") > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 保存收费类别
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public string GetDetailEdit(DetailFormModel dm)
        {
            if (string.IsNullOrEmpty(dm.DetailID))
            {
                dm.DetailID = "0";
            }
            if (dm.Name.Length < 1 || dm.Name.Length > 32)
            {
                return "收费类别名称不能为空，并且长度为1-32个字符！";
            }
            if (CheckDetailNameMethod(dm.DetailID, dm.Name))
            {
                return "该收费类别已经存在，请重新输入";
            }
            if (string.IsNullOrEmpty(dm.ParentID))
            {
                return "请选择上级类别！";
            }
            if (CheckDetailIDIsParentIDMethod(dm.DetailID, dm.ParentID))
            {
                return "不能选择自己或自己的下级作为上级类别";
            }
            if (dm.EnglishName.Length < 1 || dm.EnglishName.Length > 32)
            {
                return "英文名称不能为空，并且长度为1-4个字符！";
            }
            if (!string.IsNullOrEmpty(dm.Remark))
            {
                dm.Remark = dm.Remark.Replace("\r\n", "<br />");
            }

            string result = "no";
            if (dm.DetailID != "0")
            {
                string where = " AND DetailID=@DetailID";
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@DetailID", dm.DetailID)
                };
                DetailModel editdm = DetailBLL.DetailModelByWhere(where, paras);

                LogBLL.CreateLog("T_Pro_Detail", this.UserId.ToString(), editdm, dm);//写日志
                editdm.Remark = dm.Remark;
                editdm.Name = dm.Name;
                editdm.ParentID = dm.ParentID;
                editdm.EnglishName = dm.EnglishName;
                editdm.UpdateID = this.UserId.ToString();
                editdm.UpdateTime = DateTime.Now.ToString();
                if (DetailBLL.UpdateDetail(editdm) > 0)
                {
                    string where1 = " and DetailID=@DetailID";
                    SqlParameter[] paras1 = new SqlParameter[] { new SqlParameter("@DetailID", editdm.DetailID) };
                    DetailSubjectModel dlsu = DetailSubjectBLL.DetailSubjectModelByWhere(where1, paras1);
                    if (dlsu.SubjectID != dm.SubjectID)
                    {
                        DetailSubjectBLL.DeleteDetailSubject(editdm.DetailID);
                        DetailSubjectModel dsm = new DetailSubjectModel();
                        dsm.SubjectID = dm.SubjectID;
                        dsm.DetailID = dm.DetailID;

                        if (DetailSubjectBLL.InsertDetailSubject(dsm) > 0)
                        {
                            result = "yes";
                        }

                    }
                    else
                    {
                        result = "yes";
                    }
                }
            }
            else
            {
                dm.Status = "1";
                dm.CreateID = this.UserId.ToString();
                dm.CreateTime = DateTime.Now.ToString();
                dm.UpdateID = this.UserId.ToString();
                dm.UpdateTime = DateTime.Now.ToString();
                dm.DetailID = DetailBLL.InsertDetail(dm).ToString();
                DetailSubjectModel dsm = new DetailSubjectModel();
                dsm.SubjectID = dm.SubjectID;
                dsm.DetailID = dm.DetailID;
                if (DetailSubjectBLL.InsertDetailSubject(dsm) > 0)
                {
                    result = "yes";
                }

            }
            return result;
        }

        /// <summary>
        /// 启用、停用判断父、子节点状态
        /// </summary>
        /// <returns></returns>
        public AjaxResult CheckIsHandle()
        {
            string detailId = Request.Form["DetailID"];
            string status = Request.Form["Status"];
            if (status == "2")
            {
                string where = " AND Status = 1 AND ParentID = @DetailID  ";
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@DetailID", detailId)
                };
                DataTable dt = DetailBLL.DetailTableByWhere(where, paras, "");
                if (dt.Rows.Count > 0)
                {
                    return AjaxResult.Error("请先停用所有子类别");
                }
                else
                {
                    return AjaxResult.Success();
                }
            }
            else if (status == "1")
            {
                string where = " AND Status = 2 AND DetailID = (Select ParentID from T_Pro_Detail WHERE DetailID = @DetailID)";
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@DetailID", detailId)
                };
                DataTable dt = DetailBLL.DetailTableByWhere(where, paras, "");
                if (dt.Rows.Count > 0)
                {
                    return AjaxResult.Error("请先启用父类别");
                }
                else
                {
                    return AjaxResult.Success();
                }
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetUpdateDetailStatus()
        {
            string detailId = Request.Form["ID"];
            string status = Request.Form["Value"];

            string where = " AND DetailID = @DetailID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@DetailID", detailId)
            };
            DetailModel dm = DetailBLL.DetailModelByWhere(where, paras);
            dm.Status = status;
            if (DetailBLL.UpdateDetail(dm) > 0)
            {
                return AjaxResult.Success();
            }
            else
            {
                return AjaxResult.Error("发现系统错误");
            }
        }

        /// <summary>
        /// 获取下拉菜单
        /// </summary>
        /// <returns></returns>
        public string GetDetailCommonbox()
        {
            return DetailBLL.GetDetailCommbox();
        }
    }
}
