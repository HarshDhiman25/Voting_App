namespace Voting_Test.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Vote
    {
        [Key]
        public int VoteId { get; set; } 

        [Required]
        public string UserId { get; set; } 

        [Required]
        public ApplicationUser User { get; set; } 

        [Required]
        public int OptionId { get; set; } 

        [Required]
        public Option Option { get; set; } 

        [Required]
        public int PollId { get; set; } 

        [Required]
        public Poll Poll { get; set; } 
    }

}
