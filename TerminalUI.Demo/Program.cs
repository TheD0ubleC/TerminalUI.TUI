using System;
using System.Diagnostics;
using TerminalUI;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // 初始化 TUI (Initialize TUI)
        TUI tUI = new TUI(
            TUI.TUIType.PowerShell, // 使用 PowerShell 作为终端类型 (Use PowerShell as terminal type)
            40,                     // 宽度 (Width)
            24,                     // 高度 (Height)
            OpeningAnimation: false, // 是否启用启动动画 (Enable opening animation)
            InitialTitleType: Style.TitleType.Mid, // 标题位置类型 (Title position type)
            Title: "TUI Calculator",               // 标题内容 (Title content)
            FPSMax: 0               // 最大帧率 (Max FPS, 0 for unlimited)
        );

        // 创建输入框组件 (Create input box component)
        TUI.Component.TLabel input = new TUI.Component.TLabel
        {
            X = 5,
            Y = 3,
            Text = "",
            Width = 71,
            Height = 5,
        };
        tUI.AddComponent(input); // 添加到 TUI (Add to TUI)

        // 创建数字按钮 (Create number buttons)
        List<TUI.Component.TButton> buttons = new List<TUI.Component.TButton>();
        int buttonWidth = 7;      // 按钮宽度 (Button width)
        int buttonHeight = 3;     // 按钮高度 (Button height)

        // 数字按钮位置 (Positions for number buttons)
        int[,] buttonPositions = {
            { 5, 10 }, { 13, 10 }, { 21, 10 }, // 第一行 (Row 1)
            { 5, 14 }, { 13, 14 }, { 21, 14 }, // 第二行 (Row 2)
            { 5, 18 }, { 13, 18 }, { 21, 18 }  // 第三行 (Row 3)
        };

        // 添加数字按钮到界面 (Add number buttons to interface)
        for (int i = 0; i < 9; i++)
        {
            buttons.Add(new TUI.Component.TButton
            {
                X = buttonPositions[i, 0],
                Y = buttonPositions[i, 1],
                Width = buttonWidth,
                Height = buttonHeight,
                Text = "  " + (i + 1).ToString() // 按钮文本为数字 (Button text is the number)
            });
            tUI.AddComponent(buttons[i]);
        }

        // 添加 "0" 按钮 (Add "0" button)
        TUI.Component.TButton zeroButton = new TUI.Component.TButton
        {
            X = 29,
            Y = 10,
            Width = buttonWidth,
            Height = buttonHeight * 3 + 2,
            Text = "  0"
        };
        tUI.AddComponent(zeroButton);

        // 版本信息按钮 (Version info button)
        TUI.Component.TButton ver = new TUI.Component.TButton
        {
            X = 69,
            Y = 18,
            Height = 3,
            Width = 7,
            Text = "  V",
            
        };

        // 防止重复触发动画的标志 (Flag to prevent animation re-triggering)
        bool isAnimating = false;

        // 点击动作：显示版本信息动画 (OnClick action: show version info animation)
        ver.OnClickAction = async () =>
        {
            
            if (isAnimating) return; // 如果动画正在运行，直接返回 (Return if animation is running)
            isAnimating = true;

            string versionInfo = "Hey there!You are now looking at *TerminalUI.TUI.Demo* - TUI-Calculator!!!";
            string originalTitle = tUI.Title; // 保存原始标题 (Save original title)

            // 逐字删除当前标题 (Clear current title character by character)
            for (int i = originalTitle.Length; i > 0; i--)
            {
                tUI.Title = originalTitle.Substring(0, i - 1); // 每次减少一个字符 (Remove one character at a time)
                await Task.Delay(30); // 控制清除动画速度 (Control clearing animation speed)
            }

            // 显示版本信息逐字显示 (Show version info character by character)
            tUI.Title = "";
            foreach (char c in versionInfo)
            {
                tUI.Title += c; // 每次增加一个字符 (Add one character at a time)
                await Task.Delay(30); // 控制显示动画速度 (Control display animation speed)
            }

            // 停留 10 秒 (Stay for 10 seconds)
            await Task.Delay(10000);

            // 逐字删除版本信息 (Clear version info character by character)
            for (int i = tUI.Title.Length; i > 0; i--)
            {
                tUI.Title = tUI.Title.Substring(0, i - 1); // 每次减少一个字符 (Remove one character at a time)
                await Task.Delay(30); // 控制清除动画速度 (Control clearing animation speed)
            }

            // 恢复原始标题逐字显示 (Restore original title character by character)
            foreach (char c in originalTitle)
            {
                tUI.Title += c; // 每次增加一个字符 (Add one character at a time)
                await Task.Delay(30); // 控制恢复动画速度 (Control restoration animation speed)
            }
            isAnimating = false; // 动画结束 (Animation finished)
        };

        tUI.AddComponent(ver); // 添加到 TUI (Add to TUI)

        // 运算符按钮 (Operator buttons)
        string[] operators = { "  +", "  -", "  *", "  /", "  =", "  C", "  (", "  )", "  ^", " l n", " log", "  π", " cos", " tan" };
        List<TUI.Component.TButton> operatorButtons = new List<TUI.Component.TButton>();

        // 运算符按钮位置 (Positions for operator buttons)
        int[,] operatorPositions = {
            { 37, 10 }, { 37, 14 }, { 37, 18 }, // +, -, *
            { 45, 18 }, { 45, 10 }, { 45, 14 }, // /, =, C
            { 53, 10 }, { 53, 14 }, { 53, 18 }, // (, ), ^
            { 69, 10 }, { 61, 10 }, { 61, 14 }, // ln, log, π
            { 61, 18 }, { 69, 14 }              // cos, tan
        };

        // 添加运算符按钮到界面 (Add operator buttons to interface)
        for (int i = 0; i < operators.Length; i++)
        {
            operatorButtons.Add(new TUI.Component.TButton
            {
                X = operatorPositions[i, 0],
                Y = operatorPositions[i, 1],
                Width = buttonWidth,
                Height = buttonHeight,
                Text = "" + operators[i]
            });
            tUI.AddComponent(operatorButtons[i]);
        }

        // 设置数字按钮点击行为 (Set number buttons' click action)
        foreach (var button in buttons)
        {
            button.OnClickAction = () => { input.Text += button.Text.Trim(); };
        }

        // 设置 "0" 按钮点击行为 (Set "0" button click action)
        zeroButton.OnClickAction = () => { input.Text += "0"; };

        // 设置运算符按钮点击行为 (Set operator buttons' click action)
        foreach (var opButton in operatorButtons)
        {
            opButton.OnClickAction = () =>
            {
                string op = opButton.Text.Trim();
                if (op == "C")
                {
                    input.Text = ""; // 清空输入框 (Clear input box)
                }
                else if (op == "=")
                {
                    try
                    {
                        double result = Evaluate(input.Text.Trim()); // 计算表达式 (Evaluate expression)
                        input.Text = result.ToString();
                    }
                    catch
                    {
                        input.Text = "Error"; // 处理错误 (Handle error)
                    }
                }
                else if (op == "π")
                {
                    input.Text += Math.PI.ToString(); // 输入 π 的值 (Insert the value of π)
                }
                else
                {
                    input.Text += " " + op + " "; // 输入运算符 (Insert operator)
                }
            };
        }

        tUI.EnableTUI(); // 启用 TUI (Enable TUI)
    }

    // 计算表达式 (Evaluate the expression)
    static double Evaluate(string expression)
    {
        try
        {
            // 处理幂运算 (Handle power operation)
            if (expression.Contains("^"))
            {
                var parts = expression.Split('^');
                double baseNum = double.Parse(parts[0].Trim());
                double exponent = double.Parse(parts[1].Trim());
                return Math.Pow(baseNum, exponent);
            }

            // 处理 ln 运算 (Handle ln operation)
            if (expression.Contains("ln"))
            {
                var number = double.Parse(expression.Replace("ln", "").Trim());
                return Math.Log(number);
            }

            // 处理 log 运算 (Handle log operation)
            if (expression.Contains("log"))
            {
                var number = double.Parse(expression.Replace("log", "").Trim());
                return Math.Log(number, 2);
            }

            // 处理 cos 运算 (Handle cos operation)
            if (expression.Contains("cos"))
            {
                var number = double.Parse(expression.Replace("cos", "").Trim());
                return Math.Cos(number * Math.PI / 180); // 转换为弧度 (Convert to radians)
            }

            // 处理 tan 运算 (Handle tan operation)
            if (expression.Contains("tan"))
            {
                var number = double.Parse(expression.Replace("tan", "").Trim());
                return Math.Tan(number * Math.PI / 180); // 转换为弧度 (Convert to radians)
            }

            // 使用 DataTable 处理其他表达式 (Handle other expressions using DataTable)
            System.Data.DataTable table = new System.Data.DataTable();
            return Convert.ToDouble(table.Compute(expression, ""));
        }
        catch
        {
            throw new InvalidOperationException("Invalid expression"); // 表达式无效时抛出异常 (Throw exception for invalid expressions)
        }
    }
}
