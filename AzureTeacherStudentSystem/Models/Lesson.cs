namespace AzureTeacherStudentSystem.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Topic { get; set; }
        public string Homework { get; set; } // Better: class Homework
        public Schedule Schedule { get; set; }
    }
}
