#region Copyright & License

// Copyright (C) 2011 by Alex Lyman
// RobotBattle.Automation is licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RobotBattle.Automation
{
    public class MatchResult
    {
        public static readonly XNamespace Namespace = "uri:RobotBattle.Automation.MatchResult";

        public MatchResult()
        {
            Teams = new List<TeamResult>();
        }

        public string StatsLogFile { get; set; }

        public string ScoreLogFile { get; set; }

        public string LoadListFile { get; set; }

        public List<TeamResult> Teams { get; private set; }

        public static MatchResult FromFiles(string loadListFile, string scoreLogFile, string statsLogFile)
        {
            var result = new MatchResult {
                LoadListFile = loadListFile,
                ScoreLogFile = scoreLogFile,
                StatsLogFile = statsLogFile,
            };

            result.ReadScoreLog();
            result.ReadStatsLog();

            return result;
        }

        public XDocument ToXml()
        {
            return new XDocument(
                new XElement(
                    Namespace + "match",
                    from team in Teams
                    where team.Robots.Any()
                    select team.ToXml()
                    )
                );
        }

        private void ReadScoreLog()
        {
            List<string> robotLines;
            List<string> teamLines;
            using (var scoreLog = File.ReadAllLines(ScoreLogFile).AsEnumerable().GetEnumerator()) {
                scoreLog.SkipUntil(line => line.StartsWith("____Robots"));
                robotLines = scoreLog.Take(Int32.Parse(GetCsvValue(scoreLog.Current, "____Robots"))).ToList();
                teamLines = scoreLog.Take(Int32.Parse(GetCsvValue(scoreLog.Current, "____Teams"))).ToList();
            }

            var robotsByName = (from robotLine in robotLines
                                let split = robotLine.Split(',')
                                select new RobotResult {
                                    Name = split[0],
                                    Version = split[1],
                                    Author = split[2],
                                    FileName = split[3],
                                    TotalScore = Int32.Parse(split[4]),
                                    Places = split.Skip(5).Select(Int32.Parse).ToArray()
                                }).ToDictionary(r => r.Name);

            foreach (var teamLine in teamLines) {
                var split = teamLine.Split(',');
                var team = new TeamResult {
                    Name = split[0],
                    TotalScore = Int32.Parse(split[1])
                };
                Teams.Add(team);
                team.Robots.AddRange(
                    from name in split.Skip(3)
                    select robotsByName[name]);
            }
        }

        private void ReadStatsLog()
        {
            var robotsByName = (from team in Teams
                                from robot in team.Robots
                                select robot).ToDictionary(r => r.Name);

            using (var statsLog = File.ReadAllLines(StatsLogFile).AsEnumerable().GetEnumerator()) {
                statsLog.SkipUntil(line => line.StartsWith("____Robots"));
                var robotLines = statsLog.Take(Int32.Parse(GetCsvValue(statsLog.Current, "____Robots")));

                var robotsById = (from line in robotLines
                                  let split = line.Split(',')
                                  select
                                      new {
                                          Id = Int32.Parse(split[4]),
                                          Robot = robotsByName[split[0]]
                                      })
                    .ToDictionary(r => r.Id, r => r.Robot);

                var headerLine = statsLog.Current.Split(',');
                foreach (var line in statsLog.TakeWhile(l => true)) {
                    var split = line.Split(',');
                    var values = headerLine.Zip(
                        split,
                        (header, value) =>
                        Tuple.Create(header, Int32.Parse(value)))
                        .ToDictionary(i => i.Item1, i => i.Item2);

                    var robot = robotsById[values["id"]];
                    var stats = new RobotGameStatistics {
                        Game = values["game"],
                        Place = values["_place"],
                        TotalKillsToRobots = values["_killstorobots[*]"],
                        TotalKillsFromRobots = values["_killsfromrobots[*]"],
                        // TODO: load additional statistics
                    };
                    robot.Statistics.Add(stats);
                }
            }
        }

        private string GetCsvValue(string line, string name)
        {
            var split = line.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length > 2)
                throw new FormatException();
            if (split[0] != name)
                throw new ArgumentException();
            return split[1];
        }
    }
}