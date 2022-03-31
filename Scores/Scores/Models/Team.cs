using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Scores.Models
{
    public class Team
    {
        [Key]
        public int TeamID { get; set; }
        [Display(Name = "PL")]
        public int Played { get { return MatchesWon + MatchesDrawn + MatchesLost; } }
        [Display(Name ="TEAM")]
        public string TeamName { get; set; }
        [Display(Name ="W")]
        public int MatchesWon { get; set; }
        [Display(Name = "D")]
        public int MatchesDrawn { get; set; }
        [Display(Name = "L")]
        public int MatchesLost { get; set; }
        [Display(Name = "PT")]
        public int Points { get { return (3 * MatchesWon) + MatchesDrawn; } }
        [Display(Name = "GF")]
        public int GoalsFor { get; set; }
        [Display(Name = "GA")]
        public int GoalsAgainst { get; set; }
        [Display(Name = "GD")]
        public int GoalDifference { get { return GoalsFor - GoalsAgainst; } }
        [Display(Name ="Max")]
        public int MaxPoints { get { return ((38 - Played) * 3) + Points; } }
        public string TeamNameReplace(string team)
        {
            return team switch
            {
                "Brighton" => "Brighton and Hove Albion",
                "Leeds" => "Leeds United",
                "Leicester" => "Leicester City",
                "Man City" => "Manchester City",
                "Man Utd" => "Manchester United",
                "Newcastle" => "Newcastle United",
                "Norwich" => "Norwich City",
                "Spurs" => "Tottenham Hotspur",
                "West Ham" => "West Ham United",
                "Wolves" => "Wolverhampton Wanderers",
                _ => team
            };
        }
    }
}