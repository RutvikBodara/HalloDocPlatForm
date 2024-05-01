using Microsoft.EntityFrameworkCore;
using ProjectManagement.BAL.Interfaces;
using ProjectManagement.BAL.Repository;
using ProjectManagement.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PMSDBContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("PMSDB")));

builder.Services.AddScoped<IProjectRepo, ProjectRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Project}/{action=_ProjectTablePartial}/{id?}");

app.Run();
