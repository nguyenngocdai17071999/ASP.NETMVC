using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Dao;
using HocWeb.Areas.Admin.Code;
using Models.Framwork;
using System.Web.Mvc;

namespace HocWeb.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Admin/Content
        public ActionResult Index(int page = 1, int pageSize = 1000, string searchString = null)
        {
            var dao = new ContentDao();
            var model = dao.ListAllPaging(page, pageSize, searchString);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpGet]
        public ActionResult Edit(long ID)
        {
            var content = new ContentDao().ViewDetail(ID);
            SetViewBag();
            return View(content);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Content model)
        {
            var dao = new ContentDao();
            var result = dao.CheckContent(model.Name);
            if (ModelState.IsValid)
            {
                if (result == false)
                {
                    var session = (UserSession)Session[CommomConstants.USER_SESSION];
                    var data = new ContentDao();                   
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = session.TenTK;
                    model.ModifiedDate = DateTime.Now;
                    model.ModifiedBy = session.TenTK;
                    var id = data.Insert(model);
                    if (id > 0)
                    {
                        SetAlert("Thêm tin tức thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Thêm tin tức thất bại", "error");
                    }
                }
                else
                {
                    SetAlert("Tin tức đã tồn tại", "error");
                }
            }
            SetViewBag();
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Content content)
        {
            if (ModelState.IsValid)
            {
                var session = (UserSession)Session[CommomConstants.USER_SESSION];

                var data = new ContentDao();              
                content.ModifiedDate = DateTime.Now;
                content.ModifiedBy = session.TenTK;
                var result = data.update(content);
                if (result)
                {
                    SetAlert("Sửa tin tức thành công", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Sửa không thành công", "error");
                }
            }
            SetViewBag();
            return View(content);
        }
        public void SetViewBag(long? selectedID=null)
        {
            var dao = new CategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name",selectedID);
        }
        public ActionResult Delete(long ID)
        {
            new ContentDao().Delete(ID);
            SetAlert("Xóa thành công", "success");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ContentDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public ActionResult Detail(long id)
        {
            var result = new ContentDao().ViewDetail(id);
            return View(result);
        }

    }
}