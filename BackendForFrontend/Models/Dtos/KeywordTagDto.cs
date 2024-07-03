namespace BackendForFrontend.Models.Dtos
{
    public class KeywordTagDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int KeywordId { get; set; }

        public string value { get; set; }
    }
}