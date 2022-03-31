using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Scores.Models
{
	public class JsonLoader
	{
		readonly string filePath = @".\wwwroot\files\pl.json";
		public List<JsonMatch> Matches { get; set; }
		public JsonLoader()
		{
			Matches = JsonConvert.DeserializeObject<List<JsonMatch>>(File.ReadAllText(filePath));
		}		
	}
}
