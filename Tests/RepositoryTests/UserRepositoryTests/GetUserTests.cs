namespace Tests.RepositoryTests.UserRepositoryTests
{
   using System;
   using System.Threading.Tasks;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class GetUserTests : UserRepositoryTestsBase
   {
      [TestMethod]
      public async Task ShouldReturnSuccessfulWhenUserWithIdAvailable()
      {
         // Arrange
         const string Id = "1";
         const string Name = "Jennifer";
         await UserRepositoryUtils.AddUserAsync(this.Context, Id, Name);

         // Test
         var result = await this.RepositoryUnderTest.GetUserAsync(Id);

         // Assert
         Assert.IsNotNull(result);
         Assert.AreEqual(Id, result.Id);
         Assert.AreEqual(Name, result.Name);
      }

      [TestMethod]
      [ExpectedException(typeof(InvalidOperationException))]
      public async Task ShouldReturnErrorWhenUserWithIdMissing()
      {
         // Arrange
         const string Id = "1";

         // Test
         await this.RepositoryUnderTest.GetUserAsync(Id);
      }
   }
}