using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{
    public class EBookDto
    {
        public string ProductName { get; set; }
        public int Id { get; set; }
       
        public int ProductId { get; set; }
        public string FileLink { get; set; }
        public string Sample { get; set; }
    }
}