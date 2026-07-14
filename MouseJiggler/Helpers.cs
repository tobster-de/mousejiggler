using System.Diagnostics;
using System.Runtime.InteropServices;
using MouseJiggler.PInvoke;

namespace MouseJiggler;

internal static class Helpers
{
    /// <summary>
    /// Tries to determine the system idle time in seconds.
    /// </summary>
    /// <param name="idleTimeInSeconds">
    /// On success, contains the elapsed time since the last mouse or keyboard input.
    /// </param>
    /// <returns><see langword="true"/> if idle time was read successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// <see cref="User32.GetLastInputInfo"/> returns the tick value of the last user input
    /// (mouse or keyboard) since system startup.
    /// </para>
    /// <para>
    /// The difference is intentionally calculated as <see cref="uint"/> so wrap-around of
    /// <see cref="Environment.TickCount"/> after long runtimes is handled correctly.
    /// </para>
    /// </remarks>
    internal static bool TryGetIdleTimeInSeconds(out double idleTimeInSeconds)
    {
        User32.LASTINPUTINFO lastInputInfo = new User32.LASTINPUTINFO
        {
            cbSize = (uint)Marshal.SizeOf<User32.LASTINPUTINFO>(),
        };

        if (!User32.GetLastInputInfo(ref lastInputInfo))
        {
            int errorCode = Marshal.GetLastWin32Error();

            Debugger.Log(level: 1,
                category: "IdleTime",
                message: $"failed to read last input info; errcode=0x{errorCode:x8}\n");

            idleTimeInSeconds = 0;
            return false;
        }

        uint elapsedMilliseconds = unchecked((uint)Environment.TickCount - (uint)lastInputInfo.dwTime);
        idleTimeInSeconds = elapsedMilliseconds / 1000d;
        return true;
    }

    #region Position checking

    private static int? _lastX, _lastY;

    /// <summary>
    /// Checks whether the mouse position has moved
    /// </summary>
    internal static bool CheckMovement()
    {
        bool result = false;
        if (User32.GetCursorPos(out int x, out int y))
        {
            result = _lastX != x || _lastY != y;

            _lastX = x;
            _lastY = y;
        }

        return result;
    }

    #endregion Position checking

    #region Jiggling

    private static int? _savedX, _savedY;

    /// <summary>
    /// Save current mouse position.
    /// </summary>
    internal static void SavePos()
    {
        if (User32.GetCursorPos(out int x, out int y))
        {
            _savedX = x;
            _savedY = y;
        }
    }

    /// <summary>
    /// Restore saved mouse position.
    /// </summary>
    internal static void RestorePos()
    {
        if (_savedX.HasValue && _savedY.HasValue)
        {
            User32.SetCursorPos(_savedX.Value, _savedY.Value);
            _savedX = null;
            _savedY = null;
        }
    }

    /// <summary>
    ///     Jiggle the mouse; i.e., fake a mouse movement event.
    /// </summary>
    /// <param name="deltaX">The mouse will be moved by delta pixels along the X axis.</param>
    /// <param name="deltaY">The mouse will be moved by delta pixels along the Y axis.</param>
    internal static void Jiggle(int deltaX, int? deltaY = null)
    {
        var inp = new User32.INPUT
        {
            Type = User32.InputType.INPUT_MOUSE,
            Data = new User32.INPUT.InputUnion
            {
                Mouse = new User32.MOUSEINPUT
                {
                    X = deltaX,
                    Y = deltaY ?? deltaX,
                    Data = 0,
                    Flags = User32.MOUSEEVENTF.MOUSEEVENTF_MOVE,
                    Time = 0,
                    ExtraInfo = IntPtr.Zero,
                },
            },
        };

        uint returnValue = User32.SendInput(1, new[] { inp }, Marshal.SizeOf<User32.INPUT>());

        if (returnValue != 1)
        {
            int errorCode = Marshal.GetLastWin32Error();

            Debugger.Log(level: 1,
                category: "Jiggle",
                message:
                $"failed to insert event to input stream; retval={returnValue}, errcode=0x{errorCode:x8}\n");
        }
    }

    #endregion Jiggling
}