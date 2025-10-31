using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string LogoPath { get; set; }
        public string Language { get; set; }
    }
}
