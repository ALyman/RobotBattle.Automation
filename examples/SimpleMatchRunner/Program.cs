﻿#region Copyright & License

// Copyright (C) 2011 by Alex Lyman
// RobotBattle.Automation is licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php

#endregion

using System;
using System.Xml;
using RobotBattle.Automation;

namespace SimpleMatchRunner
{
    internal class Program
    {
        private static void Main()
        {
            Console.SetWindowSize(120, 40);
            Console.SetBufferSize(120, 1000);

            var match = new MatchBuilder { Games = 10 };
            match.Teams.Add(
                RobotBuilder.FromFile(@"C:\Program Files (x86)\Robot Battle\fire.prg"),
                RobotBuilder.FromFile(@"C:\Program Files (x86)\Robot Battle\fire.prg"),
                RobotBuilder.FromFile(@"C:\Program Files (x86)\Robot Battle\fire.prg")
                );
            match.Teams.Add(
                RobotBuilder.FromFile(@"C:\Program Files (x86)\Robot Battle\fire.prg"),
                RobotBuilder.FromFile(@"C:\Program Files (x86)\Robot Battle\fire.prg")
                );
            using (var writer = XmlWriter.Create(Console.Out, new XmlWriterSettings { Indent = true })) {
                match.ToXml().WriteTo(writer);
            }
            Console.WriteLine();
            var runner = new MatchRunner(match);
            var result = runner.Run();
            using (var writer = XmlWriter.Create(Console.Out, new XmlWriterSettings { Indent = true })) {
                result.ToXml().WriteTo(writer);
            }
            Console.ReadKey();
        }
    }
}