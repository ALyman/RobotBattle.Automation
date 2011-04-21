#region Copyright & License

// Copyright (C) 2011 by Alex Lyman
// RobotBattle.Automation is licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php

#endregion

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

        public string Name { get; set; }
        public ICollection<RobotBuilder> Robots { get; private set; }

        public XElement ToXml()
        {
            return new XElement(
                MatchBuilder.Namespace + "team",
                from robot in Robots
                select robot.ToXml(),
                Name == null ? null : new XAttribute("name", Name)
                );
        }
    }
}