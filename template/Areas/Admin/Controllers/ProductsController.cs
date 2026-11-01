using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using template.Models;
using template.Models.ViewModels;

namespace template.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private PhongCachXanhDBEntities1 db = new PhongCachXanhDBEntities1();

        // GET: Admin/Products
        [HttpGet]
        public ActionResult Index(string searchTerm)
        {
            var model = new ProductSearchVM();
            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Tìm kiếm theo tên sản phẩm, mô tả hoặc tên danh mục
                products = products.Where(p => p.Title.Contains(searchTerm) ||
                                               p.Description.Contains(searchTerm) ||
                                               p.Category.CategoryName.Contains(searchTerm));

                model.SearchTerm = searchTerm;
            }

            model.Products = products.ToList();
            return View(model);
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Đảm bảo Include bảng Category để tránh lỗi NullReference khi gọi Category.CategoryName ở View
            Product product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase ProductImage, IEnumerable<HttpPostedFileBase> ProductImages1)
        {
            if (ModelState.IsValid)
            {
                // 1. Xử lý lưu Ảnh chính vào ProductImage (nếu có chọn)
                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProductImage.FileName);
                    string path = Server.MapPath("~/Content/images/" + fileName);

                    string directory = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    ProductImage.SaveAs(path);
                    product.ProductImage = "/Content/images/" + fileName;
                }
                else
                {
                    product.ProductImage = null;
                }

                // 2. Xử lý lưu NHIỀU ảnh phụ vào ProductImages1 (Nối bằng dấu phẩy)
                if (ProductImages1 != null && ProductImages1.Any(f => f != null && f.ContentLength > 0))
                {
                    List<string> subImagePaths = new List<string>();
                    foreach (var file in ProductImages1)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string subFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                            string subPath = Server.MapPath("~/Content/images/" + subFileName);

                            string directory = Path.GetDirectoryName(subPath);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            file.SaveAs(subPath);
                            subImagePaths.Add("/Content/images/" + subFileName);
                        }
                    }

                    if (subImagePaths.Count > 0)
                    {
                        product.ProductImages1 = string.Join(",", subImagePaths);
                    }
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,Brand,Title,Price,SalePrice,ProductType,Color,Description,StockQuantity,ProductImage,ProductImages1")] Product product, HttpPostedFileBase ProductImage, IEnumerable<HttpPostedFileBase> ProductImages1)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin sản phẩm cũ trong Database để xử lý logic ảnh
                var existingProduct = db.Products.AsNoTracking().FirstOrDefault(p => p.ProductID == product.ProductID);

                // 1. Xử lý ảnh chính khi Edit
                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProductImage.FileName);
                    var path = Server.MapPath("~/Content/images/" + fileName);
                    ProductImage.SaveAs(path);
                    product.ProductImage = "/Content/images/" + fileName;
                }
                else
                {
                    // Giữ lại ảnh chính cũ nếu không upload ảnh mới
                    product.ProductImage = existingProduct?.ProductImage;
                    db.Entry(product).Property(x => x.ProductImage).IsModified = false;
                }

                // 2. Xử lý NHIỀU ảnh phụ khi Edit (Cộng dồn ảnh mới vào ảnh cũ)
                List<string> allSubImages = new List<string>();
                if (!string.IsNullOrEmpty(existingProduct?.ProductImages1))
                {
                    allSubImages.AddRange(existingProduct.ProductImages1.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                }

                bool hasNewSubImages = false;
                if (ProductImages1 != null && ProductImages1.Any(f => f != null && f.ContentLength > 0))
                {
                    foreach (var file in ProductImages1)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string subFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                            string subPath = Server.MapPath("~/Content/images/" + subFileName);
                            file.SaveAs(subPath);
                            allSubImages.Add("/Content/images/" + subFileName);
                            hasNewSubImages = true;
                        }
                    }
                }

                if (hasNewSubImages)
                {
                    product.ProductImages1 = string.Join(",", allSubImages);
                }
                else
                {
                    product.ProductImages1 = existingProduct?.ProductImages1;
                    db.Entry(product).Property(x => x.ProductImages1).IsModified = false;
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            if (product != null)
            {
                // 1. Xóa tất cả các chi tiết đơn hàng (OrderDetails) liên quan đến sản phẩm này trước
                var orderDetails = db.OrderDetails.Where(od => od.ProductID == id).ToList();
                if (orderDetails.Any())
                {
                    db.OrderDetails.RemoveRange(orderDetails);
                }

                // 2. Sau đó mới tiến hành xóa sản phẩm
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}