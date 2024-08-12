namespace Voting_Test.Models
{
    public class PollWinners
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string PollQuestion { get; set; }
        public string PollingRoomName { get; set; }
        public int TotalVotes { get; set; }
        public string WinnerMessage { get; set; }
        public DateTime WinningDate { get; set; }
    }
}
