using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Demo.PL.MappingProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Program
    {
        // Entry Point
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            #region 1. Configure Services that allow DI - DI Container
            Builder.Services.AddControllersWithViews();
            var conn = Builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine("CONN = " + conn);

            Builder.Services.AddDbContext<MvcDemoDbContext>(Options =>
            {
                Options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
            }); //, ServiceLifetime.Singleton// Allow Dependancy Injection 
            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow Dependancy Injection Class Department Repository
                                                                                       //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            Builder.Services.AddAutoMapper(M => M.AddProfiles(new List<Profile>() { new UserProfile(), new EmployeeProfile(), new RoleProfile() }));

            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<UserManager<AuthUser>>();
            Builder.Services.AddIdentity<AuthUser, IdentityRole>(Options =>
            {
                Options.Password.RequireNonAlphanumeric = true; // @#
                Options.Password.RequireDigit = true; // 1151
                Options.Password.RequireLowercase = true; // aa
                Options.Password.RequireUppercase = true; // AA
            })
                .AddEntityFrameworkStores<MvcDemoDbContext>().AddDefaultTokenProviders();
            Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(Options =>
                {
                    Options.LoginPath = "/Account/Login";
                    Options.AccessDeniedPath = "/Home/Error";
                });
            #endregion

            var app = Builder.Build(); // Kestrel
            // Configure the HTTP request pipeline. 
            #region Configure Http Requests Pipelines Middlewares
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection(); // Redirect HTTP To HTTPS
            app.UseStaticFiles(); // To Send Request For Resources
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
            #endregion
            app.Run();
        }
    }
}
