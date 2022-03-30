namespace PPL.Models
{
    public class Book
    {
        public int Id_book { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateOnly Year_published { get; set; }
        public string Description_book { get; set; }
        public string Book_content { get; set; }
        public int Page { get; set; }
        public string Url_cover { get; set; }
        public string Category { get; set; }
        public DateTime Added_time { get; set; }
        public string Keywords { get; set; }
    }
}
