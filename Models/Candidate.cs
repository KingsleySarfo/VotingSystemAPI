namespace VotingSystemAPI.Data.Models
{
    public class Candidate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string ImageUrl { get; set; } // 🔥 NEW
    }
}