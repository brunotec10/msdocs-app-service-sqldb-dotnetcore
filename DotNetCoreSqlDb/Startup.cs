﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Models;

namespace DotNetCoreSqlDb
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
            services.AddControllersWithViews();
            services.AddDbContext<MyDatabaseContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            #region csp
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; style-src 'self' https://stackpath.bootstrapcdn.com/; script-src 'Self' 'sha256-uprgzvaZUDGYSlu68ZReh5GYCM96KruqsQItjnN8RZ0=' 'sha256-B5zLS6+agtge/Sk/CeMAp+Fmq/V+Nnk8XRBQz9Eg7Xk=' 'sha256-wWUaDZXboWpnFzhzBk5bDowyUJgmKfIi72KineAaGso=' https://stackpath.bootstrapcdn.com/ https://cdnjs.cloudflare.com; frame-ancestors 'none'; form-action 'self'");
                await next();
            });
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Todos}/{action=Index}/{id?}");
            });
        }
    }
}
