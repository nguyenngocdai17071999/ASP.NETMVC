using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HocWeb.Models;
using HocWeb.Areas.Admin.Controllers;
using Models.Framwork;
using HocWeb.Areas.Admin.Code;
using Models.Dao;
using PagedList;

namespace HocWeb.Controllers
{
    public class TrangChuController : Controller
    {
        // GET: TrangChu
        DoAnWEB db = new DoAnWEB();
        List<Product> listpro = new List<Product>();
        public ActionResult Index()
        {
           
            return View();
        }
        public PartialViewResult NewSanPhamPartial()
        {
            var SanPham = db.Products.OrderBy(n=>n.CreatedDate).ToList();
            return PartialView(SanPham);
        }
        public ActionResult NewSanPham(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            List<Product> Lstsanpham = db.Products.Take(15).OrderBy(n => n.CreatedDate).ToList();

            if (Lstsanpham.Count == 0)
            {
                ViewBag.Product = "Không có sản phẩm nào";
                return null;
            }
            if (Lstsanpham.Count < pageSize)
                pageSize = Lstsanpham.Count;
            return View(Lstsanpham.ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult DienThoaiPartial()
        {
            var SanPham = db.Products.Where(n => n.CategoryID == 1).Take(3).ToList();
            return PartialView(SanPham);
        }
        public PartialViewResult DoanhMucPartial()
        {
            var DoanhMuc= db.ProductCategories.ToList();
            return PartialView(DoanhMuc);
        }

        public PartialViewResult SanPhamKhuyenMaiPartial()
        {
            var SPKM = db.Products.Where(x=>x.Price!=x.PromotionPrice).Take(5).ToList();
            return PartialView(SPKM);
        }
        public List<Product> SanPham(int proCate)
        {
            return db.Products.Where(n => n.CategoryID == proCate).OrderBy(n => n.Price).ToList();
        }
        public ViewResult DoanhMucSP(int? page,int proCate=0)
        {
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ProductCategory procate = db.ProductCategories.SingleOrDefault(n => n.ID == proCate);
            if(procate==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Product> Lstsanpham = db.Products.Where(n => n.CategoryID == proCate).OrderBy(n => n.Price).ToList();

            if (Lstsanpham.Count==0)
            {
                ViewBag.Product = "Không có sản phẩm nào";
                return null;
            }
            if (Lstsanpham.Count< pageSize)
                pageSize = Lstsanpham.Count;
            return View(Lstsanpham.ToPagedList(pageNumber, pageSize));
        }
        public ViewResult SanPhamTheoHang(int? page, int brandID = 0)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            Brand procate = db.Brands.SingleOrDefault(n => n.ID == brandID);
            if (procate == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Product> Lstsanpham = db.Products.Where(n => n.BrandID == brandID).OrderBy(n => n.Price).ToList();

            if (Lstsanpham.Count == 0)
            {
                ViewBag.Product = "Không có sản phẩm nào";
                return null;
            }
            if (Lstsanpham.Count < pageSize)
                pageSize = Lstsanpham.Count;
            return View(Lstsanpham.ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult LaptopPartial()
        {
            var lap = db.Products.Where(n => n.CategoryID == 41).Take(3).ToList();
            return PartialView(lap);
        }
        public PartialViewResult PhuKienPartial()
        {
            var phukien= db.Products.Where(n => n.CategoryID == 43).Take(3).ToList();
            return PartialView(phukien);
        }
        public PartialViewResult TabletPartial()
        {
            var tablet = db.Products.Where(n => n.CategoryID == 2).Take(3).ToList();
            return PartialView(tablet);
        }
        public PartialViewResult Slider()
        {
            var slider = db.Contents.Take(5).ToList();
            return PartialView(slider);
        }
        public PartialViewResult HangPartial()
        {
            var hang = db.ProductCategories.Take(6).ToList();
            return PartialView(hang);
        }
        [HttpGet]
        public ActionResult InfoUser()
        {
            var session = (UserSession)Session[CommomConstants.USER_SESSION];          
            var result = new UserDao().ViewDetail(session.UserID);
            return View(result);
        }
      
        [HttpPost]
        public ActionResult InfoUser(User user)
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
                 dao.UpdateDetail(user);              
            }
            return View(user);
        }
        public ActionResult Logout()
        {
            Session[CommomConstants.USER_SESSION] = null;
            return View("index");
        }
        public ActionResult LoiPhanQuyen()
        {            
            return View();
        }
        public ViewResult ReturnProduct()
        {
            return View();
        }
        public ViewResult FreeShip()
        {
            return View();
        }
        public ViewResult SecurePayment()
        {
            return View();
        }
    }
}