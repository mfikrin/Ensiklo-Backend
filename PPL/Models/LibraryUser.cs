namespace PPL.Models
{
    public class LibraryUser
    {
        public int Id_user { get; set; }
        public int Id_book { get; set; }
        public int At_page { get; set; }
        public DateTime Last_readtime { get; set; }
        public bool finish_reading { get; set; }
        public DateTime Added_time_to_library { get; set; }

    }
}
