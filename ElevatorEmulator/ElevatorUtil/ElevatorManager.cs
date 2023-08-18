using ElevatorEmulator.Elevator;
using ElevatorEmulator.ElevatorUtil.Algorithms.Driection;
using ElevatorEmulator.ElevatorUtil.Algorithms.ElevatorStopAtFloor;
using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Events;
using ElevatorEmulator.ElevatorUtil.Managers.Route;
using ElevatorEmulator.ElevatorUtil.Motors;

namespace ElevatorEmulator.ElevatorUtil
{
    /// <summary>
    /// Manages the elevator's operations, routing, direction and stoppage at floors.
    /// The manager coordinates with algorithms to determine direction, stops, and route execution.
    /// </summary>
    public class ElevatorManager : IElevatorManager
    {
        /// <summary>
        /// Manages the elevator's route by adding, skipping, and completing requests.
        /// </summary>
        private IRouteManager RouteManager { get; set; }

        /// <summary>
        /// Determines the direction in which the elevator should move.
        /// </summary>
        private ISelectDirectionAlgorithm DirectionAlgorithm { get; set; }

        /// <summary>
        /// Determines if the elevator should stop at the floor.
        /// </summary>
        private IStopAtFloorAlgorithm StopAtFloorAlgorithm { get; set; }

        /// <summary>
        /// Holds the decision regarding whether the elevator should stop or skip the next floor.
        /// </summary>
        private ElevatorStopResolution ShouldStopAtNextFloor { get; set; } = ElevatorStopResolution.NoStop;

        /// <summary>
        /// Represents the direction in which the elevator is intended to move.
        /// </summary>
        private Direction Direction { get; set; }

        /// <summary>
        /// The motor responsible for physically driving the elevator's movement.
        /// </summary>
        public IElevatorMotor Motor { get; }

        /// <summary>
        /// Tells if the elevator is handling requests.
        /// </summary>
        public bool IsHandlingRequests { get; set; } = false;

        /// <summary>
        /// Event that's triggered when the elevator passes a floor.
        /// </summary>
        public event ElevatorPassedFloorEventHandler OnElevatorPassedFloor;

        /// <summary>
        /// Event that's triggered when the elevator stops at a floor.
        /// </summary>
        public event ElevatorStoppedAtFloorEventHandler OnElevatorStoppedAtFloor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorManager"/> class with the provided elevator motor, route manager, direction selection algorithm, and floor stop algorithm.
        /// Additionally, sets up event handlers for motor stop and floor passing events.
        /// </summary>
        /// <param name="motor">The elevator's motor responsible for moving the elevator.</param>
        /// <param name="routeManager">Manages the routes or sequences in which the elevator stops.</param>
        /// <param name="directionAlgorithm">Algorithm determining the direction of the elevator.</param>
        /// <param name="stopAtFloorAlgorithm">Algorithm determining if the elevator should stop at a specific floor.</param>
        public ElevatorManager(
            IElevatorMotor motor,
            IRouteManager routeManager,
            ISelectDirectionAlgorithm directionAlgorithm,
            IStopAtFloorAlgorithm stopAtFloorAlgorithm)
        {
            Motor = motor;
            RouteManager = routeManager;
            DirectionAlgorithm = directionAlgorithm;
            StopAtFloorAlgorithm = stopAtFloorAlgorithm;

            Motor.OnMotorStop += OnMotorStop;
            Motor.OnMotorPassedFloor += OnMotorPassedFloor;

        }

        /// <summary>
        /// Default constructor initializing the motor with a default weight limit.
        /// </summary>
        public ElevatorManager() : this(new ElevatorMotor(2500), new RouteManager(), new CompleteOneDirection(), new StopIfInDirectionAndUnderweight()) { }

        /// <summary>
        /// Handles the motor's event for when the elevator passes a floor. It determines if the elevator should stop or continue moving.
        /// </summary>
        /// <param name="sender">Object that originated the event.</param>
        /// <param name="e">Event data containing information about the passed floor and direction.</param>
        private void OnMotorPassedFloor(object sender, MotorPassedFloorArgs e)
        {
            // If the elevator is moving, clear the stationary request queue
            RouteManager.StoppedElevatorRequestQueue = new List<ElevatorRequest>();
            // Calculate if the elevator should stop at the next floor
            ShouldStopAtNextFloor = StopAtFloorAlgorithm.ShouldElevatorStopAtFloor(Motor.Sensor, RouteManager.UpwardRoute, RouteManager.DownwardRoute);

            // If the elevator should skip the next floor, then the route manager needs to the handle the skipped route
            if (ShouldStopAtNextFloor == ElevatorStopResolution.Skip)
            {
                RouteManager.HandleSkippedRequest(Motor.Sensor);
            }
            // Fire an event for the elevator that the elevator has passed a floor.  
            OnElevatorPassedFloor?.Invoke(this, new ElevatorPassedFloorEventArgs(e.Floor, e.Direction, DateTime.Now));
        }

        /// <summary>
        /// Handles the event for when the elevator stops at a floor, invoking appropriate events and completing route requests.
        /// </summary>
        /// <param name="sender">Object that originated the event.</param>
        /// <param name="e">Event data containing information about the stopped floor.</param>
        private void OnMotorStop(object sender, MotorStopEventArgs e)
        {
            // On stopping, fire an event and complete the request.  
            OnElevatorStoppedAtFloor?.Invoke(this, new ElevatorStoppedAtFloorEventArgs(e.Floor, DateTime.Now));
            RouteManager.CompleteRequest(Motor.Sensor.Direction);
        }

        /// <summary>
        /// Adds a floor stop request to the elevator's route and updates the elevator's direction accordingly.
        /// </summary>
        /// <param name="elevatorRequest">The request details, such as destination floor and direction.</param>
        public void AddStop(ElevatorRequest elevatorRequest)
        {
            if (elevatorRequest.Floor != Motor.Sensor.CurrentFloor)
            {
                RouteManager.AddStop(elevatorRequest, Motor.Sensor);
                Direction = DirectionAlgorithm.DetermineNextDirection(Motor.Sensor, RouteManager.StoppedElevatorRequestQueue, RouteManager.UpwardRoute, RouteManager.DownwardRoute);
            }
        }

        /// <summary>
        /// Executes the elevator's route, fulfilling stop requests until the route is empty.
        /// </summary>
        public async Task ExecuteRoute()
        {
            IsHandlingRequests = true;
            // While there are requests to handle
            while (RouteManager.UpwardRoute.Count > 0 || RouteManager.DownwardRoute.Count > 0)
            {
                // Determine the next floor based on direction
                int nextFloor = Direction == Direction.Up ? Motor.Sensor.CurrentFloor + 1 : Motor.Sensor.CurrentFloor - 1;
                await Motor.ChangeOneFloorAsync(Motor.Sensor.CurrentFloor, nextFloor);
                // See if the elevator should stop at next floor and act accordingly.  
                if (ShouldStopAtNextFloor == ElevatorStopResolution.Stop)
                {
                    await Motor.StopAtFloorAsync(Motor.Sensor.CurrentFloor);
                    Direction = DirectionAlgorithm.DetermineNextDirection(Motor.Sensor, RouteManager.StoppedElevatorRequestQueue, RouteManager.UpwardRoute, RouteManager.DownwardRoute);
                }
            }
            IsHandlingRequests = false;
        }
    }
}
