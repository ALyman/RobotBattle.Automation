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
    public class MatchBuilder
    {
        public static readonly XNamespace Namespace = "x-schema:res://winrob32.exe/listschema.xml";

        public MatchBuilder()
        {
            Games = 100;
            Width = 400;
            Height = 400;
            Teams = new Collection<TeamBuilder>();
        }

        public int Games { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ICollection<TeamBuilder> Teams { get; private set; }

        public XDocument ToXml()
        {
            return new XDocument(
                new XElement(
                    Namespace + "match",
                    from team in Teams
                    where team.Robots.Any()
                    select team.ToXml(),
                    new XAttribute("games", Games),
                    new XAttribute("width", Width),
                    new XAttribute("height", Height)
                    )
                );
        }
    }
}