using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Scores.Models
{
	public class JsonLoader
	{		
		public List<JsonMatch> Matches { get; set; }
		public JsonLoader(string filePath)
		{
			Matches = JsonConvert.DeserializeObject<List<JsonMatch>>(File.ReadAllText(filePath));
		}		
	}
}
