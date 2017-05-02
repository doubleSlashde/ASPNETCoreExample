namespace Tests.RepositoryTests.UserRepositoryTests
{
   using System.Threading.Tasks;

   using DataDAL;
   using DataDAL.Tables;

   public static class UserRepositoryUtils
   {
      public static async Task AddUserAsync(DataDAL context, string id, string name)
      {
         context.User.Add(new User { Id = id, Name = name });
         await context.SaveChangesAsync();
      }
   }
}