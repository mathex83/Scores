using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Scores.Models
{
    public class JsonSaver
    {
		readonly string filePath = @".\wwwroot\files\pl.json";
		
		public JsonSaver(List<Match> matches)
		{
			if (matches.Count != 0)
			{
				string jsonString;
				//JsonSerializer jsonSerializer = new JsonSerializer();
				List<JsonMatch> jsonMatches = new List<JsonMatch>();
				foreach (Match m in matches)
				{
					jsonMatches.Add(new JsonMatch()
					{
						ID = m.ID,
						MatchDate = m.MatchDate,
						HomeTeam = m.HomeTeam,
						HomeScore = m.HomeScore.ToString(),
						AwayScore = m.AwayScore.ToString(),
						AwayTeam = m.AwayTeam
					});
				}
				jsonString =
					JsonConvert.SerializeObject(jsonMatches, Formatting.Indented);
				File.WriteAllText(filePath, jsonString);
			}			
		}
	}
}
