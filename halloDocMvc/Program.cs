
using AdminHalloDoc.Repositories.Admin.Repository;
using HalloDoc.BAL.Interface;
using HalloDoc.BAL.Repository;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using hellodocsrsmvc.Models;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

//for excel nuget packet epplus
ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial

// Add services to the container.
builder.Services.AddControllersWithViews();

//contianer for database connection
builder.Services.AddDbContext<HalloDocDBContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("HalloDocDBContext")));

//at run time compilation to update razor pages

builder.Services.AddScoped<IContactRepository, ConctactRepository>(); 
builder.Services.AddScoped<IPatientDashboardRepo, PatientDashboardRepo>(); 
builder.Services.AddScoped<IAdminPartialsRepo, AdminPartialsRepo>();
builder.Services.AddScoped<IJwtAuthInterface, JwtAuthInterface>();
builder.Services.AddScoped<IAgreementRepo, AgreementRepo>();
builder.Services.AddScoped<IScheduleRepo, ScheduleRepo>();
builder.Services.AddScoped<IProviderRepo, ProviderRepo>();
builder.Services.AddScoped<IAssignmentRepo, AssignmentRepo>();

builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(
options => options.IdleTimeout = TimeSpan.FromDays(7)
);

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
app.UseSession();
app.UseRouting();
app.UseRotativa();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
