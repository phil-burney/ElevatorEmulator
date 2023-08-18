

namespace ElevatorEmulator.Elevator
{
    /// <summary>
    /// Represents a request to the elevator system, indicating a desired destination or origin floor and the origin of the request.
    /// </summary>
    public class ElevatorRequest : IComparable<ElevatorRequest>
    {
        /// <summary>
        /// Gets the origin of the elevator request, which indicates whether the request came from inside or outside the elevator.
        /// </summary>
        public RequestOrigin Origin { get; }

        /// <summary>
        /// Gets the desired floor associated with this request.
        /// </summary>
        public int Floor { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorRequest"/> class.
        /// </summary>
        /// <param name="origin">The origin of the request.</param>
        /// <param name="floor">The desired floor.</param>
        public ElevatorRequest(RequestOrigin origin, int floor)
        {
            Origin = origin;
            Floor = floor;
        }

        /// <summary>
        /// Compares the current instance with another instance of <see cref="ElevatorRequest"/> based on the desired floor.
        /// </summary>
        /// <param name="other">The other instance of <see cref="ElevatorRequest"/> to compare with.</param>
        /// <returns>A value indicating the relative order of the objects being compared.</returns>
        public int CompareTo(ElevatorRequest other)
        {
            return Floor.CompareTo(other.Floor);
        }
    }
}