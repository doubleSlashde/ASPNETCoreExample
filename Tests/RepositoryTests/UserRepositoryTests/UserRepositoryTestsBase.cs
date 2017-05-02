namespace Tests.RepositoryTests.UserRepositoryTests
{
   using AspNetCoreExample.Repositories;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   public class UserRepositoryTestsBase : TestBase
   {
      protected UserRepository RepositoryUnderTest { get; private set; }

      [TestInitialize]
      public override void SetUp()
      {
         base.SetUp();
         this.RepositoryUnderTest = new UserRepository(this.Context);
      }
   }
}