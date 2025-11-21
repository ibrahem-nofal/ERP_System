using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(70)]
        public string Name { get; set; }
        public string Detail { get; set; }

        public ICollection<ItemCategory> ItemCategories { get; set; }

    }
}
