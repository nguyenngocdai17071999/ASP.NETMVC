using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Framwork;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Code;
using PagedList;
using HocWeb.Areas.Admin.Models;
using Models.Dao;

namespace HocWeb.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        [HttpGet]
        public ActionResult Detail(long id)
        {
            var result = new OrderDao().ViewDetail(id);
            var dao = new UserDao().ViewDetail(result.CustomerID);
            IList<OrderDetail> product = new OrderDao().GetAll(id);
            IList<User> user = new List<User>();
            user.Add(dao);
            ViewData["KHACHHANG"] = user;
            ViewData["SANPHAM"] = product;
            return View(result);
        }
        public ActionResult Index(int page = 1, int pageSize = 1000, string searchString=null)
        {
            var dao = new OrderDao();
            var model = dao.ListAllPaging(page, pageSize, searchString);
            ViewBag.SearchString = searchString;
            return View(model);
        }
       
        [HttpPost]
        public ActionResult Detail(Order orders)
        {
            if (ModelState.IsValid)
            {
                var result = new OrderDao().ViewDetail(orders.ID);
                var dao = new UserDao().ViewDetail(result.CustomerID);
                IList<OrderDetail> product = new OrderDao().GetAll(orders.ID);
                IList<User> user = new List<User>();
                user.Add(dao);
                ViewData["KHACHHANG"] = user;
                ViewData["SANPHAM"] = product;
                var order = new OrderDao();
                order.update(orders);
                SetAlert("Chỉnh sửa trạng thái thành công", "success");
                RedirectToAction("index");
             
            }           
            return View(orders);
        }
    }
}