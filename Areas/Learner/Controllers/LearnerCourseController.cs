using DataAccess.Repository.IRepository;
using Models;
using Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Education_System_MVC_.Areas.Learner.Controllers
{
    [Area("Learner")]
    [Authorize(Roles = Roles.Role_Learner)]
    public class LearnerCourseController : Controller
    {
        private readonly ILearnerCourseRepository _learnerCourse;
        private readonly ICourseRepository _course;
        public LearnerCourseController(ILearnerCourseRepository learnerCourse,ICourseRepository course)
        {
            _learnerCourse = learnerCourse;
            _course = course;
        }
        
        public async Task<IActionResult> MyIndex(int pageSize = 3, int pageNumber = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Learner_Course> mycourseList = (await _learnerCourse.GetAllAsync(x => x.LearnerId == userId)).ToList();

            var coursesIds = mycourseList.Select(x => x.CourseId);

            List<Course> mycourses = (await _course.GetAllAsync(c => coursesIds.Contains(c.Id), includeProperties: "instructor,category,learner_courses", pageSize: pageSize, pageNumber: pageNumber)).ToList();

            int totalRecords = mycourseList.Count();
            int pages = (int)Math.Ceiling((double)totalRecords / pageSize);
            CoursesVM courses = new CoursesVM()
            {
                Data = mycourses,
                currentPage = pageNumber,
                pageSize = pageSize,
                totalPages = pages
            };

            return View(courses);
        }
        [HttpGet]
        public async Task<IActionResult> Join(int id)
        {
            Course coursefromDB = await _course.GetAsync(x => x.Id == id, includeProperties: "instructor,category,learner_courses");
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.isJoined = await _learnerCourse.GetAsync(lc => lc.CourseId == id && lc.LearnerId == userId);

            return View(coursefromDB);
        }
        [HttpPost,ActionName("Join")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinPost(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Learner_Course learner_Course = new Learner_Course
            {
                LearnerId = userId,
                CourseId = id
            };
            await _learnerCourse.AddAsync(learner_Course);
            await _learnerCourse.SaveAsync();
			TempData["success"] = "Joined successfully";
			return RedirectToAction("MyIndex");
        }
        [HttpGet]
        public async Task<IActionResult> Leave(int id)
        {
            Course course = await _course.GetAsync(c => c.Id == id, includeProperties: "instructor,category,learner_courses");
            return View(course);
        }
        [HttpPost,ActionName("Leave")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeavePost(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Learner_Course? learner_course = await _learnerCourse.GetAsync(lc => lc.CourseId == id && lc.LearnerId == userId);
            _learnerCourse.Remove(learner_course);
            await _learnerCourse.SaveAsync();
			TempData["success"] = "Left successfully";
			return RedirectToAction("myindex");
        }
    }
}
