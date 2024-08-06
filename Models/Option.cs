using System.ComponentModel.DataAnnotations;

namespace Voting_Test.Models
{
    public class Option
    {
        [Key]
        public int OptionId { get; set; } 

        [Required(ErrorMessage = "The option text is required.")]
        [StringLength(100, ErrorMessage = "The option text cannot exceed 100 characters.")]
        public string Text { get; set; }

        [Required]
        public int PollId { get; set; }

        [Required]
        public Poll Poll { get; set; }

        // Navigation property for votes
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }

}
