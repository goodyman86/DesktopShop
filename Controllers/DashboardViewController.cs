using System.Web.Mvc;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [AdminAuthFilter]
    public class DashboardViewController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Dashboard";
            ViewBag.ActivePage = "Dashboard";
            return View();
        }

        public ActionResult Products()
        {
            ViewBag.Title = "Quản lý Sản Phẩm";
            ViewBag.ActivePage = "Products";
            return View();
        }

        public ActionResult Orders()
        {
            ViewBag.Title = "Quản lý Đơn Hàng";
            ViewBag.ActivePage = "Orders";
            return View();
        }

        public ActionResult Categories()
        {
            ViewBag.Title = "Quản lý Danh Mục";
            ViewBag.ActivePage = "Categories";
            return View();
        }

        public ActionResult Revenue()
        {
            ViewBag.Title = "Xem Doanh Thu";
            ViewBag.ActivePage = "Revenue";
            return View();
        }
    }
}
