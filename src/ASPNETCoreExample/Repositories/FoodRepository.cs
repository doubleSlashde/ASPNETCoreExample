namespace ASPNETCoreExample.Repositories {
   using System;
   using System.Collections.Concurrent;
   using System.Collections.Generic;
   using System.Linq;

   using Models;

   public class FoodRepository : IFoodRepository {
      private readonly ConcurrentDictionary<int, FoodItem> storage = new ConcurrentDictionary<int, FoodItem>();

      public FoodItem GetSingle(int id) {
         FoodItem item;
         return this.storage.TryGetValue(id, out item) ? item : null;
      }

      public FoodItem Add(FoodItem item) {
         item.Id = !this.GetAll().Any() ? 1 : this.GetAll().Max(value => value.Id) + 1;

         if (this.storage.TryAdd(item.Id, item)) {
            return item;
         }

         throw new Exception("Item could not be added.");
      }

      public void Delete(int id) {
         FoodItem item;

         if (!this.storage.TryRemove(id, out item)) {
            throw new Exception("Item could not be removed.");
         }
      }

      public FoodItem Update(int id, FoodItem item) {
         this.storage.TryUpdate(id, item, this.GetSingle(id));
         return item;
      }

      public ICollection<FoodItem> GetAll() {
         return this.storage.Values;
      }

      public int Count() {
         return this.storage.Count;
      }
   }
}