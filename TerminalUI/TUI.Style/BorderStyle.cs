namespace TerminalUI
{
    public partial class Style
    {
        public class BorderStyle
        {
            public char TopLeft { get; set; } = '+';
            public char TopRight { get; set; } = '+';
            public char BottomLeft { get; set; } = '+';
            public char BottomRight { get; set; } = '+';
            public char Horizontal { get; set; } = '-';
            public char Vertical { get; set; } = '|';

            // 默认边框样式
            public static readonly BorderStyle Default = new BorderStyle();

            public BorderStyle() { }

            public BorderStyle(char topLeft, char topRight, char bottomLeft, char bottomRight, char horizontal, char vertical)
            {
                TopLeft = topLeft;
                TopRight = topRight;
                BottomLeft = bottomLeft;
                BottomRight = bottomRight;
                Horizontal = horizontal;
                Vertical = vertical;
            }
        }

    }
}
