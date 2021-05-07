using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
   public class CreatedMultipleRestaurantsRequirement : IAuthorizationRequirement
   {
      public CreatedMultipleRestaurantsRequirement(int miniumRestaurantsCreated)
      {
         MiniumRestaurantsCreated = miniumRestaurantsCreated;
      }
      public int MiniumRestaurantsCreated { get; }
   }
}
