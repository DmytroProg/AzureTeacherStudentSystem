using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AzureTeacherStudentSystem.Data;
using AzureTeacherStudentSystem.Models;
using System.Diagnostics;
using AzureTeacherStudentSystem.DTOs;

namespace AzureTeacherStudentSystem.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly DataContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(DataContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<StudentDTO> Student { get;set; } = default!;

        public async Task OnGetAsync()
        {
            /*var stopwatch = Stopwatch.StartNew();

            var students = await _cacheService.GetCacheData<IEnumerable<StudentDTO>>("students");

            string method = "";

            if (students == null)
            {
                Student = await _context.Students
                    .Include(s => s.Group)
                    .Select(s => new StudentDTO
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email,
                        GroupName = s.Group.Name,
                    })
                    .ToListAsync();

                method = "database";
                await _cacheService.AddCacheData("students", Student);
            }
            else
            {
                method = "cache";
                Student = new List<StudentDTO>(students);
            }

            stopwatch.Stop();
            _logger.LogInformation($"[GET] {method}: {stopwatch.ElapsedMilliseconds}");*/
            Student = await _context.Students
                    .Include(s => s.Group)
                    .Select(s => new StudentDTO
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email,
                        GroupName = s.Group.Name,
                    })
                    .ToListAsync();
        }
    }
}
