using ElevatorEmulator.Elevator;
using ElevatorEmulator.ElevatorUtil;
using ElevatorEmulator.Log;
using System.Text.RegularExpressions;

const int MAX_FLOORS = 10;



string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
string logDirectory = Path.Combine(currentDirectory, "Logs");

if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}

string filePath = Path.Combine(logDirectory, $"log-{DateTime.Now.Minute}.txt");

// This will delete the file if it exists, effectively overwriting it on next write.
if (File.Exists(filePath))
{
    File.Delete(filePath);
}
ElevatorActivityLogger logger = new ElevatorActivityLogger(filePath);
Elevator elevator = new Elevator(MAX_FLOORS, logger, new ElevatorManager());

while (true)
{
    Console.WriteLine($"Please enter input (e.g., 5U, 4D, or just a number like 7) [1-{MAX_FLOORS}]:");
    string input = Console.ReadLine();

    if (string.IsNullOrEmpty(input))
    {
        while (elevator.ElevatorManager.IsHandlingRequests) { }
        Console.WriteLine("Input is empty. Exiting.");
        return;
    }

    // Check if the user wants to quit the application
    if (input == "Q")
    {
        while (elevator.ElevatorManager.IsHandlingRequests) { }
        Console.WriteLine("Exiting the application. Goodbye!");
        return;
    }

    // Check if the input is a number followed by a U or D
    Match udMatch = Regex.Match(input, @"^(\d+)([UD])$");

    // Check if the input is just a number
    Match numberMatch = Regex.Match(input, @"^\d+$");

    if (udMatch.Success)
    {
        int floor = int.Parse(udMatch.Groups[1].Value);
        char direction = char.Parse(udMatch.Groups[2].Value);

        if (floor > MAX_FLOORS)
        {
            Console.WriteLine($"There are only {MAX_FLOORS} floors. Please try again.");
            continue;
        }

        if (direction == 'U')
        {
            logger.LogUserEvent(DateTime.Now, "INPUT", "User on floor " + floor + " has requested to go UP.");
            elevator.PressButtonOutsideElevator(floor);

            continue;
        }
        else if (direction == 'D')
        {
            logger.LogUserEvent(DateTime.Now, "INPUT", "User on floor " + floor + " has requested to go DOWN.");
            elevator.PressButtonOutsideElevator(floor);

            continue;
        }
    }
    else if (numberMatch.Success)
    {
        int floor = int.Parse(numberMatch.Value);

        if (floor > MAX_FLOORS)
        {
            Console.WriteLine($"There are only {MAX_FLOORS} floors. Please try again.");
            continue;
        }
        logger.LogUserEvent(DateTime.Now, "INPUT", "User in elevator has requested floor " + floor + ".");
        elevator.PressButtonInsideElevator(floor);
        continue;
    }
    else
    {
        Console.WriteLine("Invalid input format. Please try again.");
    }
}



