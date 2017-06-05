﻿using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

namespace Grabacr07.KanColleViewer.Views
{
    public class PopupNonTopmost : Popup
    {
        protected override void OnOpened(EventArgs e)
    {
        var hwnd = ((HwndSource)PresentationSource.FromVisual(this.Child)).Handle;
        RECT rect;

        if (GetWindowRect(hwnd, out rect))
        {
            SetWindowPos(hwnd, -2, rect.Left, rect.Top, (int)this.Width, (int)this.Height, 0);
        }
    }

    #region P/Invoke imports & definitions

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32", EntryPoint = "SetWindowPos")]
    public static extern int SetWindowPos(IntPtr hWnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);

    #endregion
}
}

