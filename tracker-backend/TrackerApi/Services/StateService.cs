using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services;

public class StateService : IStateService
{
    private int _currentLineIndex = 0;
    private readonly object _lock = new();

    public int GetCurrentLineIndex => _currentLineIndex;

    public void ResetCurrentLine()
    {
        lock (_lock)
        {
            _currentLineIndex = 0;
        }
    }

    public void SetNextCurrentLine()
    {
        lock (_lock)
        {
            _currentLineIndex++;
        }
    }
}