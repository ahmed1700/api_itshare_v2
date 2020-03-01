using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CompanyEmployees.Extenstions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using System.IO;
using Microsoft.AspNetCore.Internal;
using AutoMapper;
using Contracts;

namespace CompanyEmployees
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerServices();
            services.ConfigureSqlContext(Configuration);
            services.ConfigureRepositroryManager();
            services.AddMvc(config =>
            {
                config.ReturnHttpNotAcceptable = true;
            }).AddXmlDataContractSerializerFormatters().AddCustomCSVFormatter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env , ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.ConfigurExceptionHandler(logger);
            app.UseCors("CorsPloicy");
            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All});
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
