namespace TerminalUI
{
    public partial class TUI
    {
        public partial class Component
        {
            public class TLabel : Component
            {

                public override void Render(char[,] buffer)
                {
                    if (!Visible) return; // 如果不可见则直接返回 Skip if not visible

                    // 绘制标签边框 Draw label borders
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            int bufferX = X + x; // 缓冲区 X 坐标 Buffer X coordinate
                            int bufferY = Y + y; // 缓冲区 Y 坐标 Buffer Y coordinate

                            if (y == 0 || y == Height - 1)
                            {
                                buffer[bufferY, bufferX] = (x == 0 || x == Width - 1) ? '+' : '='; // 顶部或底部边框 Top/Bottom border
                            }
                            else if (x == 0 || x == Width - 1)
                            {
                                buffer[bufferY, bufferX] = '*'; // 左右边框 Left/Right border
                            }
                            else
                            {
                                buffer[bufferY, bufferX] = ' '; // 填充空白 Fill empty space
                            }
                        }
                    }

                    // 渲染文本 Render button text
                    int textStartX = X + 1; // 文本起始 X（水平偏移） Text start X (horizontal offset)
                    int textStartY = Y + Height / 2; // 文本起始 Y（垂直居中） Text start Y (vertically centered)
                    RenderTextWithWidth(buffer, textStartX, textStartY, Text, Width - 2); // 限制宽度 Limit text width
                }
            }
        }
    }
}
