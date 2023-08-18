using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;

namespace ElevatorEmulator.ElevatorUtil.Motors
{
    /// <summary>
    /// Represents the elevator's motor, which is responsible for managing the elevator's movement between floors.
    /// It provides methods to change floors and stop at specific floors while raising events accordingly.
    /// </summary>
    public class ElevatorMotor : IElevatorMotor
    {
        /// <summary>
        /// Event that's triggered when the elevator passes a floor.
        /// </summary>
        public event MotorPassedFloorHandler OnMotorPassedFloor;

        /// <summary>
        /// Event that's triggered when the elevator stops at a floor.
        /// </summary>
        public event MotorStopEventHandler OnMotorStop;

        /// <summary>
        /// Gets or sets the sensor responsible for detecting the elevator's current floor, direction, and motion state.
        /// </summary>
        public IElevatorSensor Sensor { get; set; }

        private static int FLOOR_SPEED_MS = 3000;

        private static int STOP_TIME_MS = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorMotor"/> class.
        /// </summary>
        /// <param name="weightLimit">The weight limit for the elevator.</param>
        public ElevatorMotor(int weightLimit)
        {
            Sensor = new ElevatorSensor(weightLimit);
        }

        /// <summary>
        /// Simulates the elevator's movement from the initial floor to the final floor.
        /// Events are raised when the elevator starts and completes its movement.
        /// </summary>
        /// <param name="initialFloor">The starting floor.</param>
        /// <param name="finalFloor">The destination floor.</param>
        public async Task ChangeOneFloorAsync(int initialFloor, int finalFloor)
        {
            Direction direction = finalFloor > initialFloor ? Direction.Up : Direction.Down;

            // Set sensor properties to initial values
            Sensor.CurrentFloor = initialFloor;
            Sensor.NextFloor = finalFloor;
            Sensor.Direction = direction;
            Sensor.MotionState = MotionState.Moving;

            // Raise the event indicating the elevator has started its movement
            OnMotorPassedFloor?.Invoke(this, new MotorPassedFloorArgs(initialFloor, direction, DateTime.Now));

            await Task.Delay(FLOOR_SPEED_MS);

            // Update the sensor properties after the movement
            Sensor.CurrentFloor = finalFloor;
            Sensor.NextFloor = direction == Direction.Up ? finalFloor + 1 : finalFloor - 1;
            Sensor.Direction = direction;
            Sensor.MotionState = MotionState.Moving;
        }

        /// <summary>
        /// Simulates the elevator's stopping at a specified floor and raises the appropriate event.
        /// </summary>
        /// <param name="floor">The floor at which the elevator stops.</param>
        public async Task StopAtFloorAsync(int floor)
        {
            // Set the sensor properties indicating the elevator has stopped
            Sensor.CurrentFloor = floor;
            Sensor.MotionState = MotionState.Stopped;

            // Raise the event for stopping the elevator
            OnMotorStop?.Invoke(this, new MotorStopEventArgs(floor, DateTime.Now));

            await Task.Delay(STOP_TIME_MS);
        }
    }

    /// <summary>
    /// Provides data for the OnMotorPassedFloor event, containing details about the floor passed and the timestamp.
    /// </summary>
    public class MotorPassedFloorArgs : EventArgs
    {
        /// <summary>
        /// Gets the number of the floor the elevator passed.
        /// </summary>
        public int Floor { get; init; }

        /// <summary>
        /// Gets the direction of the elevator's movement.
        /// </summary>
        public Direction Direction { get; init; }

        /// <summary>
        /// Gets the timestamp when the elevator passed the floor.
        /// </summary>
        public DateTime TimeStamp { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MotorPassedFloorArgs"/> class.
        /// </summary>
        /// <param name="floor">The floor number the elevator passed.</param>
        /// <param name="direction">The direction of the elevator's movement.</param>
        /// <param name="timeStamp">The timestamp of the event.</param>
        public MotorPassedFloorArgs(int floor, Direction direction, DateTime timeStamp)
        {
            Floor = floor;
            Direction = direction;
            TimeStamp = timeStamp;
        }
    }

    /// <summary>
    /// Represents a delegate for the OnMotorPassedFloor event.
    /// </summary>
    public delegate void MotorPassedFloorHandler(object sender, MotorPassedFloorArgs e);

    /// <summary>
    /// Provides data for the OnMotorStop event, containing details about the floor where the elevator stopped and the timestamp.
    /// </summary>
    public class MotorStopEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the floor number where the elevator stopped.
        /// </summary>
        public int Floor { get; init; }

        /// <summary>
        /// Gets the timestamp when the elevator stopped at the floor.
        /// </summary>
        public DateTime TimeStamp { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MotorStopEventArgs"/> class.
        /// </summary>
        /// <param name="floor">The floor number where the elevator stopped.</param>
        /// <param name="timeStamp">The timestamp of the event.</param>
        public MotorStopEventArgs(int floor, DateTime timeStamp)
        {
            Floor = floor;
            TimeStamp = timeStamp;
        }
    }

    /// <summary>
    /// Represents a delegate for the OnMotorStop event.
    /// </summary>
    public delegate void MotorStopEventHandler(object sender, MotorStopEventArgs e);
}
