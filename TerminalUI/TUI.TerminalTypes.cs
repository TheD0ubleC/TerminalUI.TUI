using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalUI
{
    partial class TUI
    {
        // 定义 TUI 类型，用于表示终端界面的类型
        // Defines TUIType, representing the type of terminal interface
        public enum TUIType
        {
            PowerShell // 表示 PowerShell 类型 Represents PowerShell type
        }

        // 定义 FrameType，用于表示帧的类型（上下左右）
        // Defines FrameType, representing frame types (Top, Left, Right, Bottom)
        internal class FrameType
        {
            public static readonly FrameType Top = new FrameType(0); // 顶部帧 Top frame
            public static readonly FrameType Left = new FrameType(1); // 左侧帧 Left frame
            public static readonly FrameType Right = new FrameType(2); // 右侧帧 Right frame
            public static readonly FrameType Bottom = new FrameType(3); // 底部帧 Bottom frame

            public int Index { get; } // 提供整型索引 Provides integer index

            private FrameType(int index)
            {
                Index = index;
            }

            // 隐式转换：允许直接将 FrameType 用作数组索引
            // Implicit conversion: Allows FrameType to be used as an array index
            public static implicit operator int(FrameType frameType)
            {
                return frameType.Index;
            }
        }
    }
}
