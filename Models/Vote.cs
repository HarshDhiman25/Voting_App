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
        public int PollingRoomId { get; set; }
        public PollingRoom PollingRoom { get; set; }

        [Required]
        public int PollId { get; set; }
        public Poll Poll { get; set; }

        public DateTime VoteDate { get; set; }
    }

}
