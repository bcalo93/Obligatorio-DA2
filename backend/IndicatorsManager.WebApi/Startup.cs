using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IndicatorsManager.DataAccess;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.BusinessLogic;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.Domain;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;

namespace IndicatorsManager.WebApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddJsonOptions(options => 
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info 
                { 
                    Title = "My API",
                    Version = "v1" 
                });
            });
            
            services.AddDbContext<DbContext, DomainContext>(
                o => o.UseSqlServer(Configuration.GetConnectionString("IndicatorsManagerDbMac"))
            );

            services.AddScoped<ILogic<User>, UserLogic>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IUserQuery, UserRepository>();
            
            services.AddScoped<ILogic<Area>, AreaLogic>();
            services.AddScoped<IRepository<Area>, AreaRepository>();
            services.AddScoped<IAreaQuery, AreaRepository>();
            services.AddScoped<IUserAreaLogic, AreaLogic>();

            services.AddScoped<IIndicatorLogic, IndicatorLogic>();
            services.AddScoped<IRepository<Indicator>, IndicatorRepository>();
            services.AddScoped<IIndicatorQuery, IndicatorRepository>();

            services.AddScoped<IRepository<IndicatorItem>, IndicatorItemRepository>();
            services.AddScoped<IIndicatorItemLogic, IndicatorItemLogic>();
            
            services.AddScoped<IQueryRunner, QueryRunner>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<ISessionLogic, SessionLogic>();
            services.AddScoped<IRepository<Log>, LogRepository>();

            
            services.AddScoped<IReportLogic, ReportLogic>();
            services.AddScoped<ILogQuery, LogRepository>();

            services.AddScoped<IIndicatorImportLogic, IndicatorImportLogic>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors("AllowAllHeaders");
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
