using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _25DH110463_LTW.Models;

namespace _25DH110463_LTW.Controllers
{
    public class AdminOrderController : Controller
    {
        // Khởi tạo Database. 
        // (Lưu ý: Nếu tên DbContext của bạn khác MyStoreEntities thì hãy đổi lại cho đúng nhé)
        private MyStoreEntities db = new MyStoreEntities();

        // 1. HIỂN THỊ DANH SÁCH TOÀN BỘ ĐƠN HÀNG
        public ActionResult Index()
        {
            // Lấy danh sách tất cả đơn hàng, sắp xếp ngày mới nhất lên đầu
            var orders = db.Orders.OrderByDescending(o => o.OrderDate).ToList();
            return View(orders);
        }
        // 2. HIỂN THỊ FORM CẬP NHẬT TRẠNG THÁI
        // GET: AdminOrder/UpdateStatus/5
        public ActionResult UpdateStatus(int id)
        {
            // Tìm đơn hàng theo ID
            var order = db.Orders.SingleOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // 3. LƯU TRẠNG THÁI MỚI VÀO DATABASE
        // POST: AdminOrder/UpdateStatus/5
        [HttpPost]
        public ActionResult UpdateStatus(int id, string PaymentStatus)
        {
            var order = db.Orders.SingleOrDefault(o => o.OrderID == id);
            if (order != null)
            {
                // Cập nhật trạng thái mới
                order.PaymentStatus = PaymentStatus;
                db.SaveChanges();
            }
            // Quay về trang danh sách đơn hàng
            return RedirectToAction("Index");
        }
    }
}