using System.Collections.Generic;

namespace Domain.Types
{
    public class Envelope<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}