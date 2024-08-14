namespace ClientUtilsProject.Utils;

public interface ISportLogger
{
    void Verbose(string message);
    void Information(string message);

    public ValueTask DisposeAsync();

}