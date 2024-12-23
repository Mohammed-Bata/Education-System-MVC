using DataAccess.Repository.IRepository;
using Models;
using Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Education_System_MVC_.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _category;
        public CategoryController(ICategoryRepository category)
        {
            _category = category;
        }
        [Authorize(Roles = Roles.Role_Instructor + "," + Roles.Role_Admin)]
        public async Task<IActionResult> Index()
        {
            List<Category> categoryList = (await _category.GetAllAsync()).ToList();
            return View(categoryList);
        }
        [HttpGet]
        [Authorize(Roles = Roles.Role_Admin)]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Role_Admin)]
        public async Task<IActionResult> Create(Category obj)
        {
            Category? cat = await _category.GetAsync(x => x.Name == obj.Name);
            if (cat != null)
            {
                ModelState.AddModelError("Name", "Category is already exist");
            }

            if (ModelState.IsValid)
            {
                await _category.AddAsync(obj);
                await _category.SaveAsync();
                TempData["success"] = "Created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        [Authorize(Roles = Roles.Role_Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = await _category.GetAsync(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Role_Admin)]
        public async Task<IActionResult> DeletePost(int id)
        {
            Category? obj = await _category.GetAsync(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _category.Remove(obj);
            await _category.SaveAsync();
            TempData["success"] = "Deleted successfully";
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = Roles.Role_Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = await _category.GetAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Role_Admin)]
        public async Task<IActionResult> Edit(Category obj)
        {
            Category? cat = await _category.GetAsync(x => x.Name == obj.Name);
            if (cat != null)
            {
                ModelState.AddModelError("Name", "Category is already exist");
            }
            if (ModelState.IsValid)
            {
                _category.Update(obj);
                await _category.SaveAsync();
                TempData["success"] = "Edited successfully";
                return RedirectToAction("index");
            }

            return View();
        }
    }
}
