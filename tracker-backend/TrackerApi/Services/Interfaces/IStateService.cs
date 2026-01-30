namespace TrackerApi.Services.Interfaces;

public interface IStateService
{
    int GetCurrentLine { get; }
    void SetNextCurrentLine();
    void ResetCurrentLine();
}