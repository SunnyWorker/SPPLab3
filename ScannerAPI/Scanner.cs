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
        return new DirectoryType(requiredDirectory);
    }
}