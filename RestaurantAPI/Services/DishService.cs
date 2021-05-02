using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Controllers;
using RestaurantAPI.Entieties;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
   public class DishService : IDishService
   {
      private readonly RestaurantDbContext _dbContext;
      private readonly IMapper _mapper;

      public DishService(RestaurantDbContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }      

      public void DeleteOne(int restaurantId, int dishId)
      {
         var restaurant = GetRestaurantById(restaurantId);

         var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);

         if (dish is null || dish.RestaurantId != restaurantId) throw new NotFoundException("Dish not found");

         _dbContext.Remove(dish);
         _dbContext.SaveChanges();
      }

      public void DeleteAll(int restaurantId)
      {
         var restaurant = GetRestaurantById(restaurantId);

         _dbContext.RemoveRange(restaurant.Dishes);
         _dbContext.SaveChanges();
      }

      public List<DishDto> GetAll(int restaurantId)
      {
         var restaurant = GetRestaurantById(restaurantId);

         var dishes = _mapper.Map<List<DishDto>>(restaurant.Dishes);

         return dishes;
      }

      public DishDto GetById(int restaurantId, int dishId)
      {
         var restaurant = GetRestaurantById(restaurantId);

         var dish = _dbContext.Dishes.FirstOrDefault(d => d.Id == dishId);

         if (dish is null || dish.RestaurantId != restaurantId) throw new NotFoundException("Dish not found");

         var dishDto = _mapper.Map<DishDto>(dish);

         return dishDto;
      }

      public int Create(int restaurantId, CreateDishDto dto)
      {
         var restaurant = GetRestaurantById(restaurantId);

         var dish = _mapper.Map<Dish>(dto);

         dish.RestaurantId = restaurantId;

         _dbContext.Dishes.Add(dish);
         _dbContext.SaveChanges();

         return dish.Id;
      }

      private Restaurant GetRestaurantById(int restaurantId)
      {
         var restaurant = _dbContext.Restaurants.Include(r => r.Dishes).FirstOrDefault(r => r.Id == restaurantId);

         if (restaurant is null) throw new NotFoundException("Restaurant not found");

         return restaurant;
      }
   }
}
