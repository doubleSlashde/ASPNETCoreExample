namespace AspNetCoreExample.Repositories
{
   using System.Linq;
   using System.Threading.Tasks;

   using DataDAL;
   using DataDAL.Tables;

   using Microsoft.EntityFrameworkCore;

   public class UserRepository : IUserRepository
   {
      private readonly DataDAL dataDal;

      public UserRepository(DataDAL dataDal)
      {
         this.dataDal = dataDal;
      }

      public async Task<User> GetUserAsync(string id)
      {
         return await this.dataDal.User.Where(item => item.Id == id)
                   .FirstAsync();
      }

      public async Task SetUserAsync(User user)
      {
         var newUser = await this.dataDal.User.AddAsync(user);
         await newUser.Context.SaveChangesAsync();
      }
   }
}