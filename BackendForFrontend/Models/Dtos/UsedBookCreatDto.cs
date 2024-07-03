using System.Text.Json.Serialization;

namespace BackendForFrontend.Models.Dtos
{
    public class UsedBookCreatDto
    {
       
        public string ISBN { get; set; }

        public string BookStatus { get; set; }
        public int? CategoryId { get; set; }
        public IFormFile ImageFile { get; set; }
        public decimal Price { get; set; }



    }
}
