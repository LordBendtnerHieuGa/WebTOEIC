using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Repositories;
using WebToeic.WebAppMVC.Services;
using WebToeic.WebAppMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebToeic.WebAppMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<WebToeicDbContext>();

            builder.Services.AddScoped<GrammarRepository>();
            builder.Services.AddScoped<GrammarService>();

            builder.Services.AddScoped<ListenRepository>();
            builder.Services.AddScoped<ListenService>();

            builder.Services.AddScoped<ExamRepository>();
            builder.Services.AddScoped<ExamService>();

            builder.Services.AddScoped<ReadRepository>();
            builder.Services.AddScoped<ReadService>();

            builder.Services.AddScoped<VocabularyRepository>();
            builder.Services.AddScoped<VocabularyService>();

            builder.Services.AddScoped<UserManager<User>>();
            builder.Services.AddScoped<SignInManager<User>>();

            //builder.Services.AddScoped<UserManager<AccVM>>();

            builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<WebToeicDbContext>().AddDefaultTokenProviders();
            /*builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<WebToeicDbContext>().AddDefaultTokenProviders();
            builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<WebToeicDbContext>()
            .AddDefaultTokenProviders();*/

            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.AccessDeniedPath = "/Account/Login";
                option.LoginPath = "/Account/Login";
                option.LogoutPath = "/Account/Logout";
                option.Cookie.Name = "AuthWebToeicApplication";
                option.ExpireTimeSpan = TimeSpan.FromDays(30);
                option.SlidingExpiration = true;

            });

           

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                  name: "Areas",
                  pattern: "{area:exists}/{controller=AdminHome}/{action=Index}/{id?}");

            app.Run();
        }
    }
}