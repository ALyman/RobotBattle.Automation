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
    public class RobotGameStatistics
    {
        public RobotGameStatistics()
        {
            HitsToRobots = new Dictionary<RobotResult, int>();
            HitsFromRobots = new Dictionary<RobotResult, int>();
            KillsToRobots = new Dictionary<RobotResult, int>();
            KillsFromRobots = new Dictionary<RobotResult, int>();
            DamageToRobots = new Dictionary<RobotResult, int>();
            DamageFromRobots = new Dictionary<RobotResult, int>();
            CollisionsWithRobots = new Dictionary<RobotResult, int>();
        }

        public int Game { get; set; }
        public int Place { get; set; }

        public int TotalKillsFromRobots { get; set; }
        public int TotalKillsToRobots { get; set; }

        public int KillsFromMines { get; set; }
        public int KillsFromWalls { get; set; }
        public int KillsFromErrors { get; set; }
        public int KillsFromMisc { get; set; }
        public int KillsFromTimeouts { get; set; }
        public int ShotsFired { get; set; }
        public int HitsToMines { get; set; }
        public int HitsToCookies { get; set; }
        public int TotalHitsToRobots { get; set; }
        public int TotalHitsFromRobots { get; set; }
        public int AverageHitStrengthToRobots { get; set; }
        public int TotalDamageToRobots { get; set; }
        public int TotalDamageFromRobots { get; set; }
        public int TotalDamageFromMines { get; set; }
        public int TotalDamageFromWalls { get; set; }
        public int TotalDamageFromJar { get; set; }
        public int TotalBonusFromCookies { get; set; }
        public int TotalCollisionsWithRobots { get; set; }
        public int Points { get; set; }

        public IDictionary<RobotResult, int> HitsToRobots { get; private set; }
        public IDictionary<RobotResult, int> HitsFromRobots { get; private set; }
        public IDictionary<RobotResult, int> KillsToRobots { get; private set; }
        public IDictionary<RobotResult, int> KillsFromRobots { get; private set; }
        public IDictionary<RobotResult, int> DamageToRobots { get; private set; }
        public IDictionary<RobotResult, int> DamageFromRobots { get; private set; }
        public IDictionary<RobotResult, int> CollisionsWithRobots { get; private set; }

        // TODO: load additional statistics

        public XElement ToXml()
        {
            return new XElement(
                MatchResult.Namespace + "robot",
                new XAttribute("Game", Game),
                new XAttribute("Place", Place),
                new XAttribute("Points", Points),
                new XAttribute("KillsFromMines", KillsFromMines),
                new XAttribute("KillsFromWalls", KillsFromWalls),
                new XAttribute("KillsFromErrors", KillsFromErrors),
                new XAttribute("KillsFromMisc", KillsFromMisc),
                new XAttribute("KillsFromTimeouts", KillsFromTimeouts),
                new XAttribute("ShotsFired", ShotsFired),
                new XAttribute("HitsToMines", HitsToMines),
                new XAttribute("HitsToCookies", HitsToCookies),
                new XAttribute("TotalHitsToRobots", TotalHitsToRobots),
                new XAttribute("TotalHitsFromRobots", TotalHitsFromRobots),
                new XAttribute("AverageHitStrengthToRobots", AverageHitStrengthToRobots),
                new XAttribute("TotalDamageToRobots", TotalDamageToRobots),
                new XAttribute("TotalDamageFromRobots", TotalDamageFromRobots),
                new XAttribute("TotalDamageFromMines", TotalDamageFromMines),
                new XAttribute("TotalDamageFromWalls", TotalDamageFromWalls),
                new XAttribute("TotalDamageFromJar", TotalDamageFromJar),
                new XAttribute("TotalBonusFromCookies", TotalBonusFromCookies),
                new XAttribute("TotalCollisionsWithRobots", TotalCollisionsWithRobots),
                new XAttribute("TotalKillsToRobots", TotalKillsToRobots),
                new XAttribute("TotalKillsFromRobots", TotalKillsFromRobots),
                ToXml(HitsToRobots, "HitsToRobots", "Count"),
                ToXml(HitsFromRobots, "HitsFromRobots", "Count"),
                ToXml(KillsToRobots, "KillsToRobots", "Count"),
                ToXml(KillsFromRobots, "KillsFromRobots", "Count"),
                ToXml(DamageToRobots, "DamageToRobots", "Value"),
                ToXml(DamageFromRobots, "DamageFromRobots", "Value"),
                ToXml(CollisionsWithRobots, "CollisionsWithRobots", "Count")
                );
        }

        private XElement ToXml(IDictionary<RobotResult, int> dictionary, string elementName, string valueName)
        {
            return new XElement(MatchResult.Namespace + elementName,
                                from pair in dictionary
                                orderby pair.Key.Id
                                select new XElement(
                                    MatchResult.Namespace + "robot",
                                    new XAttribute("id", pair.Key.Id),
                                    new XAttribute(valueName, pair.Value)
                                    ));
        }
    }
}