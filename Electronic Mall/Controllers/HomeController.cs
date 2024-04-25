using Electronic_Mall.Data;
using Electronic_Mall.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Electronic_Mall.Controllers
{
    public class HomeController : Controller
    {
        //localhost:7171/controller/Action in controller
        //localhost:7171/home/index

        public IActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
           
            var categories = db.Categories.Include(c => c.Products).ToList();
            ViewBag.Categories = categories;

            
             var products = db.Products.ToList();
             ViewBag.Products = products;

            return View();
        }
        //localhost:7171/home/privacy
        public IActionResult Privacy()
        {
            return View();
        }
        //localhost:7171/home/Category
        public IActionResult Category()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            /*Include() fetches data from two tables. So here, data from the Category
             * table and data from the Product table have been fetched*/
            var cats = db.Categories.Include(c => c.Products).ToList();

            return View(cats);

        }

        public ActionResult SubCategory(int subCategoryId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var products = db.Products.Where(p => p.Categoryid == subCategoryId).ToList();

            return View(products);
        }



        //localhost:7171/Home/Product?productId=3
        public ActionResult Product(int? productId)
        {
            if (productId == null)
            {
                return RedirectToAction("SubCategory", "Home");
            }
            else
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var product = db.Products.FirstOrDefault(p => p.Productid == productId);

                if (product == null)
                {
                    return RedirectToAction("SubCategory", "Home");
                }
                else
                {
                    return View(product);
                }
            }
        }


        //localhost:7171/home/cart
        public IActionResult Cart()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

