namespace AspNetCoreExample.Attributes
{
   using System;

   using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.Filters;

   [AttributeUsage(AttributeTargets.Method)]
   public class ValidateModelAttribute : ActionFilterAttribute
   {
      public override void OnActionExecuting(ActionExecutingContext context)
      {
         if (context.ActionArguments.Values.Contains(null))
         {
            context.Result = new BadRequestResult();
         }

         if (!context.ModelState.IsValid)
         {
            context.Result = new BadRequestObjectResult(context.ModelState);
         }
      }
   }
}