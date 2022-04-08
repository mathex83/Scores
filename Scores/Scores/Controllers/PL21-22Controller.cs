using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Scores.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Scores.Controllers
{
    public class PL21_22Controller : Controller
    {
        private readonly ILogger<PL21_22Controller> _logger;
        private static readonly string filePath = @".\wwwroot\files\pl.json";
        private static readonly JsonLoader r = new(filePath);
        private readonly List<Match> MatchList = new();
        private List<Team> LeagueTable = new();
        Team replaceTeam = new();

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
        public PL21_22Controller(ILogger<PL21_22Controller> logger)
        {
            _logger = logger;
        }

        public IActionResult PL2122Matches(string sortOrder)
        {
            PopulateMatchList();
            var matches = from m in MatchList select m;
            foreach (Match m in matches)
            {
                m.HomeTeam = replaceTeam.TeamNameReplace(m.HomeTeam);
                m.AwayTeam = replaceTeam.TeamNameReplace(m.AwayTeam);
            }

            #region Input for MatchSorting
            ViewData["DateSort"]=
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
        public IActionResult PL2122Table(string sortOrder)
        {
            PopulateLeagueTable();
            var league = from t in LeagueTable select t;
            foreach (Team t in league)
            {
                t.TeamName = replaceTeam.TeamNameReplace(t.TeamName);
            }

            #region Input for LeagueSorting
            ViewData["WSort"] =
                    string.IsNullOrEmpty(sortOrder) || sortOrder == "w_desc" ? "w" : "w_desc";
            ViewData["DSort"] =
                    string.IsNullOrEmpty(sortOrder) || sortOrder == "d_desc" ? "d" : "d_desc";
            ViewData["LSort"] =
                    string.IsNullOrEmpty(sortOrder) || sortOrder == "l_desc" ? "l" : "l_desc";
            ViewData["PtSort"] =
                    string.IsNullOrEmpty(sortOrder) || sortOrder == "pt_desc" ? "pt" : "pt_desc";
            ViewData["PtMax"] =
                string.IsNullOrEmpty(sortOrder) || sortOrder == "ptMax_desc" ? "ptMax" : "ptMax_desc";
            #endregion

            #region LeagueSortingSwitch
            switch (sortOrder)
            {
                case "w":
                    league = league.OrderBy(t => t.MatchesWon);
                    break;
                case "w_desc":
                    league = league.OrderByDescending(t => t.MatchesWon);
                    break;
                case "d":
                    league = league.OrderBy(t => t.MatchesDrawn);
                    break;
                case "d_desc":
                    league = league.OrderByDescending(t => t.MatchesDrawn);
                    break;
                case "l":
                    league = league.OrderBy(t => t.MatchesLost);
                    break;
                case "l_desc":
                    league = league.OrderByDescending(t => t.MatchesLost);
                    break;
                case "pt":
                    league = league.OrderBy(t => t.Points);
                    break;
                case "pt_desc":
                    league = league.OrderByDescending(t => t.Points);
                    break;
                case "ptMax":
                    league = league.OrderBy(t => t.MaxPoints);
                    break;
                case "ptMax_desc":
                    league = league.OrderByDescending((t) => t.MaxPoints);
                    break;
                default:
                    league = league.OrderByDescending(t => t.Points)
                        .ThenByDescending(t => t.GoalDifference);
                    break;
            }
            #endregion

            return View(league);
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
            return RedirectToAction("PL2122Matches");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
