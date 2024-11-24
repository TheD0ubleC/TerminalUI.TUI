using static TerminalUI.Style;

namespace TerminalUI
{
    public partial class TUI
    {
        public partial class Component
        {
            internal class TTitle : Component
            {
                private BorderStyle customBorderStyle; // 用户自定义样式

                public BorderStyle BorderStyle
                {
                    get => customBorderStyle;
                    set
                    {
                        customBorderStyle = value;
                        BuildEffectiveBorderStyle(); // 更新有效样式
                    }
                }

                private BorderStyle effectiveBorderStyle; // 当前生效的样式
                public BorderStyle EffectiveBorderStyle => effectiveBorderStyle; // 只读

                public Style.TitleType Alignment { get; set; }

                public TTitle(string text, Style.TitleType alignment, BorderStyle borderStyle = null, int zIndex = 0)
                {
                    Text = text ?? string.Empty;
                    Alignment = alignment;
                    BorderStyle = borderStyle; // 初始化用户样式
                    ZIndex = zIndex; // 设置ZIndex
                    BuildEffectiveBorderStyle(); // 构建默认生效样式
                }

                public void BuildEffectiveBorderStyle()
                {
                    effectiveBorderStyle = customBorderStyle ?? ParentTUI?.BorderStyle ?? ComponentDefaultBorderStyle;
                }

                public override void Render(char[,] buffer)
                {
                    if (!Visible) return;

                    // 获取缓冲区大小
                    int bufferHeight = buffer.GetLength(0);
                    int bufferWidth = buffer.GetLength(1);

                    // 计算有效的渲染范围
                    int startX = Math.Max(X, 0);
                    int endX = Math.Min(X + Width, bufferWidth);
                    int startY = Math.Max(Y - 1, 0); // 上边框
                    int endY = Math.Min(Y + 2, bufferHeight); // 下边框

                    if (startX >= endX || startY >= endY) return; // 如果完全超出缓冲区，则跳过渲染

                    // 修复边框前，先清空缓冲区的边框区域，避免残留
                    for (int x = startX; x < endX; x++)
                    {
                        if (Y - 1 >= 0 && Y - 1 < bufferHeight) buffer[Y - 1, x] = ' '; // 清空上边框
                        if (Y + 1 >= 0 && Y + 1 < bufferHeight) buffer[Y + 1, x] = ' '; // 清空下边框
                    }

                    // 计算文本渲染起始位置
                    int textWidth = GetTextWidth(Text);
                    int textStartX = X + 1; // 默认左对齐起始位置

                    switch (Alignment)
                    {
                        case TitleType.Left:
                            textStartX = X + 1; // 左对齐
                            break;
                        case TitleType.Mid:
                            textStartX = X + (Width - textWidth) / 2; // 居中对齐
                            break;
                        case TitleType.Right:
                            textStartX = X + Width - textWidth - 1; // 右对齐
                            break;
                    }

                    // 限制文本渲染范围，避免覆盖边框
                    textStartX = Math.Max(textStartX, startX);
                    int textEndX = Math.Min(textStartX + textWidth, endX - 1);

                    // 如果文本太长，添加空格以避免第一个字被覆盖
                    string adjustedText = Text;
                    if (textStartX <= X + 1) // 如果起始点非常靠左
                    {
                        adjustedText = " " + Text; // 在文本前添加一个空格
                    }

                    // 渲染文本
                    if (Y >= 0 && Y < bufferHeight && textStartX < textEndX)
                    {
                        string truncatedText = adjustedText.Substring(0, Math.Min(adjustedText.Length, textEndX - textStartX));
                        RenderTextWithWidth(buffer, textStartX, Y, truncatedText, textEndX - textStartX);
                    }

                    // 绘制标题的边框
                    for (int x = startX; x < endX; x++)
                    {
                        if (Y - 1 >= 0 && Y - 1 < bufferHeight) // 上边框
                        {
                            buffer[Y - 1, x] = (x == X) ? EffectiveBorderStyle.TopLeft
                                                       : (x == X + Width - 1) ? EffectiveBorderStyle.TopRight
                                                       : EffectiveBorderStyle.Horizontal;
                        }

                        if (Y + 1 >= 0 && Y + 1 < bufferHeight) // 下边框
                        {
                            buffer[Y + 1, x] = (x == X) ? EffectiveBorderStyle.BottomLeft
                                                       : (x == X + Width - 1) ? EffectiveBorderStyle.BottomRight
                                                       : EffectiveBorderStyle.Horizontal;
                        }
                    }

                    // 修复右侧边框被覆盖的问题
                    if (endX == X + Width && endX - 1 < bufferWidth && Y >= 0 && Y < bufferHeight)
                    {
                        buffer[Y, endX - 1] = EffectiveBorderStyle.Vertical; // 强制渲染右侧边框
                    }

                    // 修复左侧边框被覆盖的问题
                    if (startX == X && startX >= 0 && Y >= 0 && Y < bufferHeight)
                    {
                        buffer[Y, startX] = EffectiveBorderStyle.Vertical; // 强制渲染左侧边框
                    }
                }



            }
        }
    }
}
