using GlobalMicMute.Tools.win32;

namespace GlobalMicMute.Tools.utils
{
    public class HotKeys
    {
        public static void RegisterGlobalHotKey(int id, IntPtr handle, Keys hotkey, uint modifiers)
        {

            int UniqueHotkeyId = id;

            uint HotKeyCode = (uint)hotkey;


            UnsafeNativeMethods.RegisterHotKey(
                handle, UniqueHotkeyId, modifiers, HotKeyCode
            );
        }
    }
}
