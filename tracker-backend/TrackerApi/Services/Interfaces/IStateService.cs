namespace TrackerApi.Services.Interfaces;

public interface IStateService
{
    int GetCurrentLineIndex { get; }
    void SetNextCurrentLine();
    void ResetCurrentLine();
}