using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.ServiceProcess;
using TrayStatus.Core;
using TrayStatus.StatusProviders.GitLabRunner.Icons;

namespace TrayStatus.StatusProviders.GitLabRunner;

class GitLabRunnerStatusProvider : IStatusProvider, IDisposable
{
    private readonly WindowsService windowsServiceStatus;
    private readonly IconProvider iconProvider;
    private readonly ISubject<Stream> iconSubject = new ReplaySubject<Stream>(1);
    private readonly ISubject<string> textSubject = new ReplaySubject<string>(1);
    private ServiceControllerStatus? serviceStatus;
    private readonly Collection<IDisposable> subscriptions = new Collection<IDisposable>();

    public GitLabRunnerStatusProvider(IconProvider iconProvider)
    {
        this.iconProvider = iconProvider ?? throw new ArgumentNullException(nameof(iconProvider));

        OnStateChanged(new NotRunningServiceState(iconProvider));

        windowsServiceStatus = new WindowsService("gitlab-runner");
        Subscribe(windowsServiceStatus.ObserveStatus(TimeSpan.FromSeconds(3)), ChangeState);
    }

    private void ChangeState(ServiceControllerStatus serviceStatus)
    {
        if (this.serviceStatus == serviceStatus)
            return;

        IServiceState state = serviceStatus switch
        {
            ServiceControllerStatus.Running => new RunningServiceState(iconProvider),
            _ => new NotRunningServiceState(iconProvider),
        };
        OnStateChanged(state);

        this.serviceStatus = serviceStatus;
    }

    private void OnStateChanged(IServiceState state)
    {
        this.iconSubject.OnNext(state.Icon);
        this.textSubject.OnNext(state.Text);
    }

    public IObservable<Stream> GetIconSource() => iconSubject;
    public IObservable<string> GetTextSource() => textSubject;

    private void Subscribe<T>(IObservable<T> observable, Action<T> onNext) => subscriptions.Add(observable.Subscribe(onNext));

    public void Dispose()
    {
        windowsServiceStatus.Dispose();

        foreach (var x in subscriptions)
            x.Dispose();
    }

    interface IServiceState
    {
        Stream Icon { get; }
        string Text { get; }
    }

    class RunningServiceState : IServiceState
    {
        public Stream Icon { get; }
        public string Text => "GitLab runner service is running";

        public RunningServiceState(IconProvider iconProvider) => Icon = iconProvider.GetRunning();
    }

    class NotRunningServiceState : IServiceState
    {
        public Stream Icon { get; }
        public string Text => "GitLab runner service is not running";

        public NotRunningServiceState(IconProvider iconProvider) => Icon = iconProvider.GetNotRunning();
    }
}