namespace ASPNETCoreExample.Repositories {
   using System;
   using System.Collections.Concurrent;
   using System.Collections.Generic;
   using System.Linq;

   using Models;

   /// <summary>
   /// Repository of <see cref="FoodItem"/>.
   /// </summary>
   public class FoodRepository : IFoodRepository {
      private readonly ConcurrentDictionary<int, FoodItem> storage = new ConcurrentDictionary<int, FoodItem>();

      /// <summary>
      /// Get a single <see cref="FoodItem"/> with given <paramref name="id"/>.
      /// </summary>
      /// <param name="id">Id of the <see cref="FoodItem"/> to get.</param>
      /// <returns><see cref="FoodItem"/> with given <paramref name="id"/></returns>
      public FoodItem GetSingle(int id) {
         FoodItem item;
         return this.storage.TryGetValue(id, out item) ? item : null;
      }

      /// <summary>
      /// Add given <see cref="FoodItem"/>.
      /// </summary>
      /// <param name="item"><see cref="FoodItem"/> to add.</param>
      /// <returns>Added <see cref="FoodItem"/>.</returns>
      /// <exception cref="Exception"></exception>
      public FoodItem Add(FoodItem item) {
         item.Id = !this.GetAll().Any() ? 1 : this.GetAll().Max(value => value.Id) + 1;

         if (this.storage.TryAdd(item.Id, item)) {
            return item;
         }

         throw new Exception("Item could not be added.");
      }

      /// <summary>
      /// Delete <see cref="FoodItem"/> with given <paramref name="id"/>.
      /// </summary>
      /// <param name="id">Id of the <see cref="FoodItem"/> to delete.</param>
      /// <exception cref="Exception"></exception>
      public void Delete(int id) {
         FoodItem item;

         if (!this.storage.TryRemove(id, out item)) {
            throw new Exception("Item could not be removed.");
         }
      }

      /// <summary>
      /// Update the values of given <paramref name="item"/> for <see cref="FoodItem"/> with given <paramref name="id"/>.
      /// </summary>
      /// <param name="id">Id of the <see cref="FoodItem"/> to update.</param>
      /// <param name="item">New values of the <see cref="FoodItem"/> to update.</param>
      /// <returns>Updated <see cref="FoodItem"/>.</returns>
      public FoodItem Update(int id, FoodItem item) {
         this.storage.TryUpdate(id, item, this.GetSingle(id));
         return item;
      }

      /// <summary>
      /// Get all available <see cref="FoodItem"/>.
      /// </summary>
      /// <returns>Collection of <see cref="FoodItem"/>.</returns>
      public ICollection<FoodItem> GetAll() {
         return this.storage.Values;
      }

      /// <summary>
      /// Count of <see cref="FoodItem"/> available within the storage.
      /// </summary>
      /// <returns>Count of <see cref="FoodItem"/> available within the storage.</returns>
      public int Count() {
         return this.storage.Count;
      }
   }
}