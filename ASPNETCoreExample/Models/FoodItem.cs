namespace ASPNETCoreExample.Models {
   using System;

   using ASPNETCoreExample.Repositories;

   /// <summary>
   /// Model of <see cref="FoodRepository"/>.
   /// </summary>
   public class FoodItem {
      /// <summary>
      /// Unique id.
      /// </summary>
      public int Id {
         get;
         set;
      }

      /// <summary>
      /// Name of the given item.
      /// </summary>
      public string Name {
         get;
         set;
      }

      /// <summary>
      /// Calories of the given item.
      /// </summary>
      public int Calories {
         get;
         set;
      }

      /// <summary>
      /// Time when the given item is created.
      /// </summary>
      public DateTime Created {
         get;
         set;
      }
   }
}