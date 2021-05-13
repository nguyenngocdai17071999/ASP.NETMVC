using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HocWeb.Areas.Admin.Models;
using Models.Dao;
using Models.Framwork;
using HocWeb.Areas.Admin.Code;
using System.Web.Mvc;

namespace HocWeb.Areas.Admin.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Admin/Register
    
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                var dao = new UserDao();
                if(dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if(dao.CheckUserEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var mahoa = Encryptor.MD5Hash(model.Passswords);
                    var user = new User();                

                    user.UserName = model.UserName;
                    user.Passwords = mahoa;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.CMND= model.CMND;
                    user.Phone = model.Phone;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.CreatedBy = "Normal";
                    user.Status = true;
                    user.Position = "1";
                    var result= dao.Insert(user);
                    if(result >0)
                    {
                        ViewBag.Success = "Đăng kí thành công ";
                        model = new RegisterModel();
                        
                    }                  
                    else
                    {
                        ModelState.AddModelError("", "Đăng kí không thành công");
                    }
                    
                }
            }
            return View(model);
        }
       
    }
}