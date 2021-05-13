using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HocWeb.Models;
using PagedList;
using Commom;
using System.Configuration;
using HocWeb.Areas.Admin.Code;
using Models.Framwork;
using Models.Dao;
using System.Web.Script.Serialization;
using Models.ViewModel;

namespace HocWeb.Controllers
{
    public class ThongTinShopController : Controller
    {
        // GET: ThongTinShop
        public ActionResult MuaTraGop()
        {
            return View();
        }
        public ActionResult Index()
        {
            var model = new ContactDao().GetActiveContact();
            return View(model);
        }

        [HttpPost]
        public JsonResult Send(string name, string mobile, string address, string email, string content)
        {
            var feedback = new Feedback();
            feedback.Name = name;
            feedback.Email = email;
            feedback.CreatedDate = DateTime.Now;
            feedback.Phone = mobile;
            feedback.Content = content;
            feedback.Address = address;
            feedback.Status = true;
            var id = new ContactDao().InsertFeedBack(feedback);
            string content1 = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/vendor/SentFeedBack.html"));
            content1 = content1.Replace("{{Name}}", name);
            content1 = content1.Replace("{{Address}}", address);
            content1 = content1.Replace("{{Email}}", email);
            content1 = content1.Replace("{{Detail}}", content);

            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            try
            {
                new MailHelper().SentMail(toEmail, "Thông tin Feed back", content1);
                new MailHelper().SentMail(feedback.Email, "Thông tin Feed back", content1);
            }
            catch { }
          
            if (id > 0)
            {
              
                return Json(new
                {
                    status = true,
                   

            });
                //sent
                
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }

        }

     
    }
}