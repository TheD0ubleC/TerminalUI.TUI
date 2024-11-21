namespace TerminalUI
{
    public partial class Style
    {
        // TitleType 定义：用于表示标题的对齐方式
        // TitleType Definition: Represents title alignment
        public class TitleType
        {
            public static readonly TitleType Left = new TitleType(0); // 左对齐 Left alignment
            public static readonly TitleType Mid = new TitleType(1); // 居中对齐 Center alignment
            public static readonly TitleType Right = new TitleType(2); // 右对齐 Right alignment
            public static readonly TitleType None = new TitleType(3); // 无标题 No title

            public int Index { get; } // 提供整型索引 Provides integer index

            private TitleType(int index)
            {
                Index = index;
            }

            public static implicit operator int(TitleType titleType)
            {
                return titleType.Index; // 隐式转换为整型 Implicitly convert to integer
            }
        }

        // TitleManager 定义：管理标题相关逻辑
        // TitleManager Definition: Manages title-related logic
        internal class TitleManager
        {
            public string Title { get; set; } // 标题文本 Title text
            public TitleType TitleType { get; set; } // 标题对齐方式 Title alignment type
            public bool ShowTitle { get; set; } // 是否显示标题 Whether to show title

            public TitleManager(string title, TitleType titleType, bool showTitle)
            {
                Title = title;
                TitleType = titleType;
                ShowTitle = showTitle;
            }
        }
    }
}
