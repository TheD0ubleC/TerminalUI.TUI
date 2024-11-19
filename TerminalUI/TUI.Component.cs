namespace TerminalUI
{
    public partial class TUI
    {


        public abstract partial class Component
        {
            public int X { get; set; } // 组件的 X 坐标
            public int Y { get; set; } // 组件的 Y 坐标
            public int Width { get; set; } // 宽度
            public int Height { get; set; } // 高度
            public int ZIndex { get; set; } // Z 层级
            public bool Visible { get; set; } = true; // 是否可见
            public string Text { get; set; } = ""; // 显示的文本

            // 虚拟的 OnClick 方法
            public virtual void OnClick() { }

            // 抽象方法，子类必须实现
            public abstract void Render(char[,] buffer);

            // 虚拟的 HandleClick 方法
            public virtual void HandleClick(int mouseX, int mouseY)
            {
                if (mouseX >= X && mouseX < X + Width &&
                    mouseY >= Y && mouseY < Y + Height)
                {
                    OnClick();
                }
            }

            // 虚拟的 HandleKey 方法
            public virtual void HandleKey(ConsoleKey key) { }
        }
    }
}
