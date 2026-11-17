using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _25DH110463_LTW.Models; 

namespace _25DH110463_LTW.Controllers
{
    public class AdminProductController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // 1. HIỂN THỊ DANH SÁCH SẢN PHẨM
        // GET: AdminProduct/Index
        public ActionResult Index()
        {
            // Lấy danh sách sản phẩm, sắp xếp theo ID giảm dần (sản phẩm mới nhất lên đầu)
            var products = db.Products.OrderByDescending(p => p.ProductID).ToList();
            return View(products);
        }
        // 2. THÊM SẢN PHẨM MỚI
        // GET: AdminProduct/Create
        public ActionResult Create()
        {
            // Lấy danh sách danh mục từ DB để hiển thị dạng DropDownList (menu xổ xuống)
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: AdminProduct/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Lưu ý: Có thêm tham số HttpPostedFileBase uploadImage để nhận file ảnh từ form
        public ActionResult Create(_25DH110463_LTW.Models.Product product, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng có chọn file ảnh không
                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    // Lấy tên file
                    string fileName = System.IO.Path.GetFileName(uploadImage.FileName);

                    // Tạo đường dẫn lưu file vào thư mục Images của project
                    // (Bạn đảm bảo project của mình có thư mục tên là "Images" nhé)
                    string path = Server.MapPath("~/Images/" + fileName);

                    // Lưu file lên server
                    uploadImage.SaveAs(path);

                    // Lưu tên file vào cột ProductImage trong Database
                    product.ProductImage = fileName;
                }

                // Thêm sản phẩm vào DB và lưu lại
                db.Products.Add(product);
                db.SaveChanges();

                // Trở về trang Danh sách
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, load lại DropDownList và hiển thị form
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }
    }
}