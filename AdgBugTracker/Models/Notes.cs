using System.ComponentModel.DataAnnotations.Schema;

namespace AdgBugTracker.Models
{
    public class Notes
    {
        public int Id { get; set; }
        public int BugId { get; set; }
        public required string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public required string UpdatedBy { get; set; }
        public required virtual Bug Bug { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual required User CreatedUser { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual required User UpdatedUser { get; set; }
    }
}
