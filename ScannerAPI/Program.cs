// See https://aka.ms/new-console-template for more information

using ScannerAPI.Help;

var watch = new System.Diagnostics.Stopwatch();
watch.Start();
DirectoryType directoryType = new("D:\\Hamachi");
watch.Stop();
Console.WriteLine(directoryType);

