using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataStore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace secAPI
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

            services.AddDbContext<Context>();
            services.AddTransient<UserInitializer>();
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<Context>();

            services.AddCors(opt =>
            {
                opt.AddPolicy("someOrigins",
                    b =>
                    {
                        b.AllowAnyOrigin();
                        b.AllowAnyMethod();
                        b.AllowAnyHeader();
                        b.AllowCredentials();
                    });
                //opt.
            });
            //services.Configure<IdentityOptions>(cfg => cfg.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents {
            //    OnRedirectToLogin = (ctx) =>
            //    {
            //        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
            //            ctx.Response.StatusCode = 401;

            //        return Task.CompletedTask;
            //    },
            //    OnRedirectToAccessDenied = (ctx) =>
            //    {
            //        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
            //            ctx.Response.StatusCode = 403;

            //        return Task.CompletedTask;
            //    },
            //});

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["tokens:issuer"],
                        ValidAudience = Configuration["tokens:audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["tokens:key"])),
                        ClockSkew = TimeSpan.Zero, // remove delay of token when expire
                        ValidateLifetime = true,
                    };
                });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserInitializer userInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseIdentity();

            app.UseAuthentication();

            app.UseCors("someOrigins");

            app.UseHttpsRedirection();
            app.UseMvc();

            userInitializer.Seed().Wait();
        }
    }
}
