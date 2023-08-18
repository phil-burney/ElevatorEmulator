using ElevatorEmulator.Elevator;
using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;

namespace ElevatorEmulator.ElevatorUtil.Algorithms.ElevatorStopAtFloor
{
    public class StopIfInDirectionAndUnderweight : IStopAtFloorAlgorithm
    {
        /// <summary>
        /// A checklist for the elevator to run at each floor to determine if it should stop at that floor or not.  
        /// 
        /// If the stop is in the direction that the elevator is currently travelling and the elevator is below the weight limit, then the elevator will stop.
        /// If the elevator is above the weight limit, then only stops that were requested from inside the elevator will be made, to all users to exit.
        /// Any stops that were skipped will be made when the elevator changes direction. 
        /// </summary>
        /// <returns>Whether or not the elevator should stop at this next floor</returns>
        public ElevatorStopResolution ShouldElevatorStopAtFloor(IElevatorSensor sensor, List<ElevatorRequest> upwardRoute, List<ElevatorRequest> downwardRoute)
        {
            // Declare and instantiate all needed variables
            ElevatorRequest? requestToBeFulfilled = null;

            // If the next floor is on the upward route queue and the elevator is travelling upward, then the upward route request queue is being cleared
            // Record the request; we will need it later
            if (upwardRoute.Count > 0 && upwardRoute.First().Floor == sensor.NextFloor && sensor.Direction == Enums.Direction.Up)
            {
                requestToBeFulfilled = upwardRoute.First();
            }
            // If the next floor is on the downward route queue and the elevator is travelling downward, then the downward route request queue is being cleared
            // Record the request; we will need it later
            if (downwardRoute.Count > 0 && downwardRoute.Last().Floor == sensor.NextFloor && sensor.Direction == Enums.Direction.Down)
            {
                requestToBeFulfilled = downwardRoute.Last();
            }
            // If there are no requests for this floor, then we know that we can move on to the next floor
            if (requestToBeFulfilled == null)
            {
                return ElevatorStopResolution.NoStop;
            }
            // If there is a request to be fulfilled for this floor, check to see if the elevator is overweight and that the request came from outside the elevator.
            // If these conditions are met, then this floor will be skipped and we need to reprocess the request to be fulfilled at a later time.   
            if (requestToBeFulfilled != null && sensor.WeightLimit <= sensor.Weight && requestToBeFulfilled.Origin == RequestOrigin.OutsideElevator)
            {
                return ElevatorStopResolution.Skip;
            }
            // If the request can in fact be fulfilled, stop at this floor.   
            return ElevatorStopResolution.Stop;

        }
    }
}
