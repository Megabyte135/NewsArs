using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NewsArs.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Заголовок")]
        public string Title { get; set; }
        [Required]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        [DisplayName("Автор")]
        public User Author { get; set; }
        [DisplayName("Изображение")]
        public string Image { get; set; }
        [Required]
        [DisplayName("Текст")]
        public string Text { get; set; }
        [Required]
        [DisplayName("Текст-превью")]
        public string PreviewText { get; set; }
        [Required]
        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
