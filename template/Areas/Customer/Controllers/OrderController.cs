using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using template.Models;
using template.Models.ViewModels;

namespace template.Areas.Customer.Controllers
{
    public class OrderController : Controller
    {
        private PhongCachXanhDBEntities1 db = new PhongCachXanhDBEntities1();

        // Lấy giỏ hàng từ Session
        private List<CartItemViewModel> GetSessionCart()
        {
            var cart = Session["Cart"] as List<CartItemViewModel>;
            if (cart == null)
            {
                cart = new List<CartItemViewModel>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // GET: Customer/Order/Checkout
        public ActionResult Checkout()
        {
            var cart = GetSessionCart();
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            return View(cart);
        }

        // POST: Customer/Order/PlaceOrder (Xử lý lưu đơn hàng và thông tin thanh toán vào SQL Server)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(string fullName, string phone, string address, string note, string paymentType, string cardHolderName, string cardNumberLast4, string cardExpiryDate, string cardBrand)
        {
            var cart = GetSessionCart();
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            try
            {
                // 0. Kiểm tra tồn kho trước khi tiến hành tạo đơn hàng (Dùng AsNoTracking để không bị dính validate entity)
                foreach (var item in cart)
                {
                    var productInDb = db.Products.AsNoTracking().FirstOrDefault(p => p.ProductID == item.ProductID);
                    if (productInDb == null || productInDb.StockQuantity < item.Quantity)
                    {
                        ModelState.AddModelError("", $"Sản phẩm '{item.Title}' không đủ số lượng trong kho (chỉ còn lại {productInDb?.StockQuantity ?? 0} sản phẩm).");
                        return View("Checkout", cart);
                    }
                }

                int? userId = Session["UserID"] as int?;

                // 1. Tạo đơn hàng mới trong bảng Orders, bao gồm thông tin thanh toán
                var order = new Order
                {
                    UserID = userId,
                    Subtotal = cart.Sum(x => x.Total),
                    OrderStatus = "Chờ xử lý",
                    CreatedAt = DateTime.Now,
                    OrderNote = $"Người nhận: {fullName} | SĐT: {phone} | ĐC: {address}. Ghi chú: {note}",

                    // Lưu thông tin phương thức thanh toán
                    PaymentType = string.IsNullOrEmpty(paymentType) ? "COD" : paymentType
                };

                // Nếu chọn thanh toán bằng thẻ tín dụng, gán thông tin thẻ tương ứng từ form
                if (order.PaymentType == "CreditCard")
                {
                    order.CardHolderName = cardHolderName;
                    order.CardNumberLast4 = cardNumberLast4;
                    order.CardExpiryDate = cardExpiryDate;
                    order.CardBrand = cardBrand;

                    order.CreditCard = $"{cardBrand} - **** **** **** {cardNumberLast4} (Chủ thẻ: {cardHolderName}, Hạn: {cardExpiryDate})";
                }
                else
                {
                    order.CreditCard = "Thanh toán khi nhận hàng (COD)";
                }

                db.Orders.Add(order);
                db.SaveChanges(); // Lưu Order và OrderDetails trước

                // 2. Lưu chi tiết đơn hàng vào bảng OrderDetails và cập nhật giảm tồn kho trực tiếp qua SQL
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    db.OrderDetails.Add(orderDetail);

                    // Trừ tồn kho trực tiếp bằng SQL để hoàn toàn vô hiệu hóa cơ chế validate ProductImage ngầm của EF
                    db.Database.ExecuteSqlCommand(
                        "UPDATE Products SET StockQuantity = CASE WHEN StockQuantity - {0} < 0 THEN 0 ELSE StockQuantity - {0} END WHERE ProductID = {1}",
                        item.Quantity, item.ProductID
                    );
                }

                db.SaveChanges();

                // 3. Xóa giỏ hàng sau khi đặt thành công
                Session["Cart"] = null;

                // 4. Chuyển hướng sang trang thành công kèm theo OrderID
                return RedirectToAction("OrdersSuccess", new { id = order.OrderID });
            }
            catch (DbEntityValidationException dbEx)
            {
                var errorMessages = dbEx.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => $"[Trường: {x.PropertyName} - Lỗi: {x.ErrorMessage}]");

                string fullErrorMessage = string.Join(" | ", errorMessages);

                ModelState.AddModelError("", "Lỗi Validate dữ liệu: " + fullErrorMessage);
                return View("Checkout", cart);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMsg += " -> " + ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMsg += " -> " + ex.InnerException.InnerException.Message;
                    }
                }

                ModelState.AddModelError("", "Lỗi khi xử lý đơn hàng: " + errorMsg);
                return View("Checkout", cart);
            }
        }

        // GET: Customer/Order/OrdersSuccess/5
        public ActionResult OrdersSuccess(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home2");
            }

            var order = db.Orders.Include(o => o.OrderDetails.Select(d => d.Product)).FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // GET: Customer/Order/OrderHistory
        public ActionResult OrderHistory()
        {
            int? userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = db.Orders
                .Include(o => o.OrderDetails.Select(d => d.Product))
                .Where(o => o.UserID == userId.Value)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return View(orders);
        }

        // GET: Customer/Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("OrderHistory");
            }

            int? userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = db.Orders
                          .Include(o => o.OrderDetails.Select(d => d.Product))
                          .FirstOrDefault(o => o.OrderID == id && o.UserID == userId.Value);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }
    }
}