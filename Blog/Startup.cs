using Blog.Configuration;
using Blog.Data;
using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog;
public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<SmtpSettings>(_config.GetSection("SmtpSettings"));
            
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_config["DefaultConnection"]));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<AppDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
            });
            
            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IFileManager, FileManager>();
            //services.AddSingleton<IEmailService, EmailService>();
            
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.CacheProfiles.Add("Monthly", new CacheProfile {Duration = 60 * 60 * 24 * 7 * 4});
            });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
            }
            
            app.UseDeveloperExceptionPage();
            
            app.UseStaticFiles(); 
            
            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
