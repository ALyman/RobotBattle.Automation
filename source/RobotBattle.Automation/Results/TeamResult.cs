using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RobotBattle.Automation
{
	public class TeamResult
	{
		public string Name;
		public List<RobotResult> Robots = new List<RobotResult>();
		public int TotalScore;

		public XElement ToXml()
		{
			return new XElement(MatchResult.Namespace + "team",
			                    from robot in Robots
			                    select robot.ToXml(),
			                    new XAttribute("name", Name),
			                    new XAttribute("totalScore", TotalScore)
				);
		}
	}
}