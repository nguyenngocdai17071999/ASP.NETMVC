using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using HocWeb.Areas.Admin.Models;
using System.Web.Mvc;
using System.Configuration;
using HocWeb.Areas.Admin.Code;
using Models.Framwork;
using Commom;
using Models.Dao;


namespace HocWeb.Areas.Admin.Controllers
{
    public class ForgotPasswordController : Controller
    {
        // GET: Admin/ForgotPassword
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Doimatkhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ForgotPW model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();

                if (dao.CheckUserEmail(model.Email))
                {
                    var userSession = new UserSession();
                    var user = dao.GetByEmail(model.Email);
                    userSession.UserID = user.UserID;
                    Session.Add(CommomConstants.USER_SESSION, userSession);
                    string chuoi = GenerateRandomPassword(6);
                        user.CodeChangePass = chuoi;
                        dao.Update(user);
                        string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/sentmail.html"));
                        content = content.Replace("{{UserName}}", user.UserName);
                        content = content.Replace("{{Passwords}}", chuoi);

                        var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                        try
                        {
                            new MailHelper().SentMail(toEmail, "Yêu cầu đặt lại mật khẩu", content);
                            new MailHelper().SentMail(model.Email, "Yêu cầu đặt lại mật khẩu", content);
                        }
                        catch
                        { }
                       
                        return RedirectToAction("Doimatkhau", "ForgotPassword");                                     
                }
                else
                {
                    ModelState.AddModelError("", " Email chưa được đăng kí");
                }
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Doimatkhau(Doimatkhau model)
        {
            if (ModelState.IsValid)
            {             
                var dao = new UserDao();
            
                var session = (UserSession)Session[CommomConstants.USER_SESSION];
                var result = dao.doimatkhau(session.UserID, Encryptor.MD5Hash(model.NewPassswords),model.Code);
                if (result == 2)
                {                  
                    return RedirectToAction("Index", "Login");
                }
                else if (result == 1)
                {
                    ModelState.AddModelError("", " Bạn phải nhập mật khẩu khác");
                }
                else if (result == 3)
                {
                    ModelState.AddModelError("", " Code không chính xác");
                }               
            }
            return View();
        }
        private static string GenerateRandomPassword(int length)
        {
            string allowedLetterChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            string allowedNumberChars = "0123456789";
            char[] chars = new char[length];
            Random rd = new Random();
            bool useLetter = true;
            for (int i = 0; i < length; i++)
            {
                if (useLetter)
                {
                    chars[i] = allowedLetterChars[rd.Next(0, allowedLetterChars.Length)];
                    useLetter = false;
                }
                else
                {
                    chars[i] = allowedNumberChars[rd.Next(0, allowedNumberChars.Length)];
                    useLetter = true;
                }
            }
            return new string(chars);
        }
    }
     
}
