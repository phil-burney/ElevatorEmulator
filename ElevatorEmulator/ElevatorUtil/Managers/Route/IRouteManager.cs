using ElevatorEmulator.Elevator;
using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;

namespace ElevatorEmulator.ElevatorUtil.Managers.Route
{
    /// <summary>
    /// Provides the interface for managing elevator routes, 
    /// including adding stops, handling skipped requests, and completing requests.
    /// </summary>
    public interface IRouteManager
    {
        /// <summary>
        /// Gets the list of elevator requests for the downward direction.
        /// </summary>
        List<ElevatorRequest> DownwardRoute { get; }

        /// <summary>
        /// Gets or sets the list of elevator requests that have been stopped.
        /// </summary>
        List<ElevatorRequest> StoppedElevatorRequestQueue { get; set; }

        /// <summary>
        /// Gets the list of elevator requests for the upward direction.
        /// </summary>
        List<ElevatorRequest> UpwardRoute { get; }

        /// <summary>
        /// Adds a new stop request for the elevator.
        /// </summary>
        /// <param name="request">The elevator request detailing the desired stop.</param>
        /// <param name="sensor">The sensor interface that provides the current state and direction of the elevator.</param>
        void AddStop(ElevatorRequest request, IElevatorSensor sensor);

        /// <summary>
        /// Completes a request for the elevator, removing the request from the queue.
        /// </summary>
        /// <param name="direction">The direction (Up or Down) for which the request is completed.</param>
        /// <returns>The completed elevator request.</returns>
        ElevatorRequest CompleteRequest(Direction direction);

        /// <summary>
        /// Handles the scenario when an elevator request is skipped.
        /// </summary>
        /// <param name="sensor">The sensor interface that provides the current state and direction of the elevator.</param>
        void HandleSkippedRequest(IElevatorSensor sensor);
    }
}
