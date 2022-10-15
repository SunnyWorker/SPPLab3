// See https://aka.ms/new-console-template for more information

using ScannerAPI;
using ScannerAPI.Help;

IScanner scanner = new Scanner();
DirectoryInfo directoryInfo = new DirectoryInfo("D:\\Arduino");
DirectoryType directoryType = scanner.Analyze(directoryInfo);
Console.WriteLine(directoryType);