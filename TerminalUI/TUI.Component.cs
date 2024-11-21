﻿namespace TerminalUI
{
    public partial class TUI
    {
        // 抽象组件类定义
        // Abstract base class for components
        public abstract partial class Component
        {
            public int X { get; set; } // 组件的 X 坐标 / Component X coordinate
            public int Y { get; set; } // 组件的 Y 坐标 / Component Y coordinate
            public int Width { get; set; } // 宽度 / Component width
            public int Height { get; set; } // 高度 / Component height

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
                    int textWidth = GetTextWidth(text); // 计算文本宽度 / Calculate text width
                    Width = Math.Max(Width, textWidth + 2); // 更新组件宽度 / Update component width
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

            // 计算填充空格的文本 / Get padded text with spaces
            public static string GetPaddedText(string text, int targetWidth)
            {
                int textWidth = GetTextWidth(text); // 计算文本宽度 / Calculate text width
                int padding = Math.Max(0, targetWidth - textWidth); // 计算需要填充的空格数 / Calculate padding spaces
                return text + new string(' ', padding); // 返回填充后的文本 / Return padded text
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
