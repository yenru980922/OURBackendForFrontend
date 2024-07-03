using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int GroupId { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
    }
}