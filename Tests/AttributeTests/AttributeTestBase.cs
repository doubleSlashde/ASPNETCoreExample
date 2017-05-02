namespace Tests.AttributeTests
{
   using System.Collections.Generic;

   using AspNetCoreExample.Controllers;

   using Microsoft.AspNetCore.Http;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.Abstractions;
   using Microsoft.AspNetCore.Mvc.Filters;
   using Microsoft.AspNetCore.Routing;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   using Moq;

   public class AttributeTestBase
   {
      protected UserController Controller { get; private set; }

      [TestInitialize]
      public virtual void SetUp()
      {
         this.Controller = new UserController(null, null);
      }

      protected ActionExecutingContext CreateActionExecutingContext(object controller, IDictionary<string, object> requestParameters)
      {
         var httpContextMock = new Mock<HttpContext>();
         var routeDataMock = new Mock<RouteData>();
         var actionDescriptorMock = new Mock<ActionDescriptor>();
         var actionContext = new ActionContext(httpContextMock.Object, routeDataMock.Object, actionDescriptorMock.Object);

         var filters = new List<IFilterMetadata>();
         return new ActionExecutingContext(actionContext, filters, requestParameters, controller);
      }
   }
}