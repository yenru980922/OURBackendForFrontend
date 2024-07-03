using BackendForFrontend.Models.EFModels;


namespace BackendForFrontend.Models.Dtos
{
    public class ArticleDto
    {
        public int ArticleID { get; set; }

        public int EmployeeID { get; set; }

        public string Title { get; set; }

        public DateTime PublishTime { get; set; }
        public string Content { get; set; }

        public string Category { get; set; }

        public virtual Employee Employee { get; set; }
    }
}