using RestaurantAPI.Entieties;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
   public interface IRestaurantService
   {
      int Create(CreateRestaurantDto dto);
      PagedResult<RestaurantDto> GetAll(RestaurantQuery query);
      RestaurantDto GetOne(int id);
      void Delete(int id);
      void Update(UpdateRestaurantDto dto, int id);
   }
}