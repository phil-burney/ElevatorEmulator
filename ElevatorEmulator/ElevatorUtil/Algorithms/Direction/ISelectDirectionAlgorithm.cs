using ElevatorEmulator.Elevator;
using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;

namespace ElevatorEmulator.ElevatorUtil.Algorithms.Driection
{
    /// <summary>
    /// Provides the interface for direction selection algorithms for elevators.
    /// </summary>
    public interface ISelectDirectionAlgorithm
    {
        /// <summary>
        /// Determines the next direction that an elevator should take based on various parameters.
        /// </summary>
        /// <param name="sensor">The sensor interface that provides the current state and direction of the elevator.</param>
        /// <param name="requestQueue">A list of elevator requests in the order they were received.</param>
        /// <param name="upwardRoute">A list of elevator requests that require the elevator to move in the upward direction.</param>
        /// <param name="downwardRoute">A list of elevator requests that require the elevator to move in the downward direction.</param>
        /// <returns>The determined direction (Up, Down, etc.) for the elevator to take next.</returns>
        Direction DetermineNextDirection(IElevatorSensor sensor, List<ElevatorRequest> requestQueue, List<ElevatorRequest> upwardRoute, List<ElevatorRequest> downwardRoute);
    }
}
