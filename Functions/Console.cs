using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Eze.IO.Terminal.UI;

namespace Eze.IO.Terminal.Functions
{
    static class ConsoleHandler
    {        
        public static event Action OnCommandClose;
        public static event Action OnCommandRun;
        public static event Action OnTrueExit;
        public static event ConsoleOutputHandler OnDirectoryChange;
        public static event ConsoleOutputHandler OnTitleChange;
        public static event ConsoleOutputHandler OnOutput;
        private static List<String> consoleCalls = new List<string>();

        private static string consoleInput = string.Empty;
        public static string ConsoleInput
        {
            get
            {
                return consoleInput;
            }
            private set
            {
                consoleInput = value;
                consoleCalls.Add(consoleInput);
            }
        }

        private static int commandCount = 0;
        public static string GetCall(ConsoleCall call = ConsoleCall.Restart)
        {
            try
            {
                int x = 0;
                var c = consoleCalls.Count;
                //MessageBox.Show(c.ToString());
                switch (call)
                {
                    case ConsoleCall.Last:
                        if (c > 0)
                        {
                            x = (commandCount - 1);
                            if (commandCount == 0)
                                return consoleCalls.Last();
                            if (x <= -1)
                                break;
                            else
                                commandCount = x;
                            return consoleCalls[x];
                        }
                        break;
                    case ConsoleCall.Previous:
                        if (c > 0)
                        {
                            x = (commandCount + 1);
                            if (commandCount == 0)
                                return consoleCalls.First();
                            if (x <= -1 || x > c)
                                break;
                            else
                                commandCount = x;
                            return consoleCalls[x];
                        }
                        break;
                    default:
                        break;
                }
                return null;
            }
            catch(Exception ew) { return ew.ToString(); }
        }

        private static Process p = null;
        private static string suggestedTitle = null;
        public static Task RunCommand(string command = null)
        {
            return Task.Run(async() =>
            {
                try
                {
                    p = new Process();
                    p.StartInfo = new ProcessStartInfo()
                    {
                        FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"),
                        Arguments = (String.IsNullOrEmpty(command) ? String.Format("/c cd {0}", _currentDir) : String.Format("/c cd {0} & {1} & exit", _currentDir, command)),
                        WorkingDirectory = _currentDir,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        CreateNoWindow = true
                    };
                    p.EnableRaisingEvents = true;
                    p.Exited += OnConsoleExit;
                    try
                    {
                        try { p.Start(); } catch { /* do nothing */ }
                        OnCommandRun();
                        MessageBox.Show(consoleCalls.Count.ToString());
                    }
                    catch { /* do nothing */ }                   
                    if (string.IsNullOrEmpty(suggestedTitle))
                        suggestedTitle = command;

                    if (string.IsNullOrEmpty(command))
                        if (!p.HasExited)
                            p.Kill();

                    ConsoleInput = command;
                    OnDirectoryChange?.Invoke(_currentDir);
                    if (!string.IsNullOrEmpty(command))
                    {
                        OnTitleChange?.Invoke(suggestedTitle);
                        await sendInput(command);
                    }
                    await getOutput();

                    p.WaitForExit();
                    OnCommandClose?.Invoke();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            });
        }

        private static void OnConsoleExit(object _s, EventArgs _e)
        {
            if (!string.IsNullOrEmpty(ConsoleInput))
                foreach (var x in ConsoleInput.ToLower().Split(' '))
                    if(!string.IsNullOrEmpty(x))
                        if (x == "exit")
                            OnTrueExit?.Invoke();
            consoleCalls.Clear();
        }

        private static Task getOutput()
        {
            return Task.Run(async() =>
            {
                var o = p.StandardOutput;
                var er = p.StandardError;
                while (!o.EndOfStream)
                {
                    await sendOutput(await o.ReadLineAsync());
                }  
                while (!er.EndOfStream)
                {
                    OnTitleChange?.Invoke(null);
                    await sendOutput(await er.ReadLineAsync());
                }
                if (o != null)
                    o.Close();
                if (er != null)
                    er.Close();
            });
        }

        private static Task sendInput(String input)
        {
            return Task.Run(() =>
            {
                if (p != null)
                {
                    var i = p.StandardInput;
                    if (i != null)
                    {
                        if (i.BaseStream.CanWrite)
                            i.Write(input);
                        i.Flush();
                    }
                }
            });
        }

        private static String _currentDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        private static Task sendOutput(String o)
        {
            return Task.Run(() =>
            {
                if (p != null)
                {
                    string path = string.Empty;
                    if (o.Contains('>'))
                        path = (Directory.Exists(path = o.Replace(">", string.Empty)) ? path : string.Empty);
                    if (!string.IsNullOrEmpty(path))
                        OnDirectoryChange?.Invoke(_currentDir = path);
                    else
                        OnOutput?.Invoke(o);
                }
            });
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
