using System.Collections.Generic;
using System.Linq;

namespace Scores.Models
{
    public class League
    {
        public List<Team> LeagueTable { get; set; }
        public League(List<Match> matches)
        {
            LeagueTable = new List<Team>();
            List<string> teamNames = matches.Select(m => m.HomeTeam).Distinct().ToList();

            for (int i = 0; i < teamNames.Count; i++)
            {
                int w = matches.Where(m => m.HomeTeam == teamNames[i] && m.MatchResult == 1).ToList().Count + 
                    matches.Where(m => m.AwayTeam == teamNames[i] && m.MatchResult == 2).ToList().Count;
                int d = matches.Where(m => m.HomeTeam == teamNames[i] && m.MatchResult == 0).ToList().Count + 
                    matches.Where(m => m.AwayTeam == teamNames[i] && m.MatchResult == 0).ToList().Count;
                int l = matches.Where(m => m.HomeTeam == teamNames[i] && m.MatchResult == 2).ToList().Count + 
                    matches.Where(m => m.AwayTeam == teamNames[i] && m.MatchResult == 1).ToList().Count;
                int gf = (from m in matches where m.HomeTeam == teamNames[i] && m.HomeScore >= 0 select (int)m.HomeScore).ToList().Sum() +
                    (from m in matches where m.AwayTeam == teamNames[i] && m.HomeScore >= 0 select (int)m.AwayScore).ToList().Sum();
                int ga = (from m in matches where m.HomeTeam == teamNames[i] && m.HomeScore >= 0 select (int)m.AwayScore).ToList().Sum() +
                    (from m in matches where m.AwayTeam == teamNames[i] && m.HomeScore >= 0 select (int)m.HomeScore).ToList().Sum();

                LeagueTable.Add(new Team
                {
                    TeamID = i,
                    TeamName = teamNames[i],
                    MatchesWon = w,
                    MatchesDrawn = d,
                    MatchesLost = l,
                    GoalsFor = gf,
                    GoalsAgainst = ga
                });
            }
        }
    }
}
