using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework_Slider.Models
{
    public class Expert:BaseEntity
    {
        public string Image { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }

        [NotMapped, Required(ErrorMessage = "Don't be empty")]
        public IFormFile Photo { get; set; }
    }
}
