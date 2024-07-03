using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{
    public class BookSellersDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? Compiled { get; set; }

        public string BankAccount { get; set; }
    }
}