using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace Core.Domain
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }

        public User()
        {
            Vehicles = new Collection<Vehicle>();
        }
    }
}