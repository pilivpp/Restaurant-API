using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entieties;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
   public class RestaurantService : IRestaurantService
   {
      private readonly RestaurantDbContext _dbContext;
      private readonly IMapper _mapper;
      private readonly ILogger<RestaurantService> _logger;
      private readonly IAuthorizationService _authorizationService;
      private readonly IUserContextService _userContextService;

      public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService, IUserContextService userContextService)
      {
         _dbContext = dbContext;
         _mapper = mapper;
         _logger = logger;
         _authorizationService = authorizationService;
         _userContextService = userContextService;
      }

      public void Update(UpdateRestaurantDto dto, int id)
      {
         var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

         if (restaurant is null) throw new NotFoundException("Restaurant not found");

         var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

         if (!authorizationResult.Succeeded)
         {
            throw new ForbidException();
         }

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

         var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

         if (!authorizationResult.Succeeded)
         {
            throw new ForbidException();
         }

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

      public PagedResult<RestaurantDto> GetAll(RestaurantQuery query)
      {
         var baseQuery = _dbContext.Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .Where(r => query.SearchPhrase == null ||
               (r.Name.ToLower().Contains(query.SearchPhrase.ToLower()) || r.Description.ToLower().Contains(query.SearchPhrase.ToLower())));

         var restaurants = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

         var totalItemsCount =  baseQuery.Count();

         var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

         var result = new PagedResult<RestaurantDto>(restaurantsDtos, totalItemsCount, query.PageSize, query.PageNumber); 

         return result;
      }

      public int Create(CreateRestaurantDto dto)
      {
         var restaurant = _mapper.Map<Restaurant>(dto);
         restaurant.CreatedById = _userContextService.GetUserId;
         _dbContext.Add(restaurant);
         _dbContext.SaveChanges();

         return restaurant.Id;
      }
   }
}
