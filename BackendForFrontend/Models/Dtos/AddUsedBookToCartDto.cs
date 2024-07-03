using System.ComponentModel.DataAnnotations;

namespace BackendForFrontend.Models.Dtos
{
    public class AddUsedBookToCartDto
    {
        [Required]
        public int MemberID { get; set; }

        [Required]
        public int BookID { get; set; }
    }
}
