using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdgBugTracker.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int ProjectId { get; set; }
        public required string UserId { get; set; }
        public bool IsResolved { get; set; }
        public string ClosedById { get; set; } = null!;
        public DateTime ClosedDate { get; set; }
        public string ReopenedById { get; set; } = null!;
        public DateTime ReopenedDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public required virtual Project Project { get; set; }
        [ForeignKey("ClosedById")]
        public virtual User? ClosedUser { get; set; }
        [ForeignKey("ReopenedById")]
        public virtual User? ReopenedUser { get; set; }

    }
}
