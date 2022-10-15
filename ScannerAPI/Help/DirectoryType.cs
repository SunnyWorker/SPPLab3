namespace ScannerAPI.Help;

public class DirectoryType
{
    public long Size
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
        Size = 0;
    }

    public override string ToString()
    {
        string output = "-"+_directoryInfo.Name+" - "+Size+"\n";
        foreach (var innerFile in InnerFiles)
        {
            output += innerFile;
            output += "\n";
        }

        foreach (var innerDirectory in InnerDirectories)
        {
            output += innerDirectory;
        }

        return output;
    }
}