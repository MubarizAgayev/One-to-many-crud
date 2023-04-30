using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework_Slider.Models
{
    public class Blog:BaseEntity
    {
        public string Header { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }
        [NotMapped, Required(ErrorMessage = "Don't be empty")]
        public IFormFile Photo { get; set; }
    }
}
