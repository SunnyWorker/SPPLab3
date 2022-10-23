using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using ScannerAPI.Help;
using WPFFolderBrowser;


namespace ScannerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static DirectoryType directoryType;

        private void ThreadWork()
        {
            CancellationTokenSource source = new();
            string path = "";
            Dispatcher.Invoke(() =>
            {
                path = TextBox.Text.Replace("\\","\\\\");
            });
            directoryType = new(path);
            Dispatcher.Invoke(() =>
            {
                TextBox.Visibility = Visibility.Collapsed;
                Button1.Visibility = Visibility.Collapsed;
                Button2.Visibility = Visibility.Collapsed;
                cancellationButton.Visibility = Visibility.Visible;
                stateTextBlock.Visibility = Visibility.Visible;
                stateTextBlock.Text = "Генерируем DirectoryType...";
            });
            directoryType.Analyze();
            Dispatcher.Invoke(() =>
            {
                cancellationButton.Visibility = Visibility.Collapsed;
                stateTextBlock.Text = "Рисуем TreeView...";
            });
            Thread.Sleep(100);
            Dispatcher.Invoke(() =>
            {
                treeView.ItemsSource = new List<DirectoryType>(){directoryType};
            });
            Dispatcher.Invoke(() =>
            {
                Viewer.Visibility = Visibility.Visible;
                returnButton.Visibility = Visibility.Visible;
                stateTextBlock.Visibility = Visibility.Collapsed;
            });
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
            Thread thread = new Thread(ThreadWork);
            thread.Start();
        }

        private void Choose_Directory(object sender, RoutedEventArgs e)
        {
            WPFFolderBrowserDialog dialog = new WPFFolderBrowserDialog();
            dialog.InitialDirectory = "d:\\";
            if ((bool)dialog.ShowDialog())
            {
                TextBox.Text = dialog.FileName;
            }
        }

        private void ReturnButton_OnClick(object sender, RoutedEventArgs e)
        {
            treeView.ItemsSource = new List<DirectoryType>();
            returnButton.Visibility = Visibility.Collapsed;
            Viewer.Visibility = Visibility.Collapsed;
            TextBox.Visibility = Visibility.Visible;
            Button1.Visibility = Visibility.Visible;
            Button2.Visibility = Visibility.Visible;
        }

        private void CancellationButton_OnClick(object sender, RoutedEventArgs e)
        {
            directoryType.Cancel();
        }
    }
}