using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VotingSystemAPI.Data;
using VotingSystemAPI.Data.DTOs;
using VotingSystemAPI.Data.Models;
using VotingSystemAPI.Data.Models.VotingSystemAPI.Data.Models;

namespace VotingSystemAPI.Data.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("cast")]
        public IActionResult CastVote([FromBody] VoteDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var alreadyVoted = _context.Votes
                .Any(v => v.UserId == userId && v.Position == dto.Position);

            if (alreadyVoted)
            {
                return BadRequest($"You have already voted for {dto.Position}");
            }

            var candidate = _context.Candidates.FirstOrDefault(c => c.Id == dto.CandidateId);

            if (candidate == null)
            {
                return NotFound("Candidate not found");
            }

            if (candidate.Position != dto.Position)
            {
                return BadRequest("Invalid position for selected candidate");
            }

            var vote = new Vote
            {
                UserId = userId,
                CandidateId = dto.CandidateId,
                Position = dto.Position
            };

            _context.Votes.Add(vote);
            _context.SaveChanges();

            return Ok("Vote cast successfully");
        }

        [HttpGet("results")]
        public IActionResult GetResults()
        {
            var results = _context.Candidates
                .Select(c => new
                {
                    Candidate = c.Name,
                    Position = c.Position,
                    Votes = _context.Votes.Count(v => v.CandidateId == c.Id)
                })
                .ToList();

            return Ok(results);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("reset")]
        public IActionResult ResetVotes()
        {
            var votes = _context.Votes.ToList();

            _context.Votes.RemoveRange(votes);
            _context.SaveChanges();

            return Ok("All votes cleared. Election reset.");
        }
    }
}