namespace VotingSystemAPI.Data.Models

{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        // existing code
    }
    namespace VotingSystemAPI.Data.Models
    {
        public class Vote
        {
            public int Id { get; set; }

            public int UserId { get; set; }
            public int CandidateId { get; set; }

            public string Position { get; set; } // Important!

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
    }
}