using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services;

public class StateService : IStateService
{
    private int _currentLine = 0;
    private readonly object _lock = new();

    public int GetCurrentLine => _currentLine;

    public void ResetCurrentLine()
    {
        lock (_lock)
        {
            _currentLine = 0;
        }
    }

    public void SetNextCurrentLine()
    {
        lock (_lock)
        {
            _currentLine++;
        }
    }
}