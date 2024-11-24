using static TerminalUI.Style;

namespace TerminalUI
{
    public partial class TUI
    {
        // 抽象组件类定义
        // Abstract base class for components

        public abstract partial class Component
        {
            public BorderStyle BorderStyle { get; set; }

            // 新增：控制组件是否自动调整大小 / New: Control whether the component auto-resizes
            public bool AutoResize { get; set; } = false;
            protected virtual void AdjustSizeToFitText()
            {
                int textWidth = GetTextWidth(Text); // 计算文本宽度
                Width = Math.Max(Width, textWidth + 2); // 至少容纳文本宽度
                Height = Math.Max(Height, 3); // 至少容纳边框和内容
            }


            // 组件默认的边框样式（可以被子类覆盖） / Default border style for the component
            protected virtual BorderStyle? ComponentDefaultBorderStyle => null;

            public Component(BorderStyle? borderStyle = null)
            {
                // 优先级：外部提供的 borderStyle > 组件默认样式 > 全局默认样式
                BorderStyle = borderStyle ?? ComponentDefaultBorderStyle ?? BorderStyle.Default;
            }

            public int X { get; set; } // 组件的 X 坐标 / Component X coordinate
            public int Y { get; set; } // 组件的 Y 坐标 / Component Y coordinate
            private int width;
            public int Width
            {
                get => width;
                set
                {
                    if (!AutoResize || value > width)
                    {
                        width = value; // 更新宽度
                    }
                }
            }

            private int height;
            public int Height
            {
                get => height;
                set
                {
                    if (!AutoResize || value > height)
                    {
                        height = value; // 更新高度
                    }
                }
            }

            private int zIndex; // Z 层级 / Z-index
            public int ZIndex
            {
                get => zIndex;
                set
                {
                    zIndex = value;
                    ParentTUI?.UpdateZIndex(); // 通知父级重新排序 / Notify parent to update sorting
                }
            }

            public bool Visible { get; set; } = true; // 是否可见 / Whether the component is visible
            private string text = ""; // 组件显示的文本 / Text displayed by the component
            public string Text
            {
                get => text;
                set
                {
                    text = value;
                    if (AutoResize)
                    {
                        AdjustSizeToFitText(); // 自动调整大小
                    }
                }
            }

            public TUI ParentTUI { get; set; } // 指向父 TUI / Reference to parent TUI

            // 点击事件（虚拟方法，可被子类重写） / Click event (virtual, can be overridden)
            public virtual void OnClick() { }

            // 抽象方法，子类必须实现 / Abstract method, must be implemented by subclasses
            public abstract void Render(char[,] buffer);

            // 判断字符是否为全角字符 / Determine if a character is full-width
            public static bool IsFullWidth(char c)
            {
                // Unicode 全角字符范围 / Unicode full-width character ranges
                return (c >= 0x1100 && c <= 0x11FF) ||
                       (c >= 0x2E80 && c <= 0xA4CF) ||
                       (c >= 0xAC00 && c <= 0xD7AF) ||
                       (c >= 0xF900 && c <= 0xFAFF) ||
                       (c >= 0xFE10 && c <= 0xFE1F) ||
                       (c >= 0xFF00 && c <= 0xFFEF);
            }

            // 计算文本总宽度 / Calculate the total width of text
            public static int GetTextWidth(string text)
            {
                int width = 0;
                foreach (char c in text)
                {
                    width += IsFullWidth(c) ? 2 : 1;
                }
                return width;
            }

            // 渲染文本至缓冲区 / Render text to the buffer
            public static void RenderTextWithWidth(char[,] buffer, int startX, int startY, string text, int maxWidth)
            {
                int renderedWidth = 0; // 当前已渲染宽度 / Current rendered width
                for (int i = 0; i < text.Length && renderedWidth < maxWidth; i++)
                {
                    char c = text[i];
                    int charWidth = IsFullWidth(c) ? 2 : 1;

                    // 如果字符超出目标宽度，则停止渲染 / Stop rendering if character exceeds target width
                    if (renderedWidth + charWidth > maxWidth) { break; }

                    // 渲染当前字符 / Render the current character
                    buffer[startY, startX + renderedWidth] = c;

                    // 如果是全角字符，占用两个单元 / If full-width, occupy two units
                    if (charWidth == 2 && renderedWidth + 1 < maxWidth)
                    {
                        buffer[startY, startX + renderedWidth + 1] = '\0'; // 占用但不填空格 / Occupy without filling a space
                    }

                    // 更新渲染宽度 / Update rendered width
                    renderedWidth += charWidth;
                }
            }
        }








    }
}
