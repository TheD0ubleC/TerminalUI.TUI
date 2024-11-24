using static TerminalUI.Style;

namespace TerminalUI
{
    public partial class TUI
    {
        public partial class Component
        {
            public class TLabel : Component
            {
                // 定义 TLabel 默认边框样式
                protected override BorderStyle ComponentDefaultBorderStyle => new BorderStyle('=', '=', '=', '=', '=', '*');

                public override void Render(char[,] buffer)
                {
                    if (!Visible) return;

                    // 调整大小以适应文本
                    if (AutoResize)
                    {
                        AdjustSizeToFitText();
                    }

                    // 获取缓冲区大小
                    int bufferHeight = buffer.GetLength(0);
                    int bufferWidth = buffer.GetLength(1);

                    // 计算组件有效渲染区域
                    int startX = Math.Max(X, 0);
                    int endX = Math.Min(X + Width, bufferWidth);
                    int startY = Math.Max(Y, 0);
                    int endY = Math.Min(Y + Height, bufferHeight);

                    if (startX >= endX || startY >= endY) return;

                    // 绘制边框和内容
                    for (int y = startY; y < endY; y++)
                    {
                        for (int x = startX; x < endX; x++)
                        {
                            int relativeX = x - X;
                            int relativeY = y - Y;

                            if (relativeY == 0)
                            {
                                buffer[y, x] = (relativeX == 0) ? BorderStyle.TopLeft
                                              : (relativeX == Width - 1) ? BorderStyle.TopRight
                                              : BorderStyle.Horizontal;
                            }
                            else if (relativeY == Height - 1)
                            {
                                buffer[y, x] = (relativeX == 0) ? BorderStyle.BottomLeft
                                              : (relativeX == Width - 1) ? BorderStyle.BottomRight
                                              : BorderStyle.Horizontal;
                            }
                            else
                            {
                                buffer[y, x] = (relativeX == 0 || relativeX == Width - 1) ? BorderStyle.Vertical : ' ';
                            }
                        }
                    }

                    // 渲染文本
                    int textStartX = Math.Max(X + 1, startX);
                    int textEndX = Math.Min(X + Width - 1, endX);
                    int textStartY = Y + Height / 2;

                    if (textStartY >= 0 && textStartY < bufferHeight)
                    {
                        string truncatedText = Text.Substring(0, Math.Min(Text.Length, textEndX - textStartX));
                        RenderTextWithWidth(buffer, textStartX, textStartY, truncatedText, textEndX - textStartX);
                    }
                }

            }
        }
    }
}
