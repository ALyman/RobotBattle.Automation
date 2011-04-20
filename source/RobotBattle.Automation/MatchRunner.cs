using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace RobotBattle.Automation
{
	public class MatchRunner
	{
		private readonly MatchBuilder matchBuilder;

		public MatchRunner(MatchBuilder matchBuilder)
		{
			this.matchBuilder = matchBuilder;
		}

		public Task<MatchResult> RunAsync()
		{
			return Task.Factory.StartNew<MatchResult>(Run, TaskCreationOptions.LongRunning);
		}

		public MatchResult Run()
		{
			const string robotBattleExe = @"C:\Program Files (x86)\Robot Battle\winrob32.exe";

			string loadListFile = Path.GetTempPath() + "loadlist.ll";
			string scoreLogFile = Path.GetTempPath() + "score.log";
			string statsLogFile = Path.GetTempPath() + "stats.log";

			using (XmlWriter writer = XmlWriter.Create(loadListFile, new XmlWriterSettings { Indent = true })) {
				matchBuilder.ToXml().WriteTo(writer);
			}


			var processStartInfo = new ProcessStartInfo {
				FileName = robotBattleExe,
				Arguments = string.Format(
					"\"{0}\" /t /lf \"{1}\" /slf \"{2}\" /slg 1 /lo 1 /slo 1",
					loadListFile,
					scoreLogFile,
					statsLogFile
					),
#if DEBUG
				WindowStyle = ProcessWindowStyle.Normal
#else
				UseShellExecute = false,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Minimized
#endif
			};

			Process process = Process.Start(processStartInfo);
			process.WaitForExit();

			return MatchResult.FromFiles(loadListFile, scoreLogFile, statsLogFile);
		}
	}
}