#region Copyright & License

// Copyright (C) 2011 by Alex Lyman
// RobotBattle.Automation is licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php

#endregion

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml;

namespace RobotBattle.Automation
{
    public class MatchRunner
    {
        private readonly bool hideWindow;
        private readonly MatchBuilder matchBuilder;

        public MatchRunner(MatchBuilder matchBuilder, bool hideWindow = true)
        {
            this.matchBuilder = matchBuilder;
            this.hideWindow = hideWindow;
        }

        public Task<MatchResult> RunAsync()
        {
            return Task.Factory.StartNew<MatchResult>(Run, TaskCreationOptions.LongRunning);
        }

        public MatchResult Run()
        {
            const string robotBattleExePath = @"C:\Program Files (x86)\Robot Battle\winrob32.exe";

            var loadListFile = Path.GetTempPath() + "loadlist.ll";
            var scoreLogFile = Path.GetTempPath() + "score.log";
            var statsLogFile = Path.GetTempPath() + "stats.log";

            using (var writer = XmlWriter.Create(loadListFile, new XmlWriterSettings { Indent = true })) {
                matchBuilder.ToXml().WriteTo(writer);
            }


            if (hideWindow) {
                var desktopName = "RobotBattle.Automation." + Guid.NewGuid();
                var desktopPtr = NativeMethods.CreateDesktop(desktopName, null, null, 0, 0xff, IntPtr.Zero);
                try {
                    if (desktopPtr == IntPtr.Zero)
                        throw new InvalidOperationException("Failed to create the secondary desktop");

                    // set startup parameters.
                    var si = new NativeMethods.StartupInfo();
                    si.cb = Marshal.SizeOf(si);
                    si.lpDesktop = desktopName;

                    var pi = new NativeMethods.ProcessInformation();

                    var path = string.Format(
                        "\"{0}\" \"{1}\" /t /lf \"{2}\" /slf \"{3}\" /slg 1 /lo 1 /slo 1",
                        robotBattleExePath,
                        loadListFile,
                        scoreLogFile,
                        statsLogFile
                        );

                    if (
                        !NativeMethods.CreateProcess(null, path, IntPtr.Zero, IntPtr.Zero, true,
                                                     0x00000020, /*NORMAL_PRIORITY_CLASS*/
                                                     IntPtr.Zero, null, ref si, ref pi))
                        throw new InvalidOperationException("Failed to launch the Robot Battle process");

                    Process.GetProcessById(pi.dwProcessId)
                        .WaitForExit();
                } finally {
                    NativeMethods.CloseDesktop(desktopPtr);
                }
            } else {
                Process.Start(new ProcessStartInfo {
                    FileName = robotBattleExePath,
                    Arguments = string.Format(
                        "\"{1}\" /t /lf \"{2}\" /slf \"{3}\" /slg 1 /lo 1 /slo 1",
                        robotBattleExePath,
                        loadListFile,
                        scoreLogFile,
                        statsLogFile
                                  )
                }).WaitForExit();
            }

            return MatchResult.FromFiles(loadListFile, scoreLogFile, statsLogFile);
        }

        #region Nested type: NativeMethods

        [SuppressMessage("StyleCop.CSharp.NameingRules", "SA1307:FieldNamesMustNotBeginWithUnderscore",
            Justification = "P/Invoke structures should have the names from the MSDN documentation")]
        private static class NativeMethods
        {
            // ReSharper disable MemberCanBePrivate.Local

            [DllImport("user32.dll", EntryPoint = "CreateDesktop", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr CreateDesktop(
                [MarshalAs(UnmanagedType.LPWStr)] string desktopName,
                [MarshalAs(UnmanagedType.LPWStr)] string device, // must be null.
                [MarshalAs(UnmanagedType.LPWStr)] string deviceMode, // must be null,
                [MarshalAs(UnmanagedType.U4)] int flags, // use 0
                [MarshalAs(UnmanagedType.U4)] int accessMask,
                [MarshalAs(UnmanagedType.SysInt)] IntPtr attributes);

            [DllImport("user32.dll", EntryPoint = "CloseDesktop", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CloseDesktop(IntPtr handle);

            [DllImport("kernel32.dll")]
            public static extern bool CreateProcess(
                string lpApplicationName,
                string lpCommandLine,
                IntPtr lpProcessAttributes,
                IntPtr lpThreadAttributes,
                bool bInheritHandles,
                int dwCreationFlags,
                IntPtr lpEnvironment,
                string lpCurrentDirectory,
                ref StartupInfo lpStartupInfo,
                ref ProcessInformation lpProcessInformation
                );

            #region Nested type: ProcessInformation

            [StructLayout(LayoutKind.Sequential)]
            public struct ProcessInformation
            {
                public readonly IntPtr hProcess;
                public readonly IntPtr hThread;
                public readonly int dwProcessId;
                public readonly int dwThreadId;
            }

            #endregion

            #region Nested type: StartupInfo

            [StructLayout(LayoutKind.Sequential)]
            public struct StartupInfo
            {
                public int cb;
                public readonly string lpReserved;
                public string lpDesktop;
                public readonly string lpTitle;
                public readonly int dwX;
                public readonly int dwY;
                public readonly int dwXSize;
                public readonly int dwYSize;
                public readonly int dwXCountChars;
                public readonly int dwYCountChars;
                public readonly int dwFillAttribute;
                public readonly int dwFlags;
                public readonly short wShowWindow;
                public readonly short cbReserved2;
                public readonly IntPtr lpReserved2;
                public readonly IntPtr hStdInput;
                public readonly IntPtr hStdOutput;
                public readonly IntPtr hStdError;
            }

            #endregion

            // ReSharper restore MemberCanBePrivate.Local
        }

        #endregion
    }
}