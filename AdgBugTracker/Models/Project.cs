using System.ComponentModel.DataAnnotations.Schema;

namespace AdgBugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public required virtual User User { get; set; }
    }
}
