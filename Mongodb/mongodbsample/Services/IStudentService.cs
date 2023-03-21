using mongodbsample.Models;

namespace mongodbsample.Services
{
    public interface IStudentService
    {
        List<Students> Get();
        Students Get(string id);
        Task<Students> Create(Students student);
        void Update(Students student);
        void Remove(string id);
    }
}
