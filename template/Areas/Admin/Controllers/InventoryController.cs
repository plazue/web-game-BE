using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using template.Models;
using System.Data.Entity; // Bắt buộc phải có để sử dụng được phương thức .Include() cho Entity Framework

namespace template.Areas.Admin.Controllers
{
    public class InventoryController : Controller
    {
        private PhongCachXanhDBEntities1 db = new PhongCachXanhDBEntities1();

        // GET: Admin/Inventory

        public ActionResult Index()
        {
           
            var products = db.Products.Include(p => p.Category).ToList();

            return View(products);
        }
    }
}