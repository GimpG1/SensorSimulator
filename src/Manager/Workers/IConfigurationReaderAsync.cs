using Configuration.Struct;

namespace Manager.Workers;

public interface IConfigurationReaderAsync
{
    Task Read(ConfigurationBase reader);
    Task Resolve();
}