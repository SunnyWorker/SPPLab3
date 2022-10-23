using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ScannerAPI.Help;
using WPFFolderBrowser;

namespace ScannerUI;

public class ApplicationViewModel : INotifyPropertyChanged
{
    private static DirectoryType directoryType;

    private string textBoxString;
    public string TextBoxString
    {
        get { return textBoxString;}
        set
        {
            textBoxString = value;
            OnPropertyChanged();
        }
    }
    
    private string textBlockText;
    public string TextBlockText
    {
        get { return textBlockText;}
        set
        {
            textBlockText = value;
            OnPropertyChanged();
        }
    }
    
    private Visibility textBoxVisibility;
    public Visibility TextBoxVisibility
    {
        get { return textBoxVisibility;}
        set
        {
            textBoxVisibility = value;
            OnPropertyChanged();
        }
    }
    
    private Visibility returnButtonVisibility;
    public Visibility ReturnButtonVisibility
    {
        get { return returnButtonVisibility;}
        set
        {
            returnButtonVisibility = value;
            OnPropertyChanged();
        }
    }
    
    private Visibility textBlockVisibility;
    public Visibility TextBlockVisibility
    {
        get { return textBlockVisibility;}
        set
        {
            textBlockVisibility = value;
            OnPropertyChanged();
        }
    }
    
    private Visibility cancellationButtonVisibility;
    public Visibility CancellationButtonVisibility
    {
        get { return cancellationButtonVisibility;}
        set
        {
            cancellationButtonVisibility = value;
            OnPropertyChanged();
        }
    }
    
    private Visibility scrollViewerVisibility;
    public Visibility ScrollViewerVisibility
    {
        get { return scrollViewerVisibility;}
        set
        {
            scrollViewerVisibility = value;
            OnPropertyChanged();
        }
    }
    
    private Visibility chooseButtonVisibility;
    public Visibility ChooseButtonVisibility
    {
        get { return chooseButtonVisibility;}
        set
        {
            chooseButtonVisibility = value;
            OnPropertyChanged();
        }
    }
    
    private Visibility generateButtonVisibility;
    public Visibility GenerateButtonVisibility
    {
        get { return generateButtonVisibility;}
        set
        {
            generateButtonVisibility = value;
            OnPropertyChanged();
        }
    }
    

    private TreeView treeView;

    private Dispatcher dispatcher;
    
    private void GenerateTreeView()
    {
        CancellationTokenSource source = new();
        string path = "";
        path = TextBoxString.Replace("\\","\\\\");
        directoryType = new(path);
        
        TextBoxVisibility = Visibility.Collapsed;
        GenerateButtonVisibility = Visibility.Collapsed;
        ChooseButtonVisibility = Visibility.Collapsed;
        CancellationButtonVisibility = Visibility.Visible;
        TextBlockVisibility = Visibility.Visible;
        TextBlockText = "Генерируем DirectoryType...";
        
        directoryType.Analyze();
        CancellationButtonVisibility = Visibility.Collapsed;
        TextBlockText = "Рисуем TreeView...";
        
        Thread.Sleep(100);
        dispatcher.Invoke(() =>
        {
            treeView.ItemsSource = new List<DirectoryType>(){directoryType};
        });
        ScrollViewerVisibility = Visibility.Visible; 
        ReturnButtonVisibility = Visibility.Visible; 
        TextBlockVisibility = Visibility.Collapsed;
    }
    public ApplicationViewModel(MainWindow window)
    {
        treeView = window.treeView;
        dispatcher = window.Dispatcher;
        TextBoxString = "";
        GenerateButtonVisibility = Visibility.Visible;
        ChooseButtonVisibility = Visibility.Visible;
        TextBoxVisibility = Visibility.Visible;
        ScrollViewerVisibility = Visibility.Collapsed;
        ReturnButtonVisibility = Visibility.Collapsed;
        TextBlockVisibility = Visibility.Collapsed;
        TextBlockText = "";
        CancellationButtonVisibility = Visibility.Collapsed;
        treeView.ItemsSource = new List<DirectoryType>();
    }

    private RelayCommand chooseCommand;
    public RelayCommand ChooseCommand
    {
        get
        {
            return chooseCommand ??
                   (chooseCommand = new RelayCommand(obj =>
                   {
                       ChooseDirectory();
                   }));
        }
    }
    
    private RelayCommand generateCommand;
    public RelayCommand GenerateCommand
    {
        get
        {
            return generateCommand ??
                   (generateCommand = new RelayCommand(obj =>
                   {
                       Thread thread = new Thread(GenerateTreeView);
                       thread.Start();
                   }));
        }
    }
    
    private RelayCommand cancelCommand;
    public RelayCommand CancelCommand
    {
        get
        {
            return cancelCommand ??
                   (cancelCommand = new RelayCommand(obj =>
                   {
                       Cancel();
                   }));
        }
    }
    
    private RelayCommand returnCommand;
    public RelayCommand ReturnCommand
    {
        get
        {
            return returnCommand ??
                   (returnCommand = new RelayCommand(obj =>
                   {
                       Return();
                   }));
        }
    }

    private void ChooseDirectory()
    {
        WPFFolderBrowserDialog dialog = new WPFFolderBrowserDialog();
        dialog.InitialDirectory = "d:\\";
        if ((bool)dialog.ShowDialog())
        {
            TextBoxString = dialog.FileName;
        }
    }
    
    private void Return()
    {
        treeView.ItemsSource = new List<DirectoryType>();
        ReturnButtonVisibility = Visibility.Collapsed;
        ScrollViewerVisibility = Visibility.Collapsed;
        TextBoxVisibility = Visibility.Visible;
        GenerateButtonVisibility = Visibility.Visible;
        ChooseButtonVisibility = Visibility.Visible;
    }
    
    private void Cancel()
    {
        directoryType.Cancel();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}