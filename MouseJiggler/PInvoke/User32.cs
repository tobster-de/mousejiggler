using System;
using System.Runtime.InteropServices;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

#pragma warning disable SA1300 // Element must begin with upper-case letter

namespace MouseJiggler.PInvoke;

public partial class User32
{
    [DllImport("user32.dll")]
    internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,
        int cbSize);

    public enum InputType : uint
    {
        /// <summary>
        /// The event is a mouse event. Use the <see cref="INPUT.InputUnion.Mouse"/> structure of the union.
        /// </summary>
        INPUT_MOUSE = 0,

        /// <summary>
        /// The event is a keyboard event. Use the <see cref="INPUT.InputUnion.Keyboard"/> structure of the union.
        /// </summary>
        INPUT_KEYBOARD = 1,

        /// <summary>
        /// The event is a hardware event. Use the <see cref="INPUT.InputUnion.Hardware"/> structure of the union.
        /// </summary>
        INPUT_HARDWARE = 2,
    }

    /// <summary>
    /// Used by <see cref="User32.SendInput"/> to store information for synthesizing input events such as keystrokes,
    /// mouse movement, and mouse clicks.
    /// </summary>
    internal struct INPUT
    {
        /// <summary>
        /// The type of the input event.
        /// </summary>
        public InputType Type;

        /// <summary>
        /// The union of mouse, keyboard and hardware input.
        /// </summary>
        public InputUnion Data;

        /// <summary>
        /// Describes some kind of input.
        /// </summary>
        /// <remarks>
        /// This struct is a union where all fields share memory address space.
        /// </remarks>
        /// <devremarks>
        /// From http://www.pinvoke.net/default.aspx/Structures/INPUT.html:
        /// The last 3 fields are a union, which is why they are all at the same memory offset.
        /// On 64-Bit systems, the offset of the <see cref="Mouse"/>, <see cref="Keyboard"/> and <see cref="Hardware"/> fields is 8,
        /// because the nested struct uses the alignment of its biggest member, which is 8
        /// (due to the 64-bit pointer in <see cref="KEYBDINPUT.ExtraInfo"/>).
        /// By separating the union into its own structure, rather than placing the
        /// <see cref="Mouse"/>, <see cref="Keyboard"/> and <see cref="Hardware"/> fields directly in the INPUT structure,
        /// we assure that the .NET structure will have the correct alignment on both 32 and 64 bit.
        /// </devremarks>
        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            /// <summary>
            /// The information about a simulated mouse event.
            /// This field shares memory with the <see cref="Keyboard"/> and <see cref="Hardware"/> fields.
            /// </summary>
            [FieldOffset(0)] public MOUSEINPUT Mouse;

            /// <summary>
            /// The information about a simulated keyboard event.
            /// This field shares memory with the <see cref="Mouse"/> and <see cref="Hardware"/> fields.
            /// </summary>
            [FieldOffset(0)] public KEYBDINPUT Keyboard;

            /// <summary>
            /// The information about a simulated hardware event.
            /// This field shares memory with the <see cref="Mouse"/> and <see cref="Hardware"/> fields.
            /// </summary>
            [FieldOffset(0)] public HARDWAREINPUT Hardware;
        }
    }

    public struct MOUSEINPUT
    {
        /// <summary>
        /// The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the dwFlags member. Absolute data is specified as the x coordinate of the mouse; relative data is specified as the number of pixels moved.
        /// </summary>
        public int X;

        /// <summary>
        /// The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the dwFlags member. Absolute data is specified as the y coordinate of the mouse; relative data is specified as the number of pixels moved.
        /// </summary>
        public int Y;

        /// <summary>
        /// If dwFlags contains <see cref="MOUSEEVENTF.MOUSEEVENTF_WHEEL"/>, then <see cref="Data"/> specifies the amount of wheel movement. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as <see cref="WHEEL_DELTA"/>, which is 120.
        /// If dwFlags does not contain <see cref="MOUSEEVENTF.MOUSEEVENTF_WHEEL"/>, <see cref="MOUSEEVENTF.MOUSEEVENTF_XDOWN"/>, or <see cref="MOUSEEVENTF.MOUSEEVENTF_XUP"/>, then mouseData should be zero.
        /// If dwFlags contains <see cref="MOUSEEVENTF.MOUSEEVENTF_XDOWN"/> or <see cref="MOUSEEVENTF.MOUSEEVENTF_XUP"/>, then mouseData specifies which X buttons were pressed or released.
        /// </summary>
        public uint Data;

        /// <summary>
        /// A set of bit flags that specify various aspects of mouse motion and button clicks. The bits in this member can be any reasonable combination of the following values.
        /// See MSDN docs for more info.
        /// </summary>
        public MOUSEEVENTF Flags;

        /// <summary>
        /// The time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own time stamp.
        /// </summary>
        public uint Time;

        /// <summary>
        /// An additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra information.
        /// </summary>
        public IntPtr ExtraInfo;
    }
        
    /// <summary>
    /// A set of bit flags that specify various aspects of mouse motion and button clicks. The bits in this member can be any reasonable combination of the following values.
    /// </summary>
    /// <remarks>
    /// The bit flags that specify mouse button status are set to indicate changes in status, not ongoing conditions.
    /// For example, if the left mouse button is pressed and held down, MOUSEEVENTF_LEFTDOWN is set when the left button is first pressed, but not for subsequent motions.
    /// Similarly, MOUSEEVENTF_LEFTUP is set only when the button is first released.
    /// You cannot specify both the MOUSEEVENTF_WHEEL flag and either MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP flags simultaneously in the <see cref="MOUSEINPUT.Flags"/> parameter,
    /// because they both require use of the <see cref="MOUSEINPUT.Data" /> field.
    /// </remarks>
    [Flags]
    public enum MOUSEEVENTF : uint
    {
        MOUSEEVENTF_ABSOLUTE = 0x8000,
        MOUSEEVENTF_HWHEEL = 0x01000,
        MOUSEEVENTF_MOVE = 0x0001,
        MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,
        MOUSEEVENTF_LEFTDOWN = 0x0002,
        MOUSEEVENTF_LEFTUP = 0x0004,
        MOUSEEVENTF_RIGHTDOWN = 0x0008,
        MOUSEEVENTF_RIGHTUP = 0x0010,
        MOUSEEVENTF_MIDDLEDOWN = 0x0020,
        MOUSEEVENTF_MIDDLEUP = 0x0040,
        MOUSEEVENTF_VIRTUALDESK = 0x4000,
        MOUSEEVENTF_WHEEL = 0x0800,
        MOUSEEVENTF_XDOWN = 0x0080,
        MOUSEEVENTF_XUP = 0x0100,
    }
        
    /// <summary>
    /// Contains information about a simulated keyboard event.
    /// </summary>
    public struct KEYBDINPUT
    {
        /// <summary>
        /// A virtual-key code. The code must be a value in the range 1 to 254. If the Flags member specifies KEYEVENTF_UNICODE, wVk must be 0.
        /// </summary>
        public VirtualKey wVk;

        /// <summary>
        /// A hardware scan code for the key.
        /// If <see cref="Flags"/> specifies <see cref="KEYEVENTF.KEYEVENTF_UNICODE"/>,
        /// <see cref="wScan"/> specifies a Unicode character which is to be sent to the foreground application.
        /// </summary>
        public ScanCode wScan;

        /// <summary>
        /// Specifies various aspects of a keystroke.
        /// This member can be certain combinations of the following values.
        /// </summary>
        public KEYEVENTF Flags;

        /// <summary>
        /// The time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp.
        /// </summary>
        public uint Time;

        /// <summary>
        /// An additional value associated with the keystroke.
        /// Use the GetMessageExtraInfo function to obtain this information.
        /// </summary>
        public IntPtr ExtraInfo;
    }

    /// <summary>
    /// Specifies various aspects of a keystroke. This member can be certain combinations of the following values.
    /// </summary>
    [Flags]
    public enum KEYEVENTF : uint
    {
        /// <summary>
        /// If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).
        /// </summary>
        KEYEVENTF_EXTENDED_KEY = 0x0001,

        /// <summary>
        /// If specified, the key is being released. If not specified, the key is being pressed.
        /// </summary>
        KEYEVENTF_KEYUP = 0x0002,

        /// <summary>
        /// If specified, <see cref="KEYBDINPUT.wScan"/> identifies the key and <see cref="KEYBDINPUT.wVk"/> is ignored.
        /// </summary>
        KEYEVENTF_SCANCODE = 0x0008,

        /// <summary>
        /// If specified, the system synthesizes a <see cref="VirtualKey.VK_PACKET"/> keystroke.
        /// The <see cref="KEYBDINPUT.wVk"/> parameter must be zero.
        /// This flag can only be combined with the <see cref="KEYEVENTF_KEYUP"/> flag.
        /// For more information, see the Remarks section.
        /// </summary>
        KEYEVENTF_UNICODE = 0x0004,
    }

    /// <summary>
    /// Contains information about a simulated message generated by an input device other than a keyboard or mouse.
    /// </summary>
    public struct HARDWAREINPUT
    {
        /// <summary>
        /// The message generated by the input hardware.
        /// </summary>
        public uint uMsg;

        /// <summary>
        /// The low-order word of the lParam parameter for <see cref="uMsg"/>.
        /// </summary>
        public ushort wParamL;

        /// <summary>
        /// The high-order word of the lParam parameter for <see cref="uMsg"/>.
        /// </summary>
        public ushort wParamH;
    }
}