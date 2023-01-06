using System.Reactive.Linq;
using System.ServiceProcess;

namespace TrayStatus.StatusProviders.GitLabRunner;

class WindowsService : IDisposable
{
    private readonly ServiceController serviceController;

    public WindowsService(string serviceName)
    {
        serviceController = new ServiceController(serviceName);
    }

    public IObservable<ServiceControllerStatus> ObserveStatus(TimeSpan refreshInterval)
    {
        var first = Observable.Return(serviceController.Status);

        var refreshed = Observable.Interval(refreshInterval)
            .Select(_ =>
            {
                serviceController.Refresh();
                return serviceController.Status;
            });

        return Observable.Concat(first, refreshed);
    }

    public void Dispose()
    {
        serviceController.Dispose();
    }
}
