using System;
using System.Collections.Generic;
using System.Linq;
using HocWeb.Areas.Admin.Models;
using HocWeb.Areas.Admin.Code;
using Models.Framwork;
using Models.Dao;
using System.Web;
using System.Web.Mvc;


namespace HocWeb.Areas.Admin.Controllers
{
    public class CateProductController : BaseController
    {
        // GET: Admin/CateProduct
        public ActionResult Index(int page = 1, int pageSize = 1000, string searchString = null)
        {
            var dao = new CateProductDao();
            var model = dao.ListAllPaging(page, pageSize, searchString);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(long ID)
        {
            var category = new CateProductDao().ViewDetail(ID);
            return View(category);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new CateProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        [HttpPost]
        public JsonResult ChangeShowOnHome(long id)
        {
            var result = new CateProductDao().ChangeShowOnHome(id);
            return Json(new
            {
                ShowOnHome = result
            });
        }
        [HttpPost]
        public ActionResult Create(ProductCategory model)
        {
            var dao = new CateProductDao();
            var result = dao.CheckProduct(model.Name);
          
            if (ModelState.IsValid)
            {
                if(result==false)
                {
                    var session = (UserSession)Session[CommomConstants.USER_SESSION];
                    model.CreatedDate = DateTime.Now;
                    model.ModifiedDate = DateTime.Now;
                    model.ModifiedBy = session.TenTK;
                    model.CreatedBy = session.TenTK;
                    var id = new CateProductDao().Insert(model);
                    if (id > 0)
                    {
                        SetAlert("Thêm danh mục thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Thêm danh mục thất bại", "error");
                    }
                }             
                else
                {
                    SetAlert("Tên danh mục đã tồn tại", "error");
                }
               
                
            }
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                var session = (UserSession)Session[CommomConstants.USER_SESSION];

                var dao = new CateProductDao();
                category.ModifiedDate = DateTime.Now;
                category.ModifiedBy = session.TenTK;
                var result = dao.update(category);
                if (result)
                {
                     SetAlert("Sửa danh mục sản phẩm thành công", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Sửa danh mục không thành công", "error");
                }
            }
            return View(category);
        }
        public ActionResult Detail(long id)
        {
            var result = new CateProductDao().ViewDetail(id);
            return View(result);
        }
    }
}