namespace TerminalUI
{
    public partial class Style
    {
        // TitleType 定义：用于表示标题的对齐方式
        public class TitleType
        {
            public static readonly TitleType Left = new TitleType(0);
            public static readonly TitleType Mid = new TitleType(1);
            public static readonly TitleType Right = new TitleType(2);
            public static readonly TitleType None = new TitleType(3);

            public int Index { get; } // 提供整型索引

            private TitleType(int index)
            {
                Index = index;
            }
            public static implicit operator int(TitleType titleType)
            {
                return titleType.Index;
            }
        }

        // TitleManager 定义：管理标题相关逻辑
        internal class TitleManager
        {
            public string Title { get; set; }
            public TitleType TitleType { get; set; }
            public bool ShowTitle { get; set; }

            public TitleManager(string title, TitleType titleType, bool showTitle)
            {
                Title = title;
                TitleType = titleType;
                ShowTitle = showTitle;
            }
        }
    }
}
