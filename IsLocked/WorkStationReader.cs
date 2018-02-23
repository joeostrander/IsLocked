using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IsLocked
{
    class WorkStationReader
    {
        const Int32 DESKTOP_CREATEMENU = 0x4;
        const Int32 DESKTOP_CREATEWINDOW = 0x2;
        const Int32 DESKTOP_ENUMERATE = 0x40;
        const Int32 DESKTOP_HOOKCONTROL = 0x8;
        const Int32 DESKTOP_READOBJECTS = 0x1;
        const Int32 DESKTOP_SWITCHDESKTOP = 0x100;
        const Int32 DESKTOP_WRITEOBJECTS = 0x80;
        const Int32 GENERIC_WRITE = 0x40000000;
        const Int32 HWND_BROADCAST = 0xFFFF;
        const Int32 WM_HOTKEY = 0x312;
        const Int32 MOD_ALT = 0x1;
        const Int32 MOD_CONTROL = 0x2;
        const Int32 VK_DELETE = 0x2E;
        const Int32 UOI_NAME = 2;

        public struct LockStatus
        {
            public string Message;
            public bool IsLocked;
        }

        [DllImport("user32.dll", SetLastError = true, EntryPoint="OpenDesktopA")]
        static extern IntPtr OpenDesktop(string lpszDesktop, uint dwFlags, bool fInherit, uint dwDesiredAccess);

        
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool CloseDesktop(IntPtr hDesktop);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SwitchDesktop(IntPtr hDesktop);
        
        IntPtr hWnd;
        bool boolSwitchedOk;
        int intLastError;


        public LockStatus GetLockStatus()
        {
            LockStatus ivarreturn = new LockStatus();

            hWnd = OpenDesktop("Default", 0, false, DESKTOP_SWITCHDESKTOP);

            if (hWnd == IntPtr.Zero)
            {
                ivarreturn.Message = "Error with OpenDesktop: " + Marshal.GetLastWin32Error();
                ivarreturn.IsLocked = false;
            }
            else
            {
                boolSwitchedOk = SwitchDesktop(hWnd);
                intLastError = Marshal.GetLastWin32Error();
                
                if (boolSwitchedOk)
                {
                    ivarreturn.Message = "Unlocked";
                    ivarreturn.IsLocked = false;
                    
                }
                else
                {
                    if (intLastError == 0)
                    {
                        ivarreturn.Message = "Locked";
                        ivarreturn.IsLocked = true;
                        CloseDesktop(hWnd);
                    }
                    else
                    {
                        ivarreturn.Message = "Error with SwitchDesktop: " + intLastError;
                        ivarreturn.IsLocked = false;
                        CloseDesktop(hWnd);
                    }
                }
            }

            return ivarreturn;

        }

    }
}
