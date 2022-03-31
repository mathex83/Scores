using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Scores.Models
{
    public class Match
    {
        [Key]
        public int ID { get; private set; }

        [Display(Name = "Home")]
        public string HomeTeam { get; set; }

        [Display(Name = "HS")]
        public int? HomeScore { get; set; }

        [Display(Name = "AS")]
        public int? AwayScore { get; set; }

        [Display(Name = "Away")]
        public string AwayTeam { get; set; }

        public string MatchDate { get; set; }

        public int MatchResult { get; set; }
        public List<Match> MatchList { get; set; }
        public Match(JsonMatch jsonMatch)
        {
            ID = jsonMatch.ID;
            MatchDate = jsonMatch.MatchDate;
            HomeTeam = jsonMatch.HomeTeam;
            HomeScore = jsonMatch.HomeScore == "" ? null : int.Parse(jsonMatch.HomeScore);
            AwayScore = jsonMatch.HomeScore == "" ? null : int.Parse(jsonMatch.AwayScore);
            AwayTeam = jsonMatch.AwayTeam;
            MatchResult = GetMatchWinner(HomeScore, AwayScore);
        }

        private static int GetMatchWinner(int? h, int? a)
        {
            {
                if (h == null || a == null) return -1;  //not played
                else if (h > a) return 1;   //Home win
                else if (h == a) return 0;  //Draw
                else return 2;              //Away win
            }
        }
    }
}
