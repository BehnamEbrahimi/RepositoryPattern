using Domain.Interfaces;

namespace Domain.Types
{
    public class VehicleFilter : IFilter
    {
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public string SortBy { get; set; }
        public bool IsSortDescending { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
    }
}