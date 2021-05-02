﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entieties;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
   public class RestaurantService : IRestaurantService
   {
      private readonly RestaurantDbContext _dbContext;
      private readonly IMapper _mapper;
      private readonly ILogger<RestaurantService> _logger;

      public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
      {
         _dbContext = dbContext;
         _mapper = mapper;
         _logger = logger;
      }

      public void Update(UpdateRestaurantDto dto, int id)
      {
         var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

         if (restaurant is null) throw new NotFoundException("Restaurant not found");

         restaurant.Name = dto.Name;
         restaurant.Description = dto.Description;
         restaurant.Category = dto.Category;
         restaurant.HasDelivery = dto.HasDelivery;
         restaurant.ContactEmail = dto.ContactEmail;
         restaurant.ContactNumber = dto.ContactNumber;

         _dbContext.SaveChanges();
      }

      public void Delete(int id)
      {
         _logger.LogWarning($"DELETE action invoked for restaurant with id: {id} ");

         var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

         if (restaurant is null) throw new NotFoundException("Restaurant not found");

         _dbContext.Remove(restaurant);

         if (restaurant != null)
         {
            var restaurantAddress = _dbContext.Addresses.SingleOrDefault(r => r.Id == restaurant.AddressId);

            _dbContext.Remove(restaurantAddress);
         }

         _dbContext.SaveChanges();
      }

      public RestaurantDto GetOne(int id)
      {
         var restaurant = _dbContext.Restaurants.Include(r => r.Address).Include(r => r.Dishes).FirstOrDefault(r => r.Id == id);

         if (restaurant is null) throw new NotFoundException("Restaurant not found");

         var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

         return restaurantDto;
      }

      public IEnumerable<RestaurantDto> GetAll()
      {
         var restaurants = _dbContext.Restaurants.Include(r => r.Address).Include(r => r.Dishes).ToList();

         var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

         return restaurantsDtos;
      }

      public int Create(CreateRestaurantDto dto)
      {
         var restaurant = _mapper.Map<Restaurant>(dto);

         _dbContext.Add(restaurant);
         _dbContext.SaveChanges();

         return restaurant.Id;
      }
   }
}
