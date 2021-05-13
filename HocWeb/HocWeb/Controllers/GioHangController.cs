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
    public class GioHangController : Controller
    {
        private string CartSession = "CartSession";
        DoAnWEB db = new DoAnWEB();
        List<Product> listpro = new List<Product>();
        // GET: GioHang
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public ActionResult AddItem(long productID, int quantity)
        {
            var product = new ProductDao().ViewDetail(productID);
            var cart = Session[CartSession];
            if (cart != null)
            {

                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.product.ID == productID))
                {
                    foreach (var item in list)
                    {
                        if (item.product.ID == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tao moi doi tuong cart item
                    var item = new CartItem();
                    item.product = new Product();
                    item.product = product;
                    item.product.ID = productID;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[CartSession] = list;
            }
            else
            {
                //tao moi doi tuong cart item
                var item = new CartItem();
                item.product = new Product();
                item.product = product;
                item.product.ID = productID;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                Session[CartSession] = list;
            }
            return RedirectToAction("Index","GioHang");
        }
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);

        }
        public JsonResult DelAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Del(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.product.ID == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Update(string cartModel)
        {
            var jsoncart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsoncart.SingleOrDefault(x => x.product.ID == item.product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;

                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        [HttpGet]
        public ActionResult Buy()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Buy(string fname,string lname, string phone, string city, string address, string email,string country)
        {
            var sesson = (UserSession)Session[CommomConstants.USER_SESSION];
            var oder = new Order();


            if (sesson != null)
            {
                if (sesson.ChucVu == "2" || sesson.ChucVu == "3")
                {
                    return RedirectToAction("LoiPhanQuyen", "TrangChu");
                }
                
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }                  
            oder.ShipName = fname + lname;
            oder.CreatedDate = DateTime.Now;
            oder.ShipAddress = address+"/" + city+"/"+ country;
            oder.ShipMobile = phone;
            oder.ShipEmail = email;
            oder.CustomerID = sesson.UserID;
            oder.Status = 1;
            decimal? total = 0;
            try
            {
                var id = new OrderDao_client().Insert(oder);
                var cart = (List<CartItem>)Session[CartSession];
                var detailDao = new OrderDetailDao_client();
                foreach (var item in cart)
                {
                    var productUpDate = new Product();
                    productUpDate = new ProductDao().GetByID(item.product.ID);
                    if (productUpDate.Quantity > 0)
                    {
                        productUpDate.Quantity--;
                        var proDaoUpdate = new ProductDao();
                        proDaoUpdate.update(productUpDate);
                        var orderDetail = new OrderDetail();
                        orderDetail.ProductID = item.product.ID;
                        orderDetail.OrderID = id;
                        orderDetail.Price = item.product.Price;
                        orderDetail.Quantity = item.Quantity;
                        detailDao.Insert(orderDetail);
                        total += item.product.Price;
                        DelAll();
                    }
                    else
                    {
                        return Redirect("/Het-Hang");
                    }
                }
            }
            catch(Exception ex){
                //ghi log
                return Redirect("/Loi-thanh-toan");
            }
            
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/vendor/SentHoaDon.html"));
            content = content.Replace("{{UserName}}", sesson.TenTK);
            content = content.Replace("{{Name}}", sesson.Name);
            content = content.Replace("{{NameShip}}", oder.ShipName);
            content = content.Replace("{{Phone}}", oder.ShipName);
            content = content.Replace("{{Email}}", oder.ShipEmail);
            content = content.Replace("{{Address}}", oder.ShipAddress);
            content = content.Replace("{{Total}}", total.ToString());
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            try
            {
                new MailHelper().SentMail(toEmail, "Thông tin hóa đơn", content);
                new MailHelper().SentMail(oder.ShipEmail, "Thông tin hóa đơn", content);
            }
            catch { }
          
        
            return Redirect("/hoan-thanh");
        }

        public ActionResult Complete()
        {
            return View();
        }
        public PartialViewResult OderedPartial(long cusID)
            {
            var model = (from a in db.Orders
                                join b in db.OrderDetails
                                on a.ID equals b.OrderID join c in db.Products on b.ProductID equals c.ID
                                where a.CustomerID ==cusID
                                select new
                                {
                                    CateMetaTitle = c.MetaTitle,
                                    CateName = c.Name,
                                    CreatedDate = a.CreatedDate,
                                    ID = a.ID,
                                    Images = c.Image,
                                    Name = c.Name,
                                    MetaTitle = c.MetaTitle,
                                    Price = c.Price
                                }).AsEnumerable().Select(x => new ProductViewModel()
                                 {
                                     CateMetaTitle = x.MetaTitle,
                                     CateName = x.Name,
                                     CreatedDate = x.CreatedDate,
                                     ID = x.ID,
                                     Images = x.Images,
                                     Name = x.Name,
                                     MetaTitle = x.MetaTitle,
                                     Price = x.Price
                                 });
            return PartialView(model.ToList());
        }
    }
}