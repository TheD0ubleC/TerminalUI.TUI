using static TerminalUI.Style;

namespace TerminalUI
{
    public partial class TUI
    {
        public partial class Component
        {
            internal class TTitle : Component
            {
                public TitleType Alignment { get; set; } = TitleType.Mid; // 默认居中对齐

                public TTitle(string text, TitleType alignment = null, BorderStyle? borderStyle = null)
                {
                    Text = text;
                    Alignment = alignment ?? TitleType.Mid; // 默认对齐方式
                    Height = 1; // 标题默认高度为 1
                    BorderStyle = borderStyle ?? BorderStyle.Default;
                }

                public override void Render(char[,] buffer)
                {
                    if (!Visible) return;

                    // 绘制边框
                    for (int x = 0; x < Width; x++)
                    {
                        int bufferX = X + x;
                        if (x == 0)
                        {
                            buffer[Y, bufferX] = BorderStyle.TopLeft; // 左上角
                        }
                        else if (x == Width - 1)
                        {
                            buffer[Y, bufferX] = BorderStyle.TopRight; // 右上角
                        }
                        else
                        {
                            buffer[Y, bufferX] = BorderStyle.Horizontal; // 顶部边框
                        }
                    }

                    // 绘制左侧和右侧边框
                    for (int y = 0; y < Height; y++)
                    {
                        int bufferY = Y + y;

                        buffer[bufferY, X] = BorderStyle.Vertical; // 左侧边框
                        buffer[bufferY, X + Width - 1] = BorderStyle.Vertical; // 右侧边框
                    }

                    // 绘制底部边框
                    for (int x = 0; x < Width; x++)
                    {
                        int bufferX = X + x;
                        if (x == 0)
                        {
                            buffer[Y + Height - 1, bufferX] = BorderStyle.BottomLeft; // 左下角
                        }
                        else if (x == Width - 1)
                        {
                            buffer[Y + Height - 1, bufferX] = BorderStyle.BottomRight; // 右下角
                        }
                        else
                        {
                            buffer[Y + Height - 1, bufferX] = BorderStyle.Horizontal; // 底部边框
                        }
                    }

                    // 渲染标题文本
                    int textWidth = GetTextWidth(Text);
                    int textStartX = 0;

                    switch (Alignment)
                    {
                        case var type when type == TitleType.Left:
                            textStartX = X + 1; // 左对齐
                            break;
                        case var type when type == TitleType.Mid:
                            textStartX = X + (Width - textWidth) / 2; // 居中对齐
                            break;
                        case var type when type == TitleType.Right:
                            textStartX = X + Width - textWidth - 1; // 右对齐
                            break;
                    }

                    RenderTextWithWidth(buffer, textStartX, Y - 1, Text, Width - 2);
                }
            }
        }
    }
}
