using mongodbsample.Models;
using MongoDB.Driver;

namespace mongodbsample.Services
{
    public class StudentService : IStudentService
    {
        private readonly IMongoCollection<Students> _students;
        private readonly IStudentStoreDatabaseSettings _settings;
        public StudentService(IStudentStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            _settings = settings;
            var studendDb = mongoClient.GetDatabase(_settings.DatabaseName);
            _students = studendDb.GetCollection<Students>(settings.StudentCourseCollectionName);
        }
        public async Task<Students> Create(Students student)
        {
            await _students.InsertOneAsync(student);
            return student;
        }

        public List<Students> Get()
        {
            return _students.Find<Students>(student => true).ToList();
        }

        public Students Get(string id)
        {
            return _students.Find<Students>(student => student.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _students.DeleteOne(student => student.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Update(Students student)
        {
            _students.ReplaceOne(student => student.Id.Equals(student.Id, StringComparison.InvariantCultureIgnoreCase), student);
        }
    }
}
