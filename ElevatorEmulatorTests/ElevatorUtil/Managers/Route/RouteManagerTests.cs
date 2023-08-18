using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElevatorEmulator.ElevatorUtil.Managers.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorEmulator.Elevator;
using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;

namespace ElevatorEmulator.ElevatorUtil.Managers.Route.Tests
{
    [TestClass()]
    public class RouteManagerTests
    {

        private RouteManager _routeManager;
        private ElevatorSensor _sensor;

        [TestInitialize]
        public void TestInitialize()
        {
            _routeManager = new RouteManager();
            _sensor = new ElevatorSensor(2000) { CurrentFloor = 1, Weight = 1500 };
        }
        /// <summary>
        /// Add stop for moving elevator
        /// </summary>
        [TestMethod]
        public void AddStopTest1()
        {
            _sensor.MotionState = MotionState.Moving;
            _sensor.Direction = Direction.Up;

            ElevatorRequest request = new ElevatorRequest(RequestOrigin.InsideElevator, 3) ;

            _routeManager.AddStop(request, _sensor);

            Assert.IsTrue(_routeManager.UpwardRoute.Contains(request));
        }

        /// <summary>
        /// Add stop for stopped elevator
        /// </summary>
        [TestMethod]
        public void AddStopTest2()
        {
            _sensor.MotionState = MotionState.Stopped;

            ElevatorRequest request = new ElevatorRequest(RequestOrigin.InsideElevator, 3);

            _routeManager.AddStop(request, _sensor);

            Assert.IsTrue(_routeManager.StoppedElevatorRequestQueue.Contains(request));
        }

        /// <summary>
        /// Handle completed request for up direction
        /// </summary>
        [TestMethod()]
        public void CompleteRequestTest1()
        {
            ElevatorRequest request = new ElevatorRequest(RequestOrigin.InsideElevator, 3);

            _routeManager.UpwardRoute.Add(request);

            var completedRequest = _routeManager.CompleteRequest(Direction.Up);

            Assert.AreEqual(request, completedRequest);
            Assert.IsFalse(_routeManager.UpwardRoute.Contains(request));
        }

        /// <summary>
        /// Handle completed request for down direction
        /// </summary>
        [TestMethod()]
        public void CompleteRequestTest2()
        {
            ElevatorRequest request = new ElevatorRequest(RequestOrigin.InsideElevator, 3);

            _routeManager.DownwardRoute.Add(request);

            var completedRequest = _routeManager.CompleteRequest(Direction.Down);

            Assert.AreEqual(request, completedRequest);
            Assert.IsFalse(_routeManager.DownwardRoute.Contains(request));
        }

        /// <summary>
        /// Handle skipped request for up direction
        /// </summary>
        [TestMethod]
        public void HandleSkippedRequestTest1()
        {
            ElevatorRequest request = new ElevatorRequest(RequestOrigin.InsideElevator, 3);

            _routeManager.UpwardRoute.Add(request);
            _sensor.Direction = Direction.Up;

            _routeManager.HandleSkippedRequest(_sensor);

            Assert.IsFalse(_routeManager.UpwardRoute.Contains(request));
            Assert.IsTrue(_routeManager.DownwardRoute.Contains(request));
        }
        /// <summary>
        /// Handle skipped request for down direction
        /// </summary>
        [TestMethod()]
        public void HandleSkippedRequestTest2()
        {
            ElevatorRequest request = new ElevatorRequest(RequestOrigin.InsideElevator, 3);

            _routeManager.DownwardRoute.Add(request);
            _sensor.Direction = Direction.Down;

            _routeManager.HandleSkippedRequest(_sensor);

            Assert.IsFalse(_routeManager.DownwardRoute.Contains(request));
            Assert.IsTrue(_routeManager.UpwardRoute.Contains(request));
        }
    }
}
