using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElevatorEmulator.ElevatorUtil.Algorithms.ElevatorStopAtFloor;
using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;
using System.Collections.Generic;
using ElevatorEmulator.Elevator;

namespace ElevatorEmulator.Tests
{
    [TestClass()]
    public class StopIfInDirectionAndUnderweightTests
    {
        private readonly StopIfInDirectionAndUnderweight algorithm = new StopIfInDirectionAndUnderweight();

        [TestMethod()]
        public void ShouldReturnNoStopWhenNoRequestForThisFloorTest()
        {
            var sensor = new ElevatorSensor(2500) { CurrentFloor = 1, NextFloor = 3, Direction = Direction.Up };

            var resolution = algorithm.ShouldElevatorStopAtFloor(
                sensor,
                new List<ElevatorRequest>(), // empty upwardRoute
                new List<ElevatorRequest>()  // empty downwardRoute
            );

            Assert.AreEqual(ElevatorStopResolution.NoStop, resolution);
        }

        [TestMethod()]
        public void ShouldReturnStopWhenRequestForFloorAndUnderweightTest()
        {
            var sensor = new ElevatorSensor(2000) { CurrentFloor = 1, NextFloor = 3, Direction = Direction.Up, Weight = 1500 };

            var resolution = algorithm.ShouldElevatorStopAtFloor(
                sensor,
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 3) },
                new List<ElevatorRequest>()
            );

            Assert.AreEqual(ElevatorStopResolution.Stop, resolution);
        }

        [TestMethod()]
        public void ShouldReturnSkipWhenRequestFromOutsideAndOverweightTest()
        {
            var sensor = new ElevatorSensor(2000) { CurrentFloor = 1, NextFloor = 3, Direction = Direction.Up, Weight = 2100 };

            var resolution = algorithm.ShouldElevatorStopAtFloor(
                sensor,
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.OutsideElevator, 3) },
                new List<ElevatorRequest>()
            );

            Assert.AreEqual(ElevatorStopResolution.Skip, resolution);
        }

        [TestMethod()]
        public void ShouldReturnStopWhenRequestFromInsideAndOverweightTest()
        {
            var sensor = new ElevatorSensor(2000) { CurrentFloor = 1, NextFloor = 3, Direction = Direction.Up, Weight = 2100 };

            var resolution = algorithm.ShouldElevatorStopAtFloor(
                sensor,
                new List<ElevatorRequest> { new ElevatorRequest(RequestOrigin.InsideElevator, 3) },
                new List<ElevatorRequest>()
            );

            Assert.AreEqual(ElevatorStopResolution.Stop, resolution);
        }
    }
}
