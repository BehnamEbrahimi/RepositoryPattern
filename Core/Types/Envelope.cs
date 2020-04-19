using System.Collections.Generic;

namespace Core.Types
{
    public class Envelope<T>
    {
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
    }
}