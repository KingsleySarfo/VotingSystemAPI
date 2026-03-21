using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingSystemAPI.Data;
using VotingSystemAPI.Data.DTOs;
using VotingSystemAPI.Data.Models;

namespace VotingSystemAPI.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CandidateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET ALL CANDIDATES
        [HttpGet]
        public IActionResult GetCandidates()
        {
            var candidates = _context.Candidates.ToList();
            return Ok(candidates);
        }

        // ✅ ADD CANDIDATE (for now open, later restrict to Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCandidate([FromForm] CandidateCreateDto dto)
        {
            if (dto.Image == null || dto.Image.Length == 0)
                return BadRequest("Image is required");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            var candidate = new Candidate
            {
                Name = dto.Name,
                Position = dto.Position,
                ImageUrl = $"/images/{fileName}" // 🔥 Save path
            };

            _context.Candidates.Add(candidate);
            _context.SaveChanges();

            return Ok(candidate);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCandidate(int id, [FromForm] CandidateCreateDto dto)
        {
            var candidate = _context.Candidates.Find(id);

            if (candidate == null)
                return NotFound("Candidate not found");

            candidate.Name = dto.Name;
            candidate.Position = dto.Position;

            if (dto.Image != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                candidate.ImageUrl = $"/images/{fileName}";
            }

            _context.SaveChanges();

            return Ok(candidate);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteCandidate(int id)
        {
            var candidate = _context.Candidates.Find(id);

            if (candidate == null)
                return NotFound("Candidate not found");

            _context.Candidates.Remove(candidate);
            _context.SaveChanges();

            return Ok("Candidate deleted");
        } 
    }
}