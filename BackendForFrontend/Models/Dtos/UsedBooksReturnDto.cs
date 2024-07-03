using BackendForFrontend.Models.EFModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendForFrontend.Models.Dtos
{
    public class UsedBooksReturnDto
    {
        public int Id { get; set; }

        public int OrderID { get; set; }

        public int MemberID { get; set; }

        public DateTime ApplicationDate { get; set; }

        public string ReturnReason { get; set; }

        public string Status { get; set; }
    }
}
