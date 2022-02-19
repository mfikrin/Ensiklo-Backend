namespace PPL.Models
{
    public class Book
    {
        public string idBook { get; set; }
        public string title { get; set; }
        public int rating { get; set; }
        public string description_book { get; set; }
        public string publisher { get; set; }
        public string url_cover { get; set; }
        public string[] author { get; set; }
    }
}
