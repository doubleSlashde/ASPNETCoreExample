namespace AspNetCoreExample.Filters
{
   using System.Threading.Tasks;

   using DataDAL;

   using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.Filters;
   using Microsoft.EntityFrameworkCore;

   public class ValidateUserExistsFilter : IAsyncActionFilter
   {
      private const string UserIdKey = "id";

      private readonly DataDAL dataDal;

      public ValidateUserExistsFilter(DataDAL dataDal)
      {
         this.dataDal = dataDal;
      }

      public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
      {
         if (!context.ActionArguments.ContainsKey(UserIdKey))
         {
            context.Result = new BadRequestResult();
            return;
         }

         if (!await this.UserExists(context))
         {
            context.Result = new ForbidResult();
            return;
         }

         await next();
      }

      private async Task<bool> UserExists(ActionExecutingContext context)
      {
         string userId = context.ActionArguments[UserIdKey]
            .ToString();
         return await this.dataDal.User.CountAsync(item => item.Id == userId) > 0;
      }
   }
}