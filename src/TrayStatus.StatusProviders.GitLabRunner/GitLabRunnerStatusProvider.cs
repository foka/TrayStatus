using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.ServiceProcess;
using TrayStatus.Core;
using TrayStatus.StatusProviders.GitLabRunner.Icons;

namespace TrayStatus.StatusProviders.GitLabRunner;

class GitLabRunnerStatusProvider : IStatusProvider, IDisposable
{
    private readonly WindowsServiceController windowsService;
    private readonly IconProvider iconProvider;
    private ServiceControllerStatus? serviceStatus;
    private readonly ISubject<Stream> iconSubject = new ReplaySubject<Stream>(1);
    private readonly ISubject<string> textSubject = new ReplaySubject<string>(1);
    private readonly ISubject<IEnumerable<StatusCommand>> commandsSubject = new ReplaySubject<IEnumerable<StatusCommand>>(1);
    private readonly Collection<IDisposable> subscriptions = new Collection<IDisposable>();

    public GitLabRunnerStatusProvider(IconProvider iconProvider)
    {
        this.iconProvider = iconProvider ?? throw new ArgumentNullException(nameof(iconProvider));

        windowsService = new WindowsServiceController("gitlab-runner", TimeSpan.FromSeconds(5));

        OnStateChanged(new NotRunningServiceState(iconProvider, windowsService));

        Subscribe(windowsService.StatusSource, ChangeState);
    }

    private void ChangeState(ServiceControllerStatus serviceStatus)
    {
        if (this.serviceStatus == serviceStatus)
            return;

        ServiceState state = serviceStatus switch
        {
            ServiceControllerStatus.Running => new RunningServiceState(iconProvider, windowsService),
            _ => new NotRunningServiceState(iconProvider, windowsService),
        };
        OnStateChanged(state);

        this.serviceStatus = serviceStatus;
    }

    private void OnStateChanged(ServiceState state)
    {
        this.iconSubject.OnNext(state.Icon);
        this.textSubject.OnNext(state.Text);
        this.commandsSubject.OnNext(state.Commands);
    }

    public IObservable<Stream> IconSource => iconSubject;
    public IObservable<string> TextSource => textSubject;
    public IObservable<IEnumerable<StatusCommand>> CommandsSource => commandsSubject;

    private void Subscribe<T>(IObservable<T> observable, Action<T> onNext) => subscriptions.Add(observable.Subscribe(onNext));

    public void Dispose()
    {
        windowsService.Dispose();

        foreach (var x in subscriptions)
            x.Dispose();
    }


    abstract class ServiceState
    {
        public Stream Icon { get; protected set; } = null!;
        public string Text { get; protected set; } = "";
        public IEnumerable<StatusCommand> Commands { get; protected set; } = Enumerable.Empty<StatusCommand>();
    }

    class RunningServiceState : ServiceState
    {
        public RunningServiceState(IconProvider iconProvider, WindowsServiceController windowsService)
        {
            Icon = iconProvider.GetRunning();
            Text = "GitLab runner service is running";
            Commands = new []
            {
                new StatusCommand("Stop runner", () => windowsService.Stop())
            };
        }
    }

    class NotRunningServiceState : ServiceState
    {
        public NotRunningServiceState(IconProvider iconProvider, WindowsServiceController windowsService)
        {
            Icon = iconProvider.GetNotRunning();
            Text = "GitLab runner service is not running";
            Commands = new []
            {
                new StatusCommand("Start runner", () => windowsService.Start())
            };
        }
    }
}