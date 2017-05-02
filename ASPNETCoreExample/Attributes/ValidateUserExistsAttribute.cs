namespace AspNetCoreExample.Attributes
{
   using System;

   using AspNetCoreExample.Filters;

   using Microsoft.AspNetCore.Mvc;

   /// <summary>
   /// Attribute that validates if the user exists for the given id.
   /// </summary>
   [AttributeUsage(AttributeTargets.Method)]
   public class ValidateUserExistsAttribute : TypeFilterAttribute
   {
      public ValidateUserExistsAttribute()
         : base(typeof(ValidateUserExistsFilter))
      {
      }
   }
}