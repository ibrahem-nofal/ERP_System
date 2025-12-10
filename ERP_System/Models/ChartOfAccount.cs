namespace ERP_System.Models
{
    public class ChartOfAccount
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "يرجى إدخال كود الحساب")]
        public string Code { get; set; }        
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "يرجى إدخال اسم الحساب")]
        public string Name { get; set; }

        public string? ParentCode { get; set; } 

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "يرجى اختيار نوع الحساب")]
        public string AccountType { get; set; }  

        public int Level { get; set; }            

        public bool IsActive { get; set; } = true;

        public bool IsLeaf { get; set; } = false;

        public string? NormalBalance { get; set; } 

        public string? Notes { get; set; }

     
        public ChartOfAccount? Parent { get; set; }
        public ICollection<ChartOfAccount>? Children { get; set; }
    }

}
