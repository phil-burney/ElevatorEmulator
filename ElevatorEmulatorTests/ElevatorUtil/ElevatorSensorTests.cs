using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElevatorEmulator.ElevatorUtil;
using ElevatorEmulator.ElevatorUtil.Enums;
using ElevatorEmulator.ElevatorUtil.Sensors;

namespace ElevatorEmulator.Tests
{
    [TestClass()]
    public class ElevatorSensorTests
    {
        [TestMethod()]
        public void ElevatorSensorInitializationWithWeightLimitTest()
        {
            int expectedWeightLimit = 500;


            var elevatorSensor = new ElevatorSensor(expectedWeightLimit);


            Assert.AreEqual(expectedWeightLimit, elevatorSensor.WeightLimit);
            Assert.AreEqual(1, elevatorSensor.CurrentFloor);
        }

        [TestMethod()]
        public void ElevatorSensorSetDirectionChangesDirectionTest()
        {

            var elevatorSensor = new ElevatorSensor(500);


            elevatorSensor.Direction = Direction.Up;


            Assert.AreEqual(Direction.Up, elevatorSensor.Direction);
        }

        [TestMethod()]
        public void ElevatorSensorSetMotionStateChangesMotionStateTest()
        {

            var elevatorSensor = new ElevatorSensor(500);


            elevatorSensor.MotionState = MotionState.Moving;


            Assert.AreEqual(MotionState.Moving, elevatorSensor.MotionState);
        }

        [TestMethod()]
        public void ElevatorSensorSetWeightChangesWeightTest()
        {

            var elevatorSensor = new ElevatorSensor(500);
            int expectedWeight = 250;


            elevatorSensor.Weight = expectedWeight;


            Assert.AreEqual(expectedWeight, elevatorSensor.Weight);
        }

        [TestMethod()]
        public void ElevatorSensorSetNextFloorChangesNextFloorTest()
        {

            var elevatorSensor = new ElevatorSensor(500);
            int expectedNextFloor = 5;


            elevatorSensor.NextFloor = expectedNextFloor;


            Assert.AreEqual(expectedNextFloor, elevatorSensor.NextFloor);
        }

    }
}