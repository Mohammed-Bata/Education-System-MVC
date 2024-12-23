using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository
{
    public class LearnerCourseRepository : Repository<Learner_Course>, ILearnerCourseRepository
    {
        private readonly ApplicationDbContext _db;
        public LearnerCourseRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(Learner_Course obj)
        {
            _db.Learners_Courses.Update(obj);
        }
    }
}
