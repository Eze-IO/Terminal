using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Eze.IO.Terminal.Functions;

namespace Eze.IO.Terminal.UI
{
    /// <summary>
    /// Class for terminal ui/window
    /// </summary>
    public partial class Terminal : Window
    {
        [DllImport("user32.dll")]
        private static extern bool BlockInput(bool block);

        public Terminal()
        {
            InitializeComponent();
            CrystalCursor.IsEnabled = true;
            this.Cursor = CrystalCursor.AppStarting;
            this.InputBlock.Cursor = CrystalCursor.IBeam;
            this.Title = null;
        }

        private bool IsAdministrator
        {
            get
            {
                using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    return principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
        }

        private new String Title
        {
            get
            {
                var t = this.ConsoleTitle.Text;
                if(IsAdministrator)
                    t = string.Format("Administrator: {0}", t);
                base.Title = t;
                return t;
            }
            set
            {
                this.ConsoleTitle.Text = (String.IsNullOrEmpty(value) ? Assembly.GetEntryAssembly().Location : value);
                if(IsAdministrator)
                    this.ConsoleTitle.Text = string.Format("Administrator: {0}", this.ConsoleTitle.Text);
                base.Title = this.ConsoleTitle.Text;
            }
        }

        private void Box_Unloaded(object sender, RoutedEventArgs e)
        {
            //
        }

        private async void InputBlock_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox input = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                this.Cursor = CrystalCursor.AppStarting;
                await ConsoleHandler.RunCommand(input.Text);
                this.Cursor = CrystalCursor.Arrow;
            }

            if (e.Key == Key.Up)
            {
                var x = ConsoleHandler.GetCall(ConsoleCall.Previous);
                if (!string.IsNullOrEmpty(x))
                    input.Text = x;
                input.Focus();
                Scroller.ScrollToBottom();
            }

            if (e.Key == Key.Down)
            {
                var x = ConsoleHandler.GetCall(ConsoleCall.Last);
                if(!string.IsNullOrEmpty(x))
                    input.Text = x;
                input.Focus();
                Scroller.ScrollToBottom();
            }
        }

        private async void Box_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Scroller.Cursor = CrystalCursor.Hand;
                ConsoleHandler.OnOutput += ConsoleHandler_OnOutput;
                ConsoleHandler.OnCommandClose += ConsoleHandler_OnCommandClose;
                ConsoleHandler.OnCommandRun += ConsoleHandler_OnCommandRun;
                ConsoleHandler.OnDirectoryChange += ConsoleHandler_OnDirectoryChange;
                ConsoleHandler.OnTitleChange += ConsoleHandler_OnTitleChange;
                ConsoleHandler.OnTrueExit += ConsoleHandler_OnConsoleExit;
                this.Cursor = CrystalCursor.ArrowCD;
                
