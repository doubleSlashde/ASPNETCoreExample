namespace ASPNETCoreExample {
   using System.IO;

   using Dtos;

   using Microsoft.AspNetCore.Builder;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.Extensions.Configuration;
   using Microsoft.Extensions.DependencyInjection;
   using Microsoft.Extensions.Logging;
   using Microsoft.Extensions.PlatformAbstractions;

   using Models;

   using Repositories;

   using Swashbuckle.Swagger.Model;

   /// <summary>
   /// Start class of the API.
   /// </summary>
   public class Startup {
      /// <summary>
      /// Constructor.
      /// </summary>
      /// <param name="env"></param>
      public Startup(IHostingEnvironment env) {
         var builder =
            new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                                      .AddJsonFile("appsettings.json")
                                      .AddEnvironmentVariables();

         this.Configuration = builder.Build();
      }

      /// <summary>
      /// App config.
      /// </summary>
      private IConfiguration Configuration {
         get;
      }

      /// <summary>
      /// This method gets called by the runtime. Use this method to add services to the container.
      /// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
      /// </summary>
      /// <param name="services"></param>
      public void ConfigureServices(IServiceCollection services) {
         services.AddSingleton<IFoodRepository, FoodRepository>();
         services.AddMvc();

         // Inject an implementation of ISwaggerProvider with defaulted settings applied
         services.AddSwaggerGen();
         services.ConfigureSwaggerGen(
                                      options => {
                                         options.SingleApiVersion(this.CreateSwaggerInfo());

                                         // Determine base path for the application.
                                         string basePath = PlatformServices.Default.Application.ApplicationBasePath;

                                         // Set the comments path for the swagger json and ui.
                                         string xmlPath = Path.Combine(basePath, "ASPNETCoreExample.xml");
                                         options.IncludeXmlComments(xmlPath);
                                      });
      }

      /// <summary>
      /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      /// </summary>
      /// <param name="app"></param>
      /// <param name="env"></param>
      /// <param name="loggerFactory"></param>
      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
         loggerFactory.AddConsole();

         if (env.IsDevelopment()) {
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

      private static DefaultFilesOptions CreateDefaultFilesOptions() {
         DefaultFilesOptions options = new DefaultFilesOptions();
         options.DefaultFileNames.Clear();
         options.DefaultFileNames.Add("MyIndex.html");

         return options;
      }

      private static void InitMapper() {
         AutoMapper.Mapper.Initialize(
                                      mapper => {
                                         mapper.CreateMap<FoodItem, FoodDto>().ReverseMap();
                                      });
      }

      private Info CreateSwaggerInfo() {
         return new Info {
                            Version = this.Configuration["version"],
                            Title = this.Configuration["title"],
                            Description = this.Configuration["description"],
                            Contact = new Contact {
                                                     Name = this.Configuration["contact-name"],
                                                     Email = this.Configuration["contact-email"],
                                                     Url = this.Configuration["contact-url"]
                                                  }
                         };
      }
   }
}