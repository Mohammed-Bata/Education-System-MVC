using Models;

namespace DataAccess.Repository.IRepository
{
    public interface ILearnerCourseRepository:IRepository<Learner_Course>
    {
        void Update(Learner_Course obj);
    }
}
