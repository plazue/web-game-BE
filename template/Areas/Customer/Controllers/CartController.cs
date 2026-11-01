using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using template.Models;
using template.Models.ViewModels;

namespace template.Areas.Customer.Controllers
{
    public class CartController : Controller
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

        // GET: Customer/Cart/Index
        public ActionResult Index()
        {
            var cart = GetSessionCart();
            return View(cart);
        }

        // POST: Customer/Cart/AddToCart
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = db.Products.Find(productId);
            if (product == null)
            {
                return HttpNotFound();
            }

            var cart = GetSessionCart();
            var item = cart.FirstOrDefault(p => p.ProductID == productId);

            // Kiểm tra xem sản phẩm có đang có giá khuyến mãi hợp lệ hay không
            decimal finalPrice = (product.SalePrice != null && product.SalePrice > 0 && product.SalePrice < product.Price)
                                 ? product.SalePrice.Value
                                 : product.Price;

            if (item != null)
            {
                item.Quantity += quantity;
                // Cập nhật lại giá mới nhất (phòng trường hợp sản phẩm vừa được Admin đổi giá sale)
                item.Price = finalPrice;
            }
            else
            {
                cart.Add(new CartItemViewModel
                {
                    ProductID = product.ProductID,
                    Title = product.Title,
                    Price = finalPrice, // Sử dụng SalePrice nếu có, ngược lại dùng Price gốc
                    Image = product.ProductImage,
                    Quantity = quantity
                });
            }

            Session["Cart"] = cart;
            return RedirectToAction("Index");
        }

        // POST: Customer/Cart/UpdateQuantity
        [HttpPost]
        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = GetSessionCart();
            var item = cart.FirstOrDefault(p => p.ProductID == productId);
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    cart.Remove(item);
                }
            }
            Session["Cart"] = cart;
            return RedirectToAction("Index");
        }

        // GET: Customer/Cart/RemoveItem
        public ActionResult RemoveItem(int productId)
        {
            var cart = GetSessionCart();
            var item = cart.FirstOrDefault(p => p.ProductID == productId);
            if (item != null)
            {
                cart.Remove(item);
            }
            Session["Cart"] = cart;
            return RedirectToAction("Index");
        }

        // Lấy tổng số lượng sản phẩm hiển thị lên badge giỏ hàng
        public ActionResult CartBadge()
        {
            var cart = Session["Cart"] as List<template.Models.ViewModels.CartItemViewModel>;
            int totalQty = cart != null ? cart.Sum(x => x.Quantity) : 0;
            return Content($"<span class=\"cart-badge\">{totalQty}</span>");
        }

        // Lấy danh sách sản phẩm thu nhỏ hiển thị khi rê chuột vào icon giỏ hàng
        public ActionResult CartSubmenu()
        {
            var cart = Session["Cart"] as List<template.Models.ViewModels.CartItemViewModel>;
            return PartialView("_CartSubmenu", cart);
        }
    }
}