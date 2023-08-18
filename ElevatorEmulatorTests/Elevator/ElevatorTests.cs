using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElevatorEmulator.Elevator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorEmulator.Log;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using System.Drawing;

namespace ElevatorEmulator.Elevator.Tests
{
    [TestClass()]
    public class ElevatorTests
    {
        [TestMethod()]
        public void ElevatorTest()
        {
            var logger = new ElevatorActivityLogger("filePath");
            var elevator = new Elevator(10, logger, new ElevatorUtil.ElevatorManager());

            Assert.IsNotNull(elevator.ElevatorManager);
            Assert.AreEqual(logger, elevator.ElevatorActivityLogger);
        }

        [TestMethod()]
        public async Task PressButtonInsideElevatorTest()
        {
            var logger = new ElevatorActivityLogger("filePath");
            var elevator = new Elevator(10, logger, new ElevatorUtil.ElevatorManager());
            try
            {
                await elevator.PressButtonInsideElevator(0);
                Assert.Fail();
            } catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("The elevator does not service the requested floor."));
            }
            await elevator.PressButtonInsideElevator(5);

            Assert.AreEqual(elevator.ElevatorManager.Motor.Sensor.CurrentFloor, 5);

        }
        [TestMethod()]
        public async Task PressButtonOutsideElevatorTest()
        {
            var logger = new ElevatorActivityLogger("filePath");
            var elevator = new Elevator(10, logger, new ElevatorUtil.ElevatorManager());
            try
            {
                await elevator.PressButtonOutsideElevator(11);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("The elevator does not service the requested floor."));
            }
            await elevator.PressButtonOutsideElevator(5);
            Assert.AreEqual(elevator.ElevatorManager.Motor.Sensor.CurrentFloor, 5);
        }
    }
}