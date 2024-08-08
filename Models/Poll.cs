using Microsoft.CodeAnalysis.Options;
using System.ComponentModel.DataAnnotations;

namespace Voting_Test.Models
{

    public class Poll
    {
        [Key]
        public int PollId { get; set; }

        [Required(ErrorMessage = "The poll question is required.")]
        [StringLength(200, ErrorMessage = "The question cannot exceed 200 characters.")]
        public string Question { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        // Navigation properties
        public int? PollingRoomId { get; set; }
        public PollingRoom PollingRoom { get; set; }
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
