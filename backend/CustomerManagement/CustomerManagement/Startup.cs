using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerManagement.Data;
using CustomerManagement.Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CustomerManagement
{
    public class Startup
    { 
       
            public Startup(IHostingEnvironment env)
            {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                 .AddEnvironmentVariables();
            Configuration = builder.Build();
            }
          

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            string sqlConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<CustomerContext>(options =>
           { 
               options.UseSqlServer(sqlConnectionString);
           });

            services.AddIdentity<AppUser, IdentityRole>()
                  .AddEntityFrameworkStores<CustomerContext>();


            services.AddTransient<DbSeeder>();
            services.AddTransient<UserDbSeeder>();
            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddScoped<IStatesRepository, StatesRepository>();

            services.ConfigureApplicationCookie(config =>
            {
                config.Events =
                  new CookieAuthenticationEvents
                  {
                      OnRedirectToLogin = (ctx) =>
                      {
                          if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                          {
                              ctx.Response.StatusCode = 401;
                          }

                          return Task.CompletedTask;
                      },
                      OnRedirectToAccessDenied = (ctx) =>
                      {
                          if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                          {
                              ctx.Response.StatusCode = 403;
                          }

                          return Task.CompletedTask;
                      }
                  };
            });

            services.AddAuthentication()
                .AddJwtBearer(jwt =>
    {
        jwt.TokenValidationParameters = new TokenValidationParameters()
        {

           
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
            ValidAudience = Configuration["Tokens:Audience"],
            ValidIssuer = Configuration["Tokens:Issuer"],
           
            // When receiving a token, check that it is still valid.
            ValidateLifetime = true,

            // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time
            // when validating the lifetime. As we're creating the tokens locally and validating them on the same
            // machines which should have synchronised time, this can be set to zero. Where external tokens are
            // used, some leeway here could be useful.
            ClockSkew = TimeSpan.FromMinutes(0)
        };
    });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build());
            });

            services.AddCors(config =>
           {
               config.AddPolicy("Ozgul", bldr =>
                {
                     bldr.AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowAnyOrigin();
                });
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env , ILoggerFactory loggerFactory, DbSeeder dbSeeder,UserDbSeeder userSeeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseIdentity();
            app.UseAuthentication();
            //dbSeeder.SeedAsync().Wait();
            userSeeder.SeedUserAndRolesAndClaims().Wait();
            app.UseCors("Ozgul");
            app.UseMvc();
        }
    }
}
