namespace BackendForFrontend.Models.Dtos
{
    public class BooksDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string Author { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }
    }
}