                prepareConsole();
                SetInputEvents(GetCurrentInput(input));
                await ConsoleHandler.RunCommand(null);
                this.Activate();
            }
            catch (Exception ex) { throw ex; }
        }

        private void ConsoleHandler_OnTitleChange(string output)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Title = output;
            });           
        }

        private void ConsoleHandler_OnDirectoryChange(string output)
        {
            this.Dispatcher.Invoke(() =>
            {
                SetDirectoryPath(input, output);
            });
        }

        private void ConsoleHandler_OnConsoleTitleChange(string output)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Title = output;
            });
        }

        private TextBox output;
        private StackPanel input;
        private Border caret;
        private void prepareConsole()
        {
            try
            {
                input = XamlReader.Parse(XamlWriter.Save(inputSection)) as StackPanel;
                output = XamlReader.Parse(XamlWriter.Save(outputSection)) as TextBox;
                consoleItems.Children.Remove(outputSection);
                consoleItems.Children.Remove(inputSection);
                caret = GetCaret(input);
            }
            catch { return; }
        }

        private void SetInputEvents(TextBox inputBox)
        {
            inputBox.SelectionChanged += (_sender, _e) => AdjustCaret();
            inputBox.LayoutUpdated += (_sender, _e) => AdjustCaret();
            inputBox.LostFocus += InputLostFocus;
            inputBox.GotFocus += InputGotFocus;
            inputBox.Focus();
        }

        private void InputLostFocus(object _sender, RoutedEventArgs _e)
        {
            caret.Visibility = Visibility.Collapsed;
        }

        private void InputGotFocus(object _sender, RoutedEventArgs _e)
        {
            caret.Visibility = Visibility.Visible;
        }

        private void AdjustCaret()
        {
            var caretLocation = InputBlock.GetRectFromCharacterIndex(InputBlock.CaretIndex).Location;

            if (!double.IsInfinity(caretLocation.X))
            {
                Canvas.SetLeft(caret, caretLocation.X);
            }

            if (!double.IsInfinity(caretLocation.Y))
            {
                Canvas.SetBottom(caret, caretLocation.Y);
            }
        }

        private void ConsoleHandler_OnConsoleExit()
        {
            this.Dispatcher.Invoke(() =>
            {
                ConsoleHandler.OnOutput -= ConsoleHandler_OnOutput;
                ConsoleHandler.OnCommandClose -= ConsoleHandler_OnCommandClose;
                ConsoleHandler.OnCommandRun -= ConsoleHandler_OnCommandRun;
                ConsoleHandler.OnDirectoryChange -= ConsoleHandler_OnDirectoryChange;
                Application.Current.Shutdown();
            });
        }

        private void ConsoleHandler_OnCommandRun()
        {
            this.Dispatcher.Invoke(() =>
            {
                BlockInput(true);
                this.consoleItems.IsEnabled = false;
                this.Cursor = CrystalCursor.Wait;

                if (input.Visibility == Visibility.Visible)
                {
                    input.IsEnabled = false;

                    if (caret.Visibility == Visibility.Visible)
                    {
                        //caret = null;
                        RemoveCaret(input);
                    }
                }
            });
        }

        private void ConsoleHandler_OnCommandClose()
        {
            this.Dispatcher.Invoke(() =>
            {
                var i = XamlReader.Parse(XamlWriter.Save(input)) as StackPanel;
                var o = XamlReader.Parse(XamlWriter.Save(output)) as TextBox;
                i.Cursor = CrystalCursor.IBeam;
                o.Text = string.Empty;

                ResetInput(i);
                output = o;
                input = i;
                input.IsEnabled = true;
                caret = GetCaret(input);

                consoleItems.Children.Add(input);
                this.Cursor = CrystalCursor.Arrow;
                Scroller.ScrollToBottom();
                this.consoleItems.IsEnabled = true;
                BlockInput(false);
            });
        }

        private void ConsoleHandler_OnOutput(string o)
        {
            this.Dispatcher.Invoke(() =>
            {
                if(output!=null)
                    if(output.Visibility==Visibility.Visible
                    &&!consoleItems.Children.Contains(output))
                        consoleItems.Children.Add(output);
                if (output!=null)
                    output.Text = (string.IsNullOrEmpty(output.Text)? o:output.Text + Environment.NewLine + o);
            });     
        }

        private TextBox GetCurrentInput(StackPanel container)
        {
            List<TextBox> children = GetLogicalChildCollection<TextBox>(container);
            foreach (var element in children)
                if (element is TextBox &&
                        ((TextBox)element).Name != "inputSign")
                    return ((TextBox)element);
            return null;
        }

        private void ResetInput(StackPanel container)
        {
            List<TextBox> children = GetLogicalChildCollection<TextBox>(container);
            foreach (var element in children)
            {
                if (element is TextBox && 
                    ((TextBox)element).Name != "inputSign")
                {
                    ((TextBox)element).Text = string.Empty;
                    ((TextBox)element).PreviewKeyDown += InputBlock_KeyDown;
                    ((TextBox)element).MouseRightButtonDown += InputBlock_MouseRightButtonDown;
                    SetInputEvents(((TextBox)element));
                }
            }
        }

        private void SetDirectoryPath(StackPanel container, String path)
        {
            List<TextBox> children = GetLogicalChildCollection<TextBox>(container);
            foreach (var element in children)
            {
                if (element is TextBox &&
                    ((TextBox)element).Name == "inputSign")
                {
                    ((TextBox)element).Text = String.Format("{0}>", path);
                }
            }
        }

        private Border GetCaret(object container)
        {
            try
            {
                List<Border> children = GetLogicalChildCollection<Border>(container);
                foreach (var c in children)
                    if (c.Name == "InputCaret")
                        return c;
                return null;
            }
            catch { return null; }
        }

        private void RemoveCaret(object container)
        {
            try
            {
                List<Canvas> children = GetLogicalChildCollection<Canvas>(container);
                foreach (var c in children)
                    ((StackPanel)container).Children.Remove(c);
                FocusManager.RemoveGotFocusHandler((DependencyObject)container, InputGotFocus);
                FocusManager.RemoveLostFocusHandler((DependencyObject)container, InputLostFocus);
                Keyboard.ClearFocus();
            } catch { return; }
        }

        private void InputBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox input = sender as TextBox;
            if(input != null)
            {
                var pos = input.CaretIndex;
                if (Clipboard.ContainsText())
                    input.Text = input.Text.Insert
                        (pos, Clipboard.GetText(TextDataFormat.Text));
            }
        }

        private void Scroller_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (input.Visibility==Visibility.Visible)
            {
                TextBox i = GetCurrentInput(input);
                i.Focus();
            }
        }

        private void DragHandle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.Cursor = CrystalCursor.Hand;
                this.DragMove();
            }
            else
            {
                this.Cursor = CrystalCursor.Arrow;
                return;
            }
        }

        private void DragHandle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.Cursor = CrystalCursor.Arrow;
            }
            else
            {
                return;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ConsoleHandler_OnConsoleExit();
        }

        private void Box_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.MaximizeButton.Content = this.Resources["MaximizeSign"];
                this.Frame.Margin = new Thickness(2);
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                this.MaximizeButton.Content = this.Resources["RestoreSign"];
                this.Frame.Margin = new Thickness(5);
            }  
        }

        private void Box_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.InputBlock.Focus();
        }

        private static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }

        /// <summary>
        /// Gets the list of routed event handlers subscribed to the specified routed event.
        /// </summary>
        /// <param name="element">The UI element on which the event is defined.</param>
        /// <param name="routedEvent">The routed event for which to retrieve the event handlers.</param>
        /// <returns>The list of subscribed routed event handlers.</returns>
        public static RoutedEventHandlerInfo[] GetRoutedEventHandlers(UIElement element, RoutedEvent routedEvent)
        {
            // Get the EventHandlersStore instance which holds event handlers for the specified element.
            // The EventHandlersStore class is declared as internal.
            var eventHandlersStoreProperty = typeof(UIElement).GetProperty(
                "EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
            object eventHandlersStore = eventHandlersStoreProperty.GetValue(element, null);

            // Invoke the GetRoutedEventHandlers method on the EventHandlersStore instance 
            // for getting an array of the subscribed event handlers.
            var getRoutedEventHandlers = eventHandlersStore.GetType().GetMethod(
                "GetRoutedEventHandlers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var routedEventHandlers = (RoutedEventHandlerInfo[])getRoutedEventHandlers.Invoke(
                eventHandlersStore, new object[] { routedEvent });

            return routedEventHandlers;
        }
    }
}
