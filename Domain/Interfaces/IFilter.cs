namespace Domain.Interfaces
{
    public interface IFilter
    {
        string SortBy { get; set; }
        bool IsSortDescending { get; set; }
        int Page { get; set; }
        byte PageSize { get; set; }
    }
}