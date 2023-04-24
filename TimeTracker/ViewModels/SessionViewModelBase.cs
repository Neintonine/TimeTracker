using TimeTracker.Types;

namespace TimeTracker.ViewModels;

public class SessionViewModelBase : ViewModelBase
{
    public SessionHandler SessionHandler { get; protected set; }

    public SessionViewModelBase(SessionHandler sessionHandler)
    {
        SessionHandler = sessionHandler;
    }
}