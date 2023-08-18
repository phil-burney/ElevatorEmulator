using ElevatorEmulator.Elevator;
using ElevatorEmulator.ElevatorUtil.Events;
using ElevatorEmulator.ElevatorUtil.Motors;

namespace ElevatorEmulator.ElevatorUtil
{
    public interface IElevatorManager
    {
        bool IsHandlingRequests { get; set; }
        IElevatorMotor Motor { get; }

        event ElevatorPassedFloorEventHandler OnElevatorPassedFloor;
        event ElevatorStoppedAtFloorEventHandler OnElevatorStoppedAtFloor;

        void AddStop(ElevatorRequest elevatorRequest);
        Task ExecuteRoute();
    }
}