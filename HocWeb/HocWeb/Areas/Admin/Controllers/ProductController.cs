using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Dao;
using Models.Framwork;
using HocWeb.Areas.Admin.Code;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using OfficeOpenXml;
using System.Drawing;

namespace HocWeb.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
       public ActionResult Index(int page = 1, int pageSize = 1000, string searchString = null)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging(page, pageSize, searchString);
            ViewBag.SearchString = searchString;
            SetViewBag1();
            return View(model);
        }
        public ActionResult XuatSanPham(int page = 1, int pageSize = 1000, string searchString = null)
        {
            ProductDao dao = new ProductDao();
            List<Product> products = dao.GetListProduct();
            var model = dao.ListAllPaging(page, pageSize, searchString);
            ViewBag.SearchString = searchString;
            SetViewBag1();
            return View(products);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            SetViewBag1();
            return View();
        }
        [HttpGet]
        public ActionResult Edit(long ID)
        {
            var product = new ProductDao().ViewDetail(ID);
            SetViewBag1();
            SetViewBag();
            return View(product);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product)
        {
            var dao =  new ProductDao();
            var result = dao.CheckProduct(product.Name);
            if (ModelState.IsValid)
            {
                if (result ==false)
                {                 
                    var session = (UserSession)Session[CommomConstants.USER_SESSION];
                    var data = new ProductDao();
                    if (product.PromotionPrice == null)
                        product.PromotionPrice = 0;
                    product.CreatedDate = DateTime.Now;
                    product.CreatedBy = session.TenTK;
                    product.ModifiedDate = DateTime.Now;
                   product.ModifiedBy = session.TenTK;
                    var id=data.Insert(product);
                    if (id > 0)
                    {
                        SetAlert("Thêm sản phẩm thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Thêm sản phẩm thất bại", "error");
                    }                   
                }
                else
                {
                    SetAlert("Sản phẩm đã tồn tại", "error");
                }
            }
            SetViewBag();
            SetViewBag1();
            return View(product);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var session = (UserSession)Session[CommomConstants.USER_SESSION];

                var data = new ProductDao();
                if (product.PromotionPrice == null)
                    product.PromotionPrice = 0;
                product.ModifiedDate = DateTime.Now;
                product.ModifiedBy = session.TenTK;
                var result = data.update(product);
                if (result)
                {
                   SetAlert("Sửa sản phẩm thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    SetAlert("Sửa không thành công", "error");
                }
            }
            SetViewBag();
            SetViewBag1();
            return View(product);
        }
        public void SetViewBag(long? selectedID=null)
        {
            var dao = new CateProductDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name",selectedID);
        }
        public void SetViewBag1(long? selectedID = null)
        {
            var dao = new ProductDao();
            ViewBag.BrandID = new SelectList(dao.ListAll(), "ID", "Name", selectedID);
        }
        public ActionResult Delete(long ID)
        {
            var oders = new OrderDao();
            bool result = oders.hasOrderDetail(ID);
            if (result == false)
            {
                SetAlert("Xoá thất bại !!!", "error");
            }
            else
            {
                new ProductDao().Delete(ID);
                SetAlert("Xóa thành công", "success");
            }                      
            return RedirectToAction("Index", "Product");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public ActionResult DashBoard()
        {
            var dao = new ProductDao();
            var list = dao.ListAllProduct();
            List<int> reparttitons = new List<int>();
            var posion = list.Select(x => x.Price).Distinct();
            foreach (var item in posion)
            {
                reparttitons.Add(list.Count(x => x.Price == item));
            }

            var rep = reparttitons;
            ViewBag.POSI = posion;
            ViewBag.REP = reparttitons.ToList();
            return View();
        }
      [HttpPost]
      public ActionResult Import(HttpPostedFileBase excelfile)
        {

        
            if(excelfile.ContentLength==0)
            {
                SetAlert("Chọn thư mục Excel", "error");
                return View("Index");
            }
            else
            {
                if(excelfile.FileName.EndsWith("xls")|| excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    excelfile.SaveAs(path);

                    Excel.Application application = new Excel.Application();
                    Excel.Workbook  workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<Product> products = new List<Product>();
            
                    SetAlert("Thành công", "success");
                    return View("Success");
                }
                else
                {
                    SetAlert("Không thể mở thư mục", "error");
                    return View("Index");
                }

            }
           
        }
        public ActionResult Export()
        {
            try
            {
                ProductDao dao = new ProductDao();
                List<Product> products = dao.GetListProduct();
                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Report");


                worksheet.Cells["A1"].Value = "Danh sách sản phẩm";
                worksheet.Cells["B1"].Value = "Com1";

                worksheet.Cells["A2"].Value = "Report";
                worksheet.Cells["B2"].Value = "Report1";

                worksheet.Cells["A3"].Value = "Date";
                worksheet.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

                worksheet.Cells["A6"].Value = "ID";
                worksheet.Cells["B6"].Value = "Name";
                worksheet.Cells["C6"].Value =  "Code";
                worksheet.Cells["D6"].Value =  "Price";
                worksheet.Cells["E6"].Value =  "BrandName";
                worksheet.Cells["F6"].Value =  "CategoryName";
                worksheet.Cells["G6"].Value = "Quantity";
                worksheet.Cells["H6"].Value =  "ViewCount";
                worksheet.Cells["I6"].Value = "CreatedDate";

                int row = 7;
                foreach (var item in products)
                {
                    if (item.ID < 5)
                    {
                        worksheet.Row(row).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Row(row).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("blue")));
                    }
                   
                    
                    worksheet.Cells[string.Format("A{0}",row)].Value = item.ID;
                    worksheet.Cells[string.Format("B{0}", row)].Value = item.Name;
                    worksheet.Cells[string.Format("C{0}", row)].Value = item.Code;
                    worksheet.Cells[string.Format("D{0}", row)].Value = item.Price;
                    worksheet.Cells[string.Format("E{0}", row)].Value = item.Brand.Name;
                    worksheet.Cells[string.Format("F{0}", row)].Value = item.ProductCategory.Name;
                    worksheet.Cells[string.Format("G{0}", row)].Value = item.Quantity;
                    worksheet.Cells[string.Format("H{0}", row)].Value = item.ViewCount;
                    worksheet.Cells[string.Format("I{0}", row)].Value = item.CreatedDate.ToString("MM/dd/yyyy");
                    row++;

                }
                worksheet.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            
         
                SetAlert("Xuất Excel thành công", "success");
            }
            catch(Exception ex)
            {
                ViewBag.Result = ex.Message;
            }
            return View("XuatSanPham");
        }
        public ActionResult Detail(long id)
        {
            var result = new ProductDao().ViewDetail(id);
            return View(result);
        }

    }
}