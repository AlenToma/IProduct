//using AspNetCoreCustomUserManager.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OAuth.Security.Interface;
using System;

namespace OAuth.Security
{
    public partial class Startup
    {
        private IConfigurationRoot configuration;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
              .SetBasePath(hostingEnvironment.ContentRootPath);

            this.configuration = configurationBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.ExpireTimeSpan = TimeSpan.FromDays(7);
              }
              );

            services.AddScoped<IUserManager, UserManager>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (hostingEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
                //applicationBuilder.UseDatabaseErrorPage(); this is for ef
                applicationBuilder.UseBrowserLink();
            }

            applicationBuilder.UseAuthentication();
            applicationBuilder.UseStaticFiles();
            applicationBuilder.UseMvcWithDefaultRoute();
        }
    }
}