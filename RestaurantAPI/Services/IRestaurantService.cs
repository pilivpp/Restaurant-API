using RestaurantAPI.Entieties;
using RestaurantAPI.Models;
using System.Collections.Generic;

namespace RestaurantAPI.Services
{
   public interface IRestaurantService
   {
      int Create(CreateRestaurantDto dto);
      IEnumerable<RestaurantDto> GetAll();
      RestaurantDto GetOne(int id);
      void Delete(int id);
      void Update(UpdateRestaurantDto dto, int id);
   }
}