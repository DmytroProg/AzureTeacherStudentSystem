using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AzureTeacherStudentSystem.Data;
using AzureTeacherStudentSystem.Models;
using Azure.Storage.Blobs;
using Azure.Data.Tables;
using AzureTeacherStudentSystem.DTOs;

namespace AzureTeacherStudentSystem.Pages
{
    public class EditLessonModel : PageModel
    {
        private readonly DataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly TableServiceClient _tableServiceClient;

        public EditLessonModel(DataContext context, BlobServiceClient blobServiceClient, TableServiceClient tableServiceClient = null)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
            _tableServiceClient = tableServiceClient;
        }

        [BindProperty]
        public Lesson Lesson { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .Include(x => x.Schedule)
                .ThenInclude(x => x.Group)
                .ThenInclude(x => x.Students)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }
            Lesson = lesson;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile? file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    Lesson.Schedule = await _context.Schedules
                        .Include(x => x.Group)
                        .ThenInclude(x => x.Students)
                        .FirstOrDefaultAsync(x => x.Lessons.Select(x => x.Id).Contains(Lesson.Id));

                    var blobContainer = _blobServiceClient.GetBlobContainerClient("homework-files");
                    await blobContainer.CreateIfNotExistsAsync();   
                    var blobClient = blobContainer.GetBlobClient(file.FileName);

                    using (var stream = file.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, true);
                    }

                    Lesson.Homework = blobClient.Uri.ToString();

                    //await _tableServiceClient.CreateTableIfNotExistsAsync("student_teacher_table");
                    var table = _tableServiceClient.GetTableClient("homeworks");
                    await table.CreateIfNotExistsAsync();

                    foreach(var student in Lesson.Schedule.Group.Students)
                    {
                        await table.AddEntityAsync(new HomeworkEntity()
                        {
                            Topic = Lesson.Topic,
                            StudentId = student.Id,
                            LessonId = Lesson.Id
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonExists(Lesson.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.Id == id);
        }
    }
}
