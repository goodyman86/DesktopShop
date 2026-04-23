using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesktopShop.Domain.Entities;
using DesktopShop.Infrastructure.Data;
using DesktopShop.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            if (Session["AdminId"] != null)
                return RedirectToAction("Index", "DashboardView");

            ViewBag.Title = "Đăng nhập";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                ViewBag.Title = "Đăng nhập";
                return View();
            }

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var hash = IdentitySeed.HashPassword(password);

            using (var connection = new SqlConnection(connectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connection);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var admin = context.Customers
                        .FirstOrDefault(u => u.Username == username && u.PasswordHash == hash && u.Role == "Admin" && u.IsActive);

                    if (admin != null)
                    {
                        Session["AdminId"] = admin.Id;
                        Session["AdminName"] = admin.FullName;
                        Session["AdminUsername"] = admin.Username;
                        return RedirectToAction("Index", "DashboardView");
                    }
                }
            }

            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng.";
            ViewBag.Username = username;
            ViewBag.Title = "Đăng nhập";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        // Forgot Password - Step 1: Verify username and email
        public ActionResult ForgotPassword()
        {
            ViewBag.Title = "Quên mật khẩu";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string username, string email)
        {
            ViewBag.Title = "Quên mật khẩu";
            ViewBag.Username = username;
            ViewBag.Email = email;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connection);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var admin = context.Customers.FirstOrDefault(c => 
                        c.Username == username && 
                        c.Email == email && 
                        c.Role == "Admin" && 
                        c.IsActive);

                    if (admin == null)
                    {
                        ViewBag.Error = "Tên đăng nhập hoặc email không đúng.";
                        return View();
                    }

                    // Store username in TempData to pass to ResetPassword view
                    TempData["ResetUsername"] = username;
                    return RedirectToAction("ResetPassword");
                }
            }
        }

        // Reset Password - Step 2: Create new password
        public ActionResult ResetPassword()
        {
            if (TempData["ResetUsername"] == null)
            {
                return RedirectToAction("ForgotPassword");
            }

            ViewBag.Title = "Đặt lại mật khẩu";
            ViewBag.Username = TempData["ResetUsername"];
            TempData.Keep("ResetUsername"); // Keep for POST
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string username, string newPassword, string confirmPassword)
        {
            ViewBag.Title = "Đặt lại mật khẩu";
            ViewBag.Username = username;

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                TempData["ResetUsername"] = username;
                return View();
            }

            if (newPassword.Length < 6)
            {
                ViewBag.Error = "Mật khẩu phải có ít nhất 6 ký tự.";
                TempData["ResetUsername"] = username;
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp.";
                TempData["ResetUsername"] = username;
                return View();
            }

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connection);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var admin = context.Customers.FirstOrDefault(c => 
                        c.Username == username && 
                        c.Role == "Admin" && 
                        c.IsActive);

                    if (admin == null)
                    {
                        ViewBag.Error = "Tài khoản không tồn tại.";
                        return View();
                    }

                    // Update password
                    admin.PasswordHash = IdentitySeed.HashPassword(newPassword);
                    context.SaveChanges();

                    TempData["LoginSuccess"] = "Đặt lại mật khẩu thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
            }
        }

        // Admin Profile
        public ActionResult Profile()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");

            ViewBag.Title = "Thông tin tài khoản";

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connection);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var adminId = (int)Session["AdminId"];
                    var admin = context.Customers.FirstOrDefault(c => c.Id == adminId && c.Role == "Admin");

                    if (admin == null)
                        return RedirectToAction("Login");

                    ViewBag.Admin = admin;
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(string fullName, string email, string phone, string address)
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");

            ViewBag.Title = "Thông tin tài khoản";

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connection);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var adminId = (int)Session["AdminId"];
                    var admin = context.Customers.FirstOrDefault(c => c.Id == adminId && c.Role == "Admin");

                    if (admin == null)
                        return RedirectToAction("Login");

                    if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
                    {
                        ViewBag.Error = "Họ tên và Email không được để trống.";
                        ViewBag.Admin = admin;
                        return View("Profile");
                    }

                    // Check email uniqueness (exclude self)
                    if (context.Customers.Any(c => c.Email == email && c.Id != adminId))
                    {
                        ViewBag.Error = "Email đã được sử dụng bởi tài khoản khác.";
                        ViewBag.Admin = admin;
                        return View("Profile");
                    }

                    admin.FullName = fullName;
                    admin.Email = email;
                    admin.Phone = phone;
                    admin.Address = address;
                    context.SaveChanges();

                    Session["AdminName"] = admin.FullName;

                    ViewBag.Success = "Cập nhật thông tin thành công!";
                    ViewBag.Admin = admin;
                }
            }

            return View("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");

            ViewBag.Title = "Thông tin tài khoản";

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connection);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var adminId = (int)Session["AdminId"];
                    var admin = context.Customers.FirstOrDefault(c => c.Id == adminId && c.Role == "Admin");

                    if (admin == null)
                        return RedirectToAction("Login");

                    ViewBag.Admin = admin;

                    if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
                    {
                        ViewBag.PasswordError = "Vui lòng nhập đầy đủ thông tin.";
                        return View("Profile");
                    }

                    if (IdentitySeed.HashPassword(currentPassword) != admin.PasswordHash)
                    {
                        ViewBag.PasswordError = "Mật khẩu hiện tại không đúng.";
                        return View("Profile");
                    }

                    if (newPassword.Length < 6)
                    {
                        ViewBag.PasswordError = "Mật khẩu mới phải có ít nhất 6 ký tự.";
                        return View("Profile");
                    }

                    if (newPassword != confirmNewPassword)
                    {
                        ViewBag.PasswordError = "Mật khẩu xác nhận không khớp.";
                        return View("Profile");
                    }

                    admin.PasswordHash = IdentitySeed.HashPassword(newPassword);
                    context.SaveChanges();

                    ViewBag.PasswordSuccess = "Đổi mật khẩu thành công!";
                }
            }

            return View("Profile");
        }

        [HttpPost]
        public ActionResult UpdateAvatar(string avatarUrl)
        {
            if (Session["AdminId"] == null)
                return Json(new { success = false, message = "Chưa đăng nhập" });

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connection);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var adminId = (int)Session["AdminId"];
                    var admin = context.Customers.FirstOrDefault(c => c.Id == adminId && c.Role == "Admin");

                    if (admin == null)
                        return Json(new { success = false, message = "Không tìm thấy tài khoản" });

                    admin.AvatarUrl = avatarUrl;
                    context.SaveChanges();

                    return Json(new { success = true });
                }
            }
        }
    }
}
