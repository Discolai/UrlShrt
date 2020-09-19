using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UrlShrt.Data;
using UrlShrt.Dtos;
using UrlShrt.Services;

namespace UrlShrt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UrlShrtDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<ISlugConfiguration, SlugConfiguration>();
            services.AddCors();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    return CommonResponse.CreateResponse(modelState: context.ModelState, status: (int)HttpStatusCode.BadRequest);
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UrlShrtDbContext context, ILogger<Startup> logger, Microsoft.Extensions.Hosting.IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var retries = 10;
            var delayMs = 5000;
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    context.Database.Migrate();
                    break;
                }
                catch (Microsoft.Data.SqlClient.SqlException ex)
                {
                    if (i == retries - 1)
                    {
                        logger.LogError(ex, "Migration failed {retries}/{max_retries}. shutting down, {time} UTC", i + 1, retries, DateTime.UtcNow);
                        applicationLifetime.StopApplication();
                    }
                    else
                    {
                        logger.LogWarning("Migration failed on attempt {retries}/{max_retries}. Delaying exection for {delayMs} ms, {time} UTC", i + 1, retries, delayMs, DateTime.UtcNow);
                    }
                }
                Task.Delay(delayMs);

            }
        }
    }
}
