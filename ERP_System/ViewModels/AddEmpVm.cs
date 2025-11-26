using ERP_System.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP_System.ViewModels
{
    public class AddEmpVm
    {
        [Required(ErrorMessage = "الرجاء إدخال اسم الموظف.")]
        [StringLength(150, ErrorMessage = "الاسم يجب ألا يزيد عن 150 حرفاً.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار نوع الوظيفة.")]
        public int RoleType { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار النوع.")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال العنوان.")]
        [StringLength(200, ErrorMessage = "العنوان يجب ألا يزيد عن 200 حرف.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال رقم الهوية.")]
        [StringLength(25, ErrorMessage = "رقم الهوية يجب ألا يزيد عن 25 حرف.")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار تاريخ الميلاد.")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار المؤهل الدراسي.")]
        public int Qualification { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار الحالة الوظيفية.")]
        public int State { get; set; }


        [MinLength(1, ErrorMessage = "الرجاء إدخال رقم هاتف واحد على الأقل.")]
        public List<string> Phones { get; set; } = new List<string>();

        [Required(ErrorMessage = "الرجاء اختيار صورة للموظف.")]
        public IFormFile EmpImage { get; set; }
    }
}
