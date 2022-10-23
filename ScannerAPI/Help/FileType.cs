namespace ScannerAPI.Help;

public class FileType
{
    public string Name
    {
        get
        {
            return _fileInfo.Name;
        }
    }
    public double Percent
    {
        get;
        set;
    }

    public string PercentString
    {
        get
        {
            return String.Format("{0:0.00}", Percent);
        }
    }
    public long Size
    {
        get
        {
            return _fileInfo.Length;
        }
    }

    private FileInfo _fileInfo;

    public FileType(FileInfo fileInfo)
    {
        _fileInfo = fileInfo;
    }
    
    public override string ToString()
    {
        string output = "";
        output += _fileInfo.Name+" - "+Size+" - "+String.Format("{0:0.00}", Percent)+" %";
        return output;
    }
}