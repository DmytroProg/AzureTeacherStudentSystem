using Microsoft.AspNetCore.Mvc.RazorPages;
using AzureTeacherStudentSystem.Data;
using AzureTeacherStudentSystem.Models;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzureTeacherStudentSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DataContext _context;

        public IndexModel(DataContext context)
        {
            _context = context;
            Lessons = new List<Lesson>();
            Groups = new List<Group>();
            DateFilter = DateTime.Now;
        }

        public DateTime DateFilter { get; set; }
        public List<Lesson> Lessons { get; set; }
        public List<Group> Groups { get; set; }

        public static int? SelectedGroupId { get; set; } = null;

        public async Task OnGetAsync()
        {
            HttpContext.Session.SetInt32("role", (int)UserRole.Teacher);

            DateFilter = DateTime.Now;
            Groups = _context.Groups.ToList();
            Lessons = SelectedGroupId == null ? _context.Lessons.ToList() 
                : _context.Lessons
                .Include(x => x.Schedule)
                .ThenInclude(x => x.Group)
                .Where(x => x.Schedule.Group.Id == SelectedGroupId)
                .ToList();
        }

        public int GetEmptyDaysCount()
        {
            return (int)new DateTime(DateFilter.Year, DateFilter.Month, 1).DayOfWeek;
        }

        public IActionResult OnPostAsync(int group)
        {
            SelectedGroupId = group;
            return RedirectToPage("./Index");
        }
    }
}
