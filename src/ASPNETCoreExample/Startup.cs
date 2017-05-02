namespace ASPNETCoreExample
{
   using System;
   using System.IO;

   using AspNetCoreExample.Repositories;

   using ASPNETCoreExample.Dtos;
   using ASPNETCoreExample.Models;
   using ASPNETCoreExample.Repositories;

   using DataDAL;

   using Microsoft.AspNetCore.Builder;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.EntityFrameworkCore;
   using Microsoft.EntityFrameworkCore.Infrastructure;
   using Microsoft.Extensions.Configuration;
   using Microsoft.Extensions.DependencyInjection;
   using Microsoft.Extensions.Logging;

   using Serilog;

   using Swashbuckle.Swagger.Model;

   public class Startup
   {
      public Startup(IHostingEnvironment env)
      {
         var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
            .AddEnvironmentVariables();
         this.Configuration = builder.Build();

         Log.Logger = new LoggerConfiguration().WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "logs/{Date}.txt"))
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .CreateLogger();
      }

      private IConfiguration Configuration { get; }

      public void ConfigureServices(IServiceCollection services)
      {
         // Add framework services
         services.AddMvc();

         // Inject an implementation of ISwaggerProvider with defaulted settings applied
         services.AddSwaggerGen();
         services.AddSwaggerGen(options =>
            {
               options.SingleApiVersion(this.CreateSwaggerInfo());
               options.IncludeXmlComments($"{AppContext.BaseDirectory}\\ASPNETCoreExample.xml");
            });

         // Add database connection
         services.AddEntityFramework();
         services.AddDbContext<DataDAL>(o => o.UseSqlServer(this.Configuration["Data:ConnectionString"]));

         // Add repositories
         services.AddTransient<IFoodRepository, FoodRepository>();
         services.AddTransient<IUserRepository, UserRepository>();
      }

      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
      {
         loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
         loggerFactory.AddDebug();
         loggerFactory.AddSerilog();

         if (env.IsDevelopment())
         {
            // Show info about an exception if one occurs only in development environment
            app.UseDeveloperExceptionPage();
         }

         app.UseMvc();

         DefaultFilesOptions options = CreateDefaultFilesOptions();
         app.UseDefaultFiles(options);
         app.UseFileServer();

         InitMapper();

         // Enable middleware to use swagger ui
         app.UseSwagger();
         app.UseSwaggerUi();
      }

      private static DefaultFilesOptions CreateDefaultFilesOptions()
      {
         DefaultFilesOptions options = new DefaultFilesOptions();
         options.DefaultFileNames.Clear();
         options.DefaultFileNames.Add("MyIndex.html");

         return options;
      }

      private static void InitMapper()
      {
         AutoMapper.Mapper.Initialize(mapper =>
            {
               mapper.CreateMap<FoodItem, FoodDto>()
                  .ReverseMap();
            });
      }

      private Info CreateSwaggerInfo()
      {
         return new Info
                {
                   Version = this.Configuration["version"],
                   Title = this.Configuration["title"],
                   Description = this.Configuration["description"],
                   Contact = new Contact
                             {
                                Name = this.Configuration["contact-name"],
                                Email = this.Configuration["contact-email"],
                                Url = this.Configuration["contact-url"]
                             }
                };
      }
   }
}