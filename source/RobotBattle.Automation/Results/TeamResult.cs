﻿#region Copyright & License

// Copyright (C) 2011 by Alex Lyman
// RobotBattle.Automation is licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RobotBattle.Automation
{
    public class TeamResult
    {
        public TeamResult()
        {
            Robots = new List<RobotResult>();
        }

        public string Name { get; set; }
        public List<RobotResult> Robots { get; private set; }
        public int TotalScore { get; set; }

        public XElement ToXml()
        {
            return new XElement(
                MatchResult.Namespace + "team",
                from robot in Robots
                select robot.ToXml(),
                new XAttribute("name", Name),
                new XAttribute("totalScore", TotalScore)
                );
        }
    }
}