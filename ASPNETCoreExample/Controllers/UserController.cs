namespace AspNetCoreExample.Controllers
{
   using System;
   using System.Threading.Tasks;

   using AspNetCoreExample.Repositories;

   using DataDAL.Tables;

   using Microsoft.AspNetCore.Mvc;
   using Microsoft.Extensions.Logging;

   [Route("api/[controller]")]
   public class UserController : Controller
   {
      private readonly ILogger<UserController> logger;

      private readonly IUserRepository repository;

      public UserController(IUserRepository repository, ILogger<UserController> logger)
      {
         this.repository = repository;
         this.logger = logger;
      }

      /// <summary>
      /// Get the user with id.
      /// </summary>
      /// <param name="id">The id of the user to get.</param>
      /// <returns>The user with id.</returns>
      /// <response code="200">The user with id.</response>
      /// <response code="400">If getting the user failed.</response>
      [HttpGet("{id}")]
      [ProducesResponseType(typeof(User), 200)]
      [ProducesResponseType(typeof(string), 400)]
      public async Task<IActionResult> GetUserAsync(string id)
      {
         try
         {
            var result = await this.repository.GetUserAsync(id);
            return this.Ok(result);
         }
         catch (Exception e)
         {
            string errorMessage = $"Error while getting the user: {e.InnerException?.Message ?? e.Message}";
            this.logger.LogError(errorMessage);
            return this.BadRequest(errorMessage);
         }
      }
   }
}