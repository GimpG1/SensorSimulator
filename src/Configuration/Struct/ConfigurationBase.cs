using System.Reflection;

namespace Configuration.Struct;

public abstract class ConfigurationBase
{
    private Func<Task>? _readConfig;
    protected IConfiguration? Configuration;
    public abstract string ConfigName { get; }
    public string ConfigPath => 
        Path.Combine(Path.GetDirectoryName(
            Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().Location).Path))!
            , "Configuration", 
            ConfigName);

    protected void SetReadMethod(Func<Task>? reader)
    {
        _readConfig = reader;
    }

    public async Task ReadAsync()
    {
        if (Exists())
        {
            await Task.FromResult(_readConfig?.Invoke());
        }

        await Task.FromResult(Task.CompletedTask);
    }

    private bool Exists()
    {
        return !string.IsNullOrEmpty(ConfigName) && File.Exists(ConfigPath);
    }
}