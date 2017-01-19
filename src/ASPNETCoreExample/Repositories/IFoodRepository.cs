namespace ASPNETCoreExample.Repositories {
   using System.Collections.Generic;

   using Models;

   /// <summary>
   /// Interface of Repository for <see cref="FoodItem"/>.
   /// </summary>
   public interface IFoodRepository {
      /// <summary>
      /// Get a single <see cref="FoodItem"/> with given <paramref name="id"/>.
      /// </summary>
      /// <param name="id">Id of the <see cref="FoodItem"/> to get.</param>
      /// <returns><see cref="FoodItem"/> with given <paramref name="id"/></returns>
      FoodItem GetSingle(int id);

      /// <summary>
      /// Add given <see cref="FoodItem"/>.
      /// </summary>
      /// <param name="item"><see cref="FoodItem"/> to add.</param>
      /// <returns>Added <see cref="FoodItem"/>.</returns>
      FoodItem Add(FoodItem item);

      /// <summary>
      /// Delete <see cref="FoodItem"/> with given <paramref name="id"/>.
      /// </summary>
      /// <param name="id">Id of the <see cref="FoodItem"/> to delete.</param>
      void Delete(int id);

      /// <summary>
      /// Update the values of given <paramref name="item"/> for <see cref="FoodItem"/> with given <paramref name="id"/>.
      /// </summary>
      /// <param name="id">Id of the <see cref="FoodItem"/> to update.</param>
      /// <param name="item">New values of the <see cref="FoodItem"/> to update.</param>
      /// <returns>Updated <see cref="FoodItem"/>.</returns>
      FoodItem Update(int id, FoodItem item);

      /// <summary>
      /// Get all available <see cref="FoodItem"/>.
      /// </summary>
      /// <returns>Collection of <see cref="FoodItem"/>.</returns>
      IEnumerable<FoodItem> GetAll();

      /// <summary>
      /// Count of <see cref="FoodItem"/> available within the storage.
      /// </summary>
      /// <returns>Count of <see cref="FoodItem"/> available within the storage.</returns>
      int Count();
   }
}