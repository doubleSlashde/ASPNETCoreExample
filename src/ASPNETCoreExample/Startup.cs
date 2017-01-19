namespace ASPNETCoreExample {
   using Dtos;

   using Microsoft.AspNetCore.Builder;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.Extensions.DependencyInjection;
   using Microsoft.Extensions.Logging;

   using Models;

   using Repositories;

   public class Startup {
      // This method gets called by the runtime. Use this method to add services to the container.
      // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
      public void ConfigureServices(IServiceCollection services) {
         services.AddSingleton<IFoodRepository, FoodRepository>();
         services.AddMvc();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
         loggerFactory.AddConsole();

         if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
         }

         app.UseMvc();

         DefaultFilesOptions options = CreateDefaultFilesOptions();
         app.UseDefaultFiles(options);
         app.UseFileServer();

         InitMapper();
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
   }
}