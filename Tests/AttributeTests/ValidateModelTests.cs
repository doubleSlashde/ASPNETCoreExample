namespace Tests.AttributeTests
{
   using System.Collections.Generic;

   using AspNetCoreExample.Attributes;

   using DataDAL.Tables;

   using Microsoft.AspNetCore.Mvc;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class ValidateModelTests : AttributeTestBase
   {
      private const string ModelKey = "Model";

      private ValidateModelAttribute attributeToTest;

      [TestInitialize]
      public override void SetUp()
      {
         base.SetUp();
         this.attributeToTest = new ValidateModelAttribute();
      }

      [TestMethod]
      public void ShouldReturnOkIfModelIsValid()
      {
         // Arrange
         var model = new User();
         var requestParameter = this.GetValidateModelAttributeRequestParameters(model);
         var actionExecutingContext = this.CreateActionExecutingContext(this.Controller, requestParameter);

         // Test
         this.attributeToTest.OnActionExecuting(actionExecutingContext);

         // Assert
         Assert.IsNull(actionExecutingContext.Result);
      }

      [TestMethod]
      public void ShouldReturnBadRequestIfModelIsInvalid()
      {
         // Arrange
         const string InvalidModel = "test";
         var requestParameter = this.GetValidateModelAttributeRequestParameters(InvalidModel);
         var actionExecutingContext = this.CreateActionExecutingContext(this.Controller, requestParameter);
         actionExecutingContext.ModelState.AddModelError(ModelKey, "Invalid Model");

         // Test
         this.attributeToTest.OnActionExecuting(actionExecutingContext);

         // Assert
         Assert.IsNotNull(actionExecutingContext.Result);
         Assert.IsInstanceOfType(actionExecutingContext.Result, typeof(BadRequestObjectResult));
      }

      [TestMethod]
      public void ShouldReturnBadRequestIfModelIsNotAvailable()
      {
         // Arrange
         const string NotAvailableModel = null;
         var requestParameter = this.GetValidateModelAttributeRequestParameters(NotAvailableModel);
         var actionExecutingContext = this.CreateActionExecutingContext(this.Controller, requestParameter);

         // Test
         this.attributeToTest.OnActionExecuting(actionExecutingContext);

         // Assert
         Assert.IsNotNull(actionExecutingContext.Result);
         Assert.IsInstanceOfType(actionExecutingContext.Result, typeof(BadRequestResult));
      }

      private IDictionary<string, object> GetValidateModelAttributeRequestParameters(object model)
      {
         return new Dictionary<string, object> { { ModelKey, model } };
      }
   }
}