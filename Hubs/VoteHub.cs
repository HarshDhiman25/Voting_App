using Microsoft.AspNetCore.SignalR;

namespace Voting_Test.Hubs
{
    public class VoteHub : Hub
    {
        public async Task SendVoteUpdate()
        {
            await Clients.All.SendAsync("ReceiveVoteUpdate");
        }
    }
}
