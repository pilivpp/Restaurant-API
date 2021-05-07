using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entieties;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
   [ApiController]
   [Route("api/restaurant")]
   [Authorize]
   public class RestaurantController : ControllerBase
   {
      private readonly IRestaurantService _restaurantService;

      public RestaurantController(IRestaurantService restaurantService)
      {
         _restaurantService = restaurantService;
      }

      [HttpPut("{id}")]
      public ActionResult UpdateRestaurant([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
      {
         _restaurantService.Update(dto, id);

         return Ok(); 
      }

      [HttpDelete("{id}")]
      public ActionResult DeleteRestaurant([FromRoute] int id)
      {
         _restaurantService.Delete(id);

         return NoContent();
      }

      [HttpPost]
      [Authorize(Roles = "Manager, Admin")]
      public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
      {
         var id = _restaurantService.Create(dto);

         return Created($"api/restaurant/{id}", null);
      }

      [HttpGet]
      [Authorize(Policy = "HasNationality")]
      public ActionResult<IEnumerable<RestaurantDto>> GetAllRestaurants()
      {
         var restaurantsDtos = _restaurantService.GetAll();

         return Ok(restaurantsDtos);      
      }

      [HttpGet("{id}")]
      public ActionResult<RestaurantDto> GetOneRestaurant([FromRoute] int id)
      {
         var restaurantDto = _restaurantService.GetOne(id);

         return Ok(restaurantDto);
      }
   }
}
