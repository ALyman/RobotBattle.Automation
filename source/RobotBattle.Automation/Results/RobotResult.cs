using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RobotBattle.Automation
{
	public class RobotResult
	{
		public string Author;
		public string FileName;
		public string Name;
		public int[] Places;
		public List<RobotGameStatistics> Statistics = new List<RobotGameStatistics>();
		public int TotalScore;
		public string Version;

		public XElement ToXml()
		{
			return new XElement(MatchResult.Namespace + "robot",
			                    new XAttribute("name", Name),
			                    new XAttribute("version", Version),
			                    new XAttribute("author", Author),
			                    new XAttribute("totalScore", TotalScore),
			                    new XElement(MatchResult.Namespace + "file", FileName),
			                    Places.Select((count, index) => new XElement(MatchResult.Namespace + "place",
			                                                                 new XAttribute("place", index + 1),
			                                                                 new XAttribute("count", count)
			                                                    	)),
			                    new XElement(MatchResult.Namespace + "statistics",
			                                 from stat in Statistics
			                                 select stat.ToXml()
			                    	)
				);
		}
	}
}