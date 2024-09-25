using Azure.Identity;
using AzureTeacherStudentSystem;
using AzureTeacherStudentSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(builder.Configuration["VaultUri"]);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(builder.Configuration["Develop"]));
//builder.Services.AddSingleton(_ => ConnectionMultiplexer.Connect(builder.Configuration["Redis"]).GetDatabase());
//builder.Services.AddTransient<ICacheService, RedisCacheService>();

builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(30);
    opt.Cookie.IsEssential = true;
    opt.Cookie.HttpOnly = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["StorageAccountConnectionString"]);
    clientBuilder.AddTableServiceClient(builder.Configuration["StorageAccountConnectionString"]);
    clientBuilder.AddQueueServiceClient(builder.Configuration["StorageAccountConnectionString"]);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
