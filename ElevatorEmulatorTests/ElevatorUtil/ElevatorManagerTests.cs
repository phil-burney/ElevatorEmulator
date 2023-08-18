using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElevatorEmulator.Elevator;
using ElevatorEmulator.ElevatorUtil.Enums;

namespace ElevatorEmulator.ElevatorUtil.Tests
{
    [TestClass()]
    public class ElevatorManagerTests
    {
        /// <summary>
        /// Ensures that the stops taken in an executed route matches the expected route.
        /// </summary>
        /// <param name="expectedRoute">The route that the elevator is expected to take</param>
        /// <param name="actualRoute">The actual route that the elevator takes</param>
        private void AssertExecutedRoutesAreEqual(List<int> expectedRoute, List<int> actualRoute)
        {
            Assert.AreEqual(expectedRoute.Count, actualRoute.Count);
            for (int i = 0; i < expectedRoute.Count; i++)
            {
                Assert.AreEqual(expectedRoute[i], actualRoute[i]);
            }
        }
        /// <summary>
        /// Scenario 1:
        /// - Initial State: Elevator is at ground floor.
        /// - User Action: Press button for floor 5.
        /// - Expected Outcome: Elevator travels to floor 5.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest1()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
            };

            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));

            await controller.ExecuteRoute();

            Assert.AreEqual(1, stops.Count);
            Assert.AreEqual(5, stops[0]);

        }
        /// <summary>
        /// Scenario 2:
        /// - Initial State: Elevator is at ground floor.
        /// - User Action: Press button for top floor. Midway, request to return to ground floor.
        /// - Expected Outcome: Elevator travels to top floor, pauses for a second, then returns to ground floor.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest2()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
            };

            controller.Motor.OnMotorPassedFloor += (s, e) =>
            {
                if (e.Floor == 6 && e.Direction == Enums.Direction.Up)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 1));
                }
            };

            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 10));
            await controller.ExecuteRoute();

            Assert.AreEqual(2, stops.Count);
            Assert.AreEqual(10, stops[0]);
            Assert.AreEqual(1, stops[1]);

        }

        /// <summary>
        /// Scenario 3:
        /// - Initial State: Elevator is at ground floor.
        /// - User Action: Press button for 8th floor. At the 4th floor, add 3rd floor to the route. At the 5th floor, add 4th floor to the route.
        /// - Expected Outcome: Elevator travels to the 8th floor, pauses, then travels to the 4th floor, then to the 3rd floor.
        /// Also test to see if the elevator will run even if the routes are added after the route execution starts. 
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest3()
        {
            List<int> stops = new List<int>();

            List<int> passes = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
            };

            controller.Motor.OnMotorPassedFloor += (s, e) =>
            {
                passes.Add(e.Floor);
                if ( e.Floor == 4 && e.Direction == Direction.Up)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 3));
                }
                if (e.Floor == 5 && e.Direction == Direction.Up)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 4));
                }
            };
            
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 8));
            await controller.ExecuteRoute();
            Assert.AreEqual(12, passes.Count);
            AssertExecutedRoutesAreEqual(new List<int> { 8, 4, 3 }, stops);

        }

        /// <summary>
        /// Scenario 4:
        /// - Initial State: Elevator is at 5th floor.
        /// - User Action: Press button for 10th floor. Between 5th and 6th floor, add 5th and 7th floors to the route.
        /// - Expected Outcome: Elevator stops at the 7th floor, then at the 10th floor (the 5th floor will not count since user was just there).
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest4()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.Sensor.CurrentFloor = 5;

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
            };

            controller.Motor.OnMotorPassedFloor += (s, e) =>
            {
                if (e.Floor == 5 && e.Direction == Enums.Direction.Up)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 7));
                }
            };

            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 10));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 7, 10 }, stops);

        }

        /// <summary>
        /// Scenario 5:
        /// - Initial State: Elevator is at 5th floor.
        /// - User Action: Press button for 10th floor. Between 6th and 7th floor, add 5th and 7th floors to the route.
        /// - Expected Outcome: Elevator stops at the 10th floor, then 7th floor, and finally 5th floor.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest5()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.Sensor.CurrentFloor = 5;

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
            };

            controller.Motor.OnMotorPassedFloor += (s, e) =>
            {
                if (e.Floor == 6 && e.Direction == Enums.Direction.Up)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 7));
                }
            };

            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 10));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 10, 7, 5 }, stops);

        }

        /// <summary>
        /// Scenario 6:
        /// - Initial State: Elevator is at 1st floor.
        /// - User Action: Go to 4th floor. Once at the 4th floor, press button for 5th floor. Between 4th and 5th floor, press button for 6th floor.
        /// - Expected Outcome: Elevator stops at 4th, 5th, and 6th floors in sequence.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest6()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
                if (e.Floor == 4)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
                }
            };

            controller.Motor.OnMotorPassedFloor += (s, e) =>
            {
                if (e.Floor == 4 && e.Direction == Enums.Direction.Up)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 6));
                }
            };

            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 4));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 4, 5, 6 }, stops);

        }

        /// <summary>
        /// Scenario 7:
        /// - Initial State: Elevator is at 5th floor.
        /// - User Action: During the stop, select 4th and 6th floors.
        /// - Expected Outcome: Elevator stops first at the 4th floor and then at the 6th floor.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest7()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
                if (e.Floor == 5)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 4));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 6));
                }
            };


            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 5, 4, 6 }, stops);

        }

        /// <summary>
        /// Scenario 8:
        /// - Initial State: Elevator is at 10th floor.  The user elects to visit the 1st and 5th floors.
        /// - User Action: During the stop, select 4th and 6th floors.
        /// - Expected Outcome: Elevator stops first at the 5th, 4th, 1st, and 6th floors.  
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest8()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.Sensor.CurrentFloor = 10;

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
                if (e.Floor == 5)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 4));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 6));
                }
            };


            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 1));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 5, 4, 1, 6 }, stops);

        }

        /// <summary>
        /// Scenario 8:
        /// - Initial State: Elevator is at 5th floor.
        /// - User Action: During the stop, select 6th and 4th floors.
        /// - Expected Outcome: Elevator stops first at the 6th floor and then at the 4th floor.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest9()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
                if (e.Floor == 5)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 6));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 4));
                }
            };



            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 5, 6, 4 }, stops);

        }

        /// <summary>
        /// Scenario 9:
        /// - Initial State: Elevator is at 5th floor.
        /// - User Action: During the stop, select 3rd and 6th floors.
        /// - Expected Outcome: Elevator stops first at the 6th floor and then at the 3rd floor.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest10()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
                if (e.Floor == 5)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 3));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 6));
                }
            };



            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 5, 6, 3 }, stops);

        }

        /// <summary>
        /// Scenario 10:
        /// - Initial State: Elevator is slated to reach the 5th and 10th floors.
        /// - User Action: During the stop on the 5th floor, select 7th and 4th floors.
        /// - Expected Outcome: Elevator stops first at the 5th, 7th, 10th, and 4th floor.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest11()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
                if (e.Floor == 5)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 7));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 4));
                }
            };



            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 10));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 5, 7,10,4 }, stops);

        }

        /// <summary>
        /// Scenario 11:
        /// - Initial State: Elevator is at 5th floor.
        /// - User Action: During the stop, select 6th, 4th, 1st, 2nd, 8th, and 9th floors.
        /// - Expected Outcome: Elevator stops in sequence at the 6th, 8th, 9th, 2nd, and 1st floors.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest12()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
                if (e.Floor == 5)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 6));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 1));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 2));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 8));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 9));
                }
            };



            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 5, 6, 8, 9, 2, 1 }, stops);

        }

        /// <summary>
        /// Scenario 12:
        /// - Initial State: Elevator is slated to reach the 5th and 10th floors.
        /// - User Action: During the stop at the 5th floor, select 4th, 2nd, 1st, 7th, 8th, and 9th floors.
        /// - Expected Outcome: Elevator stops in sequence at the 5th, 7th, 8th, 9th, 10th, 9th, 8th,  4th, 2nd, and 1st floors.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest13()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
                if (e.Floor == 5)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 7));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 1));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 2));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 4));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 8));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 9));
                }
            };



            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 10));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 5, 7, 8, 9, 10, 4, 2, 1 }, stops);

        }

        /// <summary>
        /// Scenario 13:
        /// - Initial State: Elevator is at 10th floor.
        /// - User Action: Select 5th floor. Between the 9th and 8th floors, select 7th floor. Between the 7th and 6th floors, select 6th floor.
        /// - Expected Outcome: Elevator stops in sequence at the 7th, 5th, and 6th floors.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest14()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();
            controller.Motor.Sensor.CurrentFloor = 10;

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
            };

            controller.Motor.OnMotorPassedFloor += (s, e) =>
            {
                if (e.Floor == 9 && e.Direction == Enums.Direction.Down)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 7));
                }
                if (e.Floor == 7 && e.Direction == Enums.Direction.Down)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 6));
                }
            };

            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 7, 5, 6 }, stops);

        }

        /// <summary>
        /// Scenario 14:
        /// - Initial State: Elevator is at 1st floor.
        /// - User Action: Select 5th and 10th floors. At the 5th floor, select the 6th, 7th, and 4th floors. Between the 6th and 7th floors, select the 2nd floor.
        /// - Expected Outcome: Elevator stops in sequence at the 5th, 6th, 7th, 10th, 4th, and 2nd floors.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteTest15()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);
                if (e.Floor == 5)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 6));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 7));
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 4));
                }
            };

            controller.Motor.OnMotorPassedFloor += (s, e) =>
            {
                if (e.Floor == 6 && e.Direction == Enums.Direction.Up)
                {
                    controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 2));
                }
            };

            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 10));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 5, 6, 7, 10, 4, 2 }, stops);

        }
        /// <summary>
        /// Weighted Scenario 1:
        /// - Initial State: Elevator is at 1st floor with a weight of 2700.
        /// - Internal Requests: Floors 2nd and 10th.
        /// - External Requests: Floors 3rd and 4th.
        /// - Weight Change: Drops to 2600 at 2nd floor and 0 at 10th floor.
        /// - Expected Outcome: Elevator stops sequentially at 2nd, 10th, 4th, and 3rd floors.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteWithWeightTest1()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);

                if (e.Floor == 10)
                {
                    controller.Motor.Sensor.Weight = 0;
                }
            };

            controller.Motor.Sensor.Weight = 2600;
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 2));
            controller.AddStop(new ElevatorRequest(RequestOrigin.OutsideElevator, 3));
            controller.AddStop(new ElevatorRequest(RequestOrigin.OutsideElevator, 4));
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 10));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 2, 10, 4, 3 }, stops);

        }
        /// <summary>
        /// Weighted Scenario 1:
        /// - Initial State: Elevator is at 10th floor with a weight of 2600.
        /// - Internal Requests: Floors 5th and 1st
        /// - External Requests: Floors 3rd and 4th.
        /// - Weight Change: Drops to 0 at 1st floor.
        /// - Expected Outcome: Elevator stops sequentially at 5th, 1st, 3rd, and 4th floors.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteWithWeightTest2()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();

            controller.Motor.Sensor.CurrentFloor = 10;

            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);

                if (e.Floor == 1)
                {
                    controller.Motor.Sensor.Weight = 0;
                }
            };

            controller.Motor.Sensor.Weight = 2600;
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 5));
            controller.AddStop(new ElevatorRequest(RequestOrigin.OutsideElevator, 3));
            controller.AddStop(new ElevatorRequest(RequestOrigin.OutsideElevator, 4));
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 1));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 5, 1, 3, 4 }, stops);

        }


        /// <summary>
        /// Weighted Scenario 3:
        /// - Initial State: Elevator is at 1st floor with a weight of 2400.
        /// - Internal Requests: Floors 4,6,8,10,12
        /// - External Requests: Floors 2,3,5,7,9,11
        /// - Weight Change: Starts at 2400, increases by 200 at odd floors and decreases by 200 at even floors. 
        /// Drops to 1300 at 12th floor
        /// - Expected Outcome: Elevator stops sequentially at floors 2, 4, 6, 8, 10, 12, 11, 9, 7, 5, 3.
        /// </summary>
        [TestMethod()]
        public async Task ExecuteRouteWithWeightTest3()
        {
            List<int> stops = new List<int>();

            ElevatorManager controller = new ElevatorManager();


            controller.Motor.OnMotorStop += (s, e) =>
            {
                stops.Add(e.Floor);

                if (e.Floor % 2 == 0)
                {
                    controller.Motor.Sensor.Weight += 200;
                }
                else
                {
                    controller.Motor.Sensor.Weight -= 200;
                }

                if (e.Floor == 12)
                {
                    controller.Motor.Sensor.Weight = 1300;
                }
            };

            controller.Motor.Sensor.Weight = 2400;
            controller.AddStop(new ElevatorRequest(RequestOrigin.OutsideElevator, 2));
            controller.AddStop(new ElevatorRequest(RequestOrigin.OutsideElevator, 3));
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 4));
            controller.AddStop(new ElevatorRequest(RequestOrigin.OutsideElevator, 5));
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 6));
            controller.AddStop(new ElevatorRequest(RequestOrigin.OutsideElevator, 7));
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 8));
            controller.AddStop(new ElevatorRequest(RequestOrigin.OutsideElevator, 9));
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 10));
            controller.AddStop(new ElevatorRequest(RequestOrigin.OutsideElevator, 11));
            controller.AddStop(new ElevatorRequest(RequestOrigin.InsideElevator, 12));
            await controller.ExecuteRoute();

            AssertExecutedRoutesAreEqual(new List<int> { 2, 4, 6, 8, 10, 12, 11, 9, 7, 5, 3 }, stops);

        }
    }
}