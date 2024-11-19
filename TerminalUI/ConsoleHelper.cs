using System.Runtime.InteropServices;

public class ConsoleHelper
{
    [StructLayout(LayoutKind.Sequential)]
    private struct Coord
    {
        public short X;
        public short Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SmallRect
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ConsoleScreenBufferInfo
    {
        public Coord Size;
        public Coord CursorPosition;
        public short Attributes;
        public SmallRect Window;
        public Coord MaximumWindowSize;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out ConsoleScreenBufferInfo csbi);

    private const int STD_OUTPUT_HANDLE = -11;
    private static readonly IntPtr ConsoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);

    public static (int width, int height) GetConsoleSize()
    {
        if (GetConsoleScreenBufferInfo(ConsoleHandle, out ConsoleScreenBufferInfo csbi))
        {
            return (csbi.Window.Right - csbi.Window.Left + 1, csbi.Window.Bottom - csbi.Window.Top + 1);
        }
        return (0, 0);
    }
}
