using System.Collections.Generic;

namespace Application.Dtos
{
    public class EnvelopeDto<T>
    {
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
    }
}