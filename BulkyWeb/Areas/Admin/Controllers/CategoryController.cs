
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
        
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
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
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
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
            Category? CategoryFromDb = _unitOfWork.Category.Get(u => u.Id==id);
            //Category? CategoryFromDb1 = _db.categories.FirstOrDefault(u=>u.Id==id);
            //Category? CategoryFromDb2 = _db.categories.Where(u=> u.Id==id).FirstOrDefault();
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
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
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
            Category? CategoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
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
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category Successfully Deleted";
            return RedirectToAction("Index");

        
        }

    }
}
