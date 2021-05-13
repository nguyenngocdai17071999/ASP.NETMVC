using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Framwork;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Code;
using System.Text;
using System.Text.RegularExpressions;
using PagedList;
using System.IO;
using HocWeb.Areas.Admin.Models;
using Models.Dao;

namespace HocWeb.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {

        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page=1,int pageSize=1000,string searchString=null)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(page, pageSize, searchString);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]

        public ActionResult ChangePassword()
        {         
            return View();
        }
        [HasCredential(RoleID = "VIEW_USER")]
        [HttpGet]
        public ActionResult Detail(long id)
        {
            var result = new UserDao().ViewDetail(id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Detail(User user)
        {
            if (ModelState.IsValid)
            {
                var session = (UserSession)Session[CommomConstants.USER_SESSION];
                var dao = new UserDao();
                if (!string.IsNullOrEmpty(user.Passwords))
                {
                    var encrytedMd5 = Encryptor.MD5Hash(user.Passwords);
                    user.Passwords = encrytedMd5;
                    user.ModifiedBy = session.TenTK;
                    user.ModifiedDate = DateTime.Now;
                    user.Position = session.ChucVu;
                }
                var result = dao.UpdateDetail(user);
                if (result)
                {
                    SetAlert("Chỉnh sửa thành công", "success");
                }
                else
                {
                    SetAlert("Cập nhật tài khoản không thành công", "error");
                }

            }
            return View(user);

        }
        [HttpGet]
        public ActionResult Profilee()
        {
            var session = (UserSession)Session[CommomConstants.USER_SESSION];
            var user = new UserDao().ViewDetail(session.UserID);
            return View(user);
        }
        [HttpPost]
        public ActionResult Profilee(User user)
        {
            if (ModelState.IsValid)
            {
                var session = (UserSession)Session[CommomConstants.USER_SESSION];
                var dao = new UserDao();
                if (!string.IsNullOrEmpty(user.Passwords))
                {
                    var encrytedMd5 = Encryptor.MD5Hash(user.Passwords);
                    user.Passwords = encrytedMd5;
                    user.ModifiedBy = session.TenTK;
                    user.ModifiedDate = DateTime.Now;
                    user.Position = session.ChucVu;
                }
                var result = dao.UpdateDetail(user);
                if (result)
                {
                    SetAlert("Chỉnh sửa thành công", "success");
                }
                else
                {
                    SetAlert("Cập nhật tài khoản không thành công", "error");
                }

            }
            return View(user);

        }

       public ActionResult Logout()
        {
            Session[CommomConstants.USER_SESSION] = null;
            return Redirect("/Admin/Login/Index");
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {

            return View();
        }
        
         [HttpGet]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(long id)
        {
            var user = new UserDao().ViewDetail(id);     
            return View(user);
        }
        [HttpPost]       
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user)
        {            
            if (ModelState.IsValid)
            {
                var session = (UserSession)Session[CommomConstants.USER_SESSION];
                var dao = new UserDao();
                if (dao.CheckUserName(user.UserName))
                {
                    SetAlert("Tên đăng nhập đã có người đăng kí", "error");
                }
                else if (dao.CheckUserEmail(user.Email))
                {
                    SetAlert("Tài khoản Email đã tồn tại", "error");
                }
                else
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(user.Passwords);
                    user.Passwords = encryptedMd5Pas;
                    user.CreatedDate = DateTime.Now;
                    user.CreatedBy = "Admin";
                    user.ModifiedBy = session.TenTK;
                   
                    long id = dao.Insert(user);
                    if (id > 0)
                    {
                        SetAlert("Thêm User thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Thêm User không thành công", "error");
                    }
                }
            }
            return View(user);
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user)
        {
           
            if (ModelState.IsValid)
            {
                var session = (UserSession)Session[CommomConstants.USER_SESSION];
                var dao = new UserDao();
                if (!string.IsNullOrEmpty(user.Passwords))
                {
                    var encrytedMd5 = Encryptor.MD5Hash(user.Passwords);
                    user.Passwords = encrytedMd5;
                    user.ModifiedBy = session.TenTK;
                    user.ModifiedDate = DateTime.Now;
                }               
                var result = dao.Update(user);
                    if (result)
                    {
                        SetAlert("Chỉnh sửa thành công", "success");
                        return RedirectToAction("Index","User");
                    }
                    else
                    {
                        SetAlert("Cập nhật tài khoản không thành công", "error");
                    }
                             
            }
            return View(user);
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            var oders = new OrderDao();
            bool result = oders.hasOrder(id);
            if(result==false)
            {
                SetAlert("Xoá thất bại !!!", "error");
                
            }
            else
            {
                new UserDao().Delete(id);
                SetAlert("Xóa thành công", "success");
            }         
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
  
        public ActionResult ChangePassword(Changepass model)
        {
            if (ModelState.IsValid)
            {
                var session = (UserSession)Session[CommomConstants.USER_SESSION];
                var dao = new UserDao();
             
                    var result = dao.ChangePass(session.UserID, Encryptor.MD5Hash(model.newPassswords));
                    if (result == 2 )
                    {
                        SetAlert("Đổi mật khẩu thành công", "success");
                        return RedirectToAction("Index","Default");
                    }
                    else if(result==1)
                    {                     
                        ModelState.AddModelError("", " Bạn phải nhập mật khẩu khác");
                    }                                         
           }
            return View(model);
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public ActionResult DashBoard()
        {
            var dao = new UserDao();
            var list = dao.GetAll();
            List<int> reparttitons = new List<int>();
            var posion = list.Select(x => x.Position).Distinct();
            foreach(var item in posion)
            {
                reparttitons.Add(list.Count(x => x.Position == item));
            }

            var rep = reparttitons;
            ViewBag.POSI = posion ;
            ViewBag.REP = reparttitons.ToList();
            return View();
        }
        
    }
}