using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorEmulator.ElevatorUtil.Enums
{
    /// <summary>
    /// Enum that represnts the outcomes of evaluating whether on not the elevator should
    /// stop at a given floor
    /// </summary>
    public enum ElevatorStopResolution
    {
        Stop,
        NoStop,
        Skip
    }
}
