using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Application.Dtos
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public IdNameDto Make { get; set; }
        public IdNameDto Model { get; set; }
        public bool IsRegistered { get; set; }
        public DateTime LastUpdate { get; set; }
        public ContactDto Contact { get; set; }
        public ICollection<IdNameDto> Features { get; set; }

        public VehicleDto()
        {
            Features = new Collection<IdNameDto>();
        }
    }
}