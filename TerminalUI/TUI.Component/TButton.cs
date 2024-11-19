namespace TerminalUI
{
    public partial class TUI
    {
        public partial class Component
        {
            public class TButton : Component
            {
                public Action OnClickAction { get; set; } // 定义按钮的点击事件

                public override void OnClick()
                {
                    OnClickAction?.Invoke(); // 调用绑定的事件
                }

                public override void Render(char[,] buffer)
                {
                    if (!Visible) return;

                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            int bufferX = X + x;
                            int bufferY = Y + y;

                            if (y == 0 || y == Height - 1)
                            {
                                buffer[bufferY, bufferX] = (x == 0 || x == Width - 1) ? '+' : '-';
                            }
                            else if (x == 0 || x == Width - 1)
                            {
                                buffer[bufferY, bufferX] = '|';
                            }
                            else if (y == Height / 2 && x - 1 < Text.Length)
                            {
                                buffer[bufferY, bufferX] = Text[x - 1];
                            }
                            else
                            {
                                buffer[bufferY, bufferX] = ' ';
                            }
                        }
                    }
                }
            }
        }
    }
}
