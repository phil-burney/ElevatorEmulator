using ElevatorEmulator.Elevator;
using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;

namespace ElevatorEmulator.ElevatorUtil.Algorithms.ElevatorStopAtFloor
{
    /// <summary>
    /// Provides the interface for algorithms that determine whether the elevator should stop at a particular floor.
    /// </summary>
    public interface IStopAtFloorAlgorithm
    {
        /// <summary>
        /// Determines whether the elevator should stop at the current floor, based on the provided state and request lists.
        /// </summary>
        /// <param name="sensor">The sensor interface that provides the current state and direction of the elevator.</param>
        /// <param name="upwardRoute">A list of elevator requests that require the elevator to move in the upward direction.</param>
        /// <param name="downwardRoute">A list of elevator requests that require the elevator to move in the downward direction.</param>
        /// <returns>
        /// A resolution decision (<see cref="ElevatorStopResolution"/>) indicating whether the elevator should stop, continue moving, etc.
        /// </returns>
        ElevatorStopResolution ShouldElevatorStopAtFloor(IElevatorSensor sensor, List<ElevatorRequest> upwardRoute, List<ElevatorRequest> downwardRoute);
    }
}
