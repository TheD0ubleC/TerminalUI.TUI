using System;
using System.Text;
using System.Threading;
using static TerminalUI.TUI;
using static TerminalUI.TUI.Component.TTitle;
using static TerminalUI.TUI.Component;
using static TerminalUI.Style;

namespace TerminalUI
{
    partial class TUI
    {
        private readonly TUIType tUIType;//测试阶段 仅支持powershell
        private readonly int tUIWidth;
        private readonly int tUIHeight;
        private bool tUIEnabled = false;
        private bool tUIRunning = true;

        private Style.TitleType titleType;
        public event Action OnDraw;
        public event Action OnMove;
        private int fpsMax; // Max frame rate
        private int frameInterval; // 每帧间隔 (毫秒)
        private DateTime lastFrameTime; // 上一帧绘制时间

        private char[,] backBuffer;
        private char[,] frontBuffer;
        public ConsoleColor CursorColor { get; set; } = ConsoleColor.Green; // 默认指针颜色

        public int CursorX { get; set; }
        public int CursorY { get; set; }
        public char CursorStyle { get; set; }
        private Style.BorderStyle borderStyle = Style.BorderStyle.Window; // 默认全局边框样式

        public Style.BorderStyle BorderStyle
        {
            get => borderStyle;
            set
            {
                borderStyle = value ?? Style.BorderStyle.Default;
                UpdateTitleEffectiveBorderStyle(); // 更新标题的有效边框样式
            }
        }

        private List<Component> components = new List<Component>();

        public int FPSMax
        {
            get => fpsMax;
            set
            {
                fpsMax = value;
                frameInterval = fpsMax > 0 ? 1000 / fpsMax : 0; // 动态计算帧间隔
            }
        }

        public string Title
        {
            get => titleComponent?.Text ?? string.Empty; // 如果没有标题组件，返回空字符串
            set
            {
                if (titleComponent != null)
                {
                    titleComponent.Text = value; // 更新标题文本
                    BuildUI(); // 更新 UI
                }
            }
        }

        public Style.TitleType TitleType
        {
            get => titleComponent?.Alignment ?? Style.TitleType.None; // 如果没有标题组件，返回无标题
            set
            {
                if (titleComponent != null)
                {
                    titleComponent.Alignment = value; // 更新对齐方式
                    BuildUI(); // 更新 UI
                }
            }
        }

        public void UpdateTitleEffectiveBorderStyle()
        {
            titleComponent?.BuildEffectiveBorderStyle();
        }

        private TTitle titleComponent;

        // 构造函数 Constructor
        public TUI(TUIType terminalUIType, int Width, int Height, bool ShowTitle = true, string Title = "", Style.TitleType? InitialTitleType = null, char ICursorStyle = '#', bool OpeningAnimation = false, int FPSMax = 0, BorderStyle DefaultBorderStyle = null)
        {
            tUIType = terminalUIType;
            tUIWidth = Width * 2;
            tUIHeight = Height;
            CursorStyle = ICursorStyle;

            // 初始化标题组件
            var alignment = InitialTitleType ?? Style.TitleType.Mid;
            titleComponent = new TTitle(Title, ShowTitle ? alignment : Style.TitleType.None)
            {
                X = 0,
                Y = 1,
                Width = tUIWidth,
            };

            if (DefaultBorderStyle != null) { BorderStyle = DefaultBorderStyle; }

            AddComponent(titleComponent); // 将标题组件添加到组件列表

            // 其他初始化逻辑
            this.FPSMax = FPSMax;
            CursorX = tUIWidth / 2;
            CursorY = tUIHeight / 2;

            ErroCheck(); // 检查尺寸合法性

            // 初始化缓冲区
            frontBuffer = new char[tUIHeight, tUIWidth];
            backBuffer = new char[tUIHeight, tUIWidth];

            Console.CursorVisible = false;

            // 如果启用启动动画
            if (OpeningAnimation)
            {
                PlayOpeningAnimation();
            }

            // 启动主线程
            Thread thread = new Thread(Loop) { IsBackground = false };
            thread.Start();

            lastFrameTime = DateTime.Now; // 初始化上一帧时间
        }



