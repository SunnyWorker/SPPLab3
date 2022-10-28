using NuGet.Frameworks;
using ScannerAPI.Help;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void DirectoryTest1()
    {
        DirectoryType directoryType = new("D:\\CodeBlocks");
        directoryType.Analyze();
        Assert.IsNotNull(directoryType);
        Assert.AreEqual(548803268, directoryType.Size);
        Assert.AreEqual(2, directoryType.InnerDirectories.Count);
        Assert.AreEqual(29, directoryType.InnerFiles.Count);
        Assert.AreEqual("CodeBlocks", directoryType.Name);
        Assert.AreEqual("MinGW",directoryType.InnerDirectories[0].Name);
        Assert.AreEqual("share",directoryType.InnerDirectories[1].Name);
        Assert.AreEqual("83,40",directoryType.InnerDirectories[0].PercentString);
        Assert.AreEqual("7,48",directoryType.InnerDirectories[1].PercentString);
    }
    
    [Test]
    public void DirectoryTest2()
    {
        DirectoryType directoryType = new("D:\\Картины");
        directoryType.Analyze();
        Assert.IsNotNull(directoryType);
        Assert.AreEqual(73015122, directoryType.Size);
        Assert.AreEqual(0, directoryType.InnerDirectories.Count);
        Assert.AreEqual(50, directoryType.InnerFiles.Count);
    }
}