using System;
using System.Linq;
using System.Web.Mvc;
using template.Models;

namespace template.Areas.Customer.Controllers
{
    public class AccountController : Controller
    {
        private PhongCachXanhDBEntities1 db = new PhongCachXanhDBEntities1();

        // GET: Customer/Account
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Edit");
            }
            return RedirectToAction("Login");
        }

        // GET: Customer/Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Customer/Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string username, string email, string password)
        {
            if (db.Users.Any(u => u.Email == email || u.Username == username))
            {
                ModelState.AddModelError("", "Email hoặc tên đăng nhập đã tồn tại!");
                return View();
            }

            var newUser = new User
            {
                Username = username,
                Email = email,
                Password = password,
                Role = "Customer",
                CreatedAt = DateTime.Now
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            return RedirectToAction("Login", "Account");
        }

        // GET: Customer/Account/Login
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Edit");
            }
            return View();
        }

        // POST: Customer/Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                Session["User"] = user;
                Session["UserID"] = user.UserID;
                Session["UserName"] = user.Username;
                Session["FullName"] = user.Username;

                // 🔴 BỔ SUNG QUAN TRỌNG: Lưu phân quyền Role vào Session
                Session["Role"] = user.Role;

                return RedirectToAction("Index", "Home2");
            }

            ModelState.AddModelError("", "Email hoặc mật khẩu không chính xác!");
            return View();
        }

        // GET: Customer/Account/Edit
        public ActionResult Edit()
        {
            int? userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = db.Users.Find(userId.Value);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Customer/Account/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string newPassword, string confirmPassword)
        {
            int? userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = db.Users.Find(userId.Value);
            if (user != null)
            {
                // Xử lý đổi mật khẩu nếu người dùng nhập
                if (!string.IsNullOrEmpty(newPassword))
                {
                    if (newPassword.Length < 6)
                    {
                        ModelState.AddModelError("", "Mật khẩu mới phải có ít nhất 6 ký tự.");
                        return View(user);
                    }

                    user.Password = newPassword;
                }

                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                ViewBag.SuccessMessage = "Cập nhật thông tin tài khoản thành công!";
            }

            return View(user);
        }

        // GET: Customer/Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home2");
        }
    }
}