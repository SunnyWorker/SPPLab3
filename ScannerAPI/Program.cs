﻿// See https://aka.ms/new-console-template for more information

using ScannerAPI.Help;

var watch = new System.Diagnostics.Stopwatch();
watch.Start();
DirectoryType directoryType = new("D:\\CodeBlocks");
directoryType.Analyze();
watch.Stop();
Console.WriteLine(directoryType);

