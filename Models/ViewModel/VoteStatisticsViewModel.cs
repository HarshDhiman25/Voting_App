namespace Voting_Test.Models.ViewModels
{
    public class VoteStatisticsViewModel
    {
        public int PollId { get; set; }
        public string PollQuestion { get; set; }
        public string PollingRoomName { get; set; }
        public int TotalVotes { get; set; }
    }
}
