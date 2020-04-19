using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Photo
    {
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Url { get; set; }

        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }
    }
}