namespace AdgBugTracker.Models
{
    public class Member
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.Now;
        public virtual required Project Project { get; set; }
    }
}
