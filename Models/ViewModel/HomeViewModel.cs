namespace Voting_Test.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Vote> Votes { get; set; }
        public IEnumerable<Poll> Polls { get; set; }
        public IEnumerable<PollingRoom> PollingRooms { get; set; }
    }
}
