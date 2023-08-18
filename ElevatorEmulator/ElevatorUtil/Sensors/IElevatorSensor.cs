using ElevatorEmulator.ElevatorUtil.Enums;

namespace ElevatorEmulator.ElevatorUtil.Sensors
{
    /// <summary>
    /// Interface for maintaining an elevator's state
    /// </summary>
    public interface IElevatorSensor
    {
        /// <summary>
        /// Current floor that the elevator is on
        /// </summary>
        int CurrentFloor { get; set; }
        /// <summary>
        /// Direction that the elevator is going (up or down)
        /// </summary>
        Direction Direction { get; set; }
        /// <summary>
        /// State of motion of the elevator (stopped or moving)
        /// </summary>
        MotionState MotionState { get; set; }
        /// <summary>
        /// The next floor that the elevator will stop at
        /// </summary>
        int NextFloor { get; set; }
        /// <summary>
        /// The current weight of the elevator
        /// </summary>
        int Weight { get; set; }
        /// <summary>
        /// The weight limit of the elevator
        /// </summary>
        int WeightLimit { get; }
    }
}