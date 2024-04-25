using Electronic_Mall.Data;
using Electronic_Mall.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Electronic_Mall.Controllers
{
    public class CategoryController : Controller
    {
        //localhost:7171/controller/Action in controller

        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ApplicationDbContext db;

        public CategoryController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext db)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.db = db;
        }    

        [HttpGet]
        //localhost:7171/Category/ReadCategories
        //get server my vivew
        public IActionResult ReadCategories()
        {
            //Display the data in the table
            return View(db.Categories.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string fileName = "";

            if (model.Photo != null)
            {
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "assets", "img"); 
              
                fileName = Guid.NewGuid().ToString() + "-" + model.Photo.FileName;
                string filePath = Path.Combine(uploadFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(fileStream);
                }
            }

            Category newCategory = new Category
            {
                Categoryname = model.CategoryName,
                Photo = fileName
            };

            await db.Categories.AddAsync(newCategory);
            await db.SaveChangesAsync();

            return RedirectToAction("ReadCategories"); 
        }

        //localhost:7171/Category/EditCategory
        [HttpGet]
        public IActionResult EditCategory(int categoryId)
        {
            return View(db.Categories.Find(categoryId));
        }
        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                //                         System.Data.Entity
                db.Entry(category).State = EntityState.Modified;

                db.SaveChanges();
   return RedirectToAction("ReadCategories");
            }
            else
            {
            
                return View(category);
            }
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category =db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public ActionResult DeletedSuccess(int id)
        {
            var category =db.Categories.Find(id);
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
        }

      
       




    }
}






  
