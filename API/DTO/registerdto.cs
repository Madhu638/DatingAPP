using System.ComponentModel.DataAnnotations;
namespace API.DTO
{
    public class registerdto
    {
        [Required]
        public string username { get; set; } 

        [Required]
        public string password { get; set; }
    }
}