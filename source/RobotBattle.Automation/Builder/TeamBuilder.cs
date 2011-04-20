using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace RobotBattle.Automation
{
	public class TeamBuilder
	{
		public TeamBuilder()
		{
			Robots = new Collection<RobotBuilder>();
		}

		public ICollection<RobotBuilder> Robots { get; private set; }

		public XElement ToXml()
		{
			return new XElement(MatchBuilder.Namespace + "team",
			                    from robot in Robots
			                    select robot.ToXml());
		}
	}
}