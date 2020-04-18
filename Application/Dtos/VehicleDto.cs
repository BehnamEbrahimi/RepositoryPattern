using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class VehicleDto
    {
        public int Id { get; set; }

        public IdNameDto Model { get; set; }

        public IdNameDto Make { get; set; }

        public bool IsRegistered { get; set; }

        [Required]
        public ContactDto Contact { get; set; }

        public DateTime LastUpdate { get; set; }

        public ICollection<int> Features { get; set; }

        public VehicleDto()
        {
            Features = new Collection<int>();
        }
    }
}