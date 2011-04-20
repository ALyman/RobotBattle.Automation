using System;
using System.Xml.Linq;

namespace RobotBattle.Automation
{
	public class RobotGameStatistics
	{
		public int Game;
		public int Place;
		public int TotalKillsFromRobots;
		public int TotalKillsToRobots;
		// TODO: load additional statistics

		public int KillsToRobots { get; set; }

		public XElement ToXml()
		{
			return new XElement(MatchResult.Namespace + "robot",
			                    new XAttribute("Game", Game),
			                    new XAttribute("Place", Place),
			                    new XAttribute("TotalKillsToRobots", TotalKillsToRobots),
			                    new XAttribute("TotalKillsFromRobots", TotalKillsFromRobots)
				);
		}
	}
}