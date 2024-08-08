namespace Voting_Test.Models
{
    public class PollsByRoomViewModel
    {
        public int PollingRoomId { get; set; }
        public IList<Poll> Polls { get; set; } = new List<Poll>();
        public bool UserHasVoted { get; set; }
    }
}
