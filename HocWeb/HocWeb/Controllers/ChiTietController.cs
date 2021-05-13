using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HocWeb.Models;
using Models.Framwork;
using Models.Dao;
using Models.ViewModel;

namespace HocWeb.Controllers
{
    public class ChiTietController : Controller
    {
        DoAnWEB db = new DoAnWEB();
        // GET: ChiTiet
        public ActionResult ChiTiet()
        {
            return View();
        }
        public ViewResult XemChiTiet(int productID=0)
        {
            Product sanpham = db.Products.SingleOrDefault(n => n.ID == productID);
            if(sanpham==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        public PartialViewResult HienChiTietSanPhamPartial(int proID)
        {
            var SanPham = db.Products.Single(n => n.ID == proID);
            return PartialView(SanPham);
        }
        public PartialViewResult Relative(int cateID)
        {
            var sanpham= db.Products.Where(n => n.CategoryID == cateID).ToList();
            return PartialView(sanpham);
        }
        public PartialViewResult RelativeBrand(int brandID)
        {
            var sanpham = db.Products.Where(n => n.BrandID == brandID).Take(5).ToList();
            return PartialView(sanpham);
        }
        public PartialViewResult SalesPartial()
        {
            var SPKM = db.Products.Take(3).ToList();
            return PartialView(SPKM);
        }
        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string keyword, int page = 1, int pageSize = 1)
        {
            int totalRecord = 0;
            var model = new ProductDao();
            var model2 = new List<ProductViewModel>();
            model2 = model.Search(keyword, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            ViewBag.Keyword = keyword;
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model2);
        }
        //bài viết tin tức
        public ViewResult XemChiTiet_tintuc(int contentID = 0)
        {
            Content baiviet = db.Contents.SingleOrDefault(n => n.ID == contentID);
            if (baiviet == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(baiviet);
        }
        public PartialViewResult ChiTietTinTucPartial(int proID)
        {
            var Tin = db.Contents.Single(n => n.ID == proID);
            return PartialView(Tin);
        }
        public PartialViewResult RelativeTinTucPartial()
        {
            var content = db.Contents.ToList();
            return PartialView(content);
        }
        [HttpPost]
        public ActionResult Send_FB(string name, string mobile, string address, string email, string content)
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
            return Redirect("/hoan-thanh");
        }
    }
}