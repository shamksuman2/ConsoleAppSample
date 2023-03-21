namespace mongodbsample.Models
{
    public interface IStudentStoreDatabaseSettings
    {
        string StudentCourseCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
