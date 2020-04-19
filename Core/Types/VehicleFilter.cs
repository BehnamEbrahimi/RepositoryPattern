using Core.Interfaces;

namespace Core.Types
{
    public class VehicleFilter : IFilter
    {
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public string UserId { get; set; }
        public string SortBy { get; set; }
        public bool IsSortDescending { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
    }
}