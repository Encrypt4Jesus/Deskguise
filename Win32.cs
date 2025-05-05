using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace Deskguise.WindowsNative
{
    [Flags]
    public enum REG_NOTIFY_CHANGE : uint
    {
        /// <summary>Notify the caller if a subkey is added or deleted</summary>
        NAME = 0x1,
        /// <summary>Notify the caller of changes to the attributes of the key, such as the security descriptor information</summary>
        ATTRIBUTES = 0x2,
        /// <summary>Notify the caller of changes to a value of the key. This can include adding or deleting a value, or changing an existing value</summary>
        LAST_SET = 0x4,
        /// <summary> Notify the caller of changes to the security descriptor of the key</summary>
        SECURITY = 0x8
    }

    internal static class Win32
    {
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern uint RegNotifyChangeKeyValue(SafeRegistryHandle key, bool watchSubTree, uint notifyFilter, SafeWaitHandle regEvent, bool async);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, UIntPtr wParam, IntPtr lParam);
    }
}
