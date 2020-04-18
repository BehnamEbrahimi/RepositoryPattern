using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Application.Dtos
{
    public class MakeDto : IdNameDto
    {
        public ICollection<IdNameDto> Models { get; set; }

        public MakeDto()
        {
            Models = new Collection<IdNameDto>();
        }
    }
}
