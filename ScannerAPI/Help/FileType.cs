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
}