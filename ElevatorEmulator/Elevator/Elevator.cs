using ElevatorEmulator.ElevatorUtil;
using ElevatorEmulator.Log;

namespace ElevatorEmulator.Elevator
{
    /// <summary>
    /// Represents an elevator, handling button presses, movement, and logging activity.
    /// </summary>
    public class Elevator
    {
        /// <summary>
        /// Gets the manager responsible for handling the elevator's logic and movement.
        /// </summary>
        public IElevatorManager ElevatorManager { get; }

        /// <summary>
        /// Gets the logger used for recording the elevator's activities.
        /// </summary>
        public ElevatorActivityLogger ElevatorActivityLogger { get; }

        /// <summary>
        /// Total number of floors that the elevator can service.
        /// </summary>
        public int NumberOfFloors;

        /// <summary>
        /// Initializes a new instance of the <see cref="Elevator"/> class.
        /// </summary>
        /// <param name="numberOfFloors">The number of floors the elevator can service.</param>
        /// <param name="logger">The logger to record the elevator's activities.</param>
        public Elevator(int numberOfFloors, ElevatorActivityLogger logger, ElevatorManager manager)
        {
            NumberOfFloors = numberOfFloors;
            ElevatorManager = manager;
            ElevatorManager.OnElevatorPassedFloor += ElevatorManager_OnElevatorPassedFloor;
            ElevatorManager.OnElevatorStoppedAtFloor += ElevatorManager_OnElevatorStoppedAtFloor;
            ElevatorActivityLogger = logger;
        }

        /// <summary>
        /// Handle event for when elevator stops at a floor
        /// </summary>
        /// <param name="sender">Where event came from.</param>
        /// <param name="e">Event arguments.</param>
        private void ElevatorManager_OnElevatorStoppedAtFloor(object sender, ElevatorUtil.Events.ElevatorStoppedAtFloorEventArgs e)
        {
            ElevatorActivityLogger.LogElevatorEvent(e.TimeStamp, "ELEVATOR STOP", "Floor " + e.Floor);
        }

        /// <summary>
        /// Handle event for when elevator passes a floor
        /// </summary>
        /// <param name="sender">Where event came from.</param>
        /// <param name="e">Event arguments.</param>
        private void ElevatorManager_OnElevatorPassedFloor(object sender, ElevatorUtil.Events.ElevatorPassedFloorEventArgs e)
        {
            ElevatorActivityLogger.LogElevatorEvent(e.TimeStamp, "ELEVATOR PASSED", "Floor " + e.Floor);
        }

        /// <summary>
        /// Simulates a button press inside the elevator.
        /// </summary>
        /// <param name="floor">The floor number to which the elevator should travel.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PressButtonInsideElevator(int floor)
        {
            ValidateFloor(floor);
            ElevatorManager.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, floor));
            if (!ElevatorManager.IsHandlingRequests)
            {
                await ElevatorManager.ExecuteRoute();
            }
        }

        /// <summary>
        /// Simulates a button press outside the elevator.
        /// </summary>
        /// <param name="floor">The floor number from which a call was made to the elevator.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PressButtonOutsideElevator(int floor)
        {
            ValidateFloor(floor);
            ElevatorManager.AddStop(new ElevatorRequest(RequestOrigin.OutsideElevator, floor));
            if (!ElevatorManager.IsHandlingRequests)
            {
                await ElevatorManager.ExecuteRoute();
            }
        }

        /// <summary>
        /// Validates the floor number to ensure it's within the elevator's service range.
        /// </summary>
        /// <param name="floor">The floor number to validate.</param>
        private void ValidateFloor(int floor)
        {
            if (floor < 1 || floor > NumberOfFloors)
            {
                throw new ArgumentOutOfRangeException("The elevator does not service the requested floor.");
            }
        }
    }
}
