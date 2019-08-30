using System;
namespace Rent.Scheduler.Cron
{
    public delegate void CrontabFieldAccumulator(int start, int end, int interval);
}
