using Microsoft.AspNetCore.SignalR;

namespace Voting_Test.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class VoteHub : Hub
    {
        public async Task NotifyVoteUpdate()
        {
            await Clients.All.SendAsync("ReceiveVoteUpdate");
        }
    }
}