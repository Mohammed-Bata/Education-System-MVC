using DataAccess.Data;
using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _db;
        public CourseRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(Course obj)
        {
            _db.Courses.Update(obj);
        }
    }
}
