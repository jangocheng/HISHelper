﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductReleaseSystem.Models.IRepository;
using ProductReleaseSystem.ProductRelease;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductReleaseSystem.Controllers
{
    public class DemandController : Controller
    {
        IDemand _demand;
        public DemandController(IDemand idemand)
        {
            _demand = idemand;
        }
        /// <summary>
        /// 需求提交页面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("demand")]
        
        public IActionResult Demand()
        {
            return View();
        }
        #region 实施添加需求
        /// <summary>
        /// 实施添加需求
        /// </summary>
        /// <param name="requestform"></param>
        /// <returns></returns>
        [HttpPost("adddemand")] 
        public IActionResult AddDemand(RequestForm requestform)
        {
           
            try
            {
                var list = _demand.InsertDemand(requestform);
                return new JsonResult(new { result = 1, message = $"添加成功！", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                // _log.Log(NLog.LogLevel.Error, $"查询全部部门：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"添加失败：{exc.Message}" });
            }
        }
        #endregion

        #region 部门添加
        /// <summary>
        /// 部门添加
        /// </summary>
        /// <returns></returns>
        [HttpPost("departments")]
        public IActionResult Departments()
        {
            try
            {
                var list = _demand.SelectDepartments();
                return new JsonResult(new { result = 1, message = $"查询开发人员成功！", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                // _log.Log(NLog.LogLevel.Error, $"查询全部部门：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"查询开发人员失败！：{exc.Message}" });
            }
        }
        #endregion
        #region 通过姓名用户查询ID
        /// <summary>
        /// 通过姓名用户查询ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("chaxunid")]
        public IActionResult InsertUser(string name)
        {
            try

            {
                var list = _demand.InsertUsers(name);
                return new JsonResult(new { result = 1, message = $"查询用户成功！", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                // _log.Log(NLog.LogLevel.Error, $"查询全部部门：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"查询用户失败！：{exc.Message}" });
            };
        }
        #endregion

        #region  上传图片
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="env">服务端内部数据对象</param>
        /// <returns></returns>
        [HttpPost("uploadimage")]
        public JsonResult UploadImg([FromServices]IHostingEnvironment env)
        {
            var files = HttpContext.Request.Form.Files;

            var list = new List<string>();
            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName);
                var imgPath = @"/upload/myimage/" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + new Random().Next(100) + ext;
                var stream = file.OpenReadStream();
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);


                var filestream = new FileStream(env.WebRootPath + imgPath, FileMode.CreateNew, FileAccess.ReadWrite);
                filestream.Write(bytes, 0, bytes.Length);
                filestream.Flush();
                filestream.Dispose();
                list.Add(imgPath);

            }
            return new JsonResult(list, new Newtonsoft.Json.JsonSerializerSettings());

        }
        #endregion
    }
}