using DataAccess.Repository.IRepository;
using Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Education_System_MVC_.Areas.Learner.Controllers
{
    [Area("Learner")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _category;
        private readonly ICourseRepository _course;

        public HomeController(ILogger<HomeController> logger,ICategoryRepository category,ICourseRepository course)
        {
            _category = category;
            _logger = logger;
            _course = course;
        }

        public async Task<IActionResult> Index(int? categoryId,int pageSize = 3, int pageNumber = 1)
        {
            List<Category> categories = (await _category.GetAllAsync()).ToList();
            List<Course> courseList = new List<Course>();
            int totalRecords = 0;
            if (categoryId > 0)
            {
                courseList = (await _course.GetAllAsync(c => c.CategoryId == categoryId, includeProperties: "instructor,category,learner_courses", pageSize: pageSize, pageNumber: pageNumber)).ToList();
                totalRecords = (await _course.GetAllAsync(c => c.CategoryId == categoryId)).Count();
            }
            else
            {
                courseList = (await _course.GetAllAsync(includeProperties: "instructor,category,learner_courses", pageSize: pageSize, pageNumber: pageNumber)).ToList();
                totalRecords = (await _course.GetAllAsync()).Count();
            }
            
            int pages = (int)Math.Ceiling((double)totalRecords / pageSize);
            CoursesVM courses = new CoursesVM()
            {
                Data = courseList,
                currentPage = pageNumber,
                pageSize = pageSize,
                totalPages = pages,
                Categories = categories
            };
            return View(courses);
        }

        public IActionResult Privacy()
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
