using System;

namespace Rent.Models.TimePlanning.Enums
{

    [Flags]
    public enum WorkModificationFlags
    {
        None = 0,
        Registered = 1,

        Note = 2,
        StartTimeMins = 4,
        EndTimeMins = 8,
        BreakMins = 16,
        IsVisible = 32,


    }

}