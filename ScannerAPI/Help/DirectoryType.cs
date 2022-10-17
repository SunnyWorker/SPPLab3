using System.Collections.Concurrent;

namespace ScannerAPI.Help;

public class DirectoryType
{
    private static Semaphore pool = new(3, 3);

    private DirectoryInfo directoryInfo;
    
    private int Nesting;

    private double Percent;

    private long Size;

    private bool ready;

    private DirectoryType Parent;

    private List<DirectoryType> InnerDirectories;

    private List<FileType> InnerFiles;
    
    public DirectoryType(DirectoryInfo directoryInfo, DirectoryType parent, int nesting)
    {
        Nesting = nesting;
        this.directoryInfo = directoryInfo;
        Parent = parent;
        ready = false;
        InnerDirectories = new List<DirectoryType>();
        InnerFiles = new List<FileType>();
        Size = 0;
    }
    
    public DirectoryType(DirectoryInfo directoryInfo) : this(directoryInfo, null, 0)
    {
        
    }

    public void Analyze(object? state)
    {
        pool.WaitOne();
        long fileSize = 0;
        foreach (var fileInfo in directoryInfo.GetFiles())
        {
            if(fileInfo.LinkTarget!=null) continue;
            FileType fileType = new FileType(fileInfo);
            fileType.Nesting = Nesting + 1;
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
            DirectoryType innerDirectoryType = new DirectoryType(innerDirectoryInfo,this,Nesting+1);
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

    private void TryAddSize(object? state)
    {
        pool.WaitOne();
        Percentage();
        if (Parent != null)
        {
            Parent.TryAddSize(this);
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
        for (int i = 0; i < Nesting; i++)
        {
            output += "\t";
        }
        output += "-"+directoryInfo.Name+" - "+Size+" - "+String.Format("{0:0.00}", Percent)+" % "+Nesting+"\n";
        foreach (var innerFile in InnerFiles)
        {
            output += innerFile;
        }

        foreach (var innerDirectory in InnerDirectories)
        {
            output += innerDirectory;
        }

        return output;
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