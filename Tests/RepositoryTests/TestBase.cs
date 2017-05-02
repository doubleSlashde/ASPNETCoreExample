namespace Tests.RepositoryTests
{
   using System;

   using DataDAL;

   using Microsoft.Data.Sqlite;
   using Microsoft.EntityFrameworkCore;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   public class TestBase
   {
      private SqliteConnection connection;

      protected DataDAL Context { get; private set; }

      [TestCleanup]

      // ReSharper disable once UnusedMember.Global, because it's a TestCleanup-method, which must be public.
      public virtual void CleanUp()
      {
         this.connection.Close();
      }

      [TestInitialize]

      // ReSharper disable once MemberCanBeProtected.Global, because TestInitialize-methods must be public.
      public virtual void SetUp()
      {
         this.connection = CreateConnection();
         this.connection.Open();

         this.Context = EnsureDatabaseCreated(this.connection);
      }

      public static SqliteConnection CreateConnection()
      {
         // In-memory database only exists while the connection is open
         var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
         return new SqliteConnection(connectionStringBuilder.ToString());
      }

      public static DataDAL EnsureDatabaseCreated(SqliteConnection connection)
      {
         var contextOptions = new DbContextOptionsBuilder<DataDAL>().UseSqlite(connection)
            .Options;

         // Create the schema in the database if not available
         var context = (DataDAL)Activator.CreateInstance(typeof(DataDAL), contextOptions);
         context.Database.OpenConnection();
         context.Database.EnsureCreated();

         return context;
      }
   }
}