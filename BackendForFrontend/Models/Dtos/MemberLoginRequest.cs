using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{
    public class MemberLoginRequest
    {
        
        [EmailAddress]
        public required string Email { get; set; }

        public required string Password { get; set; }

        // AAAAAAA
        public string Captcha { get; set; }

        public string CacheKey { get; set; }
    }
}
