using RestaurantAPI.Models;
using System.Collections.Generic;

namespace RestaurantAPI.Services
{
   public interface IDishService
   {
      int Create(int restaurantId, CreateDishDto dto);
      public DishDto GetById(int restaurantId, int dishId);      
      public List<DishDto> GetAll(int restaurantId);
      public void DeleteAll(int restaurantId);
      public void DeleteOne(int restaurantId, int dishId);
   }
}