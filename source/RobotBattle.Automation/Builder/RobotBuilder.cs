using System;
using System.IO;
using System.Xml.Linq;

namespace RobotBattle.Automation
{
	public class RobotBuilder
	{
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
			return new XElement(MatchBuilder.Namespace + "robot",
			                    new XElement(MatchBuilder.Namespace + "file", FileName));
		}
	}
}