﻿#pragma checksum "..\..\..\UI\Terminal.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "D05A5E5C0B5E6640330CA4527FB2211AAB7466FE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Eze.IO.Terminal.UI;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Eze.IO.Terminal.UI {
    
    
    /// <summary>
    /// Terminal
    /// </summary>
    public partial class Terminal : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\..\UI\Terminal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Eze.IO.Terminal.UI.Terminal window;
        
        #line default
        #line hidden
        
        
        #line 8 "..\..\..\UI\Terminal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Frame;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\UI\Terminal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border border;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\UI\Terminal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer Scroller;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\UI\Terminal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel consoleItems;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\UI\Terminal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel inputSection;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\UI\Terminal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox inputSign;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\UI\Terminal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox InputBlock;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/terminal;component/ui/terminal.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UI\Terminal.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.window = ((Eze.IO.Terminal.UI.Terminal)(target));
            
            #line 7 "..\..\..\UI\Terminal.xaml"
            this.window.SizeChanged += new System.Windows.SizeChangedEventHandler(this.Window_SizeChanged);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\UI\Terminal.xaml"
            this.window.Unloaded += new System.Windows.RoutedEventHandler(this.window_Unloaded);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\UI\Terminal.xaml"
            this.window.Loaded += new System.Windows.RoutedEventHandler(this.window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Frame = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.border = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            this.Scroller = ((System.Windows.Controls.ScrollViewer)(target));
            
            #line 14 "..\..\..\UI\Terminal.xaml"
            this.Scroller.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Scroller_MouseDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.consoleItems = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 6:
            this.inputSection = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.inputSign = ((System.Windows.Controls.TextBox)(target));
            
            #line 17 "..\..\..\UI\Terminal.xaml"
            this.inputSign.Loaded += new System.Windows.RoutedEventHandler(this.TextBox_Loaded);
            
            #line default
            #line hidden
            return;
            case 8:
            this.InputBlock = ((System.Windows.Controls.TextBox)(target));
            
            #line 18 "..\..\..\UI\Terminal.xaml"
            this.InputBlock.KeyDown += new System.Windows.Input.KeyEventHandler(this.InputBlock_KeyDown);
            
            #line default
            #line hidden
            
            #line 18 "..\..\..\UI\Terminal.xaml"
            this.InputBlock.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.InputBlock_MouseRightButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

