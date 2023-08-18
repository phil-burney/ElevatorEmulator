using ElevatorEmulator.ElevatorUtil.Enums;

namespace ElevatorEmulator.ElevatorUtil.Events
{
    /// <summary>
    /// Represents a handler for when the elevator passes a floor. 
    /// </summary>
    internal class ElevatorPassedFloorHandler
    {
    }

    /// <summary>
    /// Represents the set of arguments to be passed to the ElevatorPassedFloor event.
    /// </summary>
    public class ElevatorPassedFloorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the floor number that the elevator passed.
        /// </summary>
        public int Floor { get; init; }

        /// <summary>
        /// Gets the direction in which the elevator was moving when it passed the floor.
        /// </summary>
        public Direction Direction { get; init; }

        /// <summary>
        /// Gets the timestamp indicating when the elevator passed the floor.
        /// </summary>
        public DateTime TimeStamp { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorPassedFloorEventArgs"/> class.
        /// </summary>
        /// <param name="floor">The floor number that the elevator passed.</param>
        /// <param name="direction">The direction in which the elevator was moving when it passed the floor.</param>
        /// <param name="timeStamp">The timestamp indicating when the elevator passed the floor.</param>
        public ElevatorPassedFloorEventArgs(int floor, Direction direction, DateTime timeStamp)
        {
            Floor = floor;
            Direction = direction;
            TimeStamp = timeStamp;
        }
    }

    /// <summary>
    /// Represents a delegate for the ElevatorPassedFloor event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments containing details about the passed floor.</param>
    public delegate void ElevatorPassedFloorEventHandler(object sender, ElevatorPassedFloorEventArgs e);
}
