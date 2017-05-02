namespace DataDAL
{
   using global::DataDAL.Tables;

   using Microsoft.EntityFrameworkCore;

   public class DataDAL : DbContext
   {
      public DataDAL(DbContextOptions<DataDAL> options)
         : base(options)
      {
      }

      public DbSet<User> User { get; set; }
   }
}