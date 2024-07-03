﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{
    public class MemberDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }


        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        
    }
}