        private void PlayOpeningAnimation()
        {
            // 初始化缓冲区为空格 Initialize buffer with spaces
            for (int y = 0; y < tUIHeight; y++)
            {
                for (int x = 0; x < tUIWidth; x++)
                {
                    backBuffer[y, x] = ' ';
                }
            }

            int maxDistance = tUIWidth + tUIHeight - 2; // 最大距离计算

            // 动画主循环 Animation loop
            for (int d = 0; d <= maxDistance; d++)
            {
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

                Draw(); // 渲染当前动画帧 Render the current frame
            }

            BuildUI(); // 动画完成后生成 UI
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

            while (tUIRunning) // 外层死循环 Ensure thread persistence
            {
                if (tUIEnabled)
                {
                    BuildUI(); // 更新缓冲区
                    Draw(); // 渲染到屏幕
                }
            }
        }
        private void HandleInput()
        {
            // 处理输入逻辑 Handle user input
            while (tUIRunning)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;

                    switch (key)
                    {
                        case ConsoleKey.Enter: // 模拟点击事件 Simulate click event
                            foreach (var component in components)
                            {
                                if (CursorX >= component.X && CursorX < component.X + component.Width &&
                                    CursorY >= component.Y && CursorY < component.Y + component.Height)
                                {
                                    component.OnClick(); // 调用点击事件 Call OnClick
                                    break;
                                }
                            }
                            break;

                        case ConsoleKey.UpArrow: // 上方向键 Up arrow
                            CursorY = Math.Max(1, CursorY - 1); // 确保不超出边框 Ensure not out of bounds
                            OnMove?.Invoke();
                            break;

                        case ConsoleKey.DownArrow: // 下方向键 Down arrow
                            CursorY = Math.Min(tUIHeight - 2, CursorY + 1);
                            OnMove?.Invoke();
                            break;

                        case ConsoleKey.LeftArrow: // 左方向键 Left arrow
                            CursorX = Math.Max(1, CursorX - 1);
                            OnMove?.Invoke();
                            break;

                        case ConsoleKey.RightArrow: // 右方向键 Right arrow
                            CursorX = Math.Min(tUIWidth - 2, CursorX + 1);
                            OnMove?.Invoke();
                            break;
                    }
                }
            }
        }

        public void ForceDraw()
        {
            // 强制绘制当前画面 Force drawing the current frame
            Draw();
        }

        private void BuildUI()
        {
            borderStyle ??= Style.BorderStyle.Default;

            for (int y = 0; y < tUIHeight; y++)
            {
                for (int x = 0; x < tUIWidth; x++)
                {
                    backBuffer[y, x] = ' '; // 填充内部区域
                }
            }

            foreach (var component in components.Where(c => !(c is TTitle)))
            {
                component.Render(backBuffer);
            }

            for (int y = 0; y < tUIHeight; y++)
            {
                for (int x = 0; x < tUIWidth; x++)
                {
                    if (y == 0)
                    {
                        backBuffer[y, x] = (x == 0) ? borderStyle.TopLeft
                                        : (x == tUIWidth - 1) ? borderStyle.TopRight
                                        : borderStyle.Horizontal; // 顶部边框
                    }
                    else if (y == tUIHeight - 1)
                    {
                        backBuffer[y, x] = (x == 0) ? borderStyle.BottomLeft
                                        : (x == tUIWidth - 1) ? borderStyle.BottomRight
                                        : borderStyle.Horizontal; // 底部边框
                    }
                    else if (x == 0 || x == tUIWidth - 1)
                    {
                        backBuffer[y, x] = borderStyle.Vertical; // 左右边框
                    }
                }
            }

            foreach (var title in components.OfType<TTitle>())
            {
                title.BuildEffectiveBorderStyle();
                title.Render(backBuffer);
            }
            backBuffer[CursorY, CursorX] = CursorStyle;
        }





        private void Draw()
        {
            // 控制帧率限制
            if (fpsMax > 0)
            {
                TimeSpan elapsed = DateTime.Now - lastFrameTime;
                if (elapsed.TotalMilliseconds < frameInterval)
                {
                    int sleepTime = frameInterval - (int)elapsed.TotalMilliseconds;
                    if (sleepTime > 0) { Thread.Sleep(sleepTime); }
                }
            }

            int bufferHeight = Math.Min(tUIHeight, Console.WindowHeight);
            int bufferWidth = Math.Min(tUIWidth, Console.WindowWidth);

            for (int y = 0; y < bufferHeight; y++)
            {
                for (int x = 0; x < bufferWidth; x++)
                {
                    // 仅在内容发生变化时更新
                    if (frontBuffer[y, x] != backBuffer[y, x] || (y == CursorY && x == CursorX))
                    {
                        // 检查光标是否在有效范围内
                        if (x >= 0 && x < Console.BufferWidth && y >= 0 && y < Console.BufferHeight)
                        {
                            Console.SetCursorPosition(x, y);
                            if (y == CursorY && x == CursorX)
                            {
                                Console.ForegroundColor = CursorColor; // 设置指针颜色
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Gray; // 默认颜色
                            }
                            Console.Write(backBuffer[y, x]);
                            frontBuffer[y, x] = backBuffer[y, x]; // 同步前缓冲区
                        }
                    }
                }
            }

            Console.ResetColor(); // 重置控制台颜色
            lastFrameTime = DateTime.Now; // 更新上一帧时间
            OnDraw?.Invoke(); // 触发绘制事件
        }



        private ConsoleColor GetColorForPosition(int x, int y)
        {
            foreach (var component in components)
            {
                // 检查当前位置是否属于组件范围
                if (x >= component.X && x < component.X + component.Width &&
                    y >= component.Y && y < component.Y + component.Height)
                {
                    // 判断位置是边框还是文字
                    if (x == component.X || x == component.X + component.Width - 1 ||
                        y == component.Y || y == component.Y + component.Height - 1)
                    {
                        // 如果是顶角，返回顶角颜色
                        if ((x == component.X && y == component.Y) ||
                            (x == component.X + component.Width - 1 && y == component.Y) ||
                            (x == component.X && y == component.Y + component.Height - 1) ||
                            (x == component.X + component.Width - 1 && y == component.Y + component.Height - 1))
                        {
                            return component.CornerColor;
                        }

                        // 返回边框颜色
                        return component.BorderColor;
                    }

                    // 返回文字颜色
                    return component.TextColor;
                }
            }

            return ConsoleColor.Gray; // 默认颜色
        }





        public void UpdateZIndex()
        {
            components = components.OrderBy(c => c.ZIndex).ToList();

            // 确保TTitle始终在最后
            var title = components.OfType<TTitle>().FirstOrDefault();
            if (title != null)
            {
                components.Remove(title);
                components.Add(title); // 移动到列表末尾
            }
        }


        public void AddComponent(Component component)
        {
            component.ParentTUI = this; // 设置父组件引用
            components.Add(component);

            // 如果是TTitle，确保有效边框样式已被初始化
            if (component is TTitle title)
            {
                title.BuildEffectiveBorderStyle();
            }

            UpdateZIndex(); // 重新排序
        }





        public void EnableTUI()
        {
            tUIEnabled = true; // 启用 TUI Enable TUI
        }

        public void DisableTUI()
        {
            tUIEnabled = false; // 禁用 TUI Disable TUI
        }

        public void StopTUI()
        {
            tUIRunning = false; // 停止循环 Stop loop
        }
    }
}
