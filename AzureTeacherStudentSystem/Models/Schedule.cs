namespace AzureTeacherStudentSystem.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Subject Subject { get; set; }
        public Group Group { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
    }
}
