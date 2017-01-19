namespace ASPNETCoreExample.Controllers {
   using System.Collections.Generic;
   using System.Linq;

   using Dtos;

   using Microsoft.AspNetCore.JsonPatch;
   using Microsoft.AspNetCore.Mvc;

   using Models;

   using Repositories;

   [Route("api/[controller]")]
   public class FoodController : Controller {
      private readonly IFoodRepository foodRepository;

      public FoodController(IFoodRepository foodRepository) {
         this.foodRepository = foodRepository;
      }

      [HttpGet]
      public IActionResult GetAllFoodItems() {
         ICollection<FoodItem> foodItems = this.foodRepository.GetAll();
         IEnumerable<FoodDto> mappedItems = foodItems.Select(AutoMapper.Mapper.Map<FoodDto>);

         return this.Ok(mappedItems);
      }

      [HttpGet("{id:int}", Name = "GetSingleFoodItem")]
      public IActionResult GetSingleFoodItem(int id) {
         FoodItem foodItem = this.foodRepository.GetSingle(id);

         if (foodItem == null) {
            return this.NotFound();
         }

         return this.Ok(AutoMapper.Mapper.Map<FoodItem>(foodItem));
      }

      [HttpPost]
      public IActionResult AddNewFoodItem([FromBody] FoodDto foodDto) {
         if (foodDto == null) {
            return this.BadRequest();
         }

         if (!this.ModelState.IsValid) {
            return this.BadRequest(this.ModelState);
         }

         FoodItem foodItem = this.foodRepository.Add(AutoMapper.Mapper.Map<FoodItem>(foodDto));

         return this.CreatedAtRoute(
                                    "GetSingleFoodItem",
                                    new {
                                           id = foodItem.Id
                                        },
                                    AutoMapper.Mapper.Map<FoodDto>(foodItem));
      }

      [HttpPut("{id:int}")]
      public IActionResult UpdateFoodItem(int id, [FromBody] FoodDto foodDto) {
         var foodItemToCheck = this.foodRepository.GetSingle(id);
         if (foodItemToCheck == null) {
            return this.NotFound();
         }

         if (id != foodDto.Id) {
            return this.BadRequest("Ids do not match!");
         }

         if (!this.ModelState.IsValid) {
            return this.BadRequest(this.ModelState);
         }

         FoodItem foodItem = this.foodRepository.Update(id, AutoMapper.Mapper.Map<FoodItem>(foodDto));
         return this.Ok(AutoMapper.Mapper.Map<FoodDto>(foodItem));
      }

      [HttpPatch("{id:int}")]
      public IActionResult PartialUpdate(int id, [FromBody] JsonPatchDocument<FoodDto> foodDtoPatchDoc) {
         if (foodDtoPatchDoc == null) {
            return this.BadRequest();
         }

         var foodItemExistingEntity = this.foodRepository.GetSingle(id);
         if (foodItemExistingEntity == null) {
            return this.NotFound();
         }

         FoodDto foodDto = AutoMapper.Mapper.Map<FoodDto>(foodItemExistingEntity);
         foodDtoPatchDoc.ApplyTo(foodDto, this.ModelState);

         if (!this.ModelState.IsValid) {
            return this.BadRequest(this.ModelState);
         }

         FoodItem foodItem = this.foodRepository.Update(id, AutoMapper.Mapper.Map<FoodItem>(foodDto));
         return this.Ok(AutoMapper.Mapper.Map<FoodDto>(foodItem));
      }

      [HttpDelete("{id:int}")]
      public IActionResult Remove(int id) {
         var foodItem = this.foodRepository.GetSingle(id);
         if (foodItem == null) {
            return this.NotFound();
         }

         this.foodRepository.Delete(id);

         return this.NoContent();
      }
   }
}