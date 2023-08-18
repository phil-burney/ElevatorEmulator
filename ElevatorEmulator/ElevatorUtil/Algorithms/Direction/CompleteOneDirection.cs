using ElevatorEmulator.Elevator;
using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;

namespace ElevatorEmulator.ElevatorUtil.Algorithms.Driection
{
    public class CompleteOneDirection : ISelectDirectionAlgorithm
    {
        /// <summary>
        /// Basic test to determine the next direction that an elevator should take.  
        /// If one of the routes is empty, then the elevator should change directions. 
        /// </summary>
        /// 
        public Direction DetermineNextDirection(IElevatorSensor sensor, List<ElevatorRequest> requestQueue, List<ElevatorRequest> upwardRoute, List<ElevatorRequest> downwardRoute)
        {
            // If one route has requests and the other one doesn't, set the elevator in that direction and return
            if (upwardRoute.Count > 0 && downwardRoute.Count == 0)
            {
                return Direction.Up;
            }
            if (downwardRoute.Count > 0 && upwardRoute.Count == 0)
            {
                return Direction.Down;
            }
            // If both routes have requests, then there needs to be a tie breaker.
            // If one route has a request closer to the elevator's current position, then the elevator is set in that direction.
            // If both routes have request that are the same distance from the elevator's current position, then the elevator
            // moves toward the first request that was made.
            if (requestQueue.Count > 0)
            {
                // If the elevator was already completing a route, then let the elevator complete the route.
                if (upwardRoute.Count + downwardRoute.Count > requestQueue.Count)
                {
                    return sensor.Direction;
                }

                // Calculate the distances between the nearest downward floor and upward floor
                int upwardDist = upwardRoute[0].Floor - sensor.CurrentFloor;
                int downwardDist = sensor.CurrentFloor - downwardRoute[downwardRoute.Count - 1].Floor;
                // If they are equal, the first request made wins out.  Otherwise, the closest floor wins out
                if (upwardDist == downwardDist)
                {
                    return requestQueue[0].Floor > sensor.CurrentFloor ? Direction.Up : Direction.Down;
                }
                else
                {
                    return upwardDist > downwardDist ? Direction.Down : Direction.Up;
                }
            }
            // If none of the above criterion are met, then keep the direction the same.  
            return sensor.Direction;
        }
    }
}
