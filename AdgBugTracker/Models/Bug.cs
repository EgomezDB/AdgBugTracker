using Microsoft.AspNetCore.Identity;

namespace AdgBugTracker.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public bool IsResolved { get; set; }
        public int ClosedById { get; set; }
        public DateTime ClosedDate { get; set; }
        public int ReopenedById { get; set; }
        public DateTime ReopenedDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public required virtual Project Project { get; set; }
    }
}
