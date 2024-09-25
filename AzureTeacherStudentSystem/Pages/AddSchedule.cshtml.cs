using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AzureTeacherStudentSystem.Data;
using AzureTeacherStudentSystem.Models;

namespace AzureTeacherStudentSystem.Pages
{
    public class AddScheduleModel : PageModel
    {
        private readonly DataContext _context;

        public AddScheduleModel(DataContext context)
        {
            _context = context;

        }

        public List<Teacher> Teachers { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<Group> Groups { get; set; }

        public IActionResult OnGet()
        {
            Teachers = _context.Teachers.ToList();
            Subjects = _context.Subjects.ToList();
            Groups = _context.Groups.ToList();
            return Page();
        }

        [BindProperty]
        public Schedule Schedule { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int teacher, int subject, int group)
        {
            Schedule.Subject = await _context.Subjects.FindAsync(subject);
            Schedule.Teacher = await _context.Teachers.FindAsync(teacher);
            Schedule.Group = await _context.Groups.FindAsync(group);

            _context.Schedules.Add(Schedule);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
