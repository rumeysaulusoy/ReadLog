namespace ReadLog.Models {
    public class ReadingProgress {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int PagesRead { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
        
    }
}
