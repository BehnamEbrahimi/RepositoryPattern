using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Model
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public int MakeId { get; set; }

        public Make Make { get; set; }
    }
}