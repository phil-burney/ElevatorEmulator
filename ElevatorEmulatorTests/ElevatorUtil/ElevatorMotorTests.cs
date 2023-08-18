using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ElevatorEmulator.ElevatorUtil.Motors;

namespace ElevatorEmulator.ElevatorUtil.Tests
{
    [TestClass()]
    public class ElevatorMotorTests
    {

        /// <summary>
        /// Test that the elevator will change one floor, and that all appropiate events are called,
        /// and that the delay is correct
        /// </summary>
        [TestMethod()]
        public async Task ChangeOneFloorTest()
        {
            int floor = 0;
            ElevatorMotor motor = new ElevatorMotor(2500);
            Stopwatch stopwatch = new Stopwatch();

            motor.OnMotorPassedFloor += (s, e) =>
            {
                floor = e.Floor;
            };

            // Start test
            stopwatch.Start();

            await motor.ChangeOneFloorAsync(5, 6);

            stopwatch.Stop();

            // Make assertions and check that delay is correct.
            Assert.AreEqual(5, floor);
            Assert.IsTrue(stopwatch.ElapsedMilliseconds >= 3000 && stopwatch.ElapsedMilliseconds < 3200);  // Add a buffer 
            
            stopwatch.Restart();

            await motor.ChangeOneFloorAsync(5, 4);

            stopwatch.Stop();

            Assert.AreEqual(5, floor);
            Assert.IsTrue(stopwatch.ElapsedMilliseconds >= 3000 && stopwatch.ElapsedMilliseconds < 3200);
        }

        /// <summary>
        /// Test that the elevator will stop at a floor, and that all appropiate events are called,
        /// and that the delay is correct
        /// </summary>
        [TestMethod()]
        public async Task StopAtFloorTest()
        {
            int floor = 0;
            ElevatorMotor motor = new(2500);
            motor.OnMotorStop += (s, e) =>
            {
                floor = e.Floor;
            };

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            await motor.StopAtFloorAsync(5);
            stopwatch.Stop();

            Assert.IsTrue(stopwatch.ElapsedMilliseconds > 1000);
            Assert.AreEqual(5, floor);


        }

       
    }
}