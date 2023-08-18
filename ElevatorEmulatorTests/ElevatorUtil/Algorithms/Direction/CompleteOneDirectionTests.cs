using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElevatorEmulator.ElevatorUtil.Algorithms.Driection;
using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;
using System.Collections.Generic;
using ElevatorEmulator.Elevator;

namespace ElevatorEmulator.Tests
{
    [TestClass()]
    public class CompleteOneDirectionTests
    {
        private readonly CompleteOneDirection algorithm = new CompleteOneDirection();

        [TestMethod()]
        public void ReturnUpWhenUpwardRouteHasRequestsAndDownwardRouteIsEmptyTest()
        {
            var sensor = new ElevatorSensor(2500) { CurrentFloor = 1 };

            var direction = algorithm.DetermineNextDirection(
                sensor,
                new List<ElevatorRequest>(),
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 1) },
                new List<ElevatorRequest>());

            Assert.AreEqual(Direction.Up, direction);
        }

        [TestMethod()]
        public void ShouldReturnDownWhenDownwardRouteHasRequestsAndUpwardRouteIsEmptyTest()
        {
            var sensor = new ElevatorSensor(2500) { CurrentFloor = 2 };

            var direction = algorithm.DetermineNextDirection(
                sensor,
                new List<ElevatorRequest>(),
                new List<ElevatorRequest>(),
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 1) });

            Assert.AreEqual(Direction.Down, direction);
        }

        [TestMethod()]
        public void ShouldKeepCurrentDirectionWhenElevatorWasAlreadyCompletingARouteTest()
        {
            var sensor = new ElevatorSensor(2500) { CurrentFloor = 2, Direction = Direction.Up };

            var direction = algorithm.DetermineNextDirection(
                sensor,
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 3) },
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 4) },
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 1) });

            Assert.AreEqual(Direction.Up, direction);
        }

        [TestMethod()]
        public void ShouldChooseDirectionBasedOnFirstRequestWhenDistancesAreEqualTest()
        {
            var sensor = new ElevatorSensor(2500) { CurrentFloor = 2 };

            var direction = algorithm.DetermineNextDirection(
                sensor,
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 1), new ElevatorRequest(RequestOrigin.InsideElevator, 3) },
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 3) },
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 1) }
             );

            Assert.AreEqual(Direction.Down, direction);
        }

        [TestMethod()]
        public void ShouldChooseClosestDirectionWhenOneRouteIsClearlyCloserTest()
        {
            var sensor = new ElevatorSensor(2500) { CurrentFloor = 2 };

            var direction = algorithm.DetermineNextDirection(
                sensor,
                new List<ElevatorRequest>(),
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 3) },
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 1), new ElevatorRequest(RequestOrigin.InsideElevator, 0) });

            Assert.AreEqual(Direction.Up, direction);
        }

        [TestMethod()]
        public void ShouldKeepCurrentDirectionWhenNoOtherConditionsAreMetTest()
        {
            var sensor = new ElevatorSensor(2500) { CurrentFloor = 2, Direction = Direction.Up };

            var direction = algorithm.DetermineNextDirection(
                sensor,
                new List<ElevatorRequest>(),
                new List<ElevatorRequest>(),
                new List<ElevatorRequest>());

            Assert.AreEqual(Direction.Up, direction);
        }
    }
}
