namespace AspNetCoreExample.Repositories
{
   using System.Threading.Tasks;

   using DataDAL.Tables;

   public interface IUserRepository
   {
      Task<User> GetUserAsync(string id);
   }
}