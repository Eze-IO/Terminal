using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Eze.IO.Terminal.UI
{
    public static class CrystalCursor
    {
        private static IntPtr hhook = new IntPtr();

        [DllImport("user32.dll")]
        private static extern bool GetCursorInfo(out CursorInfo pci);

        [DllImport("user32.dll")]
        private static extern IntPtr SetCursor(IntPtr hCursor);

        [DllImport("user32.dll")]
        private static extern bool DestroyCursor(IntPtr hCursor);

        [DllImport("user32.dll")]
        private static extern IntPtr CopyCursor(IntPtr hCursor);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursor(IntPtr hCursor, uint cursorName);

        [DllImport("user32.dll")]
        private static extern int ShowCursor(bool visible);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursorFromFile(String str);

        public static IntPtr CursorHandle
        {
            get
            {
                CursorInfo pci;
                pci.cbSize = Marshal.SizeOf(typeof(CursorInfo));
                GetCursorInfo(out pci);
                return pci.hCursor;
            }
        }

        private struct CursorInfo
        {
            public Int32 cbSize; //size in bytes; structure
            public Int32 flags; //state of cursor; 0 = hidden/ CURSOR_SHOWING = showing
            public IntPtr hCursor; //handle of cursor
            public POINT ptScreenPos; //A POINT structure
        }

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
        IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr
           hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
           uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        private const int OBJID_CURSOR = -9;
        private const int CHILDID_SELF = 0;
        private const uint EVENT_OBJECT_HIDE = 0x8003;
        private const uint EVENT_OBJECT_SHOW = 0x8002;
        private const uint EVENT_OBJECT_NAMECHANGE = 0x800C;
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        private static WinEventDelegate procDelegate = new WinEventDelegate(WinEventProc);

        private static void WinEventProc(IntPtr hWinEventHook, uint eventType,
        IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (hwnd == IntPtr.Zero && idObject == OBJID_CURSOR && idChild == CHILDID_SELF)
            {
                switch (eventType)
                {
                    case EVENT_OBJECT_HIDE:
                        //cursor hidden
                        _visible.Invoke();
                        break;
                    case EVENT_OBJECT_SHOW:
                        //cursor shown
                        _visible.Invoke();
                        break;
                    case EVENT_OBJECT_NAMECHANGE:
                        //cursor changed
                        _change.Invoke();
                        break;
                }
            }
        }

        private static event CustomMouseHandler _visible;
        private static event CustomMouseHandler _change;

        public static event CustomMouseHandler CursorVisiblityChanged
        {
            add
            {
                hhook = SetWinEventHook(EVENT_OBJECT_NAMECHANGE, EVENT_OBJECT_NAMECHANGE, IntPtr.Zero,
                procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
                _visible += value;
            }
            remove
            {
                UnhookWinEvent(hhook);
                DestroyCursor(CursorHandle);
                SetCursor(CopyCursor(LoadCursor(IntPtr.Zero, SystemCursors.Normal.ToUint())));
                _visible -= value;
            }
        }
        public static event CustomMouseHandler OnCursorChange
        {
            add
            {
                hhook = SetWinEventHook(EVENT_OBJECT_NAMECHANGE, EVENT_OBJECT_NAMECHANGE, IntPtr.Zero,
                procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
                _change += value;
            }
            remove
            {
                UnhookWinEvent(hhook);
                DestroyCursor(CursorHandle);
                SetCursor(CopyCursor(LoadCursor(IntPtr.Zero, SystemCursors.Normal.ToUint())));
                _change -= value;
            }
        }

        public static Boolean IsVisible
        {
            get
            {
                CursorInfo pci;
                pci.cbSize = Marshal.SizeOf(typeof(CursorInfo));
                GetCursorInfo(out pci);
                if (pci.flags != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    ShowCursor(value);
                }
                catch
                {
                    throw;
                }
            }

        }

        private const Int32 CURSOR_SHOWING = 0x0001;
        public static Cursor CurrentCursor
        {
            get
            {
                CursorInfo pci;
                pci.cbSize = Marshal.SizeOf(typeof(CursorInfo));

                if(GetCursorInfo(out pci))
                {
                    if(pci.flags == CURSOR_SHOWING)
                    {
                        Icon ico = Icon.FromHandle(pci.hCursor);
                        using(var ms = new MemoryStream())
                        {
                            ico.Save(ms);
                            return new Cursor(ms);
                        }
                    }
                }

                return null;
            }
        }

        private static Stream GetResourceStream(String resource, bool animated = false)
        {
            try
            {
                var assem = Assembly.GetExecutingAssembly();
                return assem.GetManifestResourceStream(resource);
            }
            catch { return null; }
        }

        public static Cursor Help
        {
            get
            {
                if(!IsEnabled) { return Cursors.Help; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.help.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor Arrow
        {
            get
            {
                if (!IsEnabled) { return Cursors.Arrow; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.normal_select.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor ArrowCD
        {
            get
            {
                if (!IsEnabled) { return Cursors.ArrowCD; }
                return Arrow;
            }
        }

        public static Cursor AppStarting
        {
            get
            {
                if (!IsEnabled) { return Cursors.AppStarting; }
                try
                {
                    
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.app_starting_3D_ring.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor IBeam
        {
            get
            {
                if (!IsEnabled) { return Cursors.IBeam; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.text_select.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor Wait
        {
            get
            {
                if (!IsEnabled) { return Cursors.Wait; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.busy_ring.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor UpArrow
        {
            get
            {
                if (!IsEnabled) { return Cursors.UpArrow; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.alternative_select.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor Hand
        {
            get
            {
                if (!IsEnabled) { return Cursors.Hand; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.hand_select.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor SizeAll
        {
            get
            {
                if (!IsEnabled) { return Cursors.SizeAll; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.move_2.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor No
        {
            get
            {
                if (!IsEnabled) { return Cursors.Help; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.unavailable.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor Cross
        {
            get
            {
                if (!IsEnabled) { return Cursors.Cross; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.precision_select.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor SizeNESW
        {
            get
            {
                if (!IsEnabled) { return Cursors.SizeNESW; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.diagonal_resize _2_2.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor SizeNWSE
        {
            get
            {
                if (!IsEnabled) { return Cursors.SizeNWSE; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.diagonal_resize _1_2.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor SizeNS
        {
            get
            {
                if (!IsEnabled) { return Cursors.SizeNS; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.vertical_resize_2.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor SizeWE
        {
            get
            {
                if (!IsEnabled) { return Cursors.SizeNS; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.horizontal_resize_2.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor Pen
        {
            get
            {
                if (!IsEnabled) { return Cursors.Pen; }
                try
                {
                    return new Cursor(GetResourceStream("Eze.IO.Terminal.Cursors.pen.ani"));
                }
                catch { throw new ApplicationException("Failed to create cursor!"); }
            }
        }

        public static Cursor None
        {
            get
            {
                if (!IsEnabled) { return Cursors.None; }
                return No;
            }
        }

        private static bool _enabled = false;
        public static Boolean IsEnabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if(!value)
                {
                    if(DestroyCursor(CursorHandle))
                    {
                        SetCursor(LoadCursor(IntPtr.Zero, SystemCursors.Normal.ToUint()));
                    }
                }
                 _enabled = value;
            }
        }
    }

    public static class MouseAdvance
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorInfo(out CursorInfo pci);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public Int32 x;
            public Int32 y;
        }

        private struct CursorInfo
        {
            public Int32 cbSize; //size in bytes; structure
            public Int32 flags; //state of cursor; 0 = hidden/ CURSOR_SHOWING = showing
            public IntPtr hCursor; //handle of cursor
            public POINT ptScreenPos; //A POINT structure
        }

        public static System.Windows.Point GetPosition()
        {
            CursorInfo pci;
            pci.cbSize = Marshal.SizeOf(typeof(CursorInfo));
            GetCursorInfo(out pci);
            return new System.Windows.Point(pci.ptScreenPos.x, pci.ptScreenPos.y);
        }
    }

    public enum SystemCursors : uint
    {
        /// <summary>
        /// Standard arrow and small hourglass
        /// </summary>
        AppStarting = 32650,
        /// <summary>
        /// Standard arrow
        /// </summary>
        Normal = 32512,
        /// <summary>
        /// Crosshair
        /// </summary>
        Cross = 32515,
        /// <summary>
        /// Windows 2000/XP: Hand
        /// </summary>
        Hand= 32649,
        /// <summary>
        /// Arrow and question mark
        /// </summary>
        Help = 32651,
        /// <summary>
        /// I-beam
        /// </summary>
        IBeam = 32513,
        /// <summary>
        /// Slashed circle
        /// </summary>
        No = 32648,
        /// <summary>
        /// Four-pointed arrow pointing north, south, east, and west
        /// </summary>
        SizeAll = 32646,
        /// <summary>
        /// Double-pointed arrow pointing northeast and southwest
        /// </summary>
        SizeNESW = 32643,
        /// <summary>
        /// Double-pointed arrow pointing north and south
        /// </summary>
        SizeNS = 32645,
        /// <summary>
        /// Double-pointed arrow pointing northwest and southeast
        /// </summary>
        SizeNWSE = 32642,
        /// <summary>
        /// Double-pointed arrow pointing west and east
        /// </summary>
        SizeWE = 32644,
        /// <summary>
        /// Vertical arrow
        /// </summary>
        Up = 32516,
        /// <summary>
        /// Hourglass
        /// </summary>
        Wait = 32514
    }

    public static class CursorAdvance
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetCursor(IntPtr hCursor);

        public static uint ToUint(this SystemCursors cursor)
        {
            return (UInt32)cursor;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public Int32 x;
        public Int32 y;
    }

    public delegate void CustomMouseHandler();
    public delegate void MouseMovedHandler();
    public delegate void MouseHoverHandler(IntPtr handle);
}
