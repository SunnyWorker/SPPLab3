// See https://aka.ms/new-console-template for more information

using ScannerAPI;
using ScannerAPI.Help;

IScanner scanner = new Scanner();
DirectoryInfo directoryInfo = new DirectoryInfo("D:\\Hamachi");
DirectoryType directoryType = scanner.Analyze(directoryInfo);
scanner.SetPercents(directoryType);
Console.WriteLine(directoryType);