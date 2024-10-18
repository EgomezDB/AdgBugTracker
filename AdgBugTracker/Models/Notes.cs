namespace AdgBugTracker.Models
{
    public class Notes
    {
        public int Id { get; set; }
        public int BugId { get; set; }
        public required string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public required virtual Bug Bug { get; set; }
    }
}
