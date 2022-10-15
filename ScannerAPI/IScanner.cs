using ScannerAPI.Help;

namespace ScannerAPI;

public interface IScanner
{
    DirectoryType Analyze(DirectoryInfo directoryInfo);
}