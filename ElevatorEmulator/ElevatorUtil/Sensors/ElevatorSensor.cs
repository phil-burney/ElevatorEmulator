using ElevatorEmulator.ElevatorUtil.Enums;
using System;

namespace ElevatorEmulator.ElevatorUtil.Sensors
{
    /// <summary>
    /// Represents an elevator sensor which monitors and provides information about the elevator's state.
    /// </summary>
    public class ElevatorSensor : IElevatorSensor
    {
        /// <summary>
        /// Gets or sets the current direction of the elevator.
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Gets or sets the current motion state of the elevator.
        /// </summary>
        public MotionState MotionState { get; set; }

        /// <summary>
        /// Gets or sets the current floor on which the elevator is present.
        /// </summary>
        public int CurrentFloor { get; set; } = 1;

        /// <summary>
        /// Gets or sets the next target floor for the elevator.
        /// </summary>
        public int NextFloor { get; set; }

        /// <summary>
        /// Gets or sets the current weight inside the elevator.
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Gets the weight limit of the elevator.
        /// </summary>
        public int WeightLimit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorSensor"/> class.
        /// </summary>
        /// <param name="weightLimit">The maximum weight capacity of the elevator.</param>
        public ElevatorSensor(int weightLimit)
        {
            WeightLimit = weightLimit;
        }
    }
}
