#region Copyright & License

// Copyright (C) 2011 by Alex Lyman
// RobotBattle.Automation is licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php

#endregion

using System;
using System.Collections.Generic;

namespace RobotBattle.Automation
{
    public static class Extensions
    {
        public static TeamBuilder Add(this ICollection<TeamBuilder> teams, params RobotBuilder[] robots)
        {
            var team = new TeamBuilder();
            foreach (var robot in robots) {
                team.Robots.Add(robot);
            }
            teams.Add(team);
            return team;
        }
    }
}