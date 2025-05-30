﻿using System.Diagnostics;
using System.Windows.Input;

namespace CAT.Views.Pages.ReminderPages;

public partial class ReminderInterfaceShow
{
    public ReminderInterfaceShow()
    {
        InitializeComponent();
    }

    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ViewsClass.MainWindowClass.DragMove();
    }

    public void Close(object sender, RoutedEventArgs e)
    {
        Visibility = Visibility.Collapsed;
    }

    public async void Update(object sender, RoutedEventArgs e)
    {
        Visibility = Visibility.Collapsed;

        var jObject = await Api.GetApi("http://cat.chmlfrp.com/update/update.json");

        var exe = Path.Combine(
            Paths.DataPath,
            "CAT.exe"
        );

        if (await Api.Downloadasync(jObject["url"]?.ToString(), exe))
        {
            Reminders.Reminder_Box_Show("下载成功");
            Reminders.LogsOutputting("下载成功");

            var processPath = Process.GetCurrentProcess().MainModule?.FileName;
            var batchContent =
                $"""timeout /t 1 /nobreak & move /y "{exe}" "{processPath}" & start "" "{processPath}" & exit""";
            var process = new Process();
            ProcessStartInfo processInfo = new(
                "cmd.exe",
                $"/c {batchContent}"
            )
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.StartInfo = processInfo;
            process.Start();

            ViewsClass.MainWindowClass.Close();
        }
        else
        {
            Reminders.Reminder_Box_Show("更新失败", "red");
            Reminders.LogsOutputting("更新失败");
            Visibility = Visibility.Collapsed;
        }
    }

    private void TextTextBlock_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var offset = TextTextBlock.HorizontalOffset - e.Delta;
        TextTextBlock.ScrollToHorizontalOffset(offset);
        e.Handled = true;
    }
}