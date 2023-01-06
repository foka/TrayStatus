using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.ServiceProcess;

namespace TrayStatus.StatusProviders.GitLabRunner;

class WindowsServiceController : IDisposable
{
    private readonly ServiceController serviceController;
    private readonly ISubject<ServiceControllerStatus> manualStatus = new ReplaySubject<ServiceControllerStatus>(1);

    public WindowsServiceController(string serviceName, TimeSpan refreshInterval)
    {
        serviceController = new ServiceController(serviceName);

        CheckStatus();
        StatusSource = CreateStatusSource(refreshInterval);
    }

    public IObservable<ServiceControllerStatus> StatusSource { get; }

    private IObservable<ServiceControllerStatus> CreateStatusSource(TimeSpan refreshInterval)
    {
        var intervalStatus = Observable.Interval(refreshInterval)
            .Select(_ =>
            {
                serviceController.Refresh();
                return serviceController.Status;
            });

        return Observable.Merge(manualStatus, intervalStatus);
    }

    private void CheckStatus()
    {
        serviceController.Refresh();
        manualStatus.OnNext(serviceController.Status);
    }

    public void Stop()
    {
        serviceController.Stop();
        CheckStatus();
    }

    public void Start()
    {
        serviceController.Start();
        CheckStatus();
    }

    public void Dispose()
    {
        serviceController.Dispose();
    }
}
