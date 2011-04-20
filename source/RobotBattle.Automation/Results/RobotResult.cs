#region Copyright & License

// Copyright (C) 2011 by Alex Lyman
// RobotBattle.Automation is licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RobotBattle.Automation
{
    public class RobotResult
    {
        public RobotResult()
        {
            Statistics = new List<RobotGameStatistics>();
        }

        public string Author { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public int[] Places { get; set; }
        public List<RobotGameStatistics> Statistics { get; private set; }
        public int TotalScore { get; set; }
        public string Version { get; set; }

        public XElement ToXml()
        {
            return new XElement(
                MatchResult.Namespace + "robot",
                new XAttribute("name", Name),
                new XAttribute("version", Version),
                new XAttribute("author", Author),
                new XAttribute("totalScore", TotalScore),
                new XElement(MatchResult.Namespace + "file", FileName),
                Places.Select(
                    (count, index) => new XElement(
                                          MatchResult.Namespace + "place",
                                          new XAttribute("place", index + 1),
                                          new XAttribute("count", count)
                                          )
                    ),
                new XElement(
                    MatchResult.Namespace + "statistics",
                    from stat in Statistics
                    select stat.ToXml()
                    )
                );
        }
    }
}