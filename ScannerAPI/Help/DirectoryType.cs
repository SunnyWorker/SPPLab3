using System.Collections.Concurrent;
using DirectoryInfo = System.IO.DirectoryInfo;

namespace ScannerAPI.Help;

public class DirectoryType
{
    private static Semaphore pool = new(12, 12);

    private DirectoryInfo directoryInfo;

    private double Percent;

    private static CancellationTokenSource cts;

    public string PercentString
    {
        get
        {
            return String.Format("{0:0.00}", Percent);
        }
    }

    public long Size
    {
        get;
        set;
    }

    private bool ready;

    private DirectoryType Parent;

    public String Name
    {
        get
        {
            return directoryInfo.Name;
        }
    }

    public List<DirectoryType> InnerDirectories
    {
        get;
    }

    public List<FileType> InnerFiles
    {
        get;
    }
    
    private DirectoryType(DirectoryInfo directoryInfo, DirectoryType parent)
    {
        cts = new();
        this.directoryInfo = directoryInfo;
        Parent = parent;
        ready = false;
        InnerDirectories = new List<DirectoryType>();
        InnerFiles = new List<FileType>();
        Size = 0;
    }
    
    public DirectoryType(string path) : this(new(path), null)
    {
        
    }

    public void Analyze()
    {
        ThreadPool.QueueUserWorkItem(Analyze);
        //Thread.Sleep(9000);
        WaitTask();
        Percent = 100;
    }

    private void Analyze(object obj)
    {
        pool.WaitOne();
        try
        {
            directoryInfo.GetFiles();
        }
        catch (UnauthorizedAccessException e)
        {
            ThreadPool.QueueUserWorkItem(TryAddSize);
            pool.Release();
            return;
        }
        long fileSize = 0;
        foreach (var fileInfo in directoryInfo.GetFiles())
        {
            if (cts.Token.IsCancellationRequested)
            {
                pool.Release();
                return;
            }
            if(fileInfo.LinkTarget!=null) continue;
            FileType fileType = new FileType(fileInfo);
            InnerFiles.Add(fileType);
            fileSize += fileType.Size;
        }

        try
        {
            Monitor.Enter(this);
            Size += fileSize;
        }
        finally
        {
            Monitor.Exit(this);
        }

        
        
        if (directoryInfo.GetDirectories().Length == 0)
        {
            ThreadPool.QueueUserWorkItem(TryAddSize);
        }


        foreach (var innerDirectoryInfo in directoryInfo.GetDirectories())
        {
            if (cts.Token.IsCancellationRequested)
            {
                pool.Release();
                return;
            }
            DirectoryType innerDirectoryType = new DirectoryType(innerDirectoryInfo,this);
            InnerDirectories.Add(innerDirectoryType);
        }
        
        foreach (var innerDirectoryType in InnerDirectories)
        {
            ThreadPool.QueueUserWorkItem(innerDirectoryType.Analyze);
        }
        pool.Release();

    }

    private void Percentage()
    {
        foreach (var fileType in InnerFiles)
        {
            double size1 = fileType.Size * 100;
            double size2 = Size;
            if (size1 < 1 && size2 < 1)
            {
                fileType.Percent = 0;
                continue;
            }
            try
            {
                fileType.Percent = size1 / size2;
            }
            catch (DivideByZeroException e)
            {
                fileType.Percent = 0;
            }
        }

        foreach (var innerDirectoryType in InnerDirectories)
        {
            double size1 = innerDirectoryType.Size * 100;
            double size2 = Size;
            if (size1 < 1 && size2 < 1)
            {
                innerDirectoryType.Percent = 0;
                continue;
            }
            try
            {
                innerDirectoryType.Percent = size1 / size2;
            }
            catch (DivideByZeroException e)
            {
                innerDirectoryType.Percent = 0;
            }
        }

    }

    private void TryAddSize(object? obj)
    {
        pool.WaitOne();
        Percentage();
        if (Parent != null)
        {
            if (cts.Token.IsCancellationRequested)
            {
                pool.Release();
                return;
            }
            Parent.TryAddSize(this);
        }
        else
        {
            ready = true;
        }
        pool.Release();
    }

    private void TryAddSize(DirectoryType childDirectory)
    {
        try
        {
            Monitor.Enter(this);
            Size += childDirectory.Size;
            childDirectory.ready = true;
            foreach (var innerDirectory in InnerDirectories)
            {
                if(innerDirectory.ready==false) return;
                if (cts.Token.IsCancellationRequested)
                {
                    return;
                }
            }
            if (cts.Token.IsCancellationRequested)
            {
                return;
            }
            Percentage();
        
            if (Parent != null)
            {
                Parent.TryAddSize(this);
            }
            else
            {
                ready = true;
            }
        }
        finally
        {
            Monitor.Exit(this);
        }
    }

    public override string ToString()
    {
        string output = "";
        output += directoryInfo.Name + " - " + Size + " - " + String.Format("{0:0.00}", Percent) + " %";
        return output;
    }

    public void Cancel()
    {
        ready = true;
        cts.Cancel();
    }

    
    public void WaitTask()
    {
        if (Parent==null)
        {
            while (!ready)
            {

            }
        }
    }
}