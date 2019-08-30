using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rent.Scheduler
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
