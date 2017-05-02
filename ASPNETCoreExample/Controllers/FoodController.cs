namespace ASPNETCoreExample.Controllers
{
   using System.Collections.Generic;
   using System.Linq;

   using ASPNETCoreExample.Dtos;
   using ASPNETCoreExample.Models;
   using ASPNETCoreExample.Repositories;

   using Microsoft.AspNetCore.JsonPatch;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.Extensions.Logging;

   [Route("api/[controller]")]
   public class FoodController : Controller
   {
      private readonly IFoodRepository foodRepository;

      private readonly ILogger logger;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="foodRepository">Repository of <see cref="FoodItem"/>.</param>
      /// <param name="logger">Logger of this class.</param>
      public FoodController(IFoodRepository foodRepository, ILogger<FoodController> logger)
      {
         this.foodRepository = foodRepository;
         this.logger = logger;
      }

      /// <summary>
      /// Get all items of food.
      /// </summary>
      /// <returns>All items of food.</returns>
      /// <response code="200">Returns all items of food.</response>
      [HttpGet]
      [ProducesResponseType(typeof(IEnumerable<FoodDto>), 200)]
      public IActionResult GetAllFoodItems()
      {
         IEnumerable<FoodItem> foodItems = this.foodRepository.GetAll();
         IEnumerable<FoodDto> mappedItems = foodItems.Select(AutoMapper.Mapper.Map<FoodDto>);

         return this.Ok(mappedItems);
      }

      /// <summary>
      /// Get a single food item of given <paramref name="id"/>.
      /// </summary>
      /// <param name="id">Id of the <see cref="FoodItem"/> to get.</param>
      /// <returns><see cref="FoodItem"/> of given <paramref name="id"/>.</returns>
      /// <response code="404">If the item cannot be found.</response>
      /// <response code="200">Returns the single food item.</response>
      [HttpGet("{id:int}", Name = "GetSingleFoodItem")]
      [ProducesResponseType(typeof(FoodDto), 404)]
      [ProducesResponseType(typeof(FoodDto), 200)]
      public IActionResult GetSingleFoodItem(int id)
      {
         FoodItem foodItem = this.foodRepository.GetSingle(id);

         if (foodItem == null)
         {
            this.logger.LogWarning("Item with {ID} not found", id);
            return this.NotFound();
         }

         return this.Ok(AutoMapper.Mapper.Map<FoodDto>(foodItem));
      }

      /// <summary>
      /// Add a new food item.
      /// </summary>
      /// <param name="foodDto">Mapped <see cref="FoodItem"/> to add.</param>
      /// <returns>Link to get the food item, which was added and also the added <see cref="FoodDto"/> object.</returns>
      /// <response code="400">If the given food item is null or invalid.</response>
      /// <response code="201">Returns the created food item.</response>
      [HttpPost]
      [ProducesResponseType(typeof(FoodDto), 400)]
      [ProducesResponseType(typeof(FoodDto), 201)]
      public IActionResult AddNewFoodItem([FromBody] FoodDto foodDto)
      {
         if (foodDto == null)
         {
            return this.BadRequest();
         }

         if (!this.ModelState.IsValid)
         {
            return this.BadRequest(this.ModelState);
         }

         FoodItem foodItem = this.foodRepository.Add(AutoMapper.Mapper.Map<FoodItem>(foodDto));

         return this.CreatedAtRoute("GetSingleFoodItem", new { id = foodItem.Id }, AutoMapper.Mapper.Map<FoodDto>(foodItem));
      }

      /// <summary>
      /// Update the given food item with the food item of the given <paramref name="id"/>.
      /// </summary>
      /// <param name="id">Id of the <see cref="FoodItem"/> to update.</param>
      /// <param name="foodDto">New values of the <see cref="FoodItem"/> to update.</param>
      /// <returns>Updated <see cref="FoodDto"/> object.</returns>
      /// <response code="404">If the given food item cannot be found.</response>
      /// <response code="400">If the given id doesn't match the id of the given food item or the given food item is invalid.</response>
      /// <response code="200">Returns the updated food item.</response>
      [HttpPut("{id:int}")]
      [ProducesResponseType(typeof(FoodDto), 404)]
      [ProducesResponseType(typeof(FoodDto), 400)]
      [ProducesResponseType(typeof(FoodDto), 200)]
      public IActionResult UpdateFoodItem(int id, [FromBody] FoodDto foodDto)
      {
         var foodItemToCheck = this.foodRepository.GetSingle(id);
         if (foodItemToCheck == null)
         {
            return this.NotFound();
         }

         if (id != foodDto.Id)
         {
            return this.BadRequest("Ids do not match!");
         }

         if (!this.ModelState.IsValid)
         {
            return this.BadRequest(this.ModelState);
         }

         FoodItem foodItem = this.foodRepository.Update(id, AutoMapper.Mapper.Map<FoodItem>(foodDto));
         return this.Ok(AutoMapper.Mapper.Map<FoodDto>(foodItem));
      }

      /// <summary>
      /// Partial update the given food data with the food item of the given <paramref name="id"/>.
      /// </summary>
      /// <param name="id">Id of the <see cref="FoodItem"/> to partial update.</param>
      /// <param name="foodDtoPatchDoc">New values of the <see cref="FoodItem"/> to partial update.</param>
      /// <returns>Updated <see cref="FoodDto"/> object.</returns>
      /// <response code="400">If the given food data is null or invalid.</response>
      /// <response code="404">If the food item with the given id cannot be found.</response>
      /// <response code="200">Returns the partial updated food item.</response>
      [HttpPatch("{id:int}")]
      [ProducesResponseType(typeof(FoodDto), 400)]
      [ProducesResponseType(typeof(FoodDto), 404)]
      [ProducesResponseType(typeof(FoodDto), 200)]
      public IActionResult PartialUpdate(int id, [FromBody] JsonPatchDocument<FoodDto> foodDtoPatchDoc)
      {
         if (foodDtoPatchDoc == null)
         {
            return this.BadRequest();
         }

         var foodItemExistingEntity = this.foodRepository.GetSingle(id);
         if (foodItemExistingEntity == null)
         {
            return this.NotFound();
         }

         FoodDto foodDto = AutoMapper.Mapper.Map<FoodDto>(foodItemExistingEntity);
         foodDtoPatchDoc.ApplyTo(foodDto, this.ModelState);

         if (!this.ModelState.IsValid)
         {
            return this.BadRequest(this.ModelState);
         }

         FoodItem foodItem = this.foodRepository.Update(id, AutoMapper.Mapper.Map<FoodItem>(foodDto));
         return this.Ok(AutoMapper.Mapper.Map<FoodDto>(foodItem));
      }

      /// <summary>
      /// Remove the food item with the given <paramref name="id"/>.
      /// </summary>
      /// <param name="id">Id of the <see cref="FoodItem"/> to remove.</param>
      /// <returns>Nothing.</returns>
      /// <response code="404">If the food item with the given id cannot be found.</response>
      /// <response code="204">If the food item of the given id was successfully deleted.</response>
      [HttpDelete("{id:int}")]
      [ProducesResponseType(typeof(FoodDto), 404)]
      [ProducesResponseType(typeof(FoodDto), 204)]
      public IActionResult Remove(int id)
      {
         var foodItem = this.foodRepository.GetSingle(id);
         if (foodItem == null)
         {
            return this.NotFound();
         }

         this.foodRepository.Delete(id);

         return this.NoContent();
      }
   }
}