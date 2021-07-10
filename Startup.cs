namespace ProjectsToDoList
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging.AzureAppServices;
    using ProjectsToDoList.DataAccess.Repositories;
    using ProjectsToDoList.DataAccess.TableStorage;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Models;
    using ProjectsToDoList.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 6;
            })
            .AddDefaultTokenProviders();

            services.AddScoped<ICloudStorageAccountHelper, CloudStorageAccountHelper>();
            services.AddScoped<IProjectsRepository, TableStorageProjectsRepository>();
            services.AddScoped<ITasksRepository, TableStorageTasksRepository>();
            services.AddScoped<IUsersRepository, TableStorageUsersRepository>();
            services.AddScoped<IProjectsService, ProjectsService>();
            services.AddScoped<IUserStore<User>, AuthenticationService>();
            services.AddScoped<IRoleStore<Role>, AuthenticationService>();

            services.AddRazorPages();

            services.AddAntiforgery(header => header.HeaderName = "XSRF-TOKEN");

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/ViewOnly/";
                options.AccessDeniedPath = "/ViewOnly/";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
