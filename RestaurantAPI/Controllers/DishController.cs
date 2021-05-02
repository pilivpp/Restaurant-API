using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
   [ApiController]
   [Route("api/restaurant/{restaurantId}/dish")]
   public class DishController : ControllerBase
   {
      private readonly IDishService _dishService;

      public DishController(IDishService dishService)
      {
         _dishService = dishService;
      }

      [HttpDelete("{dishId}")]
      public ActionResult DeleteOneDish([FromRoute]int restaurantId, [FromRoute] int dishId)
      {
         _dishService.DeleteOne(restaurantId, dishId);

         return NoContent();
      }

      [HttpDelete]
      public ActionResult DeleteAllDishes([FromRoute] int restaurantId)
      {
         _dishService.DeleteAll(restaurantId);

         return NoContent();
      }

      [HttpGet]
      public ActionResult<List<DishDto>> GetAllDishes([FromRoute] int restaurantId)
      {
        var listOfDishes = _dishService.GetAll(restaurantId);

         return Ok(listOfDishes);
      }

      [HttpGet("{dishId}")]
      public ActionResult<DishDto> GetOneDish([FromRoute] int restaurantId, [FromRoute] int dishId)
      {
         DishDto dish = _dishService.GetById(restaurantId, dishId);

         return Ok(dish);
      }

      [HttpPost]
      public ActionResult PostDish([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
      {
         var newDishId = _dishService.Create(restaurantId, dto);

         return Created($"ap/restaurant/{restaurantId}/dish/{newDishId}", null);
      }
   }
}
