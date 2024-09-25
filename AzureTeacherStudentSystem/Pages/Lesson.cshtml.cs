using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AzureTeacherStudentSystem.Data;
using AzureTeacherStudentSystem.Models;

namespace AzureTeacherStudentSystem.Pages
{
    public class LessonModel : PageModel
    {
        private readonly DataContext _context;

        public LessonModel(DataContext context)
        {
            _context = context;
        }

        public Lesson Lesson { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons.FirstOrDefaultAsync(m => m.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }
            else
            {
                Lesson = lesson;
            }
            return Page();
        }
    }
}
