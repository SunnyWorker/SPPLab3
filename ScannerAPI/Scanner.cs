using ScannerAPI.Help;

namespace ScannerAPI;

public class Scanner : IScanner
{
    public DirectoryType Analyze(DirectoryInfo directoryInfo)
    {
        return Analyze(directoryInfo,null);
    }

    private DirectoryType Analyze(DirectoryInfo requiredDirectory, DirectoryType parent)
    {
        DirectoryType directoryType = new DirectoryType(requiredDirectory);
        foreach (var fileInfo in requiredDirectory.GetFiles())
        {
            FileType fileType = new FileType(fileInfo);
            directoryType.InnerFiles.Add(fileType);
            directoryType.Size += fileType.Size;
        }

        foreach (var directoryInfo in requiredDirectory.GetDirectories())
        {
            DirectoryType innerDirectoryType = new DirectoryType(directoryInfo);
            directoryType.InnerDirectories.Add(Analyze(directoryInfo,directoryType));
            directoryType.Size += innerDirectoryType.Size;
        }

        return directoryType;
    }
}