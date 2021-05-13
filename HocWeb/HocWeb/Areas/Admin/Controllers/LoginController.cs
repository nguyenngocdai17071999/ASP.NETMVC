using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Models;
using HocWeb.Areas.Admin.Code;
using Models;
using System.Configuration;
using Models.Framwork;
using Models.Dao;
using Google.Apis.Logging;
using Facebook;

namespace HocWeb.Areas.Admin.Controllers
{
    
    public class LoginController : Controller
    {
        // GET: Admin/Login
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.TenTK, Encryptor.MD5Hash(model.MatKhau));
               
                if(result==2)
                {
                    var user = dao.GetByID(model.TenTK);
                    var userSession = new UserSession();
                    userSession.TenTK = user.UserName;
                    userSession.UserID = user.UserID;
                    userSession.Avartar = user.Avatar;
                    userSession.ChucVu = user.Position;
                    userSession.Name = user.FirstName + " " + user.LastName;
                    var listCredentials = dao.GetListCredential(model.TenTK);
                    Session.Add(CommomConstants.SESSION_CREDENTIAL, listCredentials);
                    Session.Add(CommomConstants.USER_SESSION,userSession);

                    if(user.Position=="1")
                    {
                        ModelState.AddModelError("", "Bạn không có quyền đăng nhập trang với tư cách này");
                    }
                    return RedirectToAction("Index", "Default");
                } else if(result ==0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }            
                else if(result==1)
                {
                    var user = dao.GetByID(model.TenTK);
                    var userSession = new UserSession();
                    userSession.TenTK = user.UserName;
                    userSession.UserID = user.UserID;
                    userSession.Avartar = user.Avatar;
                    userSession.ChucVu = user.Position;
                    userSession.Name = user.FirstName + " " + user.LastName;
                    Session.Add(CommomConstants.USER_SESSION, userSession);
                    if (user.Position == "2")
                    {
                        ModelState.AddModelError("", "Bạn không có quyền đăng nhập trang với tư cách này");
                    }
                    return RedirectToAction("Index", "TrangChu", new {area = "" });
                }               
            }
            return View("Index");        
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                reponse_type ="code",
                scope ="email",
            }) ;
            return Redirect(loginUrl.AbsoluteUri);

        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            if(!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string username = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.Email = email;
                user.UserName = username;
                user.Status = true;
                user.Position = "1";
                user.FirstName = firstname;
                user.LastName = middlename + " " + lastname;
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if(resultInsert>0)
                {                 
                    var userSession = new UserSession();
                    userSession.UserID = 42;
                    userSession.Name = user.FirstName + " " + user.LastName;
                    userSession.ChucVu = user.Position;
                    Session.Add(CommomConstants.USER_SESSION, userSession);
                } 
            }
            return RedirectToAction("Index", "TrangChu", new { area = "" });
        }


    }
}