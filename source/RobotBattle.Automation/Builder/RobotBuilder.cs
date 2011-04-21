#region Copyright & License

// Copyright (C) 2011 by Alex Lyman
// RobotBattle.Automation is licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php

#endregion

using System;
using System.IO;
using System.Xml.Linq;
using RobotBattle.Automation.Builder;

namespace RobotBattle.Automation
{
    public class RobotBuilder
    {
        private RobotBuilder()
        {
        }

        public Point? Position { get; set; }
        public int? Heading { get; set; }
        public string FileName { get; private set; }

        public static RobotBuilder FromFile(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("can not find the specified robot", fileName);
            return new RobotBuilder {
                FileName = fileName
            };
        }

        public XElement ToXml()
        {
            return new XElement(
                MatchBuilder.Namespace + "robot",
                new XElement(MatchBuilder.Namespace + "file", FileName),
                Position == null ? null : new XAttribute("xpos", Position.Value.X),
                Position == null ? null : new XAttribute("ypos", Position.Value.Y),
                Heading == null ? null : new XAttribute("heading", Heading)
                );
        }
    }
}