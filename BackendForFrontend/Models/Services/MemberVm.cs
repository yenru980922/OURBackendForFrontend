using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.ViewModels
{
    public class MemberVm
    {
        public int Id { get; set; }

        [Display(Name = "姓名")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "性別")]
        public bool Gender { get; set; }

        [Display(Name = "生日")]
        [Column(TypeName = "date")]
        public string DateOfBirth { get; set; }


        [Required]
        [StringLength(50)]
        [EmailAddress]
        [Display(Name = "信箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "會員等級")]
        public string MembersLevel { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "地址")]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "手機號碼")]
        public string PhoneNumber { get; set; }

        [Display(Name = "會員點數")]
        public int Points { get; set; }

    }
}