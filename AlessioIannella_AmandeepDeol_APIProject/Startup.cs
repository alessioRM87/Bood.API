using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using AlessioIannella_AmandeepDeol_APIProject.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using AlessioIannella_AmandeepDeol.API.Models.Context;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Swashbuckle.AspNetCore.Swagger;

namespace AlessioIannella_AmandeepDeol_APIProject
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;

        //}

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });



            services.AddMvc();

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
            });

            services.Add(new ServiceDescriptor(typeof(BooksContext), new BooksContext(Configuration.GetConnectionString("DefaultConnection"))));
            services.Add(new ServiceDescriptor(typeof(UsersContext), new UsersContext(Configuration.GetConnectionString("DefaultConnection"))));
            services.Add(new ServiceDescriptor(typeof(MoodsContext), new MoodsContext(Configuration.GetConnectionString("DefaultConnection"))));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "COMP 306 API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API");
            });
        }
    }
}
