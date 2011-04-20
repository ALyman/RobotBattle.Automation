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