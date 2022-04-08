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
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }

        [Display(Name = "Away")]
        public string AwayTeam { get; set; }

        //public string MatchDate { get; set; }
        
        public int MatchDay { get; set; }
        
        public int MatchMonth { get; set; }
        
        public int MatchYear { get; set; }
        
        public int MatchHour { get; set; }
        
        public int MatchMinute { get; set; }

        public int? MatchResult { get; set; }
        
        public List<Match> MatchList { get; set; }
        
        public Match(JsonMatch jsonMatch)
        {
            ID = jsonMatch.ID;
            //MatchDate = jsonMatch.MatchDate;
            MatchDay = int.Parse(jsonMatch.MatchDay);
            MatchMonth = int.Parse(jsonMatch.MatchMonth);
            MatchYear = int.Parse(jsonMatch.MatchYear);
            MatchHour = int.Parse(jsonMatch.MatchHour);
            MatchMinute = int.Parse(jsonMatch.MatchMinute);
            HomeTeam = jsonMatch.HomeTeam;
            HomeScore = jsonMatch.HomeScore == "" ? null : int.Parse(jsonMatch.HomeScore);
            AwayScore = jsonMatch.AwayScore == "" ? null : int.Parse(jsonMatch.AwayScore);
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
