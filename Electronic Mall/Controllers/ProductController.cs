using Electronic_Mall.Data;
using Electronic_Mall.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Mono.TextTemplating;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.CodeAnalysis;
using System;
using Microsoft.Extensions.Hosting;

namespace Electronic_Mall.Controllers
{
    public class ProductController : Controller
    {

        //localhost:7171/controller/Action in controller

        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment hc)
        {
            this.db = db;
            hostingEnvironment = hc;
        }



        public IActionResult ReadProducts(string searchTerm, int? category, int? minQuantity, decimal? minPrice, decimal? maxPrice)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var categoriesQuery = db.Categories.Include(c => c.Products).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                categoriesQuery = categoriesQuery.Where(c => c.Products.Any(p => p.Name.Contains(searchTerm)));
            }

            if (category.HasValue && category.Value > 0)
            {
                categoriesQuery = categoriesQuery.Where(c => c.Categoryid == category.Value);
            }


            if (minQuantity.HasValue || minPrice.HasValue || maxPrice.HasValue)
            {
                categoriesQuery = categoriesQuery.Select(c => new Category
                {
                    Categoryid = c.Categoryid,
                    Categoryname = c.Categoryname,
                    Photo = c.Photo,
                    Products = c.Products
                        .Where(p => (!minQuantity.HasValue || p.Quantity >= minQuantity.Value) &&
                                    (!minPrice.HasValue || p.Price >= minPrice.Value) &&
                                    (!maxPrice.HasValue || p.Price <= maxPrice.Value))
                        .ToList()
                });
            }

            var model = categoriesQuery.ToList();
            return View(model);
        }

        [HttpGet]
        //localhost:7171/Product/AddProduct
        public IActionResult AddProduct()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var categories = db.Categories.ToList();
            return View(categories);

            

        }
        [HttpPost]
        //await-->async
        public IActionResult AddProduct(ProductViewModel product)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            // product.ProductName,ProductDescription,ProductPrice,ProductQuantity,Photo,ProductCategory
            //if (ModelState.IsValid)
            // {
            string fileName = "";

            if (product.Photo != null)
            {
                //string uploadFolder = Server.MapPath("~/images");
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "assets", "img");
                fileName = Guid.NewGuid().ToString() + "-" + product.Photo.FileName;
                string filePath = Path.Combine(uploadFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.Photo.CopyToAsync(fileStream); // Use async for file operations
                }
            }
            // Add product to database
            Product newProduct = new Product
            {
                Categoryid = product.Categoryid,
                Name = product.ProductName,
                Description = product.ProductDescription,
                Price = product.ProductPrice,
                Quantity = product.ProductQuantity,
                Photo = fileName
            };
            db.Products.AddAsync(newProduct);
            //await db.SaveChangesAsync();
            db.SaveChangesAsync();

            ViewBag.Success = "Record added successfully!";
            return RedirectToAction("ReadProducts"); // Redirect to a success page or product list
                                                     // }
        }

        //localhost:7171/Product/EditProduct
        [HttpGet]
        public IActionResult EditProduct(int productId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var product = db.Products.Find(productId);
            if (product == null) { return RedirectToAction("ReadProducts"); }
            var productViewModel = new ProductViewModel()
            {
                Categoryid = product.Categoryid,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductPrice = product.Price,
                ProductQuantity = product.Quantity,
            };
            ViewData["Productid"] = product.Productid;
            ViewData["Photo"] = product.Photo;

            return View(productViewModel);
            //   return View(db.Products.Find(productId));
        }

        [HttpPost]

        public ActionResult EditProduct(int id, ProductViewModel productVM)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var product = db.Products.Find(id);
            if (product == null) { return RedirectToAction("ReadProducts"); }
            if (!ModelState.IsValid)
            {
                ViewData["Productid"] = product.Productid;
                ViewData["Photo"] = product.Photo;


                // update img file
                string newFileName = product.Photo;
                if (productVM.Photo != null)
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "assets", "img");
                    newFileName = Guid.NewGuid().ToString() + "-" + productVM.Photo.FileName;
                    string imageFullPath = Path.Combine(uploadFolder, newFileName);
                    using (var stream = new FileStream(imageFullPath, FileMode.Create))
                    {
                        productVM.Photo.CopyTo(stream);
                    }

                    // delete old photo
                    string oldImageFullPath = Path.Combine(hostingEnvironment.WebRootPath, "assets", "img", product.Photo);
                    if (System.IO.File.Exists(oldImageFullPath))
                    {
                        System.IO.File.Delete(oldImageFullPath);
                    }

                    //update product in DB
                    product.Categoryid = productVM.Categoryid;
                    product.Name = productVM.ProductName;
                    product.Description = productVM.ProductDescription;
                    product.Price = productVM.ProductPrice;
                    product.Quantity = productVM.ProductQuantity;
                    product.Photo = newFileName;
                    db.SaveChanges();
                    return RedirectToAction("ReadProducts");
                }
            }
            return RedirectToAction("ReadProducts");
        }
        public IActionResult Delete(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var product = db.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("ReadProducts");
            }

            string imageFullPath = Path.Combine(hostingEnvironment.WebRootPath, "assets", "img", product.Photo);


            System.IO.File.Delete(imageFullPath);


            db.Products.Remove(product);
            db.SaveChanges();

            return RedirectToAction("ReadProducts");
        }

        /*
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public ActionResult DeletedSuccess(int id)
        {
            var category = db.Categories.Find(id);
            if (category != null)
            {
                db.Categories.Remove(category);
                try
                {
                    db.SaveChanges();
                    return View();
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception and handle appropriately (e.g., display error message)
                    ModelState.AddModelError("", "Delete failed. See your system administrator.");
                    return View("Delete", category); // Re-render Delete view with error message
                }
            }

            return NotFound(); // Category not found
        }*/

    }
}


