﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using System.Collections;
using System.IO;

namespace ProductReleaseSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// 允许所有登录者
        /// </summary>
        /// <param name="returnUrl">如果用户访问的不是登录页，returnUrl将把这个url传进来，待登录成功后返回这个地址</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            //判断是否验证
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                //把返回地址保存在前台的hide表单中
                ViewBag.returnUrl = returnUrl;
            }
            ViewBag.error = null;
            return View();
        }

        /// <summary>
        /// 允许所有登录者
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="returnUrl">返回u</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(string username, string password, string returnUrl)
        {
            //从数据库验证用户，关取出用户所需要信息
            var users = new List<dynamic>() {
                new { ID = 1, UserName = "zsf",Password="111", Name = "张三丰", RoleTypeID = 1, RoleType = "admin", RoleTypeName = "管理员" },
                 new { ID = 2, UserName = "zwj",Password="222", Name = "张无忌", RoleTypeID = 2, RoleType = "user", RoleTypeName = "普通用户" }
            };
            var user = users.SingleOrDefault(u => u.UserName == username && u.Password == password);
            if (user != null)
            {
                //登录成功后，设置声明
                var claims = new Claim[] {
                      new Claim(ClaimTypes.UserData,username),
                      new Claim(ClaimTypes.Role,user.RoleType),
                      new Claim(ClaimTypes.Name,user.Name),
                      new Claim(ClaimTypes.Sid,user.ID.ToString())
                };
                HttpContext.Authentication.SignInAsync("loginvalidate", new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookie")));
                HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));
                return new RedirectResult(returnUrl == null ? "/" : returnUrl);
            }
            else
            {
                ViewBag.error = "用户名或密码错误！";
                return View();
            }
        }

        [HttpGet("send")]
        public IActionResult UpFile()
        {
            return View();
        }

        [HttpPost("sendfile")]
        public async Task<IActionResult> UpFile([FromServices] IHostingEnvironment env)
        {
            try
            {
                var file = HttpContext.Request.Form.Files[0];
                var filePath = env.WebRootPath;
                var fileName = file.FileName;
                var path = filePath + '\\' + fileName;
                if (!Directory.Exists(path))
                {
                    using (var fStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fStream);
                    }
                    return Ok(new { result = 1, message = "上传文件成功" });
                }
                else
                {
                    return new JsonResult(new { result=0,message="文件已存在"});
                }
            }catch(Exception exc)
            {
                return new JsonResult(new {result=0,message=exc.Message });
            }
        }
    }
}