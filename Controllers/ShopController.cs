using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using DesktopShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class ShopController : Controller
    {
        private ApplicationDbContext CreateDbContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var connection = new SqlConnection(connectionString);
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connection);
            return new ApplicationDbContext(optionsBuilder.Options);
        }

        public ActionResult Index(string keyword, int? categoryId, string sort, int page = 1)
        {
            ViewBag.Title = "Cửa hàng";
            int pageSize = 8;

            using (var context = CreateDbContext())
            {
                var query = context.Products
                    .Include(p => p.Category)
                    .Where(p => p.IsActive);

                // Search by name
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(p => p.Name.Contains(keyword));
                }

                // Filter by category
                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    query = query.Where(p => p.CategoryId == categoryId.Value);
                }

                // Sort by price
                switch (sort)
                {
                    case "price_asc":
                        query = query.OrderBy(p => p.Price);
                        break;
                    case "price_desc":
                        query = query.OrderByDescending(p => p.Price);
                        break;
                    default:
                        query = query.OrderByDescending(p => p.CreatedAt);
                        break;
                }

                int totalItems = query.Count();
                int totalPages = (totalItems + pageSize - 1) / pageSize;
                if (page < 1) page = 1;
                if (page > totalPages && totalPages > 0) page = totalPages;

                var products = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var categories = context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToList();

                ViewBag.Products = products;
                ViewBag.Categories = categories;
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalItems = totalItems;
                ViewBag.Keyword = keyword;
                ViewBag.CategoryId = categoryId;
                ViewBag.Sort = sort;
            }

            return View();
        }

        public ActionResult Detail(int id)
        {
            using (var context = CreateDbContext())
            {
                var product = context.Products
                    .Include(p => p.Category)
                    .FirstOrDefault(p => p.Id == id && p.IsActive);

                if (product == null)
                    return HttpNotFound();

                var relatedProducts = context.Products
                    .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id && p.IsActive)
                    .Take(4)
                    .ToList();

                ViewBag.Title = product.Name;
                ViewBag.Product = product;
                ViewBag.RelatedProducts = relatedProducts;
            }

            return View();
        }
    }
}
