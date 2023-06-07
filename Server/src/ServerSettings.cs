using System.Reflection;


public class ServerSettings
{
    public readonly string? DirectoryPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    public readonly Version? Version = Assembly.GetExecutingAssembly().GetName().Version;
    public ushort Port { get; set; } = 17230;

}