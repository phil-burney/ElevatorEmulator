using System;

namespace ElevatorEmulator.ElevatorUtil.Events
{
    /// <summary>
    /// Provides data for the ElevatorStoppedAtFloor event, indicating details about where and when the elevator stopped.
    /// </summary>
    public class ElevatorStoppedAtFloorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the floor number where the elevator stopped.
        /// </summary>
        public int Floor { get; init; }

        /// <summary>
        /// Gets the timestamp indicating when the elevator stopped at the floor.
        /// </summary>
        public DateTime TimeStamp { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorStoppedAtFloorEventArgs"/> class.
        /// </summary>
        /// <param name="floor">The floor number where the elevator stopped.</param>
        /// <param name="timeStamp">The timestamp indicating when the elevator stopped at the floor.</param>
        public ElevatorStoppedAtFloorEventArgs(int floor, DateTime timeStamp)
        {
            Floor = floor;
            TimeStamp = timeStamp;
        }
    }

    /// <summary>
    /// Represents a delegate for the ElevatorStoppedAtFloor event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments containing details about the floor where the elevator stopped.</param>
    public delegate void ElevatorStoppedAtFloorEventHandler(object sender, ElevatorStoppedAtFloorEventArgs e);
}
