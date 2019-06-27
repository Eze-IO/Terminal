using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Eze.IO.Terminal
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledError;
        }

        private void OnUnhandledError(object s, UnhandledExceptionEventArgs ue)
        {
            var ex = (Exception)ue.ExceptionObject;
            File.WriteAllText("error.log", ex.ToString());
        }
    }
}
