using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalUI
{
    partial class TUI
    {
        public enum TUIType
        {
            PowerShell,
        }
        internal class FrameType
        {
            public static readonly FrameType Top = new FrameType(0);
            public static readonly FrameType Left = new FrameType(1);
            public static readonly FrameType Right = new FrameType(2);
            public static readonly FrameType Bottom = new FrameType(3);

            public int Index { get; } // 提供整型索引

            private FrameType(int index)
            {
                Index = index;
            }

            // 隐式转换：允许直接将 FrameType 用作数组索引
            public static implicit operator int(FrameType frameType)
            {
                return frameType.Index;
            }
        }

    }
}
