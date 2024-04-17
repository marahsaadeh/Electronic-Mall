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
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        //localhost:7171/Category/ReadCategories
        //get server my vivew
        public IActionResult ReadCategories()
        {
            //Display the data in the table
            return View(db.Categories.ToList());
        }

        [HttpPost]
        public IActionResult AddCategory(string categoryName)
        {
            db.Categories.Add(new Category { Categoryname = categoryName });
            db.SaveChanges();

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






  
