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
            Games = 10;

            Teams = new Collection<TeamBuilder>();
        }

        public int Games { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        public Version Compatibility { get; set; }

        public bool? ZeroStartHeading { get; set; }
        public bool? NewStartPos { get; set; }
        public int? StartEnergy { get; set; }
        public int? CeaseFire { get; set; }
        public string Name { get; set; }
        public ushort? TurnMode { get; set; }
        public int? RadarRange { get; set; }
        public int? MissileRange { get; set; }
        public bool? AllowSliding { get; set; }
        public bool? FastMissiles { get; set; }
        public bool? DecelStops { get; set; }
        public bool? UpdatedAccel { get; set; }
        public bool? AdditiveRotation { get; set; }
        public bool? AllowRadio { get; set; }
        public int? Timeout { get; set; }
        public int? CMRate { get; set; }

        public ICollection<TeamBuilder> Teams { get; private set; }
        public MatchResult Results { get; set; }

        public XDocument ToXml()
        {
            return new XDocument(
                new XElement(
                    Namespace + "match",
                    from team in Teams
                    where team.Robots.Any()
                    select team.ToXml(),
                    new XAttribute("games", Games),
                    Name == null ? null : new XAttribute("name", Name),
                    Width == null ? null : new XAttribute("width", Width),
                    Height == null ? null : new XAttribute("height", Height),
                    Compatibility == null
                        ? null
                        : new XAttribute("compat", string.Join(".", Compatibility.Major, Compatibility.Minor)),
                    ZeroStartHeading == null ? null : new XAttribute("zerostartheading", ToXml(ZeroStartHeading)),
                    NewStartPos == null ? null : new XAttribute("newstartpos", ToXml(NewStartPos)),
                    StartEnergy == null ? null : new XAttribute("startenergy", StartEnergy),
                    CeaseFire == null ? null : new XAttribute("ceasefire", CeaseFire),
                    TurnMode == null ? null : new XAttribute("turnmode", TurnMode),
                    RadarRange == null ? null : new XAttribute("radarrange", RadarRange),
                    MissileRange == null ? null : new XAttribute("missilerange", MissileRange),
                    AllowSliding == null ? null : new XAttribute("allowsliding", ToXml(AllowSliding)),
                    FastMissiles == null ? null : new XAttribute("fastmissiles", ToXml(FastMissiles)),
                    DecelStops == null ? null : new XAttribute("decelstops", ToXml(DecelStops)),
                    UpdatedAccel == null ? null : new XAttribute("updatedaccel", ToXml(UpdatedAccel)),
                    AdditiveRotation == null ? null : new XAttribute("additiverotation", ToXml(AdditiveRotation)),
                    AllowRadio == null ? null : new XAttribute("allowradio", ToXml(AllowRadio)),
                    Timeout == null ? null : new XAttribute("timeout", Timeout),
                    CMRate == null ? null : new XAttribute("cmrate", CMRate)
                    )
                );
        }

        private int? ToXml(bool? value)
        {
            if (value != null)
                return Convert.ToInt32((bool) value);

            return null;
        }
    }
}