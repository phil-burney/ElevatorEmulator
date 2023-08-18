using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;

namespace ElevatorEmulator.ElevatorUtil.Motors
{
    public interface IElevatorMotor
    {

        /// <summary>
        /// The motor has a sensor, which takes readings from the motor. 
        /// </summary>
        public IElevatorSensor Sensor { get; }
        /// <summary>
        /// Triggered when the elevator passes a floor.
        /// </summary>
        event MotorPassedFloorHandler OnMotorPassedFloor;
        /// <summary>
        /// Triggered when the elevator stops at a floor.
        /// </summary>
        event MotorStopEventHandler OnMotorStop;



        /// <summary>
        /// Simulates the elevator moving one floor.  When the elevator starts moving, it passes a floor so an event is fired for that.
        /// When the elevator reaches the new floor, the elevator has changed floors, and an event fires for that.  
        /// </summary>
        /// <param name="initialFloor">Current floor.</param>
        /// <param name="finalFloor">Floor to move to.</param>
        Task ChangeOneFloorAsync(int initialFloor, int finalFloor);

        /// <summary>
        /// Simulates the elevator stopping at a specified floor.
        /// </summary>
        /// <param name="floor">Floor where the elevator stops.</param>
        Task StopAtFloorAsync(int floor);
    }
}