


using System;
using System.Text;
using System.Threading;
using static TerminalUI.TUI;

namespace TerminalUI
{
    partial class TUI
    {
        private readonly TUIType tUIType;
        private readonly int tUIWidth;
        private readonly int tUIHeight;
        private bool tUIEnabled = false;
        private bool tUIRunning = true;
        private string[] frame = new string[4];

        private string title;
        private bool showTitle;
        private Style.TitleType titleType;
        public event Action OnDraw;




        private char[,] backBuffer;
        private char[,] frontBuffer;

        private int cursorX;
        private int cursorY;
        public char cursorStyle { get; set; }
        private List<Component> components = new List<Component>();





        public string Title
        {
            get => titleManager.Title;
            set
            {
                titleManager.Title = value;
                BuildUI(); // 更新 UI
            }
        }

        public Style.TitleType TitleType
        {
            get => titleManager.TitleType;
            set
            {
                titleManager.TitleType = value;
                BuildUI(); // 更新 UI
            }
        }


        private Style.TitleManager titleManager; // 定义 titleManager

        // 在构造函数中初始化
        public TUI(TUIType terminalUIType, int Width, int Height, bool ShowTitle = true, string Title = "", Style.TitleType InitialTitleType = null, char CursorStyle = '#', bool OpeningAnimation = false)
        {


            tUIType = terminalUIType;
            tUIWidth = Width * 2;
            tUIHeight = Height;
            cursorStyle = CursorStyle;
            showTitle = ShowTitle;
            title = Title;
            titleType = InitialTitleType ?? Style.TitleType.Mid;
            titleManager = new Style.TitleManager(title, titleType, showTitle);
            // 初始化 titleManager


            cursorX = tUIWidth / 2;
            cursorY = tUIHeight / 2;
            ErroCheck();

            frontBuffer = new char[tUIHeight, tUIWidth];
            backBuffer = new char[tUIHeight, tUIWidth];

            Console.CursorVisible = false;
            if (OpeningAnimation) { PlayOpeningAnimation(); }
            Thread thread = new Thread(Loop) { IsBackground = false };
            thread.Start();
        }
        private void PlayOpeningAnimation()
        {
            // 初始化缓冲区为空格
            for (int y = 0; y < tUIHeight; y++)
            {
                for (int x = 0; x < tUIWidth; x++)
                {
                    backBuffer[y, x] = ' ';
                }
            }

            // 最大距离是宽度和高度之和
            int maxDistance = tUIWidth + tUIHeight - 2;

            // 动画主循环
            for (int d = 0; d <= maxDistance; d++)
            {
                // 从左上角展开
                for (int y = 0; y <= d; y++)
                {
                    int x = d - y;
                    if (x < tUIWidth && y < tUIHeight)
                    {
                        if (y == 0 || y == tUIHeight - 1)
                        {
                            backBuffer[y, x] = (x == 0 || x == tUIWidth - 1) ? '+' : '-';
                        }
                        else if (x == 0 || x == tUIWidth - 1)
                        {
                            backBuffer[y, x] = '|';
                        }
                    }
                }

                // 从右下角展开
                for (int y = tUIHeight - 1; y >= tUIHeight - 1 - d; y--)
                {
                    int x = (tUIWidth - 1) - (d - (tUIHeight - 1 - y));
                    if (x >= 0 && y >= 0 && x < tUIWidth && y < tUIHeight)
                    {
                        if (y == 0 || y == tUIHeight - 1)
                        {
                            backBuffer[y, x] = (x == 0 || x == tUIWidth - 1) ? '+' : '-';
                        }
                        else if (x == 0 || x == tUIWidth - 1)
                        {
                            backBuffer[y, x] = '|';
                        }
                    }
                }

                // 绘制当前动画帧
                Draw();
                Thread.Sleep(10); // 控制动画帧速率
            }

            // 动画完成后生成完整的 UI
            BuildUI();
            Draw();
        }





        private void ErroCheck()
        {
            if ((tUIWidth <= 2 || tUIHeight <= 2))
            {
                throw new Exception($"[Error] The Width or Height must be greater than 2. Width={tUIWidth}, Height={tUIHeight}");
            }

        }

        private void Loop()
        {
            Thread inputThread = new Thread(HandleInput) { IsBackground = true };
            inputThread.Start();

            while (tUIRunning) // 外层死循环，保证线程一直存在
            {
                if (tUIEnabled)
                {
                    // 不再分发点击事件
                    BuildUI(); // 更新缓冲区
                    Draw();    // 渲染到屏幕
                }
            }
        }


