using System.Reflection;

namespace TestHelper;

public class FileInfoHelper
{
    public string? GetAssemblyLocation(Type type)
    {
        var uriPath = new UriBuilder(Assembly.GetExecutingAssembly().Location);
        return Path.GetDirectoryName(Uri.UnescapeDataString(uriPath.Path));
    }
}