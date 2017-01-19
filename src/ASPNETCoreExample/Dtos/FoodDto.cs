namespace ASPNETCoreExample.Dtos {
   using System;
   using System.ComponentModel.DataAnnotations;

   using ASPNETCoreExample.Models;

   /// <summary>
   /// Data transfer object of <see cref="FoodItem"/>.
   /// </summary>
   public class FoodDto {
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
      [Required]
      public string Name {
         get;
         set;
      }

      /// <summary>
      /// Calories of the given item.
      /// </summary>
      [Required]
      [Range(0, 1000, ErrorMessage = "Das sind zu viele Kalorien für dich!!")]
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