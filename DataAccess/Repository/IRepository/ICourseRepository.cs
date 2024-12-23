using Models;

namespace DataAccess.Repository.IRepository
{
    public interface ICourseRepository:IRepository<Course>
    {
        void Update(Course obj);
    }
}
