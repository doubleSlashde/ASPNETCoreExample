namespace ASPNETCoreExample.Dtos {
   using System;
   using System.ComponentModel.DataAnnotations;

   public class FoodDto {
      public int Id {
         get;
         set;
      }

      [Required]
      public string Name {
         get;
         set;
      }

      [Required]
      [Range(0, 1000, ErrorMessage = "Das sind zu viele Kalorien für dich!!")]
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