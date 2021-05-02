using AutoMapper;
using RestaurantAPI.Entieties;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI
{
   public class RestaurantMappingProfile : Profile
   {
      public RestaurantMappingProfile()
      {
         CreateMap<Restaurant, RestaurantDto>()
            .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
            .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
            .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

         CreateMap<CreateRestaurantDto, Restaurant>()
            .ForMember(r => r.Address, c => c.MapFrom(dto => new Address() { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));

         CreateMap<UpdateRestaurantDto, Restaurant>();

         CreateMap<Dish, DishDto>();

         CreateMap<CreateDishDto, Dish>();

         CreateMap<Dish, List<DishDto>>();
      }
   }
}
