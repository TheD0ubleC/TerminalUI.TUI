using static TerminalUI.Style;

namespace TerminalUI
{
    public partial class TUI
    {
        public partial class Component
        {
            public class TButton : Component
            {
                // 定义 TButton 默认边框样式
                protected override BorderStyle ComponentDefaultBorderStyle => new BorderStyle('+', '+', '+', '+', '-', '|');

                public Action OnClickAction { get; set; }

                public override void OnClick()
                {
                    OnClickAction?.Invoke();
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

                            if (y == 0)
                            {
                                buffer[bufferY, bufferX] = (x == 0) ? BorderStyle.TopLeft
                                                    : (x == Width - 1) ? BorderStyle.TopRight
                                                    : BorderStyle.Horizontal;
                            }
                            else if (y == Height - 1)
                            {
                                buffer[bufferY, bufferX] = (x == 0) ? BorderStyle.BottomLeft
                                                    : (x == Width - 1) ? BorderStyle.BottomRight
                                                    : BorderStyle.Horizontal;
                            }
                            else
                            {
                                buffer[bufferY, bufferX] = (x == 0 || x == Width - 1) ? BorderStyle.Vertical : ' ';
                            }
                        }
                    }

                    // 渲染文本
                    int textStartX = X + 1;
                    int textStartY = Y + Height / 2;
                    RenderTextWithWidth(buffer, textStartX, textStartY, Text, Width - 2);
                }
            }
        }
    }
}
