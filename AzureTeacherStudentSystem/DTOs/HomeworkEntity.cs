using Azure;
using Azure.Data.Tables;

namespace AzureTeacherStudentSystem.DTOs
{
    public class HomeworkEntity : ITableEntity
    {
        public HomeworkEntity()
        {
            PartitionKey = nameof(HomeworkEntity);
            RowKey = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            ETag = new();
        }

        public int StudentId { get; set; }
        public string Topic { get; set; }
        public int LessonId { get; set; }
        public bool IsComplete { get; set; }
        public int Grade { get; set; }


        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
