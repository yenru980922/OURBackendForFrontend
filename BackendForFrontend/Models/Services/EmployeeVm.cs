using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.ViewModels
{
    public class EmployeeVm
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "性別")]
        public bool Gender { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "地址")]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "信箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "手機號碼")]
        public string PhoneNumber { get; set; }

        [Required]        
        [Display(Name = "權限名稱")]

        public int GroupId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "密碼")]
        public string Password { get; set; }
    }
}