namespace ScannerAPI.Help;

public class DirectoryType
{
    public int Size
    {
        get;
        set;
    }
    
    public List<DirectoryType> InnerDirectories
    {
        get;
    }

    public List<FileType> InnerFiles
    {
        get;
    }

    private DirectoryInfo _directoryInfo;

    public DirectoryType(DirectoryInfo directoryInfo)
    {
        _directoryInfo = directoryInfo;
        InnerDirectories = new List<DirectoryType>();
        InnerFiles = new List<FileType>();
    }



}