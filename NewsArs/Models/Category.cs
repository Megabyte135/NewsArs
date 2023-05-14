using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NewsArs.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }
    }
}
