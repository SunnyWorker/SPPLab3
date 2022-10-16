using ScannerAPI.Help;

namespace ScannerAPI;

public class Scanner : IScanner
{
    private static Semaphore pool;
    public DirectoryType Analyze(DirectoryInfo directoryInfo)
    {
        // pool = new Semaphore(0, 3);
        DirectoryType directoryType = new DirectoryType(directoryInfo,null,0);
        return Analyze(directoryType);
    }

    private DirectoryType Analyze(DirectoryType directoryType)
    {
        // pool.WaitOne();
        long fileSize = 0;
        foreach (var fileInfo in directoryType.DirectoryInfo.GetFiles())
        {
            FileType fileType = new FileType(fileInfo);
            fileType.Nesting = directoryType.Nesting + 1;
            directoryType.InnerFiles.Add(fileType);
            fileSize += fileType.Size;
        }

        if (directoryType.DirectoryInfo.GetDirectories().Length == 0)
        {
            directoryType.TryAddSize(fileSize);
        }
        else
        {
            directoryType.Size += fileSize;
        }

        foreach (var innerDirectoryInfo in directoryType.DirectoryInfo.GetDirectories())
        {
            DirectoryType innerDirectoryType = new DirectoryType(innerDirectoryInfo,directoryType,directoryType.Nesting+1);
            directoryType.InnerDirectories.Add(innerDirectoryType);
        }
        
        foreach (var innerDirectoryType in directoryType.InnerDirectories)
        {
            Analyze(innerDirectoryType);
        }

        // pool.Release();
        return directoryType;
    }

    // private DirectoryType AnalyzeInThread(DirectoryInfo requiredDirectory, DirectoryType parent, int nesting)
    // {
    //     Thread thread = new Thread(new ParameterizedThreadStart(Analyze));
    //     thread.Start(requiredDirectory,parent,nesting);
    // }

    public void SetPercents(DirectoryType directoryType)
    {
        foreach (var fileType in directoryType.InnerFiles)
        {
            double size1 = fileType.Size * 100;
            double size2 = directoryType.Size;
            try
            {
                fileType.Percent = size1 / size2;
            }
            catch (ArithmeticException e)
            {
                fileType.Percent = 0;
            }
        }

        foreach (var innerDirectoryType in directoryType.InnerDirectories)
        {
            double size1 = innerDirectoryType.Size * 100;
            double size2 = directoryType.Size;
            try
            {
                innerDirectoryType.Percent = size1 / size2;
            }
            catch (ArithmeticException e)
            {
                innerDirectoryType.Percent = 0;
            }
            SetPercents(innerDirectoryType);
        }

    }
}