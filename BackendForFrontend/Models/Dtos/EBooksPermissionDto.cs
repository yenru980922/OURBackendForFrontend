using BackendForFrontend.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{
    public class EBooksPermissionDto
    {
        public int Id { get; set; }

        public int BookID { get; set; }

		public string BookName { get; set; }

		public int MemberID { get; set; }
        
        public string MemberName { get; set; }

        public DateTime CreateDate { get; set; }
        
        public string PermissionType { get; set; }
    }
}