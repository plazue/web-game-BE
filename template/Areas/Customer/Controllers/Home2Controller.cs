using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using template.Models;

namespace template.Areas.Customer.Controllers
{
    public class Home2Controller : Controller
    {
        private PhongCachXanhDBEntities1 db = new PhongCachXanhDBEntities1();

        // 1. Trang chủ (Load danh sách sản phẩm nổi bật / best seller còn hàng từ DB)
        // GET: Customer/Home2/Index
        public ActionResult Index()
        {
            // Chỉ lấy sản phẩm còn hàng (StockQuantity > 0)
            var products = db.Products
                             .Include(p => p.Category)
                             .Where(p => p.StockQuantity > 0)
                             .Take(8)
                             .ToList();
            return View(products);
        }

        // 2. Danh sách sản phẩm (Hỗ trợ lọc theo từ khóa tìm kiếm, danh mục, thương hiệu, sắp xếp giá và loại bỏ hàng hết)
        // GET: Customer/Home2/ProductList
        public ActionResult ProductList(int? categoryId, string searchKeyword, string brand, string sortOrder)
        {
            // Bắt đầu truy vấn và loại bỏ ngay những sản phẩm đã hết hàng (StockQuantity <= 0)
            var products = db.Products
                             .Include(p => p.Category)
                             .Where(p => p.StockQuantity > 0)
                             .AsQueryable();

            // Lọc theo danh mục
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryID == categoryId.Value);
            }

            // Lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                products = products.Where(p => p.Title.Contains(searchKeyword) || p.Brand.Contains(searchKeyword));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                products = products.Where(p => p.Brand == brand);
            }

            // Sắp xếp theo giá
            if (sortOrder == "price-asc")
            {
                products = products.OrderBy(p => p.Price);
            }
            else if (sortOrder == "price-desc")
            {
                products = products.OrderByDescending(p => p.Price);
            }
            else
            {
                products = products.OrderBy(p => p.ProductID); // Mặc định
            }

            // Truyền danh sách thương hiệu ra ViewBag (chỉ lấy thương hiệu của các sản phẩm còn hàng)
            ViewBag.Brands = db.Products
                               .Where(p => p.StockQuantity > 0 && p.Brand != null)
                               .Select(p => p.Brand)
                               .Distinct()
                               .ToList();

            ViewBag.Categories = db.Categories.ToList();

            return View(products.ToList());
        }

        // 3. Chi tiết sản phẩm & Lấy sản phẩm tương tự cùng danh mục (còn hàng)
        // GET: Customer/Home2/ProductDetail/5
        public ActionResult ProductDetail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductID == id);

            // Nếu không tìm thấy hoặc sản phẩm đó đã hết hàng thì báo lỗi 404 hoặc chuyển hướng
            if (product == null || product.StockQuantity <= 0)
            {
                return HttpNotFound();
            }

            // Lấy danh sách sản phẩm tương tự cùng Category (chỉ lấy các sản phẩm còn hàng và loại trừ sản phẩm hiện tại)
            var relatedProducts = db.Products
                .Where(p => p.CategoryID == product.CategoryID && p.ProductID != product.ProductID && p.StockQuantity > 0)
                .Take(4)
                .ToList();

            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }
    }
}