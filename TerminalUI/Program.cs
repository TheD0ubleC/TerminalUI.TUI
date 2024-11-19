using System;
using System.Diagnostics;
using TerminalUI;

class Program
{
    static void Main(string[] args)
    {
        // 初始化 TUI
        TUI tUI = new TUI(
            TUI.TUIType.PowerShell,
            50,
            30,
            OpeningAnimation: false,
            InitialTitleType: Style.TitleType.Mid,
            Title: "FPS Demo"
        );

        // 定义帧率统计变量
        int frameCount = 0; // 帧计数器
        double fps = 0;     // 当前帧率
        Stopwatch stopwatch = new Stopwatch(); // 用于计时
        stopwatch.Start(); // 启动计时器

        // 每帧执行的逻辑
        tUI.OnDraw += () =>
        {
            // 统计帧数
            frameCount++;

            // 每秒更新一次 FPS
            if (stopwatch.Elapsed.TotalSeconds >= 1.0)
            {
                fps = frameCount / stopwatch.Elapsed.TotalSeconds; // 计算帧率
                frameCount = 0; // 重置帧计数器
                stopwatch.Restart(); // 重置计时器
            }

            // 更新 TUI 的标题显示帧率
            tUI.Title = $"FPS: {fps:F2} - Time: {DateTime.Now:HH:mm:ss}";
        };

        // 添加一个按钮组件
        TUI.Component.TButton button1 = new TUI.Component.TButton
        {
            X = 10,
            Y = 5,
            Width = 20,
            Height = 3,
            Text = "Click Me!"
        };

        // 按钮点击事件
        button1.OnClickAction = () =>
        {
            tUI.TitleType = Style.TitleType.Left;
        };

        tUI.AddComponent(button1);

        // 启动 TUI
        tUI.EnableTUI();
    }
}
