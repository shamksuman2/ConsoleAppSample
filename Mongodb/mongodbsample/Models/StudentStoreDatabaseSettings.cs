namespace mongodbsample.Models
{
    public class StudentStoreDatabaseSetting : IStudentStoreDatabaseSettings
    {
    
        public string StudentCourseCollectionName { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;
    }
}
