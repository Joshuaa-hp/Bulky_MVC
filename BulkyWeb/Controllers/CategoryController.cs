using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category newCategory)
        {
            //TODO: Probably want better custom validation (i.e. does category already exist)
            //Adding random custom validation
            if(newCategory.Name == newCategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "DisplayOrder cannot match Name");
            }

            if(ModelState.IsValid)
            {
                _db.Categories.Add(newCategory);
                _db.SaveChanges();
                TempData["success"] = "Category Added Successfully.";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            //Category? categoryFromDb = _db.Categories.Find(id);
            Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id==id);
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (categoryFromDb1 == null)
            {
                return NotFound();
            }

            return View(categoryFromDb1);
        }

        [HttpPost]
        public IActionResult Edit(Category newCategory)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(newCategory);
                _db.SaveChanges();
                TempData["success"] = "Category Updated Successfully.";
                return RedirectToAction("Index");
            }

            return View();
        }

        //TODO: This is redundant, yes this is an MVC app but we really don't need a delete view
        //should just be able to use the POST and redirect to list
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);

            if (categoryFromDb1 == null)
            {
                return NotFound();
            }

            return View(categoryFromDb1);
        }

        //TODO: probably want to create a soft delete option??
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if(obj ==null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category Deleted Successfully.";
            return RedirectToAction("Index");
        }
    }
}
