// See https://aka.ms/new-console-template for more information

using System.Net.WebSockets;
using System.Threading.Channels;
using ScannerAPI;
using ScannerAPI.Help;

var watch = new System.Diagnostics.Stopwatch();
watch.Start();
DirectoryType directoryType = new DirectoryType(new DirectoryInfo("D:\\Steam"));
ThreadPool.QueueUserWorkItem(directoryType.Analyze);
directoryType.WaitTask();
watch.Stop();
Console.WriteLine(directoryType);

