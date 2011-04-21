#region Copyright & License

// Copyright (C) 2011 by Alex Lyman
// RobotBattle.Automation is licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php

#endregion

using System;

namespace RobotBattle.Automation.Builder
{
    public struct Point
    {
        private int x;

        private int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X { get { return x; } set { x = value; } }

        public int Y { get { return y; } set { y = value; } }
    }
}