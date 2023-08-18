namespace ElevatorEmulator.Log
{
    /// <summary>
    /// Provides functionality to log elevator-related events to a specified file.
    /// </summary>
    public class ElevatorActivityLogger
    {
        /// <summary>
        /// The path to the log file where event messages are recorded.
        /// </summary>
        private readonly string _logFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorActivityLogger"/> class with the specified log file path.
        /// </summary>
        /// <param name="logFilePath">Path to the log file.</param>
        public ElevatorActivityLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        /// <summary>
        /// Logs events related to the elevator's operations.
        /// </summary>
        /// <param name="timestamp">The time the event occurred.</param>
        /// <param name="eventType">Describes the type or nature of the event.</param>
        /// <param name="message">A detailed message about the event.</param>
        public void LogElevatorEvent(DateTime timestamp, string eventType, string message)
        {
            LogEvents(timestamp, eventType, message);
        }

        /// <summary>
        /// Logs events related to user interactions with the elevator.
        /// </summary>
        /// <param name="timestamp">The time the event occurred.</param>
        /// <param name="eventType">Describes the type or nature of the event.</param>
        /// <param name="message">A detailed message about the event.</param>
        public void LogUserEvent(DateTime timestamp, string eventType, string message)
        {
            LogEvents(timestamp, eventType, message);
        }

        /// <summary>
        /// Helper method to format and record events to the log file.
        /// </summary>
        /// <param name="timestamp">The time the event occurred.</param>
        /// <param name="eventType">Describes the type or nature of the event.</param>
        /// <param name="message">A detailed message about the event.</param>
        private void LogEvents(DateTime timestamp, string eventType, string message)
        {
            string timestampStr = timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"); // Format: 2023-08-18 12:34:56.789
            string formattedMessage = $"[{timestampStr}] [{eventType}] {message}";

            // Appends the formatted message to the log file.
            File.AppendAllText(_logFilePath, formattedMessage + Environment.NewLine);
        }
    }
}
