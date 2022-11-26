using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Scores.Models;
using Scores.Models.DTOs;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Scores.Models.Extensions;

namespace Scores.Controllers
{
    public class WC22Controller : Controller
    {
        private readonly ILogger<WC22Controller> _logger;
        private static readonly string filePath = @".\wwwroot\files\wc2022.json";
        private static readonly JsonLoader r = new(filePath);
        private readonly List<Match> MatchList = new();
        private List<Team> LeagueTable = new();
        Team replaceTeam = new();

        public WC22Controller(ILogger<WC22Controller> logger)
        {
            _logger = logger;
        }
        private void PopulateMatchList()
        {
            MatchList.Clear();
            foreach (JsonMatch jsonMatch in r.Matches)
            {
                MatchList.Add(new Match(jsonMatch));
            }
        }
        private void PopulateLeagueTable()
        {
            PopulateMatchList();
            League league = new League(MatchList);
            LeagueTable = league.LeagueTable;
        }
        
        public IActionResult WC22Matches(string sortOrder)
        {
            PopulateMatchList();
            var matches = from m in MatchList select m;

            #region Input for MatchSorting
            ViewData["DateSort"] =
                string.IsNullOrEmpty(sortOrder) || sortOrder == "d_desc" ? "d" : "d_desc";
            ViewData["HomeTeamSort"] =
                    string.IsNullOrEmpty(sortOrder) || sortOrder == "h_desc" ? "h" : "h_desc";
            ViewData["AwayTeamSort"] =
                    string.IsNullOrEmpty(sortOrder) || sortOrder == "a_desc" ? "a" : "a_desc";
            #endregion

            #region MatchesSortingSwitch
            switch (sortOrder)
            {
                case "d":
                    matches = matches.OrderBy(m => m.MatchYear)
                        .ThenBy(m => m.MatchMonth)
                        .ThenBy(m => m.MatchDay)
                        .ThenBy(m => m.MatchHour)
                        .ThenBy(m => m.MatchMinute);
                    break;
                case "d_desc":
                    matches = matches.OrderByDescending(m => m.MatchYear)
                        .ThenByDescending(m => m.MatchMonth)
                        .ThenByDescending(m => m.MatchDay)
                        .ThenByDescending(m => m.MatchHour)
                        .ThenByDescending(m => m.MatchMinute);
                    break;
                case "h":
                    matches = matches.OrderBy(m => m.HomeTeam).ThenBy(m => m.ID);
                    break;
                case "h_desc":
                    matches = matches.OrderByDescending(m => m.HomeTeam).ThenBy(m => m.ID);
                    break;
                case "a":
                    matches = matches.OrderBy(m => m.AwayTeam).ThenBy(m => m.ID);
                    break;
                case "a_desc":
                    matches = matches.OrderByDescending(m => m.AwayTeam).ThenBy(m => m.ID);
                    break;
                default:
                    matches = matches.OrderBy(m => m.MatchYear)
                        .ThenBy(m => m.MatchMonth)
                        .ThenBy(m => m.MatchDay)
                        .ThenBy(m => m.MatchHour)
                        .ThenBy(m => m.MatchMinute);
                    break;
            }
            #endregion

            return View(matches);
        }
        private List<Team> SortLeague(List<Team> league,string sortOrder)
        {
            #region TableSortingSwitch
            switch (sortOrder)
            {
                case "w":
                    league = league.OrderBy(t => t.MatchesWon).ToList();
                    break;
                case "w_desc":
                    league = league.OrderByDescending(t => t.MatchesWon).ToList();
                    break;
                case "d":
                    league = league.OrderBy(t => t.MatchesDrawn).ToList();
                    break;
                case "d_desc":
                    league = league.OrderByDescending(t => t.MatchesDrawn).ToList();
                    break;
                case "l":
                    league = league.OrderBy(t => t.MatchesLost).ToList();
                    break;
                case "l_desc":
                    league = league.OrderByDescending(t => t.MatchesLost).ToList();
                    break;
                case "pt":
                    league = league.OrderBy(t => t.Points).ToList();
                    break;
                case "pt_desc":
                    league = league.OrderByDescending(t => t.Points).ToList();
                    break;
                default:
                    league = league.OrderByDescending(t => t.Points)
                        .ThenByDescending(t => t.GoalDifference).ToList();
                    break;
            }
            #endregion
            return league;
        }
        public IActionResult WC22Tables(string sortOrder)
        {
            PopulateLeagueTable();

            List<string> a = new() { "Ecuador", "Holland", "Qatar","Senegal" };
            List<string> b = new() { "England", "Iran", "USA", "Wales" };
            List<string> c = new() { "Argentina", "Mexico", "Poland", "Saudi Arabia" };
            List<string> d = new() { "Australia", "Denmark", "France", "Tunisia" };
            List<string> e = new() { "Costa Rica", "Germany", "Japan", "Spain" };
            List<string> f = new() { "Belgium", "Canada", "Croatia", "Morocco" };
            List<string> g = new() { "Brazil", "Cameroun", "Schwitzerland", "Serbia"};
            List<string> h = new() { "Ghana", "Portugal", "South Korea", "Uruguay" };

            var league = from t in LeagueTable select t;

            #region Input for TableSorting
            ViewData["WSort"] =
                    string.IsNullOrEmpty(sortOrder) || sortOrder == "w_desc" ? "w" : "w_desc";
            ViewData["DSort"] =
                    string.IsNullOrEmpty(sortOrder) || sortOrder == "d_desc" ? "d" : "d_desc";
            ViewData["LSort"] =
                    string.IsNullOrEmpty(sortOrder) || sortOrder == "l_desc" ? "l" : "l_desc";
            ViewData["PtSort"] =
                    string.IsNullOrEmpty(sortOrder) || sortOrder == "pt_desc" ? "pt" : "pt_desc";
            #endregion

            List<List<Team>> groupList = new();
            groupList.Add(SortLeague(new() { LeagueTable.First(x => x.TeamName == a[0]), LeagueTable.First(x => x.TeamName == a[1]), LeagueTable.First(x => x.TeamName == a[2]), LeagueTable.First(x => x.TeamName == a[3]) }, sortOrder));
            groupList.Add(SortLeague(new() { LeagueTable.First(x => x.TeamName == b[0]), LeagueTable.First(x => x.TeamName == b[1]), LeagueTable.First(x => x.TeamName == b[2]), LeagueTable.First(x => x.TeamName == b[3]) }, sortOrder));
            groupList.Add(SortLeague(new() { LeagueTable.First(x => x.TeamName == c[0]), LeagueTable.First(x => x.TeamName == c[1]), LeagueTable.First(x => x.TeamName == c[2]), LeagueTable.First(x => x.TeamName == c[3]) }, sortOrder));
            groupList.Add(SortLeague(new() { LeagueTable.First(x => x.TeamName == d[0]), LeagueTable.First(x => x.TeamName == d[1]), LeagueTable.First(x => x.TeamName == d[2]), LeagueTable.First(x => x.TeamName == d[3]) }, sortOrder));
            groupList.Add(SortLeague(new() { LeagueTable.First(x => x.TeamName == e[0]), LeagueTable.First(x => x.TeamName == e[1]), LeagueTable.First(x => x.TeamName == e[2]), LeagueTable.First(x => x.TeamName == e[3]) }, sortOrder));
            groupList.Add(SortLeague(new() { LeagueTable.First(x => x.TeamName == f[0]), LeagueTable.First(x => x.TeamName == f[1]), LeagueTable.First(x => x.TeamName == f[2]), LeagueTable.First(x => x.TeamName == f[3]) }, sortOrder));
            groupList.Add(SortLeague(new() { LeagueTable.First(x => x.TeamName == g[0]), LeagueTable.First(x => x.TeamName == g[1]), LeagueTable.First(x => x.TeamName == g[2]), LeagueTable.First(x => x.TeamName == g[3]) }, sortOrder));
            groupList.Add(SortLeague(new() { LeagueTable.First(x => x.TeamName == h[0]), LeagueTable.First(x => x.TeamName == h[1]), LeagueTable.First(x => x.TeamName == h[2]), LeagueTable.First(x => x.TeamName == h[3]) }, sortOrder));
            
            return View(groupList);
        }

        // GET: JsonMatches/Edit/5
        public IActionResult Edit(int id)
        {
            JsonMatch jsonMatch = r.Matches.Where(m => m.ID == id).FirstOrDefault();
            return View(jsonMatch);
        }

        // POST: JsonMatches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(JsonMatch jsonMatch)
        {
            JsonMatch match = r.Matches.Where(m => m.ID == jsonMatch.ID).FirstOrDefault();
            match.MatchDay = match.MatchDay;
            match.MatchMonth = match.MatchMonth;
            match.MatchYear = match.MatchYear;
            match.MatchHour = match.MatchHour;
            match.MatchMinute = match.MatchMinute;
            match.HomeTeam = jsonMatch.HomeTeam;
            match.HomeScore = jsonMatch.HomeScore == "" ? "" : jsonMatch.HomeScore;
            match.AwayScore = jsonMatch.AwayScore == "" ? "" : jsonMatch.AwayScore;
            match.AwayTeam = jsonMatch.AwayTeam;
            PopulateMatchList();

            JsonSaver saver = new(MatchList, filePath);
            return RedirectToAction("WC22Matches");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
