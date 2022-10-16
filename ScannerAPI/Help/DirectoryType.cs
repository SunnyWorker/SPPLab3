namespace ScannerAPI.Help;

public class DirectoryType
{
    public int Nesting
    {
        get;
    }
    public double Percent
    {
        get;
        set;
    }
    public long Size
    {
        get;
        set;
    }
    
    public bool Ready
    {
        get;
        set;
    }

    public DirectoryType Parent
    {
        get;
    }
    
    public List<DirectoryType> InnerDirectories
    {
        get;
    }

    public List<FileType> InnerFiles
    {
        get;
    }

    public DirectoryInfo DirectoryInfo
    {
        get;
    }

    public DirectoryType(DirectoryInfo directoryInfo, DirectoryType parent, int nesting)
    {
        Nesting = nesting;
        DirectoryInfo = directoryInfo;
        Parent = parent;
        Ready = false;
        InnerDirectories = new List<DirectoryType>();
        InnerFiles = new List<FileType>();
        Size = 0;
    }

    public void TryAddSize(long sizeToAdd)
    {
        Size += sizeToAdd;
        if(Parent!=null) Parent.TryAddSize(this);
    }

    public void TryAddSize(DirectoryType childDirectory)
    {
        Size += childDirectory.Size;
        childDirectory.Ready = true;
        foreach (var innerDirectory in InnerDirectories)
        {
            if(innerDirectory.Ready==false) return;
        }
        if(Parent!=null) Parent.TryAddSize(this);
    }

    public override string ToString()
    {
        string output = "";
        for (int i = 0; i < Nesting; i++)
        {
            output += "\t";
        }
        output += "-"+DirectoryInfo.Name+" - "+Size+" - "+String.Format("{0:0.00}", Percent)+" % "+Nesting+"\n";
        foreach (var innerFile in InnerFiles)
        {
            output += innerFile;
        }

        foreach (var innerDirectory in InnerDirectories)
        {
            output += innerDirectory;
        }

        return output;
    }
}