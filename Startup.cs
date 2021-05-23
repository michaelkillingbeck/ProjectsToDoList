namespace ProjectsToDoList
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using ProjectsToDoList.DataAccess;
    using ProjectsToDoList.DataAccess.Repositories;
    using ProjectsToDoList.DataAccess.TableStorage;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICloudStorageAccountHelper, CloudStorageAccountHelper>();
            services.AddScoped<IProjectsRepository, TableStorageProjectsRepository>();
            services.AddScoped<ITasksRepository, TableStorageTasksRepository>();
            services.AddScoped<IProjectsService, ProjectsService>();
            services.AddRazorPages();
            services.AddAntiforgery(header => header.HeaderName = "XSRF-TOKEN");
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
