namespace ASPNETCoreExample {
   using System.IO;

   using Microsoft.AspNetCore.Hosting;

   /// <summary>
   /// Main class.
   /// </summary>
   public class Program {
      /// <summary>
      /// Main method.
      /// </summary>
      /// <param name="args"></param>
      public static void Main(string[] args) {
         var host =
            new WebHostBuilder().UseKestrel()
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseIISIntegration()
                                .UseStartup<Startup>()
                                .Build();

         host.Run();
      }
   }
}