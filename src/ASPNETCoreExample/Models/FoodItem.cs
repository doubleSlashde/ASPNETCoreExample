namespace ASPNETCoreExample.Models {
   using System;

   public class FoodItem {
      public int Id {
         get;
         set;
      }

      public string Name {
         get;
         set;
      }

      public int Calories {
         get;
         set;
      }

      public DateTime Created {
         get;
         set;
      }
   }
}