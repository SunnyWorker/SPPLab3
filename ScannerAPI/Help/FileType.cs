namespace ScannerAPI.Help;

public class FileType
{
    public int Nesting
    {
        get;
        set;
    }
    public double Percent
    {
        get;
        set;
    }
    public long Size
    {
        get
        {
            return _fileInfo.Length;
        }
    }

    private FileInfo _fileInfo;

    public FileType(FileInfo fileInfo)
    {
        _fileInfo = fileInfo;
    }
    
    public override string ToString()
    {
        string output = "";
        for (int i = 0; i < Nesting; i++)
        {
            output += "\t";
        }
        output += "-"+_fileInfo.Name+" - "+Size+" - "+String.Format("{0:0.00}", Percent)+" % "+Nesting+"\n";

        return output;
    }
}