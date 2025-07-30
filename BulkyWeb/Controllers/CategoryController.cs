using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
        
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db=db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order cannot Exactly match the Category Name.");
            }
         

            if (ModelState.IsValid)
            {
                _db.categories.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Category Successfully Created";
                return RedirectToAction("Index");   
            }
            return View();
        }
        //new edit functionality
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) 
            {
                return NotFound();
            }
            Category? CategoryFromDb = _db.categories.Find(id);
            Category? CategoryFromDb1 = _db.categories.FirstOrDefault(u=>u.Id==id);
            Category? CategoryFromDb2 = _db.categories.Where(u=> u.Id==id).FirstOrDefault();
            if (CategoryFromDb == null) 
            {
                return NotFound();
            }

            return View(CategoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _db.categories.Update(obj);
                _db.SaveChanges();
                TempData["Success"] = "Category Successfully Updated";  
                return RedirectToAction("Index");
            }
            return View();
        }// add new delete functionality
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? CategoryFromDb = _db.categories.Find(id);
            //Category? CategoryFromDb1 = _db.categories.FirstOrDefault(u => u.Id == id);
            //Category? CategoryFromDb2 = _db.categories.Where(u => u.Id == id).FirstOrDefault();
            if (CategoryFromDb == null)
            {
                return NotFound();
            }

            return View(CategoryFromDb);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.categories.Remove(obj);
            _db.SaveChanges();
            TempData["Success"] = "Category Successfully Deleted";
            return RedirectToAction("Index");

        
        }

    }
}
