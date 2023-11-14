#region header

// MouseJiggler - Helpers.cs
// 
// Created by: Alistair J R Young (avatar) at 2021/01/20 7:40 PM.

#endregion

#region using

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MouseJiggler.PInvoke;

#endregion

namespace MouseJiggler;

internal static class Helpers
{
    #region Jiggling

    /// <summary>
    ///     Jiggle the mouse; i.e., fake a mouse movement event.
    /// </summary>
    /// <param name="delta">The mouse will be moved by delta pixels along both X and Y.</param>
    internal static void Jiggle(int delta)
    {
        var inp = new User32.INPUT
        {
            Type = User32.InputType.INPUT_MOUSE,
            Data = new User32.INPUT.InputUnion
            {
                Mouse = new User32.MOUSEINPUT
                {
                    X = delta,
                    Y = delta,
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