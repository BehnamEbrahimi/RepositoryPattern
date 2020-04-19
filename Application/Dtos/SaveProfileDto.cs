using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class SaveProfileDto
    {
        [Required]
        public string DisplayName { get; set; }
    }
}