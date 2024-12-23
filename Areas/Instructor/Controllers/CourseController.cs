using DataAccess.Repository.IRepository;
using Models;
using Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Education_System_MVC_.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class CourseController : Controller
    {
 
        private readonly ICourseRepository _course;
        private readonly ICategoryRepository _category;
        private readonly ILearnerCourseRepository _learnerCourse;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CourseController(ICategoryRepository category,ICourseRepository course,ILearnerCourseRepository learnerCourse , IWebHostEnvironment webHostEnvironment)
        {
            _category = category;
            _course = course;
            _learnerCourse = learnerCourse;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Role_Instructor)]
        public  async Task<IActionResult> Create()
        {
             List<Category> categoryList =  (await _category.GetAllAsync()).ToList();
            ViewBag.SelectList = new SelectList(categoryList, "Id","Name");
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.UserId = userId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Role_Instructor)]
        public async Task<IActionResult> Create(Course obj, IFormFile file)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string imagepath = Path.Combine(wwwRootPath, @"images\Course");

                    using(var filestream = new FileStream(Path.Combine(imagepath, filename),FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    obj.ImageUrl = @"\images\Course\"+filename;
                }
                await _course.AddAsync(obj);
                await _course.SaveAsync();
                TempData["success"] = "Created successfully";

				return RedirectToAction("MyIndex");   
            }
            List<Category> categoryList = (await _category.GetAllAsync()).ToList();
            ViewBag.SelectList = new SelectList(categoryList, "Id", "Name");
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;

            return View();
        }
        [Authorize(Roles = Roles.Role_Admin)]
        public async Task<IActionResult> Index()
        {
            List<Course> courseList = (await _course.GetAllAsync(includeProperties: "instructor,category,learner_courses")).ToList();

            return View(courseList);
        }
        [Authorize(Roles = Roles.Role_Instructor)]
        public async Task<IActionResult> MyIndex(int pageSize = 3, int pageNumber = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Course> courseList = (await _course.GetAllAsync(c=>c.InstructorId==userId,includeProperties: "instructor,category,learner_courses",pageSize:pageSize,pageNumber:pageNumber)).ToList();
            int totalRecords = (await _course.GetAllAsync(c => c.InstructorId == userId)).Count();
            int pages = (int)Math.Ceiling((double)totalRecords / pageSize);
            CoursesVM courses = new CoursesVM()
            {
                Data = courseList,
                currentPage = pageNumber,
                pageSize = pageSize,
                totalPages = pages
            };

            return View(courses);
        }
        [Authorize(Roles = Roles.Role_Instructor + "," + Roles.Role_Admin)]
        public async Task<IActionResult> Details(int id)
        {
            Course coursefromDB = await _course.GetAsync(x => x.Id == id, includeProperties: "instructor,category,learner_courses");
            return View(coursefromDB);
        }
        [HttpGet]
        [Authorize(Roles = Roles.Role_Instructor)]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Course? course = await _course.GetAsync(x => x.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            List<Category> categoryList = (await _category.GetAllAsync()).ToList();
            ViewBag.SelectList = new SelectList(categoryList, "Id", "Name");
        
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Role_Instructor)]
        public async Task<IActionResult> Edit(Course obj,IFormFile? newimage)
        {

            if (ModelState.IsValid)
            {
               
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (newimage != null)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(newimage.FileName);
                        string imagepath = Path.Combine(wwwRootPath, @"images\Course");

                        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                        using (var filestream = new FileStream(Path.Combine(imagepath, filename), FileMode.Create))
                        {
                            newimage.CopyTo(filestream);
                        }
                        obj.ImageUrl = @"\images\Course\" + filename;
                    }
       
                _course.Update(obj);
                await _course.SaveAsync();
				TempData["success"] = "Edit successfully";
				return RedirectToAction("MyIndex");
            }

            return View();
        }
        [HttpGet]
        [Authorize(Roles = Roles.Role_Instructor + "," + Roles.Role_Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Course? courseFromDb = await _course.GetAsync(u => u.Id == id, includeProperties: "instructor,category");

            if (courseFromDb == null)
            {
                return NotFound();
            }
            return View(courseFromDb);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Role_Instructor + "," + Roles.Role_Admin)]
        public async Task<IActionResult> DeletePost(int id)
        {
            Course? obj = await _course.GetAsync(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            List<Learner_Course> range = (await _learnerCourse.GetAllAsync(lc => lc.CourseId == id)).ToList();
            foreach (var course in range)
            {
                _learnerCourse.Remove(course);
            }
            await _learnerCourse.SaveAsync();
            _course.Remove(obj);
            await _course.SaveAsync();
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            TempData["success"] = "Deleted successfully";
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home", new { area = "Learner" });
            }
			return RedirectToAction("MyIndex");
        }
    }
}