        private void HandleInput()
        {
            while (tUIRunning)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;

                    switch (key)
                    {
                        case ConsoleKey.Enter: // 模拟点击事件
                            foreach (var component in components)
                            {
                                if (cursorX >= component.X && cursorX < component.X + component.Width &&
                                    cursorY >= component.Y && cursorY < component.Y + component.Height)
                                {
                                    component.OnClick(); // 调用点击事件（虚方法）
                                    break;
                                }
                            }
                            break;

                        case ConsoleKey.UpArrow:
                            cursorY = Math.Max(1, cursorY - 1); // 确保不超出边框
                            break;

                        case ConsoleKey.DownArrow:
                            cursorY = Math.Min(tUIHeight - 2, cursorY + 1);
                            break;

                        case ConsoleKey.LeftArrow:
                            cursorX = Math.Max(1, cursorX - 1);
                            break;

                        case ConsoleKey.RightArrow:
                            cursorX = Math.Min(tUIWidth - 2, cursorX + 1);
                            break;
                    }
                }
            }
        }



        private void BuildUI()
        {
            // 构造整个 UI，包括边框
            for (int y = 0; y < tUIHeight; y++)
            {
                for (int x = 0; x < tUIWidth; x++)
                {
                    // 绘制最外层顶部边框（索引 0 行）
                    if (y == 0)
                    {
                        backBuffer[y, x] = (x == 0 || x == tUIWidth - 1) ? '+' : '-';
                    }
                    // 绘制标题框的顶部边界（索引 2 行）
                    else if (y == 2)
                    {
                        backBuffer[y, x] = (x == 0 || x == tUIWidth - 1) ? '+' : '-';
                    }
                    // 绘制标题文字行（索引 1 行）
                    else if (y == 1 && titleManager.ShowTitle && titleManager.TitleType != Style.TitleType.None)
                    {
                        if (x == 0 || x == tUIWidth - 1)
                        {
                            backBuffer[y, x] = '|'; // 左右边框
                        }
                        else
                        {
                            backBuffer[y, x] = ' '; // 默认空格填充
                        }
                    }
                    // 绘制底部边框（索引 tUIHeight - 1 行）
                    else if (y == tUIHeight - 1)
                    {
                        backBuffer[y, x] = (x == 0 || x == tUIWidth - 1) ? '+' : '-';
                    }
                    // 绘制左右边框（其他行）
                    else if (x == 0 || x == tUIWidth - 1)
                    {
                        backBuffer[y, x] = '|';
                    }
                    // 填充内部为空格
                    else
                    {
                        backBuffer[y, x] = ' ';
                    }
                }
            }

            // 渲染标题文字（放在索引 1 行）
            if (titleManager.ShowTitle && titleManager.TitleType != Style.TitleType.None)
            {
                int titleStartX = 1; // 默认左对齐
                if (titleManager.TitleType == Style.TitleType.Mid)
                {
                    titleStartX = (tUIWidth - titleManager.Title.Length) / 2;
                }
                else if (titleManager.TitleType == Style.TitleType.Right)
                {
                    titleStartX = tUIWidth - titleManager.Title.Length - 1;
                }

                // 渲染标题文字（确保绘制在第 1 行）
                for (int i = 0; i < titleManager.Title.Length && titleStartX + i < tUIWidth - 1; i++)
                {
                    backBuffer[1, titleStartX + i] = titleManager.Title[i];
                }
            }

            // 渲染所有组件
            foreach (var component in components)
            {
                component.Render(backBuffer);
            }

            // 渲染指针
            backBuffer[cursorY, cursorX] = cursorStyle;
        }












        private void Draw()
        {
            StringBuilder output = new StringBuilder();

            // 获取当前控制台窗口的高度
            int visibleHeight = Math.Min(tUIHeight, Console.WindowHeight);
            int visibleWidth = Math.Min(tUIWidth, Console.WindowWidth);

            // 拼接可见区域的内容
            for (int y = 0; y < visibleHeight; y++)
            {
                for (int x = 0; x < visibleWidth; x++)
                {
                    if (frontBuffer[y, x] != backBuffer[y, x])
                    {
                        frontBuffer[y, x] = backBuffer[y, x]; // 更新前缓冲区
                    }
                    output.Append(backBuffer[y, x]);
                }
                if (y < visibleHeight - 1) // 防止额外换行
                {
                    output.AppendLine();
                }
            }

            // 设置光标到左上角
            Console.SetCursorPosition(0, 0);

            // 一次性输出整个缓冲区内容
            Console.Write(output.ToString());

            // 触发 OnDraw 事件
            OnDraw?.Invoke();
        }


        public void AddComponent(Component component)
        {
            components.Add(component);
            components = components.OrderBy(c => c.ZIndex).ToList(); // 根据 ZIndex 排序
        }






        public void EnableTUI()
        {
            tUIEnabled = true;
        }

        public void DisableTUI()
        {
            tUIEnabled = false;
        }

        public void StopTUI()
        {
            tUIRunning = false; // 退出外层死循环，结束线程
        }
    }
}
