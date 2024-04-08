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
<<<<<<< HEAD

        //localhost:7171/Home/SubCategory?subCategoryId={id}

=======
        //localhost:7171/Home/SubCategory?subCategoryId={id}
>>>>>>> 2965ca98be0b149daf1e0d1a8ab8c08b58993e45
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

