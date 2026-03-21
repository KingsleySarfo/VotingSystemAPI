using Microsoft.AspNetCore.Http;

namespace VotingSystemAPI.Data.DTOs
{
    public class CandidateCreateDto
    {
        public string Name { get; set; }

        public string Position { get; set; }

        public IFormFile Image { get; set; } // 🔥 FILE
    }
}