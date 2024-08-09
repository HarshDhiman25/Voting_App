using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Voting_Test.Models
{
    public class PollingRoom
    {
        public int PollingRoomId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

       
        public DateTime? EndDate { get; set; }

        public ICollection<Poll> Polls { get; set; }
    }
}
