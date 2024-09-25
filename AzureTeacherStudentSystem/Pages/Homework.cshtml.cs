using Azure.Data.Tables;
using AzureTeacherStudentSystem.Data;
using AzureTeacherStudentSystem.DTOs;
using AzureTeacherStudentSystem.Migrations;
using AzureTeacherStudentSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AzureTeacherStudentSystem.Pages
{
    public class HomeworkModel : PageModel
    {
        private readonly TableServiceClient _tableServiceClient;

        public HomeworkModel(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
            Homeworks = new List<HomeworkEntity>();
        }

        public List<HomeworkEntity> Homeworks { get; set; }

        public async Task OnGetAsync()
        {
            int studentId = 1;
            var table = _tableServiceClient.GetTableClient("homeworks");
            Homeworks = table.Query<HomeworkEntity>()
                .AsEnumerable()
                //.Where(x => x.StudentId == studentId)
                .ToList();
        }
    }
}
