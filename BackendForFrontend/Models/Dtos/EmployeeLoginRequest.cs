using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{
    public class EmployeeLoginRequest
    {
        
        public required string Account { get; set; }

        public required string Password { get; set; }
    }
}
