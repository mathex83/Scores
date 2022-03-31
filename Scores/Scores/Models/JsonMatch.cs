using System.ComponentModel.DataAnnotations;

namespace Scores.Models
{
	public class JsonMatch
	{
		[Key]
		public int ID { get; set; }
        public string MatchDate { get; set; }
        public string HomeTeam { get; set; }
		public string HomeScore { get; set; }
		public string AwayScore { get; set; }
		public string AwayTeam { get; set; }
	}
}
