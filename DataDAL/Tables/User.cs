namespace DataDAL.Tables
{
   using System.ComponentModel.DataAnnotations;
   using System.ComponentModel.DataAnnotations.Schema;

   [Table("User")]
   public class User
   {
      [Key, Required, StringLength(450)]
      public string Id { get; set; }

      [StringLength(20), Required]
      public string Name { get; set; }
   }
}