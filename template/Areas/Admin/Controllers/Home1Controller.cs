using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using template.Models;

namespace template.Areas.Admin.Controllers
{
    public class Home1Controller : Controller
    {
        private PhongCachXanhDBEntities1 db = new PhongCachXanhDBEntities1();

        // GET: Admin/Home1/Index (Trang bảng thống kê)
        public ActionResult Index()
        {
            // Thống kê hàng hóa theo từng danh mục (Loại hàng)
            var stats = db.Categories.Select(c => new CategoryStatVM
            {
                CategoryName = c.CategoryName,
                Count = c.Products.Count(),
                MaxPrice = c.Products.Any() ? c.Products.Max(p => (decimal?)p.Price) ?? 0 : 0,
                MinPrice = c.Products.Any() ? c.Products.Min(p => (decimal?)p.Price) ?? 0 : 0,
                AvgPrice = c.Products.Any() ? c.Products.Average(p => (decimal?)p.Price) ?? 0 : 0
            }).ToList();

            return View(stats);
        }

        // GET: Admin/Home1/Chart (Trang hiển thị Google Chart)
        public ActionResult Chart()
        {
            var stats = db.Categories.Select(c => new CategoryStatVM
            {
                CategoryName = c.CategoryName,
                Count = c.Products.Count()
            }).ToList();

            return View(stats);
        }
    }

    // ViewModel phục vụ thống kê
    public class CategoryStatVM
    {
        public string CategoryName { get; set; }
        public int Count { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal AvgPrice { get; set; }
    }
}