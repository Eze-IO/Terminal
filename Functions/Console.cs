using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Eze.IO.Terminal.UI;

namespace Eze.IO.Terminal.Functions
{
    static class ConsoleHandler
    {
        public static event ConsoleOutputHandler OnOutput;
        public static event Action OnCommandClose;
        public static event Action OnCommandRun;
        private static List<String> consoleCalls = new List<string>();

        private static string consoleInput = string.Empty;
        public static string ConsoleInput
        {
            get
            {
                return consoleInput;
            }
            set
            {
                consoleCalls.Add(value);
                consoleInput = value;
            }
        }

        public static string GetCall(ConsoleCall call = ConsoleCall.Restart)
        {
            try
            {
                var c = consoleCalls.Count;
                if (call == ConsoleCall.Restart)
                {  }
                

                if (call == ConsoleCall.Previous)
                {
                    if (consoleCalls.Contains(ConsoleInput))
                    {
                        var x = consoleCalls.FindIndex(v => v.StartsWith(ConsoleInput))+1;
                        if (consoleCalls.ElementAtOrDefault(x) != null)
                            return consoleCalls[x];
                    }
                }

                if (call == ConsoleCall.Last)
                {
                    if (consoleCalls.Contains(ConsoleInput))
                    {
                        var x = consoleCalls.FindIndex(v => v.StartsWith(ConsoleInput))-1;
                        if (consoleCalls.ElementAtOrDefault(x) != null)
                            return consoleCalls[x];
                    }
                }

                return null;
            }
            catch(Exception ew) { return ew.ToString(); }
        }

        public static async void RunCommand(string command = null)
        {
            if (command == null)
                command = ConsoleInput;

            var x = ConsoleInput.ToLower();
            if (x == "exit" || x == "exits.exe")
                Application.Current.Shutdown(0);

            List<Process> pList = new List<Process>();
            foreach (string cs in command.Split('&'))
            {
                var f = cs.Split(' ')[0];
                var a = cs.Replace(f, string.Empty);
                var _f = f;


                if (Path.GetExtension(_f) != ".exe")
                   _f = Path.ChangeExtension(_f, ".exe");
                    
                if (!File.Exists(_f))
                {
                    OnCommandRun?.Invoke();
                    OnOutput?.Invoke(string.Format("'{0}' is not a recognized file or program.", f));
                    OnCommandClose?.Invoke();
                    return;
                }

                OnCommandRun?.Invoke();
                var p = Process.Start(new ProcessStartInfo()
                {
                    FileName = _f,
                    Arguments = a,
                    //WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                });

                pList.Add(p);
                p.OutputDataReceived += (o, s) => OnOutput?.Invoke(s.Data);
            }

            foreach (Process _p in pList)
            {
                while (!_p.HasExited)
                { await Task.Delay(1000); }
            }
            ConsoleInput = null;
            OnCommandClose?.Invoke();
        }
    }

    public delegate void ConsoleOutputHandler(String output);

    public enum ConsoleCall
    {
        Restart = 0x00,
        Previous = 0x02,
        Last = 0x04
    }
}
