namespace Configuration;

public interface IRead
{
    Task ReadAsync();
    Task ReadConfig();
}