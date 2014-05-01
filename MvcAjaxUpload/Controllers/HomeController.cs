using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAjaxUpload.Models;

namespace MvcAjaxUpload.Controllers
{
    public class HomeController : Controller
    {
        #region 上传单个文件

        //显示
        public ActionResult Index()
        {
            return View();
        }

        //接收上传
        [HttpPost]
        public ActionResult UploadFile()
        {
            List<UploadFileResult> results = new List<UploadFileResult>();
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0 || hpf == null)
                {
                    continue;
                }

                var fileName = DateTime.Now.ToString("yyyyMMddhhmmss") +
                               hpf.FileName.Substring(hpf.FileName.LastIndexOf('.'));
                string pathForSaving = Server.MapPath("~/AjaxUpload");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    hpf.SaveAs(Path.Combine(pathForSaving, fileName));
                    results.Add(new UploadFileResult()
                    {
                        FilePath = Url.Content(String.Format("~/AjaxUpload/{0}", fileName)),
                        FileName = fileName,
                        IsValid = true,
                        Length = hpf.ContentLength,
                        Message = "上传成功",
                        Type = hpf.ContentType
                    });
                }
            }

            return Json(new
            {
                name = results[0].FileName,
                type = results[0].Type,
                size = string.Format("{0} bytes", results[0].Length),
                path = results[0].FilePath,
                msg = results[0].Message
            });
        }

        #endregion

        #region 上传多个文件
        public ActionResult ShowMultiple()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UplpadMultipleFiles()
        {
            List<UploadFileResult> results = new List<UploadFileResult>();
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0 || hpf == null)
                {
                    continue;
                }

                //var fileName = DateTime.Now.ToString("yyyyMMddhhmmss") +
                //               hpf.FileName.Substring(hpf.FileName.LastIndexOf('.'));
                var fileName = Guid.NewGuid().ToString() +
                               hpf.FileName.Substring(hpf.FileName.LastIndexOf('.'));
                string pathForSaving = Server.MapPath("~/AjaxUpload");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    hpf.SaveAs(Path.Combine(pathForSaving, fileName));
                    results.Add(new UploadFileResult()
                    {
                        FilePath = Url.Content(String.Format("~/AjaxUpload/{0}", fileName)),
                        FileName = fileName,
                        IsValid = true,
                        Length = hpf.ContentLength,
                        Message = "上传成功",
                        Type = hpf.ContentType
                    });
                }
            }

            return Json(new
            {
                name = results[0].FileName,
                type = results[0].Type,
                size = string.Format("{0} bytes", results[0].Length),
                path = results[0].FilePath,
                msg = results[0].Message
            });

        }
        #endregion

        #region 共用方法
        /// <summary>
        /// 检查是否要创建上传文件夹，如果没有就创建
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    //TODO：处理异常
                    result = false;
                }
            }
            return result;
        }

        //根据文件名称删除文件
        [HttpPost]
        public ActionResult DeleteFileByName(string name)
        {
            string pathForSaving = Server.MapPath("~/AjaxUpload");
            System.IO.File.Delete(Path.Combine(pathForSaving, name));
            return Json(new
            {
                msg = true
            });
        }
        #endregion
    }
}
