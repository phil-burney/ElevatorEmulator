using ElevatorEmulator.Elevator;
using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;

namespace ElevatorEmulator.ElevatorUtil.Managers.Route
{
    public class RouteManager : IRouteManager
    {
        /// <summary>
        /// Gets the list of elevator requests in the upward direction.
        /// </summary>
        public List<ElevatorRequest> UpwardRoute { get; } = new List<ElevatorRequest>();

        /// <summary>
        /// Gets the list of elevator requests in the downward direction.
        /// </summary>
        public List<ElevatorRequest> DownwardRoute { get; } = new List<ElevatorRequest>();
        /// <summary>
        /// A queue of requests that the elevator accumulates while stopped
        /// </summary>
        public List<ElevatorRequest> StoppedElevatorRequestQueue { get; set; } = new List<ElevatorRequest>();

        /// <summary>
        /// Calculates the route for a stopped elevator.  If the requested floor is greater than the current 
        /// floor, it will be part of the upward route.  Otherwise, it is part of the downward route.  
        /// </summary>
        /// <param name="request">The request that the method is to process.</param>
        /// <returns>The direction that the controller should be turning the motor</returns> 
        private void CalculateRouteMotorStopped(ElevatorRequest request, IElevatorSensor sensor)
        {

            if (request.Floor > sensor.CurrentFloor)
                AddToUpwardRoute(request);
            else if (request.Floor < sensor.CurrentFloor)
                AddToDownwardRoute(request);
        }

        /// <summary>
        /// Calculates the route for an elevator in motion.  If a floor is requested that is in the direction that the elevator is currently going,
        /// it must be at least one floor away.  Otherwise, the elevator will stop at the requested floor when it changes direction.  
        /// </summary>
        /// <param name="request">The request that the method is to process.</param>
        private void CalculateRouteElevatorInMotion(ElevatorRequest request, IElevatorSensor sensor)
        {
            switch (sensor.Direction)
            {
                case Direction.Up:
                    if (sensor.CurrentFloor < request.Floor - 1)
                        AddToUpwardRoute(request);
                    else
                        AddToDownwardRoute(request);
                    break;

                case Direction.Down:
                    if (sensor.CurrentFloor > request.Floor + 1)
                        AddToDownwardRoute(request);
                    else
                        AddToUpwardRoute(request);
                    break;
            }
        }

        /// <summary>
        /// Adds a request to the list of requests that the elevator will fulfill on the way down and
        /// uses binary search to insert the request at the appropiate place.  
        /// </summary>
        /// <param name="request">The request to be added to the list of request that the elevator wil
        /// complete on the way down</param>
        private void AddToDownwardRoute(ElevatorRequest request)
        {
            int index = DownwardRoute.BinarySearch(request);
            if (index < 0)
            {
                index = ~index; // Bitwise complement to get the index of the next element that is larger than the given one or the insertion point.
            }
            DownwardRoute.Insert(index, request);
        }

        /// <summary>
        /// Adds a request to the list of requests that the elevator will fulfill on the way up and
        /// uses binary search to insert the request at the appropiate place. 
        /// </summary>
        /// <param name="request">The request to be added to the list of request that the elevator wil
        /// complete on the way up</param>
        private void AddToUpwardRoute(ElevatorRequest request)
        {
            int index = UpwardRoute.BinarySearch(request);
            if (index < 0)
            {
                index = ~index; // Bitwise complement to get the index of the next element that is larger than the given one or the insertion point.
            }
            UpwardRoute.Insert(index, request);
        }
        /// <summary>
        /// Adds a stop to the elevator's route.  Different behavior occurs depending on whether or not the elevator is moving.
        /// </summary>
        /// <param name="request">The request that the method is to process.</param>
        /// <returns>The direction that the controller should turn the motor.</returns>
        public void AddStop(ElevatorRequest request, IElevatorSensor sensor)
        {
            switch (sensor.MotionState)
            {
                case MotionState.Moving:
                    CalculateRouteElevatorInMotion(request, sensor);
                    break;
                case MotionState.Stopped:
                    StoppedElevatorRequestQueue.Add(request);
                    CalculateRouteMotorStopped(request, sensor);
                    break;
            }
        }

        /// <summary>
        /// Completes a request for the elevator, removing the request from the queue.
        /// </summary>
        /// <param name="direction">The direction (Up or Down) for which the request is completed.</param>
        /// <returns>The completed elevator request.</returns>
        public ElevatorRequest CompleteRequest(Direction direction)
        {
            if (direction == Direction.Up)
            {
                ElevatorRequest retRequest = UpwardRoute[0];
                UpwardRoute.RemoveAt(0);
                return retRequest;
            }
            else
            {
                ElevatorRequest retRequest = DownwardRoute[DownwardRoute.Count - 1];
                DownwardRoute.RemoveAt(DownwardRoute.Count - 1);
                return retRequest;
            }
        }

        /// <summary>
        /// Handles a request that was skipped by re-assigning it to the opposite direction.
        /// </summary>
        /// <param name="sensor">Provides information about the elevator's current state and direction.</param>

        public void HandleSkippedRequest(IElevatorSensor sensor)
        {
            ElevatorRequest request = CompleteRequest(sensor.Direction);

            if (sensor.Direction == Direction.Up)
            {
                AddToDownwardRoute(request);
            }
            else if (sensor.Direction == Direction.Down)
            {
                AddToUpwardRoute(request);
            }
        }
    }
}
