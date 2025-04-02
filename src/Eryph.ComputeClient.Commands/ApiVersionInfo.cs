namespace Eryph.ComputeClient.Commands;

public class ApiVersionInfo(int major, int minor)
{
    public int Major { get; } = major;

    public int Minor { get; } = minor;

    public bool IsCompatible(int major, int minor)
    {
        return Major == major && Minor >= minor;
    }
}
