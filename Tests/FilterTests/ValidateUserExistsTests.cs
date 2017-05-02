namespace Tests.FilterTests
{
   using System.Collections.Generic;
   using System.Threading.Tasks;

   using AspNetCoreExample.Filters;

   using Microsoft.AspNetCore.Mvc;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   using Tests.RepositoryTests.UserRepositoryTests;

   [TestClass]
   public class ValidateUserExistsTests : FilterTestBase
   {
      private const string IdKey = "id";

      private ValidateUserExistsFilter filterToTest;

      [TestInitialize]
      public override void SetUp()
      {
         base.SetUp();
         this.filterToTest = new ValidateUserExistsFilter(this.Context);
      }

      [TestMethod]
      public async Task ShouldReturnOkIfUserExists()
      {
         // Arrange
         const string Id = "1";
         const string Name = "Jennifer";
         await UserRepositoryUtils.AddUserAsync(this.Context, Id, Name);

         var requestParameter = this.GetRequestParameters(Id);
         var actionExecutingContext = this.CreateActionExecutingContext(this.Controller, requestParameter);

         // Test
         await this.filterToTest.OnActionExecutionAsync(actionExecutingContext, this.DelegateMock);

         // Assert
         Assert.IsNull(actionExecutingContext.Result);
      }

      [TestMethod]
      public async Task ShouldReturnBadRequestIfIdMissing()
      {
         // Arrange
         var requestParameter = new Dictionary<string, object>();
         var actionExecutingContext = this.CreateActionExecutingContext(this.Controller, requestParameter);

         // Test
         await this.filterToTest.OnActionExecutionAsync(actionExecutingContext, this.DelegateMock);

         // Assert
         Assert.IsNotNull(actionExecutingContext.Result);
         Assert.IsInstanceOfType(actionExecutingContext.Result, typeof(BadRequestResult));
      }

      [TestMethod]
      public async Task ShouldReturnFirbidIfUserNotExists()
      {
         // Arrange
         const string IdOfNotExistingUser = "1";
         var requestParameter = this.GetRequestParameters(IdOfNotExistingUser);
         var actionExecutingContext = this.CreateActionExecutingContext(this.Controller, requestParameter);

         // Test
         await this.filterToTest.OnActionExecutionAsync(actionExecutingContext, this.DelegateMock);

         // Assert
         Assert.IsNotNull(actionExecutingContext.Result);
         Assert.IsInstanceOfType(actionExecutingContext.Result, typeof(ForbidResult));
      }

      private IDictionary<string, object> GetRequestParameters(object id)
      {
         return new Dictionary<string, object> { { IdKey, id } };
      }
   }
}