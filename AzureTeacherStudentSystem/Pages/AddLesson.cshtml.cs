using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AzureTeacherStudentSystem.Data;
using AzureTeacherStudentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureTeacherStudentSystem.Pages
{
    public class AddLessonModel : PageModel
    {
        private readonly DataContext _context;

        public AddLessonModel(DataContext context)
        {
            _context = context;
        }

        public List<Teacher> Teachers { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<Group> Groups { get; set; }

        public IActionResult OnGet()
        {
            Teachers = _context.Schedules.GroupBy(x => x.Teacher).Select(x => x.Key).ToList();
            Subjects = _context.Schedules.GroupBy(x => x.Subject).Select(x => x.Key).ToList();
            Groups = _context.Schedules.GroupBy(x => x.Group).Select(x => x.Key).ToList();
            return Page();
        }

        [BindProperty]
        public Lesson Lesson { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int teacher, int subject, int group)
        {
            Lesson.Schedule = await _context.Schedules.FirstOrDefaultAsync(x => x.Teacher.Id == teacher &&
            x.Subject.Id == subject && x.Group.Id == group);
            Lesson.Homework = "";

            _context.Lessons.Add(Lesson);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
