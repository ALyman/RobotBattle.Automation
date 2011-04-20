using System;
using System.Xml.Linq;

namespace RobotBattle.Automation
{
    public class RobotGameStatistics
    {
        public int Game { get; set; }
        public int Place { get; set; }
        public int TotalKillsFromRobots { get; set; }
        public int TotalKillsToRobots { get; set; }
        // TODO: load additional statistics

        public XElement ToXml()
        {
            return new XElement(
                MatchResult.Namespace + "robot",
                new XAttribute("Game", Game),
                new XAttribute("Place", Place),
                new XAttribute("TotalKillsToRobots", TotalKillsToRobots),
                new XAttribute("TotalKillsFromRobots", TotalKillsFromRobots)
                );
        }
    }
}