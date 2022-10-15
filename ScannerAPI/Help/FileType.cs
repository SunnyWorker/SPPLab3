namespace ScannerAPI.Help;

public class FileType
{
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
        string output = "-"+_fileInfo.Name+" - "+Size;

        return output;
    }
}